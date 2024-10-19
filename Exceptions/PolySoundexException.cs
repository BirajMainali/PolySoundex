using System;

namespace PolySoundex.Exceptions
{
    public class PolySoundexException  : Exception
    {
        public PolySoundexException(string message) : base("PolySoundex: " + message)
        {
        }
    }
}