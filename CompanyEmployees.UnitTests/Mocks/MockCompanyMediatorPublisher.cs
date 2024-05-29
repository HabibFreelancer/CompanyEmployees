using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.UnitTests.Mocks
{
    public class MockCompanyMediatorPublisher
    {
        public static Mock<IPublisher> GetMockCompanyMediatorPublisher()
        {
            var mockCompanyPublisher = new Mock<IPublisher>();
            return mockCompanyPublisher;
        }
    }
}
