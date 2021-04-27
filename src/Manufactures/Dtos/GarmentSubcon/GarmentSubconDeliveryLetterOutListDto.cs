using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSubcon
{
    public class GarmentSubconDeliveryLetterOutListDto : BaseDto
    {
        public GarmentSubconDeliveryLetterOutListDto(GarmentSubconDeliveryLetterOut garmentSubconDeliveryLetterOutList)
        {
            Id = garmentSubconDeliveryLetterOutList.Identity;
            DLNo = garmentSubconDeliveryLetterOutList.DLNo;
            DLType = garmentSubconDeliveryLetterOutList.DLType;
            ContractNo = garmentSubconDeliveryLetterOutList.ContractNo;
            ContractType = garmentSubconDeliveryLetterOutList.ContractType;
            DLDate = garmentSubconDeliveryLetterOutList.DLDate;
            UENNo = garmentSubconDeliveryLetterOutList.UENNo;
            Remark = garmentSubconDeliveryLetterOutList.Remark;
            CreatedBy = garmentSubconDeliveryLetterOutList.AuditTrail.CreatedBy;
            IsUsed = garmentSubconDeliveryLetterOutList.IsUsed;
        }

        public Guid Id { get; set; }
        public string DLNo { get; set; }
        public string DLType { get; set; }
        public string ContractNo { get; set; }
        public string ContractType { get; set; }
        public DateTimeOffset DLDate { get; set; }
        public string UENNo { get; set; }

        public string Remark { get; set; }
        public bool IsUsed { get; set; }
    }
}
