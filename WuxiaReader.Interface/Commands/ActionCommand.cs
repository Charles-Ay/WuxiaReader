using System;
using System.Windows.Input;

namespace WuxiaReader.Interface.Commands
{
    public class ActionCommand : ICommand
    {
        private readonly Action _executeFunction;
        private readonly Func<bool> _canExecute;
        
        public ActionCommand(Action execute, Func<bool> canExecute = null)
        {
            _executeFunction = execute;
            _canExecute = canExecute;
        }
        
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter)
        {
            _executeFunction();
        }

        public void UpdateCanExecuteState()
        {
            CanExecuteChanged?.Invoke(this, null);
        }

        public event EventHandler CanExecuteChanged;
    }
    
    public class ActionCommand<T> : ICommand
    {
        private readonly Action<T> _executeFunction;
        private readonly Predicate<T> _canExecute;
        
        public ActionCommand(Action<T> execute, Predicate<T> canExecute = null)
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

        public void Execute(object parameter)
        {
            if (!(parameter is T param))
                throw new ArgumentException(nameof(parameter));
            
            _executeFunction(param);
        }

        public void UpdateCanExecuteState()
        {
            CanExecuteChanged?.Invoke(this, null);
        }

        public event EventHandler CanExecuteChanged;
    }
}