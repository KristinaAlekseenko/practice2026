using System;
using System.Collections.Concurrent;
using System.Threading;

namespace task17
{
    public class ServerThread
    {
        private readonly BlockingCollection<ICommand> _taskQueue = new BlockingCollection<ICommand>();
        private readonly IScheduler _scheduler;
        private readonly Thread _workerThread;
        private Action _currentAction;
        private volatile bool _shouldStop = false;

        public Thread Thread => _workerThread;

        public ServerThread(IScheduler scheduler)
        {
            _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            _currentAction = DefaultBehavior;
            _workerThread = new Thread(Run);
        }

        public void Add(ICommand cmd)
        {
            try
            {
                _taskQueue.Add(cmd);
            }
            catch (InvalidOperationException)
            {
            }
        }

        public void HardStop()
        {
            _shouldStop = true;
            _taskQueue.CompleteAdding();
        }

        public void SoftStop()
        {
            _taskQueue.CompleteAdding();
            UpdateBehavior(() =>
            {
                if (_scheduler.HasCommand())
                {
                    ExecuteCommand(_scheduler.Select());
                    return;
                }

                if (_taskQueue.TryTake(out ICommand cmd))
                {
                    ExecuteCommand(cmd);
                    return;
                }

                HardStop();
            });
        }

        private void DefaultBehavior()
        {
            try
            {
                if (_scheduler.HasCommand())
                {
                    ExecuteCommand(_scheduler.Select());
                    return;
                }

                if (_taskQueue.TryTake(out ICommand cmd, 100))
                {
                    ExecuteCommand(cmd);
                }
            }
            catch (InvalidOperationException)
            {
                HardStop();
            }
        }

        private void ExecuteCommand(ICommand cmd)
        {
            try
            {
                cmd.Execute();
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(cmd, ex);
            }
        }

        public void UpdateBehavior(Action nextBehavior)
        {
            _currentAction = nextBehavior ?? throw new ArgumentNullException(nameof(nextBehavior));
        }

        public void Join()
        {
            _workerThread.Join();
        }

        private void Run()
        {
            while (!_shouldStop)
            {
                _currentAction();
            }
        }

        public void Start()
        {
            _workerThread.Start();
        }
    }
}
