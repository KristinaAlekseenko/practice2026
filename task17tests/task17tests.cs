using System;
using System.Threading;
using Xunit;
using task17;

namespace task17tests
{
    public class task17tests : IDisposable
    {
        private readonly ServerThread _serverThread;

        public task17tests()
        {
            _serverThread = new ServerThread();
            ExceptionHandler.Clear();
        }

        public void Dispose()
        {
            _serverThread.HardStop();
            _serverThread.Join();
        }

        [Fact]
        public void HardStop_ShouldStopProcessing_AndClearQueue()
        {
            var commandExecuted = false;
            var testCommand = new TestCommand(() => commandExecuted = true);

            _serverThread.Start();
            _serverThread.Add(testCommand);
            Thread.Sleep(200);

            _serverThread.HardStop();
            _serverThread.Join();
            Assert.True(commandExecuted);
            Assert.False(_serverThread.Thread.IsAlive);
        }

        [Fact]
        public void SoftStop_ShouldProcessRemainingCommands_ThenStop()
        {
            var commandCount = 0;
            var commands = new[]
            {
                new TestCommand(() => Interlocked.Increment(ref commandCount)),
                new TestCommand(() => Interlocked.Increment(ref commandCount)),
                new TestCommand(() => Interlocked.Increment(ref commandCount))
            };

            _serverThread.Start();
            foreach (var cmd in commands)
            {
                _serverThread.Add(cmd);
            }
            _serverThread.SoftStop();
            _serverThread.Join();
            Assert.Equal(3, commandCount);
            Assert.False(_serverThread.Thread.IsAlive);
        }

        [Fact]
        public void StopCommand_ShouldThrowException_WhenExecutedInWrongThread()
        {
            _serverThread.Start();
            var hardStopCommand = new HardStopCommand(_serverThread);
            var softStopCommand = new SoftStopCommand(_serverThread);

            Assert.Throws<InvalidOperationException>(() => hardStopCommand.Execute());
            Assert.Throws<InvalidOperationException>(() => softStopCommand.Execute());
        }

        private class TestCommand : ICommand
        {
            private readonly Action _action;

            public TestCommand(Action action)
            {
                _action = action;
            }

            public void Execute()
            {
                _action?.Invoke();
            }
        }
    }
}
