using Serilog;
using System;

namespace EmailWorker
{
    public static class EnviromentChecker
    {
       public static  string CheckAndLogEnviroment(string variable)
        {
            var enviroment = Environment.GetEnvironmentVariable(variable);
            Log.Information($"Environment [{variable}]: [{enviroment}]");
            return enviroment;
        } 
    }
}
