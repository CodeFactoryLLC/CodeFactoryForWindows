using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs
{
    /// <summary>
    /// Immutable command notification message.
    /// </summary>
    public class CommandNotificationMessage
    {
        /// <summary>
        /// Backing field for the message property.
        /// </summary>
        private readonly string _message;

        /// <summary>
        /// Backing field for the category property.
        /// </summary>
        private readonly string _category;

        /// <summary>
        /// Backing field for the status property.
        /// </summary>
        private readonly CommandNotificationStatus _status;

        /// <summary>
        /// Initializes the <see cref="CommandNotificationMessage"/>
        /// </summary>
        /// <param name="message">The message to be returned.</param>
        /// <param name="category">What category the message belongs to.</param>
        /// <param name="status">The status of the message.</param>
        protected CommandNotificationMessage(string message, string category, CommandNotificationStatus status)
        {
            _message = message;
            _category = category;
            _status = status;
        }

        /// <summary>
        /// The message to be provided to the executing command. 
        /// </summary>
        public string Message => _message;

        /// <summary>
        /// The category the message belongs to.
        /// </summary>
        public string Category => _category;

        /// <summary>
        /// The current status of the notification message. 
        /// </summary>
        public CommandNotificationStatus Status => _status;

        /// <summary>
        /// Initializes a instance of the <see cref="CommandNotificationMessage"/>
        /// </summary>
        /// <param name="message">The message to be returned.</param>
        /// <param name="category">What category the message belongs to.</param>
        /// <param name="status">The status of the message.</param>
        /// <returns>The created instance.</returns>
        public static CommandNotificationMessage Init(string message, string category, CommandNotificationStatus status)
        {
            return new CommandNotificationMessage(message, category, status);
        }
    }
}
