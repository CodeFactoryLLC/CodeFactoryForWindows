using MessagePack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Represents a configuration parameter with a selected value and a collection of available values for selection.
    /// </summary>
    /// <remarks>This class provides functionality to manage a selected value and a list of possible values
    /// for configuration purposes. It implements property change notifications to support data binding
    /// scenarios.</remarks>
    [MessagePackObject]
    public class ConfigParameterSelectedValue: PropertyChangedBase
    {
        //Backing field for the selected value.
        string _selectedValue;

        //Backing field for the list of values that are available for selection.
        ObservableCollection<ConfigParameterListValue> _selectionValues = new ObservableCollection<ConfigParameterListValue>();

        /// <summary>
        /// Gets or sets the currently selected value.
        /// </summary>
        [Key(0)]
        public string SelectedValue
        {
            get => _selectedValue;
            set { _selectedValue = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The list of values that are available for selection.
        /// </summary>
        [Key(1)]
        public ObservableCollection<ConfigParameterListValue> SelectionValues
        {
            get => _selectionValues;
            set { _selectionValues = value; OnPropertyChanged(); }
        }

    }
}
