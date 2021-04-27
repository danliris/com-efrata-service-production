using Barebone.Controllers;
using Manufactures.Domain.GarmentSubconFinishingIns.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    [ApiController]
    [Authorize]
    [Route("subcon-finishing-ins")]
    public class GarmentSubconFinishingInController : ControllerApiBase
    {
        public GarmentSubconFinishingInController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentSubconFinishingInCommand command)
        {
            try
            {
                VerifyUser();

                var order = await Mediator.Send(command);

                return Ok(order.Identity);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            RemoveGarmentSubconFinishingInCommand command = new RemoveGarmentSubconFinishingInCommand(guid);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }

    }
}
