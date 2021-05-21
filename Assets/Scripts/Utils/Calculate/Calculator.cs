using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Symbolics;
using expr = MathNet.Symbolics.Expression;
using symexpr = MathNet.Symbolics.SymbolicExpression;
using symdiff = MathNet.Symbolics.Calculus;
using symfuncs = MathNet.Symbolics.Function;

public static class StaticMethods {
    public static double Average(IEnumerable<double> data) {
        return MathNet.Numerics.Statistics.Statistics.Mean(data);
    }
    public static double Variance(IEnumerable<double> data) {
        return MathNet.Numerics.Statistics.Statistics.PopulationVariance(data);
    }
    public static double Moment(IEnumerable<double> data) {
        throw new NotImplementedException();
    }
    public static string NumberFormat(double Input) {
        string Output;

        if(Math.Abs(Input) > 0.01 && Math.Abs(Input) < 1000) {
            Output = Input.ToString("f4");
        }
        else if((Input - 0) == 0) {
            Output = "0";
        }
        else {
            Output = Input.ToString("E");
        }

        return Output;
    }
}
public class CalcVariable {
    public List<double> values;
    public double ub;
    public CalcVariable(double ub, int measures) {
        this.ub = ub; values = new List<double>(measures);
    }
    public (double, double, double) CalcUncertain() {// value,ua,u
        int n = 0;
        double sum1 = 0, sum2 = 0;
        foreach(var item in values) {
            sum1 += item; n++;
        }
        double average = sum1 / n;
        foreach(var item in values) {
            sum2 += (item - average) * (item - average) / (n * (n - 1));
        }
        return (average, Math.Sqrt(sum2), Math.Sqrt(sum2 + ub * ub));
    }
}
public class CalcArgs {
    private Dictionary<string, CalcVariable> vars;//变量
    private Dictionary<string, double> cons;//常量
    public int arrlen { get; private set; }
    public CalcArgs(int measures) {
        vars = new Dictionary<string, CalcVariable>(); cons = new Dictionary<string, double>();
        arrlen = measures;
    }
    public static readonly HashSet<string> keywords = new HashSet<string>(){
            "pi","e","abs","acos","asin","atan","sin","cos","tan","cot","sec","csc","j","sqrt","pow","sinh","cosh","tanh","exp","ln","lg"
        };
    public bool ValidVarname(string v) {
        if(keywords.Contains(v)) {
            return false;
        }
        else {
            if(vars.ContainsKey(v) || !char.IsLetter(v[0]) || Regex.IsMatch(v, @"[^a-zA-Z0-9]")) {
                return false;
            }
            return true;
        }
    }
    public bool SetConstant(string varname, double val) {
        if(!cons.ContainsKey(varname)) {
            if(ValidVarname(varname)) {
                cons[varname] = val; return true;
            }
            return false;
        }
        else {
            cons[varname] = val;
            return true;
        }
    }
    public bool AddVariable(string varname, double ub) {
        //先检查变量如果没有出现过就加入
        if(ValidVarname(varname)) {
            vars[varname] = new CalcVariable(ub, arrlen);
            return true;
        }
        else {
            return false;
        }
    }
    public bool Measure(string varname, double[] values) {
        //修改测量值 成功返回true
        if(vars.ContainsKey(varname)) {
            vars[varname].values.Clear(); vars[varname].values.AddRange(values); return true;
        }
        return false;
    }
    public static (symexpr, symexpr) Calculate(string expression, CalcArgs argobj) {//return (value,uncertain)
        symexpr val = symexpr.Parse(expression), unc = 0;
        foreach(var item in argobj.vars) {
            symexpr uncvar = symexpr.Parse($"u_{item.Key}");
            unc += (val.Differentiate(item.Key) * uncvar).Pow(2);
        }
        return (val, unc.Sqrt());
    }
    public static (double, double) CalculateValue(symexpr valexpr, symexpr uncexpr, CalcArgs argobj) {
        //return (value, uncertain)
        Dictionary<string, FloatingPoint> vals = new Dictionary<string, FloatingPoint>(argobj.cons.Count + 2 * argobj.vars.Count);
        foreach(var item in argobj.cons) {
            vals[item.Key] = argobj.cons[item.Key];
        }
        foreach(var item in argobj.vars) {
            var unc = argobj.vars[item.Key].CalcUncertain();
            vals[item.Key] = unc.Item1;
            vals[$"u_{item.Key}"] = unc.Item3;
        }
        return (valexpr.Evaluate(vals).RealValue, uncexpr.Evaluate(vals).RealValue);
    }
}
