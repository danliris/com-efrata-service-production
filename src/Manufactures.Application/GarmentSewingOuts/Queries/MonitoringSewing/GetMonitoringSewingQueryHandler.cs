using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.CostCalculationGarmentDataProductionReport;
using Infrastructure.External.DanLirisClient.Microservice;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.HOrderDataProductionReport;
using Manufactures.Domain.GarmentPreparings.Repositories;
using System.Text;
using System.Net.Http;
using Manufactures.Domain.GarmentCuttingIns.Repositories;

namespace Manufactures.Application.GarmentSewingOuts.Queries.MonitoringSewing
{
	public class GetMonitoringSewingQueryHandler : IQueryHandler<GetMonitoringSewingQuery, GarmentMonitoringSewingListViewModel>
	{
		protected readonly IHttpClientService _http;
		private readonly IStorage _storage;
		private readonly IGarmentSewingOutRepository garmentSewingOutRepository;
		private readonly IGarmentSewingOutItemRepository garmentSewingOutItemRepository;
		private readonly IGarmentLoadingRepository garmentLoadingRepository;
		private readonly IGarmentLoadingItemRepository garmentLoadingItemRepository;
		private readonly IGarmentPreparingRepository garmentPreparingRepository;
		private readonly IGarmentPreparingItemRepository garmentPreparingItemRepository;
		private readonly IGarmentBalanceSewingRepository garmentBalanceSewingRepository;
		private readonly IGarmentCuttingInRepository garmentCuttingInRepository;

		public GetMonitoringSewingQueryHandler(IStorage storage, IServiceProvider serviceProvider)
		{
			_storage = storage;
			garmentSewingOutRepository = storage.GetRepository<IGarmentSewingOutRepository>();
			garmentSewingOutItemRepository = storage.GetRepository<IGarmentSewingOutItemRepository>();
			garmentPreparingRepository = storage.GetRepository<IGarmentPreparingRepository>();
			garmentPreparingItemRepository = storage.GetRepository<IGarmentPreparingItemRepository>();
			garmentLoadingRepository = storage.GetRepository<IGarmentLoadingRepository>();
			garmentLoadingItemRepository = storage.GetRepository<IGarmentLoadingItemRepository>();
			garmentBalanceSewingRepository = storage.GetRepository<IGarmentBalanceSewingRepository>();
			garmentCuttingInRepository = storage.GetRepository<IGarmentCuttingInRepository>();
			_http = serviceProvider.GetService<IHttpClientService>();
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

        public async Task<CostCalculationGarmentDataProductionReport> GetDataCostCal(List<string> ro, string token)
        {
			CostCalculationGarmentDataProductionReport costCalculationGarmentDataProductionReport = new CostCalculationGarmentDataProductionReport();

			var listRO = string.Join(",", ro.Distinct());
			var costCalculationUri = SalesDataSettings.Endpoint + $"cost-calculation-garments/data/";

			var httpContent = new StringContent(JsonConvert.SerializeObject(listRO), Encoding.UTF8, "application/json");

			var httpResponse = await _http.SendAsync(HttpMethod.Get, costCalculationUri, token, httpContent);

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
			else
			{
				var err = await httpResponse.Content.ReadAsStringAsync();

			}

			return costCalculationGarmentDataProductionReport;
        }

		class monitoringView
		{
			public string roJob { get; internal set; }
			public string article { get; internal set; }
			public string buyerCode { get; internal set; }
			public double qtyOrder { get; internal set; }
			public double stock { get; internal set; }
			public string style { get; internal set; }
			public double sewingQtyPcs { get; internal set; }
			public double loadingQtyPcs { get; internal set; }
			public string uomUnit { get; internal set; }
			public double remainQty { get; internal set; }
			public decimal price { get; internal set; }
		}
		class ViewBasicPrices
		{
			public string RO { get; internal set; }
			public decimal BasicPrice { get; internal set; }
			public int Count { get; internal set; }
		}
		class ViewFC
		{
			public string RO { get; internal set; }
			public double FC { get; internal set; }
			public int Count { get; internal set; }
		}
		public async Task<GarmentMonitoringSewingListViewModel> Handle(GetMonitoringSewingQuery request, CancellationToken cancellationToken)
		{
			DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom, new TimeSpan(7, 0, 0));
			DateTimeOffset dateTo = new DateTimeOffset(request.dateTo, new TimeSpan(7, 0, 0));
			DateTimeOffset dateBalance = (from a in garmentBalanceSewingRepository.Query.OrderByDescending(s => s.CreatedDate)
										  select a.CreatedDate).FirstOrDefault();

			var QueryRoSewingOut = (from a in garmentSewingOutRepository.Query
									 where a.UnitId == request.unit && a.SewingOutDate <= dateTo
									 select a.RONo).Distinct();
			var QueryRoLoading = (from a in garmentLoadingRepository.Query
								  where a.UnitId == request.unit && a.LoadingDate <= dateTo
								  select a.RONo).Distinct();
			var QueryRo = QueryRoSewingOut.Union(QueryRoLoading).Distinct();
 
			var sumbasicPrice = (from a in garmentPreparingRepository.Query
								 join b in garmentPreparingItemRepository.Query on a.Identity equals b.GarmentPreparingId
								 /*where a.ProcessDate  <= dateTo(request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) &&
							   a.UnitId == request.unit*/
								 select new { a.RONo, b.BasicPrice })
					.GroupBy(x => new { x.RONo }, (key, group) => new ViewBasicPrices
					{
						RO = key.RONo,
						BasicPrice = Math.Round(Convert.ToDecimal(group.Sum(s => s.BasicPrice)), 2),
						Count = group.Count()
					});
			var sumFCs = (from a in garmentCuttingInRepository.Query
						  where /*(request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && */ a.CuttingType == "Main Fabric" //&&
						 /*a.UnitId == request.unit && a.CuttingInDate <= dateTo*/
						  select new { a.FC, a.RONo })
						 .GroupBy(x => new { x.RONo }, (key, group) => new ViewFC
						 {
							 RO = key.RONo,
							 FC = group.Sum(s => s.FC),
							 Count = group.Count()
						 });
			var queryBalanceSewing = from a in garmentBalanceSewingRepository.Query
									  where a.CreatedDate < dateFrom && a.UnitId == request.unit //&& a.RoJob == "2010810"
									  select new monitoringView { price = a.Price, buyerCode = a.BuyerCode, loadingQtyPcs = a.LoadingQtyPcs, remainQty = 0, stock = a.Stock, sewingQtyPcs = 0, roJob = a.RoJob, article = a.Article, qtyOrder = a.QtyOrder, style = a.Style, uomUnit = "PCS" };

			var QuerySewingOut = from a in (from aa in garmentSewingOutRepository.Query
											where aa.UnitId == request.unit && aa.SewingOutDate <= dateTo
											select aa)
											join b in garmentSewingOutItemRepository.Query on a.Identity equals b.SewingOutId
								  select new monitoringView { price = Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == a.RONo select aa.BasicPrice / aa.Count).FirstOrDefault()) , loadingQtyPcs = 0, uomUnit = "PCS", remainQty = 0, stock = a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance ? -b.Quantity : 0, sewingQtyPcs = a.SewingOutDate >= dateFrom ? b.Quantity : 0, roJob = a.RONo, article = a.Article    };
			var QueryLoading = from a in (from aa in garmentLoadingRepository.Query
										  where aa.UnitId == request.unit && aa.LoadingDate <= dateTo select aa)
										  join b in garmentLoadingItemRepository.Query on a.Identity equals b.LoadingId
							   select new monitoringView { price = Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == a.RONo select aa.BasicPrice / aa.Count).FirstOrDefault()), loadingQtyPcs = a.LoadingDate >= dateFrom ? b.Quantity : 0, sewingQtyPcs = 0, uomUnit = "PCS", remainQty = 0, stock = a.LoadingDate < dateFrom && a.LoadingDate > dateBalance ? b.Quantity : 0, roJob = a.RONo, article = a.Article };
			var queryNow = queryBalanceSewing.Union(QuerySewingOut).Union(QueryLoading);
			var querySum = queryNow.ToList().GroupBy(x => new { x.roJob, x.article, x.uomUnit }, (key, group) => new
			{
				 
				RoJob = key.roJob,
				Stock = group.Sum(s => s.stock),
				UomUnit = key.uomUnit,
				Article = key.article,
				SewingQtyPcs = group.Sum(s => s.sewingQtyPcs ),
				Loading = group.Sum(s => s.loadingQtyPcs)
			}).OrderBy(s => s.RoJob);
			GarmentMonitoringSewingListViewModel listViewModel = new GarmentMonitoringSewingListViewModel();
			List<GarmentMonitoringSewingDto> monitoringDtos = new List<GarmentMonitoringSewingDto>();
			foreach (var item in querySum)
			{
				GarmentMonitoringSewingDto dto = new GarmentMonitoringSewingDto
				{
					roJob = item.RoJob,
					article = item.Article,
					uomUnit = item.UomUnit,
					sewingOutQtyPcs = item.SewingQtyPcs,
					loadingQtyPcs = item.Loading,
					stock = item.Stock,
					remainQty = item.Stock + item.Loading - item.SewingQtyPcs
				};
				monitoringDtos.Add(dto);
			}
			listViewModel.garmentMonitorings = monitoringDtos;
			var data = from a in monitoringDtos
					   where a.stock > 0 || a.loadingQtyPcs > 0 || a.sewingOutQtyPcs > 0 || a.remainQty > 0
					   select a;
			var roList = (from a in data
						  select a.roJob).Distinct().ToList();
			var roBalance = from a in garmentBalanceSewingRepository.Query
							select new CostCalViewModel { comodityName = a.Style, buyerCode = a.BuyerCode, hours = a.Hours, qtyOrder = a.QtyOrder, ro = a.RoJob };

			CostCalculationGarmentDataProductionReport costCalculation = await GetDataCostCal(roList, request.token);

			foreach (var item in roBalance)
			{
				costCalculation.data.Add(item);
			}

			foreach (var garment in data)
			{
				garment.buyerCode = garment.buyerCode == null ? (from cost in costCalculation.data where cost.ro == garment.roJob select cost.buyerCode).FirstOrDefault() : garment.buyerCode;
				garment.style = garment.style == null ? (from cost in costCalculation.data where cost.ro == garment.roJob select cost.comodityName).FirstOrDefault() : garment.style;
				garment.price = Math.Round(Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == garment.roJob select aa.BasicPrice / aa.Count).FirstOrDefault()), 2) * Convert.ToDecimal((from cost in sumFCs where cost.RO == garment.roJob select cost.FC / cost.Count).FirstOrDefault()) == 0 ? Convert.ToDecimal((from a in queryBalanceSewing.ToList() where a.roJob == garment.roJob select a.price).FirstOrDefault()) : Math.Round(Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == garment.roJob select aa.BasicPrice / aa.Count).FirstOrDefault()), 2) * Convert.ToDecimal((from cost in sumFCs where cost.RO == garment.roJob select cost.FC / cost.Count).FirstOrDefault());
				garment.nominal = Math.Round((Convert.ToDecimal(garment.stock + garment.loadingQtyPcs - garment.sewingOutQtyPcs)) * garment.price, 2);
				garment.qtyOrder = (from cost in costCalculation.data where cost.ro == garment.roJob select cost.qtyOrder).FirstOrDefault();
			}
			listViewModel.garmentMonitorings = data.ToList();
			return listViewModel;
		}
	}
}
