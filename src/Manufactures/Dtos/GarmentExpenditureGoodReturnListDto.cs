using Manufactures.Domain.GarmentExpenditureGoodReturns;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
    public class GarmentExpenditureGoodReturnListDto : BaseDto
    {
        public GarmentExpenditureGoodReturnListDto(GarmentExpenditureGoodReturn garmentExpenditureGoodReturn)
        {
            Id = garmentExpenditureGoodReturn.Identity;
            ReturNo = garmentExpenditureGoodReturn.ReturNo;
            RONo = garmentExpenditureGoodReturn.RONo;
            Article = garmentExpenditureGoodReturn.Article;
            Unit = new UnitDepartment(garmentExpenditureGoodReturn.UnitId.Value, garmentExpenditureGoodReturn.UnitCode, garmentExpenditureGoodReturn.UnitName);
            ReturDate = garmentExpenditureGoodReturn.ReturDate;
            ReturType = garmentExpenditureGoodReturn.ReturType;
            Comodity = new GarmentComodity(garmentExpenditureGoodReturn.ComodityId.Value, garmentExpenditureGoodReturn.ComodityCode, garmentExpenditureGoodReturn.ComodityName);
            Buyer = new Buyer(garmentExpenditureGoodReturn.BuyerId.Value, garmentExpenditureGoodReturn.BuyerCode, garmentExpenditureGoodReturn.BuyerName);
            Invoice = garmentExpenditureGoodReturn.Invoice;
            CreatedBy = garmentExpenditureGoodReturn.AuditTrail.CreatedBy;
        }

        public Guid Id { get; internal set; }
        public string ReturNo { get; internal set; }
        public UnitDepartment Unit { get; internal set; }
        public string ReturType { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public GarmentComodity Comodity { get; internal set; }
        public Buyer Buyer { get; internal set; }
        public DateTimeOffset ReturDate { get; internal set; }
        public string Invoice { get; internal set; }
        public double TotalQuantity { get; set; }
    }
}
