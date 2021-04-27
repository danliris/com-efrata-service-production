using Manufactures.Domain.GarmentScrapSources.Commands;
using Manufactures.Domain.GarmentScrapSources.Repositories;
using Manufactures.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
	[ApiController]
	[Authorize]
	[Route("scrap-transactions")]
	public class GarmentScrapTransactionController : Barebone.Controllers.ControllerApiBase
	{
		private readonly IGarmentScrapTransactionRepository _garmentScrapTransactionRepository;
		private readonly IGarmentScrapTransactionItemRepository _garmentScrapTransactionItemRepository;

		public GarmentScrapTransactionController(IServiceProvider serviceProvider) : base(serviceProvider)
		{
			_garmentScrapTransactionRepository = Storage.GetRepository<IGarmentScrapTransactionRepository>();
			_garmentScrapTransactionItemRepository = Storage.GetRepository<IGarmentScrapTransactionItemRepository>();

		}

		[HttpGet]
		public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
		{
			VerifyUser();

			var query = _garmentScrapTransactionRepository.Read(page, size, order, keyword, filter);
			var count = query.Count();

			List<GarmentScrapTransactionDto> listDtos = _garmentScrapTransactionRepository
				.Find(query)
				.Select(data => new GarmentScrapTransactionDto(data)).Where(s => s.TransactionType == "IN")
				.ToList();

			await Task.Yield();
			return Ok(listDtos, info: new
			{
				page,
				size,
				count
			});
		}
		[HttpGet("out")]
		public async Task<IActionResult> GetOut(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
		{
			VerifyUser();

			var query = _garmentScrapTransactionRepository.Read(page, size, order, keyword, filter);
			var count = query.Count();

			List<GarmentScrapTransactionDto> listDtos = _garmentScrapTransactionRepository
				.Find(query)
				.Select(data => new GarmentScrapTransactionDto(data)).Where(s=>s.TransactionType== "OUT")
				.ToList();

			await Task.Yield();
			return Ok(listDtos, info: new
			{
				page,
				size,
				count
			});
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(string id)
		{
			Guid guid = Guid.Parse(id);
			VerifyUser();

			GarmentScrapTransactionDto garmentScrapTransactionDto = _garmentScrapTransactionRepository.Find(o => o.Identity == guid).Select(ScrapTransaction => new GarmentScrapTransactionDto(ScrapTransaction)
			{
				Items = _garmentScrapTransactionItemRepository.Find(o => o.ScrapTransactionId == ScrapTransaction.Identity).OrderBy(i=>i.ScrapClassificationName).Select(ScrapTransactionItem => new GarmentScrapTransactionItemDto(ScrapTransactionItem)
				).ToList()
			}
			).FirstOrDefault();

			await Task.Yield();
			return Ok(garmentScrapTransactionDto);
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] PlaceGarmentScrapTransactionCommand command)
		{
			VerifyUser();

			var data = await Mediator.Send(command);

			return Ok(data.Identity);
		}
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(string id)
		{
			Guid guid = Guid.Parse(id);

			VerifyUser();

			RemoveGarmentScrapTransactionCommand command = new RemoveGarmentScrapTransactionCommand(guid);
			var data = await Mediator.Send(command);

			return Ok(data.Identity);
		}


		[HttpPut("{id}")]
		public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentScrapTransactionCommand command)
		{
			Guid guid = Guid.Parse(id);

			command.SetIdentity(guid);

			VerifyUser();

			var order = await Mediator.Send(command);

			return Ok(order.Identity);
		}
	}
}
