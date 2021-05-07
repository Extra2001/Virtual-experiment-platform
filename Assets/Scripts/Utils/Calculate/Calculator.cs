using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Symbolics;
using expr = MathNet.Symbolics.Expression;
using symexpr = MathNet.Symbolics.SymbolicExpression;
using symdiff = MathNet.Symbolics.Calculus;
using symfuncs = MathNet.Symbolics.Function;
public class MakeExpressionResult {
    public string resexpr, uncexpr;
    public List<string> argtable;
    public symexpr sym, unc;
}
public static class Calculate {
    public static Tuple<double, double> CalcUncertain(double[] data) {
        int n = 0;
        double sum1 = 0, sum2 = 0;
        foreach(var item in data) {
            sum1 += item; n++;
        }
        double average = sum1 / n;
        foreach(var item in data) {
            sum2 += (item - average) * (item - average) / (n * (n - 1));
        }
        return new Tuple<double, double>(average, Math.Sqrt(sum2));
    }
    public static void CalcExpression(string exprstr, Dictionary<string, double[]> data) {
        symexpr sym = symexpr.Parse(exprstr), res = 0;
        Dictionary<string, FloatingPoint> nom = new Dictionary<string, FloatingPoint>(data.Count * 2 + 2);
        foreach(var item in data) {
            string uname = $"u_{item.Key}";
            var av_unc = CalcUncertain(item.Value);
            nom[item.Key] = av_unc.Item1;
            nom[uname] = av_unc.Item2;
            symexpr s = item.Key;
            symexpr us = uname;
            res += (sym.Differentiate(s) * us).Pow(2);
        }
        res = res.Sqrt();
        Console.WriteLine(sym);
        Console.WriteLine();
        Console.WriteLine(res);
        Console.WriteLine();
        Console.WriteLine(sym.ToLaTeX());
        Console.WriteLine();
        Console.WriteLine(res.ToLaTeX());
        var calcres = sym.Evaluate(nom);
        var calcu = res.Evaluate(nom);
        Console.WriteLine(calcres);
        Console.WriteLine(calcu);       
    }
    public static Tuple<string,string> MakeUncertain(string exprstr,string[] vars) {
        symexpr sym = symexpr.Parse(exprstr);
        foreach(var item in vars) {

        }
        return new Tuple<string, string>(sym.ToLaTeX(), null);
    }
}
