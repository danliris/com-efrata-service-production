using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSubcon;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings
{
    public class GarmentServiceSubconCutting : AggregateRoot<GarmentServiceSubconCutting, GarmentServiceSubconCuttingReadModel>
    {

        public string SubconNo { get; private set; }
        public string SubconType { get; private set; }
        public UnitDepartmentId UnitId { get; private set; }
        public string UnitCode { get; private set; }
        public string UnitName { get; private set; }
        public DateTimeOffset SubconDate { get; private set; }

        public bool IsUsed { get; internal set; }

        public GarmentServiceSubconCutting(Guid identity, string subconNo, string subconType, UnitDepartmentId unitId, string unitCode, string unitName, DateTimeOffset subconDate, bool isUsed) : base(identity)
        {
            Identity = identity;
            SubconNo = subconNo;
            SubconType = subconType;
            UnitId = unitId;
            UnitCode = unitCode;
            UnitName = unitName;
            SubconDate = subconDate;
            IsUsed = isUsed;

            ReadModel = new GarmentServiceSubconCuttingReadModel(Identity)
            {
                SubconDate = SubconDate,
                SubconNo = SubconNo,
                SubconType = SubconType,
                UnitCode = UnitCode,
                UnitId = UnitId.Value,
                UnitName = UnitName,
                IsUsed=isUsed

            };

            ReadModel.AddDomainEvent(new OnServiceSubconCuttingPlaced(Identity));
        }

        public GarmentServiceSubconCutting(GarmentServiceSubconCuttingReadModel readModel) : base(readModel)
        {
            UnitName = readModel.UnitName;
            UnitId = new UnitDepartmentId(readModel.UnitId);
            UnitCode = readModel.UnitCode;
            SubconDate = readModel.SubconDate;
            SubconNo = readModel.SubconNo;
            SubconType = readModel.SubconType;
            IsUsed = readModel.IsUsed;
        }

        public void SetDate(DateTimeOffset subconDate)
        {
            if (subconDate != SubconDate)
            {
                SubconDate = subconDate;
                ReadModel.SubconDate = subconDate;

                MarkModified();
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

        protected override GarmentServiceSubconCutting GetEntity()
        {
            return this;
        }
    }
}
