using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MainProject
{
    class RelayingCommand<T> : ICommand
    {
        #region Fields
        readonly Predicate<T> _canExecute;
        readonly Action<T> _execute;
        #endregion //Fields



        #region Constructors
        public RelayingCommand(Action<T> action) : this(action, null) { }
        /// <summary>
        /// Create new command with 2 parameter equivalent with execute and canexecute
        /// </summary>
        /// <exception cref="Exception">Throw when no execute method is called</exception>
        public RelayingCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute ==null)
            {
                throw new NullReferenceException("The Execute is null");
            }
            _execute = execute;
            _canExecute = canExecute;
        }
        #endregion //Constructors



        #region ICommand Members
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested += value; }
        }
        
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }
        #endregion //ICommand Members
    }
}
