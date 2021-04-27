using Barebone.Tests;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Application.GarmentSubcon.GarmentSubconDeliveryLetterOuts.CommandHandlers;
using Xunit;
using System.Threading.Tasks;
using System.Threading;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Commands;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ReadModels;
using System.Linq;
using System.Linq.Expressions;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using FluentAssertions;
using Manufactures.Domain.GarmentSubconCuttingOuts.Repositories;
using Manufactures.Domain.GarmentSubconCuttingOuts;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;

namespace Manufactures.Tests.CommandHandlers.GarmentSubcon.GarmentSubconDeliveryLetterOuts
{
    public class UpdateGarmentSubconDeliveryLetterOutCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSubconDeliveryLetterOutRepository> _mockSubconDeliveryLetterOutRepository;
        private readonly Mock<IGarmentSubconDeliveryLetterOutItemRepository> _mockSubconDeliveryLetterOutItemRepository;
        private readonly Mock<IGarmentSubconCuttingOutRepository> _mockSubconCuttingOutRepository;

        public UpdateGarmentSubconDeliveryLetterOutCommandHandlerTests()
        {
            _mockSubconDeliveryLetterOutRepository = CreateMock<IGarmentSubconDeliveryLetterOutRepository>();
            _mockSubconDeliveryLetterOutItemRepository = CreateMock<IGarmentSubconDeliveryLetterOutItemRepository>();
            _mockSubconCuttingOutRepository = CreateMock<IGarmentSubconCuttingOutRepository>();

            _MockStorage.SetupStorage(_mockSubconDeliveryLetterOutRepository);
            _MockStorage.SetupStorage(_mockSubconDeliveryLetterOutItemRepository);
            _MockStorage.SetupStorage(_mockSubconCuttingOutRepository);
        }

        private UpdateGarmentSubconDeliveryLetterOutCommandHandler CreateUpdateGarmentSubconDeliveryLetterOutCommandHandler()
        {
            return new UpdateGarmentSubconDeliveryLetterOutCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_BB()
        {
            // Arrange
            Guid SubconDeliveryLetterOutGuid = Guid.NewGuid();
            Guid sewingInId = Guid.NewGuid();
            UpdateGarmentSubconDeliveryLetterOutCommandHandler unitUnderTest = CreateUpdateGarmentSubconDeliveryLetterOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateGarmentSubconDeliveryLetterOutCommand UpdateGarmentSubconDeliveryLetterOutCommand = new UpdateGarmentSubconDeliveryLetterOutCommand()
            {
                ContractNo = "test",
                ContractType = "SUBCON BAHAN BAKU",
                DLDate = DateTimeOffset.Now.AddDays(1),
                DLType = "PROSES",
                EPOItemId = 1,
                IsUsed = false,
                PONo = "test",
                Remark = "test",
                TotalQty = 10,
                UsedQty = 10,
                SubconContractId = new Guid(),
                UENId = 1,
                UENNo = "test",
                Items = new List<GarmentSubconDeliveryLetterOutItemValueObject>
                {
                    new GarmentSubconDeliveryLetterOutItemValueObject
                    {
                        Product = new Product(1, "ProductCode", "ProductName"),
                        Quantity=1,
                        DesignColor= "ColorD",
                        SubconDeliveryLetterOutId=Guid.NewGuid(),
                        FabricType="test",
                        ProductRemark="test",
                        UENItemId=1,
                        Uom=new Uom(1,"UomUnit"),
                        UomOut=new Uom(1,"UomUnit"),
                        ContractQuantity=1
                    }
                },
            };
            UpdateGarmentSubconDeliveryLetterOutCommand.SetIdentity(SubconDeliveryLetterOutGuid);

            _mockSubconDeliveryLetterOutRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>()
               {
                    new GarmentSubconDeliveryLetterOut(SubconDeliveryLetterOutGuid,"","",Guid.NewGuid(),"","SUBCON BAHAN BAKU",DateTimeOffset.Now,1,"","",1,"",false).GetReadModel()
               }.AsQueryable());
            _mockSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconDeliveryLetterOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSubconDeliveryLetterOutItem>()
                {
                    new GarmentSubconDeliveryLetterOutItem(Guid.Empty,SubconDeliveryLetterOutGuid,1,new ProductId(1),"code","name","remark","color",1,new UomId(1),"unit",new UomId(1),"unit","fabType",new Guid(),"","","")
                });

            _mockSubconDeliveryLetterOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOut>()));
            _mockSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOutItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentSubconDeliveryLetterOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_CT()
        {
            // Arrange
            Guid SubconDeliveryLetterOutGuid = Guid.NewGuid();
            Guid subconCuttingOutGuid = Guid.NewGuid();
            UpdateGarmentSubconDeliveryLetterOutCommandHandler unitUnderTest = CreateUpdateGarmentSubconDeliveryLetterOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateGarmentSubconDeliveryLetterOutCommand UpdateGarmentSubconDeliveryLetterOutCommand = new UpdateGarmentSubconDeliveryLetterOutCommand()
            {
                ContractNo = "test",
                ContractType = "SUBCON CUTTING",
                DLDate = DateTimeOffset.Now,
                DLType = "RE PROSES",
                EPOItemId = 1,
                IsUsed = false,
                PONo = "test",
                Remark = "test",
                TotalQty = 10,
                UsedQty = 10,
                SubconContractId = new Guid(),
                UENId = 1,
                UENNo = "test",
                Items = new List<GarmentSubconDeliveryLetterOutItemValueObject>
                {
                    new GarmentSubconDeliveryLetterOutItemValueObject
                    {
                        Product = new Product(1, "ProductCode", "ProductName"),
                        Quantity=1,
                        DesignColor= "ColorD",
                        SubconDeliveryLetterOutId=Guid.NewGuid(),
                        FabricType="test",
                        ProductRemark="test",
                        UENItemId=1,
                        Uom=new Uom(1,"UomUnit"),
                        UomOut=new Uom(1,"UomUnit"),
                        ContractQuantity=1,
                        SubconCuttingOutId=subconCuttingOutGuid,
                        RONo="ro",
                        POSerialNumber="aa",
                        SubconCuttingOutNo="no"
                    }
                },
            };
            UpdateGarmentSubconDeliveryLetterOutCommand.SetIdentity(SubconDeliveryLetterOutGuid);

            _mockSubconDeliveryLetterOutRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>()
               {
                    new GarmentSubconDeliveryLetterOut(SubconDeliveryLetterOutGuid,"","",Guid.NewGuid(),"","SUBCON CUTTING",DateTimeOffset.Now,1,"","",1,"",false).GetReadModel()
               }.AsQueryable());
            _mockSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconDeliveryLetterOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSubconDeliveryLetterOutItem>()
                {
                    new GarmentSubconDeliveryLetterOutItem(Guid.Empty,SubconDeliveryLetterOutGuid,1,new ProductId(1),"code","name","remark","color",1,new UomId(1),"unit",new UomId(1),"unit","fabType",subconCuttingOutGuid,"","","")
                });

            GarmentSubconCuttingOut garmentSubconCuttingOut = new GarmentSubconCuttingOut(subconCuttingOutGuid, "no", "", new UnitDepartmentId(1), "", "", DateTimeOffset.Now, "ro", "", new GarmentComodityId(1), "", "", 1, 1, "", false);

            _mockSubconCuttingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingOutReadModel>
                {
                    garmentSubconCuttingOut.GetReadModel()
                }.AsQueryable());


            _mockSubconDeliveryLetterOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOut>()));
            _mockSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOutItem>()));

            _mockSubconCuttingOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconCuttingOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconCuttingOut>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentSubconDeliveryLetterOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_JS()
        {
            Guid SubconDeliveryLetterOutGuid = Guid.NewGuid();
            Guid subconCuttingOutGuid = Guid.NewGuid();
            UpdateGarmentSubconDeliveryLetterOutCommandHandler unitUnderTest = CreateUpdateGarmentSubconDeliveryLetterOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateGarmentSubconDeliveryLetterOutCommand UpdateGarmentSubconDeliveryLetterOutCommand = new UpdateGarmentSubconDeliveryLetterOutCommand()
            {
                ContractNo = "test",
                ContractType = "SUBCON JASA",
                DLDate = DateTimeOffset.Now,
                DLType = "RE PROSES",
                EPOItemId = 1,
                IsUsed = false,
                PONo = "test",
                Remark = "test",
                TotalQty = 10,
                UsedQty = 10,
                SubconContractId = new Guid(),
                UENId = 1,
                UENNo = "test",
                Items = new List<GarmentSubconDeliveryLetterOutItemValueObject>
                {
                    new GarmentSubconDeliveryLetterOutItemValueObject
                    {
                        Product = new Product(1, "ProductCode", "ProductName"),
                        Quantity=1,
                        DesignColor= "ColorD",
                        SubconDeliveryLetterOutId=Guid.NewGuid(),
                        FabricType="test",
                        ProductRemark="test",
                        UENItemId=1,
                        Uom=new Uom(1,"UomUnit"),
                        UomOut=new Uom(1,"UomUnit"),
                        ContractQuantity=1,
                        SubconCuttingOutId=subconCuttingOutGuid,
                        RONo="ro",
                        POSerialNumber="aa",
                        SubconCuttingOutNo="no"
                    }
                },
            };
            UpdateGarmentSubconDeliveryLetterOutCommand.SetIdentity(SubconDeliveryLetterOutGuid);

            _mockSubconDeliveryLetterOutRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>()
               {
                    new GarmentSubconDeliveryLetterOut(SubconDeliveryLetterOutGuid,"","",Guid.NewGuid(),"","SUBCON CUTTING",DateTimeOffset.Now,1,"","",1,"",false).GetReadModel()
               }.AsQueryable());
            _mockSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconDeliveryLetterOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSubconDeliveryLetterOutItem>()
                {
                    new GarmentSubconDeliveryLetterOutItem(Guid.Empty,SubconDeliveryLetterOutGuid,1,new ProductId(1),"code","name","remark","color",1,new UomId(1),"unit",new UomId(1),"unit","fabType",subconCuttingOutGuid,"","","")
                });

            GarmentSubconCuttingOut garmentSubconCuttingOut = new GarmentSubconCuttingOut(subconCuttingOutGuid, "no", "", new UnitDepartmentId(1), "", "", DateTimeOffset.Now, "ro", "", new GarmentComodityId(1), "", "", 1, 1, "", false);

            _mockSubconCuttingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingOutReadModel>
                {
                    garmentSubconCuttingOut.GetReadModel()
                }.AsQueryable());


            _mockSubconDeliveryLetterOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOut>()));
            _mockSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOutItem>()));

            _mockSubconCuttingOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconCuttingOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconCuttingOut>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentSubconDeliveryLetterOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
