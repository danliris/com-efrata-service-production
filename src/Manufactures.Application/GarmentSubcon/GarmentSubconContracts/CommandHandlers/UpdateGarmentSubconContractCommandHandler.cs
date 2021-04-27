using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.SubconContracts;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Commands;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSubcon.GarmentSubconContracts.CommandHandlers
{
    public class UpdateGarmentSubconContractCommandHandler : ICommandHandler<UpdateGarmentSubconContractCommand, GarmentSubconContract>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconContractRepository _garmentSubconContractRepository;

        public UpdateGarmentSubconContractCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSubconContractRepository = storage.GetRepository<IGarmentSubconContractRepository>();
        }

        public async Task<GarmentSubconContract> Handle(UpdateGarmentSubconContractCommand request, CancellationToken cancellationToken)
        {
            var subconContract = _garmentSubconContractRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSubconContract(o)).Single();

            subconContract.SetAgreementNo(request.AgreementNo);
            subconContract.SetBPJNo(request.BPJNo);
            subconContract.SetContractNo(request.ContractNo);
            subconContract.SetContractType(request.ContractType);
            subconContract.SetDueDate(request.DueDate);
            subconContract.SetFinishedGoodType(request.FinishedGoodType);
            subconContract.SetJobType(request.JobType);
            subconContract.SetQuantity(request.Quantity);
            subconContract.SetSupplierCode(request.Supplier.Code);
            subconContract.SetSupplierId(new SupplierId(request.Supplier.Id));
            subconContract.SetSupplierName(request.Supplier.Name);
            subconContract.SetContractDate(request.ContractDate);
            subconContract.SetIsUsed(request.IsUsed);
            subconContract.Modify();
            await _garmentSubconContractRepository.Update(subconContract);

            _storage.Save();

            return subconContract;
        }
    }
}
