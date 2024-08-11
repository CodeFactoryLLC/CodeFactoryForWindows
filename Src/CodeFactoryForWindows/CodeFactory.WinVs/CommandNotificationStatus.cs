using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs
{
    /// <summary>
    /// Enumeration on the current status of a command notification message.
    /// </summary>
    public enum CommandNotificationStatus
    {
        /// <summary>
        /// The beginning of a automation process has started.
        /// </summary>
        Started = 0,

        /// <summary>
        /// The automation process has completed successfuly. 
        /// </summary>
        Success = 1,

        /// <summary>
        /// The automation process has ended and has failed. 
        /// </summary>
        Failed = 2,

        /// <summary>
        /// The automation process is currently running
        /// </summary>
        Running = 3,

        /// <summary>
        /// The automation process has completed. 
        /// </summary>
        Finished = 4


    }
}
