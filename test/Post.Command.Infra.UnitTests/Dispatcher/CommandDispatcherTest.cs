using FluentAssertions;
using Moq;
using Post.Command.Infra.Dispatchers;

namespace Post.Command.Infra.UnitTests.Dispatcher
{
    public class CommandDispatcherTest
    {
        [Fact]
        public async void SendAsync_WhenCommandAreRegistered_MustCallCommandHandler()
        {
            var sampleCommand = new SampleCommand();
            var handlerMock = new Mock<Func<SampleCommand, Task>>();
            var sut = new CommandDispatcher();

            sut.RegisterHandler(handlerMock.Object);
            await sut.SendAsync(sampleCommand);

            handlerMock.Verify(h => h(sampleCommand), Times.Once);
        }

        [Fact]
        public void RegisterHandler_WhenCommandIsAlreadyRegistered_MustThrowInvalidOperationException()
        {            
            var handlerMock = new Mock<Func<SampleCommand, Task>>();
            var sut = new CommandDispatcher();

            sut.RegisterHandler(handlerMock.Object);
            
            sut.Invoking(x => x.RegisterHandler(handlerMock.Object))
               .Should().Throw<InvalidOperationException>()
               .WithMessage($"Handler { typeof(SampleCommand)} is already registered.");
        }

        [Fact]
        public async void SendAsync_WhenCommandIsNotRegistered_MustThrowArgumentNullException()
        {
            var sampleCommand = new SampleCommand();
            var handlerMock = new Mock<Func<SampleCommand, Task>>();
            var sut = new CommandDispatcher();            

            await sut.Invoking(async x => await x.SendAsync(sampleCommand))
                     .Should().ThrowAsync<ArgumentNullException>()
                     .WithMessage("Command handler not registered (Parameter 'Post.Command.Infra.UnitTests.SampleCommand')");
        }
    }
}