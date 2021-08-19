/************************************************************************************
    作者：曹北健
    描述：计算库
*************************************************************************************/
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Symbolics;
using expr = MathNet.Symbolics.Expression;
using symexpr = MathNet.Symbolics.SymbolicExpression;
using symdiff = MathNet.Symbolics.Calculus;
using symfuncs = MathNet.Symbolics.Function;
public struct CheckFloat {//带有效数字的小数
    public decimal Value { get; private set; }
    public decimal TrueValue => Value * (decimal)Math.Pow(10, HiDigit);
    public int EffectiveDigit { get; private set; }
    public int LoDigit { get; private set; }
    public int HiDigit { get; private set; }
    public CheckFloat(decimal truevalue) : this(truevalue.ToString()) { }
    public CheckFloat(string value) {
        EffectiveDigit = Effectiveness(value);
        if(decimal.TryParse(value, out decimal tmp)) {
            Value = tmp;
        }
        else if(double.TryParse(value, out double tmp2)) {
            Value = new decimal(tmp2);
        }
        else throw new NotSupportedException();
        if(Value != 0) {
            HiDigit = (int)Math.Floor(Math.Log10((double)Math.Abs(Value)));
            LoDigit = HiDigit - EffectiveDigit + 1;
            if(HiDigit > 0) {
                for(int i = 0;i < HiDigit;i++) {
                    Value /= 10;
                }
            }
            else if(HiDigit < 0) {
                for(int i = 0;i < -HiDigit;i++) {
                    Value *= 10;
                }
            }
        }
        else {
            LoDigit = 0; HiDigit = 0;
        }
    }
    public static double KeepEffective(double d, int n) {//保留n位有效数字
        if(d == 0.0) return 0;
        if(d > 1 || d < -1)
            n = n - (int)Math.Log10(Math.Abs(d)) - 1;
        else
            n = n + (int)Math.Log10(1.0 / Math.Abs(d));
        if(n < 0) {
            d = (int)(d / Math.Pow(10, 0 - n)) * Math.Pow(10, 0 - n);
            n = 0;
        }
        return Math.Round(d, n);
    }
    public CheckFloat KeepEffective(int n) {
        decimal tmp = decimal.Round(Value, n);
        return new CheckFloat() { Value = tmp, EffectiveDigit = n + 1, HiDigit = HiDigit, LoDigit = HiDigit - n };
    }
    public static decimal KeepEffective(decimal d, int n) {//保留小数点后n位 共n+1位有效数字
        if(d == 0) return 0;
        if(d > 1 || d < -1)
            n = n - (int)Math.Log10((double)Math.Abs(d)) - 1;
        else
            n = n + (int)Math.Log10((double)(1 / Math.Abs(d)));
        if(n < 0) {
            d = (int)((double)d / Math.Pow(10, -n)) * (decimal)(Math.Pow(10, -n));
            n = 0;
        }
        return Math.Round(d, n);
    }
    public static decimal KeepTo(decimal truevalue, int n) {//保留到第n位有效数字
        return Math.Truncate(truevalue / (decimal)Math.Pow(10, n)) * (decimal)Math.Pow(10, n);
    }
    public static int Effectiveness(string num) {//计算一个字符串表示的小数有多少位有效数字
        int digits = 0; bool lead0 = true;
        foreach(var item in num) {
            if(item == 'E' || item == 'e') {
                break;
            }
            int i = item - '0';
            if(i >= 0 && i <= 9) {
                if(i > 0 || (i == 0 && !lead0)) {
                    digits++; lead0 = false;
                }
            }
        }
        return Math.Max(digits, 1);
    }
    public static CheckFloat operator +(CheckFloat num) {
        return num;
    }
    public static CheckFloat operator -(CheckFloat num) {//取相反数
        return new CheckFloat() { EffectiveDigit = num.EffectiveDigit, HiDigit = num.HiDigit, LoDigit = num.LoDigit, Value = -num.Value };
    }
    public static CheckFloat operator +(CheckFloat lhs, CheckFloat rhs) {//有效数字的加法
        int lo = Math.Max(lhs.LoDigit, rhs.LoDigit);
        int keep1 = lhs.HiDigit - lo + 1, keep2 = rhs.HiDigit - lo + 1;
        decimal tmp = lhs.TrueValue + rhs.TrueValue;
        return new CheckFloat(KeepTo(tmp, lo).ToString($"G{Math.Max(lhs.HiDigit, rhs.HiDigit)}"));
    }
    public static CheckFloat operator -(CheckFloat lhs, CheckFloat rhs) {//有效数字的减法
        int lo = Math.Max(lhs.LoDigit, rhs.LoDigit);
        int keep1 = lhs.HiDigit - lo + 1, keep2 = rhs.HiDigit - lo + 1;
        decimal tmp = lhs.TrueValue - rhs.TrueValue;
        return new CheckFloat(KeepTo(tmp, lo).ToString($"E{Math.Max(keep1, keep2) - 1}"));
    }
    public static CheckFloat operator *(CheckFloat lhs, CheckFloat rhs) {//有效数字的乘法
        decimal tmp = KeepEffective((lhs.TrueValue * rhs.TrueValue), Math.Min(rhs.EffectiveDigit, lhs.EffectiveDigit));
        return new CheckFloat(tmp.ToString());
    }
    public static CheckFloat operator /(CheckFloat lhs, CheckFloat rhs) {//除
        decimal tmp = KeepEffective((lhs.TrueValue / rhs.TrueValue), Math.Min(rhs.EffectiveDigit, lhs.EffectiveDigit));
        return new CheckFloat(tmp.ToString());
    }
    public override string ToString() {//显示
        return $"值为{TrueValue},{EffectiveDigit}位有效数字,小数部分:{Value}";
    }
}
public static class StaticMethods {
    public static readonly HashSet<string> keywords = new HashSet<string>(){//符号计算的关键字
            "pi","e","abs","acos","asin","atan","sin","cos","tan","cot","sec","csc","j","sqrt","pow","sinh","cosh","tanh","exp","ln","lg"
        };
    public static double Average(IEnumerable<double> data) {//平均数
        return MathNet.Numerics.Statistics.Statistics.Mean(data);
    }
    public static double VarianceN(IEnumerable<double> data) {//除N的方差
        return MathNet.Numerics.Statistics.Statistics.PopulationVariance(data);
    }
    public static double Variance(IEnumerable<double> data) {//除N-1的方差
        return MathNet.Numerics.Statistics.Statistics.Variance(data);
    }
    public static double StdDev(IEnumerable<double> data) {//除N-1的标准差
        return MathNet.Numerics.Statistics.Statistics.StandardDeviation(data);
    }
    public static double CenterMoment(IEnumerable<double> data, int k) {//k阶原点矩
        double ans = 0;
        int n = 0;
        foreach(var item in data) {
            ans += Math.Pow(item, k); n++;
        }
        return ans /= n;
    }
    public static string NumberFormat(double Input) {//格式化输出float
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
    public static (double avg, double ua, double u) CalcUncertain(IEnumerable<double> data, double ub) {
        //输入:测量数据data 仪器B类不确定度ub
        //返回:(主值,A类不确定度,合成不确定度)
        int n = 0;
        double sum1 = 0, sum2 = 0;
        foreach(var item in data) {
            sum1 += item; n++;
        }
        double average = sum1 / n;
        foreach(var item in data) {
            sum2 += (item - average) * (item - average) / (n * (n - 1));
        }
        return (average, Math.Sqrt(sum2), Math.Sqrt(sum2 + ub * ub));
    }
    public static double CalcUncertain(double ua, double ub) {
        //输入:A类不确定度ua 仪器B类不确定度ub
        //返回:合成不确定度
        return Math.Sqrt(ua * ua + ub * ub);
    }
    public static double MakeWrongUncertain(double ua,double ub) {
        return Math.Abs(ua + ub);//制作错误的不确定度
    }
    public static (double,double) MakeWrongUb(double ie) {//仪器误差限
        return (ie,ie/Math.Sqrt(2));//制作错误的b类不确定度
    }
    public static bool ValidVarname(string v) {//检查v是否能作为合法物理量名
        if(keywords.Contains(v)) {//函数列表有 直接否
            return false;
        }
        else {
            if(!char.IsLetter(v[0]) || Regex.IsMatch(v, @"[^a-zA-Z0-9]")) {//只允许A-Za-z0-9
                return false;
            }
            return true;
        }
    }
    public static bool ValidExpression(string v) {//检查表达式v是否合法(后续要调)
        try {
            var expr0 = symexpr.Parse(v);
            return true;
        }
        catch(Exception) {
            return false;
        }
    }
    public static string PostResponse(string url, string data) {//发送web请求
        try {
            string r = default;
            HttpWebRequest req = WebRequest.CreateHttp(url);
            req.Method = "POST";
            req.KeepAlive = true;
            req.ContentType = "application/json";
            using(var s = req.GetRequestStream()) {
                byte[] buf = Encoding.UTF8.GetBytes(data);
                s.Write(buf, 0, buf.Length);
            }
            using(HttpWebResponse resp = req.GetResponse() as HttpWebResponse) {
                using(StreamReader sr = new StreamReader(resp.GetResponseStream(), Encoding.UTF8)) {
                    r = sr.ReadToEnd();
                }
            }
            return r;
        }
        catch(Exception ex) {
            Console.Error.WriteLine(ex);
            return null;
            //throw;
        }
    }
}
public class CalcVariable {
    public List<double> values;
    public double ub;
    public CalcVariable(double ub, int measures) {
        this.ub = ub; values = new List<double>(measures);
    }
    public CalcVariable() { }
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
public class CalcArgs {//一次计算
    private Dictionary<string, CalcVariable> vars;//变量
    private Dictionary<string, double> cons;//常量
    //public int arrlen { get; private set; }
    public CalcArgs(/*int measures*/) {
        vars = new Dictionary<string, CalcVariable>(); cons = new Dictionary<string, double>();
        //arrlen = measures;
    }
    public static readonly HashSet<string> keywords = new HashSet<string>(){
            "pi","e","abs","acos","asin","atan","sin","cos","tan","cot","sec","csc","j","sqrt","pow","sinh","cosh","tanh","exp","ln","lg"
        };
    public bool ValidVarname(string v) {//检查变量类型
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
    public bool SetConstant(string varname, double val) {//添加一个常量
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
    public bool AddVariable(string varname, double ub, int measures) {
        //先检查变量如果没有出现过就加入
        if(ValidVarname(varname)) {
            vars[varname] = new CalcVariable(ub, measures);
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
    public static (symexpr, symexpr) Calculate(string expression, CalcArgs argobj) {//return (value,uncertain) 符号计算不确定度
        symexpr val = symexpr.Parse(expression), unc = 0;
        foreach(var item in argobj.vars) {
            symexpr uncvar = symexpr.Parse($"u_{item.Key}");
            unc += (val.Differentiate(item.Key) * uncvar).Pow(2);
        }
        return (val, unc.Sqrt());
    }
    public static (double, double) CalculateValue(symexpr valexpr, symexpr uncexpr, CalcArgs argobj) {//代入数据
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
    public class UserInput {
        public string name { get; set; }
        public double value { get; set; }
        public double u { get; set; }
    }
    public static (double, double,double) CalculateValue(symexpr valexpr, symexpr uncexpr, List<UserInput> inputs) {
        //return (value, uncertain,错的)
        Dictionary<string, FloatingPoint> vals = new Dictionary<string, FloatingPoint>(inputs.Count * 2);
        foreach(var item in inputs) {
            vals[item.name] = item.value;
            vals[$"u_{item.name}"] = item.u;
        }
        double x = valexpr.Evaluate(vals).RealValue;
        return (x, uncexpr.Evaluate(vals).RealValue,x/Math.Sqrt(2));
    }
    public static symexpr GetSymexpr(string expression) {
        return symexpr.Parse(expression);
    }
}
