using System;

namespace rm.Extensions
{
    /// <summary>
    /// Exception helper class.
    /// </summary>
    internal class ExceptionHelper
    {
        /// <summary>
        /// Throw exception of type T with message exMessage if throwEx is true.
        /// </summary>
        /// <remarks>Uses reflection to create exception instance.</remarks>
        internal static void Throw<T>(bool throwEx, string exMessage)
            where T : Exception
        {
            if (throwEx)
            {
                throw (Exception)Activator.CreateInstance(typeof(T), exMessage);
            }
        }
    }
}
