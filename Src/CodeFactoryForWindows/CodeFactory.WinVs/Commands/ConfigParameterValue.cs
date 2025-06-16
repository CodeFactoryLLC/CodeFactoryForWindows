//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2025 CodeFactory, LLC
//*****************************************************************************
using System;
using System.Collections.ObjectModel;
using MessagePack;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// The type of value and value that is assigned to <see cref="ConfigParameter"/>.
    /// </summary>
    [MessagePackObject]
    public class ConfigParameterValue: PropertyChangedBase
    {
        //Backing fields for the properties
        private ConfigParameterValueType _valueType;
        private string _stringValue;
        private bool? _boolValue;
        private ObservableCollection<ConfigParameterListValue> _listValue = new ObservableCollection<ConfigParameterListValue>();
        private DateTime? _dateTimeValue;



        /// <summary>
        /// The type of value that is assigned to the parameter.
        /// </summary>
        [Key(0)]
        public ConfigParameterValueType ValueType
        {
            get => _valueType;
            set { _valueType = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The string value that is assigned to the parameter. This will be null if the value is not a string.
        /// </summary>
        [Key(1)]
        public string StringValue
        {
            get => _stringValue;
            set { _stringValue = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The boolean value that is assigned to the parameter. This will be null if the value is not a boolean.
        /// </summary>
        [Key(2)]
        public bool? BoolValue
        {
            get => _boolValue;
            set { _boolValue = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The list of string values that are assigned to the parameter. This will be null if the value is not a list.
        /// </summary>
        [Key(3)]
        public ObservableCollection<ConfigParameterListValue> ListValue
        {
            get => _listValue;
            set { _listValue = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The date time value that is assigned to the parameter. This will be null if the value is not a date time.
        /// </summary>
        [Key(4)]
        public DateTime? DateTimeValue
        {
            get => _dateTimeValue;
            set { _dateTimeValue = value; OnPropertyChanged(); }
        }

    }
}
