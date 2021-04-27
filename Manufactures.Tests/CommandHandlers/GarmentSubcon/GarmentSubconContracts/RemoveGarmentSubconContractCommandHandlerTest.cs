﻿using Barebone.Tests;
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
using Manufactures.Domain.GarmentSubcon.SubconContracts;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.GarmentSubcon.SubconContracts.ReadModels;
using System.Linq;
using FluentAssertions;

namespace Manufactures.Tests.CommandHandlers.GarmentSubcon.GarmentSubconContracts
{
    public class RemoveGarmentSubconContractCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSubconContractRepository> _mockSubconContractRepository;

        public RemoveGarmentSubconContractCommandHandlerTest()
        {
            _mockSubconContractRepository = CreateMock<IGarmentSubconContractRepository>();

            _MockStorage.SetupStorage(_mockSubconContractRepository);
        }

        private RemoveGarmentSubconContractCommandHandler CreateRemoveGarmentSubconContractCommandHandler()
        {
            return new RemoveGarmentSubconContractCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid SubconContractGuid = Guid.NewGuid();
            RemoveGarmentSubconContractCommandHandler unitUnderTest = CreateRemoveGarmentSubconContractCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentSubconContractCommand RemoveGarmentSubconContractCommand = new RemoveGarmentSubconContractCommand(SubconContractGuid);
            
            GarmentSubconContract garmentSubconContract = new GarmentSubconContract(
                SubconContractGuid, null, null, null, new SupplierId(1), "", "", null, null, null, 1, DateTimeOffset.Now, DateTimeOffset.Now , false);

            _mockSubconContractRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconContractReadModel>()
                {
                    garmentSubconContract.GetReadModel()
                }.AsQueryable());

            _mockSubconContractRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconContract>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconContract>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(RemoveGarmentSubconContractCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
