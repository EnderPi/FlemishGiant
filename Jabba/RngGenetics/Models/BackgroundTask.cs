using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RngGenetics.Models
{
    /// <summary>
    /// Model that holds a background task.
    /// </summary>
    public class BackgroundTask
    {
        /// <summary>
        /// Primary key in database
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// A detailed description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// The .NET type of the task.
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// The xml-serialized task object.
        /// </summary>
        public string SerializedTask {  get; set; }
        /// <summary>
        /// True if finished, false otherwise.
        /// </summary>
        public bool Finished { get; set; }
        /// <summary>
        /// True if currently running, false otherwise
        /// </summary>
        public bool InProgress { get; set; }
        /// <summary>
        /// Where the backup/state is stored.
        /// </summary>
        public string FileName { get; set; }


    }
}
