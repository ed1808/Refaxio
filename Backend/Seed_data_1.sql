DO $$ 
BEGIN

/* 1. INSERTAR ROLES */
INSERT INTO "Roles" ("roleName") VALUES 
('Admin'), 
('Director'), 
('Asesor');

/* 2. INSERTAR TIPOS DE DOCUMENTO */
INSERT INTO "DocumentIdTypes" ("documentIdName") VALUES 
('Cédula de Ciudadanía'), 
('NIT'), 
('Cédula de Extranjería');

/* 3. INSERTAR TIPOS DE PERSONA */
INSERT INTO "PersonType" ("personTypeName") VALUES 
('Persona Natural'), 
('Persona Jurídica');

/* 4. INSERTAR CATEGORÍAS */
INSERT INTO "Categories" ("categoryName") VALUES 
('Frenos'), ('Motor'), ('Suspensión'), ('Iluminación'), ('Filtros'), ('Lubricantes');

/* 5. INSERTAR BODEGAS (Storages) */
INSERT INTO "Storages" ("storageName", "address") VALUES 
('Bodega Principal - Centro', 'Calle 10 # 15-20, Bogotá'),
('Bodega Norte', 'Avenida Siempre Viva # 123, Chía');

/* 6. INSERTAR USUARIOS (Password: holamundo123) */
-- Hash: $2a$12$R9h/cIPz0gi.URQHeSdf3.12m0PX0YQ3p8.92IEZp9XvV9T0V.P0G
INSERT INTO "Users" ("firstName", "firstSurname", "documentIdNumber", "docTypeId", "username", "password", "roleId")
VALUES 
('Carlos', 'Admin', '10102020', (SELECT id FROM "DocumentIdTypes" WHERE "documentIdName" = 'Cédula de Ciudadanía'), 'admin_refaxio', '$2a$11$qM0a0Ut6vIhd5/5fkvI7ceCdYaTfCWmgkrBkFd8MrA93W1pV.apLG', (SELECT id FROM "Roles" WHERE "roleName" = 'Admin')),
('Laura', 'Director', '30304040', (SELECT id FROM "DocumentIdTypes" WHERE "documentIdName" = 'Cédula de Ciudadanía'), 'laura.dir', '$2a$11$qM0a0Ut6vIhd5/5fkvI7ceCdYaTfCWmgkrBkFd8MrA93W1pV.apLG', (SELECT id FROM "Roles" WHERE "roleName" = 'Director')),
('Juan', 'Asesor', '50506060', (SELECT id FROM "DocumentIdTypes" WHERE "documentIdName" = 'Cédula de Ciudadanía'), 'juan.ventas', '$2a$11$qM0a0Ut6vIhd5/5fkvI7ceCdYaTfCWmgkrBkFd8MrA93W1pV.apLG', (SELECT id FROM "Roles" WHERE "roleName" = 'Asesor'));

/* 7. INSERTAR CLIENTES (5 ejemplos) */
INSERT INTO "Customers" ("firstName", "firstSurname", "documentIdNumber", "docTypeId", "personTypeId", "email", "telephoneNumber")
VALUES 
('Andrés', 'Rodríguez', '79888999', (SELECT id FROM "DocumentIdTypes" LIMIT 1), (SELECT id FROM "PersonType" LIMIT 1), 'andres.r@email.com', '3101234567'),
('Marta', 'Gómez', '52777888', (SELECT id FROM "DocumentIdTypes" LIMIT 1), (SELECT id FROM "PersonType" LIMIT 1), 'marta.g@email.com', '3159876543'),
('Taller', 'Los Pinos SAS', '900123456', (SELECT id FROM "DocumentIdTypes" WHERE "documentIdName" = 'NIT'), (SELECT id FROM "PersonType" WHERE "personTypeName" = 'Persona Jurídica'), 'contacto@lospinos.com', '6014445566'),
('Felipe', 'Castaño', '1015666777', (SELECT id FROM "DocumentIdTypes" LIMIT 1), (SELECT id FROM "PersonType" LIMIT 1), 'f.castano@email.com', '3004445566'),
('Distribuidora', 'AutoPartes COL', '800555666', (SELECT id FROM "DocumentIdTypes" WHERE "documentIdName" = 'NIT'), (SELECT id FROM "PersonType" WHERE "personTypeName" = 'Persona Jurídica'), 'ventas@autopartes.com', '6019998877');

/* 8. INSERTAR PROVEEDORES (5 ejemplos) */
INSERT INTO "Providers" ("firstName", "firstSurname", "documentIdNumber", "docTypeId", "personTypeId", "email", "telephoneNumber", "address")
VALUES 
('Brembo', 'Colombia', '900888111', (SELECT id FROM "DocumentIdTypes" WHERE "documentIdName" = 'NIT'), (SELECT id FROM "PersonType" WHERE "personTypeName" = 'Persona Jurídica'), 'latam@brembo.com', '3201112233', 'Zona Industrial Mamonal, Cartagena'),
('Mobil', 'Lubricantes', '860000123', (SELECT id FROM "DocumentIdTypes" WHERE "documentIdName" = 'NIT'), (SELECT id FROM "PersonType" WHERE "personTypeName" = 'Persona Jurídica'), 'pedidos@mobil.co', '6013334455', 'Av. El Dorado, Bogotá'),
('Bosch', 'Automotive', '900444333', (SELECT id FROM "DocumentIdTypes" WHERE "documentIdName" = 'NIT'), (SELECT id FROM "PersonType" WHERE "personTypeName" = 'Persona Jurídica'), 'soporte@bosch.com.co', '3114445566', 'Calle 100, Bogotá'),
('Importadora', 'Oriente', '700111222', (SELECT id FROM "DocumentIdTypes" WHERE "documentIdName" = 'NIT'), (SELECT id FROM "PersonType" WHERE "personTypeName" = 'Persona Jurídica'), 'gerencia@oriente.com', '3007778899', 'Sector El Poblado, Medellín'),
('Gabriel', 'Amortiguadores', '800222999', (SELECT id FROM "DocumentIdTypes" WHERE "documentIdName" = 'NIT'), (SELECT id FROM "PersonType" WHERE "personTypeName" = 'Persona Jurídica'), 'ventas@gabriel.co', '6025556677', 'Parque Industrial, Cali');

/* 9. INSERTAR PRODUCTOS (30 productos con precios en COP) */
INSERT INTO "Products" ("sku", "productName", "productDescription", "purchasePrice", "salePrice", "brand", "categoryId") VALUES 
('FRN-001', 'Pastillas de Freno Delanteras', 'Cerámicas alta duración', 85000, 145000, 'Brembo', (SELECT id FROM "Categories" WHERE "categoryName" = 'Frenos')),
('FRN-002', 'Disco de Freno ventilado', 'Disco 280mm', 120000, 210000, 'Brembo', (SELECT id FROM "Categories" WHERE "categoryName" = 'Frenos')),
('FRN-003', 'Líquido de Frenos DOT4', 'Envase 500ml', 15000, 28000, 'Bosch', (SELECT id FROM "Categories" WHERE "categoryName" = 'Frenos')),
('LUB-001', 'Aceite Sintético 5W-30', 'Cuarto de aceite sintético', 35000, 58000, 'Mobil', (SELECT id FROM "Categories" WHERE "categoryName" = 'Lubricantes')),
('LUB-002', 'Aceite 20W-50 Mineral', 'Galón para motor alto kilometraje', 95000, 145000, 'Mobil', (SELECT id FROM "Categories" WHERE "categoryName" = 'Lubricantes')),
('LUB-003', 'Grasa Rodamientos', 'Pote 250g multiusos', 12000, 22000, 'Castrol', (SELECT id FROM "Categories" WHERE "categoryName" = 'Lubricantes')),
('FLT-001', 'Filtro de Aceite', 'Elemento filtrante blindado', 18000, 32000, 'Mann Filter', (SELECT id FROM "Categories" WHERE "categoryName" = 'Filtros')),
('FLT-002', 'Filtro de Aire', 'Panel de papel plisado', 25000, 45000, 'Mann Filter', (SELECT id FROM "Categories" WHERE "categoryName" = 'Filtros')),
('FLT-003', 'Filtro de Combustible', 'Para sistemas de inyección', 30000, 55000, 'Bosch', (SELECT id FROM "Categories" WHERE "categoryName" = 'Filtros')),
('SUS-001', 'Amortiguador Delantero', 'Presión a gas', 180000, 310000, 'Gabriel', (SELECT id FROM "Categories" WHERE "categoryName" = 'Suspensión')),
('SUS-002', 'Kit de Bujes Tijera', 'Caucho reforzado', 45000, 85000, 'Thompson', (SELECT id FROM "Categories" WHERE "categoryName" = 'Suspensión')),
('SUS-003', 'Terminal de Dirección', 'Lado derecho/izquierdo', 55000, 98000, 'Thompson', (SELECT id FROM "Categories" WHERE "categoryName" = 'Suspensión')),
('MOT-001', 'Bujía de Iridium', 'Punta fina larga duración', 28000, 48000, 'NGK', (SELECT id FROM "Categories" WHERE "categoryName" = 'Motor')),
('MOT-002', 'Correa de Distribución', 'Caucho HNBR alta resistencia', 85000, 160000, 'Gates', (SELECT id FROM "Categories" WHERE "categoryName" = 'Motor')),
('MOT-003', 'Empaque de Culata', 'Multilámina acero', 110000, 195000, 'Frako', (SELECT id FROM "Categories" WHERE "categoryName" = 'Motor')),
('ILM-001', 'Bombillo H4 Halógeno', '12V 60/55W', 12000, 25000, 'Osram', (SELECT id FROM "Categories" WHERE "categoryName" = 'Iluminación')),
('ILM-002', 'Kit Luces LED H7', '6000K Blanco Frío', 110000, 220000, 'Philips', (SELECT id FROM "Categories" WHERE "categoryName" = 'Iluminación')),
('ILM-003', 'Stop Trasero Completo', 'Lente policarbonato', 150000, 280000, 'TYC', (SELECT id FROM "Categories" WHERE "categoryName" = 'Iluminación')),
('FRN-004', 'Bomba de Freno', 'Cilindro maestro 1 pulgada', 160000, 290000, 'Bosch', (SELECT id FROM "Categories" WHERE "categoryName" = 'Frenos')),
('MOT-004', 'Bomba de Agua', 'Cuerpo de aluminio', 95000, 175000, 'GMB', (SELECT id FROM "Categories" WHERE "categoryName" = 'Motor')),
('MOT-005', 'Kit de Embrague (Clutch)', 'Disco, prensa y balinera', 450000, 780000, 'LUK', (SELECT id FROM "Categories" WHERE "categoryName" = 'Motor')),
('SUS-004', 'Resorte Helicoidal', 'Acero templado reforzado', 120000, 210000, 'Gabriel', (SELECT id FROM "Categories" WHERE "categoryName" = 'Suspensión')),
('LUB-004', 'Valvulina 80W-90', 'Para transmisiones manuales', 28000, 45000, 'Mobil', (SELECT id FROM "Categories" WHERE "categoryName" = 'Lubricantes')),
('FLT-004', 'Filtro de Aire Cabina', 'Filtro polen con carbón activo', 35000, 65000, 'Mann Filter', (SELECT id FROM "Categories" WHERE "categoryName" = 'Filtros')),
('MOT-006', 'Válvula IAC', 'Control de marcha mínima', 75000, 135000, 'Bosch', (SELECT id FROM "Categories" WHERE "categoryName" = 'Motor')),
('FRN-005', 'Manguera de Freno', 'Goma reforzada delantera', 22000, 42000, 'Brembo', (SELECT id FROM "Categories" WHERE "categoryName" = 'Frenos')),
('ILM-004', 'Exploradora Universal', 'Luz antiniebla LED', 65000, 125000, 'Osram', (SELECT id FROM "Categories" WHERE "categoryName" = 'Iluminación')),
('MOT-007', 'Termostato', 'Apertura 82 grados', 38000, 72000, 'Gates', (SELECT id FROM "Categories" WHERE "categoryName" = 'Motor')),
('SUS-005', 'Rótula Suspensión', 'Acero forjado', 48000, 88000, 'Gabriel', (SELECT id FROM "Categories" WHERE "categoryName" = 'Suspensión')),
('MOT-008', 'Bobina de Encendido', 'Tipo lápiz independiente', 95000, 175000, 'Delphi', (SELECT id FROM "Categories" WHERE "categoryName" = 'Motor'));

/* 10. INICIALIZAR INVENTARIO (Stock inicial para cada producto en Bodega Principal) */
INSERT INTO "Inventory" ("productSku", "storageId", "stock", "minStock", "location")
SELECT sku, (SELECT id FROM "Storages" WHERE "storageName" = 'Bodega Principal - Centro'), 50, 10, 'ESTANTE-A1'
FROM "Products";

END $$;