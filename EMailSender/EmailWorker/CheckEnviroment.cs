using Serilog;
using System;

namespace EmailWorker
{
    public static class EnviromentChecker
    {
        public static string GetkAndLogEnviromentVariable(string variableName)
        {
            var variable = Environment.GetEnvironmentVariable(variableName);
            Log.Information($"Value of environment variable [{variableName}]: [{variable}]");
            return variable;
        }
    }
}
