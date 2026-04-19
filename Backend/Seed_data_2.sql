/*
 * Seed_data_2.sql
 * ===============
 * Genera 180 días de transacciones realistas (compras, ventas, transferencias)
 * para validar reportes y dashboard.
 *
 * PREREQUISITO: Ejecutar Seed_data_1.sql primero.
 *
 * Triggers automáticos que se disparan:
 *   - trg_after_purchase_insert  → IN_PURCHASE  + stock++
 *   - trg_after_sale_insert      → OUT_SALE     + stock--
 *   - trg_sale_annulment         → IN_ANNULMENT + stock++  (al anular)
 *   - trg_after_transfer_item    → TRF_OUT / TRF_IN + stock ajustado
 */

DO $$
DECLARE
    /* ── Usuarios (3 vendedores distintos para "Top Vendedores") ── */
    v_user_admin   UUID := (SELECT id FROM "Users" WHERE username = 'admin_refaxio');
    v_user_dir     UUID := (SELECT id FROM "Users" WHERE username = 'laura.dir');
    v_user_asesor  UUID := (SELECT id FROM "Users" WHERE username = 'juan.ventas');
    v_current_user UUID;

    /* ── Bodegas ── */
    v_storage_a INT := (SELECT id FROM "Storages" ORDER BY id ASC  LIMIT 1);
    v_storage_b INT := (SELECT id FROM "Storages" ORDER BY id DESC LIMIT 1);
    v_current_storage INT;

    /* ── Clientes (array de IDs para rotar) ── */
    v_customers UUID[];
    v_cust_id   UUID;

    /* ── Proveedores (array de IDs para rotar) ── */
    v_providers UUID[];
    v_prov_id   UUID;

    /* ── Variables de control ── */
    v_current_date  TIMESTAMP;
    v_sale_id       UUID;
    v_purchase_id   UUID;
    v_transfer_id   UUID;
    v_prod          RECORD;
    v_qty           INT;
    v_discount      DECIMAL(5,2);
    v_sale_count    INT;
    v_detail_count  INT;
    v_user_roll     FLOAT;
    v_origin        INT;
    v_dest          INT;
BEGIN

    /* ═══════════════════════════════════════════════════════════════
       FASE 0: Cargar arrays de clientes y proveedores
       ═══════════════════════════════════════════════════════════════ */
    SELECT ARRAY_AGG(id ORDER BY "documentIdNumber")
      INTO v_customers
      FROM "Customers";

    SELECT ARRAY_AGG(id ORDER BY "documentIdNumber")
      INTO v_providers
      FROM "Providers";

    /* ═══════════════════════════════════════════════════════════════
       FASE 0.5: Crear filas de Inventario en Bodega Norte (B)
       para que los triggers de venta/compra no fallen al hacer
       UPDATE sobre filas que no existen.
       Seed_data_1 solo creó inventario en Bodega Principal (A).
       ═══════════════════════════════════════════════════════════════ */
    INSERT INTO "Inventory" ("productSku", "storageId", "stock", "minStock", "location")
    SELECT sku, v_storage_b, 0, 5, 'ESTANTE-B1'
      FROM "Products"
    ON CONFLICT ("productSku", "storageId") DO NOTHING;

    /* ═══════════════════════════════════════════════════════════════
       SIMULACIÓN: 180 DÍAS (6 MESES)
       Recorremos del día más antiguo (i=180) al más reciente (i=0)
       ═══════════════════════════════════════════════════════════════ */
    FOR i IN REVERSE 180..0 LOOP
        v_current_date := CURRENT_TIMESTAMP - (i || ' days')::INTERVAL;

        /* ─────────────────────────────────────────────────────────
           1. COMPRAS DE REABASTECIMIENTO (cada 10 días → ~18 compras)
              • 8-10 productos aleatorios, 30-60 unidades c/u
              • Rotan entre los 5 proveedores
              • Alternan entre bodega A (70%) y bodega B (30%)
           ───────────────────────────────────────────────────────── */
        IF i % 10 = 0 THEN
            -- Rotar proveedor
            v_prov_id := v_providers[1 + (i / 10) % ARRAY_LENGTH(v_providers, 1)];

            -- Alternar bodega destino: 70% bodega A, 30% bodega B
            IF RANDOM() < 0.7 THEN
                v_current_storage := v_storage_a;
            ELSE
                v_current_storage := v_storage_b;
            END IF;

            -- Rotar usuario que registra la compra
            IF i % 30 = 0 THEN
                v_current_user := v_user_admin;
            ELSIF i % 20 = 0 THEN
                v_current_user := v_user_dir;
            ELSE
                v_current_user := v_user_asesor;
            END IF;

            INSERT INTO "Purchases" ("providerInvoiceNumber", "providerId", "userId", "totalAmount", "status", "createdAt")
            VALUES ('P-INV-' || LPAD(i::TEXT, 3, '0'), v_prov_id, v_current_user, 0, 'RECEIVED', v_current_date)
            RETURNING id INTO v_purchase_id;

            -- Comprar 8 a 10 productos aleatorios
            FOR v_prod IN (SELECT sku, "purchasePrice" FROM "Products" ORDER BY RANDOM() LIMIT (8 + FLOOR(RANDOM() * 3)::INT)) LOOP
                v_qty := 30 + FLOOR(RANDOM() * 31)::INT;  -- 30 a 60 unidades
                INSERT INTO "PurchaseDetails" ("purchaseId", "productSku", "storageId", "quantity", "unitCost", "subtotal")
                VALUES (v_purchase_id, v_prod.sku, v_current_storage, v_qty, v_prod."purchasePrice", v_qty * v_prod."purchasePrice");
            END LOOP;

            -- Actualizar total de la compra
            UPDATE "Purchases"
               SET "totalAmount" = (SELECT COALESCE(SUM(subtotal), 0) FROM "PurchaseDetails" WHERE "purchaseId" = v_purchase_id)
             WHERE id = v_purchase_id;
        END IF;

        /* ─────────────────────────────────────────────────────────
           2. VENTAS DIARIAS (3-6 ventas por día)
              • Rotan entre 3 usuarios (Admin 40%, Director 30%, Asesor 30%)
              • Rotan entre 5 clientes
              • 1-3 items por venta, 1-3 unidades por item
              • Descuento 0-15%
              • 2% de probabilidad de anulación (VOIDED)
           ───────────────────────────────────────────────────────── */
        v_sale_count := 3 + FLOOR(RANDOM() * 4)::INT;  -- 3 a 6 ventas

        FOR j IN 1..v_sale_count LOOP
            -- Rotar cliente
            v_cust_id := v_customers[1 + (j + i) % ARRAY_LENGTH(v_customers, 1)];

            -- Rotar usuario vendedor: Admin ~40%, Director ~30%, Asesor ~30%
            v_user_roll := RANDOM();
            IF v_user_roll < 0.4 THEN
                v_current_user := v_user_admin;
            ELSIF v_user_roll < 0.7 THEN
                v_current_user := v_user_dir;
            ELSE
                v_current_user := v_user_asesor;
            END IF;

            v_discount := ROUND((RANDOM() * 15)::NUMERIC, 2);  -- 0.00 a 15.00

            INSERT INTO "Sales" ("invoiceNumber", "customerId", "userId", "totalAmount", "totalDiscount", "createdAt")
            VALUES (
                'FAC-' || LPAD((180 - i)::TEXT, 3, '0') || '-' || LPAD(j::TEXT, 2, '0'),
                v_cust_id, v_current_user, 0, v_discount, v_current_date
            )
            RETURNING id INTO v_sale_id;

            -- 1 a 3 items por venta (solo productos con stock disponible)
            v_detail_count := 1 + FLOOR(RANDOM() * 3)::INT;
            FOR v_prod IN (
                SELECT p.sku, p."salePrice", i2.stock
                  FROM "Products" p
                  JOIN "Inventory" i2 ON i2."productSku" = p.sku AND i2."storageId" = v_storage_a
                 WHERE i2.stock >= 3
                 ORDER BY RANDOM()
                 LIMIT v_detail_count
            ) LOOP
                v_qty := LEAST(1 + FLOOR(RANDOM() * 3)::INT, v_prod.stock);  -- nunca más que el stock
                INSERT INTO "SalesDetails" ("saleId", "productSku", "storageId", "quantity", "unitPrice", "subtotal", "discount")
                VALUES (
                    v_sale_id, v_prod.sku, v_storage_a,
                    v_qty, v_prod."salePrice",
                    v_qty * v_prod."salePrice",
                    ROUND((RANDOM() * 5)::NUMERIC, 2)
                );
            END LOOP;

            -- Actualizar total de la venta
            UPDATE "Sales"
               SET "totalAmount" = (SELECT COALESCE(SUM(subtotal), 0) FROM "SalesDetails" WHERE "saleId" = v_sale_id)
             WHERE id = v_sale_id;

            -- 2% de probabilidad de anulación (dispara trg_sale_annulment → restaura stock)
            IF RANDOM() < 0.02 THEN
                UPDATE "Sales" SET "status" = 'VOIDED' WHERE id = v_sale_id;
            END IF;
        END LOOP;

        /* ─────────────────────────────────────────────────────────
           3. TRANSFERENCIAS ENTRE BODEGAS (cada 15 días)
              • Alternan dirección: A→B / B→A
              • 3-5 productos, 3-8 unidades c/u
           ───────────────────────────────────────────────────────── */
        IF i % 15 = 0 AND v_storage_a <> v_storage_b THEN
            -- Alternar dirección
            IF (i / 15) % 2 = 0 THEN
                v_origin := v_storage_a;
                v_dest   := v_storage_b;
            ELSE
                v_origin := v_storage_b;
                v_dest   := v_storage_a;
            END IF;

            INSERT INTO "Transfers" ("originStorageId", "destinationStorageId", "userId", "createdAt")
            VALUES (v_origin, v_dest, v_user_admin, v_current_date)
            RETURNING id INTO v_transfer_id;

            -- 3 a 5 productos con stock suficiente en bodega origen
            FOR v_prod IN (
                SELECT i2."productSku" AS sku, i2.stock
                  FROM "Inventory" i2
                 WHERE i2."storageId" = v_origin AND i2.stock >= 5
                 ORDER BY RANDOM()
                 LIMIT (3 + FLOOR(RANDOM() * 3)::INT)
            ) LOOP
                v_qty := LEAST(3 + FLOOR(RANDOM() * 6)::INT, v_prod.stock);  -- nunca más que el stock disponible
                INSERT INTO "TransferDetails" ("transferId", "productSku", "quantity")
                VALUES (v_transfer_id, v_prod.sku, v_qty);
            END LOOP;
        END IF;

    END LOOP;

    /* ═══════════════════════════════════════════════════════════════
       FASE FINAL: Crear escenario de "Stock Bajo" para el reporte
       Subimos minStock de 5 productos para que stock <= minStock
       ═══════════════════════════════════════════════════════════════ */
    UPDATE "Inventory"
       SET "minStock" = stock + 10
     WHERE ("productSku", "storageId") IN (
        SELECT "productSku", "storageId"
          FROM "Inventory"
         WHERE "storageId" = v_storage_a
         ORDER BY stock ASC
         LIMIT 5
    );

    /* ═══════════════════════════════════════════════════════════════
       RESUMEN ESPERADO
       ─────────────────
       • ~810-1080 ventas (3-6/día × 181 días), ~98% COMPLETED
       • ~19 compras (cada 10 días), todas RECEIVED
       • ~12 transferencias (cada 15 días), alternando dirección
       • 3 usuarios distintos en ventas (Top Vendedores)
       • 5 clientes distintos en ventas (Top Clientes)
       • ≥5 productos con stock bajo (Stock Bajo)
       • Miles de InventoryMovements (generados por triggers)
       • Dashboard: KPIs, gráficas donut/barras con datos reales
       ═══════════════════════════════════════════════════════════════ */

END $$;