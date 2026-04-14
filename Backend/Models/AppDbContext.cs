using Microsoft.EntityFrameworkCore;

namespace Backend.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<DocumentIdType> DocumentIdTypes { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<InventoryMovement> InventoryMovements { get; set; }

    public virtual DbSet<PersonType> PersonTypes { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Provider> Providers { get; set; }

    public virtual DbSet<Purchase> Purchases { get; set; }

    public virtual DbSet<PurchaseDetail> PurchaseDetails { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<SalesDetail> SalesDetails { get; set; }

    public virtual DbSet<Storage> Storages { get; set; }

    public virtual DbSet<Transfer> Transfers { get; set; }

    public virtual DbSet<TransferDetail> TransferDetails { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.id).HasName("Categories_pkey");

            entity.Property(e => e.id).HasDefaultValueSql("uuidv7()");
            entity.Property(e => e.active).HasDefaultValue(true);
            entity.Property(e => e.categoryName).HasMaxLength(200);
            entity.Property(e => e.createdAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.id).HasName("Customers_pkey");

            entity.HasIndex(e => e.documentIdNumber, "Customers_documentIdNumber_idx");

            entity.HasIndex(e => e.documentIdNumber, "Customers_documentIdNumber_key").IsUnique();

            entity.HasIndex(e => e.firstName, "Customers_firstName_idx");

            entity.HasIndex(e => e.firstSurname, "Customers_firstSurname_idx");

            entity.Property(e => e.id).HasDefaultValueSql("uuidv7()");
            entity.Property(e => e.active).HasDefaultValue(true);
            entity.Property(e => e.createdAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.documentIdNumber).HasMaxLength(50);
            entity.Property(e => e.email).HasMaxLength(254);
            entity.Property(e => e.firstName).HasMaxLength(150);
            entity.Property(e => e.firstSurname).HasMaxLength(150);
            entity.Property(e => e.middleName).HasMaxLength(150);
            entity.Property(e => e.secondSurname).HasMaxLength(150);
            entity.Property(e => e.telephoneNumber).HasMaxLength(10);

            entity.HasOne(d => d.docType).WithMany(p => p.Customers)
                .HasForeignKey(d => d.docTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("Customers_docTypeId_fkey");

            entity.HasOne(d => d.personType).WithMany(p => p.Customers)
                .HasForeignKey(d => d.personTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("Customers_personTypeId_fkey");
        });

        modelBuilder.Entity<DocumentIdType>(entity =>
        {
            entity.HasKey(e => e.id).HasName("DocumentIdTypes_pkey");

            entity.Property(e => e.id).HasDefaultValueSql("uuidv7()");
            entity.Property(e => e.active).HasDefaultValue(true);
            entity.Property(e => e.createdAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.documentIdName).HasMaxLength(150);
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => new { e.productSku, e.storageId }).HasName("Inventory_pkey");

            entity.ToTable("Inventory");

            entity.Property(e => e.productSku).HasMaxLength(20);
            entity.Property(e => e.lastReorderDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.location).HasMaxLength(100);
            entity.Property(e => e.updatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.productSkuNavigation).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.productSku)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("Inventory_productSku_fkey");

            entity.HasOne(d => d.storage).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.storageId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("Inventory_storageId_fkey");
        });

        modelBuilder.Entity<InventoryMovement>(entity =>
        {
            entity.HasKey(e => e.id).HasName("InventoryMovements_pkey");

            entity.HasIndex(e => e.createdAt, "InventoryMovements_movement_date_idx");

            entity.HasIndex(e => new { e.productSku, e.storageId }, "InventoryMovements_movement_lookup_idx");

            entity.HasIndex(e => e.referenceId, "InventoryMovements_ref_idx");

            entity.Property(e => e.id).HasDefaultValueSql("uuidv7()");
            entity.Property(e => e.createdAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.movementType).HasMaxLength(20);
            entity.Property(e => e.productSku).HasMaxLength(20);
            entity.Property(e => e.referenceId).HasMaxLength(50);

            entity.HasOne(d => d.productSkuNavigation).WithMany(p => p.InventoryMovements)
                .HasForeignKey(d => d.productSku)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("InventoryMovements_productSku_fkey");

            entity.HasOne(d => d.storage).WithMany(p => p.InventoryMovements)
                .HasForeignKey(d => d.storageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("InventoryMovements_storageId_fkey");

            entity.HasOne(d => d.user).WithMany(p => p.InventoryMovements)
                .HasForeignKey(d => d.userId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("InventoryMovements_userId_fkey");
        });

        modelBuilder.Entity<PersonType>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PersonType_pkey");

            entity.ToTable("PersonType");

            entity.Property(e => e.id).HasDefaultValueSql("uuidv7()");
            entity.Property(e => e.active).HasDefaultValue(true);
            entity.Property(e => e.createdAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.personTypeName).HasMaxLength(150);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.sku).HasName("Products_pkey");

            entity.HasIndex(e => e.brand, "Products_brand_idx");

            entity.HasIndex(e => e.productName, "Products_productName_idx");

            entity.Property(e => e.sku).HasMaxLength(20);
            entity.Property(e => e.active).HasDefaultValue(true);
            entity.Property(e => e.brand).HasMaxLength(100);
            entity.Property(e => e.createdAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.productName).HasMaxLength(255);
            entity.Property(e => e.purchasePrice).HasPrecision(12, 2);
            entity.Property(e => e.salePrice).HasPrecision(12, 2);

            entity.HasOne(d => d.category).WithMany(p => p.Products)
                .HasForeignKey(d => d.categoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("Products_categoryId_fkey");
        });

        modelBuilder.Entity<Provider>(entity =>
        {
            entity.HasKey(e => e.id).HasName("Providers_pkey");

            entity.HasIndex(e => e.documentIdNumber, "Providers_documentIdNumber_idx");

            entity.HasIndex(e => e.documentIdNumber, "Providers_documentIdNumber_key").IsUnique();

            entity.HasIndex(e => e.email, "Providers_email_idx");

            entity.HasIndex(e => e.firstName, "Providers_firstName_idx");

            entity.HasIndex(e => e.firstSurname, "Providers_firstSurname_idx");

            entity.HasIndex(e => e.telephoneNumber, "Providers_telephoneNumber_idx");

            entity.Property(e => e.id).HasDefaultValueSql("uuidv7()");
            entity.Property(e => e.active).HasDefaultValue(true);
            entity.Property(e => e.createdAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.documentIdNumber).HasMaxLength(50);
            entity.Property(e => e.email).HasMaxLength(254);
            entity.Property(e => e.firstName).HasMaxLength(150);
            entity.Property(e => e.firstSurname).HasMaxLength(150);
            entity.Property(e => e.middleName).HasMaxLength(150);
            entity.Property(e => e.secondSurname).HasMaxLength(150);
            entity.Property(e => e.telephoneNumber).HasMaxLength(10);

            entity.HasOne(d => d.docType).WithMany(p => p.Providers)
                .HasForeignKey(d => d.docTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("Providers_docTypeId_fkey");

            entity.HasOne(d => d.personType).WithMany(p => p.Providers)
                .HasForeignKey(d => d.personTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("Providers_personTypeId_fkey");
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.HasKey(e => e.id).HasName("Purchases_pkey");

            entity.HasIndex(e => e.providerInvoiceNumber, "Purchases_provider_invoice_idx");

            entity.HasIndex(e => e.providerId, "Purchases_purchase_provider_idx");

            entity.Property(e => e.id).HasDefaultValueSql("uuidv7()");
            entity.Property(e => e.createdAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.providerInvoiceNumber).HasMaxLength(50);
            entity.Property(e => e.status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'RECEIVED'::character varying");
            entity.Property(e => e.totalAmount).HasPrecision(12, 2);

            entity.HasOne(d => d.provider).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.providerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Purchases_providerId_fkey");

            entity.HasOne(d => d.user).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.userId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Purchases_userId_fkey");
        });

        modelBuilder.Entity<PurchaseDetail>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PurchaseDetails_pkey");

            entity.HasIndex(e => e.purchaseId, "PurchaseDetails_detail_purchase_idx");

            entity.HasIndex(e => e.productSku, "PurchaseDetails_detail_purchase_product_idx");

            entity.Property(e => e.id).HasDefaultValueSql("uuidv7()");
            entity.Property(e => e.productSku).HasMaxLength(20);
            entity.Property(e => e.subtotal).HasPrecision(12, 2);
            entity.Property(e => e.taxAmount).HasPrecision(12, 2);
            entity.Property(e => e.unitCost).HasPrecision(12, 2);

            entity.HasOne(d => d.productSkuNavigation).WithMany(p => p.PurchaseDetails)
                .HasForeignKey(d => d.productSku)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PurchaseDetails_productSku_fkey");

            entity.HasOne(d => d.purchase).WithMany(p => p.PurchaseDetails)
                .HasForeignKey(d => d.purchaseId)
                .HasConstraintName("PurchaseDetails_purchaseId_fkey");

            entity.HasOne(d => d.storage).WithMany(p => p.PurchaseDetails)
                .HasForeignKey(d => d.storageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PurchaseDetails_storageId_fkey");

            entity.ToTable(tb => tb.HasTrigger("trg_after_purchase_insert"));
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.id).HasName("Roles_pkey");

            entity.Property(e => e.id).HasDefaultValueSql("uuidv7()");
            entity.Property(e => e.active).HasDefaultValue(true);
            entity.Property(e => e.createdAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.roleName).HasMaxLength(100);
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.id).HasName("Sales_pkey");

            entity.HasIndex(e => e.invoiceNumber, "Sales_invoiceNumber_key").IsUnique();

            entity.HasIndex(e => e.invoiceNumber, "Sales_invoice_idx");

            entity.HasIndex(e => e.customerId, "Sales_sale_customer_idx");

            entity.HasIndex(e => e.createdAt, "Sales_sale_date_idx");

            entity.Property(e => e.id).HasDefaultValueSql("uuidv7()");
            entity.Property(e => e.createdAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.invoiceNumber).HasMaxLength(50);
            entity.Property(e => e.status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'COMPLETED'::character varying");
            entity.Property(e => e.totalAmount).HasPrecision(12, 2);
            entity.Property(e => e.totalDiscount)
                .HasPrecision(5, 2)
                .HasDefaultValue(0m);

            entity.HasOne(d => d.customer).WithMany(p => p.Sales)
                .HasForeignKey(d => d.customerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Sales_customerId_fkey");

            entity.HasOne(d => d.user).WithMany(p => p.Sales)
                .HasForeignKey(d => d.userId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Sales_userId_fkey");

            entity.ToTable(tb => tb.HasTrigger("trg_sale_annulment"));
        });

        modelBuilder.Entity<SalesDetail>(entity =>
        {
            entity.HasKey(e => e.id).HasName("SalesDetails_pkey");

            entity.HasIndex(e => e.productSku, "SalesDetails_detail_product_idx");

            entity.HasIndex(e => e.saleId, "SalesDetails_detail_sale_idx");

            entity.Property(e => e.id).HasDefaultValueSql("uuidv7()");
            entity.Property(e => e.discount)
                .HasPrecision(5, 2)
                .HasDefaultValue(0m);
            entity.Property(e => e.productSku).HasMaxLength(20);
            entity.Property(e => e.subtotal).HasPrecision(12, 2);
            entity.Property(e => e.taxAmount).HasPrecision(12, 2);
            entity.Property(e => e.unitPrice).HasPrecision(12, 2);

            entity.HasOne(d => d.productSkuNavigation).WithMany(p => p.SalesDetails)
                .HasForeignKey(d => d.productSku)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SalesDetails_productSku_fkey");

            entity.HasOne(d => d.sale).WithMany(p => p.SalesDetails)
                .HasForeignKey(d => d.saleId)
                .HasConstraintName("SalesDetails_saleId_fkey");

            entity.HasOne(d => d.storage).WithMany(p => p.SalesDetails)
                .HasForeignKey(d => d.storageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SalesDetails_storageId_fkey");

            entity.ToTable(tb => tb.HasTrigger("trg_after_sale_insert"));
        });

        modelBuilder.Entity<Storage>(entity =>
        {
            entity.HasKey(e => e.id).HasName("Storages_pkey");

            entity.Property(e => e.active).HasDefaultValue(true);
            entity.Property(e => e.createdAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.storageName).HasMaxLength(150);
        });

        modelBuilder.Entity<Transfer>(entity =>
        {
            entity.HasKey(e => e.id).HasName("Transfers_pkey");

            entity.Property(e => e.id).HasDefaultValueSql("uuidv7()");
            entity.Property(e => e.createdAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'COMPLETED'::character varying");

            entity.HasOne(d => d.destinationStorage).WithMany(p => p.TransferdestinationStorages)
                .HasForeignKey(d => d.destinationStorageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Transfers_destinationStorageId_fkey");

            entity.HasOne(d => d.originStorage).WithMany(p => p.TransferoriginStorages)
                .HasForeignKey(d => d.originStorageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Transfers_originStorageId_fkey");

            entity.HasOne(d => d.user).WithMany(p => p.Transfers)
                .HasForeignKey(d => d.userId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Transfers_userId_fkey");
        });

        modelBuilder.Entity<TransferDetail>(entity =>
        {
            entity.HasKey(e => e.id).HasName("TransferDetails_pkey");

            entity.Property(e => e.id).HasDefaultValueSql("uuidv7()");
            entity.Property(e => e.productSku).HasMaxLength(20);

            entity.HasOne(d => d.transfer).WithMany(p => p.TransferDetails)
                .HasForeignKey(d => d.transferId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TransferDetails_transferId_fkey");

            entity.HasOne(d => d.productSkuNavigation).WithMany(p => p.TransferDetails)
                .HasForeignKey(d => d.productSku)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TransferDetails_productSku_fkey");

            entity.ToTable(tb => tb.HasTrigger("trg_after_transfer_item"));
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.id).HasName("Users_pkey");

            entity.HasIndex(e => e.documentIdNumber, "Users_documentIdNumber_idx");

            entity.HasIndex(e => e.documentIdNumber, "Users_documentIdNumber_key").IsUnique();

            entity.HasIndex(e => e.firstName, "Users_firstName_idx");

            entity.HasIndex(e => e.firstSurname, "Users_firstSurname_idx");

            entity.HasIndex(e => e.username, "Users_username_idx");

            entity.HasIndex(e => e.username, "Users_username_key").IsUnique();

            entity.Property(e => e.id).HasDefaultValueSql("uuidv7()");
            entity.Property(e => e.active).HasDefaultValue(true);
            entity.Property(e => e.createdAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.documentIdNumber).HasMaxLength(50);
            entity.Property(e => e.firstName).HasMaxLength(150);
            entity.Property(e => e.firstSurname).HasMaxLength(150);
            entity.Property(e => e.middleName).HasMaxLength(150);
            entity.Property(e => e.secondSurname).HasMaxLength(150);
            entity.Property(e => e.updatedAt).HasColumnType("timestamp without time zone");
            entity.Property(e => e.username).HasMaxLength(100);

            entity.HasOne(d => d.docType).WithMany(p => p.Users)
                .HasForeignKey(d => d.docTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("Users_docTypeId_fkey");

            entity.HasOne(d => d.role).WithMany(p => p.Users)
                .HasForeignKey(d => d.roleId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("Users_roleId_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
