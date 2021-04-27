using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSewingOuts.Queries.MonitoringSewing
{
	public class GarmentMonitoringSewingDto
	{
		public GarmentMonitoringSewingDto()
		{
		}
	
		public string roJob { get; internal set; }
		public string article { get; internal set; }
		public string buyerCode { get; internal set; }
		public double qtyOrder { get; internal set; }
		public double stock { get; internal set; }
		public double sewingOutQtyPcs { get; internal set; }
		public double loadingQtyPcs { get; internal set; }
		public string uomUnit { get; internal set; }
		public string style { get; internal set; }
		public double remainQty { get; internal set; }
		public decimal price { get; internal set; }
		public decimal nominal { get; internal set; }
		public GarmentMonitoringSewingDto(GarmentMonitoringSewingDto garmentMonitoring)
		{
			 
			roJob = garmentMonitoring.roJob;
			article = garmentMonitoring.article;
			qtyOrder = garmentMonitoring.qtyOrder;
			stock = garmentMonitoring.stock;
			sewingOutQtyPcs = garmentMonitoring.sewingOutQtyPcs;
			loadingQtyPcs = garmentMonitoring.loadingQtyPcs;
			uomUnit = garmentMonitoring.uomUnit;
			remainQty = garmentMonitoring.remainQty;
			style = garmentMonitoring.style;
			price = garmentMonitoring.price;
			buyerCode = garmentMonitoring.buyerCode;
			nominal = garmentMonitoring.nominal;
		}
	}
}
