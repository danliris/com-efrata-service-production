using Barebone.Tests;
using Manufactures.Controllers.Api.GarmentSubcon;
using Manufactures.Domain.GarmentSubcon.SubconContracts;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Commands;
using Manufactures.Domain.GarmentSubcon.SubconContracts.ReadModels;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Controllers.Api.GarmentSubcon
{
    public class GarmentSubconContractControllerTest : BaseControllerUnitTest
    {
        private readonly Mock<IGarmentSubconContractRepository> _mockGarmentSubconContractRepository;

        public GarmentSubconContractControllerTest() : base()
        {
            _mockGarmentSubconContractRepository = CreateMock<IGarmentSubconContractRepository>();

            _MockStorage.SetupStorage(_mockGarmentSubconContractRepository);
        }

        private GarmentSubconContractController CreateGarmentSubconContractController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentSubconContractController controller = (GarmentSubconContractController)Activator.CreateInstance(typeof(GarmentSubconContractController), _MockServiceProvider.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user.Object
                }
            };
            controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Bearer unittesttoken";
            controller.ControllerContext.HttpContext.Request.Headers["x-timezone-offset"] = "7";
            controller.ControllerContext.HttpContext.Request.Path = new PathString("/v1/unit-test");
            return controller;
        }

        [Fact]
        public async Task Get_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconContractController();

            _mockGarmentSubconContractRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentSubconContractReadModel>().AsQueryable());

            Guid SubconContractGuid = Guid.NewGuid();
            _mockGarmentSubconContractRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSubconContractReadModel>>()))
                .Returns(new List<GarmentSubconContract>()
                {
                    new GarmentSubconContract(SubconContractGuid, "","","", new SupplierId(1),"","","","","",1,DateTimeOffset.Now,DateTimeOffset.Now,false)
                });

            // Act
            var result = await unitUnderTest.Get();

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetSingle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconContractController();
            Guid SubconContractGuid = Guid.NewGuid();
            _mockGarmentSubconContractRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconContractReadModel, bool>>>()))
                .Returns(new List<GarmentSubconContract>()
                {
                    new GarmentSubconContract(SubconContractGuid, "","","", new SupplierId(1),"","","","","",1,DateTimeOffset.Now,DateTimeOffset.Now,false)
                });

            // Act
            var result = await unitUnderTest.Get(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconContractController();
            Guid SubconContractGuid = Guid.NewGuid();
            //_mockGarmentSubconContractRepository
            //    .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconContractReadModel, bool>>>()))
            //    .Returns(new List<GarmentSubconContract>());
            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentSubconContractCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSubconContract(SubconContractGuid, "", "a ", "", new SupplierId(1), "", "", "", "", "", 1, DateTimeOffset.Now, DateTimeOffset.Now, false));

            // Act
            var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentSubconContractCommand>());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_Throw_Exception()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconContractController();
            Guid SubconContractGuid = Guid.NewGuid();
            //_mockGarmentSubconContractRepository
            //    .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconContractReadModel, bool>>>()))
            //    .Returns(new List<GarmentSubconContract>());

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentSubconContractCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            // Assert
            await Assert.ThrowsAsync<Exception>(() => unitUnderTest.Post(It.IsAny<PlaceGarmentSubconContractCommand>()));
        }

        [Fact]
        public async Task Put_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconContractController();
            Guid SubconContractGuid = Guid.NewGuid();
            //_mockGarmentSubconContractRepository
            //    .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconContractReadModel, bool>>>()))
            //    .Returns(new List<GarmentSubconContract>());
            _MockMediator
                .Setup(s => s.Send(It.IsAny<UpdateGarmentSubconContractCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSubconContract(SubconContractGuid, "", "", "", new SupplierId(1), "", "", "", "", "", 1, DateTimeOffset.Now, DateTimeOffset.Now, false));

            // Act
            var result = await unitUnderTest.Put(Guid.NewGuid().ToString(), new UpdateGarmentSubconContractCommand());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconContractController();
            Guid SubconContractGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentSubconContractCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSubconContract(SubconContractGuid, "", "", "", new SupplierId(1), "", "", "", "", "", 1, DateTimeOffset.Now, DateTimeOffset.Now, false));

            // Act
            var result = await unitUnderTest.Delete(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

    }
}