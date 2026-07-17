using System.Collections.Generic;
using System.Linq;

namespace task17
{
    public class Scheduler : IScheduler
    {
        private readonly LinkedList<ICommand> _commands = new LinkedList<ICommand>();
        private LinkedListNode<ICommand> _current;
        private readonly object _lock = new object();

        public bool HasCommand()
        {
            lock (_lock)
            {
                return _commands.Count > 0;
            }
        }

        public ICommand Select()
        {
            lock (_lock)
            {
                if (_commands.Count == 0)
                    return null;

                if (_current == null)
                    _current = _commands.First;

                var command = _current.Value;
                _current = _current.Next ?? _commands.First;
                return command;
            }
        }

        public void Add(ICommand cmd)
        {
            lock (_lock)
            {
                _commands.AddLast(cmd);
            }
        }
    }
}
