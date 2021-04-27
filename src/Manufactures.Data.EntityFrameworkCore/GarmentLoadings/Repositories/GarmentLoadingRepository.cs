using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentLoadings;
using Manufactures.Domain.GarmentLoadings.ReadModels;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentLoadings.Repositories
{
    public class GarmentLoadingRepository : AggregateRepostory<GarmentLoading, GarmentLoadingReadModel>, IGarmentLoadingRepository
    {
        public IQueryable<GarmentLoadingReadModel> Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentLoadingReadModel>.Filter(data, FilterDictionary);
            
            List<string> SearchAttributes = new List<string>
            {
                "LoadingNo",
                "Article",
                "RONo",
                "UnitCode",
                "UnitName",
                "SewingDONo",
                "UnitFromCode",
                "UnitFromName",
                "Items.Color",
                "Items.ProductName"
            };
            data = QueryHelper<GarmentLoadingReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentLoadingReadModel>.Order(data, OrderDictionary);

            //data = data.Skip((page - 1) * size).Take(size);

            return data;
        }

        protected override GarmentLoading Map(GarmentLoadingReadModel readModel)
        {
            return new GarmentLoading(readModel);
        }
    }
}
