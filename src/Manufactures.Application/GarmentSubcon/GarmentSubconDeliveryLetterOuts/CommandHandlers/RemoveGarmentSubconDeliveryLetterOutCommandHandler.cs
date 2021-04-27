using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Commands;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories;
using Manufactures.Domain.GarmentSubconCuttingOuts;
using Manufactures.Domain.GarmentSubconCuttingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSubcon.GarmentSubconDeliveryLetterOuts.CommandHandlers
{
    public class RemoveGarmentSubconDeliveryLetterOutCommandHandler : ICommandHandler<RemoveGarmentSubconDeliveryLetterOutCommand, GarmentSubconDeliveryLetterOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconDeliveryLetterOutRepository _garmentSubconDeliveryLetterOutRepository;
        private readonly IGarmentSubconDeliveryLetterOutItemRepository _garmentSubconDeliveryLetterOutItemRepository;
        private readonly IGarmentSubconCuttingOutRepository _garmentCuttingOutRepository;

        public RemoveGarmentSubconDeliveryLetterOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSubconDeliveryLetterOutRepository = storage.GetRepository<IGarmentSubconDeliveryLetterOutRepository>();
            _garmentSubconDeliveryLetterOutItemRepository = storage.GetRepository<IGarmentSubconDeliveryLetterOutItemRepository>();
            _garmentCuttingOutRepository = storage.GetRepository<IGarmentSubconCuttingOutRepository>();
        }


        public async Task<GarmentSubconDeliveryLetterOut> Handle(RemoveGarmentSubconDeliveryLetterOutCommand request, CancellationToken cancellationToken)
        {
            var subconDeliveryLetterOut = _garmentSubconDeliveryLetterOutRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSubconDeliveryLetterOut(o)).Single();

            _garmentSubconDeliveryLetterOutItemRepository.Find(o => o.SubconDeliveryLetterOutId == subconDeliveryLetterOut.Identity).ForEach(async subconDeliveryLetterOutItem =>
            {
                subconDeliveryLetterOutItem.Remove();
                if (subconDeliveryLetterOut.ContractType == "SUBCON CUTTING")
                {
                    var subconCuttingOut = _garmentCuttingOutRepository.Query.Where(x => x.Identity == subconDeliveryLetterOutItem.SubconCuttingOutId).Select(s => new GarmentSubconCuttingOut(s)).Single();
                    subconCuttingOut.SetIsUsed(false);
                    subconCuttingOut.Modify();

                    await _garmentCuttingOutRepository.Update(subconCuttingOut);
                }
                await _garmentSubconDeliveryLetterOutItemRepository.Update(subconDeliveryLetterOutItem);
            });

            subconDeliveryLetterOut.Remove();
            await _garmentSubconDeliveryLetterOutRepository.Update(subconDeliveryLetterOut);

            _storage.Save();

            return subconDeliveryLetterOut;
        }
    }
}
