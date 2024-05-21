//using Application.Commands;
//using Contracts;
//using Entities.Models;
//using MediatR;
//using Microsoft.AspNetCore.Identity;
//using Shared.DataTransferObjects;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Application.Handlers
//{

//    internal sealed class ValidateUserHandler : IRequestHandler<ValidateUserCommand, bool>
//    {
//        private readonly ILoggerManager _logger;
//        private readonly UserManager<User> _userManager;
//        private User? _user;

//        public ValidateUserHandler(ILoggerManager logger,
//       UserManager<User> userManager)
//        {
//            _logger = logger;
//            _userManager = userManager;
//        }

//        public async Task<bool> Handle(ValidateUserCommand request, CancellationToken cancellationToken)
//        {
//            _user = await _userManager.FindByNameAsync(request.userForAuth.UserName);
//            var result = (_user != null && await _userManager.CheckPasswordAsync(_user,
//            request.userForAuth.Password));
//            if (!result)
//                _logger.LogWarn($" ValidateUser : Authentication failed. Wrong user  name or password.");
//            return result;
//        }
//    }
//}
