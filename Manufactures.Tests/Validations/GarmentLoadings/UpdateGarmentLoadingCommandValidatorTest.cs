using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentLoadings.Commands;
using Manufactures.Domain.GarmentLoadings.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentLoadings
{
    public class UpdateGarmentLoadingCommandValidatorTest
    {
        private UpdateGarmentLoadingCommandValidator GetValidationRules()
        {
            return new UpdateGarmentLoadingCommandValidator();
        }

        [Fact]
        public void Place_HaveError()
        {
            // Arrange
            var unitUnderTest = new UpdateGarmentLoadingCommand();

            // Action
            var validator = GetValidationRules();
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldHaveError();
        }

        [Fact]
        public void Place_HaveError_Date()
        {
            // Arrange
            var validator = GetValidationRules();
            var unitUnderTest = new UpdateGarmentLoadingCommand();
            unitUnderTest.LoadingDate = DateTimeOffset.Now.AddDays(-7);
            unitUnderTest.SewingDODate = DateTimeOffset.Now;

            // Action
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldHaveError();
        }

        [Fact]
        public void Place_NotHaveError()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var unitUnderTest = new UpdateGarmentLoadingCommand()
            {
                Article = "Article",
                LoadingDate = DateTimeOffset.Now,
                SewingDODate= DateTimeOffset.Now,
                Comodity = new GarmentComodity()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name",
                },
                LoadingNo = "LoadingNo",
                RONo = "RONo",
                SewingDOId = id,
                SewingDONo = "SewingDONo",
                Unit = new UnitDepartment()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name",
                },
                UnitFrom = new UnitDepartment()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name",
                },
                Items = new List<GarmentLoadingItemValueObject>()
                {
                    new GarmentLoadingItemValueObject()
                    {
                        BasicPrice=1,
                        Color ="Color",
                        DesignColor ="DesignColor",
                        IsSave =true,
                        LoadingId =id,
                        Price =1,
                        Product =new Product()
                        {
                            Id=1,
                            Code ="Code",
                            Name ="Name"
                        },
                        Quantity =1,
                        RemainingQuantity =2,
                        SewingDOItemId =id,
                        SewingDORemainingQuantity =2,
                        Size =new SizeValueObject()
                        {
                            Id =1,
                            Size="Size"
                        },
                        Uom =new Uom()
                        {
                            Id =1,
                            Unit ="unit"
                        }
                    }
                }
            };
            // Act
            var validator = GetValidationRules();
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldNotHaveError();
        }
    }
}
