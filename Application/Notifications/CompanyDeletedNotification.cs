using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Notifications
{

    /*The notification has to inherit from the INotification interface. This is
the equivalent of the IRequest we saw earlier, but for Notifications.
As we can conclude, notifications don’t return a value. They work on the
fire and forget principle, like publishers.*/

    public sealed record CompanyDeletedNotification(Guid Id, bool TrackChanges) :
 INotification;
}
