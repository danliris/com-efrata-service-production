using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentPreparings.Repositories;
using System;
using System.Collections.Generic;
using System.Linq; 
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.External.DanLirisClient.Microservice;
using Newtonsoft.Json;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using ExtCore.Data.Abstractions;
using Manufactures.Domain.GarmentPreparings.ReadModels;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.ExpenditureROResult;
using Microsoft.Extensions.DependencyInjection;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentAvalProducts.Repositories;
using Manufactures.Domain.GarmentDeliveryReturns.Repositories;
using System.IO;
using System.Data;
using OfficeOpenXml;

namespace Manufactures.Application.GarmentPreparings.Queries.GetMonitoringPrepare
{
	public class GetMonitoringPrepareQueryHandler : IQueryHandler<GetMonitoringPrepareQuery, GarmentMonitoringPrepareListViewModel>
	{
		protected readonly IHttpClientService _http;
		private readonly IStorage _storage;

		private readonly IGarmentPreparingRepository garmentPreparingRepository;
		private readonly IGarmentPreparingItemRepository garmentPreparingItemRepository;
		private readonly IGarmentCuttingInRepository garmentCuttingInRepository;
		private readonly IGarmentCuttingInItemRepository garmentCuttingInItemRepository;
		private readonly IGarmentCuttingInDetailRepository garmentCuttingInDetailRepository;
		private readonly IGarmentAvalProductRepository garmentAvalProductRepository;
		private readonly IGarmentAvalProductItemRepository garmentAvalProductItemRepository;
		private readonly IGarmentDeliveryReturnRepository garmentDeliveryReturnRepository;
		private readonly IGarmentDeliveryReturnItemRepository garmentDeliveryReturnItemRepository;

		public GetMonitoringPrepareQueryHandler(IStorage storage, IServiceProvider serviceProvider)
		{
			_storage = storage;
			garmentPreparingRepository = storage.GetRepository<IGarmentPreparingRepository>();
			garmentPreparingItemRepository = storage.GetRepository<IGarmentPreparingItemRepository>();
			garmentCuttingInRepository = storage.GetRepository<IGarmentCuttingInRepository>();
			garmentCuttingInItemRepository = storage.GetRepository<IGarmentCuttingInItemRepository>();
			garmentCuttingInDetailRepository = storage.GetRepository<IGarmentCuttingInDetailRepository>();
			garmentAvalProductRepository = storage.GetRepository<IGarmentAvalProductRepository>();
			garmentAvalProductItemRepository = storage.GetRepository<IGarmentAvalProductItemRepository>();
			garmentDeliveryReturnRepository = storage.GetRepository<IGarmentDeliveryReturnRepository>();
			garmentDeliveryReturnItemRepository = storage.GetRepository<IGarmentDeliveryReturnItemRepository>();
			_http = serviceProvider.GetService<IHttpClientService>();
		}

		public async Task<ExpenditureROResult> GetExpenditureById(List<int> id, string token)
		{
			List<ExpenditureROViewModel> expenditureRO = new List<ExpenditureROViewModel>();

			ExpenditureROResult expenditureROResult = new ExpenditureROResult();
			foreach (var item in id)
			{
				var garmentUnitExpenditureNoteUri = PurchasingDataSettings.Endpoint + $"garment-unit-expenditure-notes/ro-asal/{item}";
				var httpResponse = await _http.GetAsync(garmentUnitExpenditureNoteUri, token);

				if (httpResponse.IsSuccessStatusCode)
				{
					var a = await httpResponse.Content.ReadAsStringAsync();
					Dictionary<string, object> keyValues = JsonConvert.DeserializeObject<Dictionary<string, object>>(a);
					var b = keyValues.GetValueOrDefault("data");

					var expenditure = JsonConvert.DeserializeObject<ExpenditureROViewModel>(keyValues.GetValueOrDefault("data").ToString());
					ExpenditureROViewModel expenditureROViewModel = new ExpenditureROViewModel
					{
						ROAsal = expenditure.ROAsal,
						DetailExpenditureId = expenditure.DetailExpenditureId,
						BuyerCode=expenditure.BuyerCode
					};
					expenditureRO.Add(expenditureROViewModel);
				}
				else
				{
					//await GetExpenditureById(id, token);
				}
			}
			expenditureROResult.data = expenditureRO;
			return expenditureROResult;
		}

		class monitoringView
		{
			public string roJob { get; set; }
			public string article { get; set; }
			public string buyerCode { get; set; }
			public string productCode { get; set; }
			public string uomUnit { get; set; }
			public string roAsal { get; set; }
			public string remark { get; set; }
			public double stock { get; set; }
			public double receipt { get; set; }
			public double mainFabricExpenditure { get; set; }
			public double nonMainFabricExpenditure { get; set; }
			public double expenditure { get; set; }
			public double aval { get; set; }
			public double remainQty { get; set; }
			public decimal price { get; set; }
			public Guid prepareItemId { get; set; }
		}
		class ViewBasicPrices
		{
			public string RO { get; internal set; }
			public decimal BasicPrice { get; internal set; }
			public int Count { get; internal set; }
		}
		public async Task<GarmentMonitoringPrepareListViewModel> Handle(GetMonitoringPrepareQuery request, CancellationToken cancellationToken)
		{
			DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom, new TimeSpan(7,0,0));
			DateTimeOffset dateTo = new DateTimeOffset(request.dateTo, new TimeSpan(7, 0, 0));
			var QueryMutationPrepareNow = from a in ( from aa in garmentPreparingRepository.Query
													  where aa.UnitId == request.unit && aa.ProcessDate <= dateTo
													  select aa)
													  join b in garmentPreparingItemRepository.Query on a.Identity equals b.GarmentPreparingId
										   select new { Buyer=a.BuyerCode,RO = a.RONo, Articles = a.Article, Id = a.Identity, DetailExpend = b.UENItemId, Processdate = a.ProcessDate };
			//List<int> detailExpendId = new List<int>();
			//foreach (var item in QueryMutationPrepareNow.Distinct())
			//{
			//	detailExpendId.Add(item.DetailExpend);
			//}
			//ExpenditureROResult dataExpenditure = await GetExpenditureById(detailExpendId, request.token);

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
			var QueryMutationPrepareItemsROASAL = (from a in QueryMutationPrepareNow
												   join b in garmentPreparingItemRepository.Query on a.Id equals b.GarmentPreparingId
                           
												   where b.UENItemId == a.DetailExpend
												   select new { article = a.Articles, roJob = a.RO, buyerCode = a.Buyer, price = Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == a.RO select aa.BasicPrice / aa.Count).FirstOrDefault()), prepareitemid = b.Identity, roasal = b.ROSource });


			var QueryCuttingDONow = from a in (from data in garmentCuttingInRepository.Query where data.UnitId == request.unit && data.CuttingInDate <= dateTo select data)
									join b in garmentCuttingInItemRepository.Query on a.Identity equals b.CutInId
									join c in garmentCuttingInDetailRepository.Query on b.Identity equals c.CutInItemId
									select new monitoringView { prepareItemId = c.PreparingItemId, price = Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == a.RONo select aa.BasicPrice / aa.Count).FirstOrDefault()), expenditure = 0, aval = 0, uomUnit = "", stock = a.CuttingInDate < dateFrom ? -c.PreparingQuantity : 0, nonMainFabricExpenditure = a.CuttingType == "Non Main Fabric" && (a.CuttingInDate >= dateFrom) ? c.PreparingQuantity : 0, mainFabricExpenditure = a.CuttingType == "Main Fabric" && (a.CuttingInDate >= dateFrom) ? c.PreparingQuantity : 0, remark = c.DesignColor, receipt = 0, productCode = c.ProductCode, remainQty = 0 };

			var QueryMutationPrepareItemNow = (from d in QueryMutationPrepareNow
											   join e in garmentPreparingItemRepository.Query on d.Id equals e.GarmentPreparingId
											   //join c in dataExpenditure.data on e.UENItemId equals c.DetailExpenditureId
											   where e.UENItemId == d.DetailExpend
											   select new monitoringView { prepareItemId = e.Identity, price = Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == d.RO select aa.BasicPrice / aa.Count).FirstOrDefault()), uomUnit = "", stock = d.Processdate < dateFrom ? e.Quantity : 0, mainFabricExpenditure = 0, nonMainFabricExpenditure = 0, remark = e.DesignColor, receipt = (d.Processdate >= dateFrom ? e.Quantity : 0), productCode = e.ProductCode, remainQty = e.RemainingQuantity }).Distinct();

			var QueryAval = from a in (from data in garmentAvalProductRepository.Query where data.AvalDate <= dateTo select data)
							join b in garmentAvalProductItemRepository.Query on a.Identity equals b.APId
							join c in garmentPreparingItemRepository.Query on Guid.Parse(b.PreparingItemId) equals c.Identity
							join d in (from data in garmentPreparingRepository.Query where data.UnitId == request.unit select data) on c.GarmentPreparingId equals d.Identity
							select new monitoringView { prepareItemId = c.Identity, price = Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == a.RONo select aa.BasicPrice / aa.Count).FirstOrDefault()), expenditure = 0, aval = a.AvalDate >= dateFrom ? b.Quantity : 0, uomUnit = "", stock = a.AvalDate < dateFrom ? -b.Quantity : 0, mainFabricExpenditure = 0, nonMainFabricExpenditure = 0, remark = b.DesignColor, receipt = 0, productCode = b.ProductCode, remainQty = 0 };

			var QueryDeliveryReturn = from a in (from data in garmentDeliveryReturnRepository.Query where data.ReturnDate <= dateTo && data.UnitId == request.unit select data)
									  join b in garmentDeliveryReturnItemRepository.Query on a.Identity equals b.DRId
									  join c in garmentPreparingItemRepository.Query on Guid.Parse(b.PreparingItemId) equals (c.Identity)
									  select new monitoringView { prepareItemId = c.Identity, price = Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == a.RONo select aa.BasicPrice / aa.Count).FirstOrDefault()), expenditure = a.ReturnDate >= dateFrom ? b.Quantity : 0, aval = 0, uomUnit = "", stock = a.ReturnDate < dateFrom ? -b.Quantity : 0, mainFabricExpenditure = 0, nonMainFabricExpenditure = 0, remark = b.DesignColor, receipt = 0, productCode = b.ProductCode, remainQty = 0 };

			var queryNow = from a in (QueryMutationPrepareItemNow
							.Union(QueryCuttingDONow)
							.Union(QueryAval)
							.Union(QueryDeliveryReturn).AsEnumerable())
						   join b in QueryMutationPrepareItemsROASAL on a.prepareItemId equals b.prepareitemid
						   select new { a, b };


			var querySum = queryNow.GroupBy(x => new {x.a.price, x.b.roasal, x.b.roJob, x.b.article, x.b.buyerCode, x.a.productCode, x.a.remark }, (key, group) => new
			{
				ROAsal = key.roasal,
				ROJob = key.roJob,
				stock = group.Sum(s => s.a.stock),
				ProductCode = key.productCode,
				Article = key.article,
				buyer = key.buyerCode,
				Remark = key.remark,
				Price = key.price,
				mainFabricExpenditure = group.Sum(s => s.a.mainFabricExpenditure),
				nonmainFabricExpenditure = group.Sum(s => s.a.nonMainFabricExpenditure),
				receipt = group.Sum(s => s.a.receipt),
				Aval = group.Sum(s => s.a.aval),
				drQty = group.Sum(s => s.a.expenditure)
			}).Where(s=> s.Price >0).OrderBy(s => s.ROJob);


			GarmentMonitoringPrepareListViewModel garmentMonitoringPrepareListViewModel = new GarmentMonitoringPrepareListViewModel();
			List<GarmentMonitoringPrepareDto> monitoringPrepareDtos = new List<GarmentMonitoringPrepareDto>();
			foreach (var item in querySum)
			{
				GarmentMonitoringPrepareDto garmentMonitoringPrepareDto = new GarmentMonitoringPrepareDto()
				{
					article = item.Article,
					roJob = item.ROJob,
					productCode = item.ProductCode,
					roAsal = item.ROAsal,
					uomUnit = "MT",
					remainQty = Math.Round(item.stock + item.receipt - item.nonmainFabricExpenditure - item.mainFabricExpenditure - item.Aval - item.drQty, 2),
					stock = Math.Round(item.stock, 2),
					remark = item.Remark,
					receipt = Math.Round(item.receipt, 2),
					aval = Math.Round(item.Aval, 2),
					nonMainFabricExpenditure = Math.Round(item.nonmainFabricExpenditure, 2),
					mainFabricExpenditure = Math.Round(item.mainFabricExpenditure, 2),
					expenditure = Math.Round(item.drQty, 2),
					price = Math.Round(item.Price, 2),
					buyerCode = item.buyer,
					nominal = (item.stock + item.receipt - item.nonmainFabricExpenditure - item.mainFabricExpenditure - item.Aval - item.drQty) * Convert.ToDouble(item.Price)

				};
				monitoringPrepareDtos.Add(garmentMonitoringPrepareDto);
			}
			var datas = from aa in monitoringPrepareDtos
						where Math.Round(aa.stock, 2) > 0 || Math.Round(aa.receipt, 2) > 0 || Math.Round(aa.aval, 2) > 0 || Math.Round(aa.mainFabricExpenditure, 2) > 0 || Math.Round(aa.nonMainFabricExpenditure, 2) > 0 || Math.Round(aa.remainQty, 2) > 0
						select aa;
			monitoringPrepareDtos = datas.ToList();
			garmentMonitoringPrepareListViewModel.garmentMonitorings = monitoringPrepareDtos;

			return garmentMonitoringPrepareListViewModel;
		}
	}
}