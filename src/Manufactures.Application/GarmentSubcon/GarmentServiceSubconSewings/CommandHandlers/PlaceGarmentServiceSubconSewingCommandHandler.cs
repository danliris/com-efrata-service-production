using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Repositories;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Application.GarmentSubcon.GarmentServiceSubconSewings.CommandHandlers
{
    public class PlaceGarmentServiceSubconSewingCommandHandler : ICommandHandler<PlaceGarmentServiceSubconSewingCommand, GarmentServiceSubconSewing>
    {
        private readonly IStorage _storage;
        private readonly IGarmentServiceSubconSewingRepository _garmentServiceSubconSewingRepository;
        private readonly IGarmentServiceSubconSewingItemRepository _garmentServiceSubconSewingItemRepository;
        private readonly IGarmentServiceSubconSewingDetailRepository _garmentServiceSubconSewingDetailRepository;
        private readonly IGarmentSewingInRepository _garmentSewingInRepository;
        private readonly IGarmentSewingInItemRepository _garmentSewingInItemRepository;

        public PlaceGarmentServiceSubconSewingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentServiceSubconSewingRepository = storage.GetRepository<IGarmentServiceSubconSewingRepository>();
            _garmentServiceSubconSewingItemRepository = storage.GetRepository<IGarmentServiceSubconSewingItemRepository>();
            _garmentSewingInRepository = storage.GetRepository<IGarmentSewingInRepository>();
            _garmentSewingInItemRepository = storage.GetRepository<IGarmentSewingInItemRepository>();
            _garmentServiceSubconSewingDetailRepository= storage.GetRepository<IGarmentServiceSubconSewingDetailRepository>();
        }

        public async Task<GarmentServiceSubconSewing> Handle(PlaceGarmentServiceSubconSewingCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.ToList();

            GarmentServiceSubconSewing garmentServiceSubconSewing = new GarmentServiceSubconSewing(
                Guid.NewGuid(),
                GenerateServiceSubconSewingNo(request),
                request.ServiceSubconSewingDate.GetValueOrDefault(),
                request.IsUsed
            );

            foreach (var item in request.Items)
            {
                GarmentServiceSubconSewingItem garmentServiceSubconSewingItem = new GarmentServiceSubconSewingItem(
                    Guid.NewGuid(),
                    garmentServiceSubconSewing.Identity,
                    item.RONo,
                    item.Article,
                    new GarmentComodityId(item.Comodity.Id),
                    item.Comodity.Code,
                    item.Comodity.Name,
                    new BuyerId(item.Buyer.Id),
                    item.Buyer.Code,
                    item.Buyer.Name
                );
                //item.Id = garmentServiceSubconSewingItem.Identity;

                var SewingIn = _garmentSewingInRepository.Query.Where(x => x.RONo == item.RONo).OrderBy(a => a.CreatedDate).ToList();
                List<GarmentServiceSubconSewingDetail> SewingInDetails = new List<GarmentServiceSubconSewingDetail>();

                foreach (var sewIn in SewingIn)
                {
                    var SewingInItems = _garmentSewingInItemRepository.Query.Where(x => x.SewingInId == sewIn.Identity).OrderBy(a => a.CreatedDate).ToList();
                    foreach (var sewInItem in SewingInItems)
                    {
                        var subconSewingDetails = _garmentServiceSubconSewingDetailRepository.Query.Where(o => o.SewingInItemId == sewInItem.Identity);
                        if (subconSewingDetails != null)
                        {
                            double qty = (double)sewInItem.Quantity;
                            foreach (var subconSewingDetail in subconSewingDetails.ToList())
                            {
                                qty -= subconSewingDetail.Quantity;
                            }
                            if (qty > 0)
                            {
                                SewingInDetails.Add(new GarmentServiceSubconSewingDetail
                                (
                                    new Guid(),
                                    item.Id,
                                    sewIn.Identity,
                                    sewInItem.Identity,
                                    new ProductId(sewInItem.ProductId),
                                    sewInItem.ProductCode,
                                    sewInItem.ProductName,
                                    sewInItem.DesignColor,
                                    qty,
                                    new UomId(sewInItem.UomId),
                                    sewInItem.UomUnit,
                                    new UnitDepartmentId(sewIn.UnitId),
                                    sewIn.UnitCode,
                                    sewIn.UnitName
                                ));
                            }
                        }
                    }
                }

                foreach (var detail in item.Details)
                {
                    if (detail.IsSave)
                    {
                        var sewInDetail = SewingInDetails.Where(y => y.DesignColor == detail.DesignColor && y.UnitId== new UnitDepartmentId(detail.Unit.Id)).ToList();
                        var qty = detail.Quantity;
                        foreach (var d in sewInDetail)
                        {
                            var qtyRemains = d.Quantity - qty;
                            if (qtyRemains >= 0)
                            {
                                GarmentServiceSubconSewingDetail garmentServiceSubconSewingDetail = new GarmentServiceSubconSewingDetail(
                                    Guid.NewGuid(),
                                    garmentServiceSubconSewingItem.Identity,
                                    d.SewingInId,
                                    d.SewingInItemId,
                                    d.ProductId,
                                    d.ProductCode,
                                    d.ProductName,
                                    d.DesignColor,
                                    qty,
                                    d.UomId,
                                    d.UomUnit,
                                    d.UnitId,
                                    d.UnitCode,
                                    d.UnitName
                                );
                                await _garmentServiceSubconSewingDetailRepository.Update(garmentServiceSubconSewingDetail);
                                break;
                            }
                            else if (qtyRemains < 0)
                            {
                                qty -= d.Quantity;
                                GarmentServiceSubconSewingDetail garmentServiceSubconSewingDetail = new GarmentServiceSubconSewingDetail(
                                    Guid.NewGuid(),
                                    garmentServiceSubconSewingItem.Identity,
                                    d.SewingInId,
                                    d.SewingInItemId,
                                    d.ProductId,
                                    d.ProductCode,
                                    d.ProductName,
                                    d.DesignColor,
                                    d.Quantity,
                                    d.UomId,
                                    d.UomUnit,
                                    d.UnitId,
                                    d.UnitCode,
                                    d.UnitName
                                );
                                await _garmentServiceSubconSewingDetailRepository.Update(garmentServiceSubconSewingDetail);
                            }
                        }

                    }
                }
                await _garmentServiceSubconSewingItemRepository.Update(garmentServiceSubconSewingItem);
            }

            await _garmentServiceSubconSewingRepository.Update(garmentServiceSubconSewing);

            _storage.Save();

            return garmentServiceSubconSewing;
        }

        private string GenerateServiceSubconSewingNo(PlaceGarmentServiceSubconSewingCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");

            var prefix = $"SJS{year}{month}";

            var lastServiceSubconSewingNo = _garmentServiceSubconSewingRepository.Query.Where(w => w.ServiceSubconSewingNo.StartsWith(prefix))
                .OrderByDescending(o => o.ServiceSubconSewingNo)
                .Select(s => int.Parse(s.ServiceSubconSewingNo.Replace(prefix, "")))
                .FirstOrDefault();
            var ServiceSubconSewingNo = $"{prefix}{(lastServiceSubconSewingNo + 1).ToString("D4")}";

            return ServiceSubconSewingNo;
        }
    }
}
