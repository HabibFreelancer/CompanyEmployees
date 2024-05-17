using AutoMapper;
using Contracts;
using Entities.ConfigurationModels;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Service.Contracts;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<IAuthenticationService> _authenticationService;

        public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager
        logger, IMapper mapper, IDataShaper<EmployeeDto> dataShaper,
            UserManager<User> userManager,
        IOptions<JwtConfiguration> configuration)
        {
          
          
            _authenticationService = new Lazy<IAuthenticationService>(() =>
                            new AuthenticationService(logger, mapper, userManager, configuration));

        }
      
        
        public IAuthenticationService AuthenticationService => _authenticationService.Value;

    }
}
