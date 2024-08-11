using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs
{
    /// <summary>
    /// Central notification manager for all CodeFactory notifications for executing commands. 
    /// </summary>
    public static class CommandNotifications
    {

        /// <summary>
        /// Event that notifies subscribing commands a notification message has arrived. 
        /// </summary>
        public static event EventHandler<CommandNotificationMessage> CommandNotification;

        /// <summary>
        /// Send a notification to subscribing commands.
        /// </summary>
        /// <param name="status">The status of the notification.</param>
        /// <param name="category">The category the notification belongs to.</param>
        /// <param name="message">the message to be notified to the command.</param>
        public static Task SendCommandNotificationAsync(CommandNotificationStatus status, string category, string message)
        {

            var notification = CommandNotificationMessage.Init(message, category, status);

            RaiseNotification(notification);

            return Task.CompletedTask;

        }

        /// <summary>
        /// Raises the event <see cref="CommandNotification"/> if there are any subscribers to the event.
        /// </summary>
        /// <param name="message">The notification message to be provided to the subscriber.</param>
        private static void RaiseNotification(CommandNotificationMessage message)
        {
            CommandNotification?.Invoke(null, message);
        }
    }
}
