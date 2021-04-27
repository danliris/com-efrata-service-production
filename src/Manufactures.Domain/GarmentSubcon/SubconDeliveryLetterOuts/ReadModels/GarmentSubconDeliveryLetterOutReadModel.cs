using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ReadModels
{
    public class GarmentSubconDeliveryLetterOutReadModel : ReadModelBase
    {
        public GarmentSubconDeliveryLetterOutReadModel(Guid identity) : base(identity)
        {
        }
        public string DLNo { get; internal set; }
        public string DLType { get; internal set; }
        public Guid SubconContractId { get; internal set; }
        public string ContractNo { get; internal set; }
        public string ContractType { get; internal set; }
        public DateTimeOffset DLDate { get; internal set; }


        public int UENId { get; internal set; }
        public string UENNo { get; internal set; }
        public string PONo { get; internal set; }
        public int EPOItemId { get; internal set; }


        public string Remark { get; internal set; }
        public bool IsUsed { get; internal set; }
        public virtual List<GarmentSubconDeliveryLetterOutItemReadModel> GarmentSubconDeliveryLetterOutItem { get; internal set; }
    }
}