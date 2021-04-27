using Manufactures.Domain.GarmentSubcon.SubconContracts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentSubconContracts.Configs
{
    public class GarmentSubconContractConfig : IEntityTypeConfiguration<GarmentSubconContractReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSubconContractReadModel> builder)
        {
            builder.ToTable("GarmentSubconContracts");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.AgreementNo).HasMaxLength(50);
            builder.Property(a => a.SupplierName).HasMaxLength(100);
            builder.Property(a => a.SupplierCode).HasMaxLength(25);
            builder.Property(a => a.BPJNo).HasMaxLength(50);
            builder.Property(a => a.ContractNo).HasMaxLength(50);
            builder.Property(a => a.ContractType).HasMaxLength(50);
            builder.Property(a => a.FinishedGoodType).HasMaxLength(50);
            builder.Property(a => a.JobType).HasMaxLength(50);

            builder.HasIndex(i => i.ContractNo).IsUnique().HasFilter("[Deleted]=(0)");

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}