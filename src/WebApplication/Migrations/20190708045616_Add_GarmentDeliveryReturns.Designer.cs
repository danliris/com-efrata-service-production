// <auto-generated />
using System;
using DanLiris.Admin.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DanLiris.Admin.Web.Migrations
{
    [DbContext(typeof(AppStorageContext))]
    [Migration("20190708045616_Add_GarmentDeliveryReturns")]
    partial class Add_GarmentDeliveryReturns
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Manufactures.Domain.GarmentAvalProducts.ReadModels.GarmentAvalProductItemReadModel", b =>
                {
                    b.Property<Guid>("Identity")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("APId");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<DateTimeOffset>("CreatedDate");

                    b.Property<bool?>("Deleted");

                    b.Property<string>("DeletedBy")
                        .HasMaxLength(32);

                    b.Property<DateTimeOffset?>("DeletedDate");

                    b.Property<string>("DesignColor");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(32);

                    b.Property<DateTimeOffset?>("ModifiedDate");

                    b.Property<string>("PreparingId");

                    b.Property<string>("PreparingItemId");

                    b.Property<string>("ProductCode");

                    b.Property<int>("ProductId");

                    b.Property<string>("ProductName");

                    b.Property<double>("Quantity");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<int>("UomId");

                    b.Property<string>("UomUnit");

                    b.HasKey("Identity");

                    b.ToTable("GarmentAvalProductItems");
                });

            modelBuilder.Entity("Manufactures.Domain.GarmentAvalProducts.ReadModels.GarmentAvalProductReadModel", b =>
                {
                    b.Property<Guid>("Identity")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Article");

                    b.Property<DateTimeOffset?>("AvalDate");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<DateTimeOffset>("CreatedDate");

                    b.Property<bool?>("Deleted");

                    b.Property<string>("DeletedBy")
                        .HasMaxLength(32);

                    b.Property<DateTimeOffset?>("DeletedDate");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(32);

                    b.Property<DateTimeOffset?>("ModifiedDate");

                    b.Property<string>("RONo");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Identity");

                    b.ToTable("GarmentAvalProducts");
                });

            modelBuilder.Entity("Manufactures.Domain.GarmentCuttingIns.ReadModels.GarmentCuttingInDetailReadModel", b =>
                {
                    b.Property<Guid>("Identity")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("BasicPrice");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<DateTimeOffset>("CreatedDate");

                    b.Property<Guid>("CutInItemId");

                    b.Property<double>("CuttingInQuantity");

                    b.Property<int>("CuttingInUomId");

                    b.Property<string>("CuttingInUomUnit")
                        .HasMaxLength(10);

                    b.Property<bool?>("Deleted");

                    b.Property<string>("DeletedBy")
                        .HasMaxLength(32);

                    b.Property<DateTimeOffset?>("DeletedDate");

                    b.Property<string>("DesignColor")
                        .HasMaxLength(25);

                    b.Property<string>("FabricType")
                        .HasMaxLength(25);

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(32);

                    b.Property<DateTimeOffset?>("ModifiedDate");

                    b.Property<Guid>("PreparingItemId");

                    b.Property<double>("PreparingQuantity");

                    b.Property<int>("PreparingUomId");

                    b.Property<string>("PreparingUomUnit")
                        .HasMaxLength(10);

                    b.Property<string>("ProductCode")
                        .HasMaxLength(25);

                    b.Property<int>("ProductId");

                    b.Property<string>("ProductName")
                        .HasMaxLength(100);

                    b.Property<double>("RemainingQuantity");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Identity");

                    b.ToTable("GarmentCuttingInDetails");
                });

            modelBuilder.Entity("Manufactures.Domain.GarmentCuttingIns.ReadModels.GarmentCuttingInItemReadModel", b =>
                {
                    b.Property<Guid>("Identity")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<DateTimeOffset>("CreatedDate");

                    b.Property<Guid>("CutInId");

                    b.Property<bool?>("Deleted");

                    b.Property<string>("DeletedBy")
                        .HasMaxLength(32);

                    b.Property<DateTimeOffset?>("DeletedDate");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(32);

                    b.Property<DateTimeOffset?>("ModifiedDate");

                    b.Property<Guid>("PreparingId");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<int>("UENId");

                    b.Property<string>("UENNo")
                        .HasMaxLength(100);

                    b.HasKey("Identity");

                    b.ToTable("GarmentCuttingInItems");
                });

            modelBuilder.Entity("Manufactures.Domain.GarmentCuttingIns.ReadModels.GarmentCuttingInReadModel", b =>
                {
                    b.Property<Guid>("Identity")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Article")
                        .HasMaxLength(50);

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<DateTimeOffset>("CreatedDate");

                    b.Property<string>("CutInNo")
                        .HasMaxLength(25);

                    b.Property<DateTimeOffset>("CuttingInDate");

                    b.Property<string>("CuttingType")
                        .HasMaxLength(25);

                    b.Property<bool?>("Deleted");

                    b.Property<string>("DeletedBy")
                        .HasMaxLength(32);

                    b.Property<DateTimeOffset?>("DeletedDate");

                    b.Property<double>("FC");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(32);

                    b.Property<DateTimeOffset?>("ModifiedDate");

                    b.Property<string>("RONo")
                        .HasMaxLength(25);

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("UnitCode")
                        .HasMaxLength(25);

                    b.Property<int>("UnitId");

                    b.Property<string>("UnitName")
                        .HasMaxLength(100);

                    b.HasKey("Identity");

                    b.ToTable("GarmentCuttingIns");
                });

            modelBuilder.Entity("Manufactures.Domain.GarmentDeliveryReturns.ReadModels.GarmentDeliveryReturnItemReadModel", b =>
                {
                    b.Property<Guid>("Identity")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<DateTimeOffset>("CreatedDate");

                    b.Property<Guid>("DRId");

                    b.Property<bool?>("Deleted");

                    b.Property<string>("DeletedBy")
                        .HasMaxLength(32);

                    b.Property<DateTimeOffset?>("DeletedDate");

                    b.Property<string>("DesignColor");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(32);

                    b.Property<DateTimeOffset?>("ModifiedDate");

                    b.Property<string>("PreparingItemId");

                    b.Property<string>("ProductCode");

                    b.Property<int>("ProductId");

                    b.Property<string>("ProductName");

                    b.Property<double>("Quantity");

                    b.Property<string>("RONo");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<int>("UENItemId");

                    b.Property<int>("UnitDOItemId");

                    b.Property<int>("UomId");

                    b.Property<string>("UomUnit");

                    b.HasKey("Identity");

                    b.ToTable("GarmentDeliveryReturnItems");
                });

            modelBuilder.Entity("Manufactures.Domain.GarmentDeliveryReturns.ReadModels.GarmentDeliveryReturnReadModel", b =>
                {
                    b.Property<Guid>("Identity")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Article");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<DateTimeOffset>("CreatedDate");

                    b.Property<string>("DRNo");

                    b.Property<bool?>("Deleted");

                    b.Property<string>("DeletedBy")
                        .HasMaxLength(32);

                    b.Property<DateTimeOffset?>("DeletedDate");

                    b.Property<bool>("IsUsed");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(32);

                    b.Property<DateTimeOffset?>("ModifiedDate");

                    b.Property<string>("PreparingId");

                    b.Property<string>("RONo");

                    b.Property<DateTimeOffset>("ReturnDate");

                    b.Property<string>("ReturnType");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("StorageCode");

                    b.Property<int>("StorageId");

                    b.Property<string>("StorageName");

                    b.Property<int>("UENId");

                    b.Property<string>("UnitCode");

                    b.Property<int>("UnitDOId");

                    b.Property<string>("UnitDONo");

                    b.Property<int>("UnitId");

                    b.Property<string>("UnitName");

                    b.HasKey("Identity");

                    b.ToTable("GarmentDeliveryReturns");
                });

            modelBuilder.Entity("Manufactures.Domain.GarmentPreparings.ReadModels.GarmentPreparingItemReadModel", b =>
                {
                    b.Property<Guid>("Identity")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("BasicPrice");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<DateTimeOffset>("CreatedDate");

                    b.Property<bool?>("Deleted");

                    b.Property<string>("DeletedBy")
                        .HasMaxLength(32);

                    b.Property<DateTimeOffset?>("DeletedDate");

                    b.Property<string>("DesignColor");

                    b.Property<string>("FabricType");

                    b.Property<Guid>("GarmentPreparingId");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(32);

                    b.Property<DateTimeOffset?>("ModifiedDate");

                    b.Property<string>("ProductCode");

                    b.Property<int>("ProductId");

                    b.Property<string>("ProductName");

                    b.Property<double>("Quantity");

                    b.Property<double>("RemainingQuantity");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<int>("UENItemId");

                    b.Property<int>("UomId");

                    b.Property<string>("UomUnit");

                    b.HasKey("Identity");

                    b.ToTable("GarmentPreparingItems");
                });

            modelBuilder.Entity("Manufactures.Domain.GarmentPreparings.ReadModels.GarmentPreparingReadModel", b =>
                {
                    b.Property<Guid>("Identity")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Article");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<DateTimeOffset>("CreatedDate");

                    b.Property<bool?>("Deleted");

                    b.Property<string>("DeletedBy")
                        .HasMaxLength(32);

                    b.Property<DateTimeOffset?>("DeletedDate");

                    b.Property<bool>("IsCuttingIn");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(32);

                    b.Property<DateTimeOffset?>("ModifiedDate");

                    b.Property<DateTimeOffset?>("ProcessDate");

                    b.Property<string>("RONo");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<int>("UENId");

                    b.Property<string>("UENNo");

                    b.Property<string>("UnitCode");

                    b.Property<int>("UnitId");

                    b.Property<string>("UnitName");

                    b.HasKey("Identity");

                    b.ToTable("GarmentPreparings");
                });
#pragma warning restore 612, 618
        }
    }
}
