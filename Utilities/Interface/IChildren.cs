using System.Collections.Generic;

namespace Utilities.Interface
{
    public interface IChildren<T>
    {
        /// <summary>
        /// Children
        /// </summary>
        List<T> Children { get; set; }
    }
}