using Barebone.Tests;
using Manufactures.Application.GarmentSubcon.GarmentSubconDeliveryLetterOuts.CommandHandlers;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Commands;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ReadModels;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Manufactures.Domain.Shared.ValueObjects;
using FluentAssertions;
using Manufactures.Domain.GarmentSubconCuttingOuts.Repositories;
using Manufactures.Domain.GarmentSubconCuttingOuts;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;

namespace Manufactures.Tests.CommandHandlers.GarmentSubcon.GarmentSubconDeliveryLetterOuts
{
    public class RemoveGarmentSubconDeliveryLetterOutCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSubconDeliveryLetterOutRepository> _mockSubconDeliveryLetterOutRepository;
        private readonly Mock<IGarmentSubconDeliveryLetterOutItemRepository> _mockSubconDeliveryLetterOutItemRepository;
        private readonly Mock<IGarmentSubconCuttingOutRepository> _mockSubconCuttingOutRepository;

        public RemoveGarmentSubconDeliveryLetterOutCommandHandlerTests()
        {
            _mockSubconDeliveryLetterOutRepository = CreateMock<IGarmentSubconDeliveryLetterOutRepository>();
            _mockSubconDeliveryLetterOutItemRepository = CreateMock<IGarmentSubconDeliveryLetterOutItemRepository>();
            _mockSubconCuttingOutRepository = CreateMock<IGarmentSubconCuttingOutRepository>();

            _MockStorage.SetupStorage(_mockSubconDeliveryLetterOutRepository);
            _MockStorage.SetupStorage(_mockSubconDeliveryLetterOutItemRepository);
            _MockStorage.SetupStorage(_mockSubconCuttingOutRepository);
        }

        private RemoveGarmentSubconDeliveryLetterOutCommandHandler CreateRemoveGarmentSubconDeliveryLetterOutCommandHandler()
        {
            return new RemoveGarmentSubconDeliveryLetterOutCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_BB()
        {
            // Arrange
            Guid subconDeliveryLetterOutGuid = Guid.NewGuid();
            RemoveGarmentSubconDeliveryLetterOutCommandHandler unitUnderTest = CreateRemoveGarmentSubconDeliveryLetterOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentSubconDeliveryLetterOutCommand RemoveGarmentSubconDeliveryLetterOutCommand = new RemoveGarmentSubconDeliveryLetterOutCommand(subconDeliveryLetterOutGuid);

            _mockSubconDeliveryLetterOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>()
                {
                    new GarmentSubconDeliveryLetterOutReadModel(subconDeliveryLetterOutGuid)
                }.AsQueryable());
            _mockSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconDeliveryLetterOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSubconDeliveryLetterOutItem>()
                {
                    new GarmentSubconDeliveryLetterOutItem(Guid.Empty,subconDeliveryLetterOutGuid,1,new ProductId(1),"code","name","remark","color",1,new UomId(1),"unit",new UomId(1),"unit","fabType",new Guid(),"","","")
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
            var result = await unitUnderTest.Handle(RemoveGarmentSubconDeliveryLetterOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_CT()
        {
            // Arrange
            Guid subconCuttingOutGuid = Guid.NewGuid();
            Guid subconDeliveryLetterOutGuid = Guid.NewGuid();
            RemoveGarmentSubconDeliveryLetterOutCommandHandler unitUnderTest = CreateRemoveGarmentSubconDeliveryLetterOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentSubconDeliveryLetterOutCommand RemoveGarmentSubconDeliveryLetterOutCommand = new RemoveGarmentSubconDeliveryLetterOutCommand(subconDeliveryLetterOutGuid);

            GarmentSubconCuttingOut garmentSubconCuttingOut = new GarmentSubconCuttingOut(subconCuttingOutGuid, "no", "", new UnitDepartmentId(1), "", "", DateTimeOffset.Now, "ro", "", new GarmentComodityId(1), "", "", 1, 1, "", false);
            _mockSubconCuttingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingOutReadModel>
                {
                    garmentSubconCuttingOut.GetReadModel()
                }.AsQueryable());
            _mockSubconDeliveryLetterOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>()
                {
                    new GarmentSubconDeliveryLetterOut(subconDeliveryLetterOutGuid,"","",Guid.NewGuid(),"","SUBCON CUTTING",DateTimeOffset.Now,1,"","",1,"",false).GetReadModel()
                }.AsQueryable());
            _mockSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconDeliveryLetterOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSubconDeliveryLetterOutItem>()
                {
                    new GarmentSubconDeliveryLetterOutItem(Guid.Empty,subconDeliveryLetterOutGuid,1,new ProductId(1),"code","name","remark","color",1,new UomId(1),"unit",new UomId(1),"unit","fabType",subconCuttingOutGuid,"","","")
                });

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
            var result = await unitUnderTest.Handle(RemoveGarmentSubconDeliveryLetterOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
