using Barebone.Controllers;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Commands;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Repositories;
using Manufactures.Dtos.GarmentSubcon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api.GarmentSubcon
{
    [ApiController]
    [Authorize]
    [Route("subcon-contracts")]
    public class GarmentSubconContractController : ControllerApiBase
    {
        private readonly IGarmentSubconContractRepository _garmentSubconContractRepository;

        public GarmentSubconContractController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentSubconContractRepository = Storage.GetRepository<IGarmentSubconContractRepository>();
        }
        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentSubconContractRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            query = query.Skip((page - 1) * size).Take(size);
            double totalQty = query.Sum(b => b.Quantity);
            List<GarmentSubconContractDto> garmentSubconContractListDtos = _garmentSubconContractRepository
                .Find(query)
                .Select(subconContract => new GarmentSubconContractDto(subconContract))
                .ToList();

            var dtoIds = garmentSubconContractListDtos.Select(s => s.Id).ToList();
            await Task.Yield();
            return Ok(garmentSubconContractListDtos, info: new
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

            GarmentSubconContractDto garmentSubconContractDto = _garmentSubconContractRepository.Find(o => o.Identity == guid).Select(subcon => new GarmentSubconContractDto(subcon)
            {}).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentSubconContractDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentSubconContractCommand command)
        {
            try
            {
                VerifyUser();
                //var subcon = _garmentSubconContractRepository.Find(a => a.ContractNo.Replace(" ", "") == command.ContractNo.Replace(" ", "")).Select(o => new GarmentSubconContractDto(o)).FirstOrDefault();
                //if (subcon != null)
                //    return BadRequest(new
                //    {
                //        code = HttpStatusCode.BadRequest,
                //        error = "No/Tgl Contract sudah ada"
                //    });
                
                var order = await Mediator.Send(command);

                return Ok(order.Identity);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentSubconContractCommand command)
        {
            Guid guid = Guid.Parse(id);
            //var subcon = _garmentSubconContractRepository.Find(a => a.ContractNo.Replace(" ", "") == command.ContractNo.Replace(" ", "") && a.Identity!=command.Identity).Select(o => new GarmentSubconContractDto(o)).FirstOrDefault();
            //if (subcon != null)
            //    return BadRequest(new
            //    {
            //        code = HttpStatusCode.BadRequest,
            //        error = "No/Tgl Contract sudah ada"
            //    });
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

            RemoveGarmentSubconContractCommand command = new RemoveGarmentSubconContractCommand(guid);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);

        }
    }
}
