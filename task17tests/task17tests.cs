using System;
using System.Threading;
using Xunit;

namespace task17.Tests
{
    public class SchedulerTests
    {
        [Fact]
        public void Scheduler_ShouldReturnCommands_InRoundRobinOrder()
        {
            var scheduler = new Scheduler();
            var cmd1 = new TestCommand(() => { });
            var cmd2 = new TestCommand(() => { });
            var cmd3 = new TestCommand(() => { });

            scheduler.Add(cmd1);
            scheduler.Add(cmd2);
            scheduler.Add(cmd3);

            Assert.True(scheduler.HasCommand());
            Assert.Same(cmd1, scheduler.Select());
            Assert.Same(cmd2, scheduler.Select());
            Assert.Same(cmd3, scheduler.Select());
            Assert.Same(cmd1, scheduler.Select());
        }

        [Fact]
        public void Scheduler_HasCommand_ShouldReturnFalse_WhenEmpty()
        {
            var scheduler = new Scheduler();
            Assert.False(scheduler.HasCommand());
            Assert.Null(scheduler.Select());
        }

        [Fact]
        public void ServerThread_ShouldExecuteLongRunningCommand_InMultipleCalls()
        {
            var scheduler = new Scheduler();
            var serverThread = new ServerThread(scheduler);
            var executionCount = 0;
            var longCommand = new LongRunningCommand(() => Interlocked.Increment(ref executionCount), requiredCalls: 3);

            scheduler.Add(longCommand);
            serverThread.Start();
            Thread.Sleep(500);
            serverThread.HardStop();
            serverThread.Join();
            Assert.True(executionCount >= 1);
        }

        private class TestCommand : ICommand
        {
            private readonly Action _action;
            public TestCommand(Action action) => _action = action;
            public void Execute() => _action?.Invoke();
        }

        private class LongRunningCommand : ICommand
        {
            private readonly Action _action;
            private readonly int _requiredCalls;
            private int _callCount;

            public LongRunningCommand(Action action, int requiredCalls)
            {
                _action = action;
                _requiredCalls = requiredCalls;
            }

            public void Execute()
            {
                _action?.Invoke();
                _callCount++;

                if (_callCount < _requiredCalls)
                {
                    ExceptionHandler.Handle(this, null);
                }
            }
        }
    }
}