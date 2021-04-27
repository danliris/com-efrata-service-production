using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.CostCalculationGarmentDataProductionReport;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.HOrderDataProductionReport;
using Manufactures.Domain.GarmentExpenditureGoods.Repositories;
using System.IO;
using System.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Manufactures.Domain.GarmentCuttingIns.Repositories;

namespace Manufactures.Application.GarmentExpenditureGoods.Queries
{
	public class GetXlsExpenditureGoodQueryHandler : IQueryHandler<GetXlsExpenditureGoodQuery, MemoryStream>
	{
		protected readonly IHttpClientService _http;
		private readonly IStorage _storage;
		private readonly IGarmentExpenditureGoodRepository garmentExpenditureGoodRepository;
		private readonly IGarmentExpenditureGoodItemRepository garmentExpenditureGoodItemRepository;
		private readonly IGarmentPreparingRepository garmentPreparingRepository;
		private readonly IGarmentPreparingItemRepository garmentPreparingItemRepository;
		private readonly IGarmentCuttingInRepository garmentCuttingInRepository;

		public GetXlsExpenditureGoodQueryHandler(IStorage storage, IServiceProvider serviceProvider)
		{
			_storage = storage;
			garmentExpenditureGoodRepository = storage.GetRepository<IGarmentExpenditureGoodRepository>();
			garmentExpenditureGoodItemRepository = storage.GetRepository<IGarmentExpenditureGoodItemRepository>();
			garmentPreparingRepository = storage.GetRepository<IGarmentPreparingRepository>();
			garmentPreparingItemRepository = storage.GetRepository<IGarmentPreparingItemRepository>();
			garmentCuttingInRepository = storage.GetRepository<IGarmentCuttingInRepository>();
			_http = serviceProvider.GetService<IHttpClientService>();
		}

		class monitoringView
		{
			public string expenditureGoodNo { get; internal set; }
			public string expenditureGoodType { get; internal set; }
			public DateTimeOffset expenditureDate { get; internal set; }
			public string buyer { get; internal set; }
			public string roNo { get; internal set; }
			public string buyerArticle { get; internal set; }
			public string colour { get; internal set; }
			public string name { get; internal set; }
			public double qty { get; internal set; }
			public string invoice { get; internal set; }
			public decimal price { get; internal set; }
			public double fc { get; set; }
		}
		public async Task<CostCalculationGarmentDataProductionReport> GetDataCostCal(List<string> ro, string token)
		{
			CostCalculationGarmentDataProductionReport costCalculationGarmentDataProductionReport = new CostCalculationGarmentDataProductionReport();

			var listRO = string.Join(",", ro.Distinct());
			var costCalculationUri = SalesDataSettings.Endpoint + $"cost-calculation-garments/data/{listRO}";
			var httpResponse = await _http.GetAsync(costCalculationUri, token);

			var freeRO = new List<string>();

			if (httpResponse.IsSuccessStatusCode)
			{
				var contentString = await httpResponse.Content.ReadAsStringAsync();
				Dictionary<string, object> content = JsonConvert.DeserializeObject<Dictionary<string, object>>(contentString);
				var dataString = content.GetValueOrDefault("data").ToString();
				var listData = JsonConvert.DeserializeObject<List<CostCalViewModel>>(dataString);

				foreach (var item in ro)
				{
					var data = listData.SingleOrDefault(s => s.ro == item);
					if (data != null)
					{
						costCalculationGarmentDataProductionReport.data.Add(data);
					}
					else
					{
						freeRO.Add(item);
					}
				}
			}

			HOrderDataProductionReport hOrderDataProductionReport = await GetDataHOrder(freeRO, token);

			Dictionary<string, string> comodities = new Dictionary<string, string>();
			if (hOrderDataProductionReport.data.Count > 0)
			{
				var comodityCodes = hOrderDataProductionReport.data.Select(s => s.Kode).Distinct().ToList();
				var filter = "{\"(" + string.Join(" || ", comodityCodes.Select(s => "Code==" + "\\\"" + s + "\\\"")) + ")\" : \"true\"}";

				var masterGarmentComodityUri = MasterDataSettings.Endpoint + $"master/garment-comodities?filter=" + filter;
				var garmentComodityResponse = _http.GetAsync(masterGarmentComodityUri).Result;
				var garmentComodityResult = new GarmentComodityResult();
				if (garmentComodityResponse.IsSuccessStatusCode)
				{
					garmentComodityResult = JsonConvert.DeserializeObject<GarmentComodityResult>(garmentComodityResponse.Content.ReadAsStringAsync().Result);
					//comodities = garmentComodityResult.data.ToDictionary(d => d.Code, d => d.Name);
					foreach (var comodity in garmentComodityResult.data)
					{
						comodities[comodity.Code] = comodity.Name;
					}
				}
			}

			foreach (var hOrder in hOrderDataProductionReport.data)
			{
				costCalculationGarmentDataProductionReport.data.Add(new CostCalViewModel
				{
					ro = hOrder.No,
					buyerCode = hOrder.Codeby,
					comodityName = comodities.GetValueOrDefault(hOrder.Kode),
					hours = (double)hOrder.Sh_Cut,
					qtyOrder = (double)hOrder.Qty
				});
			}

			return costCalculationGarmentDataProductionReport;
		}
		async Task<HOrderDataProductionReport> GetDataHOrder(List<string> ro, string token)
		{
			HOrderDataProductionReport hOrderDataProductionReport = new HOrderDataProductionReport();

			var listRO = string.Join(",", ro.Distinct());
			var costCalculationUri = SalesDataSettings.Endpoint + $"local-merchandiser/horders/data-production-report-by-no/{listRO}";
			var httpResponse = await _http.GetAsync(costCalculationUri, token);

			if (httpResponse.IsSuccessStatusCode)
			{
				var contentString = await httpResponse.Content.ReadAsStringAsync();
				Dictionary<string, object> content = JsonConvert.DeserializeObject<Dictionary<string, object>>(contentString);
				var dataString = content.GetValueOrDefault("data").ToString();
				var listData = JsonConvert.DeserializeObject<List<HOrderViewModel>>(dataString);

				foreach (var item in ro)
				{
					var data = listData.SingleOrDefault(s => s.No == item);
					if (data != null)
					{
						hOrderDataProductionReport.data.Add(data);
					}
				}
			}

			return hOrderDataProductionReport;
		}
		class ViewFC
		{
			public string RO { get; internal set; }
			public double FC { get; internal set; }
			public int Count { get; internal set; }
		}
		class ViewBasicPrices
		{
			public string RO { get; internal set; }
			public decimal BasicPrice { get; internal set; }
			public int Count { get; internal set; }
		}
		public async Task<MemoryStream> Handle(GetXlsExpenditureGoodQuery request, CancellationToken cancellationToken)
		{
			DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom, new TimeSpan(7, 0, 0));
			DateTimeOffset dateTo = new DateTimeOffset(request.dateTo, new TimeSpan(7, 0, 0));


			var QueryRo = (from a in garmentExpenditureGoodRepository.Query
						   where a.UnitId == request.unit && a.ExpenditureDate >= dateFrom && a.ExpenditureDate <= dateTo
						   select a.RONo).Distinct();

			List<string> _ro = new List<string>();
			foreach (var item in QueryRo)
			{
				_ro.Add(item);
			}
			var _unitName = (from a in garmentPreparingRepository.Query
							 where a.UnitId == request.unit
							 select a.UnitName).FirstOrDefault();
			CostCalculationGarmentDataProductionReport costCalculation = await GetDataCostCal(_ro, request.token);
			GarmentMonitoringExpenditureGoodListViewModel listViewModel = new GarmentMonitoringExpenditureGoodListViewModel();
			List<GarmentMonitoringExpenditureGoodDto> monitoringDtos = new List<GarmentMonitoringExpenditureGoodDto>();
			var sumbasicPrice = (from a in garmentPreparingRepository.Query
								 join b in garmentPreparingItemRepository.Query on a.Identity equals b.GarmentPreparingId
								 where /*(request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) &&*/
								 a.UnitId == request.unit
								 select new { a.RONo, b.BasicPrice })
						.GroupBy(x => new { x.RONo }, (key, group) => new ViewBasicPrices
						{
							RO = key.RONo,
							BasicPrice = Convert.ToDecimal(group.Sum(s => s.BasicPrice)),
							Count = group.Count()
						});

			var sumFCs = (from a in garmentCuttingInRepository.Query
						  where /*(request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && */ a.CuttingType == "Main Fabric" &&
						 a.UnitId == request.unit && a.CuttingInDate <= dateTo
						  select new { a.FC, a.RONo })
						 .GroupBy(x => new { x.RONo }, (key, group) => new ViewFC
						 {
							 RO = key.RONo,
							 FC = group.Sum(s => s.FC),
							 Count = group.Count()
						 });
			var Query = from a in (from aa in garmentExpenditureGoodRepository.Query
								   where aa.UnitId == request.unit && aa.ExpenditureDate >= dateFrom && aa.ExpenditureDate <= dateTo
								   select aa)
						join b in garmentExpenditureGoodItemRepository.Query on a.Identity equals b.ExpenditureGoodId
						where a.UnitId == request.unit && a.ExpenditureDate >= dateFrom && a.ExpenditureDate <= dateTo
						select new monitoringView { fc = (from aa in sumFCs where aa.RO == a.RONo select aa.FC / aa.Count).FirstOrDefault(), price = Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == a.RONo select aa.BasicPrice / aa.Count).FirstOrDefault()), buyer = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), buyerArticle = a.BuyerCode + " " + a.Article, roNo = a.RONo, expenditureDate = a.ExpenditureDate, expenditureGoodNo = a.ExpenditureGoodNo, expenditureGoodType = a.ExpenditureType, invoice = a.Invoice, colour = b.Description, qty = b.Quantity, name = (from cost in costCalculation.data where cost.ro == a.RONo select cost.comodityName).FirstOrDefault() };



			var querySum = Query.ToList().GroupBy(x => new { x.fc, x.buyer, x.buyerArticle, x.roNo, x.expenditureDate, x.expenditureGoodNo, x.expenditureGoodType, x.invoice, x.colour, x.name }, (key, group) => new
			{
				ros = key.roNo,
				buyer = key.buyerArticle,
				expenditureDates = key.expenditureDate,
				qty = group.Sum(s => s.qty),
				expendituregoodNo = key.expenditureGoodNo,
				expendituregoodTypes = key.expenditureGoodType,
				color = key.colour,
				price = group.Sum(s => s.price),
				buyerC = key.buyer,
				names = key.name,
				invoices = key.invoice,
				fcs = key.fc

			}).OrderBy(s => s.expendituregoodNo);
			foreach (var item in querySum)
			{
				GarmentMonitoringExpenditureGoodDto dto = new GarmentMonitoringExpenditureGoodDto
				{
					roNo = item.ros,
					buyerArticle = item.buyer,
					expenditureGoodType = item.expendituregoodTypes,
					expenditureGoodNo = item.expendituregoodNo,
					expenditureDate = item.expenditureDates,
					qty = item.qty,
					colour = item.color,
					name = item.names,
					invoice = item.invoices,
					price = Math.Round(Convert.ToDecimal(Convert.ToDouble(Math.Round(item.price, 2)) * Math.Round(item.fcs, 2)), 2),
					buyerCode = item.buyerC,
					nominal = Math.Round( Convert.ToDecimal(item.qty) * Convert.ToDecimal(Convert.ToDouble(Math.Round(item.price, 2)) * Math.Round(item.fcs, 2)),2)

				};
				monitoringDtos.Add(dto);
			}
			var data = from a in monitoringDtos
					   where a.qty > 0
					   select a;
			monitoringDtos = data.ToList();
			double qty = 0;
			decimal nominal = 0;
			foreach (var item in data)
			{
				qty += item.qty;
				nominal += item.nominal;

			}
			GarmentMonitoringExpenditureGoodDto dtos = new GarmentMonitoringExpenditureGoodDto
			{
				roNo ="",
				buyerArticle = "",
				expenditureGoodType = "",
				expenditureGoodNo = "",
				expenditureDate=null, 
				qty = qty,
				colour = "",
				name = "",
				invoice = "",
				price = 0,
				buyerCode = "",
				nominal = nominal

			};
			monitoringDtos.Add(dtos);
			listViewModel.garmentMonitorings = monitoringDtos;
			var reportDataTable = new DataTable();
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "NO BON", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "TIPE PENGELUARAN", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "TGL", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "RO", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BUYER & ARTICLE", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "COLOUR", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "NAMA", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "HARGA (PCS)", DataType = typeof(decimal) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "QTY", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "NOMINAL", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "INVOICE", DataType = typeof(string) });
			int counter = 5;
			if (listViewModel.garmentMonitorings.Count > 0)
			{
				foreach (var report in listViewModel.garmentMonitorings)
				{	reportDataTable.Rows.Add(report.expenditureGoodNo, report.expenditureGoodType, report.expenditureDate.GetValueOrDefault().ToString("dd MMM yyy"), report.roNo, report.buyerArticle, report.colour, report.name, report.price, report.qty,report.nominal, report.invoice);
					counter++;
				}
			}
			using (var package = new ExcelPackage())
			{
				var worksheet = package.Workbook.Worksheets.Add("Sheet 1");
				worksheet.Cells["A" + 5 + ":K" + 5 + ""].Style.Font.Bold = true;
				worksheet.Cells["A1"].Value = "Report Barang Jadi "; worksheet.Cells["A" + 1 + ":K" + 1 + ""].Merge = true;
				worksheet.Cells["A2"].Value = "Periode " + dateFrom.ToString("dd-MM-yyyy") + " s/d " + dateTo.ToString("dd-MM-yyyy");
				worksheet.Cells["A3"].Value = "Konfeksi " + _unitName;
				worksheet.Cells["A" + 1 + ":K" + 1 + ""].Merge = true;
				worksheet.Cells["A" + 2 + ":K" + 2 + ""].Merge = true;
				worksheet.Cells["A" + 3 + ":K" + 3 + ""].Merge = true;
				worksheet.Cells["A" + 1 + ":K" + 3 + ""].Style.Font.Size = 15;
				worksheet.Cells["A" + 1 + ":K" + 5 + ""].Style.Font.Bold = true;
				worksheet.Cells["A" + 1 + ":K" + 5 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
				worksheet.Cells["A" + 1 + ":K" + 5 + ""].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
				worksheet.Cells["A5"].LoadFromDataTable(reportDataTable, true);
				worksheet.Column(8).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
				worksheet.Cells["H" + 2 + ":J" + counter + ""].Style.Numberformat.Format = "#,##0.00";
				worksheet.Column(9).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
				worksheet.Cells["I" + 2 + ":I" + counter + ""].Style.Numberformat.Format = "#,##0.00";
				worksheet.Cells["A" + 5 + ":K" + counter + ""].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
				worksheet.Cells["A" + 5 + ":K" + counter + ""].Style.Border.Top.Style = ExcelBorderStyle.Thin;
				worksheet.Cells["A" + 5 + ":K" + counter + ""].Style.Border.Left.Style = ExcelBorderStyle.Thin;
				worksheet.Cells["A" + 5 + ":K" + counter + ""].Style.Border.Right.Style = ExcelBorderStyle.Thin;
				worksheet.Cells["H" + (counter) + ":I" + (counter) + ""].Style.Font.Bold = true;
				worksheet.Cells["A" + 5 + ":K" + 5 + ""].Style.Font.Bold = true;
				var stream = new MemoryStream();
				if (request.type != "bookkeeping")
				{
					worksheet.Cells["A" + (counter) + ":H" + (counter) + ""].Merge = true;

					worksheet.Column(8).Hidden = true;
				}else
				{
					worksheet.Cells["A" + (counter) + ":G" + (counter) + ""].Merge = true;
				}
				package.SaveAs(stream);

				return stream;
			}
		}
	}
}
