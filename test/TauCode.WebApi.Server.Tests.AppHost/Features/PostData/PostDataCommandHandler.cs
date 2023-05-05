using TauCode.Cqrs.Commands;

namespace TauCode.WebApi.Server.Tests.AppHost.Features.PostData;

public class PostDataCommandHandler : CommandHandler<PostDataCommand>
{
    public override void Execute(PostDataCommand command)
    {
        var greeting = $"Hello, {command.UserName}! Your birthday is {command.Birthday:yyyy-MM-dd}.";
        command.SetResult(greeting);
    }

    public override Task ExecuteAsync(PostDataCommand command, CancellationToken cancellationToken)
    {
        this.Execute(command);
        return Task.CompletedTask;
    }
}