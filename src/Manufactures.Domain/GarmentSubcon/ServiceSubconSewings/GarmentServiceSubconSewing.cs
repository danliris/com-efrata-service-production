using System;
using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSubcon;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconSewings
{
    public class GarmentServiceSubconSewing : AggregateRoot<GarmentServiceSubconSewing, GarmentServiceSubconSewingReadModel>
    {
        public string ServiceSubconSewingNo { get; private set; }
        public DateTimeOffset ServiceSubconSewingDate { get; private set; }
        public bool IsUsed { get; internal set; }
        //
        public GarmentServiceSubconSewing(Guid identity, string serviceSubconSewingNo, DateTimeOffset serviceSubconSewingDate, bool isUsed) : base(identity)
        {
            
            Identity = identity;
            ServiceSubconSewingNo = serviceSubconSewingNo;
            ServiceSubconSewingDate = serviceSubconSewingDate;
            IsUsed = isUsed;

            ReadModel = new GarmentServiceSubconSewingReadModel(Identity)
            {
                ServiceSubconSewingNo = ServiceSubconSewingNo,
                ServiceSubconSewingDate = ServiceSubconSewingDate,
                //UnitId = UnitId.Value,
                //UnitCode = UnitCode,
                //UnitName = UnitName,
                IsUsed = IsUsed
            };

            ReadModel.AddDomainEvent(new OnGarmentServiceSubconSewingPlaced(Identity));

        }

        public GarmentServiceSubconSewing(GarmentServiceSubconSewingReadModel readModel) : base(readModel)
        {
            ServiceSubconSewingNo = readModel.ServiceSubconSewingNo;
            ServiceSubconSewingDate = readModel.ServiceSubconSewingDate;
            //UnitId = new UnitDepartmentId(readModel.UnitId);
            //UnitCode = readModel.UnitCode;
            //UnitName = readModel.UnitName;
            IsUsed = readModel.IsUsed;
        }

        public void SetDate(DateTimeOffset ServiceSubconSewingDate)
        {
            if (this.ServiceSubconSewingDate != ServiceSubconSewingDate)
            {
                this.ServiceSubconSewingDate = ServiceSubconSewingDate;
                ReadModel.ServiceSubconSewingDate = ServiceSubconSewingDate;
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

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentServiceSubconSewing GetEntity()
        {
            return this;
        }
    }
}
