﻿using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentAdjustments.Repositories;
using Manufactures.Domain.GarmentExpenditureGoods.Repositories;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Manufactures.Domain.MonitoringProductionStockFlow;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Manufactures.Domain.GarmentExpenditureGoodReturns.Repositories;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.GarmentComodityPrices.Repositories;

namespace Manufactures.Application.GarmentExpenditureGoods.Queries.GetMutationExpenditureGoods
{
    public class GarmentMutationExpenditureGoodQueryHandler : IQueryHandler<GetMutationExpenditureGoodsQuery, GarmentMutationExpenditureGoodListViewModel>
    {
        private readonly IStorage _storage;
        private readonly IGarmentBalanceMonitoringProductionStockFlowRepository garmentBalanceMonitoringProductionStockFlowRepository;
        private readonly IGarmentAdjustmentRepository garmentAdjustmentRepository;
        private readonly IGarmentAdjustmentItemRepository garmentAdjustmentItemRepository;
        private readonly IGarmentExpenditureGoodRepository garmentExpenditureGoodRepository;
        private readonly IGarmentExpenditureGoodItemRepository garmentExpenditureGoodItemRepository;
        private readonly IGarmentExpenditureGoodReturnRepository garmentExpenditureGoodReturnRepository;
        private readonly IGarmentExpenditureGoodReturnItemRepository garmentExpenditureGoodReturnItemRepository;
        private readonly IGarmentFinishingOutRepository garmentFinishingOutRepository;
        private readonly IGarmentFinishingOutItemRepository garmentFinishingOutItemRepository;
        private readonly IGarmentCuttingOutRepository garmentCuttingOutRepository ;

        public GarmentMutationExpenditureGoodQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage = storage;
            garmentBalanceMonitoringProductionStockFlowRepository = storage.GetRepository<IGarmentBalanceMonitoringProductionStockFlowRepository>();
            garmentAdjustmentRepository = storage.GetRepository<IGarmentAdjustmentRepository>();
            garmentAdjustmentItemRepository = storage.GetRepository<IGarmentAdjustmentItemRepository>();
            garmentExpenditureGoodRepository = storage.GetRepository<IGarmentExpenditureGoodRepository>();
            garmentExpenditureGoodItemRepository = storage.GetRepository<IGarmentExpenditureGoodItemRepository>();
            garmentFinishingOutRepository = storage.GetRepository<IGarmentFinishingOutRepository>();
            garmentFinishingOutItemRepository = storage.GetRepository<IGarmentFinishingOutItemRepository>();
            garmentCuttingOutRepository = storage.GetRepository<IGarmentCuttingOutRepository>();
            garmentExpenditureGoodReturnRepository = storage.GetRepository<IGarmentExpenditureGoodReturnRepository>();
            garmentExpenditureGoodReturnItemRepository = storage.GetRepository<IGarmentExpenditureGoodReturnItemRepository>();
        }

        class mutationView
        {
            public double SaldoQtyFin { get; internal set; }
            public double QtyFin { get; internal set; }
            public double AdjFin { get; internal set; }
            public double Retur { get; internal set; }
            public double QtyExpend { get; internal set; }
            //public string Comodity { get; internal set; }
            public string ComodityCode { get; internal set; }
        }

        public async Task<GarmentMutationExpenditureGoodListViewModel> Handle(GetMutationExpenditureGoodsQuery request, CancellationToken cancellationToken)
        {
            GarmentMutationExpenditureGoodListViewModel expenditureGoodListViewModel = new GarmentMutationExpenditureGoodListViewModel();
            List<GarmentMutationExpenditureGoodDto> mutationExpenditureGoodDto = new List<GarmentMutationExpenditureGoodDto>();

            DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom, new TimeSpan(7, 0, 0));
            DateTimeOffset dateTo = new DateTimeOffset(request.dateTo, new TimeSpan(7, 0, 0));
            DateTimeOffset dateBalance = (from a in garmentBalanceMonitoringProductionStockFlowRepository.Query
                                          select a.CreatedDate).FirstOrDefault();

            var querybalance = from a in (from aa in garmentBalanceMonitoringProductionStockFlowRepository.Query
                                          where aa.CreatedDate < dateFrom
                                          select new { aa.BeginingBalanceExpenditureGood, aa.Ro, aa.Comodity })
                               join b in garmentCuttingOutRepository.Query on a.Ro equals b.RONo
                               //&& a.Comodity == "BOYS SHORTS"
                               select new mutationView
                               {
                                   SaldoQtyFin = a.BeginingBalanceExpenditureGood,
                                   AdjFin = 0,
                                   //Comodity = a.Comodity,
                                   ComodityCode = b.ComodityCode,
                                   QtyExpend = 0,
                                   QtyFin = 0,
                                   Retur = 0,
                               };

            var adjust = from a in (from aa in garmentAdjustmentRepository.Query
                                           where aa.AdjustmentDate >= dateBalance && aa.AdjustmentDate <= dateTo
                                           && aa.AdjustmentType == "FINISHING"
                                           select aa)
                                join b in garmentAdjustmentItemRepository.Query on a.Identity equals b.AdjustmentId
                                select new mutationView
                                {
                                    SaldoQtyFin = a.AdjustmentDate < dateFrom && a.AdjustmentDate > dateBalance ? b.Quantity : 0,
                                    AdjFin = a.AdjustmentDate >= dateFrom ? b.Quantity : 0,
                                    ComodityCode = a.ComodityCode,
                                    QtyExpend = 0,
                                    QtyFin = 0,
                                    Retur = 0,
                                };
            var returexpend = from a in (from aa in garmentExpenditureGoodReturnRepository.Query
                                         where aa.ReturDate >= dateBalance && aa.ReturDate <= dateTo //&& aa.ComodityCode == "BR"
                                         select aa)
                              join b in garmentExpenditureGoodReturnItemRepository.Query on a.Identity equals b.ReturId
                              select new mutationView
                              {
                                  SaldoQtyFin = a.ReturDate < dateFrom && a.ReturDate > dateBalance ? b.Quantity : 0,
                                  AdjFin =  0,
                                  ComodityCode = a.ComodityCode,
                                  QtyExpend = 0,
                                  QtyFin = 0,
                                  Retur = a.ReturDate >= dateFrom ? b.Quantity : 0
                              };
            var finishingbarangjadi = from a in (from aa in garmentFinishingOutRepository.Query
                                                 where aa.FinishingOutDate >= dateBalance && aa.FinishingOutDate <= dateTo
                                                 && aa.FinishingTo == "GUDANG JADI" //&& aa.ComodityCode == "BR"
                                                 select aa)
                                      join b in garmentFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
                                      select new mutationView
                                      {
                                          SaldoQtyFin = a.FinishingOutDate.Date < dateFrom.Date && a.FinishingOutDate > dateBalance ? b.Quantity : 0,
                                          AdjFin = 0,
                                          ComodityCode = a.ComodityCode,
                                          QtyExpend = 0,
                                          QtyFin = a.FinishingOutDate>= dateFrom ? b.Quantity : 0,
                                          Retur = 0,
                                      };

            var factexpend = from a in (from aa in garmentExpenditureGoodRepository.Query
                                        where aa.ExpenditureDate >= dateBalance && aa.ExpenditureDate <= dateTo //&& aa.ComodityCode == "BR"
                                        select aa)
                             join b in garmentExpenditureGoodItemRepository.Query on a.Identity equals b.ExpenditureGoodId
                             select new mutationView
                             {
                                 SaldoQtyFin = a.ExpenditureDate < dateFrom && a.ExpenditureDate > dateBalance ? -b.Quantity : 0,
                                 AdjFin = 0,
                                 ComodityCode = a.ComodityCode,
                                 QtyExpend = a.ExpenditureDate >= dateFrom ? b.Quantity : 0,
                                 QtyFin = 0,
                                 Retur = 0,
                             };


            var queryNow = adjust.Union(querybalance).Union(returexpend).Union(finishingbarangjadi).Union(factexpend).AsEnumerable();

            var mutationTemp = queryNow.GroupBy(x => new { x.ComodityCode }, (key, group) => new
            {
                kodeBarang = key.ComodityCode,
                //namaBarang = group.FirstOrDefault().Comodity,
                pemasukan = group.Sum(x => x.Retur + x.QtyFin),
                pengeluaran = group.Sum(x=>x.QtyExpend),
                penyesuaian = 0,
                saldoAwal = group.Sum(x=>x.SaldoQtyFin),
                saldoBuku = group.Sum(x => x.SaldoQtyFin) + group.Sum(x => x.Retur + x.QtyFin) - group.Sum(x => x.QtyExpend),
                selisih = 0,
                stockOpname = 0,
                unitQtyName = "PCS"


            });

            foreach (var i in mutationTemp.Where(x => x.saldoAwal != 0 || x.pemasukan != 0 || x.pengeluaran != 0 || x.penyesuaian != 0 || x.stockOpname != 0 || x.saldoBuku != 0))
            {
                var comodity = (from a in garmentCuttingOutRepository.Query
                                where a.ComodityCode == i.kodeBarang
                                select a.ComodityName).FirstOrDefault();

                GarmentMutationExpenditureGoodDto dto = new GarmentMutationExpenditureGoodDto
                {
                    KodeBarang = i.kodeBarang,
                    NamaBarang = comodity,
                    Pemasukan = i.pemasukan,
                    Pengeluaran = i.pengeluaran,
                    Penyesuaian = i.penyesuaian,
                    SaldoAwal = i.saldoAwal,
                    SaldoBuku = i.saldoBuku,
                    Selisih = i.selisih,
                    StockOpname = i.stockOpname,
                    UnitQtyName = i.unitQtyName
                };

                mutationExpenditureGoodDto.Add(dto);
            }

            expenditureGoodListViewModel.garmentMutations = mutationExpenditureGoodDto.OrderBy(x=>x.KodeBarang).ToList();
            return expenditureGoodListViewModel;



        }

    }
}
