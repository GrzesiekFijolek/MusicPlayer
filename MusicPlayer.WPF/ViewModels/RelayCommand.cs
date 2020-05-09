using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MusicPlayer.WPF.ViewModels
{
    /// <summary>
    /// a basic command that runs an action
    /// </summary>
    class RelayCommand : ICommand
    {
        /// <summary>
        /// an action to run
        /// </summary>
        private Action _action;

        /// <summary>
        /// it decides if it is possible to execute an action
        /// </summary>
        private Func<bool> _canExecuteEvaluator;

        public event EventHandler CanExecuteChanged = (sender, e) => { };

        /// <summary>
        /// basic constructor
        /// </summary>
        /// <param name="action">an action to run</param>
        /// <param name="canExecuteEvaluator">determines whether action will be executed</param>
        public RelayCommand(Action action, Func<bool> canExecuteEvaluator)
        {
            _action = action;
            _canExecuteEvaluator = canExecuteEvaluator;
        }

        /// <summary>
        /// constructor for commands which can always be executed
        /// </summary>
        /// <param name="action"></param>
        public RelayCommand(Action action) : this(action, null) { }

        public bool CanExecute(object parameter)
        {
            return _canExecuteEvaluator == null ? true : _canExecuteEvaluator.Invoke();
        }

        public void Execute(object parameter)
        {
            _action();
        }
    }
}
