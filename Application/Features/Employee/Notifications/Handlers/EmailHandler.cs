using Application.Features.Employee.Notifications;
using Application.Features.Employee.Notifications.Commands;
using Contracts;
using Entities.ConfigurationModels.Email;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    partial  class EmailHandler : INotificationHandler<EmployeeDeletedNotification>
    {
        private readonly ILoggerManager<EmailHandler> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EmailHandler(ILoggerManager<EmailHandler> logger,
             IEmailSender emailSender,
             IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Handle(EmployeeDeletedNotification notification,
        CancellationToken cancellationToken)
        {
            _logger.LogWarn($"Delete action for the employee with id: {notification.id} has occurred.");

            /* try
             {
                 var emailAddress = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;

                 var email = new Email
                 {
                     To = emailAddress,
                     Body = $"Delete action for the employee with id: {notification.id} has occurred.",
                     Subject = "Delete employee has occurred"
                 };

                 await _emailSender.SendEmail(email);
             }
             catch (Exception ex)
             {
                 string exceptionMessage = ex.InnerException == null ? ex.Message : ex.Message + "\r\n" + ex.InnerException.Message;

                 _logger.LogError($"Delete action for the employee with id: {notification.id} has an exception : {exceptionMessage}.");
             }

             await Task.CompletedTask;
            */
        }

    }
}
