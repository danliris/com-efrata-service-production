using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSubcon;
using Manufactures.Domain.GarmentSubcon.SubconContracts.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconContracts
{
    public class GarmentSubconContract : AggregateRoot<GarmentSubconContract, GarmentSubconContractReadModel>
    {
        public string ContractType { get; private set; }
        public string ContractNo { get; private set; }
        public string AgreementNo { get; private set; }
        public SupplierId SupplierId { get; private set; }
        public string SupplierCode { get; private set; }
        public string SupplierName { get; private set; }
        public string JobType { get; private set; }
        public string BPJNo { get; private set; }
        public string FinishedGoodType { get; private set; }
        public double Quantity { get; private set; }
        public DateTimeOffset DueDate { get; private set; }
        public DateTimeOffset ContractDate { get; private set; }
        public bool IsUsed { get; private set; }


        public GarmentSubconContract(GarmentSubconContractReadModel readModel) : base(readModel)
        {
            ContractType = readModel.ContractType;
            ContractNo = readModel.ContractNo;
            AgreementNo = readModel.AgreementNo;
            SupplierId = new SupplierId(readModel.SupplierId);
            SupplierCode = readModel.SupplierCode;
            SupplierName = readModel.SupplierName;
            JobType = readModel.JobType;
            BPJNo = readModel.BPJNo;
            FinishedGoodType = readModel.FinishedGoodType;
            Quantity = readModel.Quantity;
            DueDate = readModel.DueDate;
            ContractDate = readModel.ContractDate;
            IsUsed = readModel.IsUsed;
        }

        public GarmentSubconContract(Guid identity,string contractType, string contractNo, string agreementNo, SupplierId supplierId, string supplierCode, string supplierName, string jobType, string bPJNo, string finishedGoodType, double quantity, DateTimeOffset dueDate, DateTimeOffset contractDate, bool isUsed) : base(identity)
        {
            Identity = identity;
            ContractType = contractType;
            ContractNo = contractNo;
            AgreementNo = agreementNo;
            SupplierId = supplierId;
            SupplierCode = supplierCode;
            SupplierName = supplierName;
            JobType = jobType;
            BPJNo = bPJNo;
            FinishedGoodType = finishedGoodType;
            Quantity = quantity;
            DueDate = dueDate;
            ContractDate = contractDate;
            IsUsed = isUsed;

            ReadModel = new GarmentSubconContractReadModel(Identity)
            {
                ContractType = ContractType,
                ContractNo = ContractNo,
                AgreementNo = AgreementNo,
                SupplierId = SupplierId.Value,
                SupplierCode = SupplierCode,
                SupplierName = SupplierName,
                JobType = JobType,
                BPJNo = BPJNo,
                FinishedGoodType = FinishedGoodType,
                Quantity = Quantity,
                DueDate = DueDate,
                ContractDate= ContractDate,
                IsUsed= IsUsed
            };

            ReadModel.AddDomainEvent(new OnGarmentSubconContractPlaced(Identity));
        }

        protected override GarmentSubconContract GetEntity()
        {
            return this;
        }

        public void SetSupplierId(SupplierId SupplierId)
        {
            if (this.SupplierId != SupplierId)
            {
                this.SupplierId = SupplierId;
                ReadModel.SupplierId = SupplierId.Value;
            }
        }
        public void SetSupplierCode(string SupplierCode)
        {
            if (this.SupplierCode != SupplierCode)
            {
                this.SupplierCode = SupplierCode;
                ReadModel.SupplierCode = SupplierCode;
            }
        }
        public void SetSupplierName(string SupplierName)
        {
            if (this.SupplierName != SupplierName)
            {
                this.SupplierName = SupplierName;
                ReadModel.SupplierName = SupplierName;
            }
        }
        public void SetJobType(string JobType)
        {
            if (this.JobType != JobType)
            {
                this.JobType = JobType;
                ReadModel.JobType = JobType;
            }
        }
        public void SetContractNo(string ContractNo)
        {
            if (this.ContractNo != ContractNo)
            {
                this.ContractNo = ContractNo;
                ReadModel.ContractNo = ContractNo;
            }
        }
        public void SetContractType(string ContractType)
        {
            if (this.ContractType != ContractType)
            {
                this.ContractType = ContractType;
                ReadModel.ContractType = ContractType;
            }
        }
        public void SetAgreementNo(string AgreementNo)
        {
            if (this.AgreementNo != AgreementNo)
            {
                this.AgreementNo = AgreementNo;
                ReadModel.AgreementNo = AgreementNo;
            }
        }
        public void SetDueDate(DateTimeOffset DueDate)
        {
            if (this.DueDate != DueDate)
            {
                this.DueDate = DueDate;
                ReadModel.DueDate = DueDate;
            }
        }
        public void SetBPJNo(string BPJNo)
        {
            if (this.BPJNo != BPJNo)
            {
                this.BPJNo = BPJNo;
                ReadModel.BPJNo = BPJNo;
            }
        }
        public void SetFinishedGoodType(string FinishedGoodType)
        {
            if (this.FinishedGoodType != FinishedGoodType)
            {
                this.FinishedGoodType = FinishedGoodType;
                ReadModel.FinishedGoodType = FinishedGoodType;
            }
        }
        public void SetQuantity(double Quantity)
        {
            if (this.Quantity != Quantity)
            {
                this.Quantity = Quantity;
                ReadModel.Quantity = Quantity;
            }
        }
        public void SetContractDate(DateTimeOffset ContractDate)
        {
            if (this.ContractDate != ContractDate)
            {
                this.ContractDate = ContractDate;
                ReadModel.ContractDate = ContractDate;
            }
        }
        public void SetIsUsed(bool Isused)
        {
            if (this.IsUsed != Isused)
            {
                this.IsUsed = Isused;
                ReadModel.IsUsed = Isused;
            }
        }
        public void Modify()
        {
            MarkModified();
        }
    }
}
