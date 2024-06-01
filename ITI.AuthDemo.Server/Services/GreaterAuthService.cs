using Grpc.Core;
using ITI.AuthDemo.Server.Protos;
using Microsoft.AspNetCore.Authorization;

namespace ITI.AuthDemo.Server.Services;


public class GreaterAuthService : greaterService.greaterServiceBase
{
    [Authorize(AuthenticationSchemes = Consts.ApiKeySchemeName)]
    public override Task<GreatResponse> Great(GreatRequest request, ServerCallContext context)
    {
        return Task.FromResult(new GreatResponse { Message = $"Hello {request.Name}!" });
    }
}
