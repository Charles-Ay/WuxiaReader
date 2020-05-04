using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WuxiaReader.Interface.Commands
{
    public class AsyncCommand : ICommand
    {
        private readonly Func<Task> _executeFunction;
        private readonly Func<bool> _canExecute;
        
        public AsyncCommand(Func<Task> execute, Func<bool> canExecute = null)
        {
            _executeFunction = execute;
            _canExecute = canExecute;
        }
        
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public async void Execute(object parameter)
        {
            await _executeFunction();
        }

        public event EventHandler CanExecuteChanged;
    }
    
    public class AsyncCommand<T> : ICommand
    {
        private readonly Func<T, Task> _executeFunction;
        private readonly Predicate<T> _canExecute;
        
        public AsyncCommand(Func<T, Task> execute, Predicate<T> canExecute = null)
        {
            _executeFunction = execute;
            _canExecute = canExecute;
        }
        
        public bool CanExecute(object parameter)
        {
            if (!(parameter is T param))
                throw new ArgumentException(nameof(parameter));
            
            return _canExecute == null || _canExecute(param);
        }

        public async void Execute(object parameter)
        {
            if (!(parameter is T param))
                throw new ArgumentException(nameof(parameter));
            
            await _executeFunction(param);
        }

        public event EventHandler CanExecuteChanged;
    }
}