using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSubcon.GarmentSubconContracts.CommandHandlers;
using Manufactures.Domain.GarmentSubcon.SubconContracts;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Commands;
using Manufactures.Domain.GarmentSubcon.SubconContracts.ReadModels;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSubcon.GarmentSubconContracts
{
    public class PlaceGarmentSubconContractCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSubconContractRepository> _mockSubconContractRepository;
        public PlaceGarmentSubconContractCommandHandlerTest()
        {
            _mockSubconContractRepository = CreateMock<IGarmentSubconContractRepository>();

            _MockStorage.SetupStorage(_mockSubconContractRepository);
        }
        private PlaceGarmentSubconContractCommandHandler CreatePlaceGarmentSubconContractCommandHandler()
        {
            return new PlaceGarmentSubconContractCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid sewingInItemGuid = Guid.NewGuid();
            PlaceGarmentSubconContractCommandHandler unitUnderTest = CreatePlaceGarmentSubconContractCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentSubconContractCommand placeGarmentSubconContractCommand = new PlaceGarmentSubconContractCommand()
            {
                AgreementNo = "test",
                BPJNo = "test",
                ContractNo = "test",
                ContractType= "test",
                DueDate=DateTimeOffset.Now,
                FinishedGoodType= "test",
                JobType= "test",
                Quantity= 1,
                Supplier=new Supplier {
                    Code= "test",
                    Id=1,
                    Name= "test"
                }
            };
            _mockSubconContractRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconContractReadModel>().AsQueryable());
            _mockSubconContractRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconContract>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconContract>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentSubconContractCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}