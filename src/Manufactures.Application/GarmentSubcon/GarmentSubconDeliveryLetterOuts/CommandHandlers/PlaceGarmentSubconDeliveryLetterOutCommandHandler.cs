using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Commands;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manufactures.Domain.Shared.ValueObjects;
using System.Threading;
using Manufactures.Domain.GarmentSubconCuttingOuts.Repositories;
using Manufactures.Domain.GarmentSubconCuttingOuts;

namespace Manufactures.Application.GarmentSubcon.GarmentSubconDeliveryLetterOuts.CommandHandlers
{
    public class PlaceGarmentSubconDeliveryLetterOutCommandHandler : ICommandHandler<PlaceGarmentSubconDeliveryLetterOutCommand, GarmentSubconDeliveryLetterOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconDeliveryLetterOutRepository _garmentSubconDeliveryLetterOutRepository;
        private readonly IGarmentSubconDeliveryLetterOutItemRepository _garmentSubconDeliveryLetterOutItemRepository;
        private readonly IGarmentSubconCuttingOutRepository _garmentCuttingOutRepository;

        public PlaceGarmentSubconDeliveryLetterOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSubconDeliveryLetterOutRepository = storage.GetRepository<IGarmentSubconDeliveryLetterOutRepository>();
            _garmentSubconDeliveryLetterOutItemRepository = storage.GetRepository<IGarmentSubconDeliveryLetterOutItemRepository>();
            _garmentCuttingOutRepository = storage.GetRepository<IGarmentSubconCuttingOutRepository>();
        }

        public async Task<GarmentSubconDeliveryLetterOut> Handle(PlaceGarmentSubconDeliveryLetterOutCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.ToList();

            GarmentSubconDeliveryLetterOut garmentSubconDeliveryLetterOut = new GarmentSubconDeliveryLetterOut(
                Guid.NewGuid(),
                GenerateNo(request),
                request.DLType,
                request.SubconContractId,
                request.ContractNo,
                request.ContractType,
                request.DLDate,
                request.UENId,
                request.UENNo,
                request.PONo,
                request.EPOItemId,
                request.Remark,
                request.IsUsed
            );

            foreach (var item in request.Items)
            {
                GarmentSubconDeliveryLetterOutItem garmentSubconDeliveryLetterOutItem = new GarmentSubconDeliveryLetterOutItem(
                    Guid.NewGuid(),
                    garmentSubconDeliveryLetterOut.Identity,
                    item.UENItemId,
                    new ProductId(item.Product.Id),
                    item.Product.Code,
                    item.Product.Name,
                    item.ProductRemark,
                    item.DesignColor,
                    item.Quantity,
                    new UomId(item.Uom.Id),
                    item.Uom.Unit,
                    new UomId(item.UomOut.Id),
                    item.UomOut.Unit,
                    item.FabricType,
                    item.SubconCuttingOutId,
                    item.RONo,
                    item.POSerialNumber,
                    item.SubconCuttingOutNo
                );
                if(request.ContractType=="SUBCON CUTTING")
                {
                    var subconCuttingOut= _garmentCuttingOutRepository.Query.Where(x => x.Identity == item.SubconCuttingOutId).Select(s => new GarmentSubconCuttingOut(s)).Single();
                    subconCuttingOut.SetIsUsed(true);
                    subconCuttingOut.Modify();

                    await _garmentCuttingOutRepository.Update(subconCuttingOut);
                }
                await _garmentSubconDeliveryLetterOutItemRepository.Update(garmentSubconDeliveryLetterOutItem);
            }

            await _garmentSubconDeliveryLetterOutRepository.Update(garmentSubconDeliveryLetterOut);

            _storage.Save();

            return garmentSubconDeliveryLetterOut;
        }

        private string GenerateNo(PlaceGarmentSubconDeliveryLetterOutCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var code = request.DLType == "PROSES" ? "" : "/R";
            var type = request.ContractType == "SUBCON BAHAN BAKU" ? "BB" : request.ContractType == "SUBCON CUTTING" ? "CT" : "JS";

            var prefix = $"SJK/{type}/{year}{month}";

            var lastNo = _garmentSubconDeliveryLetterOutRepository.Query.Where(w => w.DLNo.StartsWith(prefix))
                .OrderByDescending(o => o.DLNo)
                .Select(s => int.Parse(s.DLNo.Substring(11, 4)))
                .FirstOrDefault();
            var no = $"{prefix}{(lastNo + 1).ToString("D4")}{code}";

            return no;
        }
    }
}