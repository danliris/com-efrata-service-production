using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentFinishingOuts;
using Manufactures.Domain.GarmentFinishingOuts.ReadModels;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentFinishingOuts.Repositories
{
    public class GarmentFinishingOutRepository : AggregateRepostory<GarmentFinishingOut, GarmentFinishingOutReadModel>, IGarmentFinishingOutRepository
    {
        public IQueryable<GarmentFinishingOutReadModel> Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentFinishingOutReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "FinishingOutNo",
                "UnitCode",
                "UnitToCode",
                "RONo",
                "Article",
                "GarmentFinishingOutItem.ProductCode",
                "GarmentFinishingOutItem.Color",
                "FinishingTo"
            };

            data = QueryHelper<GarmentFinishingOutReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentFinishingOutReadModel>.Order(data, OrderDictionary);

            //data = data.Skip((page - 1) * size).Take(size);

            return data;
        }
		public IQueryable<GarmentFinishingOutReadModel> ReadColor(int page, int size, string order, string keyword, string filter)
		{
			var data = Query;

			Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
			data = QueryHelper<GarmentFinishingOutReadModel>.Filter(data, FilterDictionary);

			List<string> SearchAttributes = new List<string>
			{
				 
				"GarmentFinishingOutItem.Color" 
				 
			};

			data = QueryHelper<GarmentFinishingOutReadModel>.Search(data, SearchAttributes, keyword);

			Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
			data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentFinishingOutReadModel>.Order(data, OrderDictionary);

			//data = data.Skip((page - 1) * size).Take(size);

			return data;
		}

		protected override GarmentFinishingOut Map(GarmentFinishingOutReadModel readModel)
        {
            return new GarmentFinishingOut(readModel);
        }
    }
}