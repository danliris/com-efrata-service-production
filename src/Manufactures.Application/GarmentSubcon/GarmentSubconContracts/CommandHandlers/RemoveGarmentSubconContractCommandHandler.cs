using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.SubconContracts;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Commands;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSubcon.GarmentSubconContracts.CommandHandlers
{
    public class RemoveGarmentSubconContractCommandHandler : ICommandHandler<RemoveGarmentSubconContractCommand, GarmentSubconContract>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconContractRepository _garmentSubconContractRepository;

        public RemoveGarmentSubconContractCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSubconContractRepository = storage.GetRepository<IGarmentSubconContractRepository>();
        }

        public async Task<GarmentSubconContract> Handle(RemoveGarmentSubconContractCommand request, CancellationToken cancellationToken)
        {
            var subconContract = _garmentSubconContractRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSubconContract(o)).Single();

            subconContract.Remove();
            await _garmentSubconContractRepository.Update(subconContract);

            _storage.Save();

            return subconContract;
        }
    }
}
