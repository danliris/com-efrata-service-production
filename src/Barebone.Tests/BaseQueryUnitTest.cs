using ExtCore.Data.Abstractions;
using Moq;
using System;

namespace Barebone.Tests
{
    public abstract class BaseQueryUnitTest : IDisposable
    {
        protected readonly MockRepository _MockRepository;

        protected readonly Mock<IStorage> _MockStorage;

        protected BaseQueryUnitTest()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);
            _MockStorage = _MockRepository.Create<IStorage>();
        }

        public Mock<TAbstract> CreateMock<TAbstract>() where TAbstract : class
        {
            return _MockRepository.Create<TAbstract>();
        }

        public void Dispose()
        {
            _MockRepository.VerifyAll();
        }
    }
}
