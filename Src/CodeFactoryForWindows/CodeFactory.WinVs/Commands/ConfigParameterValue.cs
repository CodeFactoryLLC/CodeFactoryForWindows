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
        private string _stringDefaultValue;
        private bool? _boolValue;
        private bool? _boolDefaultValue;
        private ObservableCollection<string> _listValue = new ObservableCollection<string>();
        private ObservableCollection<string> _listDefaultValue = new ObservableCollection<string>();
        private DateTime? _dateTimeValue;
        private DateTime? _dateTimeDefaultValue;


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
        /// The string default value that is assigned to the parameter. This will be null if the value is not a string.
        /// </summary>
        [Key(2)]
        public string StringDefaultValue
        {
            get => _stringDefaultValue;
            set { _stringDefaultValue = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The boolean value that is assigned to the parameter. This will be null if the value is not a boolean.
        /// </summary>
        [Key(3)]
        public bool? BoolValue
        {
            get => _boolValue;
            set { _boolValue = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The boolean default value that is assigned to the parameter. This will be null if the value is not a boolean.
        /// </summary>
        [Key(4)]
        public bool? BoolDefaultValue
        {
            get => _boolDefaultValue;
            set { _boolDefaultValue = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The list of string values that are assigned to the parameter. This will be null if the value is not a list.
        /// </summary>
        [Key(5)]
        public ObservableCollection<string> ListValue
        {
            get => _listValue;
            set { _listValue = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The list of string default values that are assigned to the parameter. This will be null if the value is not a list.
        /// </summary>
        [Key(6)]
        public ObservableCollection<string> ListDefaultValue
        {
            get => _listDefaultValue;
            set { _listDefaultValue = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The date time value that is assigned to the parameter. This will be null if the value is not a date time.
        /// </summary>
        [Key(7)]
        public DateTime? DateTimeValue
        {
            get => _dateTimeValue;
            set { _dateTimeValue = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The date time value that is assigned to the parameter. This will be null if the value is not a date time.
        /// </summary>
        [Key(8)]
        public DateTime? DateTimeDefaultValue
        {
            get => _dateTimeDefaultValue;
            set { _dateTimeDefaultValue = value; OnPropertyChanged(); }
        }
    }
}
