// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace System.Threading.Tasks
{
    public static partial class TaskExtension
    {
        /// <summary>
        /// Joins the task until execution ends
        /// </summary>
        /// <returns>True if the task was completed successfully, false otherwise</returns>
        public static bool Join(this Task task)
        {
            try
            {
                task.GetAwaiter().GetResult();
            }
            catch (TaskCanceledException)
            { }
            switch (task.Status)
            {
                case TaskStatus.RanToCompletion: return true;
                default: return false;
            }
        }
        
        /// <summary>
        /// Joins the task until execution ends
        /// </summary>
        /// <returns>True if the task was completed successfully, false otherwise</returns>
        public static bool Join(this Task<bool> task)
        {
            bool result = false;
            try
            {
                 result = task.GetAwaiter().GetResult();
            }
            catch (TaskCanceledException)
            { }
            switch (task.Status)
            {
                case TaskStatus.RanToCompletion: return result;
                default: return false;
            }
        }
    }
}