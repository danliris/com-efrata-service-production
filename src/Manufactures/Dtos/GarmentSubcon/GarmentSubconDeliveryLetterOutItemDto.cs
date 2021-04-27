﻿using System;
using Manufactures.Domain.Shared.ValueObjects;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;

namespace Manufactures.Dtos.GarmentSubcon
{
    public class GarmentSubconDeliveryLetterOutItemDto : BaseDto
    {
        public GarmentSubconDeliveryLetterOutItemDto(GarmentSubconDeliveryLetterOutItem garmentSubconDeliveryLetterOutItem)
        {
            Id = garmentSubconDeliveryLetterOutItem.Identity;
            SubconDeliveryLetterOutId = garmentSubconDeliveryLetterOutItem.SubconDeliveryLetterOutId;
            UENItemId = garmentSubconDeliveryLetterOutItem.UENItemId;
            Product = new Product(garmentSubconDeliveryLetterOutItem.ProductId.Value, garmentSubconDeliveryLetterOutItem.ProductCode, garmentSubconDeliveryLetterOutItem.ProductName);
            ProductRemark = garmentSubconDeliveryLetterOutItem.ProductRemark;
            DesignColor = garmentSubconDeliveryLetterOutItem.DesignColor;
            Quantity = garmentSubconDeliveryLetterOutItem.Quantity;
            Uom = new Uom(garmentSubconDeliveryLetterOutItem.UomId.Value, garmentSubconDeliveryLetterOutItem.UomUnit);
            UomOut = new Uom(garmentSubconDeliveryLetterOutItem.UomOutId.Value, garmentSubconDeliveryLetterOutItem.UomOutUnit);
            FabricType = garmentSubconDeliveryLetterOutItem.FabricType;
            RONo = garmentSubconDeliveryLetterOutItem.RONo;
            POSerialNumber = garmentSubconDeliveryLetterOutItem.POSerialNumber;
            SubconCuttingOutId = garmentSubconDeliveryLetterOutItem.SubconCuttingOutId;
            SubconCuttingOutNo = garmentSubconDeliveryLetterOutItem.SubconCuttingOutNo;
        }

        public Guid Id { get; set; }
        public Guid SubconDeliveryLetterOutId { get; set; }
        public int UENItemId { get; set; }

        public Product Product { get; set; }
        public string ProductRemark { get; set; }

        public string DesignColor { get; set; }
        public double Quantity { get; set; }

        public Uom Uom { get; set; }
        public Uom UomOut { get; set; }

        public string FabricType { get; set; }

        #region Cutting
        public string RONo { get; set; }
        public Guid SubconCuttingOutId { get; set; }
        public string POSerialNumber { get; set; }
        public string SubconCuttingOutNo { get; set; }
        #endregion
        //public double ContractQuantity { get; set; }
    }
}
