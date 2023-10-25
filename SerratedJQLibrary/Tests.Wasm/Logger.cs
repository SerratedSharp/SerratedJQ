using System;

namespace Wasm;

public static class Logger
{
    public static void Log(object obj)
    {    
        foreach (var property in obj.GetType().GetProperties())
            Console.WriteLine(property.Name + ": " + property.GetValue(obj, null).ToString());
    }
}
