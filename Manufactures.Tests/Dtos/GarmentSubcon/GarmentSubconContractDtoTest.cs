using Manufactures.Domain.GarmentSubcon.SubconContracts;
using Manufactures.Dtos.GarmentSubcon;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Dtos.GarmentSubcon
{
    public class GarmentSubconContractDtoTest
    {
        [Fact]
        public void should_Success_Instantiate()
        {
            Guid id = Guid.NewGuid();
            var dto = new GarmentSubconContractDto(new GarmentSubconContract(id,"type", "SubconContractNo", "no", new Domain.Shared.ValueObjects.SupplierId(1), "Code", "Name","type","No", "type",1, DateTimeOffset.Now, DateTimeOffset.Now,false));

            Assert.NotNull(dto.Supplier);

        }
    }
}
