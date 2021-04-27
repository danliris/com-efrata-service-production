using Barebone.Controllers;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Commands;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories;
using Manufactures.Dtos.GarmentSubcon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api.GarmentSubcon
{
    [ApiController]
    [Authorize]
    [Route("subcon-delivery-letter-outs")]
    public class GarmentSubconDeliveryLetterOutController : ControllerApiBase
    {
        private readonly IGarmentSubconDeliveryLetterOutRepository _garmentSubconDeliveryLetterOutRepository;
        private readonly IGarmentSubconDeliveryLetterOutItemRepository _garmentSubconDeliveryLetterOutItemRepository;

        public GarmentSubconDeliveryLetterOutController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentSubconDeliveryLetterOutRepository = Storage.GetRepository<IGarmentSubconDeliveryLetterOutRepository>();
            _garmentSubconDeliveryLetterOutItemRepository = Storage.GetRepository<IGarmentSubconDeliveryLetterOutItemRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentSubconDeliveryLetterOutRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            double totalQty = query.Sum(a => a.GarmentSubconDeliveryLetterOutItem.Sum(b => b.Quantity));

            query = query.Skip((page - 1) * size).Take(size);

            List<GarmentSubconDeliveryLetterOutListDto> garmentSubconDeliveryLetterOutListDtos = _garmentSubconDeliveryLetterOutRepository
                .Find(query)
                .Select(subcon => new GarmentSubconDeliveryLetterOutListDto(subcon))
                .ToList();

            await Task.Yield();
            return Ok(garmentSubconDeliveryLetterOutListDtos, info: new
            {
                page,
                size,
                total,
                totalQty
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            GarmentSubconDeliveryLetterOutDto garmentSubconDeliveryLetterOutDto = _garmentSubconDeliveryLetterOutRepository.Find(o => o.Identity == guid).Select(subcon => new GarmentSubconDeliveryLetterOutDto(subcon)
            {
                Items = _garmentSubconDeliveryLetterOutItemRepository.Find(o => o.SubconDeliveryLetterOutId == subcon.Identity).Select(subconItem => new GarmentSubconDeliveryLetterOutItemDto(subconItem)
                {

                }).ToList()
            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentSubconDeliveryLetterOutDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentSubconDeliveryLetterOutCommand command)
        {
            try
            {
                VerifyUser();

                var order = await Mediator.Send(command);

                if(command.ContractType=="SUBCON BAHAN BAKU")
                    await PutGarmentUnitExpenditureNoteCreate(command.UENId);

                return Ok(order.Identity);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet("complete")]
        public async Task<IActionResult> GetComplete(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentSubconDeliveryLetterOutRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();

            var garmentSubconDeliveryLetterOutDto = _garmentSubconDeliveryLetterOutRepository.Find(query).Select(o => new GarmentSubconDeliveryLetterOutDto(o)).ToArray();
            var garmentSubconDeliveryLetterOutItemDto = _garmentSubconDeliveryLetterOutItemRepository.Find(_garmentSubconDeliveryLetterOutItemRepository.Query).Select(o => new GarmentSubconDeliveryLetterOutItemDto(o)).ToList();

            Parallel.ForEach(garmentSubconDeliveryLetterOutDto, itemDto =>
            {
                var garmentSubconDeliveryLetterOutItems = garmentSubconDeliveryLetterOutItemDto.Where(x => x.SubconDeliveryLetterOutId == itemDto.Id).OrderBy(x => x.Id).ToList();

                itemDto.Items = garmentSubconDeliveryLetterOutItems;

            });

            if (order != "{}")
            {
                Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                garmentSubconDeliveryLetterOutDto = QueryHelper<GarmentSubconDeliveryLetterOutDto>.Order(garmentSubconDeliveryLetterOutDto.AsQueryable(), OrderDictionary).ToArray();
            }

            await Task.Yield();
            return Ok(garmentSubconDeliveryLetterOutDto, info: new
            {
                page,
                size,
                count
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentSubconDeliveryLetterOutCommand command)
        {
            Guid guid = Guid.Parse(id);

            command.SetIdentity(guid);

            VerifyUser();

            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();
            var garmentSubconDeliveryLetterOut = _garmentSubconDeliveryLetterOutRepository.Find(x => x.Identity == guid).Select(o => new GarmentSubconDeliveryLetterOutDto(o)).FirstOrDefault();

            RemoveGarmentSubconDeliveryLetterOutCommand command = new RemoveGarmentSubconDeliveryLetterOutCommand(guid);
            var order = await Mediator.Send(command);
            if(garmentSubconDeliveryLetterOut.ContractType== "SUBCON BAHAN BAKU")
                await PutGarmentUnitExpenditureNoteDelete(garmentSubconDeliveryLetterOut.UENId); 

            return Ok(order.Identity);
        }
    }
}