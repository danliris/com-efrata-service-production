using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSewingDOs;
using Manufactures.Domain.GarmentSewingDOs.ReadModels;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Manufactures.Data.EntityFrameworkCore.GarmentSewingDOs.Repositories
{
    public class GarmentSewingDORepository : AggregateRepostory<GarmentSewingDO, GarmentSewingDOReadModel>, IGarmentSewingDORepository
    {
        public IQueryable<GarmentSewingDOReadModel> Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSewingDOReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "SewingDONo",
                "Article",
                "RONo",
                "UnitCode",
                "GarmentSewingDOItem.ProductCode"
            };

            data = QueryHelper<GarmentSewingDOReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSewingDOReadModel>.Order(data, OrderDictionary);

            //data = data.Skip((page - 1) * size).Take(size);

            return data;
        }

        protected override GarmentSewingDO Map(GarmentSewingDOReadModel readModel)
        {
            return new GarmentSewingDO(readModel);
        }
    }
}