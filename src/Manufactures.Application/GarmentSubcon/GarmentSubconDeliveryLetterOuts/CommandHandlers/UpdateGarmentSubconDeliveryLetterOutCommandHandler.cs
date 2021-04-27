using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Commands;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories;
using Manufactures.Domain.GarmentSubconCuttingOuts;
using Manufactures.Domain.GarmentSubconCuttingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSubcon.GarmentSubconDeliveryLetterOuts.CommandHandlers
{
    public class UpdateGarmentSubconDeliveryLetterOutCommandHandler : ICommandHandler<UpdateGarmentSubconDeliveryLetterOutCommand, GarmentSubconDeliveryLetterOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconDeliveryLetterOutRepository _garmentSubconDeliveryLetterOutRepository;
        private readonly IGarmentSubconDeliveryLetterOutItemRepository _garmentSubconDeliveryLetterOutItemRepository;
        private readonly IGarmentSubconCuttingOutRepository _garmentCuttingOutRepository;

        public UpdateGarmentSubconDeliveryLetterOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSubconDeliveryLetterOutRepository = _storage.GetRepository<IGarmentSubconDeliveryLetterOutRepository>();
            _garmentSubconDeliveryLetterOutItemRepository = _storage.GetRepository<IGarmentSubconDeliveryLetterOutItemRepository>();
            _garmentCuttingOutRepository = storage.GetRepository<IGarmentSubconCuttingOutRepository>();
        }

        public async Task<GarmentSubconDeliveryLetterOut> Handle(UpdateGarmentSubconDeliveryLetterOutCommand request, CancellationToken cancellationToken)
        {
            var subconDeliveryLetterOut = _garmentSubconDeliveryLetterOutRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSubconDeliveryLetterOut(o)).Single();

            if(subconDeliveryLetterOut.ContractType=="SUBCON BAHAN BAKU")
            {

                //subconDeliveryLetterOut.SetEPOItemId(request.EPOItemId);
                //subconDeliveryLetterOut.SetPONo(request.PONo);

                _garmentSubconDeliveryLetterOutItemRepository.Find(o => o.SubconDeliveryLetterOutId == subconDeliveryLetterOut.Identity).ForEach(async subconDeliveryLetterOutItem =>
                {
                    var item = request.Items.Where(o => o.Id == subconDeliveryLetterOutItem.Identity).Single();

                    subconDeliveryLetterOutItem.SetQuantity(item.Quantity);

                    subconDeliveryLetterOutItem.Modify();

                    await _garmentSubconDeliveryLetterOutItemRepository.Update(subconDeliveryLetterOutItem);
                });
            }
            else
            {
                _garmentSubconDeliveryLetterOutItemRepository.Find(o => o.SubconDeliveryLetterOutId == subconDeliveryLetterOut.Identity).ForEach(async subconDLItem =>
                {
                    var item = request.Items.Where(o => o.Id == subconDLItem.Identity).SingleOrDefault();
                    
                    if (item==null)
                    {
                        var subconCuttingOut = _garmentCuttingOutRepository.Query.Where(x => x.Identity == subconDLItem.SubconCuttingOutId).Select(s => new GarmentSubconCuttingOut(s)).Single();
                        subconCuttingOut.SetIsUsed(false);
                        subconCuttingOut.Modify();

                        await _garmentCuttingOutRepository.Update(subconCuttingOut);
                        subconDLItem.Remove();
                    }
                    else
                    {
                        subconDLItem.Modify();
                    }


                    await _garmentSubconDeliveryLetterOutItemRepository.Update(subconDLItem);
                });

                foreach(var item in request.Items)
                {
                    if (item.Id == Guid.Empty)
                    {
                        GarmentSubconDeliveryLetterOutItem garmentSubconDeliveryLetterOutItem = new GarmentSubconDeliveryLetterOutItem(
                            Guid.NewGuid(),
                            subconDeliveryLetterOut.Identity,
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
                        var subconCuttingOut = _garmentCuttingOutRepository.Query.Where(x => x.Identity == item.SubconCuttingOutId).Select(s => new GarmentSubconCuttingOut(s)).Single();
                        subconCuttingOut.SetIsUsed(true);
                        subconCuttingOut.Modify();

                        await _garmentCuttingOutRepository.Update(subconCuttingOut);

                        await _garmentSubconDeliveryLetterOutItemRepository.Update(garmentSubconDeliveryLetterOutItem);
                    }
                }
            }

            subconDeliveryLetterOut.SetDate(request.DLDate.GetValueOrDefault());
            subconDeliveryLetterOut.SetRemark(request.Remark);
            

            subconDeliveryLetterOut.Modify();

            await _garmentSubconDeliveryLetterOutRepository.Update(subconDeliveryLetterOut);

            _storage.Save();

            return subconDeliveryLetterOut;
        }
    }
}
