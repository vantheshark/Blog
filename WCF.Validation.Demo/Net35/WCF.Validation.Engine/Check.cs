using System;

namespace WCF.Validation.Engine
{
    public static class Check
    {
        public static void Requires(bool condition)
        {
            Requires(condition, "Condition not match");
        }

        public static void Requires(bool condition, string userMessage)
        {
            if (!condition)
            {
                throw new Exception(userMessage);
            }
        }

        public static void Requires<TException>(bool condition) where TException : Exception
        {
            if (!condition)
            {
                throw Activator.CreateInstance<TException>();
            }
        }

        public static void Requires<TException>(bool condition, string userMessage) where TException : Exception
        {
            if (!condition)
            {
                throw (Exception) Activator.CreateInstance(typeof (TException), userMessage);
            }
        }
    }
}
