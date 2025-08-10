//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2025 CodeFactory, LLC
//*****************************************************************************
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Base class that implements the INotifyPropertyChanged interface to support property changed events.
    /// </summary>
    public abstract class PropertyChangedBase:INotifyPropertyChanged
    {
        /// <summary>
        /// Event that is raised when a property has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Method to raise the property changed event.
        /// </summary>
        /// <param name="propertyName">Name of the property that has changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
    }
}
