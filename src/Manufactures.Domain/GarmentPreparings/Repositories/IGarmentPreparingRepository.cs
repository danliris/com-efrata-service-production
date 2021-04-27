using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentPreparings.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentPreparings.Repositories
{
    public interface IGarmentPreparingRepository : IAggregateRepository<GarmentPreparing, GarmentPreparingReadModel>
    {
        IQueryable<GarmentPreparingReadModel> Read(string order, List<string> select, string filter);
	}
}