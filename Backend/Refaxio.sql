CREATE TABLE "Roles" (
  "id" uuid PRIMARY KEY DEFAULT (uuidv7()),
  "active" bool DEFAULT true,
  "createdAt" timestamp DEFAULT (CURRENT_TIMESTAMP),
  "roleName" varchar(100) NOT NULL
);

CREATE TABLE "Categories" (
  "id" uuid PRIMARY KEY DEFAULT (uuidv7()),
  "active" bool DEFAULT true,
  "createdAt" timestamp DEFAULT (CURRENT_TIMESTAMP),
  "categoryName" varchar(200) NOT NULL
);

CREATE TABLE "DocumentIdTypes" (
  "id" uuid PRIMARY KEY DEFAULT (uuidv7()),
  "active" bool DEFAULT true,
  "createdAt" timestamp DEFAULT (CURRENT_TIMESTAMP),
  "documentIdName" varchar(150) NOT NULL
);

CREATE TABLE "PersonType" (
  "id" uuid PRIMARY KEY DEFAULT (uuidv7()),
  "active" bool DEFAULT true,
  "createdAt" timestamp DEFAULT (CURRENT_TIMESTAMP),
  "personTypeName" varchar(150) NOT NULL
);

CREATE TABLE "Users" (
  "id" uuid PRIMARY KEY DEFAULT (uuidv7()),
  "active" bool DEFAULT true,
  "createdAt" timestamp DEFAULT (CURRENT_TIMESTAMP),
  "firstName" varchar(150) NOT NULL,
  "middleName" varchar(150),
  "firstSurname" varchar(150) NOT NULL,
  "secondSurname" varchar(150),
  "documentIdNumber" varchar(50) UNIQUE NOT NULL,
  "docTypeId" uuid NOT NULL,
  "username" varchar(100) UNIQUE NOT NULL,
  "password" text NOT NULL,
  "roleId" uuid NOT NULL,
  "updatedAt" timestamp
);

CREATE TABLE "Customers" (
  "id" uuid PRIMARY KEY DEFAULT (uuidv7()),
  "active" bool DEFAULT true,
  "createdAt" timestamp DEFAULT (CURRENT_TIMESTAMP),
  "firstName" varchar(150) NOT NULL,
  "middleName" varchar(150),
  "firstSurname" varchar(150) NOT NULL,
  "secondSurname" varchar(150),
  "documentIdNumber" varchar(50) UNIQUE NOT NULL,
  "docTypeId" uuid NOT NULL,
  "personTypeId" uuid NOT NULL,
  "email" varchar(254),
  "telephoneNumber" varchar(10)
);

CREATE TABLE "Providers" (
  "id" uuid PRIMARY KEY DEFAULT (uuidv7()),
  "active" bool DEFAULT true,
  "createdAt" timestamp DEFAULT (CURRENT_TIMESTAMP),
  "firstName" varchar(150) NOT NULL,
  "middleName" varchar(150),
  "firstSurname" varchar(150),
  "secondSurname" varchar(150),
  "documentIdNumber" varchar(50) UNIQUE NOT NULL,
  "docTypeId" uuid NOT NULL,
  "personTypeId" uuid NOT NULL,
  "email" varchar(254) NOT NULL,
  "telephoneNumber" varchar(10) NOT NULL,
  "address" text NOT NULL
);

CREATE TABLE "Products" (
  "sku" varchar(20) PRIMARY KEY NOT NULL,
  "productName" varchar(255) NOT NULL,
  "productDescription" text,
  "purchasePrice" decimal(12,2) NOT NULL CHECK ("purchasePrice" >= 0),
  "salePrice" decimal(12,2) NOT NULL CHECK ("salePrice" >= 0),
  "brand" varchar(100) NOT NULL,
  "categoryId" uuid NOT NULL,
  "createdAt" timestamp DEFAULT (CURRENT_TIMESTAMP),
  "active" bool DEFAULT true
);

CREATE TABLE "Inventory" (
  "productSku" varchar(20) NOT NULL,
  "stock" int NOT NULL DEFAULT 0 CHECK ("stock" >= 0),
  "minStock" int NOT NULL DEFAULT 0 CHECK ("minStock" >= 0),
  "location" varchar(100),
  "storageId" int NOT NULL,
  "lastReorderDate" timestamp,
  "updatedAt" timestamp DEFAULT (CURRENT_TIMESTAMP),
  PRIMARY KEY ("productSku", "storageId")
);

CREATE TABLE "Storages" (
  "id" serial PRIMARY KEY,
  "storageName" varchar(150) NOT NULL,
  "address" text,
  "createdAt" timestamp DEFAULT (CURRENT_TIMESTAMP),
  "active" bool DEFAULT true
);

CREATE TABLE "InventoryMovements" (
  "id" uuid PRIMARY KEY DEFAULT (uuidv7()),
  "productSku" varchar(20) NOT NULL,
  "storageId" int NOT NULL,
  "movementType" varchar(20) NOT NULL,
  "quantity" int NOT NULL CHECK ("quantity" > 0),
  "balanceAfter" int NOT NULL,
  "referenceId" varchar(50),
  "userId" uuid NOT NULL,
  "createdAt" timestamp DEFAULT (CURRENT_TIMESTAMP),
  "notes" text
);

CREATE TABLE "Sales" (
  "id" uuid PRIMARY KEY DEFAULT (uuidv7()),
  "invoiceNumber" varchar(50) UNIQUE NOT NULL,
  "customerId" uuid NOT NULL,
  "userId" uuid NOT NULL,
  "totalAmount" decimal(12,2) NOT NULL DEFAULT 0,
  "totalDiscount" decimal(5,2) DEFAULT 0,
  "status" varchar(20) NOT NULL DEFAULT 'COMPLETED',
  "createdAt" timestamp DEFAULT (CURRENT_TIMESTAMP)
);

CREATE TABLE "SalesDetails" (
  "id" uuid PRIMARY KEY DEFAULT (uuidv7()),
  "saleId" uuid NOT NULL,
  "productSku" varchar(20) NOT NULL,
  "storageId" int NOT NULL,
  "quantity" int NOT NULL CHECK ("quantity" > 0),
  "unitPrice" decimal(12,2) NOT NULL,
  "taxAmount" decimal(12,2) NOT NULL DEFAULT 0,
  "subtotal" decimal(12,2) NOT NULL,
  "discount" decimal(5,2) DEFAULT 0
);

CREATE TABLE "Purchases" (
  "id" uuid PRIMARY KEY DEFAULT (uuidv7()),
  "providerInvoiceNumber" varchar(50) NOT NULL,
  "providerId" uuid NOT NULL,
  "userId" uuid NOT NULL,
  "totalAmount" decimal(12,2) NOT NULL DEFAULT 0,
  "status" varchar(20) NOT NULL DEFAULT 'RECEIVED',
  "createdAt" timestamp DEFAULT (CURRENT_TIMESTAMP)
);

CREATE TABLE "PurchaseDetails" (
  "id" uuid PRIMARY KEY DEFAULT (uuidv7()),
  "purchaseId" uuid NOT NULL,
  "productSku" varchar(20) NOT NULL,
  "storageId" int NOT NULL,
  "quantity" int NOT NULL CHECK ("quantity" > 0),
  "unitCost" decimal(12,2) NOT NULL,
  "taxAmount" decimal(12,2) NOT NULL DEFAULT 0,
  "subtotal" decimal(12,2) NOT NULL
);

CREATE TABLE "Transfers" (
  "id" uuid PRIMARY KEY DEFAULT (uuidv7()),
  "originStorageId" int NOT NULL,
  "destinationStorageId" int NOT NULL,
  "userId" uuid NOT NULL,
  "status" varchar(20) DEFAULT 'COMPLETED',
  "createdAt" timestamp DEFAULT (CURRENT_TIMESTAMP)
);

CREATE TABLE "TransferDetails" (
  "id" uuid PRIMARY KEY DEFAULT (uuidv7()),
  "transferId" uuid NOT NULL,
  "productSku" varchar(20) NOT NULL,
  "quantity" int NOT NULL CHECK ("quantity" > 0)
);

CREATE INDEX "Users_firstName_idx" ON "Users" ("firstName");

CREATE INDEX "Users_firstSurname_idx" ON "Users" ("firstSurname");

CREATE INDEX "Users_documentIdNumber_idx" ON "Users" ("documentIdNumber");

CREATE INDEX "Users_username_idx" ON "Users" ("username");

CREATE INDEX "Customers_firstName_idx" ON "Customers" ("firstName");

CREATE INDEX "Customers_firstSurname_idx" ON "Customers" ("firstSurname");

CREATE INDEX "Customers_documentIdNumber_idx" ON "Customers" ("documentIdNumber");

CREATE INDEX "Providers_firstName_idx" ON "Providers" ("firstName");

CREATE INDEX "Providers_firstSurname_idx" ON "Providers" ("firstSurname");

CREATE INDEX "Providers_documentIdNumber_idx" ON "Providers" ("documentIdNumber");

CREATE INDEX "Providers_email_idx" ON "Providers" ("email");

CREATE INDEX "Providers_telephoneNumber_idx" ON "Providers" ("telephoneNumber");

CREATE INDEX "Products_productName_idx" ON "Products" ("productName");

CREATE INDEX "Products_brand_idx" ON "Products" ("brand");

CREATE INDEX "InventoryMovements_movement_lookup_idx" ON "InventoryMovements" ("productSku", "storageId");

CREATE INDEX "InventoryMovements_movement_date_idx" ON "InventoryMovements" ("createdAt");

CREATE INDEX "InventoryMovements_ref_idx" ON "InventoryMovements" ("referenceId");

CREATE INDEX "Sales_invoice_idx" ON "Sales" ("invoiceNumber");

CREATE INDEX "Sales_sale_customer_idx" ON "Sales" ("customerId");

CREATE INDEX "Sales_sale_date_idx" ON "Sales" ("createdAt");

CREATE INDEX "SalesDetails_detail_sale_idx" ON "SalesDetails" ("saleId");

CREATE INDEX "SalesDetails_detail_product_idx" ON "SalesDetails" ("productSku");

CREATE INDEX "Purchases_provider_invoice_idx" ON "Purchases" ("providerInvoiceNumber");

CREATE INDEX "Purchases_purchase_provider_idx" ON "Purchases" ("providerId");

CREATE INDEX "PurchaseDetails_detail_purchase_idx" ON "PurchaseDetails" ("purchaseId");

CREATE INDEX "PurchaseDetails_detail_purchase_product_idx" ON "PurchaseDetails" ("productSku");

ALTER TABLE "Users" ADD FOREIGN KEY ("docTypeId") REFERENCES "DocumentIdTypes" ("id") ON DELETE RESTRICT DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE "Users" ADD FOREIGN KEY ("roleId") REFERENCES "Roles" ("id") ON DELETE RESTRICT DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE "Customers" ADD FOREIGN KEY ("docTypeId") REFERENCES "DocumentIdTypes" ("id") ON DELETE RESTRICT DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE "Customers" ADD FOREIGN KEY ("personTypeId") REFERENCES "PersonType" ("id") ON DELETE RESTRICT DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE "Providers" ADD FOREIGN KEY ("docTypeId") REFERENCES "DocumentIdTypes" ("id") ON DELETE RESTRICT DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE "Providers" ADD FOREIGN KEY ("personTypeId") REFERENCES "PersonType" ("id") ON DELETE RESTRICT DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE "Products" ADD FOREIGN KEY ("categoryId") REFERENCES "Categories" ("id") ON DELETE RESTRICT DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE "Inventory" ADD FOREIGN KEY ("productSku") REFERENCES "Products" ("sku") ON DELETE RESTRICT ON UPDATE CASCADE DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE "Inventory" ADD FOREIGN KEY ("storageId") REFERENCES "Storages" ("id") ON DELETE RESTRICT DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE "InventoryMovements" ADD FOREIGN KEY ("productSku") REFERENCES "Products" ("sku") DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE "InventoryMovements" ADD FOREIGN KEY ("storageId") REFERENCES "Storages" ("id") DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE "InventoryMovements" ADD FOREIGN KEY ("userId") REFERENCES "Users" ("id") DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE "Sales" ADD FOREIGN KEY ("customerId") REFERENCES "Customers" ("id") DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE "Sales" ADD FOREIGN KEY ("userId") REFERENCES "Users" ("id") DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE "SalesDetails" ADD FOREIGN KEY ("saleId") REFERENCES "Sales" ("id") ON DELETE CASCADE DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE "SalesDetails" ADD FOREIGN KEY ("productSku") REFERENCES "Products" ("sku") DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE "SalesDetails" ADD FOREIGN KEY ("storageId") REFERENCES "Storages" ("id") DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE "Purchases" ADD FOREIGN KEY ("providerId") REFERENCES "Providers" ("id") DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE "Purchases" ADD FOREIGN KEY ("userId") REFERENCES "Users" ("id") DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE "PurchaseDetails" ADD FOREIGN KEY ("purchaseId") REFERENCES "Purchases" ("id") ON DELETE CASCADE DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE "PurchaseDetails" ADD FOREIGN KEY ("productSku") REFERENCES "Products" ("sku") DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE "PurchaseDetails" ADD FOREIGN KEY ("storageId") REFERENCES "Storages" ("id") DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE "Transfers" ADD FOREIGN KEY ("originStorageId") REFERENCES "Storages" ("id") DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE "Transfers" ADD FOREIGN KEY ("destinationStorageId") REFERENCES "Storages" ("id") DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE "TransferDetails" ADD FOREIGN KEY ("transferId") REFERENCES "Transfers" ("id") DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE "TransferDetails" ADD FOREIGN KEY ("productSku") REFERENCES "Products" ("sku") DEFERRABLE INITIALLY IMMEDIATE;

/* ============ TRIGGERS ============ */

-- 1. Para ventas: Al insertar un detalle de venta, se actualiza el stock en la bodega correspondiente y se registra el movimiento en el Kárdex.
CREATE OR REPLACE FUNCTION fn_process_sale_stock()
RETURNS TRIGGER AS $$
DECLARE
  currentStock INT;
BEGIN
  -- Actualizar stock en la bodega específica
  UPDATE "Inventory"
  SET stock = stock - NEW.quantity,
      "updatedAt" = CURRENT_TIMESTAMP
  WHERE "productSku" = NEW."productSku"
    AND "storageId" = NEW."storageId";

  -- Obtener el stock resultante para el balanceAfter
  SELECT stock INTO currentStock FROM "Inventory"
  WHERE "productSku" = NEW."productSku"
    AND "storageId" = NEW."storageId";

  -- Insertar movimiento en el Kárdex
  INSERT INTO "InventoryMovements" (
    "productSku", "storageId", "movementType", quantity,
    "balanceAfter", "referenceId", "userId", notes
  ) VALUES (
    NEW."productSku", NEW."storageId", 'OUT_SALE', NEW.quantity,
    currentStock, NEW."saleId"::text, (SELECT "userId" FROM "Sales" WHERE id = NEW."saleId"), 
    'Venta realizada'
  );

  RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_after_sale_insert
AFTER INSERT ON "SalesDetails"
FOR EACH ROW EXECUTE FUNCTION fn_process_sale_stock();

-- 2. Para compras: Al insertar un detalle de compra, se actualiza el stock en la bodega correspondiente y se registra el movimiento en el Kárdex.
CREATE OR REPLACE FUNCTION fn_process_purchase_stock()
RETURNS TRIGGER AS $$
DECLARE
  currentStock INT;
BEGIN
  -- Actualizar stock
  UPDATE "Inventory"
  SET stock = stock + NEW.quantity,
      "updatedAt" = CURRENT_TIMESTAMP
  WHERE "productSku" = NEW."productSku"
    AND "storageId" = NEW."storageId";

  -- Obtener el stock resultante
  SELECT stock INTO currentStock FROM "Inventory"
  WHERE "productSku" = NEW."productSku"
    AND "storageId" = NEW."storageId";

  -- Insertar movimiento en el Kárdex
  INSERT INTO "InventoryMovements" (
    "productSku", "storageId", "movementType", quantity,
    "balanceAfter", "referenceId", "userId", notes
  ) VALUES (
    NEW."productSku", NEW."storageId", 'IN_PURCHASE', NEW.quantity,
    currentStock, NEW."purchaseId"::text, (SELECT "userId" FROM "Purchases" WHERE id = NEW."purchaseId"),
    'Entrada por compra'
  );

  RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_after_purchase_insert
AFTER INSERT ON "PurchaseDetails"
FOR EACH ROW EXECUTE FUNCTION fn_process_purchase_stock();

-- 2b. Para cancelaciones de compras: Al actualizar el estado de una compra a 'CANCELLED', se revierte el stock y se registra el movimiento inverso en el Kárdex.
CREATE OR REPLACE FUNCTION fn_handle_purchase_cancellation()
RETURNS TRIGGER AS $$
DECLARE
  detailRecord RECORD;
  currentStockVal INT;
BEGIN
  IF (OLD.status <> 'CANCELLED' AND NEW.status = 'CANCELLED') THEN
    FOR detailRecord IN (SELECT id, "purchaseId", "productSku", "storageId", quantity, "unitCost", "taxAmount", subtotal FROM "PurchaseDetails" WHERE "purchaseId" = NEW.id) LOOP

      -- Restar el stock de la bodega
      UPDATE "Inventory"
      SET stock = stock - detailRecord.quantity,
          "updatedAt" = CURRENT_TIMESTAMP
      WHERE "productSku" = detailRecord."productSku"
        AND "storageId" = detailRecord."storageId";

      -- Consultar el nuevo stock para el historial
      SELECT stock INTO currentStockVal FROM "Inventory"
      WHERE "productSku" = detailRecord."productSku"
        AND "storageId" = detailRecord."storageId";

      -- Registrar la reversión en el Kárdex
      INSERT INTO "InventoryMovements" (
        "productSku", "storageId", "movementType", quantity,
        "balanceAfter", "referenceId", "userId", notes
      ) VALUES (
        detailRecord."productSku", detailRecord."storageId", 'OUT_CANCELLATION', detailRecord.quantity,
        currentStockVal, NEW.id::text, NEW."userId", 'Cancelación de compra: ' || NEW."providerInvoiceNumber"
      );

    END LOOP;
  END IF;

  RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_purchase_cancellation
AFTER UPDATE ON "Purchases"
FOR EACH ROW
WHEN (OLD.status IS DISTINCT FROM NEW.status)
EXECUTE FUNCTION fn_handle_purchase_cancellation();

-- 3. Para anulaciones de ventas: Al actualizar el estado de una venta a 'VOIDED', se revierte el stock y se registra el movimiento inverso en el Kárdex.
CREATE OR REPLACE FUNCTION fn_handle_sale_annulment()
RETURNS TRIGGER AS $$
DECLARE
  detailRecord RECORD;
  currentStockVal INT;
BEGIN
  IF (OLD.status <> 'VOIDED' AND NEW.status = 'VOIDED') THEN
    FOR detailRecord IN (SELECT id, "saleId", "productSku", "storageId", quantity, "unitPrice", "taxAmount", subtotal, discount FROM "SalesDetails" WHERE "saleId" = NEW.id) LOOP

      -- Devolver el stock a la bodega original
      UPDATE "Inventory"
      SET stock = stock + detailRecord.quantity,
          "updatedAt" = CURRENT_TIMESTAMP
      WHERE "productSku" = detailRecord."productSku"
        AND "storageId" = detailRecord."storageId";

      -- Consultar el nuevo stock para el historial
      SELECT stock INTO currentStockVal FROM "Inventory"
      WHERE "productSku" = detailRecord."productSku"
        AND "storageId" = detailRecord."storageId";

      -- Registrar el re-ingreso en el Kárdex
      INSERT INTO "InventoryMovements" (
        "productSku", "storageId", "movementType", quantity,
        "balanceAfter", "referenceId", "userId", notes
      ) VALUES (
        detailRecord."productSku", detailRecord."storageId", 'IN_ANNULMENT', detailRecord.quantity,
        currentStockVal, NEW.id::text, NEW."userId", 'Anulación de factura: ' || NEW."invoiceNumber"
      );

    END LOOP;
  END IF;

  RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_sale_annulment
AFTER UPDATE ON "Sales"
FOR EACH ROW
WHEN (OLD.status IS DISTINCT FROM NEW.status)
EXECUTE FUNCTION fn_handle_sale_annulment();

-- 4. Para traslados entre bodegas: Al insertar un detalle de traslado, se actualiza el stock en ambas bodegas (origen y destino) y se registran los movimientos correspondientes en el Kárdex.
CREATE OR REPLACE FUNCTION fn_process_transfer_stock()
RETURNS TRIGGER AS $$
DECLARE
    v_origin_id INT;
    v_dest_id INT;
    v_user_id UUID;
    v_stock_orig INT;
    v_stock_dest INT;
BEGIN
    -- Obtenemos datos de la cabecera
    SELECT "originStorageId", "destinationStorageId", "userId" 
    INTO v_origin_id, v_dest_id, v_user_id
    FROM "Transfers" WHERE id = NEW."transferId";

    -- 1. RESTAR de bodega origen
    UPDATE "Inventory" SET stock = stock - NEW.quantity 
    WHERE "productSku" = NEW."productSku" AND "storageId" = v_origin_id
    RETURNING stock INTO v_stock_orig;

    -- 2. SUMAR a bodega destino
    -- Nota: Usamos INSERT ... ON CONFLICT por si el producto no existe en la bodega destino
    INSERT INTO "Inventory" ("productSku", "storageId", stock)
    VALUES (NEW."productSku", v_dest_id, NEW.quantity)
    ON CONFLICT ("productSku", "storageId") 
    DO UPDATE SET stock = "Inventory".stock + NEW.quantity
    RETURNING stock INTO v_stock_dest;

    -- 3. Registrar los DOS movimientos en el Kárdex para trazabilidad total
    -- Movimiento de Salida
    INSERT INTO "InventoryMovements" ("productSku", "storageId", "movementType", quantity, "balanceAfter", "referenceId", "userId", notes)
    VALUES (NEW."productSku", v_origin_id, 'TRF_OUT', NEW.quantity, v_stock_orig, NEW."transferId"::text, v_user_id, 'Traslado enviado');

    -- Movimiento de Entrada
    INSERT INTO "InventoryMovements" ("productSku", "storageId", "movementType", quantity, "balanceAfter", "referenceId", "userId", notes)
    VALUES (NEW."productSku", v_dest_id, 'TRF_IN', NEW.quantity, v_stock_dest, NEW."transferId"::text, v_user_id, 'Traslado recibido');

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_after_transfer_item
AFTER INSERT ON "TransferDetails"
FOR EACH ROW EXECUTE FUNCTION fn_process_transfer_stock();