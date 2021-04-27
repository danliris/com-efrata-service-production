using Barebone.Tests;
using FluentAssertions;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Application.GarmentExpenditureGoods.Queries;
using Manufactures.Application.GarmentFinishingOuts.Queries;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentExpenditureGoods;
using Manufactures.Domain.GarmentExpenditureGoods.ReadModels;
using Manufactures.Domain.GarmentExpenditureGoods.Repositories;
using Manufactures.Domain.GarmentPreparings;
using Manufactures.Domain.GarmentPreparings.ReadModels;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.CostCalculationGarmentDataProductionReport;

namespace Manufactures.Tests.Queries.GarmentExpenditureGoods
{
	public class MonitoringExpenditureGoodCommandHandlerTest : BaseCommandUnitTest
	{
		private readonly Mock<IGarmentExpenditureGoodRepository> _mockGarmentExpenditureGoodRepository;
		private readonly Mock<IGarmentExpenditureGoodItemRepository> _mockGarmentExpenditureGoodItemRepository;
		private readonly Mock<IGarmentPreparingRepository> _mockGarmentPreparingRepository;
		private readonly Mock<IGarmentPreparingItemRepository> _mockGarmentPreparingItemRepository;
		private readonly Mock<IGarmentCuttingInRepository> _mockGarmentCuttingInRepository;
		protected readonly Mock<IHttpClientService> _mockhttpService;
		private Mock<IServiceProvider> serviceProviderMock;
		public MonitoringExpenditureGoodCommandHandlerTest()
		{
			_mockGarmentExpenditureGoodRepository = CreateMock<IGarmentExpenditureGoodRepository>();
			_mockGarmentExpenditureGoodItemRepository = CreateMock<IGarmentExpenditureGoodItemRepository>();

			_MockStorage.SetupStorage(_mockGarmentExpenditureGoodRepository);
			_MockStorage.SetupStorage(_mockGarmentExpenditureGoodItemRepository);

			_mockGarmentPreparingRepository = CreateMock<IGarmentPreparingRepository>();
			_MockStorage.SetupStorage(_mockGarmentPreparingRepository);

			_mockGarmentPreparingItemRepository = CreateMock<IGarmentPreparingItemRepository>();
			_MockStorage.SetupStorage(_mockGarmentPreparingItemRepository);

			_mockGarmentCuttingInRepository = CreateMock<IGarmentCuttingInRepository>();
			_MockStorage.SetupStorage(_mockGarmentCuttingInRepository);

			serviceProviderMock = new Mock<IServiceProvider>();
			_mockhttpService = CreateMock<IHttpClientService>();

			List<CostCalViewModel> costCalViewModels = new List<CostCalViewModel> {
				new CostCalViewModel
				{
					ro="ro",
					comodityName="",
					buyerCode="",
					hours=10
				}
			};

			_mockhttpService.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"data\": " + JsonConvert.SerializeObject(costCalViewModels) + "}") });
			serviceProviderMock.Setup(x => x.GetService(typeof(IHttpClientService))).Returns(_mockhttpService.Object);
		}
		private GarmentExpenditureGoodQueryHandler CreateGetMonitoringExpenditureGoodQueryHandler()
		{
			return new GarmentExpenditureGoodQueryHandler(_MockStorage.Object, serviceProviderMock.Object);
		}
		[Fact]
		public async Task Handle_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			GarmentExpenditureGoodQueryHandler unitUnderTest = CreateGetMonitoringExpenditureGoodQueryHandler();
			CancellationToken cancellationToken = CancellationToken.None;

			Guid guidExpenditureGood = Guid.NewGuid();
			Guid guidExpenditureGoodItem = Guid.NewGuid();
			Guid guidSewingOut = Guid.NewGuid();
			Guid guidSewingOutItem = Guid.NewGuid();

			GetMonitoringExpenditureGoodQuery getMonitoring = new GetMonitoringExpenditureGoodQuery(1, 25, "{}", 1, DateTime.Now, DateTime.Now.AddDays(2), "token");

			_mockGarmentExpenditureGoodItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentExpenditureGoodItemReadModel>
				{
					new GarmentExpenditureGoodItem(guidExpenditureGoodItem,guidExpenditureGood,new Guid(),new SizeId(1),"",10,0,new UomId(1),"","",10,10).GetReadModel()
				}.AsQueryable());

			_mockGarmentExpenditureGoodRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentExpenditureGoodReadModel>
				{
					new GarmentExpenditureGood(guidExpenditureGood,"","",new UnitDepartmentId(1),"","","ro","",new GarmentComodityId(1),"","",new BuyerId(1),"","",DateTimeOffset.Now,"","",10,"",true,1).GetReadModel()
				}.AsQueryable());

			var guidGarmentPreparing = Guid.NewGuid();
			_mockGarmentPreparingRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentPreparingReadModel>
				{
					new GarmentPreparing(guidGarmentPreparing,1,"uenNo",new Domain.GarmentPreparings.ValueObjects.UnitDepartmentId(1),"unitCode","unitName",DateTimeOffset.Now,"roNo","article",true, new BuyerId(1), null, null).GetReadModel()
				}.AsQueryable());

			var guidGarmentPreparingItem = Guid.NewGuid();
			_mockGarmentPreparingItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentPreparingItemReadModel>
				{
					new GarmentPreparingItem(guidGarmentPreparingItem,1,new Domain.GarmentPreparings.ValueObjects.ProductId(1),"productCode","productName","designColor",1,new Domain.GarmentPreparings.ValueObjects.UomId(1),"uomUnit","fabricType",1,1,guidGarmentPreparing,null).GetReadModel()
				}.AsQueryable());

			var guidGarmentCuttingIn = Guid.NewGuid();
			_mockGarmentCuttingInRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentCuttingInReadModel>
				{
					new GarmentCuttingIn(guidGarmentCuttingIn,"cutInNo","cuttingType","cuttingFrom","rONo","article",new UnitDepartmentId(1),"unitCode","unitName",DateTimeOffset.Now,1).GetReadModel()
				}.AsQueryable());
			// Act
			var result = await unitUnderTest.Handle(getMonitoring, cancellationToken);

			// Assert
			result.Should().NotBeNull();
		}
	}
}
