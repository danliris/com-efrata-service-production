using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Tests.Validations.GarmentSubcon.GarmentSubconContracts
{
    public class PlaceGarmentSubconContractValidationTest
    {
        private PlaceGarmentSubconContractCommandValidator GetValidationRules()
        {
            return new PlaceGarmentSubconContractCommandValidator();
        }

        [Fact]
        public void Place_ShouldHaveError()
        {
            // Arrange

            var unitUnderTest = new PlaceGarmentSubconContractCommand();

            // Action
            var validator = GetValidationRules();
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldHaveError();
        }

        [Fact]
        public void Place_ShouldNotHaveError()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var unitUnderTest = new PlaceGarmentSubconContractCommand()
            {
                AgreementNo = "test",
                BPJNo = "test",
                ContractNo = "test",
                ContractType = "test",
                DueDate = DateTimeOffset.Now,
                FinishedGoodType = "test",
                JobType = "test",
                Quantity = 1,
                ContractDate = DateTimeOffset.Now,
                Supplier = new Supplier
                {
                    Code = "test",
                    Id = 1,
                    Name = "test"
                },
                IsUsed=false
            };
            // Action
            var validator = GetValidationRules();
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldNotHaveError();

        }

    }
}
