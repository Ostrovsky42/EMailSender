using System;
using Serilog;

namespace EmailWorker
{
    public static class EnviromentChecker
    {
        public static string GetEnviromentVariable(string variable)
        {
            if (variable == null)
            {
                Log.Error("Environment variable is null");
                throw new ArgumentException("Environment variable is null");
            }

            Log.Information($"Value of environment variable [{variable}]");
            return variable;
        }
    }
}