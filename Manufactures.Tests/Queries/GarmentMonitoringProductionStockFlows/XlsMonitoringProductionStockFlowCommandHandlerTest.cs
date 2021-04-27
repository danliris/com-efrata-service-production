using Barebone.Tests;
using FluentAssertions;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Application.GarmentMonitoringProductionStockFlows.Queries;
using Manufactures.Domain.GarmentAdjustments;
using Manufactures.Domain.GarmentAdjustments.ReadModels;
using Manufactures.Domain.GarmentAdjustments.Repositories;
using Manufactures.Domain.GarmentAvalComponents;
using Manufactures.Domain.GarmentAvalComponents.ReadModels;
using Manufactures.Domain.GarmentAvalComponents.Repositories;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.ReadModels;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentCuttingOuts;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.GarmentExpenditureGoodReturns;
using Manufactures.Domain.GarmentExpenditureGoodReturns.ReadModels;
using Manufactures.Domain.GarmentExpenditureGoodReturns.Repositories;
using Manufactures.Domain.GarmentExpenditureGoods;
using Manufactures.Domain.GarmentExpenditureGoods.ReadModels;
using Manufactures.Domain.GarmentExpenditureGoods.Repositories;
using Manufactures.Domain.GarmentFinishingIns;
using Manufactures.Domain.GarmentFinishingIns.ReadModels;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentFinishingOuts;
using Manufactures.Domain.GarmentFinishingOuts.ReadModels;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using Manufactures.Domain.GarmentLoadings;
using Manufactures.Domain.GarmentLoadings.ReadModels;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Manufactures.Domain.GarmentPreparings;
using Manufactures.Domain.GarmentPreparings.ReadModels;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Manufactures.Domain.GarmentSewingDOs;
using Manufactures.Domain.GarmentSewingDOs.ReadModels;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.GarmentSewingIns.ReadModels;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.GarmentSewingOuts;
using Manufactures.Domain.GarmentSewingOuts.ReadModels;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.CostCalculationGarmentDataProductionReport;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.HOrderDataProductionReport;

namespace Manufactures.Tests.Queries.GarmentMonitoringProductionStockFlows
{
	public class XlsMonitoringProductionStockFlowCommandHandlerTest : BaseCommandUnitTest
	{
		private readonly Mock<IGarmentCuttingOutRepository> _mockGarmentCuttingOutRepository;
		private readonly Mock<IGarmentCuttingOutItemRepository> _mockGarmentCuttingOutItemRepository;
		private readonly Mock<IGarmentCuttingOutDetailRepository> _mockGarmentCuttingOutDetailRepository;
		private readonly Mock<IGarmentCuttingInRepository> _mockGarmentCuttingInRepository;
		private readonly Mock<IGarmentCuttingInItemRepository> _mockGarmentCuttingInItemRepository;
		private readonly Mock<IGarmentCuttingInDetailRepository> _mockGarmentCuttingInDetailRepository;
		private readonly Mock<IGarmentLoadingRepository> _mockGarmentLoadingRepository;
		private readonly Mock<IGarmentLoadingItemRepository> _mockGarmentLoadingItemRepository;
		private readonly Mock<IGarmentSewingOutRepository> _mockGarmentSewingOutRepository;
		private readonly Mock<IGarmentSewingOutItemRepository> _mockGarmentSewingOutItemRepository;
		private readonly Mock<IGarmentFinishingOutRepository> _mockGarmentFinishingOutRepository;
		private readonly Mock<IGarmentFinishingOutItemRepository> _mockGarmentFinishingOutItemRepository;
		private readonly Mock<IGarmentSewingDORepository> _mockGarmentSewingDORepository;
		private readonly Mock<IGarmentSewingDOItemRepository> _mockGarmentSewingDOItemRepository;
		private readonly Mock<IGarmentExpenditureGoodRepository> _mockGarmentExpenditureGoodRepository;
		private readonly Mock<IGarmentExpenditureGoodItemRepository> _mockGarmentExpenditureGoodItemRepository;
		private readonly Mock<IGarmentFinishingInRepository> _mockGarmentFinishingInRepository;
		private readonly Mock<IGarmentFinishingInItemRepository> _mockGarmentFinishingInItemRepository;
		private readonly Mock<IGarmentAdjustmentRepository> _mockGarmentAdjustmentRepository;
		private readonly Mock<IGarmentAdjustmentItemRepository> _mockGarmentAdjustmentItemRepository;
		private readonly Mock<IGarmentAvalComponentRepository> _mockGarmentAvalComponentRepository;
		private readonly Mock<IGarmentAvalComponentItemRepository> _mockGarmentAvalComponentItemRepository;
		private readonly Mock<IGarmentExpenditureGoodReturnRepository> _mockGarmentExpenditureGoodReturnRepository;
		private readonly Mock<IGarmentExpenditureGoodReturnItemRepository> _mockGarmentExpenditureGoodReturnItemRepository;
		private readonly Mock<IGarmentSewingInRepository> _mockGarmentSewingInRepository;
		private readonly Mock<IGarmentSewingInItemRepository> _mockGarmentSewingInItemRepository;
		private readonly Mock<IGarmentComodityPriceRepository> _mockGarmentComodityPriceRepository;
		private readonly Mock<IGarmentPreparingRepository> _mockGarmentPreparingRepository;
		private readonly Mock<IGarmentPreparingItemRepository> _mockGarmentPreparingItemRepository;

		protected readonly Mock<IHttpClientService> _mockhttpService;
		private Mock<IServiceProvider> serviceProviderMock;

		public XlsMonitoringProductionStockFlowCommandHandlerTest()
		{
			_mockGarmentFinishingOutRepository = CreateMock<IGarmentFinishingOutRepository>();
			_mockGarmentFinishingOutItemRepository = CreateMock<IGarmentFinishingOutItemRepository>();
			_MockStorage.SetupStorage(_mockGarmentFinishingOutRepository);
			_MockStorage.SetupStorage(_mockGarmentFinishingOutItemRepository);

			_mockGarmentSewingOutRepository = CreateMock<IGarmentSewingOutRepository>();
			_mockGarmentSewingOutItemRepository = CreateMock<IGarmentSewingOutItemRepository>();
			_MockStorage.SetupStorage(_mockGarmentSewingOutRepository);
			_MockStorage.SetupStorage(_mockGarmentSewingOutItemRepository);

			_mockGarmentLoadingRepository = CreateMock<IGarmentLoadingRepository>();
			_mockGarmentLoadingItemRepository = CreateMock<IGarmentLoadingItemRepository>();
			_MockStorage.SetupStorage(_mockGarmentLoadingRepository);
			_MockStorage.SetupStorage(_mockGarmentLoadingItemRepository);

			_mockGarmentCuttingOutRepository = CreateMock<IGarmentCuttingOutRepository>();
			_mockGarmentCuttingOutItemRepository = CreateMock<IGarmentCuttingOutItemRepository>();
			_mockGarmentCuttingOutDetailRepository = CreateMock<IGarmentCuttingOutDetailRepository>();
			_MockStorage.SetupStorage(_mockGarmentCuttingOutRepository);
			_MockStorage.SetupStorage(_mockGarmentCuttingOutItemRepository);
			_MockStorage.SetupStorage(_mockGarmentCuttingOutDetailRepository);

			_mockGarmentSewingDORepository = CreateMock<IGarmentSewingDORepository>();
			_mockGarmentSewingDOItemRepository = CreateMock<IGarmentSewingDOItemRepository>();
			_MockStorage.SetupStorage(_mockGarmentSewingDORepository);
			_MockStorage.SetupStorage(_mockGarmentSewingDOItemRepository);

			_mockGarmentExpenditureGoodRepository = CreateMock<IGarmentExpenditureGoodRepository>();
			_mockGarmentExpenditureGoodItemRepository = CreateMock<IGarmentExpenditureGoodItemRepository>();
			_MockStorage.SetupStorage(_mockGarmentExpenditureGoodRepository);
			_MockStorage.SetupStorage(_mockGarmentExpenditureGoodItemRepository);

			_mockGarmentFinishingInRepository = CreateMock<IGarmentFinishingInRepository>();
			_mockGarmentFinishingInItemRepository = CreateMock<IGarmentFinishingInItemRepository>();
			_MockStorage.SetupStorage(_mockGarmentFinishingInRepository);
			_MockStorage.SetupStorage(_mockGarmentFinishingInItemRepository);

			_mockGarmentCuttingInRepository = CreateMock<IGarmentCuttingInRepository>();
			_mockGarmentCuttingInItemRepository = CreateMock<IGarmentCuttingInItemRepository>();
			_mockGarmentCuttingInDetailRepository = CreateMock<IGarmentCuttingInDetailRepository>();
			_MockStorage.SetupStorage(_mockGarmentCuttingInRepository);
			_MockStorage.SetupStorage(_mockGarmentCuttingInItemRepository);
			_MockStorage.SetupStorage(_mockGarmentCuttingInDetailRepository);


			_mockGarmentAdjustmentRepository = CreateMock<IGarmentAdjustmentRepository>();
			_mockGarmentAdjustmentItemRepository = CreateMock<IGarmentAdjustmentItemRepository>();
			_MockStorage.SetupStorage(_mockGarmentAdjustmentRepository);
			_MockStorage.SetupStorage(_mockGarmentAdjustmentItemRepository);

			_mockGarmentAvalComponentRepository = CreateMock<IGarmentAvalComponentRepository>();
			_mockGarmentAvalComponentItemRepository = CreateMock<IGarmentAvalComponentItemRepository>();
			_MockStorage.SetupStorage(_mockGarmentAvalComponentRepository);
			_MockStorage.SetupStorage(_mockGarmentAvalComponentItemRepository);

			_mockGarmentExpenditureGoodReturnRepository = CreateMock<IGarmentExpenditureGoodReturnRepository>();
			_mockGarmentExpenditureGoodReturnItemRepository = CreateMock<IGarmentExpenditureGoodReturnItemRepository>();
			_MockStorage.SetupStorage(_mockGarmentExpenditureGoodReturnRepository);
			_MockStorage.SetupStorage(_mockGarmentExpenditureGoodReturnItemRepository);

			_mockGarmentSewingInRepository = CreateMock<IGarmentSewingInRepository>();
			_mockGarmentSewingInItemRepository = CreateMock<IGarmentSewingInItemRepository>();
			_MockStorage.SetupStorage(_mockGarmentSewingInRepository);
			_MockStorage.SetupStorage(_mockGarmentSewingInItemRepository);

			_mockGarmentComodityPriceRepository = CreateMock<IGarmentComodityPriceRepository>();
			_MockStorage.SetupStorage(_mockGarmentComodityPriceRepository);

			_mockGarmentPreparingRepository = CreateMock<IGarmentPreparingRepository>();
			_MockStorage.SetupStorage(_mockGarmentPreparingRepository);

			_mockGarmentPreparingItemRepository = CreateMock<IGarmentPreparingItemRepository>();
			_MockStorage.SetupStorage(_mockGarmentPreparingItemRepository);

			serviceProviderMock = new Mock<IServiceProvider>();
			_mockhttpService = CreateMock<IHttpClientService>();

			List<CostCalViewModel> costCalViewModels = new List<CostCalViewModel> {
				new CostCalViewModel
				{
					ro="ro",
					comodityName="",
					buyerCode="buyer",
					hours=10
				}
			};
			List<HOrderViewModel> HOrderViewModels = new List<HOrderViewModel> {
				new HOrderViewModel
				{
					No="ro",
					Codeby="buyer",
					Qty=100,
					Kode="dd"
				}
			};
			_mockhttpService.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"data\": " + JsonConvert.SerializeObject(costCalViewModels) + "}") });
			serviceProviderMock.Setup(x => x.GetService(typeof(IHttpClientService))).Returns(_mockhttpService.Object);
		}
		private GetXlsMonitoringProductionStockFlowQueryHandler CreateGetMonitoringProductionFlowQueryHandler()
		{
			return new GetXlsMonitoringProductionStockFlowQueryHandler(_MockStorage.Object, serviceProviderMock.Object);
		}
		[Fact]
		public async Task Handle_StateUnderTest_ExpectedBehavior_bookkeeping()
		{
			// Arrange
			GetXlsMonitoringProductionStockFlowQueryHandler unitUnderTest = CreateGetMonitoringProductionFlowQueryHandler();
			CancellationToken cancellationToken = CancellationToken.None;

			Guid guidLoading = Guid.NewGuid();
			Guid guidLoadingItem = Guid.NewGuid();
			Guid guidCuttingOut = Guid.NewGuid();
			Guid guidCuttingOutItem = Guid.NewGuid();
			Guid guidCuttingOutDetail = Guid.NewGuid();
			Guid guidFinishingOut = Guid.NewGuid();
			Guid guidFinishingOutItem = Guid.NewGuid();
			Guid guidSewingOut = Guid.NewGuid();
			Guid guidSewingOutItem = Guid.NewGuid();
			Guid guidSewingIn = Guid.NewGuid();
			Guid guidSewingInItem = Guid.NewGuid();
			Guid guidSewingDO = Guid.NewGuid();
			Guid guidSewingDOItem = Guid.NewGuid();
			Guid guidCuttingIn = Guid.NewGuid();
			Guid guidCuttingInItem = Guid.NewGuid();
			Guid guidFinishingIn = Guid.NewGuid();
			Guid guidFinishingInItem = Guid.NewGuid();
			Guid guidAdjustment = Guid.NewGuid();
			Guid guidAval = Guid.NewGuid();
			Guid guidExpenditure = Guid.NewGuid();
			Guid guidExpenditureReturn = Guid.NewGuid();
			Guid guidAdjustmentItem = Guid.NewGuid();
			GetXlsMonitoringProductionStockFlowQuery getMonitoring = new GetXlsMonitoringProductionStockFlowQuery(1, 25, "{}", 1, "ro", DateTime.Now.AddDays(-5), DateTime.Now, "bookkeeping","token");

			_mockGarmentLoadingItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentLoadingItemReadModel>
				{
					new GarmentLoadingItem(guidLoadingItem,guidLoading,new Guid(),new SizeId(1),"",new ProductId(1),"","","",0,0,0, new UomId(1),"","",10).GetReadModel()
				}.AsQueryable());

			_mockGarmentLoadingRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentLoadingReadModel>
				{
					new GarmentLoading(guidLoading,"",new Guid(),"",new UnitDepartmentId(1),"","","ro","",new UnitDepartmentId(1),"","",DateTimeOffset.Now,new GarmentComodityId(1),"","").GetReadModel()
				}.AsQueryable());

			_mockGarmentCuttingOutDetailRepository
			.Setup(s => s.Query)
			.Returns(new List<GarmentCuttingOutDetailReadModel>
			{
					new GarmentCuttingOutDetail(new Guid(),guidCuttingOutItem,new SizeId(1),"","",100,100,new UomId(1),"",10,10).GetReadModel()
			}.AsQueryable());

			_mockGarmentCuttingOutItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentCuttingOutItemReadModel>
				{
					new GarmentCuttingOutItem(guidCuttingOutItem,new Guid() ,new Guid(),guidCuttingOut,new ProductId(1),"","","",100).GetReadModel()
				}.AsQueryable());
			_mockGarmentCuttingOutRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentCuttingOutReadModel>
				{
					 new GarmentCuttingOut(guidCuttingOut, "", "SEWING",new UnitDepartmentId(1),"","",DateTime.Now.AddDays(-1),"ro","article",new UnitDepartmentId(1),"","",new GarmentComodityId(1),"cm","cmo",false).GetReadModel()
				}.AsQueryable());



			_mockGarmentFinishingOutItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentFinishingOutItemReadModel>
				{
					new GarmentFinishingOutItem(guidFinishingOutItem,guidFinishingOut,new Guid(),new Guid(),new ProductId(1),"","","",new SizeId(1),"",10, new UomId(1),"","",10,10,10).GetReadModel()
				}.AsQueryable());

			_mockGarmentFinishingOutRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentFinishingOutReadModel>
				{
					new GarmentFinishingOut(guidFinishingOut,"",new UnitDepartmentId(1),"","","GUDANG JADI",DateTimeOffset.Now.AddDays(-1),"ro","",new UnitDepartmentId(1),"","",new GarmentComodityId(1),"","",false).GetReadModel()
				}.AsQueryable());


			_mockGarmentSewingOutItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSewingOutItemReadModel>
				{
					new GarmentSewingOutItem(guidSewingOutItem,guidSewingOut,new Guid(),new Guid(), new ProductId(1),"","","",new SizeId(1),"",0, new UomId(1),"","",10,100,100).GetReadModel()
				}.AsQueryable());

			_mockGarmentSewingOutRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSewingOutReadModel>
				{
					new GarmentSewingOut(guidSewingOut,"",new BuyerId(1),"","",new UnitDepartmentId(1),"","","FINISHING",DateTimeOffset.Now,"ro","",new UnitDepartmentId(1),"","",new GarmentComodityId(1),"","",true).GetReadModel()
				}.AsQueryable());

			_mockGarmentSewingDOItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSewingDOItemReadModel>
				{
					new GarmentSewingDOItem(guidSewingDOItem,guidSewingDO,guidCuttingOutDetail,guidCuttingOutItem, new ProductId(1),"","","",new SizeId(1),"",0, new UomId(1),"","",10,100,100).GetReadModel()
				}.AsQueryable());

			_mockGarmentSewingDORepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSewingDOReadModel>
				{
					new GarmentSewingDO(guidSewingDO,"",guidCuttingOut,new UnitDepartmentId(1),"","",new UnitDepartmentId(1),"","","ro","",new GarmentComodityId(1),"","",DateTimeOffset.Now).GetReadModel()
				}.AsQueryable());

			_mockGarmentCuttingInDetailRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentCuttingInDetailReadModel>
				{
					new GarmentCuttingInDetail(new Guid(),guidCuttingInItem,new Guid(),new Guid(),new Guid(),new ProductId(1),"","","","Main Fabric",10,new UomId(1),"",10,new UomId(1),"",10,100,100,1,"").GetReadModel()
				}.AsQueryable());

			_mockGarmentCuttingInItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentCuttingInItemReadModel>
				{
					new GarmentCuttingInItem(guidCuttingInItem,guidCuttingIn,new Guid(),1,"",guidSewingOut,"").GetReadModel()
				}.AsQueryable());

			_mockGarmentCuttingInRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentCuttingInReadModel>
				{
					new GarmentCuttingIn(guidCuttingIn,"","Main Fabric","","ro","",new UnitDepartmentId(1),"","",DateTimeOffset.Now.AddDays(-1),1).GetReadModel()
				}.AsQueryable());

			_mockGarmentFinishingInItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentFinishingInItemReadModel>
				{
					new GarmentFinishingInItem(guidFinishingInItem,guidFinishingIn,guidSewingOutItem,new Guid(),new Guid(),new SizeId(1),"",new ProductId(1),"","","",8,19,new UomId(1),"","",10,10).GetReadModel()
				}.AsQueryable());

			_mockGarmentFinishingInRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentFinishingInReadModel>
				{
					new GarmentFinishingIn(guidFinishingIn,"","PEMBELIAN",new UnitDepartmentId(1),"","","ro","",new UnitDepartmentId(1),"","",DateTimeOffset.Now,new GarmentComodityId(1),"","",2,"").GetReadModel()
				}.AsQueryable());

			_mockGarmentAdjustmentItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentAdjustmentItemReadModel>
				{
					new GarmentAdjustmentItem(guidAdjustmentItem,guidLoading,guidSewingDOItem,new Guid(),new Guid(),new Guid(),new SizeId(1),"",new ProductId(1),"","","",8,19,new UomId(1),"","",10).GetReadModel()
				}.AsQueryable());

			_mockGarmentAdjustmentRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentAdjustmentReadModel>
				{
					new GarmentAdjustment(guidAdjustment,"","LOADING","ro","",new UnitDepartmentId(1),"","",DateTimeOffset.Now,new GarmentComodityId(1),"","","").GetReadModel()
				}.AsQueryable());
			_mockGarmentAdjustmentItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentAdjustmentItemReadModel>
				{
					new GarmentAdjustmentItem(guidAdjustmentItem,guidLoading,guidSewingDOItem,new Guid(),new Guid(),new Guid(),new SizeId(1),"",new ProductId(1),"","","",8,19,new UomId(1),"","",10).GetReadModel()
				}.AsQueryable());

			_mockGarmentAdjustmentRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentAdjustmentReadModel>
				{
					new GarmentAdjustment(guidAdjustment,"","SEWING","ro","",new UnitDepartmentId(1),"","",DateTimeOffset.Now,new GarmentComodityId(1),"","","").GetReadModel()
				}.AsQueryable());
			_mockGarmentAdjustmentItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentAdjustmentItemReadModel>
				{
					new GarmentAdjustmentItem(guidAdjustmentItem,guidLoading,guidSewingDOItem,new Guid(),new Guid(),new Guid(),new SizeId(1),"",new ProductId(1),"","","",8,19,new UomId(1),"","",10).GetReadModel()
				}.AsQueryable());

			_mockGarmentAdjustmentRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentAdjustmentReadModel>
				{
					new GarmentAdjustment(guidAdjustment,"","FINISHING","ro","",new UnitDepartmentId(1),"","",DateTimeOffset.Now,new GarmentComodityId(1),"","","").GetReadModel()
				}.AsQueryable());
			_mockGarmentAdjustmentItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentAdjustmentItemReadModel>
				{
					new GarmentAdjustmentItem(guidAdjustmentItem,guidLoading,guidSewingDOItem,new Guid(),new Guid(),new Guid(),new SizeId(1),"",new ProductId(1),"","","",8,19,new UomId(1),"","",10).GetReadModel()
				}.AsQueryable());

			_mockGarmentAdjustmentRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentAdjustmentReadModel>
				{
					new GarmentAdjustment(guidAdjustment,"","GUDANG JADI","ro","",new UnitDepartmentId(1),"","",DateTimeOffset.Now,new GarmentComodityId(1),"","","").GetReadModel()
				}.AsQueryable());

			_mockGarmentSewingInItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSewingInItemReadModel>
				{
					new GarmentSewingInItem(guidSewingInItem,guidSewingIn,guidSewingOutItem,new Guid(),guidLoadingItem,guidFinishingOutItem,new Guid(), new ProductId(1),"","","",new SizeId(1),"",0, new UomId(1),"","",10,100,100).GetReadModel()
				}.AsQueryable());

			_mockGarmentSewingInRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSewingInReadModel>
				{
					new GarmentSewingIn(guidSewingIn,"89","FINISHING",guidLoading,"",new UnitDepartmentId(1),"","",new UnitDepartmentId(1),"","","ro","article",new GarmentComodityId(1),"","",DateTimeOffset.Now).GetReadModel()
				}.AsQueryable());

			_mockGarmentAvalComponentItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentAvalComponentItemReadModel>
				{
					new GarmentAvalComponentItem(guidAval,guidAval,new Guid(),new Guid(),guidLoadingItem, new ProductId(1),"","","","",10,0, new SizeId(1),"",100,100).GetReadModel()
				}.AsQueryable());

			_mockGarmentAvalComponentRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentAvalComponentReadModel>
				{
					new GarmentAvalComponent(guidAval,"",new UnitDepartmentId(1),"","","","ro","article",new GarmentComodityId(1),"","",DateTimeOffset.Now, false).GetReadModel()
				}.AsQueryable());
			_mockGarmentExpenditureGoodItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentExpenditureGoodItemReadModel>
				{
					new GarmentExpenditureGoodItem(new Guid(),guidExpenditure,new Guid(), new SizeId(1),"",100,100,new UomId(1),"","",100,100).GetReadModel()
				}.AsQueryable());

			_mockGarmentExpenditureGoodRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentExpenditureGoodReadModel>
				{
					new GarmentExpenditureGood(guidExpenditure,"np","SAMPLE",new UnitDepartmentId(1),"","","ro","article",new GarmentComodityId(1),"","",new BuyerId(1),"","",DateTimeOffset.Now,"","",9,"",true,1).GetReadModel()
				}.AsQueryable());
			_mockGarmentExpenditureGoodReturnItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentExpenditureGoodReturnItemReadModel>
				{
					new GarmentExpenditureGoodReturnItem(new Guid(),guidExpenditureReturn,guidExpenditure,new Guid(),new Guid(), new SizeId(1),"",100,new UomId(1),"","",100,100).GetReadModel()
				}.AsQueryable());

			_mockGarmentExpenditureGoodReturnRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentExpenditureGoodReturnReadModel>
				{
					new GarmentExpenditureGoodReturn(guidExpenditureReturn,"np","SAMPLE",new UnitDepartmentId(1),"","","ro","article",new GarmentComodityId(1),"","",new BuyerId(1),"","",DateTimeOffset.Now,"","").GetReadModel()
				}.AsQueryable());
			_mockGarmentComodityPriceRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentComodityPriceReadModel>
				{
					new GarmentComodityPrice(guidExpenditureReturn,true,DateTimeOffset.Now.AddDays(-1),new UnitDepartmentId(1),"","",new GarmentComodityId(1),"ccc","dsd",100).GetReadModel()
				}.AsQueryable());

			var guidGarmentPreparing = Guid.NewGuid();
			_mockGarmentPreparingRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentPreparingReadModel>
				{
					new GarmentPreparing(guidGarmentPreparing,1,"uenNo",new Domain.GarmentPreparings.ValueObjects.UnitDepartmentId(1),"unitCode","unitName",DateTimeOffset.Now,"roNo","article",true,new BuyerId(1), null,null).GetReadModel()
				}.AsQueryable());

			var guidGarmentPreparingItem = Guid.NewGuid();
			_mockGarmentPreparingItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentPreparingItemReadModel>
				{
					new GarmentPreparingItem(guidGarmentPreparingItem,1,new Domain.GarmentPreparings.ValueObjects.ProductId(1),"productCode","productName","designColor",1,new Domain.GarmentPreparings.ValueObjects.UomId(1),"uomUnit","fabricType",1,1,guidGarmentPreparing,null).GetReadModel()
				}.AsQueryable());



			// Act
			var result = await unitUnderTest.Handle(getMonitoring, cancellationToken);

			// Assert
			result.Should().NotBeNull();
		}

		[Fact]
		public async Task Handle_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			GetXlsMonitoringProductionStockFlowQueryHandler unitUnderTest = CreateGetMonitoringProductionFlowQueryHandler();
			CancellationToken cancellationToken = CancellationToken.None;

			Guid guidLoading = Guid.NewGuid();
			Guid guidLoadingItem = Guid.NewGuid();
			Guid guidCuttingOut = Guid.NewGuid();
			Guid guidCuttingOutItem = Guid.NewGuid();
			Guid guidCuttingOutDetail = Guid.NewGuid();
			Guid guidFinishingOut = Guid.NewGuid();
			Guid guidFinishingOutItem = Guid.NewGuid();
			Guid guidSewingOut = Guid.NewGuid();
			Guid guidSewingOutItem = Guid.NewGuid();
			Guid guidSewingIn = Guid.NewGuid();
			Guid guidSewingInItem = Guid.NewGuid();
			Guid guidSewingDO = Guid.NewGuid();
			Guid guidSewingDOItem = Guid.NewGuid();
			Guid guidCuttingIn = Guid.NewGuid();
			Guid guidCuttingInItem = Guid.NewGuid();
			Guid guidFinishingIn = Guid.NewGuid();
			Guid guidFinishingInItem = Guid.NewGuid();
			Guid guidAdjustment = Guid.NewGuid();
			Guid guidAval = Guid.NewGuid();
			Guid guidExpenditure = Guid.NewGuid();
			Guid guidExpenditureReturn = Guid.NewGuid();
			Guid guidAdjustmentItem = Guid.NewGuid();
			GetXlsMonitoringProductionStockFlowQuery getMonitoring = new GetXlsMonitoringProductionStockFlowQuery(1, 25, "{}", 1, "ro", DateTime.Now.AddDays(-5), DateTime.Now, "", "token");

			_mockGarmentLoadingItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentLoadingItemReadModel>
				{
					new GarmentLoadingItem(guidLoadingItem,guidLoading,new Guid(),new SizeId(1),"",new ProductId(1),"","","",0,0,0, new UomId(1),"","",10).GetReadModel()
				}.AsQueryable());

			_mockGarmentLoadingRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentLoadingReadModel>
				{
					new GarmentLoading(guidLoading,"",new Guid(),"",new UnitDepartmentId(1),"","","ro","",new UnitDepartmentId(1),"","",DateTimeOffset.Now,new GarmentComodityId(1),"","").GetReadModel()
				}.AsQueryable());

			_mockGarmentCuttingOutDetailRepository
			.Setup(s => s.Query)
			.Returns(new List<GarmentCuttingOutDetailReadModel>
			{
					new GarmentCuttingOutDetail(new Guid(),guidCuttingOutItem,new SizeId(1),"","",100,100,new UomId(1),"",10,10).GetReadModel()
			}.AsQueryable());

			_mockGarmentCuttingOutItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentCuttingOutItemReadModel>
				{
					new GarmentCuttingOutItem(guidCuttingOutItem,new Guid() ,new Guid(),guidCuttingOut,new ProductId(1),"","","",100).GetReadModel()
				}.AsQueryable());
			_mockGarmentCuttingOutRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentCuttingOutReadModel>
				{
					 new GarmentCuttingOut(guidCuttingOut, "", "SEWING",new UnitDepartmentId(1),"","",DateTime.Now.AddDays(-1),"ro","article",new UnitDepartmentId(1),"","",new GarmentComodityId(1),"cm","cmo",false).GetReadModel()
				}.AsQueryable());



			_mockGarmentFinishingOutItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentFinishingOutItemReadModel>
				{
					new GarmentFinishingOutItem(guidFinishingOutItem,guidFinishingOut,new Guid(),new Guid(),new ProductId(1),"","","",new SizeId(1),"",10, new UomId(1),"","",10,10,10).GetReadModel()
				}.AsQueryable());

			_mockGarmentFinishingOutRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentFinishingOutReadModel>
				{
					new GarmentFinishingOut(guidFinishingOut,"",new UnitDepartmentId(1),"","","GUDANG JADI",DateTimeOffset.Now.AddDays(-1),"ro","",new UnitDepartmentId(1),"","",new GarmentComodityId(1),"","",false).GetReadModel()
				}.AsQueryable());


			_mockGarmentSewingOutItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSewingOutItemReadModel>
				{
					new GarmentSewingOutItem(guidSewingOutItem,guidSewingOut,new Guid(),new Guid(), new ProductId(1),"","","",new SizeId(1),"",0, new UomId(1),"","",10,100,100).GetReadModel()
				}.AsQueryable());

			_mockGarmentSewingOutRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSewingOutReadModel>
				{
					new GarmentSewingOut(guidSewingOut,"",new BuyerId(1),"","",new UnitDepartmentId(1),"","","FINISHING",DateTimeOffset.Now,"ro","",new UnitDepartmentId(1),"","",new GarmentComodityId(1),"","",true).GetReadModel()
				}.AsQueryable());

			_mockGarmentSewingDOItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSewingDOItemReadModel>
				{
					new GarmentSewingDOItem(guidSewingDOItem,guidSewingDO,guidCuttingOutDetail,guidCuttingOutItem, new ProductId(1),"","","",new SizeId(1),"",0, new UomId(1),"","",10,100,100).GetReadModel()
				}.AsQueryable());

			_mockGarmentSewingDORepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSewingDOReadModel>
				{
					new GarmentSewingDO(guidSewingDO,"",guidCuttingOut,new UnitDepartmentId(1),"","",new UnitDepartmentId(1),"","","ro","",new GarmentComodityId(1),"","",DateTimeOffset.Now).GetReadModel()
				}.AsQueryable());

			_mockGarmentCuttingInDetailRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentCuttingInDetailReadModel>
				{
					new GarmentCuttingInDetail(new Guid(),guidCuttingInItem,new Guid(),new Guid(),new Guid(),new ProductId(1),"","","","Main Fabric",10,new UomId(1),"",10,new UomId(1),"",10,100,100,1,"").GetReadModel()
				}.AsQueryable());

			_mockGarmentCuttingInItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentCuttingInItemReadModel>
				{
					new GarmentCuttingInItem(guidCuttingInItem,guidCuttingIn,new Guid(),1,"",guidSewingOut,"").GetReadModel()
				}.AsQueryable());

			_mockGarmentCuttingInRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentCuttingInReadModel>
				{
					new GarmentCuttingIn(guidCuttingIn,"","Main Fabric","","ro","",new UnitDepartmentId(1),"","",DateTimeOffset.Now.AddDays(-1),1).GetReadModel()
				}.AsQueryable());

			_mockGarmentFinishingInItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentFinishingInItemReadModel>
				{
					new GarmentFinishingInItem(guidFinishingInItem,guidFinishingIn,guidSewingOutItem,new Guid(),new Guid(),new SizeId(1),"",new ProductId(1),"","","",8,19,new UomId(1),"","",10,10).GetReadModel()
				}.AsQueryable());

			_mockGarmentFinishingInRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentFinishingInReadModel>
				{
					new GarmentFinishingIn(guidFinishingIn,"","PEMBELIAN",new UnitDepartmentId(1),"","","ro","",new UnitDepartmentId(1),"","",DateTimeOffset.Now,new GarmentComodityId(1),"","",2,"").GetReadModel()
				}.AsQueryable());

			_mockGarmentAdjustmentItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentAdjustmentItemReadModel>
				{
					new GarmentAdjustmentItem(guidAdjustmentItem,guidLoading,guidSewingDOItem,new Guid(),new Guid(),new Guid(),new SizeId(1),"",new ProductId(1),"","","",8,19,new UomId(1),"","",10).GetReadModel()
				}.AsQueryable());

			_mockGarmentAdjustmentRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentAdjustmentReadModel>
				{
					new GarmentAdjustment(guidAdjustment,"","LOADING","ro","",new UnitDepartmentId(1),"","",DateTimeOffset.Now,new GarmentComodityId(1),"","","").GetReadModel()
				}.AsQueryable());
			_mockGarmentAdjustmentItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentAdjustmentItemReadModel>
				{
					new GarmentAdjustmentItem(guidAdjustmentItem,guidLoading,guidSewingDOItem,new Guid(),new Guid(),new Guid(),new SizeId(1),"",new ProductId(1),"","","",8,19,new UomId(1),"","",10).GetReadModel()
				}.AsQueryable());

			_mockGarmentAdjustmentRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentAdjustmentReadModel>
				{
					new GarmentAdjustment(guidAdjustment,"","SEWING","ro","",new UnitDepartmentId(1),"","",DateTimeOffset.Now,new GarmentComodityId(1),"","","").GetReadModel()
				}.AsQueryable());
			_mockGarmentAdjustmentItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentAdjustmentItemReadModel>
				{
					new GarmentAdjustmentItem(guidAdjustmentItem,guidLoading,guidSewingDOItem,new Guid(),new Guid(),new Guid(),new SizeId(1),"",new ProductId(1),"","","",8,19,new UomId(1),"","",10).GetReadModel()
				}.AsQueryable());

			_mockGarmentAdjustmentRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentAdjustmentReadModel>
				{
					new GarmentAdjustment(guidAdjustment,"","FINISHING","ro","",new UnitDepartmentId(1),"","",DateTimeOffset.Now,new GarmentComodityId(1),"","","").GetReadModel()
				}.AsQueryable());
			_mockGarmentAdjustmentItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentAdjustmentItemReadModel>
				{
					new GarmentAdjustmentItem(guidAdjustmentItem,guidLoading,guidSewingDOItem,new Guid(),new Guid(),new Guid(),new SizeId(1),"",new ProductId(1),"","","",8,19,new UomId(1),"","",10).GetReadModel()
				}.AsQueryable());

			_mockGarmentAdjustmentRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentAdjustmentReadModel>
				{
					new GarmentAdjustment(guidAdjustment,"","GUDANG JADI","ro","",new UnitDepartmentId(1),"","",DateTimeOffset.Now,new GarmentComodityId(1),"","","").GetReadModel()
				}.AsQueryable());

			_mockGarmentSewingInItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSewingInItemReadModel>
				{
					new GarmentSewingInItem(guidSewingInItem,guidSewingIn,guidSewingOutItem,new Guid(),guidLoadingItem,guidFinishingOutItem,new Guid(), new ProductId(1),"","","",new SizeId(1),"",0, new UomId(1),"","",10,100,100).GetReadModel()
				}.AsQueryable());

			_mockGarmentSewingInRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSewingInReadModel>
				{
					new GarmentSewingIn(guidSewingIn,"89","FINISHING",guidLoading,"",new UnitDepartmentId(1),"","",new UnitDepartmentId(1),"","","ro","article",new GarmentComodityId(1),"","",DateTimeOffset.Now).GetReadModel()
				}.AsQueryable());

			_mockGarmentAvalComponentItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentAvalComponentItemReadModel>
				{
					new GarmentAvalComponentItem(guidAval,guidAval,new Guid(),new Guid(),guidLoadingItem, new ProductId(1),"","","","",10,0, new SizeId(1),"",100,100).GetReadModel()
				}.AsQueryable());

			_mockGarmentAvalComponentRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentAvalComponentReadModel>
				{
					new GarmentAvalComponent(guidAval,"",new UnitDepartmentId(1),"","","","ro","article",new GarmentComodityId(1),"","",DateTimeOffset.Now, false).GetReadModel()
				}.AsQueryable());
			_mockGarmentExpenditureGoodItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentExpenditureGoodItemReadModel>
				{
					new GarmentExpenditureGoodItem(new Guid(),guidExpenditure,new Guid(), new SizeId(1),"",100,100,new UomId(1),"","",100,100).GetReadModel()
				}.AsQueryable());

			_mockGarmentExpenditureGoodRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentExpenditureGoodReadModel>
				{
					new GarmentExpenditureGood(guidExpenditure,"np","SAMPLE",new UnitDepartmentId(1),"","","ro","article",new GarmentComodityId(1),"","",new BuyerId(1),"","",DateTimeOffset.Now,"","",9,"",true,1).GetReadModel()
				}.AsQueryable());
			_mockGarmentExpenditureGoodReturnItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentExpenditureGoodReturnItemReadModel>
				{
					new GarmentExpenditureGoodReturnItem(new Guid(),guidExpenditureReturn,guidExpenditure,new Guid(),new Guid(), new SizeId(1),"",100,new UomId(1),"","",100,100).GetReadModel()
				}.AsQueryable());

			_mockGarmentExpenditureGoodReturnRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentExpenditureGoodReturnReadModel>
				{
					new GarmentExpenditureGoodReturn(guidExpenditureReturn,"np","SAMPLE",new UnitDepartmentId(1),"","","ro","article",new GarmentComodityId(1),"","",new BuyerId(1),"","",DateTimeOffset.Now,"","").GetReadModel()
				}.AsQueryable());
			_mockGarmentComodityPriceRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentComodityPriceReadModel>
				{
					new GarmentComodityPrice(guidExpenditureReturn,true,DateTimeOffset.Now.AddDays(-1),new UnitDepartmentId(1),"","",new GarmentComodityId(1),"ccc","dsd",100).GetReadModel()
				}.AsQueryable());
			_mockGarmentComodityPriceRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentComodityPriceReadModel>
				{
					new GarmentComodityPrice(guidExpenditureReturn,true,DateTimeOffset.Now.AddDays(2),new UnitDepartmentId(1),"","",new GarmentComodityId(1),"ccc","dsd",200).GetReadModel()
				}.AsQueryable());

			var guidGarmentPreparing = Guid.NewGuid();
			_mockGarmentPreparingRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentPreparingReadModel>
				{
					new GarmentPreparing(guidGarmentPreparing,1,"uenNo",new Domain.GarmentPreparings.ValueObjects.UnitDepartmentId(1),"unitCode","unitName",DateTimeOffset.Now,"roNo","article",true,new BuyerId(1), null,null).GetReadModel()
				}.AsQueryable());

			var guidGarmentPreparingItem = Guid.NewGuid();
			_mockGarmentPreparingItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentPreparingItemReadModel>
				{
					new GarmentPreparingItem(guidGarmentPreparingItem,1,new Domain.GarmentPreparings.ValueObjects.ProductId(1),"productCode","productName","designColor",1,new Domain.GarmentPreparings.ValueObjects.UomId(1),"uomUnit","fabricType",1,1,guidGarmentPreparing,null).GetReadModel()
				}.AsQueryable());

			// Act
			var result = await unitUnderTest.Handle(getMonitoring, cancellationToken);

			// Assert
			result.Should().NotBeNull();
		}
	}
}
