﻿using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using System;
using ExtCore.Data.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using Newtonsoft.Json;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.CostCalculationGarmentDataProductionReport;
using Infrastructure.External.DanLirisClient.Microservice;
using System.Linq;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.HOrderDataProductionReport;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentAvalComponents.Repositories;
using Manufactures.Domain.GarmentAdjustments.Repositories;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentExpenditureGoods.Repositories;
using Manufactures.Domain.GarmentExpenditureGoodReturns.Repositories;
using System.IO;
using System.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentPreparings.Repositories;
using System.Net.Http;
using System.Text;
using Manufactures.Domain.MonitoringProductionStockFlow;

namespace Manufactures.Application.GarmentMonitoringProductionStockFlows.Queries
{
	public class GetXlsMonitoringProductionStockFlowQueryHandler : IQueryHandler<GetXlsMonitoringProductionStockFlowQuery, MemoryStream>
	{
		protected readonly IHttpClientService _http;
		private readonly IStorage _storage;
		private readonly IGarmentCuttingOutRepository garmentCuttingOutRepository;
		private readonly IGarmentCuttingOutItemRepository garmentCuttingOutItemRepository;
		private readonly IGarmentCuttingOutDetailRepository garmentCuttingOutDetailRepository;
		private readonly IGarmentCuttingInRepository garmentCuttingInRepository;
		private readonly IGarmentCuttingInItemRepository garmentCuttingInItemRepository;
		private readonly IGarmentCuttingInDetailRepository garmentCuttingInDetailRepository;
		private readonly IGarmentLoadingRepository garmentLoadingRepository;
		private readonly IGarmentLoadingItemRepository garmentLoadingItemRepository;
		private readonly IGarmentSewingInRepository garmentSewingInRepository;
		private readonly IGarmentSewingInItemRepository garmentSewingInItemRepository;
		private readonly IGarmentAvalComponentRepository garmentAvalComponentRepository;
		private readonly IGarmentAvalComponentItemRepository garmentAvalComponentItemRepository;
		private readonly IGarmentAdjustmentRepository garmentAdjustmentRepository;
		private readonly IGarmentAdjustmentItemRepository garmentAdjustmentItemRepository;
		private readonly IGarmentSewingOutRepository garmentSewingOutRepository;
		private readonly IGarmentSewingOutItemRepository garmentSewingOutItemRepository;
		private readonly IGarmentFinishingOutRepository garmentFinishingOutRepository;
		private readonly IGarmentFinishingOutItemRepository garmentFinishingOutItemRepository;
		private readonly IGarmentFinishingInRepository garmentFinishingInRepository;
		private readonly IGarmentFinishingInItemRepository garmentFinishingInItemRepository;
		private readonly IGarmentExpenditureGoodRepository garmentExpenditureGoodRepository;
		private readonly IGarmentExpenditureGoodItemRepository garmentExpenditureGoodItemRepository;
		private readonly IGarmentExpenditureGoodReturnRepository garmentExpenditureGoodReturnRepository;
		private readonly IGarmentExpenditureGoodReturnItemRepository garmentExpenditureGoodReturnItemRepository;
		private readonly IGarmentSewingDORepository garmentSewingDORepository;
		private readonly IGarmentSewingDOItemRepository garmentSewingDOItemRepository;
		private readonly IGarmentComodityPriceRepository garmentComodityPriceRepository;
		private readonly IGarmentPreparingRepository garmentPreparingRepository;
        private readonly IGarmentBalanceMonitoringProductionStockFlowRepository garmentBalanceProductionStockRepository;
        private readonly IGarmentPreparingItemRepository garmentPreparingItemRepository;
		public GetXlsMonitoringProductionStockFlowQueryHandler(IStorage storage, IServiceProvider serviceProvider)
		{
			_storage = storage;
            garmentBalanceProductionStockRepository = storage.GetRepository<IGarmentBalanceMonitoringProductionStockFlowRepository>();
            garmentCuttingOutRepository = storage.GetRepository<IGarmentCuttingOutRepository>();
			garmentCuttingOutItemRepository = storage.GetRepository<IGarmentCuttingOutItemRepository>();
			garmentCuttingOutDetailRepository = storage.GetRepository<IGarmentCuttingOutDetailRepository>();
			garmentCuttingInRepository = storage.GetRepository<IGarmentCuttingInRepository>();
			garmentCuttingInItemRepository = storage.GetRepository<IGarmentCuttingInItemRepository>();
			garmentCuttingInDetailRepository = storage.GetRepository<IGarmentCuttingInDetailRepository>();
			garmentLoadingRepository = storage.GetRepository<IGarmentLoadingRepository>();
			garmentLoadingItemRepository = storage.GetRepository<IGarmentLoadingItemRepository>();
			garmentSewingInRepository = storage.GetRepository<IGarmentSewingInRepository>();
			garmentSewingInItemRepository = storage.GetRepository<IGarmentSewingInItemRepository>();
			garmentAvalComponentRepository = storage.GetRepository<IGarmentAvalComponentRepository>();
			garmentAvalComponentItemRepository = storage.GetRepository<IGarmentAvalComponentItemRepository>();
			garmentLoadingRepository = storage.GetRepository<IGarmentLoadingRepository>();
			garmentLoadingItemRepository = storage.GetRepository<IGarmentLoadingItemRepository>();
			garmentAdjustmentRepository = storage.GetRepository<IGarmentAdjustmentRepository>();
			garmentAdjustmentItemRepository = storage.GetRepository<IGarmentAdjustmentItemRepository>();
			garmentSewingOutRepository = storage.GetRepository<IGarmentSewingOutRepository>();
			garmentSewingOutItemRepository = storage.GetRepository<IGarmentSewingOutItemRepository>();
			garmentFinishingOutRepository = storage.GetRepository<IGarmentFinishingOutRepository>();
			garmentFinishingOutItemRepository = storage.GetRepository<IGarmentFinishingOutItemRepository>();
			garmentFinishingInRepository = storage.GetRepository<IGarmentFinishingInRepository>();
			garmentFinishingInItemRepository = storage.GetRepository<IGarmentFinishingInItemRepository>();
			garmentExpenditureGoodRepository = storage.GetRepository<IGarmentExpenditureGoodRepository>();
			garmentExpenditureGoodItemRepository = storage.GetRepository<IGarmentExpenditureGoodItemRepository>();
			garmentExpenditureGoodReturnRepository = storage.GetRepository<IGarmentExpenditureGoodReturnRepository>();
			garmentExpenditureGoodReturnItemRepository = storage.GetRepository<IGarmentExpenditureGoodReturnItemRepository>();
			garmentSewingDORepository = storage.GetRepository<IGarmentSewingDORepository>();
			garmentSewingDOItemRepository = storage.GetRepository<IGarmentSewingDOItemRepository>();
			garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
			garmentPreparingRepository = storage.GetRepository<IGarmentPreparingRepository>();
			garmentPreparingItemRepository = storage.GetRepository<IGarmentPreparingItemRepository>();
			_http = serviceProvider.GetService<IHttpClientService>();
		}


		class monitoringView
		{
			public string Ro { get; internal set; }
			public string BuyerCode { get; internal set; }
			public string Article { get; internal set; }
			public string Comodity { get; internal set; }
			public double QtyOrder { get; internal set; }
			public double BasicPrice { get; internal set; }
			public decimal Fare { get; internal set; }
			public double FC { get; internal set; }
			public double Hours { get; internal set; }
			public double BeginingBalanceCuttingQty { get; internal set; }
			public double BeginingBalanceCuttingPrice { get; internal set; }
			public double QtyCuttingIn { get; internal set; }
			public double PriceCuttingIn { get; internal set; }
			public double QtyCuttingOut { get; internal set; }
			public double PriceCuttingOut { get; internal set; }
			public double QtyCuttingTransfer { get; internal set; }
			public double PriceCuttingTransfer { get; internal set; }
			public double QtyCuttingsubkon { get; internal set; }
			public double PriceCuttingsubkon { get; internal set; }
			public double AvalCutting { get; internal set; }
			public double AvalCuttingPrice { get; internal set; }
			public double AvalSewing { get; internal set; }
			public double AvalSewingPrice { get; internal set; }
			public double EndBalancCuttingeQty { get; internal set; }
			public double EndBalancCuttingePrice { get; internal set; }
			public double BeginingBalanceLoadingQty { get; internal set; }
			public double BeginingBalanceLoadingPrice { get; internal set; }
			public double QtyLoadingIn { get; internal set; }
			public double PriceLoadingIn { get; internal set; }
			public double QtyLoading { get; internal set; }
			public double PriceLoading { get; internal set; }
			public double QtyLoadingInTransfer { get; internal set; }
			public double PriceLoadingInTransfer { get; internal set; }
			public double QtyLoadingAdjs { get; internal set; }
			public double PriceLoadingAdjs { get; internal set; }
			public double EndBalanceLoadingQty { get; internal set; }
			public double EndBalanceLoadingPrice { get; internal set; }
			public double BeginingBalanceSewingQty { get; internal set; }
			public double BeginingBalanceSewingPrice { get; internal set; }
			public double QtySewingIn { get; internal set; }
			public double PriceSewingIn { get; internal set; }
			public double QtySewingOut { get; internal set; }
			public double PriceSewingOut { get; internal set; }
			public double QtySewingInTransfer { get; internal set; }
			public double PriceSewingInTransfer { get; internal set; }
			public double WipSewingOut { get; internal set; }
			public double WipSewingOutPrice { get; internal set; }
			public double WipFinishingOut { get; internal set; }
			public double WipFinishingOutPrice { get; internal set; }
			public double QtySewingRetur { get; internal set; }
			public double PriceSewingRetur { get; internal set; }
			public double QtySewingAdj { get; internal set; }
			public double PriceSewingAdj { get; internal set; }
			public double EndBalanceSewingQty { get; internal set; }
			public double EndBalanceSewingPrice { get; internal set; }
			public double BeginingBalanceFinishingQty { get; internal set; }
			public double BeginingBalanceFinishingPrice { get; internal set; }
			public double FinishingInQty { get; internal set; }
			public double FinishingInPrice { get; internal set; }
			public double BeginingBalanceSubconQty { get; internal set; }
			public double BeginingBalanceSubconPrice { get; internal set; }
			public double SubconInQty { get; internal set; }
			public double SubconInPrice { get; internal set; }
			public double SubconOutQty { get; internal set; }
			public double SubconOutPrice { get; internal set; }
			public double EndBalanceSubconQty { get; internal set; }
			public double EndBalanceSubconPrice { get; internal set; }
			public double FinishingOutQty { get; internal set; }
			public double FinishingOutPrice { get; internal set; }
			public double FinishingInTransferQty { get; internal set; }
			public double FinishingInTransferPrice { get; internal set; }
			public double FinishingAdjQty { get; internal set; }
			public double FinishingAdjPrice { get; internal set; }
			public double FinishingReturQty { get; internal set; }
			public double FinishingReturPrice { get; internal set; }
			public double EndBalanceFinishingQty { get; internal set; }
			public double EndBalanceFinishingPrice { get; internal set; }
			public double BeginingBalanceExpenditureGood { get; internal set; }
			public double BeginingBalanceExpenditureGoodPrice { get; internal set; }
			public double FinishingTransferExpenditure { get; internal set; }
			public double FinishingTransferExpenditurePrice { get; internal set; }
			public double ExpenditureGoodRetur { get; internal set; }
			public double ExpenditureGoodReturPrice { get; internal set; }
			public double ExportQty { get; internal set; }
			public double ExportPrice { get; internal set; }
			public double OtherQty { get; internal set; }
			public double OtherPrice { get; internal set; }
			public double SampleQty { get; internal set; }
			public double SamplePrice { get; internal set; }
			public double ExpenditureGoodRemainingQty { get; internal set; }
			public double ExpenditureGoodRemainingPrice { get; internal set; }
			public double ExpenditureGoodAdj { get; internal set; }
			public double ExpenditureGoodAdjPrice { get; internal set; }
			public double EndBalanceExpenditureGood { get; internal set; }
			public double EndBalanceExpenditureGoodPrice { get; internal set; }
			public double ExpenditureGoodInTransfer { get; internal set; }
			public double ExpenditureGoodInTransferPrice { get; internal set; }

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
		public async Task<MemoryStream> Handle(GetXlsMonitoringProductionStockFlowQuery request, CancellationToken cancellationToken)
		{
			DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom, new TimeSpan(7, 0, 0));
			DateTimeOffset dateTo = new DateTimeOffset(request.dateTo, new TimeSpan(7, 0, 0));
            DateTimeOffset dateBalance = (from a in garmentBalanceProductionStockRepository.Query.OrderByDescending(s => s.CreatedDate)
                                          select a.CreatedDate).FirstOrDefault();
            DateTimeOffset dateFareNew = dateTo.AddDays(1);


            var sumbasicPrice = (from a in garmentPreparingRepository.Query
                                 join b in garmentPreparingItemRepository.Query on a.Identity equals b.GarmentPreparingId
                                 where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) //&&
                                                                                                                                // a.UnitId == request.unit
                                 select new { a.RONo, b.BasicPrice })
                         .GroupBy(x => new { x.RONo }, (key, group) => new ViewBasicPrices
                         {
                             RO = key.RONo,
                             BasicPrice = Convert.ToDecimal(group.Sum(s => s.BasicPrice)),
                             Count = group.Count()
                         });
            var sumFCs = (from a in garmentCuttingInRepository.Query
                          where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.CuttingType == "Main Fabric" &&
                          a.CuttingInDate <= dateTo
                          select new { a.FC, a.RONo })
                         .GroupBy(x => new { x.RONo }, (key, group) => new ViewFC
                         {
                             RO = key.RONo,
                             FC = group.Sum(s => s.FC),
                             Count = group.Count()
                         });

            var queryGroup = (from a in (from aa in garmentCuttingOutRepository.Query
                                         where (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro))
                                         /*&& aa.UnitFromId == request.unit && aa.CuttingOutDate <= dateTo && aa.CuttingOutType == "SEWING" && aa.UnitId == aa.UnitFromId*/
                                         select aa)
                              join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId

                              select new { BasicPrice = (from aa in sumbasicPrice where aa.RO == a.RONo select aa.BasicPrice / aa.Count).FirstOrDefault(), FareNew = (from aa in garmentComodityPriceRepository.Query where aa.UnitId == request.unit && a.ComodityId == aa.ComodityId && aa.Date == dateFareNew select aa.Price).FirstOrDefault(), Fare = (from aa in garmentComodityPriceRepository.Query where aa.UnitId == request.unit && a.ComodityId == aa.ComodityId && aa.IsValid == true select aa.Price).FirstOrDefault(), Ro = a.RONo, Article = a.Article, Comodity = a.ComodityName, FC = (from cost in sumFCs where cost.RO == a.RONo select cost.FC / cost.Count).FirstOrDefault() }).Distinct();



            var queryBalance = from a in
                                      (from aa in garmentBalanceProductionStockRepository.Query
                                       where (request.ro == null || (request.ro != null && request.ro != "" && aa.Ro == request.ro)) && aa.UnitId == request.unit && aa.UnitId == aa.UnitId
                                       select aa)

                               where a.CreatedDate < dateFrom && a.UnitId == request.unit //&& a.RoJob == "2010810"
                               select new monitoringView
                               {
                                   QtyCuttingIn = 0,
                                   PriceCuttingIn = 0,
                                   QtySewingIn = 0,
                                   PriceSewingIn = 0,
                                   QtyCuttingTransfer = 0,
                                   PriceCuttingTransfer = 0,
                                   QtyCuttingsubkon = 0,
                                   PriceCuttingsubkon = 0,
                                   AvalCutting = 0,
                                   AvalCuttingPrice = 0,
                                   AvalSewing = 0,
                                   AvalSewingPrice = 0,
                                   QtyLoading = 0,
                                   PriceLoading = 0,
                                   QtyLoadingAdjs = 0,
                                   PriceLoadingAdjs = 0,
                                   QtySewingOut = 0,
                                   PriceSewingOut = 0,
                                   QtySewingAdj = 0,
                                   PriceSewingAdj = 0,
                                   WipSewingOut = 0,
                                   WipSewingOutPrice = 0,
                                   WipFinishingOut = 0,
                                   WipFinishingOutPrice = 0,
                                   QtySewingRetur = 0,
                                   PriceSewingRetur = 0,
                                   QtySewingInTransfer = 0,
                                   PriceSewingInTransfer = 0,
                                   FinishingInQty = 0,
                                   FinishingInPrice = 0,
                                   SubconInQty = 0,
                                   SubconInPrice = 0,
                                   FinishingAdjQty = 0,
                                   FinishingAdjPrice = 0,
                                   FinishingTransferExpenditure = 0,
                                   FinishingTransferExpenditurePrice = 0,
                                   FinishingInTransferQty = 0,
                                   FinishingInTransferPrice = 0,
                                   FinishingOutQty = 0,
                                   FinishingOutPrice = 0,
                                   FinishingReturQty = 0,
                                   FinishingReturPrice = 0,
                                   SubconOutQty = 0,
                                   SubconOutPrice = 0,
                                   //BeginingBalanceLoadingQty = a.CuttingOutDate < dateFrom ? -c.CuttingOutQuantity : 0,
                                   //BeginingBalanceLoadingPrice = a.CuttingOutDate < dateFrom ? -c.Price : 0,
                                   BeginingBalanceLoadingQty = a.BeginingBalanceLoadingQty,
                                   BeginingBalanceLoadingPrice = a.BeginingBalanceLoadingPrice,
                                   BeginingBalanceCuttingQty = a.BeginingBalanceCuttingQty,
                                   BeginingBalanceCuttingPrice = a.BeginingBalanceCuttingPrice,
                                   BeginingBalanceFinishingQty = a.BeginingBalanceFinishingQty,
                                   BeginingBalanceFinishingPrice = a.BeginingBalanceFinishingPrice,
                                   BeginingBalanceSewingQty = a.BeginingBalanceSewingQty,
                                   BeginingBalanceSewingPrice = a.BeginingBalanceSewingPrice,
                                   BeginingBalanceExpenditureGood = a.BeginingBalanceExpenditureGood,
                                   BeginingBalanceExpenditureGoodPrice = a.BeginingBalanceExpenditureGoodPrice,
                                   BeginingBalanceSubconQty = a.BeginingBalanceSubconQty,
                                   BeginingBalanceSubconPrice = a.BeginingBalanceSubconPrice,
                                   Ro = a.Ro,
                                   ExpenditureGoodRetur = 0,
                                   ExpenditureGoodReturPrice = 0,
                                   QtyCuttingOut = 0,
                                   PriceCuttingOut = 0,
                                   ExportQty = 0,
                                   ExportPrice = 0,
                                   SampleQty = 0,
                                   SamplePrice = 0,
                                   OtherQty = 0,
                                   OtherPrice = 0,
                                   QtyLoadingInTransfer = 0,
                                   PriceLoadingInTransfer = 0,
                                   ExpenditureGoodInTransfer = 0,
                                   ExpenditureGoodInTransferPrice = 0

                               };


            var QueryCuttingOut = (from a in (from aa in garmentCuttingOutRepository.Query
                                              where aa.CuttingOutDate >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == request.unit && aa.CuttingOutDate <= dateTo && aa.CuttingOutType == "SEWING" && aa.UnitId == aa.UnitFromId
                                              select aa)
                                   join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
                                   join c in garmentCuttingOutDetailRepository.Query on b.Identity equals c.CutOutItemId

                                   select new monitoringView
                                   {
                                       QtyCuttingIn = 0,
                                       PriceCuttingIn = 0,
                                       QtySewingIn = 0,
                                       PriceSewingIn = 0,
                                       QtyCuttingTransfer = 0,
                                       PriceCuttingTransfer = 0,
                                       QtyCuttingsubkon = 0,
                                       PriceCuttingsubkon = 0,
                                       AvalCutting = 0,
                                       AvalCuttingPrice = 0,
                                       AvalSewing = 0,
                                       AvalSewingPrice = 0,
                                       QtyLoading = 0,
                                       PriceLoading = 0,
                                       QtyLoadingAdjs = 0,
                                       PriceLoadingAdjs = 0,
                                       QtySewingOut = 0,
                                       PriceSewingOut = 0,
                                       QtySewingAdj = 0,
                                       PriceSewingAdj = 0,
                                       WipSewingOut = 0,
                                       WipSewingOutPrice = 0,
                                       WipFinishingOut = 0,
                                       WipFinishingOutPrice = 0,
                                       QtySewingRetur = 0,
                                       PriceSewingRetur = 0,
                                       QtySewingInTransfer = 0,
                                       PriceSewingInTransfer = 0,
                                       FinishingInQty = 0,
                                       FinishingInPrice = 0,
                                       SubconInQty = 0,
                                       SubconInPrice = 0,
                                       FinishingAdjQty = 0,
                                       FinishingAdjPrice = 0,
                                       FinishingTransferExpenditure = 0,
                                       FinishingTransferExpenditurePrice = 0,
                                       FinishingInTransferQty = 0,
                                       FinishingInTransferPrice = 0,
                                       FinishingOutQty = 0,
                                       FinishingOutPrice = 0,
                                       FinishingReturQty = 0,
                                       FinishingReturPrice = 0,
                                       SubconOutQty = 0,
                                       SubconOutPrice = 0,
                                       //BeginingBalanceLoadingQty = a.CuttingOutDate < dateFrom ? -c.CuttingOutQuantity : 0,
                                       //BeginingBalanceLoadingPrice = a.CuttingOutDate < dateFrom ? -c.Price : 0,
                                       BeginingBalanceLoadingQty = 0,
                                       BeginingBalanceLoadingPrice = 0,
                                       BeginingBalanceCuttingQty = a.CuttingOutDate < dateFrom && a.CuttingOutDate > dateBalance ? -c.CuttingOutQuantity : 0,
                                       BeginingBalanceCuttingPrice = a.CuttingOutDate < dateFrom && a.CuttingOutDate > dateBalance ? -c.Price : 0,
                                       Ro = a.RONo,
                                       ExpenditureGoodRetur = 0,
                                       ExpenditureGoodReturPrice = 0,
                                       QtyCuttingOut = a.CuttingOutDate >= dateFrom ? c.CuttingOutQuantity : 0,
                                       PriceCuttingOut = a.CuttingOutDate >= dateFrom ? c.Price : 0,
                                       ExportQty = 0,
                                       ExportPrice = 0,
                                       SampleQty = 0,
                                       SamplePrice = 0,
                                       OtherQty = 0,
                                       OtherPrice = 0,
                                       QtyLoadingInTransfer = 0,
                                       PriceLoadingInTransfer = 0,
                                       ExpenditureGoodInTransfer = 0,
                                       ExpenditureGoodInTransferPrice = 0,
                                       BeginingBalanceFinishingQty = 0,
                                       BeginingBalanceFinishingPrice = 0
                                   });
            var QueryCuttingOutSubkon = (from a in (from aa in garmentCuttingOutRepository.Query
                                                    where aa.CuttingOutDate >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitFromId == request.unit && aa.CuttingOutDate <= dateTo && aa.CuttingOutType == "SUBKON"
                                                    select aa)
                                         join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
                                         join c in garmentCuttingOutDetailRepository.Query on b.Identity equals c.CutOutItemId
                                         select new monitoringView
                                         {
                                             QtyCuttingIn = 0,
                                             PriceCuttingIn = 0,
                                             QtySewingIn = 0,
                                             PriceSewingIn = 0,
                                             QtyCuttingOut = 0,
                                             PriceCuttingOut = 0,
                                             QtyCuttingTransfer = 0,
                                             PriceCuttingTransfer = 0,
                                             AvalCutting = 0,
                                             AvalCuttingPrice = 0,
                                             AvalSewing = 0,
                                             AvalSewingPrice = 0,
                                             QtyLoading = 0,
                                             PriceLoading = 0,
                                             QtyLoadingAdjs = 0,
                                             PriceLoadingAdjs = 0,
                                             QtySewingOut = 0,
                                             PriceSewingOut = 0,
                                             QtySewingAdj = 0,
                                             PriceSewingAdj = 0,
                                             WipSewingOut = 0,
                                             WipSewingOutPrice = 0,
                                             WipFinishingOut = 0,
                                             WipFinishingOutPrice = 0,
                                             QtySewingRetur = 0,
                                             PriceSewingRetur = 0,
                                             QtySewingInTransfer = 0,
                                             PriceSewingInTransfer = 0,
                                             FinishingInQty = 0,
                                             FinishingInPrice = 0,
                                             SubconInQty = 0,
                                             SubconInPrice = 0,
                                             FinishingAdjQty = 0,
                                             FinishingAdjPrice = 0,
                                             FinishingTransferExpenditure = 0,
                                             FinishingTransferExpenditurePrice = 0,
                                             FinishingInTransferQty = 0,
                                             FinishingInTransferPrice = 0,
                                             FinishingOutQty = 0,
                                             FinishingOutPrice = 0,
                                             FinishingReturQty = 0,
                                             FinishingReturPrice = 0,
                                             SubconOutQty = 0,
                                             SubconOutPrice = 0,
                                             //BeginingBalanceLoadingQty = a.CuttingOutDate < dateFrom ? c.CuttingOutQuantity : 0,
                                             //BeginingBalanceLoadingPrice = a.CuttingOutDate < dateFrom ? c.Price : 0,
                                             BeginingBalanceCuttingQty = a.CuttingOutDate < dateFrom && a.CuttingOutDate > dateBalance ? -c.CuttingOutQuantity : 0,
                                             Ro = a.RONo,
                                             BeginingBalanceCuttingPrice = a.CuttingOutDate < dateFrom && a.CuttingOutDate > dateBalance ? -c.Price : 0,
                                             // FC = (from cost in FC where cost.ro == a.RONo select cost.fc).FirstOrDefault(),
                                             QtyCuttingsubkon = a.CuttingOutDate >= dateFrom ? c.CuttingOutQuantity : 0,
                                             PriceCuttingsubkon = a.CuttingOutDate >= dateFrom ? c.Price : 0,
                                             ExpenditureGoodRetur = 0,
                                             ExpenditureGoodReturPrice = 0,
                                             ExportQty = 0,
                                             ExportPrice = 0,
                                             SampleQty = 0,
                                             SamplePrice = 0,
                                             OtherQty = 0,
                                             OtherPrice = 0,
                                             QtyLoadingInTransfer = 0,
                                             PriceLoadingInTransfer = 0,
                                             ExpenditureGoodInTransfer = 0,
                                             ExpenditureGoodInTransferPrice = 0,
                                             BeginingBalanceLoadingQty = 0,
                                             BeginingBalanceLoadingPrice = 0,
                                             BeginingBalanceFinishingQty = 0,
                                             BeginingBalanceFinishingPrice = 0
                                         });
            var QueryCuttingOutTransfer = (from a in (from aa in garmentCuttingOutRepository.Query
                                                      where aa.CuttingOutDate >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitFromId == request.unit && aa.CuttingOutDate <= dateTo && aa.CuttingOutType == "SEWING" && aa.UnitId != aa.UnitFromId
                                                      select aa)
                                           join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
                                           join c in garmentCuttingOutDetailRepository.Query on b.Identity equals c.CutOutItemId
                                           select new monitoringView
                                           {
                                               QtyCuttingIn = 0,
                                               PriceCuttingIn = 0,
                                               QtySewingIn = 0,
                                               PriceSewingIn = 0,
                                               QtyCuttingOut = 0,
                                               PriceCuttingOut = 0,
                                               QtyCuttingsubkon = 0,
                                               PriceCuttingsubkon = 0,
                                               AvalCutting = 0,
                                               AvalCuttingPrice = 0,
                                               AvalSewing = 0,
                                               AvalSewingPrice = 0,
                                               QtyLoading = 0,
                                               PriceLoading = 0,
                                               QtyLoadingAdjs = 0,
                                               PriceLoadingAdjs = 0,
                                               QtySewingOut = 0,
                                               PriceSewingOut = 0,
                                               QtySewingAdj = 0,
                                               PriceSewingAdj = 0,
                                               WipSewingOut = 0,
                                               WipSewingOutPrice = 0,
                                               WipFinishingOut = 0,
                                               WipFinishingOutPrice = 0,
                                               QtySewingRetur = 0,
                                               PriceSewingRetur = 0,
                                               QtySewingInTransfer = 0,
                                               PriceSewingInTransfer = 0,
                                               FinishingInQty = 0,
                                               FinishingInPrice = 0,
                                               SubconInQty = 0,
                                               SubconInPrice = 0,
                                               FinishingAdjQty = 0,
                                               FinishingAdjPrice = 0,
                                               FinishingTransferExpenditure = 0,
                                               FinishingTransferExpenditurePrice = 0,
                                               FinishingInTransferQty = 0,
                                               FinishingInTransferPrice = 0,
                                               FinishingOutQty = 0,
                                               FinishingOutPrice = 0,
                                               FinishingReturQty = 0,
                                               FinishingReturPrice = 0,
                                               SubconOutQty = 0,
                                               SubconOutPrice = 0,
                                               //BeginingBalanceLoadingQty = a.CuttingOutDate < dateFrom ? -c.CuttingOutQuantity : 0,
                                               //BeginingBalanceLoadingPrice = a.CuttingOutDate < dateFrom ? -c.Price : 0,
                                               BeginingBalanceLoadingQty = 0,
                                               BeginingBalanceLoadingPrice = 0,
                                               BeginingBalanceCuttingQty = a.CuttingOutDate < dateFrom && a.CuttingOutDate > dateBalance ? -c.CuttingOutQuantity : 0,
                                               BeginingBalanceCuttingPrice = a.CuttingOutDate < dateFrom && a.CuttingOutDate > dateBalance ? -c.Price : 0,
                                               Ro = a.RONo,
                                               QtyCuttingTransfer = a.CuttingOutDate >= dateFrom ? c.CuttingOutQuantity : 0,
                                               PriceCuttingTransfer = a.CuttingOutDate >= dateFrom ? c.Price : 0,
                                               ExpenditureGoodRetur = 0,
                                               ExpenditureGoodReturPrice = 0,
                                               ExportQty = 0,
                                               ExportPrice = 0,
                                               SampleQty = 0,
                                               SamplePrice = 0,
                                               OtherQty = 0,
                                               OtherPrice = 0,
                                               QtyLoadingInTransfer = 0,
                                               PriceLoadingInTransfer = 0,
                                               ExpenditureGoodInTransfer = 0,
                                               ExpenditureGoodInTransferPrice = 0,
                                               BeginingBalanceFinishingQty = 0,
                                               BeginingBalanceFinishingPrice = 0
                                           });
            var QueryCuttingIn = (from a in (from aa in garmentCuttingInRepository.Query
                                             where aa.CuttingInDate >= dateBalance && aa.CuttingType != "Non Main Fabric" && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == request.unit && aa.CuttingInDate <= dateTo
                                             select aa)
                                  join b in garmentCuttingInItemRepository.Query on a.Identity equals b.CutInId
                                  join c in garmentCuttingInDetailRepository.Query on b.Identity equals c.CutInItemId
                                  select new monitoringView
                                  {
                                      QtySewingIn = 0,
                                      PriceSewingIn = 0,
                                      QtyCuttingOut = 0,
                                      PriceCuttingOut = 0,
                                      QtyCuttingTransfer = 0,
                                      PriceCuttingTransfer = 0,
                                      QtyCuttingsubkon = 0,
                                      PriceCuttingsubkon = 0,
                                      AvalCutting = 0,
                                      AvalCuttingPrice = 0,
                                      AvalSewing = 0,
                                      AvalSewingPrice = 0,
                                      QtyLoading = 0,
                                      PriceLoading = 0,
                                      QtyLoadingAdjs = 0,
                                      PriceLoadingAdjs = 0,
                                      QtySewingOut = 0,
                                      PriceSewingOut = 0,
                                      QtySewingAdj = 0,
                                      PriceSewingAdj = 0,
                                      WipSewingOut = 0,
                                      WipSewingOutPrice = 0,
                                      WipFinishingOut = 0,
                                      WipFinishingOutPrice = 0,
                                      QtySewingRetur = 0,
                                      PriceSewingRetur = 0,
                                      QtySewingInTransfer = 0,
                                      PriceSewingInTransfer = 0,
                                      FinishingInQty = 0,
                                      FinishingInPrice = 0,
                                      SubconInQty = 0,
                                      SubconInPrice = 0,
                                      FinishingAdjQty = 0,
                                      FinishingAdjPrice = 0,
                                      FinishingTransferExpenditure = 0,
                                      FinishingTransferExpenditurePrice = 0,
                                      FinishingInTransferQty = 0,
                                      FinishingInTransferPrice = 0,
                                      FinishingOutQty = 0,
                                      FinishingOutPrice = 0,
                                      FinishingReturQty = 0,
                                      FinishingReturPrice = 0,
                                      SubconOutQty = 0,
                                      SubconOutPrice = 0,
                                      BeginingBalanceCuttingQty = a.CuttingInDate < dateFrom && a.CuttingInDate > dateBalance ? c.CuttingInQuantity : 0,
                                      BeginingBalanceCuttingPrice = a.CuttingInDate < dateFrom && a.CuttingInDate > dateBalance ? c.Price : 0,
                                      Ro = a.RONo,
                                      QtyCuttingIn = a.CuttingInDate >= dateFrom ? c.CuttingInQuantity : 0,
                                      PriceCuttingIn = a.CuttingInDate >= dateFrom ? c.Price : 0,
                                      ExpenditureGoodRetur = 0,
                                      ExpenditureGoodReturPrice = 0,
                                      ExportQty = 0,
                                      ExportPrice = 0,
                                      SampleQty = 0,
                                      SamplePrice = 0,
                                      OtherQty = 0,
                                      OtherPrice = 0,
                                      QtyLoadingInTransfer = 0,
                                      PriceLoadingInTransfer = 0,
                                      ExpenditureGoodInTransfer = 0,
                                      ExpenditureGoodInTransferPrice = 0,
                                      BeginingBalanceLoadingQty = 0,
                                      BeginingBalanceLoadingPrice = 0,
                                      BeginingBalanceFinishingQty = 0,
                                      BeginingBalanceFinishingPrice = 0
                                  });

            var QueryAvalCompSewing = from a in (from aa in garmentAvalComponentRepository.Query
                                                 where aa.Date >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == request.unit && aa.Date <= dateTo && aa.AvalComponentType == "SEWING"
                                                 select aa)
                                      join b in garmentAvalComponentItemRepository.Query on a.Identity equals b.AvalComponentId
                                      select new monitoringView
                                      {
                                          QtySewingIn = 0,
                                          PriceSewingIn = 0,
                                          QtyCuttingOut = 0,
                                          PriceCuttingOut = 0,
                                          QtyCuttingTransfer = 0,
                                          PriceCuttingTransfer = 0,
                                          QtyCuttingsubkon = 0,
                                          PriceCuttingsubkon = 0,
                                          AvalCutting = 0,
                                          AvalCuttingPrice = 0,
                                          QtyLoading = 0,
                                          PriceLoading = 0,
                                          QtyLoadingAdjs = 0,
                                          PriceLoadingAdjs = 0,
                                          QtySewingOut = 0,
                                          PriceSewingOut = 0,
                                          QtySewingAdj = 0,
                                          PriceSewingAdj = 0,
                                          WipSewingOut = 0,
                                          WipSewingOutPrice = 0,
                                          WipFinishingOut = 0,
                                          WipFinishingOutPrice = 0,
                                          QtySewingRetur = 0,
                                          PriceSewingRetur = 0,
                                          QtySewingInTransfer = 0,
                                          PriceSewingInTransfer = 0,
                                          FinishingInQty = 0,
                                          FinishingInPrice = 0,
                                          SubconInQty = 0,
                                          SubconInPrice = 0,
                                          FinishingAdjQty = 0,
                                          FinishingAdjPrice = 0,
                                          FinishingTransferExpenditure = 0,
                                          FinishingTransferExpenditurePrice = 0,
                                          FinishingInTransferQty = 0,
                                          FinishingInTransferPrice = 0,
                                          FinishingOutQty = 0,
                                          FinishingOutPrice = 0,
                                          FinishingReturQty = 0,
                                          FinishingReturPrice = 0,
                                          SubconOutQty = 0,
                                          SubconOutPrice = 0,
                                          BeginingBalanceCuttingQty = 0,//a.Date < dateFrom && a.Date > dateBalance ? -b.Quantity : 0,
                                          BeginingBalanceCuttingPrice = 0,//a.Date < dateFrom && a.Date > dateBalance  ? -Convert.ToDouble(b.Price) : 0,
                                          Ro = a.RONo,
                                          QtyCuttingIn = 0,
                                          PriceCuttingIn = 0,
                                          AvalSewing = a.Date >= dateFrom ? b.Quantity : 0,
                                          AvalSewingPrice = a.Date >= dateFrom ? Convert.ToDouble(b.Price) : 0,
                                          ExpenditureGoodRetur = 0,
                                          ExpenditureGoodReturPrice = 0,
                                          ExportQty = 0,
                                          ExportPrice = 0,
                                          SampleQty = 0,
                                          SamplePrice = 0,
                                          OtherQty = 0,
                                          OtherPrice = 0,
                                          QtyLoadingInTransfer = 0,
                                          PriceLoadingInTransfer = 0,
                                          ExpenditureGoodInTransfer = 0,
                                          ExpenditureGoodInTransferPrice = 0,
                                          BeginingBalanceLoadingQty = 0,
                                          BeginingBalanceLoadingPrice = 0,
                                          BeginingBalanceFinishingQty = 0,
                                          BeginingBalanceFinishingPrice = 0
                                      };
            var QueryAvalCompCutting = from a in (from aa in garmentAvalComponentRepository.Query
                                                  where aa.Date >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == request.unit && aa.Date <= dateTo && aa.AvalComponentType == "CUTTING"
                                                  select aa)
                                       join b in garmentAvalComponentItemRepository.Query on a.Identity equals b.AvalComponentId
                                       select new monitoringView
                                       {
                                           QtyCuttingIn = 0,
                                           PriceCuttingIn = 0,
                                           QtySewingIn = 0,
                                           PriceSewingIn = 0,
                                           QtyCuttingOut = 0,
                                           PriceCuttingOut = 0,
                                           QtyCuttingTransfer = 0,
                                           PriceCuttingTransfer = 0,
                                           QtyCuttingsubkon = 0,
                                           PriceCuttingsubkon = 0,
                                           AvalSewing = 0,
                                           AvalSewingPrice = 0,
                                           QtyLoading = 0,
                                           PriceLoading = 0,
                                           QtyLoadingAdjs = 0,
                                           PriceLoadingAdjs = 0,
                                           QtySewingOut = 0,
                                           PriceSewingOut = 0,
                                           QtySewingAdj = 0,
                                           PriceSewingAdj = 0,
                                           WipSewingOut = 0,
                                           WipSewingOutPrice = 0,
                                           WipFinishingOut = 0,
                                           WipFinishingOutPrice = 0,
                                           QtySewingRetur = 0,
                                           PriceSewingRetur = 0,
                                           QtySewingInTransfer = 0,
                                           PriceSewingInTransfer = 0,
                                           FinishingInQty = 0,
                                           FinishingInPrice = 0,
                                           SubconInQty = 0,
                                           SubconInPrice = 0,
                                           FinishingAdjQty = 0,
                                           FinishingAdjPrice = 0,
                                           FinishingTransferExpenditure = 0,
                                           FinishingTransferExpenditurePrice = 0,
                                           FinishingInTransferQty = 0,
                                           FinishingInTransferPrice = 0,
                                           FinishingOutQty = 0,
                                           FinishingOutPrice = 0,
                                           FinishingReturQty = 0,
                                           FinishingReturPrice = 0,
                                           SubconOutQty = 0,
                                           SubconOutPrice = 0,
                                           BeginingBalanceCuttingQty = 0,//a.Date < dateFrom && a.Date > dateBalance ? -b.Quantity : 0,
                                           BeginingBalanceCuttingPrice = 0,// a.Date < dateFrom && a.Date > dateBalance ? -Convert.ToDouble(b.Price) : 0,
                                           Ro = a.RONo,
                                           AvalCutting = a.Date >= dateFrom ? b.Quantity : 0,
                                           AvalCuttingPrice = a.Date >= dateFrom ? Convert.ToDouble(b.Price) : 0,
                                           ExpenditureGoodRetur = 0,
                                           ExpenditureGoodReturPrice = 0,
                                           ExportQty = 0,
                                           ExportPrice = 0,
                                           SampleQty = 0,
                                           SamplePrice = 0,
                                           OtherQty = 0,
                                           OtherPrice = 0,
                                           QtyLoadingInTransfer = 0,
                                           PriceLoadingInTransfer = 0,
                                           ExpenditureGoodInTransfer = 0,
                                           ExpenditureGoodInTransferPrice = 0,
                                           BeginingBalanceLoadingQty = 0,
                                           BeginingBalanceLoadingPrice = 0,
                                           BeginingBalanceFinishingQty = 0,
                                           BeginingBalanceFinishingPrice = 0
                                       };
            var QuerySewingDO = (from a in (from aa in garmentSewingDORepository.Query
                                            where aa.SewingDODate >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == request.unit && aa.UnitFromId == aa.UnitId && aa.SewingDODate <= dateTo
                                            select aa)
                                 join b in garmentSewingDOItemRepository.Query on a.Identity equals b.SewingDOId
                                 select new monitoringView
                                 {
                                     QtyCuttingIn = 0,
                                     PriceCuttingIn = 0,
                                     QtySewingIn = 0,
                                     PriceSewingIn = 0,
                                     QtyCuttingOut = 0,
                                     PriceCuttingOut = 0,
                                     QtyCuttingTransfer = 0,
                                     PriceCuttingTransfer = 0,
                                     QtyCuttingsubkon = 0,
                                     PriceCuttingsubkon = 0,
                                     AvalCutting = 0,
                                     AvalCuttingPrice = 0,
                                     AvalSewing = 0,
                                     AvalSewingPrice = 0,
                                     QtyLoading = 0,
                                     PriceLoading = 0,
                                     QtyLoadingAdjs = 0,
                                     PriceLoadingAdjs = 0,
                                     QtySewingOut = 0,
                                     PriceSewingOut = 0,
                                     QtySewingAdj = 0,
                                     PriceSewingAdj = 0,
                                     WipSewingOut = 0,
                                     WipSewingOutPrice = 0,
                                     WipFinishingOut = 0,
                                     WipFinishingOutPrice = 0,
                                     QtySewingRetur = 0,
                                     PriceSewingRetur = 0,
                                     QtySewingInTransfer = 0,
                                     PriceSewingInTransfer = 0,
                                     FinishingInQty = 0,
                                     FinishingInPrice = 0,
                                     SubconInQty = 0,
                                     SubconInPrice = 0,
                                     FinishingAdjQty = 0,
                                     FinishingAdjPrice = 0,
                                     FinishingTransferExpenditure = 0,
                                     FinishingTransferExpenditurePrice = 0,
                                     FinishingInTransferQty = 0,
                                     FinishingInTransferPrice = 0,
                                     FinishingOutQty = 0,
                                     FinishingOutPrice = 0,
                                     FinishingReturQty = 0,
                                     FinishingReturPrice = 0,
                                     SubconOutQty = 0,
                                     SubconOutPrice = 0,
                                     QtyLoadingIn = a.SewingDODate >= dateFrom ? b.Quantity : 0,
                                     PriceLoadingIn = a.SewingDODate >= dateFrom ? b.Price : 0,
                                     BeginingBalanceLoadingQty = (a.SewingDODate < dateFrom && a.SewingDODate > dateBalance) ? b.Quantity : 0,
                                     BeginingBalanceLoadingPrice = (a.SewingDODate < dateFrom && a.SewingDODate > dateBalance) ? b.Price : 0,
                                     Ro = a.RONo,
                                     ExpenditureGoodRetur = 0,
                                     ExpenditureGoodReturPrice = 0,
                                     ExportQty = 0,
                                     ExportPrice = 0,
                                     SampleQty = 0,
                                     SamplePrice = 0,
                                     OtherQty = 0,
                                     OtherPrice = 0,
                                     QtyLoadingInTransfer = 0,
                                     PriceLoadingInTransfer = 0,
                                     ExpenditureGoodInTransfer = 0,
                                     ExpenditureGoodInTransferPrice = 0,
                                     BeginingBalanceCuttingQty = 0,
                                     BeginingBalanceCuttingPrice = 0,
                                     BeginingBalanceFinishingQty = 0,
                                     BeginingBalanceFinishingPrice = 0
                                 });

            var QueryLoadingInTransfer = (from a in (from aa in garmentSewingDORepository.Query
                                                     where aa.SewingDODate >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == request.unit && aa.UnitFromId != aa.UnitId && aa.SewingDODate <= dateTo
                                                     select aa)
                                          join b in garmentSewingDOItemRepository.Query on a.Identity equals b.SewingDOId
                                          select new monitoringView
                                          {
                                              QtyCuttingIn = 0,
                                              PriceCuttingIn = 0,
                                              QtySewingIn = 0,
                                              PriceSewingIn = 0,
                                              QtyCuttingOut = 0,
                                              PriceCuttingOut = 0,
                                              QtyCuttingTransfer = 0,
                                              PriceCuttingTransfer = 0,
                                              QtyCuttingsubkon = 0,
                                              PriceCuttingsubkon = 0,
                                              AvalCutting = 0,
                                              AvalCuttingPrice = 0,
                                              AvalSewing = 0,
                                              AvalSewingPrice = 0,
                                              QtyLoading = 0,
                                              PriceLoading = 0,
                                              QtyLoadingAdjs = 0,
                                              PriceLoadingAdjs = 0,
                                              QtySewingOut = 0,
                                              PriceSewingOut = 0,
                                              QtySewingAdj = 0,
                                              PriceSewingAdj = 0,
                                              WipSewingOut = 0,
                                              WipSewingOutPrice = 0,
                                              WipFinishingOut = 0,
                                              WipFinishingOutPrice = 0,
                                              QtySewingRetur = 0,
                                              PriceSewingRetur = 0,
                                              QtySewingInTransfer = 0,
                                              PriceSewingInTransfer = 0,
                                              FinishingInQty = 0,
                                              FinishingInPrice = 0,
                                              SubconInQty = 0,
                                              SubconInPrice = 0,
                                              FinishingAdjQty = 0,
                                              FinishingAdjPrice = 0,
                                              FinishingTransferExpenditure = 0,
                                              FinishingTransferExpenditurePrice = 0,
                                              FinishingInTransferQty = 0,
                                              FinishingInTransferPrice = 0,
                                              FinishingOutQty = 0,
                                              FinishingOutPrice = 0,
                                              FinishingReturQty = 0,
                                              FinishingReturPrice = 0,
                                              SubconOutQty = 0,
                                              SubconOutPrice = 0,
                                              QtyLoadingInTransfer = a.SewingDODate >= dateFrom ? b.Quantity : 0,
                                              PriceLoadingInTransfer = a.SewingDODate >= dateFrom ? b.Price : 0,
                                              BeginingBalanceLoadingQty = (a.SewingDODate < dateFrom && a.SewingDODate > dateBalance) ? b.Quantity : 0,
                                              BeginingBalanceLoadingPrice = (a.SewingDODate < dateFrom && a.SewingDODate > dateBalance) ? b.Price : 0,
                                              Ro = a.RONo,
                                              ExpenditureGoodRetur = 0,
                                              ExpenditureGoodReturPrice = 0,
                                              ExportQty = 0,
                                              ExportPrice = 0,
                                              SampleQty = 0,
                                              SamplePrice = 0,
                                              OtherQty = 0,
                                              OtherPrice = 0,
                                              ExpenditureGoodInTransfer = 0,
                                              ExpenditureGoodInTransferPrice = 0,
                                              BeginingBalanceCuttingQty = 0,
                                              BeginingBalanceCuttingPrice = 0,
                                              BeginingBalanceFinishingQty = 0,
                                              BeginingBalanceFinishingPrice = 0
                                          });
            var QueryLoading = from a in (from aa in garmentLoadingRepository.Query
                                          where aa.LoadingDate >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == request.unit && aa.LoadingDate <= dateTo
                                          select aa)
                               join b in garmentLoadingItemRepository.Query on a.Identity equals b.LoadingId
                               select new monitoringView
                               {
                                   QtyCuttingIn = 0,
                                   PriceCuttingIn = 0,
                                   QtySewingIn = 0,
                                   PriceSewingIn = 0,
                                   QtyCuttingOut = 0,
                                   PriceCuttingOut = 0,
                                   QtyCuttingTransfer = 0,
                                   PriceCuttingTransfer = 0,
                                   QtyCuttingsubkon = 0,
                                   PriceCuttingsubkon = 0,
                                   AvalCutting = 0,
                                   AvalCuttingPrice = 0,
                                   AvalSewing = 0,
                                   AvalSewingPrice = 0,
                                   QtyLoadingAdjs = 0,
                                   PriceLoadingAdjs = 0,
                                   QtySewingOut = 0,
                                   PriceSewingOut = 0,
                                   QtySewingAdj = 0,
                                   PriceSewingAdj = 0,
                                   WipSewingOut = 0,
                                   WipSewingOutPrice = 0,
                                   WipFinishingOut = 0,
                                   WipFinishingOutPrice = 0,
                                   QtySewingRetur = 0,
                                   PriceSewingRetur = 0,
                                   QtySewingInTransfer = 0,
                                   PriceSewingInTransfer = 0,
                                   FinishingInQty = 0,
                                   FinishingInPrice = 0,
                                   SubconInQty = 0,
                                   SubconInPrice = 0,
                                   FinishingAdjQty = 0,
                                   FinishingAdjPrice = 0,
                                   FinishingTransferExpenditure = 0,
                                   FinishingTransferExpenditurePrice = 0,
                                   FinishingInTransferQty = 0,
                                   FinishingInTransferPrice = 0,
                                   FinishingOutQty = 0,
                                   FinishingOutPrice = 0,
                                   FinishingReturQty = 0,
                                   FinishingReturPrice = 0,
                                   SubconOutQty = 0,
                                   SubconOutPrice = 0,
                                   QtyLoadingInTransfer = 0,
                                   PriceLoadingInTransfer = 0,
                                   // BeginingBalanceSewingQty = a.LoadingDate < dateFrom ? b.Quantity : 0,
                                   //BeginingBalanceSewingPrice = a.LoadingDate < dateFrom ? b.Price : 0,
                                   BeginingBalanceSewingQty = 0,
                                   BeginingBalanceSewingPrice = 0,
                                   BeginingBalanceLoadingQty = a.LoadingDate < dateFrom && a.UnitId == a.UnitFromId && a.LoadingDate > dateBalance ? -b.Quantity : 0,
                                   BeginingBalanceLoadingPrice = a.LoadingDate < dateFrom && a.UnitId == a.UnitFromId && a.LoadingDate > dateBalance ? -b.Price : 0,
                                   Ro = a.RONo,
                                   QtyLoading = a.LoadingDate >= dateFrom && a.UnitId == a.UnitFromId ? b.Quantity : 0,
                                   PriceLoading = a.LoadingDate >= dateFrom && a.UnitId == a.UnitFromId ? b.Price : 0,
                                   ExpenditureGoodRetur = 0,
                                   ExpenditureGoodReturPrice = 0,
                                   ExportQty = 0,
                                   ExportPrice = 0,
                                   SampleQty = 0,
                                   SamplePrice = 0,
                                   OtherQty = 0,
                                   OtherPrice = 0,
                                   ExpenditureGoodInTransfer = 0,
                                   ExpenditureGoodInTransferPrice = 0,
                                   BeginingBalanceCuttingQty = 0,
                                   BeginingBalanceCuttingPrice = 0,
                                   BeginingBalanceFinishingQty = 0,
                                   BeginingBalanceFinishingPrice = 0
                               };
            var QueryLoadingAdj = from a in (from aa in garmentAdjustmentRepository.Query
                                             where aa.AdjustmentDate >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == request.unit && aa.AdjustmentDate <= dateTo && aa.AdjustmentType == "LOADING"
                                             select aa)
                                  join b in garmentAdjustmentItemRepository.Query on a.Identity equals b.AdjustmentId
                                  select new monitoringView
                                  {
                                      QtyCuttingIn = 0,
                                      PriceCuttingIn = 0,
                                      QtySewingIn = 0,
                                      PriceSewingIn = 0,
                                      QtyCuttingOut = 0,
                                      PriceCuttingOut = 0,
                                      QtyCuttingTransfer = 0,
                                      PriceCuttingTransfer = 0,
                                      QtyCuttingsubkon = 0,
                                      PriceCuttingsubkon = 0,
                                      AvalCutting = 0,
                                      AvalCuttingPrice = 0,
                                      AvalSewing = 0,
                                      AvalSewingPrice = 0,
                                      QtyLoading = 0,
                                      PriceLoading = 0,
                                      QtySewingOut = 0,
                                      PriceSewingOut = 0,
                                      QtySewingAdj = 0,
                                      PriceSewingAdj = 0,
                                      WipSewingOut = 0,
                                      WipSewingOutPrice = 0,
                                      WipFinishingOut = 0,
                                      WipFinishingOutPrice = 0,
                                      QtySewingRetur = 0,
                                      PriceSewingRetur = 0,
                                      QtySewingInTransfer = 0,
                                      PriceSewingInTransfer = 0,
                                      FinishingInQty = 0,
                                      FinishingInPrice = 0,
                                      SubconInQty = 0,
                                      SubconInPrice = 0,
                                      FinishingAdjQty = 0,
                                      FinishingAdjPrice = 0,
                                      FinishingTransferExpenditure = 0,
                                      FinishingTransferExpenditurePrice = 0,
                                      FinishingInTransferQty = 0,
                                      FinishingInTransferPrice = 0,
                                      FinishingOutQty = 0,
                                      FinishingOutPrice = 0,
                                      FinishingReturQty = 0,
                                      FinishingReturPrice = 0,
                                      SubconOutQty = 0,
                                      SubconOutPrice = 0,
                                      BeginingBalanceLoadingQty = a.AdjustmentDate < dateFrom && a.AdjustmentDate > dateBalance ? -b.Quantity : 0,
                                      BeginingBalanceLoadingPrice = a.AdjustmentDate < dateFrom && a.AdjustmentDate > dateBalance ? -b.Price : 0,
                                      Ro = a.RONo,
                                      QtyLoadingAdjs = a.AdjustmentDate >= dateFrom ? b.Quantity : 0,
                                      PriceLoadingAdjs = a.AdjustmentDate >= dateFrom ? b.Price : 0,
                                      ExpenditureGoodRetur = 0,
                                      ExpenditureGoodReturPrice = 0,
                                      ExportQty = 0,
                                      ExportPrice = 0,
                                      SampleQty = 0,
                                      SamplePrice = 0,
                                      OtherQty = 0,
                                      OtherPrice = 0,
                                      QtyLoadingInTransfer = 0,
                                      PriceLoadingInTransfer = 0,
                                      ExpenditureGoodInTransfer = 0,
                                      ExpenditureGoodInTransferPrice = 0,
                                      BeginingBalanceCuttingQty = 0,
                                      BeginingBalanceCuttingPrice = 0,
                                      BeginingBalanceFinishingQty = 0,
                                      BeginingBalanceFinishingPrice = 0
                                  };
            var QuerySewingIn = (from a in (from aa in garmentSewingInRepository.Query
                                            where aa.SewingInDate >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == request.unit && aa.SewingInDate <= dateTo
                                            select aa)
                                 join b in garmentSewingInItemRepository.Query on a.Identity equals b.SewingInId
                                 select new monitoringView
                                 {
                                     QtyCuttingIn = 0,
                                     PriceCuttingIn = 0,
                                     QtyCuttingOut = 0,
                                     PriceCuttingOut = 0,
                                     QtyCuttingTransfer = 0,
                                     PriceCuttingTransfer = 0,
                                     QtyCuttingsubkon = 0,
                                     PriceCuttingsubkon = 0,
                                     AvalCutting = 0,
                                     AvalCuttingPrice = 0,
                                     AvalSewing = 0,
                                     AvalSewingPrice = 0,
                                     QtyLoading = 0,
                                     PriceLoading = 0,
                                     QtyLoadingAdjs = 0,
                                     PriceLoadingAdjs = 0,
                                     QtySewingOut = 0,
                                     PriceSewingOut = 0,
                                     QtySewingAdj = 0,
                                     PriceSewingAdj = 0,
                                     WipSewingOut = 0,
                                     WipSewingOutPrice = 0,
                                     WipFinishingOut = 0,
                                     WipFinishingOutPrice = 0,
                                     QtySewingRetur = 0,
                                     PriceSewingRetur = 0,
                                     QtySewingInTransfer = 0,
                                     PriceSewingInTransfer = 0,
                                     FinishingInQty = 0,
                                     FinishingInPrice = 0,
                                     SubconInQty = 0,
                                     SubconInPrice = 0,
                                     FinishingAdjQty = 0,
                                     FinishingAdjPrice = 0,
                                     FinishingTransferExpenditure = 0,
                                     FinishingTransferExpenditurePrice = 0,
                                     FinishingInTransferQty = 0,
                                     FinishingInTransferPrice = 0,
                                     FinishingOutQty = 0,
                                     FinishingOutPrice = 0,
                                     FinishingReturQty = 0,
                                     FinishingReturPrice = 0,
                                     SubconOutQty = 0,
                                     SubconOutPrice = 0,
                                     BeginingBalanceSewingQty = (a.SewingInDate < dateFrom && a.SewingInDate > dateBalance && a.SewingFrom != "SEWING" /*&& a.SewingFrom == "FINISHING"*/) ? b.Quantity : 0,
                                     BeginingBalanceSewingPrice = (a.SewingInDate < dateFrom && a.SewingInDate > dateBalance && a.SewingFrom != "SEWING" /*&& a.SewingFrom == "FINISHING"*/) ? b.Price : 0,
                                     QtySewingIn = (a.SewingInDate >= dateFrom) && a.SewingFrom != "SEWING" ? b.Quantity : 0,
                                     PriceSewingIn = (a.SewingInDate >= dateFrom) && a.SewingFrom != "SEWING" ? b.Price : 0,
                                     Ro = a.RONo,
                                     ExpenditureGoodRetur = 0,
                                     ExpenditureGoodReturPrice = 0,
                                     ExportQty = 0,
                                     ExportPrice = 0,
                                     SampleQty = 0,
                                     SamplePrice = 0,
                                     OtherQty = 0,
                                     OtherPrice = 0,
                                     QtyLoadingInTransfer = 0,
                                     PriceLoadingInTransfer = 0,
                                     ExpenditureGoodInTransfer = 0,
                                     ExpenditureGoodInTransferPrice = 0,
                                     BeginingBalanceCuttingQty = 0,
                                     BeginingBalanceCuttingPrice = 0,
                                     BeginingBalanceLoadingQty = 0,
                                     BeginingBalanceLoadingPrice = 0,
                                     BeginingBalanceFinishingQty = 0,
                                     BeginingBalanceFinishingPrice = 0

                                 });
            var QuerySewingOut = (from a in (from aa in garmentSewingOutRepository.Query
                                             where aa.SewingOutDate >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.SewingOutDate <= dateTo
                                             select aa)
                                  join b in garmentSewingOutItemRepository.Query on a.Identity equals b.SewingOutId

                                  select new monitoringView
                                  {
                                      QtyCuttingIn = 0,
                                      PriceCuttingIn = 0,
                                      QtySewingIn = 0,
                                      PriceSewingIn = 0,
                                      QtyCuttingOut = 0,
                                      PriceCuttingOut = 0,
                                      QtyCuttingTransfer = 0,
                                      PriceCuttingTransfer = 0,
                                      AvalCutting = 0,
                                      AvalCuttingPrice = 0,
                                      AvalSewing = 0,
                                      AvalSewingPrice = 0,
                                      QtyLoading = 0,
                                      PriceLoading = 0,
                                      QtyLoadingAdjs = 0,
                                      PriceLoadingAdjs = 0,
                                      QtySewingAdj = 0,
                                      PriceSewingAdj = 0,
                                      FinishingInQty = 0,
                                      FinishingInPrice = 0,
                                      SubconInQty = 0,
                                      SubconInPrice = 0,
                                      FinishingAdjQty = 0,
                                      FinishingAdjPrice = 0,
                                      FinishingOutQty = 0,
                                      FinishingOutPrice = 0,
                                      FinishingReturQty = 0,
                                      FinishingReturPrice = 0,
                                      SubconOutQty = 0,
                                      SubconOutPrice = 0,
                                      FinishingTransferExpenditure = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitId == request.unit) ? b.Quantity : 0,
                                      FinishingTransferExpenditurePrice = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitId == request.unit) ? b.Price : 0,
                                      FinishingInTransferQty = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitToId == request.unit) ? b.Quantity : 0,
                                      FinishingInTransferPrice = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitToId == request.unit) ? b.Price : 0,
                                      BeginingBalanceFinishingQty = (a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitToId == request.unit) ? b.Quantity : 0,
                                      BeginingBalanceFinishingPrice = (a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitToId == request.unit) ? b.Price : 0,
                                      BeginingBalanceSewingQty = (a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == request.unit) ? -b.Quantity : 0 - ((a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == request.unit) ? -b.Quantity : 0) + ((a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "SEWING" && a.UnitId != a.UnitToId && a.UnitToId == request.unit) ? b.Quantity : 0) - ((a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "CUTTING" && a.UnitId == a.UnitToId && a.UnitId == request.unit) ? b.Quantity : 0) - ((a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "SEWING" && a.UnitToId != a.UnitId && a.UnitId == request.unit) ? b.Quantity : 0) - ((a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "FINISHING" && a.UnitToId != a.UnitId && a.UnitId == request.unit) ? b.Quantity : 0),
                                      BeginingBalanceSewingPrice = (a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == request.unit) ? -b.Price : 0 - ((a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == request.unit) ? -b.Price : 0) + ((a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "SEWING" && a.UnitId != a.UnitToId && a.UnitToId == request.unit) ? b.Price : 0) - ((a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "CUTTING" && a.UnitId == a.UnitToId && a.UnitId == request.unit) ? b.Price : 0) - ((a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "SEWING" && a.UnitToId != a.UnitId && a.UnitId == request.unit) ? b.Price : 0) - ((a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "FINISHING" && a.UnitToId != a.UnitId && a.UnitId == request.unit) ? b.Price : 0),

                                      QtySewingRetur = (a.SewingOutDate >= dateFrom && a.SewingTo == "CUTTING" && a.UnitId == a.UnitToId && a.UnitId == request.unit) ? b.Quantity : 0,
                                      PriceSewingRetur = (a.SewingOutDate >= dateFrom && a.SewingTo == "CUTTING" && a.UnitId == a.UnitToId && a.UnitId == request.unit) ? b.Price : 0,
                                      QtySewingInTransfer = (a.SewingOutDate >= dateFrom && a.SewingTo == "SEWING" && a.UnitId != a.UnitToId && a.UnitToId == request.unit) ? b.Quantity : 0,
                                      PriceSewingInTransfer = (a.SewingOutDate >= dateFrom && a.SewingTo == "SEWING" && a.UnitId != a.UnitToId && a.UnitToId == request.unit) ? b.Price : 0,
                                      WipSewingOut = (a.SewingOutDate >= dateFrom && a.SewingTo == "SEWING" && a.UnitToId != a.UnitId && a.UnitId == request.unit) ? b.Quantity : 0,
                                      WipSewingOutPrice = (a.SewingOutDate >= dateFrom && a.SewingTo == "SEWING" && a.UnitToId != a.UnitId && a.UnitId == request.unit) ? b.Price : 0,
                                      WipFinishingOut = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitToId != a.UnitId && a.UnitId == request.unit) ? b.Quantity : 0,
                                      WipFinishingOutPrice = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitToId != a.UnitId && a.UnitId == request.unit) ? b.Price : 0,
                                      QtySewingOut = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == request.unit) ? b.Quantity : 0,
                                      PriceSewingOut = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == request.unit) ? b.Price : 0,
                                      //BeginingBalanceExpenditureGood = (a.SewingOutDate < dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitToId == request.unit) ? -b.Quantity : 0,
                                      //BeginingBalanceExpenditureGoodPrice = (a.SewingOutDate < dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitToId == request.unit) ? -b.Price : 0,
                                      BeginingBalanceExpenditureGood = 0,
                                      BeginingBalanceExpenditureGoodPrice = 0,
                                      Ro = a.RONo,
                                      ExpenditureGoodRetur = 0,
                                      ExpenditureGoodReturPrice = 0,
                                      QtyLoadingInTransfer = 0,
                                      PriceLoadingInTransfer = 0,
                                      ExportQty = 0,
                                      ExportPrice = 0,
                                      SampleQty = 0,
                                      SamplePrice = 0,
                                      OtherQty = 0,
                                      OtherPrice = 0,
                                      ExpenditureGoodInTransfer = 0,
                                      ExpenditureGoodInTransferPrice = 0,
                                      BeginingBalanceCuttingQty = 0,
                                      BeginingBalanceCuttingPrice = 0,
                                      BeginingBalanceLoadingQty = 0,
                                      BeginingBalanceLoadingPrice = 0

                                  });

            var QuerySewingAdj = from a in (from aa in garmentAdjustmentRepository.Query
                                            where aa.AdjustmentDate >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == request.unit && aa.AdjustmentDate <= dateTo && aa.AdjustmentType == "SEWING"
                                            select aa)
                                 join b in garmentAdjustmentItemRepository.Query on a.Identity equals b.AdjustmentId
                                 select new monitoringView
                                 {
                                     QtyCuttingIn = 0,
                                     PriceCuttingIn = 0,
                                     QtySewingIn = 0,
                                     PriceSewingIn = 0,
                                     QtyCuttingOut = 0,
                                     PriceCuttingOut = 0,
                                     QtyCuttingTransfer = 0,
                                     PriceCuttingTransfer = 0,
                                     QtyCuttingsubkon = 0,
                                     PriceCuttingsubkon = 0,
                                     AvalCutting = 0,
                                     AvalCuttingPrice = 0,
                                     AvalSewing = 0,
                                     AvalSewingPrice = 0,
                                     QtyLoading = 0,
                                     PriceLoading = 0,
                                     QtyLoadingAdjs = 0,
                                     PriceLoadingAdjs = 0,
                                     QtySewingOut = 0,
                                     PriceSewingOut = 0,
                                     WipSewingOut = 0,
                                     WipSewingOutPrice = 0,
                                     WipFinishingOut = 0,
                                     WipFinishingOutPrice = 0,
                                     QtySewingRetur = 0,
                                     PriceSewingRetur = 0,
                                     QtySewingInTransfer = 0,
                                     PriceSewingInTransfer = 0,
                                     FinishingInQty = 0,
                                     FinishingInPrice = 0,
                                     SubconInQty = 0,
                                     SubconInPrice = 0,
                                     FinishingAdjQty = 0,
                                     FinishingAdjPrice = 0,
                                     FinishingTransferExpenditure = 0,
                                     FinishingTransferExpenditurePrice = 0,
                                     FinishingInTransferQty = 0,
                                     FinishingInTransferPrice = 0,
                                     FinishingOutQty = 0,
                                     FinishingOutPrice = 0,
                                     FinishingReturQty = 0,
                                     FinishingReturPrice = 0,
                                     SubconOutQty = 0,
                                     SubconOutPrice = 0,
                                     BeginingBalanceSewingQty = a.AdjustmentDate < dateFrom && a.AdjustmentDate > dateBalance ? -b.Quantity : 0,
                                     BeginingBalanceSewingPrice = a.AdjustmentDate < dateFrom && a.AdjustmentDate > dateBalance ? -b.Price : 0,
                                     Ro = a.RONo,
                                     QtySewingAdj = a.AdjustmentDate >= dateFrom ? b.Quantity : 0,
                                     PriceSewingAdj = a.AdjustmentDate >= dateFrom ? b.Price : 0,
                                     ExpenditureGoodRetur = 0,
                                     ExpenditureGoodReturPrice = 0,
                                     ExportQty = 0,
                                     ExportPrice = 0,
                                     SampleQty = 0,
                                     SamplePrice = 0,
                                     OtherQty = 0,
                                     OtherPrice = 0,
                                     QtyLoadingInTransfer = 0,
                                     PriceLoadingInTransfer = 0,
                                     ExpenditureGoodInTransfer = 0,
                                     ExpenditureGoodInTransferPrice = 0,
                                     BeginingBalanceCuttingQty = 0,
                                     BeginingBalanceCuttingPrice = 0,
                                     BeginingBalanceLoadingQty = 0,
                                     BeginingBalanceLoadingPrice = 0,
                                     BeginingBalanceFinishingQty = 0,
                                     BeginingBalanceFinishingPrice = 0
                                 };

            var QueryFinishingIn = (from a in (from aa in garmentFinishingInRepository.Query
                                               where aa.FinishingInDate >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == request.unit && aa.FinishingInDate <= dateTo
                                               select aa)
                                    join b in garmentFinishingInItemRepository.Query on a.Identity equals b.FinishingInId
                                    select new monitoringView
                                    {
                                        QtyCuttingIn = 0,
                                        PriceCuttingIn = 0,
                                        QtySewingIn = 0,
                                        PriceSewingIn = 0,
                                        QtyCuttingOut = 0,
                                        PriceCuttingOut = 0,
                                        QtyCuttingTransfer = 0,
                                        PriceCuttingTransfer = 0,
                                        QtyCuttingsubkon = 0,
                                        PriceCuttingsubkon = 0,
                                        AvalCutting = 0,
                                        AvalCuttingPrice = 0,
                                        AvalSewing = 0,
                                        AvalSewingPrice = 0,
                                        QtyLoading = 0,
                                        PriceLoading = 0,
                                        QtyLoadingAdjs = 0,
                                        PriceLoadingAdjs = 0,
                                        QtySewingOut = 0,
                                        PriceSewingOut = 0,
                                        QtySewingAdj = 0,
                                        PriceSewingAdj = 0,
                                        WipSewingOut = 0,
                                        WipSewingOutPrice = 0,
                                        WipFinishingOut = 0,
                                        WipFinishingOutPrice = 0,
                                        QtySewingRetur = 0,
                                        PriceSewingRetur = 0,
                                        QtySewingInTransfer = 0,
                                        PriceSewingInTransfer = 0,
                                        FinishingAdjQty = 0,
                                        FinishingAdjPrice = 0,
                                        FinishingTransferExpenditure = 0,
                                        FinishingTransferExpenditurePrice = 0,
                                        FinishingInTransferQty = 0,
                                        FinishingInTransferPrice = 0,
                                        FinishingOutQty = 0,
                                        FinishingOutPrice = 0,
                                        FinishingReturQty = 0,
                                        FinishingReturPrice = 0,
                                        SubconOutQty = 0,
                                        SubconOutPrice = 0,
                                        QtyLoadingInTransfer = 0,
                                        PriceLoadingInTransfer = 0,
                                        BeginingBalanceSubconQty = (a.FinishingInDate < dateFrom && a.FinishingInDate > dateBalance && a.FinishingInType == "PEMBELIAN") ? b.Quantity : 0,
                                        BeginingBalanceSubconPrice = (a.FinishingInDate < dateFrom && a.FinishingInDate > dateBalance && a.FinishingInType == "PEMBELIAN") ? b.Price : 0,
                                        BeginingBalanceFinishingQty = (a.FinishingInDate < dateFrom && a.FinishingInDate > dateBalance && a.FinishingInType != "PEMBELIAN") ? b.Quantity : 0,
                                        BeginingBalanceFinishingPrice = (a.FinishingInDate < dateFrom && a.FinishingInDate > dateBalance && a.FinishingInType != "PEMBELIAN") ? b.Price : 0,
                                        FinishingInQty = (a.FinishingInDate >= dateFrom && a.FinishingInType != "PEMBELIAN") ? b.Quantity : 0,
                                        FinishingInPrice = (a.FinishingInDate >= dateFrom && a.FinishingInType != "PEMBELIAN") ? b.Price : 0,
                                        SubconInQty = (a.FinishingInDate >= dateFrom && a.FinishingInType == "PEMBELIAN") ? b.Quantity : 0,
                                        SubconInPrice = (a.FinishingInDate >= dateFrom && a.FinishingInType == "PEMBELIAN") ? b.Price : 0,
                                        Ro = a.RONo,
                                        ExpenditureGoodRetur = 0,
                                        ExpenditureGoodReturPrice = 0,
                                        ExportQty = 0,
                                        ExportPrice = 0,
                                        SampleQty = 0,
                                        SamplePrice = 0,
                                        OtherQty = 0,
                                        OtherPrice = 0,
                                        ExpenditureGoodInTransfer = 0,
                                        ExpenditureGoodInTransferPrice = 0,
                                        BeginingBalanceCuttingQty = 0,
                                        BeginingBalanceCuttingPrice = 0,
                                        BeginingBalanceLoadingQty = 0,
                                        BeginingBalanceLoadingPrice = 0
                                    });
            var QueryFinishingOut = (from a in (from aa in garmentFinishingOutRepository.Query
                                                where aa.FinishingOutDate >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == request.unit && aa.FinishingOutDate <= dateTo && aa.FinishingTo == "GUDANG JADI"
                                                select aa)
                                     join b in garmentFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
                                     join c in garmentFinishingInItemRepository.Query on b.FinishingInItemId equals c.Identity
                                     join d in garmentFinishingInRepository.Query on c.FinishingInId equals d.Identity
                                     select new monitoringView
                                     {
                                         QtyCuttingIn = 0,
                                         PriceCuttingIn = 0,
                                         QtySewingIn = 0,
                                         PriceSewingIn = 0,
                                         QtyCuttingOut = 0,
                                         PriceCuttingOut = 0,
                                         QtyCuttingTransfer = 0,
                                         PriceCuttingTransfer = 0,
                                         QtyCuttingsubkon = 0,
                                         PriceCuttingsubkon = 0,
                                         AvalCutting = 0,
                                         AvalCuttingPrice = 0,
                                         AvalSewing = 0,
                                         AvalSewingPrice = 0,
                                         QtyLoading = 0,
                                         PriceLoading = 0,
                                         QtyLoadingAdjs = 0,
                                         PriceLoadingAdjs = 0,
                                         QtySewingOut = 0,
                                         PriceSewingOut = 0,
                                         QtySewingAdj = 0,
                                         PriceSewingAdj = 0,
                                         WipSewingOut = 0,
                                         WipSewingOutPrice = 0,
                                         WipFinishingOut = 0,
                                         WipFinishingOutPrice = 0,
                                         QtySewingRetur = 0,
                                         PriceSewingRetur = 0,
                                         QtySewingInTransfer = 0,
                                         PriceSewingInTransfer = 0,
                                         FinishingInQty = 0,
                                         FinishingInPrice = 0,
                                         SubconInQty = 0,
                                         SubconInPrice = 0,
                                         FinishingAdjQty = 0,
                                         FinishingAdjPrice = 0,
                                         FinishingTransferExpenditure = 0,
                                         FinishingTransferExpenditurePrice = 0,
                                         FinishingInTransferQty = 0,
                                         FinishingInTransferPrice = 0,
                                         FinishingReturQty = 0,
                                         FinishingReturPrice = 0,
                                         QtyLoadingInTransfer = 0,
                                         PriceLoadingInTransfer = 0,
                                         BeginingBalanceFinishingQty = (a.FinishingOutDate < dateFrom && a.FinishingOutDate > dateBalance && d.FinishingInType != "PEMBELIAN") ? -b.Quantity : 0,
                                         BeginingBalanceFinishingPrice = (a.FinishingOutDate < dateFrom && a.FinishingOutDate > dateBalance && d.FinishingInType != "PEMBELIAN") ? -b.Price : 0,
                                         BeginingBalanceExpenditureGood = ((a.FinishingOutDate < dateFrom && a.FinishingOutDate > dateBalance && d.FinishingInType != "PEMBELIAN") ? b.Quantity : 0) + ((a.FinishingOutDate < dateFrom && d.FinishingInType == "PEMBELIAN") ? b.Quantity : 0),
                                         BeginingBalanceExpenditureGoodPrice = (a.FinishingOutDate < dateFrom && a.FinishingOutDate > dateBalance && d.FinishingInType != "PEMBELIAN") ? b.Price : 0 + ((a.FinishingOutDate < dateFrom && d.FinishingInType == "PEMBELIAN") ? b.Price : 0),
                                         BeginingBalanceSubconQty = (a.FinishingOutDate < dateFrom && a.FinishingOutDate > dateBalance && d.FinishingInType == "PEMBELIAN") ? -b.Quantity : 0,
                                         BeginingBalanceSubconPrice = (a.FinishingOutDate < dateFrom && a.FinishingOutDate > dateBalance && d.FinishingInType == "PEMBELIAN") ? -b.Price : 0,

                                         FinishingOutQty = (a.FinishingOutDate >= dateFrom && d.FinishingInType != "PEMBELIAN") ? b.Quantity : 0,
                                         FinishingOutPrice = (a.FinishingOutDate >= dateFrom && d.FinishingInType != "PEMBELIAN") ? b.Price : 0,
                                         SubconOutQty = (a.FinishingOutDate >= dateFrom && d.FinishingInType == "PEMBELIAN") ? b.Quantity : 0,
                                         SubconOutPrice = (a.FinishingOutDate >= dateFrom && d.FinishingInType == "PEMBELIAN") ? b.Price : 0,
                                         Ro = a.RONo,
                                         ExpenditureGoodRetur = 0,
                                         ExpenditureGoodReturPrice = 0,
                                         ExportQty = 0,
                                         ExportPrice = 0,
                                         SampleQty = 0,
                                         SamplePrice = 0,
                                         OtherQty = 0,
                                         OtherPrice = 0,
                                         ExpenditureGoodInTransfer = 0,
                                         ExpenditureGoodInTransferPrice = 0,
                                         BeginingBalanceCuttingQty = 0,
                                         BeginingBalanceCuttingPrice = 0,
                                         BeginingBalanceLoadingQty = 0,
                                         BeginingBalanceLoadingPrice = 0

                                     });

            var QueryExpenditureGoodInTransfer = (from a in (from aa in garmentFinishingOutRepository.Query
                                                             where aa.FinishingOutDate >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId != aa.UnitToId && aa.FinishingOutDate <= dateTo && aa.FinishingTo == "GUDANG JADI" && aa.UnitToId == request.unit
                                                             select aa)
                                                  join b in garmentFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
                                                  join c in garmentFinishingInItemRepository.Query on b.FinishingInItemId equals c.Identity
                                                  join d in garmentFinishingInRepository.Query on c.FinishingInId equals d.Identity
                                                  select new monitoringView
                                                  {
                                                      QtyCuttingIn = 0,
                                                      PriceCuttingIn = 0,
                                                      QtySewingIn = 0,
                                                      PriceSewingIn = 0,
                                                      QtyCuttingOut = 0,
                                                      PriceCuttingOut = 0,
                                                      QtyCuttingTransfer = 0,
                                                      PriceCuttingTransfer = 0,
                                                      QtyCuttingsubkon = 0,
                                                      PriceCuttingsubkon = 0,
                                                      AvalCutting = 0,
                                                      AvalCuttingPrice = 0,
                                                      AvalSewing = 0,
                                                      AvalSewingPrice = 0,
                                                      QtyLoading = 0,
                                                      PriceLoading = 0,
                                                      QtyLoadingAdjs = 0,
                                                      PriceLoadingAdjs = 0,
                                                      QtySewingOut = 0,
                                                      PriceSewingOut = 0,
                                                      QtySewingAdj = 0,
                                                      PriceSewingAdj = 0,
                                                      WipSewingOut = 0,
                                                      WipSewingOutPrice = 0,
                                                      WipFinishingOut = 0,
                                                      WipFinishingOutPrice = 0,
                                                      QtySewingRetur = 0,
                                                      PriceSewingRetur = 0,
                                                      QtySewingInTransfer = 0,
                                                      PriceSewingInTransfer = 0,
                                                      FinishingInQty = 0,
                                                      FinishingInPrice = 0,
                                                      SubconInQty = 0,
                                                      SubconInPrice = 0,
                                                      FinishingAdjQty = 0,
                                                      FinishingAdjPrice = 0,
                                                      FinishingTransferExpenditure = 0,
                                                      FinishingTransferExpenditurePrice = 0,
                                                      FinishingInTransferQty = 0,
                                                      FinishingInTransferPrice = 0,
                                                      FinishingReturQty = 0,
                                                      FinishingReturPrice = 0,
                                                      BeginingBalanceFinishingQty = 0,
                                                      BeginingBalanceFinishingPrice = 0,
                                                      FinishingOutQty = 0,
                                                      FinishingOutPrice = 0,
                                                      SubconOutQty = 0,
                                                      SubconOutPrice = 0,
                                                      Ro = a.RONo,
                                                      ExpenditureGoodRetur = 0,
                                                      ExpenditureGoodReturPrice = 0,
                                                      ExportQty = 0,
                                                      ExportPrice = 0,
                                                      SampleQty = 0,
                                                      SamplePrice = 0,
                                                      OtherQty = 0,
                                                      OtherPrice = 0,
                                                      QtyLoadingInTransfer = 0,
                                                      PriceLoadingInTransfer = 0,
                                                      ExpenditureGoodInTransfer = (a.FinishingOutDate >= dateFrom) ? b.Quantity : 0,
                                                      ExpenditureGoodInTransferPrice = (a.FinishingOutDate >= dateFrom) ? b.Price : 0,
                                                      BeginingBalanceExpenditureGood = (a.FinishingOutDate < dateFrom && a.FinishingOutDate > dateBalance) ? b.Quantity : 0,
                                                      BeginingBalanceExpenditureGoodPrice = (a.FinishingOutDate < dateFrom && a.FinishingOutDate > dateBalance) ? b.Price : 0,
                                                      BeginingBalanceCuttingQty = 0,
                                                      BeginingBalanceCuttingPrice = 0,
                                                      BeginingBalanceLoadingQty = 0,
                                                      BeginingBalanceLoadingPrice = 0

                                                  });

            var QueryFinishingAdj = from a in (from aa in garmentAdjustmentRepository.Query
                                               where aa.AdjustmentDate >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == request.unit && aa.AdjustmentDate <= dateTo && aa.AdjustmentType == "FINISHING"
                                               select aa)
                                    join b in garmentAdjustmentItemRepository.Query on a.Identity equals b.AdjustmentId
                                    select new monitoringView
                                    {
                                        QtyCuttingIn = 0,
                                        PriceCuttingIn = 0,
                                        QtySewingIn = 0,
                                        PriceSewingIn = 0,
                                        QtyCuttingOut = 0,
                                        PriceCuttingOut = 0,
                                        QtyCuttingTransfer = 0,
                                        PriceCuttingTransfer = 0,
                                        QtyCuttingsubkon = 0,
                                        PriceCuttingsubkon = 0,
                                        AvalCutting = 0,
                                        AvalCuttingPrice = 0,
                                        AvalSewing = 0,
                                        AvalSewingPrice = 0,
                                        QtyLoading = 0,
                                        PriceLoading = 0,
                                        QtyLoadingAdjs = 0,
                                        PriceLoadingAdjs = 0,
                                        QtySewingOut = 0,
                                        PriceSewingOut = 0,
                                        QtySewingAdj = 0,
                                        PriceSewingAdj = 0,
                                        WipSewingOut = 0,
                                        WipSewingOutPrice = 0,
                                        WipFinishingOut = 0,
                                        WipFinishingOutPrice = 0,
                                        QtySewingRetur = 0,
                                        PriceSewingRetur = 0,
                                        QtySewingInTransfer = 0,
                                        PriceSewingInTransfer = 0,
                                        FinishingInQty = 0,
                                        FinishingInPrice = 0,
                                        SubconInQty = 0,
                                        SubconInPrice = 0,
                                        FinishingTransferExpenditure = 0,
                                        FinishingTransferExpenditurePrice = 0,
                                        FinishingInTransferQty = 0,
                                        FinishingInTransferPrice = 0,
                                        QtyLoadingInTransfer = 0,
                                        PriceLoadingInTransfer = 0,
                                        BeginingBalanceFinishingQty = a.AdjustmentDate < dateFrom && a.AdjustmentDate > dateBalance ? -b.Quantity : 0,
                                        BeginingBalanceFinishingPrice = a.AdjustmentDate < dateFrom && a.AdjustmentDate > dateBalance ? -b.Price : 0,
                                        FinishingAdjQty = a.AdjustmentDate >= dateFrom ? b.Quantity : 0,
                                        FinishingAdjPrice = a.AdjustmentDate >= dateFrom ? b.Price : 0,
                                        FinishingOutQty = 0,
                                        FinishingOutPrice = 0,
                                        FinishingReturQty = 0,
                                        FinishingReturPrice = 0,
                                        SubconOutQty = 0,
                                        SubconOutPrice = 0,
                                        Ro = a.RONo,
                                        ExpenditureGoodRetur = 0,
                                        ExpenditureGoodReturPrice = 0,
                                        ExportQty = 0,
                                        ExportPrice = 0,
                                        SampleQty = 0,
                                        SamplePrice = 0,
                                        OtherQty = 0,
                                        OtherPrice = 0,
                                        ExpenditureGoodInTransfer = 0,
                                        ExpenditureGoodInTransferPrice = 0,
                                        BeginingBalanceCuttingQty = 0,
                                        BeginingBalanceCuttingPrice = 0,
                                        BeginingBalanceLoadingQty = 0,
                                        BeginingBalanceLoadingPrice = 0
                                    };

            var QueryFinishingRetur = (from a in (from aa in garmentFinishingOutRepository.Query
                                                  where aa.FinishingOutDate >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == request.unit && aa.FinishingOutDate <= dateTo && aa.FinishingTo == "SEWING"
                                                  select aa)
                                       join b in garmentFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
                                       join c in garmentFinishingInItemRepository.Query on b.FinishingInItemId equals c.Identity
                                       join d in garmentFinishingInRepository.Query on c.FinishingInId equals d.Identity
                                       select new monitoringView
                                       {
                                           QtyCuttingIn = 0,
                                           PriceCuttingIn = 0,
                                           QtySewingIn = 0,
                                           PriceSewingIn = 0,
                                           QtyCuttingOut = 0,
                                           PriceCuttingOut = 0,
                                           QtyCuttingTransfer = 0,
                                           PriceCuttingTransfer = 0,
                                           QtyCuttingsubkon = 0,
                                           PriceCuttingsubkon = 0,
                                           AvalCutting = 0,
                                           AvalCuttingPrice = 0,
                                           AvalSewing = 0,
                                           AvalSewingPrice = 0,
                                           QtyLoading = 0,
                                           PriceLoading = 0,
                                           QtyLoadingAdjs = 0,
                                           PriceLoadingAdjs = 0,
                                           QtySewingOut = 0,
                                           PriceSewingOut = 0,
                                           QtySewingAdj = 0,
                                           PriceSewingAdj = 0,
                                           WipSewingOut = 0,
                                           WipSewingOutPrice = 0,
                                           WipFinishingOut = 0,
                                           WipFinishingOutPrice = 0,
                                           QtySewingRetur = 0,
                                           PriceSewingRetur = 0,
                                           QtySewingInTransfer = 0,
                                           PriceSewingInTransfer = 0,
                                           FinishingInQty = 0,
                                           FinishingInPrice = 0,
                                           SubconInQty = 0,
                                           SubconInPrice = 0,
                                           FinishingAdjQty = 0,
                                           FinishingAdjPrice = 0,
                                           FinishingTransferExpenditure = 0,
                                           FinishingTransferExpenditurePrice = 0,
                                           FinishingInTransferQty = 0,
                                           FinishingInTransferPrice = 0,
                                           FinishingOutQty = 0,
                                           FinishingOutPrice = 0,
                                           SubconOutQty = 0,
                                           QtyLoadingInTransfer = 0,
                                           PriceLoadingInTransfer = 0,
                                           SubconOutPrice = 0,
                                           BeginingBalanceFinishingQty = (d.FinishingInType != "PEMBELIAN" && a.FinishingOutDate < dateFrom && a.FinishingOutDate > dateBalance && a.UnitId == a.UnitToId) ? -b.Quantity : 0,
                                           BeginingBalanceFinishingPrice = (d.FinishingInType != "PEMBELIAN" && a.FinishingOutDate < dateFrom && a.FinishingOutDate > dateBalance && a.UnitId == a.UnitToId) ? -b.Price : 0,
                                           FinishingReturQty = (d.FinishingInType != "PEMBELIAN" && a.FinishingOutDate >= dateFrom && a.UnitToId == a.UnitToId) ? b.Quantity : 0,
                                           FinishingReturPrice = (d.FinishingInType != "PEMBELIAN" && a.FinishingOutDate >= dateFrom && a.UnitToId == a.UnitToId) ? b.Price : 0,
                                           Ro = a.RONo,
                                           ExpenditureGoodRetur = 0,
                                           ExpenditureGoodReturPrice = 0,
                                           ExportQty = 0,
                                           ExportPrice = 0,
                                           SampleQty = 0,
                                           SamplePrice = 0,
                                           OtherQty = 0,
                                           OtherPrice = 0,
                                           ExpenditureGoodInTransfer = 0,
                                           ExpenditureGoodInTransferPrice = 0,
                                           BeginingBalanceCuttingQty = 0,
                                           BeginingBalanceCuttingPrice = 0,
                                           BeginingBalanceLoadingQty = 0,
                                           BeginingBalanceLoadingPrice = 0
                                       });
            var QueryExpenditureGoods = (from a in (from aa in garmentExpenditureGoodRepository.Query
                                                    where aa.ExpenditureDate >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == request.unit && aa.ExpenditureDate <= dateTo
                                                    select aa)
                                         join b in garmentExpenditureGoodItemRepository.Query on a.Identity equals b.ExpenditureGoodId
                                         select new monitoringView
                                         {
                                             QtyCuttingIn = 0,
                                             PriceCuttingIn = 0,
                                             QtySewingIn = 0,
                                             PriceSewingIn = 0,
                                             QtyCuttingOut = 0,
                                             PriceCuttingOut = 0,
                                             QtyCuttingTransfer = 0,
                                             PriceCuttingTransfer = 0,
                                             QtyCuttingsubkon = 0,
                                             PriceCuttingsubkon = 0,
                                             QtyLoadingInTransfer = 0,
                                             PriceLoadingInTransfer = 0,
                                             AvalCutting = 0,
                                             AvalCuttingPrice = 0,
                                             AvalSewing = 0,
                                             AvalSewingPrice = 0,
                                             QtyLoading = 0,
                                             PriceLoading = 0,
                                             QtyLoadingAdjs = 0,
                                             PriceLoadingAdjs = 0,
                                             QtySewingOut = 0,
                                             PriceSewingOut = 0,
                                             QtySewingAdj = 0,
                                             PriceSewingAdj = 0,
                                             WipSewingOut = 0,
                                             WipSewingOutPrice = 0,
                                             WipFinishingOut = 0,
                                             WipFinishingOutPrice = 0,
                                             QtySewingRetur = 0,
                                             PriceSewingRetur = 0,
                                             QtySewingInTransfer = 0,
                                             PriceSewingInTransfer = 0,
                                             FinishingInQty = 0,
                                             FinishingInPrice = 0,
                                             SubconInQty = 0,
                                             SubconInPrice = 0,
                                             FinishingAdjQty = 0,
                                             FinishingAdjPrice = 0,
                                             FinishingTransferExpenditure = 0,
                                             FinishingTransferExpenditurePrice = 0,
                                             FinishingInTransferQty = 0,
                                             FinishingInTransferPrice = 0,
                                             FinishingOutQty = 0,
                                             FinishingOutPrice = 0,
                                             FinishingReturQty = 0,
                                             FinishingReturPrice = 0,
                                             SubconOutQty = 0,
                                             SubconOutPrice = 0,
                                             BeginingBalanceExpenditureGood = a.ExpenditureDate < dateFrom && a.ExpenditureDate > dateBalance ? -b.Quantity : 0,
                                             BeginingBalanceExpenditureGoodPrice = a.ExpenditureDate < dateFrom && a.ExpenditureDate > dateBalance ? -b.Price : 0,
                                             ExportQty = (a.ExpenditureDate >= dateFrom && a.ExpenditureType == "EXPORT") ? b.Quantity : 0,
                                             ExportPrice = (a.ExpenditureDate >= dateFrom && a.ExpenditureType == "EXPORT") ? b.Price : 0,
                                             SampleQty = (a.ExpenditureDate >= dateFrom && (a.ExpenditureType == "LAIN-LAIN")) ? b.Quantity : 0,
                                             SamplePrice = (a.ExpenditureDate >= dateFrom & (a.ExpenditureType == "LAIN-LAIN")) ? b.Price : 0,
                                             OtherQty = (a.ExpenditureDate >= dateFrom && (a.ExpenditureType == "SISA")) ? b.Quantity : 0,
                                             OtherPrice = (a.ExpenditureDate >= dateFrom && (a.ExpenditureType == "SISA")) ? b.Price : 0,
                                             Ro = a.RONo,
                                             ExpenditureGoodRetur = 0,
                                             ExpenditureGoodReturPrice = 0,
                                             ExpenditureGoodInTransfer = 0,
                                             ExpenditureGoodInTransferPrice = 0,
                                             BeginingBalanceCuttingQty = 0,
                                             BeginingBalanceCuttingPrice = 0,
                                             BeginingBalanceLoadingQty = 0,
                                             BeginingBalanceLoadingPrice = 0,
                                             BeginingBalanceFinishingQty = 0,
                                             BeginingBalanceFinishingPrice = 0
                                         });
            var QueryExpenditureGoodsAdj = from a in (from aa in garmentAdjustmentRepository.Query
                                                      where aa.AdjustmentDate >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == request.unit && aa.AdjustmentDate <= dateTo && aa.AdjustmentType == "BARANG JADI"
                                                      select aa)
                                           join b in garmentAdjustmentItemRepository.Query on a.Identity equals b.AdjustmentId
                                           select new monitoringView
                                           {
                                               QtyCuttingIn = 0,
                                               PriceCuttingIn = 0,
                                               QtySewingIn = 0,
                                               PriceSewingIn = 0,
                                               QtyCuttingOut = 0,
                                               PriceCuttingOut = 0,
                                               QtyCuttingTransfer = 0,
                                               PriceCuttingTransfer = 0,
                                               QtyCuttingsubkon = 0,
                                               PriceCuttingsubkon = 0,
                                               AvalCutting = 0,
                                               AvalCuttingPrice = 0,
                                               AvalSewing = 0,
                                               AvalSewingPrice = 0,
                                               QtyLoading = 0,
                                               PriceLoading = 0,
                                               QtyLoadingAdjs = 0,
                                               PriceLoadingAdjs = 0,
                                               QtyLoadingInTransfer = 0,
                                               PriceLoadingInTransfer = 0,
                                               QtySewingOut = 0,
                                               PriceSewingOut = 0,
                                               QtySewingAdj = 0,
                                               PriceSewingAdj = 0,
                                               WipSewingOut = 0,
                                               WipSewingOutPrice = 0,
                                               WipFinishingOut = 0,
                                               WipFinishingOutPrice = 0,
                                               QtySewingRetur = 0,
                                               PriceSewingRetur = 0,
                                               QtySewingInTransfer = 0,
                                               PriceSewingInTransfer = 0,
                                               FinishingInQty = 0,
                                               FinishingInPrice = 0,
                                               SubconInQty = 0,
                                               SubconInPrice = 0,
                                               FinishingAdjQty = 0,
                                               FinishingAdjPrice = 0,
                                               FinishingTransferExpenditure = 0,
                                               FinishingTransferExpenditurePrice = 0,
                                               FinishingInTransferQty = 0,
                                               FinishingInTransferPrice = 0,
                                               FinishingOutQty = 0,
                                               FinishingOutPrice = 0,
                                               FinishingReturQty = 0,
                                               FinishingReturPrice = 0,
                                               SubconOutQty = 0,
                                               SubconOutPrice = 0,
                                               BeginingBalanceExpenditureGood = a.AdjustmentDate < dateFrom && a.AdjustmentDate > dateBalance ? -b.Quantity : 0,
                                               BeginingBalanceExpenditureGoodPrice = a.AdjustmentDate < dateFrom && a.AdjustmentDate > dateBalance ? -b.Price : 0,
                                               ExpenditureGoodAdj = a.AdjustmentDate >= dateFrom ? b.Quantity : 0,
                                               ExpenditureGoodAdjPrice = a.AdjustmentDate >= dateFrom ? b.Price : 0,
                                               Ro = a.RONo,
                                               ExpenditureGoodRetur = 0,
                                               ExpenditureGoodReturPrice = 0,
                                               ExportQty = 0,
                                               ExportPrice = 0,
                                               SampleQty = 0,
                                               SamplePrice = 0,
                                               OtherQty = 0,
                                               OtherPrice = 0,
                                               ExpenditureGoodInTransfer = 0,
                                               ExpenditureGoodInTransferPrice = 0,
                                               BeginingBalanceCuttingQty = 0,
                                               BeginingBalanceCuttingPrice = 0,
                                               BeginingBalanceLoadingQty = 0,
                                               BeginingBalanceLoadingPrice = 0,
                                               BeginingBalanceFinishingQty = 0,
                                               BeginingBalanceFinishingPrice = 0
                                           };
            var QueryExpenditureGoodRetur = from a in (from aa in garmentExpenditureGoodReturnRepository.Query
                                                       where aa.ReturDate >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == request.unit && aa.ReturDate <= dateTo
                                                       select aa)
                                            join b in garmentExpenditureGoodReturnItemRepository.Query on a.Identity equals b.ReturId
                                            select new monitoringView
                                            {
                                                QtyCuttingIn = 0,
                                                PriceCuttingIn = 0,
                                                QtySewingIn = 0,
                                                PriceSewingIn = 0,
                                                QtyCuttingOut = 0,
                                                PriceCuttingOut = 0,
                                                QtyCuttingTransfer = 0,
                                                PriceCuttingTransfer = 0,
                                                QtyLoadingInTransfer = 0,
                                                PriceLoadingInTransfer = 0,
                                                QtyCuttingsubkon = 0,
                                                PriceCuttingsubkon = 0,
                                                AvalCutting = 0,
                                                AvalCuttingPrice = 0,
                                                AvalSewing = 0,
                                                AvalSewingPrice = 0,
                                                QtyLoading = 0,
                                                PriceLoading = 0,
                                                QtyLoadingAdjs = 0,
                                                PriceLoadingAdjs = 0,
                                                QtySewingOut = 0,
                                                PriceSewingOut = 0,
                                                QtySewingAdj = 0,
                                                PriceSewingAdj = 0,
                                                WipSewingOut = 0,
                                                WipSewingOutPrice = 0,
                                                WipFinishingOut = 0,
                                                WipFinishingOutPrice = 0,
                                                QtySewingRetur = 0,
                                                PriceSewingRetur = 0,
                                                QtySewingInTransfer = 0,
                                                PriceSewingInTransfer = 0,
                                                FinishingInQty = 0,
                                                FinishingInPrice = 0,
                                                SubconInQty = 0,
                                                SubconInPrice = 0,
                                                FinishingAdjQty = 0,
                                                FinishingAdjPrice = 0,
                                                FinishingTransferExpenditure = 0,
                                                FinishingTransferExpenditurePrice = 0,
                                                FinishingInTransferQty = 0,
                                                FinishingInTransferPrice = 0,
                                                FinishingOutQty = 0,
                                                FinishingOutPrice = 0,
                                                FinishingReturQty = 0,
                                                FinishingReturPrice = 0,
                                                SubconOutQty = 0,
                                                SubconOutPrice = 0,
                                                BeginingBalanceExpenditureGood = a.ReturDate < dateFrom && a.ReturDate > dateBalance ? b.Quantity : 0,
                                                BeginingBalanceExpenditureGoodPrice = a.ReturDate < dateFrom && a.ReturDate > dateBalance ? b.Price : 0,
                                                ExpenditureGoodRetur = a.ReturDate >= dateFrom ? b.Quantity : 0,
                                                ExpenditureGoodReturPrice = a.ReturDate >= dateFrom ? b.Price : 0,
                                                Ro = a.RONo,
                                                ExportQty = 0,
                                                ExportPrice = 0,
                                                SampleQty = 0,
                                                SamplePrice = 0,
                                                OtherQty = 0,
                                                OtherPrice = 0,
                                                ExpenditureGoodInTransfer = 0,
                                                ExpenditureGoodInTransferPrice = 0,
                                                BeginingBalanceCuttingQty = 0,
                                                BeginingBalanceCuttingPrice = 0,
                                                BeginingBalanceLoadingQty = 0,
                                                BeginingBalanceLoadingPrice = 0,
                                                BeginingBalanceFinishingQty = 0,
                                                BeginingBalanceFinishingPrice = 0,
                                                Article = a.Article

                                            };


            var queryNow = QueryCuttingIn
                .Union(queryBalance)
                .Union(QueryCuttingOut)
                .Union(QueryCuttingOutSubkon)
                .Union(QueryCuttingOutTransfer)
                .Union(QueryAvalCompCutting)
                .Union(QueryAvalCompSewing)
                .Union(QuerySewingDO)
                .Union(QueryLoading)
                .Union(QueryLoadingAdj)
                .Union(QuerySewingIn)
                .Union(QuerySewingOut)
                .Union(QuerySewingAdj)
                .Union(QueryFinishingIn)
                .Union(QueryFinishingOut)
                .Union(QueryFinishingAdj)
                .Union(QueryFinishingRetur)
                .Union(QueryExpenditureGoods)
                .Union(QueryExpenditureGoodsAdj)
                .Union(QueryExpenditureGoodRetur)
                .Union(QueryExpenditureGoodInTransfer)
                .Union(QueryLoadingInTransfer)
                .AsEnumerable();

            var querySum = (from a in queryNow
                            join b in queryGroup on a.Ro equals b.Ro
                            select new
                            {
                                b.Article,
                                b.Comodity,
                                b.FC,
                                b.BasicPrice,
                                b.Fare,
                                b.FareNew,
                                a.Ro,
                                a.BeginingBalanceCuttingQty,
                                a.BeginingBalanceCuttingPrice,
                                a.QtyCuttingIn,
                                a.PriceCuttingIn,
                                a.QtyCuttingOut,
                                a.PriceCuttingOut,
                                a.QtyCuttingTransfer,
                                a.PriceCuttingTransfer,
                                a.QtyCuttingsubkon,
                                a.PriceCuttingsubkon,
                                a.AvalCutting,
                                a.AvalCuttingPrice,
                                a.AvalSewing,
                                a.AvalSewingPrice,
                                a.BeginingBalanceLoadingQty,
                                a.BeginingBalanceLoadingPrice,
                                a.QtyLoadingIn,
                                a.PriceLoadingIn,
                                a.QtyLoading,
                                a.PriceLoading,
                                a.QtyLoadingAdjs,
                                a.PriceLoadingAdjs,
                                a.BeginingBalanceSewingQty,
                                a.BeginingBalanceSewingPrice,
                                a.QtySewingIn,
                                a.PriceSewingIn,
                                a.QtySewingOut,
                                a.PriceSewingOut,
                                a.QtySewingInTransfer,
                                a.PriceSewingInTransfer,
                                a.WipSewingOut,
                                a.WipSewingOutPrice,
                                a.WipFinishingOut,
                                a.WipFinishingOutPrice,
                                a.QtySewingRetur,
                                a.PriceSewingRetur,
                                a.QtySewingAdj,
                                a.PriceSewingAdj,
                                a.BeginingBalanceFinishingQty,
                                a.BeginingBalanceFinishingPrice,
                                a.FinishingInQty,
                                a.FinishingInPrice,
                                a.BeginingBalanceSubconQty,
                                a.BeginingBalanceSubconPrice,
                                a.SubconInQty,
                                a.SubconInPrice,
                                a.SubconOutQty,
                                a.SubconOutPrice,
                                a.FinishingOutQty,
                                a.FinishingOutPrice,
                                a.FinishingInTransferQty,
                                a.FinishingInTransferPrice,
                                a.FinishingAdjQty,
                                a.FinishingAdjPrice,
                                a.FinishingReturQty,
                                a.FinishingReturPrice,
                                a.BeginingBalanceExpenditureGood,
                                a.BeginingBalanceExpenditureGoodPrice,
                                a.ExpenditureGoodRetur,
                                a.ExpenditureGoodReturPrice,
                                a.ExportQty,
                                a.ExportPrice,
                                a.OtherQty,
                                a.OtherPrice,
                                a.SampleQty,
                                a.SamplePrice,
                                a.ExpenditureGoodAdj,
                                a.ExpenditureGoodAdjPrice,
                                a.ExpenditureGoodInTransfer,
                                a.ExpenditureGoodInTransferPrice,
                                a.QtyLoadingInTransfer,
                                a.PriceLoadingInTransfer
                            })
                .GroupBy(x => new { x.FareNew, x.Fare, x.BasicPrice, x.FC, x.Ro, x.Article, x.Comodity }, (key, group) => new
                {
                    ro = key.Ro,
                    article = key.Article,
                    comodity = key.Comodity,
                    fc = key.FC,
                    fare = key.Fare,
                    farenew = key.FareNew,
                    basicprice = key.BasicPrice,
                    qtycutting = group.Sum(s => s.QtyCuttingOut),
                    priceCuttingOut = group.Sum(s => s.PriceCuttingOut),
                    qtCuttingSubkon = group.Sum(s => s.QtyCuttingsubkon),
                    priceCuttingSubkon = group.Sum(s => s.PriceCuttingsubkon),
                    qtyCuttingTransfer = group.Sum(s => s.QtyCuttingTransfer),
                    priceCuttingTransfer = group.Sum(s => s.PriceCuttingTransfer),
                    qtyCuttingIn = group.Sum(s => s.QtyCuttingIn),
                    priceCuttingIn = group.Sum(s => s.PriceCuttingIn),
                    begining = group.Sum(s => s.BeginingBalanceCuttingQty),
                    beginingcuttingPrice = group.Sum(s => s.BeginingBalanceCuttingPrice),
                    qtyavalsew = group.Sum(s => s.AvalSewing),
                    priceavalsew = group.Sum(s => s.AvalSewingPrice),
                    qtyavalcut = group.Sum(s => s.AvalCutting),
                    priceavalcut = group.Sum(s => s.AvalCuttingPrice),
                    beginingloading = group.Sum(s => s.BeginingBalanceLoadingQty),
                    beginingloadingPrice = group.Sum(s => s.BeginingBalanceLoadingPrice),
                    qtyLoadingIn = group.Sum(s => s.QtyLoadingIn),
                    priceLoadingIn = group.Sum(s => s.PriceLoadingIn),
                    qtyloading = group.Sum(s => s.QtyLoading),
                    priceloading = group.Sum(s => s.PriceLoading),
                    qtyLoadingAdj = group.Sum(s => s.QtyLoadingAdjs),
                    priceLoadingAdj = group.Sum(s => s.PriceLoadingAdjs),
                    beginingSewing = group.Sum(s => s.BeginingBalanceSewingQty),
                    beginingSewingPrice = group.Sum(s => s.BeginingBalanceSewingPrice),
                    sewingIn = group.Sum(s => s.QtySewingIn),
                    sewingInPrice = group.Sum(s => s.PriceSewingIn),
                    sewingintransfer = group.Sum(s => s.QtySewingInTransfer),
                    sewingintransferPrice = group.Sum(s => s.PriceSewingInTransfer),
                    sewingout = group.Sum(s => s.QtySewingOut),
                    sewingoutPrice = group.Sum(s => s.PriceSewingOut),
                    sewingretur = group.Sum(s => s.QtySewingRetur),
                    sewingreturPrice = group.Sum(s => s.PriceSewingRetur),
                    wipsewing = group.Sum(s => s.WipSewingOut),
                    wipsewingPrice = group.Sum(s => s.WipSewingOutPrice),
                    wipfinishing = group.Sum(s => s.WipFinishingOut),
                    wipfinishingPrice = group.Sum(s => s.WipFinishingOutPrice),
                    sewingadj = group.Sum(s => s.QtySewingAdj),
                    sewingadjPrice = group.Sum(s => s.PriceSewingAdj),
                    finishingin = group.Sum(s => s.FinishingInQty),
                    finishinginPrice = group.Sum(s => s.FinishingInPrice),
                    finishingintransfer = group.Sum(s => s.FinishingInTransferQty),
                    finishingintransferPrice = group.Sum(s => s.FinishingInTransferPrice),
                    finishingadj = group.Sum(s => s.FinishingAdjQty),
                    finishingadjPrice = group.Sum(s => s.FinishingAdjPrice),
                    finishingout = group.Sum(s => s.FinishingOutQty),
                    finishingoutPrice = group.Sum(s => s.FinishingOutPrice),
                    finishinigretur = group.Sum(s => s.FinishingReturQty),
                    finishinigreturPrice = group.Sum(s => s.FinishingReturPrice),
                    beginingbalanceFinishing = group.Sum(s => s.BeginingBalanceFinishingQty),
                    beginingbalanceFinishingPrice = group.Sum(s => s.BeginingBalanceFinishingPrice),
                    beginingbalancesubcon = group.Sum(s => s.BeginingBalanceSubconQty),
                    beginingbalancesubconPrice = group.Sum(s => s.BeginingBalanceSubconPrice),
                    subconIn = group.Sum(s => s.SubconInQty),
                    subconInPrice = group.Sum(s => s.SubconInPrice),
                    subconout = group.Sum(s => s.SubconOutQty),
                    subconoutPrice = group.Sum(s => s.SubconOutPrice),
                    exportQty = group.Sum(s => s.ExportQty),
                    exportPrice = group.Sum(s => s.ExportPrice),
                    otherqty = group.Sum(s => s.OtherQty),
                    otherprice = group.Sum(s => s.OtherPrice),
                    sampleQty = group.Sum(s => s.SampleQty),
                    samplePrice = group.Sum(s => s.SamplePrice),
                    expendAdj = group.Sum(s => s.ExpenditureGoodAdj),
                    expendAdjPrice = group.Sum(s => s.ExpenditureGoodAdjPrice),
                    expendRetur = group.Sum(s => s.ExpenditureGoodRetur),
                    expendReturPrice = group.Sum(s => s.ExpenditureGoodReturPrice),
                    //finishinginqty =group.Sum(s=>s.FinishingInQty)
                    beginingBalanceExpenditureGood = group.Sum(s => s.BeginingBalanceExpenditureGood),
                    beginingBalanceExpenditureGoodPrice = group.Sum(s => s.BeginingBalanceExpenditureGoodPrice),
                    expenditureInTransfer = group.Sum(s => s.ExpenditureGoodInTransfer),
                    expenditureInTransferPrice = group.Sum(s => s.ExpenditureGoodInTransferPrice),
                    qtyloadingInTransfer = group.Sum(s => s.QtyLoadingInTransfer),
                    priceloadingInTransfer = group.Sum(s => s.PriceLoadingInTransfer)



                });

            GarmentMonitoringProductionStockFlowListViewModel garmentMonitoringProductionFlow = new GarmentMonitoringProductionStockFlowListViewModel();
            List<GarmentMonitoringProductionStockFlowDto> monitoringDtos = new List<GarmentMonitoringProductionStockFlowDto>();

            foreach (var item in querySum.OrderBy(s=>s.ro))
            {

                var fc = Math.Round(Convert.ToDouble(item.fc), 2);
                var basicPrice = Math.Round(Convert.ToDouble(item.basicprice) * fc, 2);

                GarmentMonitoringProductionStockFlowDto garmentMonitoringDto = new GarmentMonitoringProductionStockFlowDto()
                {
                    Article = item.article,
                    Ro = item.ro,
                    FC = fc,
                    Fare = item.fare,
                    BasicPrice = basicPrice,
                    BeginingBalanceCuttingQty = item.begining < 0 ? 0 : item.begining,
                    BeginingBalanceCuttingPrice = Math.Round(((Convert.ToDouble(item.fare) * 0.25) + basicPrice) * item.begining, 2) < 0 ? 0 : Math.Round(((Convert.ToDouble(item.fare) * 0.25) + basicPrice) * item.begining, 2),
                    QtyCuttingTransfer = Math.Round(item.qtyCuttingTransfer, 2),
                    PriceCuttingTransfer = Math.Round(((Convert.ToDouble(item.fare) * 0.25) + basicPrice) * item.qtyCuttingTransfer, 2),
                    QtyCuttingsubkon = Math.Round(item.qtCuttingSubkon, 2),
                    PriceCuttingsubkon = Math.Round(((Convert.ToDouble(item.fare) * 0.25) + basicPrice) * item.qtCuttingSubkon, 2),
                    QtyCuttingIn = Math.Round(item.qtyCuttingIn, 2),
                    PriceCuttingIn = Math.Round(((Convert.ToDouble(item.fare) * 0.25) + basicPrice) * item.qtyCuttingIn, 2),
                    QtyCuttingOut = Math.Round(item.qtycutting, 2),
                    PriceCuttingOut = Math.Round(((Convert.ToDouble(item.fare) * 0.25) + basicPrice) * item.qtycutting, 2),
                    Comodity = item.comodity,
                    AvalCutting = Math.Round(item.qtyavalcut, 2),
                    AvalCuttingPrice = Math.Round(((Convert.ToDouble(item.fare) * 0.25) + basicPrice) * item.qtyavalcut, 2),
                    AvalSewing = Math.Round(item.qtyavalsew, 2),
                    AvalSewingPrice = Math.Round(((Convert.ToDouble(item.fare) * 0.25) + basicPrice) * item.qtyavalsew, 2),
                    EndBalancCuttingeQty = Math.Round(item.begining + item.qtyCuttingIn - item.qtycutting - item.qtyCuttingTransfer - item.qtCuttingSubkon /*- item.qtyavalcut - item.qtyavalsew*/, 2) < 0 ? 0 : Math.Round(item.begining + item.qtyCuttingIn - item.qtycutting - item.qtyCuttingTransfer - item.qtCuttingSubkon /* - item.qtyavalcut - item.qtyavalsew*/, 2),
                    EndBalancCuttingePrice = Math.Round(((Convert.ToDouble(item.fare) * 0.25) + basicPrice) * (item.begining + item.qtyCuttingIn - item.qtycutting - item.qtyCuttingTransfer - item.qtCuttingSubkon /*- item.qtyavalcut - item.qtyavalsew*/), 2) < 0 ? 0 : Math.Round(((Convert.ToDouble(item.fare) * 0.25) + basicPrice) * (item.begining + item.qtyCuttingIn - item.qtycutting - item.qtyCuttingTransfer - item.qtCuttingSubkon /*- item.qtyavalcut - item.qtyavalsew*/), 2),
                    BeginingBalanceLoadingQty = Math.Round(item.beginingloading, 2) < 0 ? 0 : Math.Round(item.beginingloading, 2),
                    BeginingBalanceLoadingPrice = Math.Round(((Convert.ToDouble(item.fare) * 0.25) + basicPrice) * item.beginingloading, 2) < 0 ? 0 : Math.Round(((Convert.ToDouble(item.fare) * 0.25) + basicPrice) * item.beginingloading, 2),
                    QtyLoadingIn = Math.Round(item.qtyLoadingIn, 2),
                    PriceLoadingIn = Math.Round(((Convert.ToDouble(item.fare) * 0.25) + basicPrice) * item.qtyLoadingIn, 2),
                    QtyLoadingInTransfer = Math.Round(item.qtyloadingInTransfer, 2),
                    PriceLoadingInTransfer = Math.Round(((Convert.ToDouble(item.fare) * 0.25) + basicPrice) * item.qtyloadingInTransfer, 2),
                    QtyLoading = Math.Round(item.qtyloading, 2),
                    PriceLoading = Math.Round(((Convert.ToDouble(item.fare) * 0.25) + basicPrice) * item.qtyloading, 2),
                    QtyLoadingAdjs = Math.Round(item.qtyLoadingAdj, 2),
                    PriceLoadingAdjs = Math.Round(((Convert.ToDouble(item.fare) * 0.25) + basicPrice) * item.qtyLoadingAdj, 2),
                    EndBalanceLoadingQty = (Math.Round(item.beginingloading + item.qtyLoadingIn + item.qtyloadingInTransfer - item.qtyloading - item.qtyLoadingAdj, 2)) < 0 ? 0 : (Math.Round(item.beginingloading + item.qtyLoadingIn + item.qtyloadingInTransfer - item.qtyloading - item.qtyLoadingAdj, 2)),
                    EndBalanceLoadingPrice = (Math.Round(((Convert.ToDouble(item.fare) * 0.25) + basicPrice) * (item.beginingloading + item.qtyLoadingIn + item.qtyloadingInTransfer - item.qtyloading - item.qtyLoadingAdj), 2)) < 0 ? 0 : (Math.Round(((Convert.ToDouble(item.fare) * 0.25) + basicPrice) * (item.beginingloading + item.qtyLoadingIn + item.qtyloadingInTransfer - item.qtyloading - item.qtyLoadingAdj), 2)),
                    BeginingBalanceSewingQty = Math.Round(item.beginingSewing, 2),
                    BeginingBalanceSewingPrice = Math.Round(((Convert.ToDouble(item.fare) * 0.5) + basicPrice) * item.beginingSewing, 2),
                    QtySewingIn = Math.Round(item.sewingIn, 2),
                    PriceSewingIn = Math.Round(((Convert.ToDouble(item.fare) * 0.5) + basicPrice) * item.sewingIn, 2),
                    QtySewingOut = Math.Round(item.sewingout, 2),
                    PriceSewingOut = Math.Round(((Convert.ToDouble(item.fare) * 0.5) + basicPrice) * item.sewingout, 2),
                    QtySewingInTransfer = Math.Round(item.sewingintransfer, 2),
                    PriceSewingInTransfer = Math.Round(((Convert.ToDouble(item.fare) * 0.5) + basicPrice) * item.sewingintransfer, 2),
                    QtySewingRetur = Math.Round(item.sewingretur, 2),
                    PriceSewingRetur = Math.Round(((Convert.ToDouble(item.fare) * 0.5) + basicPrice) * item.sewingretur, 2),
                    WipSewingOut = Math.Round(item.wipsewing, 2),
                    WipSewingOutPrice = Math.Round(((Convert.ToDouble(item.fare) * 0.5) + basicPrice) * item.wipsewing, 2),
                    WipFinishingOut = Math.Round(item.wipfinishing, 2),
                    WipFinishingOutPrice = Math.Round(((Convert.ToDouble(item.fare) * 0.5) + basicPrice) * item.wipfinishing, 2),
                    QtySewingAdj = Math.Round(item.sewingadj, 2),
                    PriceSewingAdj = Math.Round(((Convert.ToDouble(item.fare) * 0.5) + basicPrice) * item.sewingadj, 2),
                    EndBalanceSewingQty = Math.Round(item.beginingSewing + item.sewingIn - item.sewingout + item.sewingintransfer - item.wipsewing - item.wipfinishing - item.sewingretur - item.sewingadj, 2),
                    EndBalanceSewingPrice = Math.Round(((Convert.ToDouble(item.fare) * 0.5) + basicPrice) * Math.Round(item.beginingSewing + item.sewingIn - item.sewingout + item.sewingintransfer - item.wipsewing - item.wipfinishing - item.sewingretur - item.sewingadj, 2), 2),
                    BeginingBalanceFinishingQty = Math.Round(item.beginingbalanceFinishing, 2),
                    BeginingBalanceFinishingPrice = Math.Round(((Convert.ToDouble(item.fare) * 0.75) + basicPrice) * item.beginingbalanceFinishing, 2),
                    FinishingInExpenditure = Math.Round(item.finishingout + item.subconout, 2),
                    FinishingInExpenditurepPrice = Math.Round((((Convert.ToDouble(item.fare) * 0.75) + basicPrice) * item.finishingout) + (((Convert.ToDouble(item.fare) * 0.75) + basicPrice) * item.subconout), 2),
                    FinishingInQty = Math.Round(item.finishingin, 2),
                    FinishingInPrice = Math.Round(((Convert.ToDouble(item.fare) * 0.75) + basicPrice) * item.finishingin, 2),
                    FinishingOutQty = Math.Round(item.finishingout, 2),
                    FinishingOutPrice = Math.Round(((Convert.ToDouble(item.fare) * 0.75) + basicPrice) * item.finishingout, 2),
                    BeginingBalanceSubconQty = Math.Round(item.beginingbalancesubcon, 2),
                    BeginingBalanceSubconPrice = Math.Round(((Convert.ToDouble(item.fare) * 0.75) + basicPrice) * item.beginingbalancesubcon, 2),
                    SubconInQty = Math.Round(item.subconIn, 2),
                    SubconInPrice = Math.Round(((Convert.ToDouble(item.fare) * 0.75) + basicPrice) * item.subconIn, 2),
                    SubconOutQty = Math.Round(item.subconout, 2),
                    SubconOutPrice = Math.Round(((Convert.ToDouble(item.fare) * 0.75) + basicPrice) * item.subconout, 2),
                    EndBalanceSubconQty = Math.Round(item.beginingbalancesubcon + item.subconIn - item.subconout, 2),
                    EndBalanceSubconPrice = Math.Round(((Convert.ToDouble(item.fare) * 0.75) + basicPrice) * (item.beginingbalancesubcon + item.subconIn - item.subconout), 2),
                    FinishingInTransferQty = Math.Round(item.finishingintransfer, 2),
                    FinishingInTransferPrice = Math.Round(((Convert.ToDouble(item.fare) * 0.75) + basicPrice) * item.finishingintransfer, 2),
                    FinishingReturQty = Math.Round(item.finishinigretur, 2),
                    FinishingReturPrice = Math.Round(((Convert.ToDouble(item.fare) * 0.75) + basicPrice) * item.finishinigretur, 2),
                    FinishingAdjQty = Math.Round(item.finishingadj, 2),
                    FinishingAdjPRice = Math.Round(((Convert.ToDouble(item.fare) * 0.75) + basicPrice) * item.finishingadj, 2),
                    BeginingBalanceExpenditureGood = Math.Round(item.beginingBalanceExpenditureGood, 2),
                    BeginingBalanceExpenditureGoodPrice = Math.Round(((Convert.ToDouble(item.fare)) + basicPrice) * item.beginingBalanceExpenditureGood, 2),
                    EndBalanceFinishingQty = Math.Round(item.beginingbalanceFinishing + item.finishingin + item.finishingintransfer - item.finishingout - item.finishingadj - item.finishinigretur, 2),
                    EndBalanceFinishingPrice = Math.Round(((Convert.ToDouble(item.fare) * 0.75) + basicPrice) * (item.beginingbalanceFinishing + item.finishingin + item.finishingintransfer - item.finishingout - item.finishingadj - item.finishinigretur), 2),
                    ExportQty = Math.Round(item.exportQty, 2),
                    ExportPrice = Math.Round(((Convert.ToDouble(item.fare)) + basicPrice) * item.exportQty, 2),
                    SampleQty = Math.Round(item.sampleQty, 2),
                    SamplePrice = Math.Round(((Convert.ToDouble(item.fare)) + basicPrice) * item.sampleQty, 2),
                    OtherQty = Math.Round(item.otherqty, 2),
                    OtherPrice = Math.Round(((Convert.ToDouble(item.fare)) + basicPrice) * item.otherqty, 2),
                    ExpenditureGoodAdj = Math.Round(item.expendAdj, 2),
                    ExpenditureGoodAdjPrice = Math.Round(((Convert.ToDouble(item.fare)) + basicPrice) * item.expendAdj, 2),
                    ExpenditureGoodRetur = Math.Round(item.expendRetur, 2),
                    ExpenditureGoodReturPrice = Math.Round(((Convert.ToDouble(item.fare)) + basicPrice) * item.expendRetur, 2),
                    ExpenditureGoodInTransfer = Math.Round(item.expenditureInTransfer, 2),
                    ExpenditureGoodInTransferPrice = Math.Round(((Convert.ToDouble(item.fare)) + basicPrice) * item.expenditureInTransfer, 2),
                    EndBalanceExpenditureGood = Math.Round(item.beginingBalanceExpenditureGood + item.finishingout + item.subconout + item.expendRetur + item.expenditureInTransfer - item.exportQty - item.otherqty - item.sampleQty - item.expendAdj, 2),
                    EndBalanceExpenditureGoodPrice = Math.Round(((Convert.ToDouble(item.fare)) + basicPrice) * (item.beginingBalanceExpenditureGood + item.finishingout + item.subconout + item.expendRetur + item.expenditureInTransfer - item.exportQty - item.otherqty - item.sampleQty - item.expendAdj), 2),
                    FareNew = item.farenew,
                    CuttingNew = Math.Round(item.farenew * Convert.ToDecimal(item.begining + item.qtyCuttingIn - item.qtycutting - item.qtyCuttingTransfer - item.qtCuttingSubkon - item.qtyavalcut - item.qtyavalsew), 2),
                    LoadingNew = Math.Round(item.farenew * Convert.ToDecimal(item.beginingloading + item.qtyLoadingIn - item.qtyloading - item.qtyLoadingAdj), 2),
                    SewingNew = Math.Round(item.farenew * Convert.ToDecimal(item.beginingSewing + item.sewingIn - item.sewingout + item.sewingintransfer - item.wipsewing - item.wipfinishing - item.sewingretur - item.sewingadj), 2),
                    FinishingNew = Math.Round(item.farenew * Convert.ToDecimal(item.beginingbalanceFinishing + item.finishingin + item.finishingintransfer - item.finishingout - item.finishingadj - item.finishinigretur), 2),
                    ExpenditureNew = Math.Round(item.farenew * Convert.ToDecimal(item.beginingBalanceExpenditureGood + item.finishingout + item.subconout + item.expendRetur + item.expenditureInTransfer - item.exportQty - item.otherqty - item.sampleQty - item.expendAdj), 2),
                    SubconNew = Math.Round(item.farenew * Convert.ToDecimal(item.beginingbalancesubcon + item.subconIn - item.subconout), 2)
                };
                monitoringDtos.Add(garmentMonitoringDto);

            }
            var data = from a in monitoringDtos
                       where a.BeginingBalanceCuttingQty > 0 || a.QtyCuttingIn > 0 || a.QtyCuttingOut > 0 || a.QtyCuttingsubkon > 0 || a.QtyCuttingTransfer > 0 || a.EndBalancCuttingeQty > 0 ||
                        a.BeginingBalanceLoadingQty > 0 || a.QtyLoading > 0 || a.QtyLoadingAdjs > 0 || a.QtyLoadingIn > 0 || a.QtyLoadingInTransfer > 0 || a.EndBalanceLoadingQty > 0 ||
                        a.BeginingBalanceSewingQty > 0 || a.QtySewingAdj > 0 || a.QtySewingIn > 0 || a.QtySewingInTransfer > 0 || a.QtySewingOut > 0 || a.QtySewingRetur > 0 || a.WipSewingOut > 0 || a.WipFinishingOut > 0 || a.EndBalanceSewingQty > 0 ||
                        a.BeginingBalanceSubconQty > 0 || a.EndBalanceSubconQty > 0 || a.SubconInQty > 0 || a.SubconOutQty > 0 || a.AvalCutting > 0 || a.AvalSewing > 0 ||
                        a.BeginingBalanceFinishingQty > 0 || a.FinishingAdjQty > 0 || a.FinishingInExpenditure > 0 || a.FinishingInQty > 0 || a.FinishingInTransferQty > 0 || a.FinishingOutQty > 0 || a.FinishingReturQty > 0 ||
                        a.BeginingBalanceExpenditureGood > 0 || a.ExpenditureGoodAdj > 0 || a.ExpenditureGoodInTransfer > 0 || a.ExpenditureGoodRemainingQty > 0 || a.ExpenditureGoodRetur > 0 || a.EndBalanceExpenditureGood > 0
                       select a;
            var roList = (from a in data
                          select a.Ro).Distinct().ToList();
            var roBalance = from a in garmentBalanceProductionStockRepository.Query
                            select new CostCalViewModel { comodityName = a.Comodity, buyerCode = a.BuyerCode, hours = a.Hours, qtyOrder = a.QtyOrder, ro = a.Ro };

            CostCalculationGarmentDataProductionReport costCalculation = await GetDataCostCal(roList, request.token);

            foreach (var item in roBalance)
            {
                costCalculation.data.Add(item);
            }

            foreach (var garment in data)
            {
                garment.BuyerCode = garment.BuyerCode == null ? (from cost in costCalculation.data where cost.ro == garment.Ro select cost.buyerCode).FirstOrDefault() : garment.BuyerCode;
                garment.Comodity = garment.Comodity == null ? (from cost in costCalculation.data where cost.ro == garment.Ro select cost.comodityName).FirstOrDefault() : garment.Comodity;
                garment.QtyOrder = (from cost in costCalculation.data where cost.ro == garment.Ro select cost.qtyOrder).FirstOrDefault();
                garment.Hours = garment.Hours == 0 ? (from cost in costCalculation.data where cost.ro == garment.Ro select cost.hours).FirstOrDefault() : garment.Hours;

                garment.BasicPrice = Math.Round(Convert.ToDouble((from aa in sumbasicPrice where aa.RO == garment.Ro select aa.BasicPrice / aa.Count).FirstOrDefault()), 2) * Convert.ToDouble((from cost in sumFCs where cost.RO == garment.Ro select cost.FC / cost.Count).FirstOrDefault()) == 0 ? Convert.ToDouble((from a in queryBalance.ToList() where a.Ro == garment.Ro select a.BasicPrice).FirstOrDefault()) : Math.Round(Convert.ToDouble((from aa in sumbasicPrice where aa.RO == garment.Ro select aa.BasicPrice / aa.Count).FirstOrDefault()), 2) * Convert.ToDouble((from cost in sumFCs where cost.RO == garment.Ro select cost.FC / cost.Count).FirstOrDefault());
            }
            garmentMonitoringProductionFlow.garmentMonitorings = data.ToList();
            garmentMonitoringProductionFlow.count = data.Count();
            double BeginingBalanceCuttingQtyTotal =0, BeginingBalanceCuttingPriceTotal=0, QtyCuttingInTotal =0, PriceCuttingInTotal=0, QtyCuttingOutTotal=0, PriceCuttingOutTotal=0, QtyCuttingTransferTotal=0 , PriceCuttingTransferTotal=0, QtyCuttingsubkonTotal= 0 , PriceCuttingsubkonTotal=0,AvalCuttingTotal=0, PriceAvalCuttingTotal =0, AvalSewingTotal =0 , PriceAvalSewingTotal=0, EndBalanceCuttingeQtyTotal =0,
				EndBalanceCuttingePriceTotal =0, BeginingBalanceLoadingQtyTotal =0, BeginingBalanceLoadingPriceTotal =0,QtyLoadingInTotal=0, QtyLoadingInTransferTotal =0, PriceLoadingInTransferTotal =0, PriceLoadingInTotal=0, QtyLoadingTotal=0, PriceLoadingTotal=0, QtyLoadingAdjsTotal =0 , PriceLoadingAdjsTotal=0,EndBalanceLoadingQtyTotal =0, EndBalanceLoadingPriceTotal =0, QtySewingInTotal=0, PriceSewingInTotal=0, QtySewingOutTotal=0, PriceSewingOutTotal=0,	QtySewingInTransferTotal=0, PriceSewingInTransferTotal=0, WipSewingOutTotal = 0, PriceWipSewingOutTotal =0, WipFinishingOutTotal=0, PriceWipFinishingOutTotal=0,
					QtySewingReturTotal =0, PriceSewingReturTotal =0, QtySewingAdjTotal=0, PriceSewingAdjTotal=0, EndBalanceSewingQtyTotal=0, EndBalanceSewingPriceTotal =0, 
					BeginingBalanceFinishingQtyTotal=0, BeginingBalanceFinishingPriceTotal=0, FinishingInQtyTotal=0, FinishingInPriceTotal=0, BeginingBalanceSubconQtyTotal=0, 
					BeginingBalanceSubconPriceTotal=0, SubconInQtyTotal =0, SubconInPriceTotal =0, SubconOutQtyTotal=0, SubconOutPriceTotal=0,
					EndBalanceSubconQtyTotal=0, EndBalanceSubconPriceTotal=0, FinishingOutQtyTotal=0, FinishingOutPriceTotal=0, FinishingInTransferQtyTotal=0, FinishingInTransferPriceTotal=0,
					FinishingAdjQtyTotal=0, FinishingAdjPriceTotal=0,  EndBalanceFinishingQtyTotal=0, EndBalanceFinishingPriceTotal =0, BeginingBalanceExpenditureGoodTotal=0, PriceBeginingBalanceExpenditureGoodTotal=0, FinishingInExpenditureTotal =0, 
					PriceFinishingInExpenditureTotal=0, ExpenditureGoodInTransferTotal=0, PriceExpenditureGoodInTransferTotal =0, ExpenditureGoodReturTotal=0, PriceExpenditureGoodReturTotal =0,
					ExportQtyTotal=0, ExportPriceTotal=0, OtherQtyTotal=0, OtherPriceTotal=0, SampleQtyTotal =0, SamplePriceTotal =0, ExpenditureGoodAdjTotal=0, PriceExpenditureGoodAdjTotal =0, EndBalanceExpenditureGoodTotal=0, PriceEndBalanceExpenditureGoodTotal =0,
					CuttingNew =0, LoadingNew =0, SewingNew=0, FinishingNew=0 , ExpenditureNew =0,subconNew = 0, BeginingBalanceSewingQtyTotal =0, BeginingBalanceSewingPriceTotal=0, FinishingReturQtyTotal=0, PriceFinishingReturTotal =0;
			
			 
			foreach (var item in data)
			{
				QtySewingReturTotal += item.QtySewingRetur;
				PriceSewingReturTotal += item.PriceSewingRetur;
				QtySewingAdjTotal += item.QtySewingAdj;
				PriceSewingAdjTotal += item.PriceSewingAdj;
				BeginingBalanceSewingQtyTotal += item.BeginingBalanceSewingQty;
				BeginingBalanceSewingPriceTotal += item.BeginingBalanceSewingPrice;
				EndBalanceSewingQtyTotal += item.EndBalanceSewingQty;
				EndBalanceSewingPriceTotal += item.EndBalanceSewingPrice;
				BeginingBalanceFinishingQtyTotal += item.BeginingBalanceFinishingQty;
				BeginingBalanceFinishingPriceTotal += item.BeginingBalanceFinishingPrice;
				FinishingInQtyTotal += item.FinishingInQty;
				FinishingInPriceTotal += item.FinishingInPrice;
				BeginingBalanceSubconQtyTotal += item.BeginingBalanceSubconQty;
				BeginingBalanceSubconPriceTotal += item.BeginingBalanceSubconPrice;
				SubconInQtyTotal += item.SubconInQty;
				SubconInPriceTotal += item.SubconInPrice;
				SubconOutQtyTotal += item.SubconOutQty;
				SubconOutPriceTotal += item.SubconOutPrice;
				EndBalanceSubconQtyTotal += item.EndBalanceSubconQty;
				EndBalanceSubconPriceTotal += item.EndBalanceSubconPrice;
				FinishingOutQtyTotal += item.FinishingOutQty;
				FinishingOutPriceTotal += item.FinishingOutPrice;
				FinishingInTransferQtyTotal += item.FinishingInTransferQty;
				FinishingInTransferPriceTotal += item.FinishingInTransferPrice;
				FinishingAdjQtyTotal += item.FinishingAdjQty;
				FinishingAdjPriceTotal += item.FinishingAdjPRice;
				EndBalanceFinishingQtyTotal += item.EndBalanceFinishingQty;
				EndBalanceFinishingPriceTotal += item.EndBalanceFinishingPrice;
				BeginingBalanceExpenditureGoodTotal += item.BeginingBalanceExpenditureGood;
				PriceBeginingBalanceExpenditureGoodTotal += item.BeginingBalanceExpenditureGoodPrice;
				FinishingInExpenditureTotal += item.FinishingInExpenditure;
				PriceFinishingInExpenditureTotal += item.FinishingInExpenditurepPrice;
				ExpenditureGoodInTransferTotal += item.ExpenditureGoodInTransfer;
				PriceExpenditureGoodInTransferTotal += item.ExpenditureGoodInTransferPrice;
				ExpenditureGoodReturTotal += item.ExpenditureGoodRetur;
				PriceExpenditureGoodReturTotal += item.ExpenditureGoodReturPrice;
				ExportQtyTotal += item.ExportQty;
				ExportPriceTotal += item.ExportPrice;
				OtherQtyTotal += item.OtherQty;
				OtherPriceTotal += item.OtherPrice;
				SampleQtyTotal += item.SampleQty;
				SamplePriceTotal += item.SamplePrice;
				ExpenditureGoodAdjTotal += item.ExpenditureGoodAdj;
				PriceExpenditureGoodAdjTotal += item.ExpenditureGoodAdjPrice;
				EndBalanceExpenditureGoodTotal += item.EndBalanceExpenditureGood;
				PriceEndBalanceExpenditureGoodTotal += item.EndBalanceExpenditureGoodPrice;
				CuttingNew += Convert.ToDouble(item.CuttingNew);
				LoadingNew += Convert.ToDouble(item.LoadingNew);
				SewingNew += Convert.ToDouble(item.SewingNew) ;
				FinishingNew += Convert.ToDouble(item.FinishingNew);
				ExpenditureNew += Convert.ToDouble(item.ExpenditureNew);
				subconNew += Convert.ToDouble(item.SubconNew);
				BeginingBalanceCuttingQtyTotal += item.BeginingBalanceCuttingQty;
				BeginingBalanceCuttingPriceTotal += item.BeginingBalanceCuttingPrice;
				QtyCuttingInTotal += item.QtyCuttingIn;
				PriceCuttingInTotal += item.PriceCuttingIn;
				QtyCuttingOutTotal += item.QtyCuttingOut;
				PriceCuttingOutTotal += item.PriceCuttingOut;
				QtyCuttingsubkonTotal += item.QtyCuttingsubkon;
				PriceCuttingsubkonTotal += item.PriceCuttingsubkon;
				QtyCuttingTransferTotal += item.QtyCuttingTransfer;
				PriceCuttingTransferTotal += item.PriceCuttingTransfer;
				AvalCuttingTotal += item.AvalCutting;
				PriceAvalCuttingTotal += item.AvalCuttingPrice;
				AvalSewingTotal += item.AvalSewing;
				PriceAvalSewingTotal += item.AvalSewingPrice;
				EndBalanceCuttingeQtyTotal += item.EndBalancCuttingeQty;
				EndBalanceCuttingePriceTotal += item.EndBalancCuttingePrice;
				BeginingBalanceLoadingQtyTotal += item.BeginingBalanceLoadingQty;
				BeginingBalanceLoadingPriceTotal += item.BeginingBalanceLoadingPrice;
				QtyLoadingInTotal += item.QtyLoadingIn;
				PriceLoadingInTotal += item.PriceLoadingIn;
				QtyLoadingInTransferTotal += item.QtyLoadingInTransfer;
				PriceLoadingInTransferTotal += item.PriceLoadingInTransfer;
				QtyLoadingTotal += item.QtyLoading;
				PriceLoadingTotal += item.PriceLoading;
				QtyLoadingAdjsTotal += item.QtyLoadingAdjs;
				PriceLoadingAdjsTotal += item.PriceLoadingAdjs;
				EndBalanceLoadingQtyTotal += item.EndBalanceLoadingQty;
				EndBalanceLoadingPriceTotal += item.EndBalanceLoadingPrice;
				QtySewingInTotal += item.QtySewingIn;
				PriceSewingInTotal += item.PriceSewingIn;
				QtySewingOutTotal += item.QtySewingOut;
				PriceSewingOutTotal += item.PriceSewingOut;
				QtySewingInTransferTotal += item.QtySewingInTransfer;
				PriceSewingInTransferTotal += item.PriceSewingInTransfer;
				WipSewingOutTotal += item.WipSewingOut;
				PriceWipSewingOutTotal += item.WipSewingOutPrice;
				WipFinishingOutTotal += item.WipFinishingOut;
				WipFinishingOutTotal += item.WipFinishingOutPrice;
				PriceWipFinishingOutTotal += item.WipFinishingOutPrice;
				FinishingReturQtyTotal += item.FinishingReturQty;
				PriceFinishingReturTotal += item.FinishingReturPrice;
			

			}
			monitoringDtos = data.ToList();
			GarmentMonitoringProductionStockFlowDto total = new GarmentMonitoringProductionStockFlowDto()
			{
				
				BeginingBalanceCuttingQty = BeginingBalanceCuttingQtyTotal,
				BeginingBalanceCuttingPrice = BeginingBalanceCuttingPriceTotal,
				QtyCuttingTransfer = QtyCuttingTransferTotal,
				PriceCuttingTransfer = PriceCuttingTransferTotal,
				QtyCuttingsubkon = QtyCuttingsubkonTotal,
				PriceCuttingsubkon = PriceCuttingsubkonTotal,
				QtyCuttingIn = QtyCuttingInTotal,
				PriceCuttingIn = PriceCuttingInTotal,
				QtyCuttingOut = QtyCuttingOutTotal,
				PriceCuttingOut = PriceCuttingOutTotal,
				AvalCutting = AvalCuttingTotal,
				AvalCuttingPrice = PriceAvalCuttingTotal,
				AvalSewing = AvalSewingTotal,
				AvalSewingPrice = PriceAvalSewingTotal,
				EndBalancCuttingeQty = EndBalanceCuttingeQtyTotal,
				EndBalancCuttingePrice = EndBalanceCuttingePriceTotal,
				BeginingBalanceLoadingQty = BeginingBalanceLoadingQtyTotal,
				BeginingBalanceLoadingPrice = BeginingBalanceLoadingPriceTotal,
				QtyLoadingIn = QtyLoadingInTotal,
				PriceLoadingIn = PriceLoadingInTotal,
				QtyLoadingInTransfer = QtyLoadingInTransferTotal,
				PriceLoadingInTransfer = PriceLoadingInTransferTotal,
				QtyLoading = QtyLoadingTotal,
				PriceLoading = PriceLoadingTotal,
				QtyLoadingAdjs = QtyLoadingAdjsTotal,
				PriceLoadingAdjs = PriceLoadingAdjsTotal,
				EndBalanceLoadingQty = EndBalanceLoadingQtyTotal,
				EndBalanceLoadingPrice = EndBalanceLoadingPriceTotal,
				BeginingBalanceSewingQty = BeginingBalanceSewingQtyTotal,
				BeginingBalanceSewingPrice = BeginingBalanceSewingPriceTotal,
				QtySewingIn = QtySewingInTotal,
				PriceSewingIn = PriceSewingInTotal,
				QtySewingOut = QtySewingOutTotal,
				PriceSewingOut = PriceSewingOutTotal,
				QtySewingInTransfer = QtySewingInTransferTotal,
				PriceSewingInTransfer = PriceSewingInTransferTotal,
				QtySewingRetur = QtySewingReturTotal,
				PriceSewingRetur = PriceSewingReturTotal,
				WipSewingOut =WipSewingOutTotal ,
				WipSewingOutPrice = PriceWipSewingOutTotal,
				WipFinishingOut = WipFinishingOutTotal,
				WipFinishingOutPrice = PriceWipFinishingOutTotal,
				QtySewingAdj = QtySewingAdjTotal,
				PriceSewingAdj = PriceSewingAdjTotal,
				EndBalanceSewingQty = EndBalanceSewingQtyTotal,
				EndBalanceSewingPrice = EndBalanceSewingPriceTotal,
				BeginingBalanceFinishingQty = BeginingBalanceFinishingQtyTotal,
				BeginingBalanceFinishingPrice = BeginingBalanceFinishingPriceTotal,
				FinishingInExpenditure = FinishingInExpenditureTotal,
				FinishingInExpenditurepPrice = PriceFinishingInExpenditureTotal,
				FinishingInQty = FinishingInQtyTotal,
				FinishingInPrice = FinishingInPriceTotal,
				FinishingOutQty = FinishingOutQtyTotal,
				FinishingOutPrice = FinishingOutPriceTotal,
				BeginingBalanceSubconQty = BeginingBalanceSubconQtyTotal,
				BeginingBalanceSubconPrice = BeginingBalanceSubconPriceTotal,
				SubconInQty = SubconInQtyTotal,
				SubconInPrice = SubconInPriceTotal,
				SubconOutQty = SubconOutQtyTotal,
				SubconOutPrice = SubconOutPriceTotal,
				EndBalanceSubconQty = EndBalanceSubconQtyTotal,
				EndBalanceSubconPrice = EndBalanceSubconPriceTotal,
				FinishingInTransferQty = FinishingInTransferQtyTotal,
				FinishingInTransferPrice = FinishingInTransferPriceTotal,
				FinishingReturQty = FinishingReturQtyTotal,
				FinishingReturPrice = PriceFinishingReturTotal,
				FinishingAdjQty = FinishingAdjQtyTotal,
				FinishingAdjPRice = FinishingAdjPriceTotal,
				BeginingBalanceExpenditureGood = BeginingBalanceExpenditureGoodTotal,
				BeginingBalanceExpenditureGoodPrice = PriceBeginingBalanceExpenditureGoodTotal,
				EndBalanceFinishingQty = EndBalanceFinishingQtyTotal,
				EndBalanceFinishingPrice = EndBalanceFinishingPriceTotal,
				ExportQty = ExportQtyTotal,
				ExportPrice = ExportPriceTotal,
				SampleQty = SampleQtyTotal,
				SamplePrice = SamplePriceTotal,
				OtherQty = OtherQtyTotal,
				OtherPrice = OtherPriceTotal,
				ExpenditureGoodAdj = ExpenditureGoodAdjTotal,
				ExpenditureGoodAdjPrice = PriceExpenditureGoodAdjTotal,
				ExpenditureGoodRetur = ExpenditureGoodReturTotal,
				ExpenditureGoodReturPrice = PriceExpenditureGoodReturTotal,
				ExpenditureGoodInTransfer = ExpenditureGoodInTransferTotal,
				ExpenditureGoodInTransferPrice = PriceExpenditureGoodInTransferTotal,
				EndBalanceExpenditureGood = EndBalanceExpenditureGoodTotal,
				EndBalanceExpenditureGoodPrice = PriceEndBalanceExpenditureGoodTotal,
				 
				CuttingNew = Convert.ToDecimal(CuttingNew),
				LoadingNew = Convert.ToDecimal(LoadingNew),
				SewingNew = Convert.ToDecimal(SewingNew),
				FinishingNew = Convert.ToDecimal(FinishingNew),
				ExpenditureNew = Convert.ToDecimal(ExpenditureNew),
				SubconNew = Convert.ToDecimal(subconNew)
			};
			monitoringDtos.Add(total);
		
		garmentMonitoringProductionFlow.garmentMonitorings = monitoringDtos;
			var reportDataTable = new DataTable();
			
			if (request.type != "bookkeeping")
			{
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "RO", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No Article", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Komoditi", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Jumlah Order", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING3", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING4", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING5", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING6", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING7", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING8", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING3", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING4", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING5", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING6", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING3", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING4", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING5", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING6", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING7", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING8", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING9", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING3", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING4", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING5", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING6", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING7", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING8", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING9", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING10", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING11", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI3", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI4", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI5", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI6", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI7", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI8", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI9", DataType = typeof(string) });

				reportDataTable.Rows.Add("", "", "", "",
				"Saldo Awal WIP Cutting", "Cutting In (WIP Cutting)", "Cutting Out / HP(WIP Loading)", "Cutting Out Transfer", "Cutting Out Subkon", "Aval Komponen dari Cutting", "Aval Komponen dari Sewing", "Saldo Akhir WIP Cutting",
				"Saldo Awal Loading", "Loading In", "Loading In Transfer", "Loading Out (WIP Sewing)	", "Adjs Loading", "Saldo Akhir Loading",
				"Saldo Awal WIP Sewing", "Sewing In (WIP Sewing)", "Sewing Out (WIP Finishing)", "Sewing In Transfer", "Sewing Out Tranfer WIP Sewing", "Sewing Out Transfer WIP Finishing", "Retur ke Cutting", "Adjs Sewing", "Saldo Akhir WIP Sewing",
				"Saldo Awal WIP Finishing", "Finishing In (WIP Finishing)", "Saldo Awal WIP Subkon", "Subkon In", "Subkon Out", "Saldo Akhir WIP Subkon", "Finishing Out (WIP BJ)", "Finishing In Transfer", "Adjs Finishing", "Retur ke Sewing", "Saldo Akhir WIP Finishing",
				"Saldo Awal Barang jadi", "Barang Jadi In/ (WIP BJ)", "Barang Jadi In Transfer", "Penerimaan Lain-lain (Retur)", "Pengiriman Export", "Pengiriman Gudang Sisa", "Pengiriman Lain-lain/Sample", "Adjust Barang Jadi", "Saldo Akhir Barang Jadi"
				);
			}
			else
			{

				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "RO", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No Article", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Buyer", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Komoditi", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Jumlah Order", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FC", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "HOURS", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "TARIF", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "HARGA BAHAN BAKU", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING3", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING4", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING5", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING6", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING7", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING8", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING9", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING10", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING11", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING12", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING513", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING614", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING15", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING16", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING3", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING4", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING5", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING6", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING7", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING8", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING9", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING10", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING11", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING12", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING3", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING4", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING5", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING6", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING7", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING8", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING9", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING10", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING11", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING12", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING13", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING14", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING15", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING16", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING17", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING18", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING3", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING4", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING5", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING6", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING7", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING8", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING9", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING10", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING11", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING12", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING13", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING14", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING15", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING16", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING17", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING18", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING19", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING20", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING21", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING22", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI3", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI4", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI5", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI6", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI7", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI8", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI9", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI10", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI11", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI12", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI13", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI14", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI15", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI16", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI17", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI18", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI19", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI20", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Nilai Baru Komersil(SaldoAkhir)", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Nilai Baru Komersil(SaldoAkhir)2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Nilai Baru Komersil(SaldoAkhir)3", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Nilai Baru Komersil(SaldoAkhir)4", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Nilai Baru Komersil(SaldoAkhir)5", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Nilai Baru Komersil(SaldoAkhir)6", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Nilai Baru Komersil(SaldoAkhir)7", DataType = typeof(string) });

				reportDataTable.Rows.Add("", "", "", "", "", "", "", "", "",
									"Saldo Awal WIP Cutting","", "Cutting In (WIP Cutting)","", "Cutting Out / HP(WIP Loading)","", "Cutting Out Transfer","", "Cutting Out Subkon","", "Aval Komponen dari Cutting","", "Aval Komponen dari Sewing","", "Saldo Akhir WIP Cutting","",
									"Saldo Awal Loading","", "Loading In","", "Loading In Transfer", "", "Loading Out (WIP Sewing)	","", "Adjs Loading","", "Saldo Akhir Loading","",
									"Saldo Awal WIP Sewing","", "Sewing In (WIP Sewing)","", "Sewing Out (WIP Finishing)","", "Sewing In Transfer","", "Sewing Out Tranfer WIP Sewing","", "Sewing Out Transfer WIP Finishing","", "Retur ke Cutting","", "Adjs Sewing","", "Saldo Akhir WIP Sewing","",
									"Saldo Awal WIP Finishing","", "Finishing In (WIP Finishing)","", "Saldo Awal WIP Subkon","", "Subkon In","", "Subkon Out","", "Saldo Akhir WIP Subkon","", "Finishing Out (WIP BJ)","", "Finishing In Transfer","", "Adjs Finishing","", "Retur ke Sewing","", "Saldo Akhir WIP Finishing","",
									"Saldo Awal Barang jadi","", "Barang Jadi In/ (WIP BJ)","", "Finishing Transfer","", "Penerimaan Lain-lain (Retur)","", "Standar Konversi Biaya","", "Pengiriman Export","", "Pengiriman Gudang Sisa","", "Pengiriman Lain-lain/Sample","", "Adjust Barang Jadi","", "Saldo Akhir Barang Jadi","",
									"Tarif","Cutting","Loading","Sewing","Finishing", "Subkon","Barang Jadi"
									);
				reportDataTable.Rows.Add("", "", "", "", "", "", "", "", "",
					"Qty","Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga",
					"Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga",
					"Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga",
					"Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga",
					"Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Tarif", "Pemakaian Bahan Baku", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty","Harga",
					"","","","","","",""
					);
				
			}
			int counter = 6;

			foreach (var report in garmentMonitoringProductionFlow.garmentMonitorings)
			{
				if (request.type != "bookkeeping")
				{
					reportDataTable.Rows.Add(report.Ro, report.Article, report.Comodity, report.QtyOrder,
					report.BeginingBalanceCuttingQty, report.QtyCuttingIn, report.QtyCuttingOut, report.QtyCuttingTransfer, report.QtyCuttingsubkon, report.AvalCutting, report.AvalSewing, report.EndBalancCuttingeQty,
					report.BeginingBalanceLoadingQty, report.QtyLoadingIn,report.QtyLoadingInTransfer, report.QtyLoading, report.QtyLoadingAdjs, report.EndBalanceLoadingQty,
					report.BeginingBalanceSewingQty, report.QtySewingIn, report.QtySewingOut, report.QtySewingInTransfer, report.WipSewingOut, report.WipFinishingOut, report.QtySewingRetur, report.QtySewingAdj, report.EndBalanceSewingQty,
					report.BeginingBalanceFinishingQty, report.FinishingInQty, report.BeginingBalanceSubconQty, report.SubconInQty, report.SubconOutQty, report.EndBalanceSubconQty, report.FinishingOutQty, report.FinishingInTransferQty, report.FinishingAdjQty, report.FinishingReturQty, report.EndBalanceFinishingQty,
					report.BeginingBalanceExpenditureGood, report.FinishingInExpenditure, report.FinishingInTransferQty, report.ExpenditureGoodRetur, report.ExportQty, report.OtherQty, report.SampleQty, report.ExpenditureGoodAdj, report.EndBalanceExpenditureGood);
					counter++;
				}
				else
				{
					reportDataTable.Rows.Add(report.Ro, report.Article, report.BuyerCode, report.Comodity, report.QtyOrder, report.FC, report.Hours, report.Fare, report.BasicPrice,
						report.BeginingBalanceCuttingQty, report.BeginingBalanceCuttingPrice, report.QtyCuttingIn,report.PriceCuttingIn, report.QtyCuttingOut,report.PriceCuttingOut, report.QtyCuttingTransfer,report.PriceCuttingTransfer, report.QtyCuttingsubkon,report.PriceCuttingsubkon, report.AvalCutting,report.AvalCuttingPrice, report.AvalSewing,report.AvalSewingPrice, report.EndBalancCuttingeQty,report.EndBalancCuttingePrice,
						report.BeginingBalanceLoadingQty, report.BeginingBalanceLoadingPrice, report.QtyLoadingIn, report.PriceLoadingIn,report.QtyLoadingInTransfer,report.PriceLoadingInTransfer, report.QtyLoading, report.PriceLoading, report.QtyLoadingAdjs, report.PriceLoadingAdjs, report.EndBalanceLoadingQty, report.EndBalanceLoadingPrice,
						report.BeginingBalanceSewingQty, report.BeginingBalanceSewingPrice, report.QtySewingIn,report.PriceSewingIn, report.QtySewingOut,report.PriceSewingOut, report.QtySewingInTransfer,report.PriceSewingInTransfer, report.WipSewingOut,report.WipSewingOutPrice, report.WipFinishingOut,report.WipFinishingOutPrice, report.QtySewingRetur,report.PriceSewingRetur, report.QtySewingAdj,report.PriceSewingAdj, report.EndBalanceSewingQty,report.EndBalanceSewingPrice,
						report.BeginingBalanceFinishingQty,report.BeginingBalanceFinishingPrice, report.FinishingInQty,report.FinishingInPrice, report.BeginingBalanceSubconQty,report.BeginingBalanceSubconPrice, report.SubconInQty,report.SubconInPrice, report.SubconOutQty,report.SubconOutPrice, report.EndBalanceSubconQty,report.EndBalanceSubconPrice, report.FinishingOutQty,report.FinishingOutPrice, report.FinishingInTransferQty,report.FinishingInTransferPrice, report.FinishingAdjQty,report.FinishingAdjPRice, report.FinishingReturQty,report.FinishingReturPrice, report.EndBalanceFinishingQty,report.EndBalanceFinishingPrice,
						report.BeginingBalanceExpenditureGood,report.BeginingBalanceExpenditureGoodPrice, report.FinishingInExpenditure,report.FinishingInExpenditurepPrice, report.FinishingInTransferQty,report.FinishingInTransferPrice, report.ExpenditureGoodRetur,report.ExpenditureGoodReturPrice,report.FinishingInExpenditure * Convert.ToDouble(report.Fare), report.FinishingInExpenditure * report.BasicPrice, report.ExportQty,report.ExportPrice, report.OtherQty,report.OtherPrice, report.SampleQty,report.SamplePrice, report.ExpenditureGoodAdj,report.ExpenditureGoodAdjPrice, report.EndBalanceExpenditureGood,report.EndBalanceExpenditureGoodPrice,
						report.FareNew, report.CuttingNew, report.LoadingNew, report.SewingNew, report.FinishingNew,report.SubconNew, report.ExpenditureNew
						);
					counter++;
				}
			}
            var _unitName = (from a in garmentFinishingOutRepository.Query
                             where a.UnitId == request.unit
                             select a.UnitName).FirstOrDefault();
            using (var package = new ExcelPackage())
			{
				var worksheet = package.Workbook.Worksheets.Add("Sheet 1");
				
				if (request.type != "bookkeeping")
				{
					worksheet.Cells["A1"].Value = "Report Produksi"; worksheet.Cells["A" + 1 + ":AU" + 1 + ""].Merge = true;
					worksheet.Cells["A2"].Value = "Periode " + dateFrom.ToString("dd-MM-yyyy") + " s/d " + dateTo.ToString("dd-MM-yyyy");
					worksheet.Cells["A3"].Value = "Konfeksi " + _unitName;
					worksheet.Cells["A" + 1 + ":AU" + 1 + ""].Merge = true;
					worksheet.Cells["A" + 2 + ":AU" + 2 + ""].Merge = true;
					worksheet.Cells["A" + 3 + ":AU" + 3 + ""].Merge = true;
					worksheet.Cells["A" + 1 + ":AU" + 3 + ""].Style.Font.Size = 15;
					worksheet.Cells["A" + 1 + ":AU" + 3 + ""].Style.Font.Bold = true;
					worksheet.Cells["A" + 1 + ":AU" + 6 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					worksheet.Cells["A" + 1 + ":AU" + 6 + ""].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					worksheet.Cells["A5"].LoadFromDataTable(reportDataTable, true);
					worksheet.Cells["E" + 5 + ":AU" + 5 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

					worksheet.Cells["E" + 5 + ":L" + 5 + ""].Merge = true;
					worksheet.Cells["M" + 5 + ":R" + 5 + ""].Merge = true;
					worksheet.Cells["S" + 5 + ":AA" + 5 + ""].Merge = true;
					worksheet.Cells["AB" + 5 + ":AL" + 5 + ""].Merge = true;
					worksheet.Cells["AM" + 5 + ":AU" + 5 + ""].Merge = true;
					worksheet.Cells["E" + 5 + ":L" + 5 + ""].Merge = true;
					worksheet.Cells["S" + 5 + ":AA" + 5 + ""].Merge = true;
					worksheet.Cells["AB" + 5 + ":AL" + 5 + ""].Merge = true;
					worksheet.Cells["AM" + 5 + ":AU" + 5 + ""].Merge = true;
					worksheet.Cells["A" + counter + ":D" + counter + ""].Merge = true;
					worksheet.Cells["A" + 5 + ":AU"  +6 + ""].Style.Font.Bold = true;
					worksheet.Cells["E" + 6 + ":AU" + counter + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
					worksheet.Cells["A" + 5 + ":AU" + counter + ""].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					worksheet.Cells["A" + 5 + ":AU" + counter + ""].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					worksheet.Cells["A" + 5 + ":AU" + counter + ""].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					worksheet.Cells["A" + 5 + ":AU" + counter + ""].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					worksheet.Cells["A" + counter + ":AU" + counter + ""].Style.Font.Bold = true;
					
				}
				else
				{
					worksheet.Cells["A1"].Value = "Report Produksi"; worksheet.Cells["A" + 1 + ":AT" + 1 + ""].Merge = true;
					worksheet.Cells["A2"].Value = "Periode " + dateFrom.ToString("dd-MM-yyyy") + " s/d " + dateTo.ToString("dd-MM-yyyy");
					worksheet.Cells["A3"].Value = "Konfeksi " + _unitName;
					worksheet.Cells["A" + 1 + ":CZ" + 1 + ""].Merge = true;
					worksheet.Cells["A" + 2 + ":CZ" + 2 + ""].Merge = true;
					worksheet.Cells["A" + 3 + ":CZ" + 3 + ""].Merge = true;
					worksheet.Cells["A" + 1 + ":CZ" + 3 + ""].Style.Font.Size = 15;
					worksheet.Cells["A" + 1 + ":CZ" + 3 + ""].Style.Font.Bold = true;
					worksheet.Cells["A" + 1 + ":CZ" + 3 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					worksheet.Cells["A" + 1 + ":CZ" + 3 + ""].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					worksheet.Cells["A5"].LoadFromDataTable(reportDataTable, true);
					worksheet.Cells["E" + 5 + ":L" + 5 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					worksheet.Cells["J" + 5 + ":Y" + 5 + ""].Merge = true;
					worksheet.Cells["Z" + 5 + ":AK" + 5 + ""].Merge = true;
					worksheet.Cells["AL" + 5 + ":BC" + 5 + ""].Merge = true;
					worksheet.Cells["BD" + 5 + ":BY" + 5 + ""].Merge = true;
					worksheet.Cells["BZ" + 5 + ":CS" + 5 + ""].Merge = true;
					worksheet.Cells["CT" + 5 + ":CZ" + 5 + ""].Merge = true;
					worksheet.Cells["A" + 5 + ":A" + 7 + ""].Merge = true;
					worksheet.Cells["B" + 5 + ":B" + 7 + ""].Merge = true;
					worksheet.Cells["C" + 5 + ":C" + 7 + ""].Merge = true;
					worksheet.Cells["D" + 5 + ":D" + 7 + ""].Merge = true;
					worksheet.Cells["E" + 5 + ":E" + 7 + ""].Merge = true;
					worksheet.Cells["F" + 5 + ":F" + 7 + ""].Merge = true;
					worksheet.Cells["G" + 5 + ":G" + 7 + ""].Merge = true;
					worksheet.Cells["H" + 5 + ":H" + 7 + ""].Merge = true;
					worksheet.Cells["I" + 5 + ":I" + 7 + ""].Merge = true;
					worksheet.Cells["J" + 6 + ":K" + 6 + ""].Merge = true;
					worksheet.Cells["L" + 6 + ":M" + 6 + ""].Merge = true;
					worksheet.Cells["N" + 6 + ":O" + 6 + ""].Merge = true;
					worksheet.Cells["P" + 6 + ":Q" + 6 + ""].Merge = true;
					worksheet.Cells["R" + 6 + ":S" + 6 + ""].Merge = true;
					worksheet.Cells["T" + 6 + ":U" + 6 + ""].Merge = true;
					worksheet.Cells["V" + 6 + ":W" + 6 + ""].Merge = true;
					worksheet.Cells["X" + 6 + ":Y" + 6 + ""].Merge = true;
					worksheet.Cells["Z" + 6 + ":AA" + 6 + ""].Merge = true;
					worksheet.Cells["AB" + 6 + ":AC" + 6 + ""].Merge = true;
					worksheet.Cells["AD" + 6 + ":AE" + 6 + ""].Merge = true;
					worksheet.Cells["AF" + 6 + ":AG" + 6 + ""].Merge = true;
					worksheet.Cells["AH" + 6 + ":AI" + 6 + ""].Merge = true;
					worksheet.Cells["AJ" + 6 + ":AK" + 6 + ""].Merge = true;
					worksheet.Cells["AL" + 6 + ":AM" + 6 + ""].Merge = true;
					worksheet.Cells["AN" + 6 + ":AO" + 6 + ""].Merge = true;
					worksheet.Cells["AP" + 6 + ":AQ" + 6 + ""].Merge = true;
					worksheet.Cells["AR" + 6 + ":AS" + 6 + ""].Merge = true;
					worksheet.Cells["AT" + 6 + ":AU" + 6 + ""].Merge = true;
					worksheet.Cells["AV" + 6 + ":AW" + 6 + ""].Merge = true;
					worksheet.Cells["AX" + 6 + ":AY" + 6 + ""].Merge = true;
					worksheet.Cells["Az" + 6 + ":BA" + 6 + ""].Merge = true;
					worksheet.Cells["BB" + 6 + ":BC" + 6 + ""].Merge = true;
					worksheet.Cells["BD" + 6 + ":BE" + 6 + ""].Merge = true;
					worksheet.Cells["BF" + 6 + ":BG" + 6 + ""].Merge = true;
					worksheet.Cells["BH" + 6 + ":BI" + 6 + ""].Merge = true;
					worksheet.Cells["BJ" + 6 + ":BK" + 6 + ""].Merge = true;
					worksheet.Cells["BL" + 6 + ":BM" + 6 + ""].Merge = true;
					worksheet.Cells["BN" + 6 + ":BO" + 6 + ""].Merge = true;
					worksheet.Cells["BP" + 6 + ":BQ" + 6 + ""].Merge = true;
					worksheet.Cells["BR" + 6 + ":BS" + 6 + ""].Merge = true;
					worksheet.Cells["BT" + 6 + ":BU" + 6 + ""].Merge = true;
					worksheet.Cells["BV" + 6 + ":BW" + 6 + ""].Merge = true;
					worksheet.Cells["BX" + 6 + ":BY" + 6 + ""].Merge = true;
					worksheet.Cells["BZ" + 6 + ":CA" + 6 + ""].Merge = true;
					worksheet.Cells["CB" + 6 + ":CC" + 6 + ""].Merge = true;
					worksheet.Cells["CD" + 6 + ":CE" + 6 + ""].Merge = true;
					worksheet.Cells["CF" + 6 + ":CG" + 6 + ""].Merge = true;
					worksheet.Cells["CH" + 6 + ":CI" + 6 + ""].Merge = true;
					worksheet.Cells["CJ" + 6 + ":CK" + 6 + ""].Merge = true;
					worksheet.Cells["CL" + 6 + ":CM" + 6 + ""].Merge = true;
					worksheet.Cells["CN" + 6 + ":CO" + 6 + ""].Merge = true;
					worksheet.Cells["CP" + 6 + ":CQ" + 6 + ""].Merge = true;
					worksheet.Cells["CR" + 6 + ":CS" + 6 + ""].Merge = true;
					worksheet.Cells["A" + (counter + 1) + ":i" + (counter + 1) + ""].Merge = true;
					worksheet.Cells["A" + 5 + ":CZ" + 7 + ""].Style.Font.Bold = true;
					worksheet.Cells["A" + (counter + 1) + ":CZ" + (counter + 1) + ""].Style.Font.Bold = true;
					worksheet.Cells["A" + 5 + ":CZ" + 7 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					worksheet.Cells["A" + 5 + ":CZ" + 6 + ""].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					worksheet.Cells["E" + 8 + ":CZ" + (counter + 1) + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
					
					worksheet.Cells["A" + 5 + ":CZ" + (counter + 1) + ""].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					worksheet.Cells["A" + 5 + ":CZ" + (counter + 1) + ""].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					worksheet.Cells["A" + 5 + ":CZ" + (counter + 1) + ""].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					worksheet.Cells["A" + 5 + ":CZ" + (counter + 1) + ""].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                  

                }
				var stream = new MemoryStream();

				package.SaveAs(stream);

				return stream;
			}
		}
	}
}