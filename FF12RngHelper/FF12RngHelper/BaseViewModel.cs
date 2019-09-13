using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace FF12RngHelper
{
    /// <summary>
    /// A base viewModel that fires PropertyChanged events as needed
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region Properties
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
        #endregion

        #region Methods
        /// <summary>
        /// Call this to fire a <see cref="PropertyChanged"/> event
        /// </summary>
        /// <param name="name">The name of the property to notify on</param>
        protected void NotifyPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        #region Command Helpers
        /// <summary>
        /// Run a command if the updating flag is not set
        /// If the flag is true (indicationg the funtion is already running) then the action is not run.
        /// If the flag is false (indicating no running function) then the action is run.
        /// Once the action is finished if it was run, then the flag is set to false
        /// </summary>
        /// <param name="updatingFlag">The bool property flag defining if the command is already running</param>
        /// <param name="action">The action to run if the command is not already running</param>
        /// <returns></returns>
        protected async Task RunCommandAsync(Expression<Func<bool>> updatingFlag, Func<Task> action)
        {
            // check if the flag property is true (the function is already running
            if (updatingFlag.GetPropertyValue())
                return;

            // Seet the property flag to indicate we are running
            updatingFlag.SetPropertyValue(true);

            try
            {
                await action();
            }
            finally
            {
                updatingFlag.SetPropertyValue(false);
            }
        }
        #endregion
    }
}
