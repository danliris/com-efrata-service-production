using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSubcon;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts
{
    public class GarmentSubconDeliveryLetterOut : AggregateRoot<GarmentSubconDeliveryLetterOut, GarmentSubconDeliveryLetterOutReadModel>
    {

        public string DLNo { get; private set; }
        public string DLType { get; private set; }
        public Guid SubconContractId { get; private set; }
        public string ContractNo { get; private set; }
        public string ContractType { get; private set; }
        public DateTimeOffset DLDate { get; private set; }

        public int UENId { get; private set; }
        public string UENNo { get; private set; }

        public string PONo { get; private set; }
        public int EPOItemId { get; private set; }

        public string Remark { get; private set; }

        public bool IsUsed { get; private set; }

        public GarmentSubconDeliveryLetterOut(Guid identity, string dLNo, string dLType, Guid subconContractId, string contractNo, string contractType, DateTimeOffset dLDate, int uENId, string uENNo, string pONo, int ePOItemId, string remark, bool isUsed) : base(identity)
        {
            Identity = identity;
            DLNo = dLNo;
            DLType = dLType;
            SubconContractId = subconContractId;
            ContractNo = contractNo;
            ContractType = contractType;
            DLDate = dLDate;
            UENId = uENId;
            UENNo = uENNo;
            PONo = pONo;
            EPOItemId = ePOItemId;
            Remark = remark;
            IsUsed = isUsed;
            ReadModel = new GarmentSubconDeliveryLetterOutReadModel(Identity)
            {
                DLDate=DLDate,
                DLNo= DLNo,
                DLType=DLType,
                SubconContractId= SubconContractId,
                ContractNo=ContractNo,
                ContractType=ContractType,
                UENId=UENId,
                UENNo=UENNo,
                PONo=PONo,
                EPOItemId=EPOItemId,
                Remark=Remark,
                IsUsed = isUsed

            };

            ReadModel.AddDomainEvent(new OnGarmentSubconDeliveryLetterOutPlaced(Identity));
        }

        public GarmentSubconDeliveryLetterOut(GarmentSubconDeliveryLetterOutReadModel readModel) : base(readModel)
        {
            DLDate = readModel.DLDate;
            Remark = readModel.Remark;
            EPOItemId = readModel.EPOItemId;
            PONo = readModel.PONo;
            UENNo = readModel.UENNo;
            UENId = readModel.UENId;
            ContractType = readModel.ContractType;
            ContractNo = readModel.ContractNo;
            SubconContractId = readModel.SubconContractId;
            DLType = readModel.DLType;
            DLNo = readModel.DLNo;
            IsUsed = readModel.IsUsed;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSubconDeliveryLetterOut GetEntity()
        {
            return this;
        }

        public void SetDate(DateTimeOffset DLDate)
        {
            if (this.DLDate != DLDate)
            {
                this.DLDate = DLDate;
                ReadModel.DLDate = DLDate;
            }
        }

        public void SetIsUsed(bool isUsed)
        {
            if (isUsed != IsUsed)
            {
                IsUsed = isUsed;
                ReadModel.IsUsed = isUsed;

                MarkModified();
            }
        }

        //public void SetPONo(string poNo)
        //{
        //    if(this.PONo != poNo)
        //    {
        //        this.PONo = poNo;
        //        ReadModel.PONo = poNo;

        //        MarkModified();
        //    }
        //}

        //public void SetEPOItemId(int ePOItemId)
        //{
        //    if(this.EPOItemId != ePOItemId)
        //    {
        //        this.EPOItemId = ePOItemId;
        //        ReadModel.EPOItemId = ePOItemId;

        //        MarkModified();
        //    }
        //}

        public void SetRemark(string remark)
        {
            if(this.Remark != remark)
            {
                this.Remark = remark;
                ReadModel.Remark = remark;

                MarkModified();
            }
        }
    }
}
