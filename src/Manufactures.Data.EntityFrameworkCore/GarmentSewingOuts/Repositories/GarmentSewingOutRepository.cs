using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSewingOuts;
using Manufactures.Domain.GarmentSewingOuts.ReadModels;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSewingOuts.Repositories
{
    public class GarmentSewingOutRepository : AggregateRepostory<GarmentSewingOut, GarmentSewingOutReadModel>, IGarmentSewingOutRepository
    {
        public IQueryable<GarmentSewingOutReadModel> Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query ;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSewingOutReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "SewingOutNo",
                "UnitCode",
                "UnitToCode",
                "RONo",
                "Article",
                "GarmentSewingOutItem.ProductCode",
                "GarmentSewingOutItem.Color"
            };

            data = QueryHelper<GarmentSewingOutReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSewingOutReadModel>.Order(data, OrderDictionary);

            //data = data.Skip((page - 1) * size).Take(size);

            return data;
        }

        public IQueryable<GarmentSewingOutReadModel> ReadComplete(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSewingOutReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "RONo",
            };

            data = QueryHelper<GarmentSewingOutReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSewingOutReadModel>.Order(data, OrderDictionary);

            //var data2 = data.Skip((page - 1) * size).Take(size);

            return data;
        }

        public IQueryable ReadDynamic(string order, string search, string select, string keyword, string filter)
        {
            var data = Query ;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSewingOutReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = JsonConvert.DeserializeObject<List<string>>(search);
            if (SearchAttributes.Count == 0)
            {
                SearchAttributes = new List<string>
                {
                    "SewingOutNo",
                    "UnitCode",
                    "UnitToCode",
                    "RONo",
                    "Article",
                    "GarmentSewingOutItem.ProductCode",
                    "GarmentSewingOutItem.Color"
                };
            }
            data = QueryHelper<GarmentSewingOutReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSewingOutReadModel>.Order(data, OrderDictionary);

            var selectedData = QueryHelper<GarmentSewingOutReadModel>.Select(data, select);

            return selectedData;
        }

        protected override GarmentSewingOut Map(GarmentSewingOutReadModel readModel)
        {
            return new GarmentSewingOut(readModel);
        }
    }
}