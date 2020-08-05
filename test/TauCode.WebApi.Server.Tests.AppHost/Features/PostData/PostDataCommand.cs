using System;
using TauCode.Cqrs.Commands;

namespace TauCode.WebApi.Server.Tests.AppHost.Features.PostData
{
    public class PostDataCommand : Command<string>
    {
        public string UserName { get; set; }
        public DateTime Birthday { get; set; }
    }
}
