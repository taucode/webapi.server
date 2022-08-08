using TauCode.Cqrs.Commands;

namespace TauCode.WebApi.Server.Tests.AppHost.Features.PostData;

public class PostDataCommandHandler : ICommandHandler<PostDataCommand>
{
    public void Execute(PostDataCommand command)
    {
        var greeting = $"Hello, {command.UserName}! Your birthday is {command.Birthday:yyyy-MM-dd}.";
        command.SetResult(greeting);
    }

    public Task ExecuteAsync(PostDataCommand command, CancellationToken cancellationToken = new CancellationToken())
    {
        this.Execute(command);
        return Task.CompletedTask;
    }
}