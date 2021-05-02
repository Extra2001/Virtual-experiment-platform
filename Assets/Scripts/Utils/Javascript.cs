using Jint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Javascript
{
    public static T Eval<T>(string expression) where T : class
    {
        Engine engine = new Engine();
        return engine.Execute(expression).GetCompletionValue().TryCast<T>();
    }

    public static double Eval(string expression)
    {
        Engine engine = new Engine();
        return engine.Execute(expression).GetCompletionValue().AsNumber();
    }
}
