/************************************************************************************
    作者：荆煦添
    描述：执行JavaScript表达式并获取值
*************************************************************************************/
using Jint;

public class Javascript
{
    /// <summary>
    /// 执行JavaScript表达式并获取值
    /// </summary>
    public static T Eval<T>(string expression) where T : class
    {
        Engine engine = new Engine();
        return engine.Execute(expression).GetCompletionValue().TryCast<T>();
    }
    /// <summary>
    /// 执行JavaScript表达式并获取值
    /// </summary>
    public static double Eval(string expression)
    {
        Engine engine = new Engine();
        return engine.Execute(expression).GetCompletionValue().AsNumber();
    }
}
