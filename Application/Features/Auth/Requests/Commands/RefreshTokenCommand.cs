using MediatR;
using Shared.DataTransferObjects.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Auth.Requests.Commands
{
    public sealed record RefreshTokenCommand(TokenDto tokenDto) : IRequest<TokenDto>;

}
