using Barebone.Tests;
using Manufactures.Application.GarmentSubcon.GarmentSubconContracts.CommandHandlers;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Commands;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Manufactures.Domain.GarmentSubcon.SubconContracts.ReadModels;
using Manufactures.Domain.GarmentSubcon.SubconContracts;
using System.Linq;
using FluentAssertions;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Tests.CommandHandlers.GarmentSubcon.GarmentSubconContracts
{
    public class UpdateGarmentSubconContractCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSubconContractRepository> _mockSubconContractRepository;

        public UpdateGarmentSubconContractCommandHandlerTest()
        {
            _mockSubconContractRepository = CreateMock<IGarmentSubconContractRepository>();

            _MockStorage.SetupStorage(_mockSubconContractRepository);
        }
        private UpdateGarmentSubconContractCommandHandler CreateUpdateGarmentSubconContractCommandHandler()
        {
            return new UpdateGarmentSubconContractCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid sewingInItemGuid = Guid.NewGuid();
            Guid SubconContractGuid = Guid.NewGuid();
            Guid sewingInId = Guid.NewGuid();
            UpdateGarmentSubconContractCommandHandler unitUnderTest = CreateUpdateGarmentSubconContractCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateGarmentSubconContractCommand UpdateGarmentSubconContractCommand = new UpdateGarmentSubconContractCommand()
            {
                AgreementNo = "test",
                BPJNo = "test",
                ContractNo = "test",
                ContractType = "test",
                DueDate = DateTimeOffset.Now,
                FinishedGoodType = "test",
                JobType = "test",
                Quantity = 1,
                Supplier = new Supplier
                {
                    Code = "test",
                    Id = 1,
                    Name = "test"
                }

            };
            UpdateGarmentSubconContractCommand.SetIdentity(SubconContractGuid);

            _mockSubconContractRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconContractReadModel>()
                {
                    new GarmentSubconContractReadModel(SubconContractGuid)
                }.AsQueryable());
            _mockSubconContractRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconContract>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconContract>()));
            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentSubconContractCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}