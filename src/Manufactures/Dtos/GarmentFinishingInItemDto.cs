using Manufactures.Domain.GarmentFinishingIns;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
    public class GarmentFinishingInItemDto : BaseDto
    {
        public GarmentFinishingInItemDto(GarmentFinishingInItem garmentFinishingInItem)
        {
            Id = garmentFinishingInItem.Identity;
            Product = new Product(garmentFinishingInItem.ProductId.Value, garmentFinishingInItem.ProductCode, garmentFinishingInItem.ProductName);
            DesignColor = garmentFinishingInItem.DesignColor;
            Size = new SizeValueObject(garmentFinishingInItem.SizeId.Value, garmentFinishingInItem.SizeName);
            Quantity = garmentFinishingInItem.Quantity;
            Uom = new Uom(garmentFinishingInItem.UomId.Value, garmentFinishingInItem.UomUnit);
            Color = garmentFinishingInItem.Color;
            RemainingQuantity = garmentFinishingInItem.RemainingQuantity;
            SewingOutItemId = garmentFinishingInItem.SewingOutItemId;
            FinishingInId = garmentFinishingInItem.FinishingInId;
            BasicPrice = garmentFinishingInItem.BasicPrice;
            Price = garmentFinishingInItem.Price;
        }

        public Guid Id { get; set; }
        public Guid SewingOutItemId { get; set; }

        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public SizeValueObject Size { get; set; }
        public double Quantity { get; set; }
        public Uom Uom { get; set; }
        public string Color { get; set; }
        public double RemainingQuantity { get; set; }
        public Guid FinishingInId { get; set; }
        public double BasicPrice { get; set; }
        public double Price { get; set; }
    }
}
