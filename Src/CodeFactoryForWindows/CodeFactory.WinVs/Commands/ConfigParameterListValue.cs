using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Data class that represents a list of values for a configuration parameter.
    /// </summary>
    [MessagePackObject]
    public class ConfigParameterListValue:PropertyChangedBase
    {
        //Backing field for the property
        private string _listValue;

        /// <summary>
        /// Gets or sets the value associated with the parameter list.
        /// </summary>
        [Key(0)]
        public string ListValue
        {
            get => _listValue;

            set
            {
                if (_listValue != value)
                {
                    _listValue = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
