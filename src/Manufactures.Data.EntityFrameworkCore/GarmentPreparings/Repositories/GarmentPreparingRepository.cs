using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentPreparings;
using Manufactures.Domain.GarmentPreparings.ReadModels;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Manufactures.Data.EntityFrameworkCore.GarmentPreparings.Repositories
{
    public class GarmentPreparingRepository : AggregateRepostory<GarmentPreparing, GarmentPreparingReadModel>, IGarmentPreparingRepository
    {
        public IQueryable<GarmentPreparingReadModel> Read(string order, List<string> select, string filter)
        {
            var data = Query.OrderByDescending(o => o.CreatedDate).AsQueryable();
            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentPreparingReadModel>.Filter(Query, FilterDictionary);

            return data;
        }
		protected override GarmentPreparing Map(GarmentPreparingReadModel readModel)
        {
            return new GarmentPreparing(readModel);
        }
    }
}