using System;
using System.Runtime.Serialization;

namespace rm.Extensions
{
    /// <summary>
    /// The exception that is thrown when the object is empty.
    /// </summary>
    [Serializable]
    public class EmptyException : Exception
    {
        public EmptyException() { }
        public EmptyException(string message)
            : base(message) { }
        public EmptyException(string message, Exception inner)
            : base(message, inner) { }
        protected EmptyException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
