using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentExpenditureGoods;
using Manufactures.Domain.GarmentExpenditureGoods.Commands;
using Manufactures.Domain.GarmentExpenditureGoods.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentExpenditureGoods.CommandHandlers
{
    public class UpdateGarmentExpenditureGoodCommandHandler : ICommandHandler<UpdateGarmentExpenditureGoodCommand, GarmentExpenditureGood>
    {
        private readonly IStorage _storage;
        private readonly IGarmentExpenditureGoodRepository _garmentExpenditureGoodRepository;
        private readonly IGarmentExpenditureGoodItemRepository _garmentExpenditureGoodItemRepository;

        public UpdateGarmentExpenditureGoodCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentExpenditureGoodRepository = storage.GetRepository<IGarmentExpenditureGoodRepository>();
            _garmentExpenditureGoodItemRepository = storage.GetRepository<IGarmentExpenditureGoodItemRepository>();

        }

        public async Task<GarmentExpenditureGood> Handle(UpdateGarmentExpenditureGoodCommand request, CancellationToken cancellationToken)
        {
            var ExpenditureGood = _garmentExpenditureGoodRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentExpenditureGood(o)).Single();

            _garmentExpenditureGoodItemRepository.Find(o => o.ExpenditureGoodId == ExpenditureGood.Identity).ForEach(async expenditureItem =>
            {
                await _garmentExpenditureGoodItemRepository.Update(expenditureItem);
            });
            ExpenditureGood.SetCarton(request.Carton);
            ExpenditureGood.SetExpenditureDate(request.ExpenditureDate);
            ExpenditureGood.SetPackingListId(request.PackingListId);
            ExpenditureGood.SetInvoice(request.Invoice);
            ExpenditureGood.SetIsReceived(request.IsReceived);
            ExpenditureGood.Modify();
            await _garmentExpenditureGoodRepository.Update(ExpenditureGood);

            _storage.Save();

            return ExpenditureGood;
        }
    }
}
