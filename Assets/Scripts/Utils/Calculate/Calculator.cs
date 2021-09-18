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
using UnityEngine;
using static DealProcessResult;

public partial class FormulaController {
    ///<summary>
    ///递归计算有效数字 cbj
    ///</summary>
    public CheckFloat GetCheckFloat() {
        return this.CalcExpression("base");
    }
    public string CheckError(string realval) {
        var root = showedCells.Where((x) => x.thisGUID.Equals("{0}")).Last();
        if(root.ReplaceFlags.Count == 2) {
            var left = showedCells.Where((x) => x.thisGUID.Equals(root.ReplaceFlags.First().Value)).Last();
            var right = showedCells.Where((x) => x.thisGUID.Equals(root.ReplaceFlags.Last().Value)).Last();
            if(right.ReplaceFlags.Count == 0 && left.ReplaceFlags.Count == 0) {
                string template = "您的公式:{0}{1}{2}={3},根据{4}结果{5}。{6}", analyse, result;
                CheckFloat x = CheckFloat.Create(left.value), y = CheckFloat.Create(right.value), a, z = CheckFloat.Create(realval);
                if(root.value.Contains("+")) {
                    a = x + y;
                    bool status = a.Equals(z);
                    analyse = status ? null : $"根据规则要保留{a.EffectiveDigit}位有效数字,正确答案为{a}";
                    result = string.Format(template, x.Original, "+", y.Original, z.Original, "加法先按小数点后位数最少的数据保留其它各数的位数,再进行计算,计算结果也使小数点后保留相同的位数。", status ? "正确" : "错误", analyse);
                    return result;
                }
                else if(root.value.Contains("-")) {
                    a = x - y;
                    bool status = a.Equals(z);
                    analyse = a.TrueValue == z.TrueValue ? null : $"根据规则要保留{a.EffectiveDigit}位有效数字,正确答案为{a}";
                    result = string.Format(template, x.Original, "-", y.Original, z.Original, "减法先按小数点后位数最少的数据保留其它各数的位数,再进行计算,计算结果也使小数点后保留相同的位数。", status ? "正确" : "错误", analyse);
                    return result;
                }
                else if(root.value.Contains("*")) {
                    a = x * y;
                    bool status = a.Equals(z);
                    analyse = a.TrueValue == z.TrueValue ? null : $"根据规则要保留{a.EffectiveDigit}位有效数字,正确答案为{a}";
                    result = string.Format(template, x.Original, "*", y.Original, z.Original, "先按有效数字最少的数据保留其它各数,再进行乘除运算,计算结果仍保留相同有效数字。", status ? "正确" : "错误", analyse);
                    return result;
                }
                else if(root.value.Contains("/")) {
                    a = x / y;
                    bool status = a.Equals(z);
                    analyse = a.TrueValue == z.TrueValue ? null : $"根据规则要保留{a.EffectiveDigit}位有效数字,正确答案为{a}";
                    result = string.Format(template, x.Original, "/", y.Original, z.Original, "先按有效数字最少的数据保留其它各数,再进行乘除运算,计算结果仍保留相同有效数字。", status ? "正确" : "错误", analyse);
                    return result;
                }
            }
        }
        return null;//不是最简单的表达式
    }

    private CheckFloat CalcExpression(string guidstr) {
        var cur = showedCells.Where((x) => x.thisGUID.Equals(guidstr)).Last();
        if(cur == null || cur.ReplaceFlags.Count == 0) {
            //没有别的子节点了 最底层的表达式就是数字
            return CheckFloat.Create(cur.value);

        }
        else if(cur.ReplaceFlags.Count == 1) {
            //函数
            CheckFloat tmp1 = CalcExpression(cur.ReplaceFlags.First().Value);
            if(cur.value.Contains("sin")) {
                return CheckFloat.Sin(tmp1);
            }
            else if(cur.value.Contains("cos")) {
                return CheckFloat.Cos(tmp1);
            }
            else if(cur.value.Contains("tan")) {
                return CheckFloat.Tan(tmp1);
            }
            else if(cur.value.Contains("asin")) {
                return CheckFloat.Asin(tmp1);
            }
            else if(cur.value.Contains("acos")) {
                return CheckFloat.Acos(tmp1);
            }
            else if(cur.value.Contains("atan")) {
                return CheckFloat.Cos(tmp1);
            }
            else if(cur.value.Contains("exp")) {
                return CheckFloat.Exp(Math.E, tmp1);
            }
            else if(cur.value.Contains("log")) {
                if(cur.value.Contains("log(10)")) {
                    return CheckFloat.Log(10.0, tmp1);
                }
                else {
                    return CheckFloat.Log(Math.E, tmp1);
                }
            }
            else if(cur.value.Contains("sqrt")) {
                return CheckFloat.Pow(tmp1, 0.5);
            }
            else if(Regex.IsMatch(cur.value, @"pow\(\s*[0-9A-F]{32}\s*,\s*2\s*\)")) {
                return CheckFloat.Pow(tmp1, 2);
            }
            else if(Regex.IsMatch(cur.value, @"pow\(\s*[0-9A-F]{32}\s*,\s*3\s*\)")) {
                return CheckFloat.Pow(tmp1, 3);
            }
            else {
                return tmp1;
            }
        }
        else if(cur.ReplaceFlags.Count == 2) {
            CheckFloat tmp1, tmp2;
            tmp1 = CalcExpression(cur.ReplaceFlags.First().Value);
            tmp2 = CalcExpression(cur.ReplaceFlags.Last().Value);
            if(cur.value.Contains("+")) {
                return tmp1 + tmp2;
            }
            else if(cur.value.Contains("-")) {
                return tmp1 - tmp2;
            }
            else if(cur.value.Contains("*")) {
                return tmp1 * tmp2;
            }
            else if(cur.value.Contains("/")) {
                return tmp1 / tmp2;
            }
            else if(cur.value.Contains("pow")) {
                return CheckFloat.Pow(tmp1, tmp2);
            }
            else {
                throw new Exception();
            }

        }
        else {
            throw new Exception();
        }
    }
}
public struct CheckFloat : IEquatable<CheckFloat> {//带有效数字的小数
    public double Value { get; private set; }
    public double TrueValue => Value * Math.Pow(10, HiDigit);
    public int EffectiveDigit { get; private set; }
    public int LoDigit { get; private set; }
    public int HiDigit { get; private set; }
    public static readonly CheckFloat PI = new CheckFloat("3.14159265358979323846", false);
    public static readonly CheckFloat E = new CheckFloat("2.71828182845904523536", false);
    public string Original { get; }
    public CheckFloat(double truevalue, bool check = true) : this(truevalue.ToString(), check) { }
    public static CheckFloat Create(string value, bool check = false) {
        if(value.Contains("PI")) {
            return PI;
        }
        else if(value.Contains("Math.E")) {
            return E;
        }
        return new CheckFloat(value, check);

    }
    public CheckFloat(string value, bool checkmaxlen = true) {
        Original = value;
        EffectiveDigit = Effectiveness(value);
        if(checkmaxlen && EffectiveDigit > 8) {
            throw new Exception("输入太精确了");
        }
        if(double.TryParse(value, out double tmp2)) {
            Value = tmp2;
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
        double tmp = Math.Round(Value, n);
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
    public static double KeepTo(double truevalue, int n) {//保留到第n位有效数字
        return Math.Round(truevalue / Math.Pow(10, n)) * Math.Pow(10, n);
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
        double tmp = lhs.TrueValue + rhs.TrueValue;
        return new CheckFloat(KeepTo(tmp, lo).ToString(), false);
    }
    public static CheckFloat operator -(CheckFloat lhs, CheckFloat rhs) {//有效数字的减法
        int lo = Math.Max(lhs.LoDigit, rhs.LoDigit);
        double tmp = lhs.TrueValue - rhs.TrueValue;
        return new CheckFloat(KeepTo(tmp, lo).ToString(), false);
    }
    public static CheckFloat operator *(CheckFloat lhs, CheckFloat rhs) {//有效数字的乘法
        double tmp = KeepEffective((lhs.TrueValue * rhs.TrueValue), Math.Min(rhs.EffectiveDigit, lhs.EffectiveDigit));
        return new CheckFloat(tmp.ToString(), false);
    }
    public static CheckFloat operator /(CheckFloat lhs, CheckFloat rhs) {//除
        double tmp = KeepEffective((lhs.TrueValue / rhs.TrueValue), Math.Min(rhs.EffectiveDigit, lhs.EffectiveDigit));
        return new CheckFloat(tmp.ToString(), false);
    }
    public string Display() {//显示
        return $"值为{TrueValue},{EffectiveDigit}位有效数字,小数部分:{Value},次数{HiDigit},最低位{LoDigit}";
    }
    public override string ToString() {
        return TrueValue.ToString($"G{EffectiveDigit}");
    }
    public static CheckFloat FunctionX(CheckFloat x, double dx, Func<double, double> fn, Func<double, double> derivative) {
        double rv = x.TrueValue;
        double v = fn(rv);
        double dy = derivative(rv) * dx;
        CheckFloat tmp = new CheckFloat(dy, false);
        return new CheckFloat(KeepTo(v, tmp.HiDigit), false);
    }
    public static CheckFloat Sin(CheckFloat x, double dx) {
        return FunctionX(x, dx, Math.Sin, Math.Cos);
    }
    public static CheckFloat Sin(CheckFloat x) {
        double dx = Math.Pow(10, x.LoDigit);
        return FunctionX(x, dx, Math.Sin, Math.Cos);
    }
    public static CheckFloat Cos(CheckFloat x, double dx) {
        return FunctionX(x, dx, Math.Cos, (X) => -Math.Sin(X));
    }
    public static CheckFloat Cos(CheckFloat x) {
        double dx = Math.Pow(10, x.LoDigit);
        return FunctionX(x, dx, Math.Cos, (X) => -Math.Sin(X));
    }
    public static CheckFloat Tan(CheckFloat x, double dx) {
        return FunctionX(x, dx, Math.Tan, (X) => 1 / (Math.Cos(X) * Math.Cos(X)));
    }
    public static CheckFloat Tan(CheckFloat x) {
        double dx = Math.Pow(10, x.LoDigit);
        return FunctionX(x, dx, Math.Tan, (X) => 1 / (Math.Cos(X) * Math.Cos(X)));
    }
    public static CheckFloat Pow(CheckFloat x, double n) {
        if(n == 1.0) return x;
        double dx = Math.Pow(10, x.LoDigit);
        return FunctionX(x, dx, (X) => Math.Pow(X, n), (X) => Math.Pow(X, n - 1) * n);
    }
    public static CheckFloat Exp(double a, CheckFloat x) {
        double dx = Math.Pow(10, x.LoDigit);
        return FunctionX(x, dx, (X) => Math.Pow(a, X), (X) => Math.Pow(a, X) * Math.Log(a));
    }
    public static CheckFloat Log(double a, CheckFloat x) {//log_{a}(x)
        double dx = Math.Pow(10, x.LoDigit);
        return FunctionX(x, dx, (X) => Math.Log(X, a), (X) => 1.0 / (X * Math.Log(a)));
    }
    public static CheckFloat Atan(CheckFloat x) {
        double dx = Math.Pow(10, x.LoDigit);
        return FunctionX(x, dx, Math.Atan, (X) => 1.0 / (X * X + 1.0));
    }
    public static CheckFloat Asin(CheckFloat x) {
        double dx = Math.Pow(10, x.LoDigit);
        return FunctionX(x, dx, Math.Asin, (X) => 1.0 / Math.Sqrt(X * X + 1.0));
    }
    public static CheckFloat Acos(CheckFloat x) {
        double dx = Math.Pow(10, x.LoDigit);
        return FunctionX(x, dx, Math.Acos, (X) => -1.0 / Math.Sqrt(X * X + 1.0));
    }
    public static CheckFloat Pow(CheckFloat x, CheckFloat n) {
        return Pow(x, n.TrueValue);
    }

    public bool Equals(CheckFloat other) {
        return HiDigit == other.HiDigit && LoDigit == other.LoDigit && TrueValue == other.TrueValue;
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
    public static bool CheckDifferenced(List<string> measure, List<string> userdifference) //检查逐差过程是否正确
    {
        List<double> answerdifference = new List<double>();
        int n;
        if(measure.Count % 2 == 0) {
            n = measure.Count / 2;
            for(int i = 0;i < n;i++) {
                answerdifference.Add((double.Parse(measure[i + n]) - double.Parse(measure[i])) - double.Parse(userdifference[i]));
            }
        }
        else {
            n = (measure.Count + 1) / 2;
            for(int i = 0;i < n - 1;i++) {
                answerdifference.Add((double.Parse(measure[i + n]) - double.Parse(measure[i])) - double.Parse(userdifference[i]));
            }
        }
        foreach(var item in answerdifference) {
            if(Math.Abs(item) > 1e-5) {
                return false;
            }
        }
        return true;
    }
    public static (double a, double b, double uncb) SuccessiveDifference(double[] x, double[] y) {
        //y=bx+a
        int n = x.Length, k = n / 2;
        Span<double> x1 = x.AsSpan(0, n / 2), x2 = x.AsSpan(n % 2 == 0 ? n / 2 : n / 2 + 1, n / 2);
        Span<double> y1 = y.AsSpan(0, n / 2), y2 = y.AsSpan(n % 2 == 0 ? n / 2 : n / 2 + 1, n / 2);
        Span<double> bk = k >= 1024 ? new double[k] : stackalloc double[k];
        double b = 0, xx = 0, yy = 0;
        for(int i = 0;i < k;i++) {
            bk[i] = (y2[i] - y1[i]) / (x2[i] - x1[i]);
            b += bk[i];
            xx += x1[i]; xx += x2[i]; yy += y1[i]; yy += y2[i];
        }
        b /= k;
        double a = (yy - b * xx) / (2 * k);
        double uncb = 0.0;
        for(int i = 0;i < k;i++) {
            uncb += (bk[i] - b) * (bk[i] - b);
        }
        uncb = Math.Sqrt(uncb / (k * (k - 1)));
        return (a, b, uncb);
    }
    public static (double b, double a, double r, double b_unca, double a_unca) LinearRegression(double[] x, double[] y) {
        //y=bx+a
        double[] xy = new double[x.Length], x2 = new double[x.Length], y2 = new double[x.Length];
        for(int i = 0;i < x.Length;i++) {
            xy[i] = x[i] * y[i];
            x2[i] = x[i] * x[i];
            y2[i] = y[i] * y[i];
        }
        double xav = Average(x), yav = Average(y), xyav = Average(xy), x2av = Average(x2), y2av = Average(y2);
        double b = (xav * yav - xyav) / (xav * xav - x2av), a = yav - b * xav, r = (xyav - xav * yav) / Math.Sqrt((y2av - yav * yav) * (x2av - xav * xav));
        double sy0 = 0;
        for(int i = 0;i < x.Length;i++) {
            sy0 += (y[i] - (a + b * x[i])) * (y[i] - (a + b * x[i]));
        }
        sy0 = Math.Sqrt(sy0 / (x.Length - 2));
        double b_unca = b * Math.Sqrt((1 / (r * r) - 1) / (x.Length - 2));
        double a_unca = Math.Sqrt(x2av) * b_unca;
        return (b, a, r, b_unca, a_unca);
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
    public static double MakeWrongUncertain(double ua, double ub) {
        return Math.Abs(ua + ub);//制作错误的不确定度
    }
    public static (double, double) MakeWrongUb(double ie) {//仪器误差限
        return (ie, ie / Math.Sqrt(2));//制作错误的b类不确定度
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
    public static string GetUaExprLatex(string varname) {
        //return string.Concat(@"u_a(", varname, @")=\sqrt{\frac{\sum_{i=1}^{n}{({", varname, @"_i}-{\overline{", varname, @"}})^2}}{n(n-1)}}");
        //return string.Concat(@"u_a(", varname, @")=\sqrt{\frac{s^2}{k}}=\sqrt{{", s2, "}{", k, "}}");
        //下面这行是不要符号表达式的
        return string.Concat(@"u_a(", varname, @")=\sqrt{\frac{s^2}{k}}");
    }
    public static string GetUbExprLatex(string varname) {
        //return string.Concat(@"u_b(", varname, @")=\frac{\Delta_{仪}}{\sqrt{3}}=\frac{", insterr.ToString(), @"}\sqrt{3}");
        //return string.Concat(@"u_b(", varname, @")=\frac{", insterr.ToString(), @"}{\sqrt{3}}");
        return string.Concat(@"u_b(", varname, @")=\frac{\Delta_{instrument}}{\sqrt{3}}");
    }
    public static string GetUncLatex(string varname, double ua, double ub) {
        return $@"u({varname})=\sqrt{{({NumberFormat(ua)}^2)+({NumberFormat(ub)}^2)}}";
    }
    public static string GetUncLatex(string varname) {
        return $@"u({varname})=\sqrt{{({{u_a({varname})}}^2)+({{u_b({varname})}}^2)}}";
    }
    public static string GetAverageLatex(string varname) {
        //return $@"\overline{{{varname}}}=\frac{{\sum_{{i = 0}}^{{n}}}}{{{{{varname}_i}}}}{{n}}";
        return string.Concat(@"\overline{", varname, @"}=\frac{\sum_{i=1}^{n}{", varname, "_i}}{n}");
        //return @"{\overline{x}=0}";
    }
    public static (bool correct, string reason) CheckUncertain(string value, string unc) {
        CheckFloat v = new CheckFloat(value, false), u = new CheckFloat(unc, false);
        if(u.EffectiveDigit != 1) {
            return (false, "不确定度保留1位有效数字");
        }
        else {
            if(v.LoDigit == u.HiDigit) {
                return (true, null);
            }
            return (false, "有效数字没有对齐");
        }
    }
}
public class CalcVariable {//2021.8.20
    public List<double> values;
    public double ub;
    public double userua, userub, userunc, useraver;//用户测量的ua,ub,用户的合成的不确定度

    public CalcVariable(double ub, int measures) {//测量了measures个数据
        this.ub = ub; values = new List<double>(measures);
    }
    public (double average, double ua, double unc) CalcUncertain() {// value,ua,u
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
    //下面的是8月20号加的
    public (string UaError, string UbError, string UncError, string AverError, bool IfError) CheckInfo() {
        var uu = CalcUncertain();
        bool flag = false;
        StringBuilder sua = new StringBuilder();
        StringBuilder sub = new StringBuilder();
        StringBuilder sunc = new StringBuilder();
        string s = null;
        if(!userua.AlmostEqual(uu.ua)) {
            flag = true;
            if(userua.AlmostEqual(uu.ua * Math.Sqrt(1.0 * values.Count / (values.Count - 1)))) {
                sua.Append("是否将根号下分母除成了k-1,S^2代表的样本方差的分母也是k-1哟\r\n");
            }
            else {
                sua.Append("其他错误");
            }
        }
        if(!ub.AlmostEqual(userub)) {
            flag = true;
            if(userub.AlmostEqual(ub * Math.Sqrt(3))) {
                sub.Append("是否忘除根号3?\r\n");
            }
            else {
                sub.Append("其他错误");
            }
        }
        if(!userunc.AlmostEqual(uu.unc)) {
            flag = true;
            if(userunc.AlmostEqual(uu.ua + ub)) {
                sunc.Append("是否直接将A类和B类不确定度直接相加，两者应该各自平方后相加开方\r\n");
            }
            else {
                sunc.Append("其他错误");
            }
        }
        if(!useraver.AlmostEqual(uu.average)) {
            flag = true;
            s = "平均值计算错了";
        }
        return (sua.ToString(), sub.ToString(), sunc.ToString(), s, flag);

        /*if(flag) {
            sb.Append("A类不确定度，B类不确定度及合成不确定度的正确答案如下");
            return (uu.average, uu.ua, uu.unc, string.Concat("检查出以下错误\r\n", sb.ToString()));
        }
        else {
            return (uu.average, uu.ua, uu.unc, null);//没有错
        }*/
    }
}
public class CalcArgs {//一次计算
    private Dictionary<string, CalcVariable> vars;//变量
    private Dictionary<string, double> cons;//常量
    //public int arrlen { get; private set; }
    public double userval, userunc;//用户输入 合成的值 总的不确定度
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
    public bool SetConstant(string varname, double val) {//添加或修改一个常量
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

    public bool MeasureUserUnput(string varname, double _userua, double _userub, double _userunc) {
        //添加用户直接测量的值
        if(vars.ContainsKey(varname)) {
            vars[varname].userua = _userua;
            vars[varname].userub = _userub;
            vars[varname].userunc = _userunc;
            return true;
        }
        return false;
    }

    public void ComplexUserUnput(double _userval, double _userunc) {
        //添加用户合成的结果（如体积）
        userval = _userval;
        userunc = _userunc;
    }

    public static (symexpr, symexpr) Calculate(string expression, CalcArgs argobj) {//return (value,uncertain) 符号计算不确定度
        symexpr val = symexpr.Parse(expression), unc = 0;
        foreach(var item in argobj.vars) {
            symexpr uncvar = symexpr.Parse($"u_{item.Key}");
            unc += (val.Differentiate(item.Key) * uncvar).Pow(2);
        }
        return (val, unc.Sqrt());
    }
    public static CalcMeasureResult CalculateMeasureValue(CalcArgs argobj) {//代入数据
        List<QuantityError> errors = new List<QuantityError>(argobj.vars.Count);
        QuantityError temp = new QuantityError();
        bool flag = false;
        var res = new CalcMeasureResult();

        foreach(var item in argobj.vars) {
            var CheckResult = argobj.vars[item.Key].CheckInfo();
            temp = new QuantityError();
            if(CheckResult.IfError) {
                flag = true;
                temp.right = false;
                if(CheckResult.UaError != "") {
                    temp.ua.right = false;
                    temp.ua.latex = StaticMethods.GetUaExprLatex(item.Key);
                    temp.ua.message = CheckResult.UaError;
                }
                if(CheckResult.UbError != "") {
                    temp.ub.right = false;
                    temp.ub.latex = StaticMethods.GetUbExprLatex(item.Key);
                    temp.ub.message = CheckResult.UbError;
                }
                if(CheckResult.UncError != "") {
                    temp.unc.right = false;
                    temp.unc.latex = StaticMethods.GetUncLatex(item.Key, item.Value.userua, item.Value.userub);
                    temp.unc.message = CheckResult.UncError;
                }
                if(CheckResult.AverError != "") {
                    temp.average.right = false;
                    temp.average.latex = StaticMethods.GetAverageLatex(item.Key);
                    temp.average.message = CheckResult.AverError;
                }
                temp.Symbol = item.Key;
                temp.Title = "对物理量" + item.Key + "的检查";
            }
            else {
                temp.right = true;
            }
            errors.Add(temp);
        }

        res.status = flag ? "计算有误" : "计算无误";
        res.err = errors;
        return res;
    }

    public static CalcResult CalculateComplexValue(string expression, CalcArgs argobj) {
        (symexpr valexpr, symexpr uncexpr) = Calculate(expression, argobj);
        QuantityError error = new QuantityError();
        bool flag = false;
        var res = new CalcResult();
        StringBuilder answer = new StringBuilder();
        StringBuilder answerunc = new StringBuilder();
        Dictionary<string, FloatingPoint> vals = new Dictionary<string, FloatingPoint>(argobj.cons.Count + 2 * argobj.vars.Count);
        foreach(var item in argobj.cons) {
            vals[item.Key] = argobj.cons[item.Key];
        }
        foreach(var item in argobj.vars) {
            var u = argobj.vars[item.Key].CalcUncertain();
            vals[item.Key] = u.average;
            vals[$"u_{item.Key}"] = u.unc;
        }
        double val1 = valexpr.Evaluate(vals).RealValue;//对的val
        double unc1 = uncexpr.Evaluate(vals).RealValue;//对的unc
        foreach(var item in argobj.vars) {
            vals[$"u_{item.Key}"] = argobj.vars[item.Key].userunc;
        }
        if(!val1.AlmostEqual(argobj.userval)) {
            flag = true;
            error.answer.right = false;
            error.answer.latex = valexpr.ToLaTeX();
            if(true) {
                answer.Append("其他错误");
            }
            error.answer.message = answer.ToString();
        }
        if(!unc1.AlmostEqual(argobj.userunc)) {
            flag = true;
            error.answerunc.right = false;
            error.answerunc.latex = uncexpr.ToLaTeX();
            if(true) {
                answerunc.Append("其他错误");
            }
            error.answerunc.message = answerunc.ToString();
        }
        error.right = !flag;
        error.Title = "合成量的检查";
        res.status = flag ? "计算有误" : "计算无误";
        res.err = error;
        //res.calcexpr = valexpr;
        //res.uncexpr = uncexpr;
        return res;
    }


    public static symexpr GetSymexpr(string expression) {
        return symexpr.Parse(expression);
    }
}
public class CalcMeasureResult {
    public string status;
    public List<QuantityError> err;//变量不确定度检查结果
    //public double val, unc, userval, userunc;//值 不确定度 用户计算值 用户计算不确定度
}

public class CalcResult {
    public string status;
    public QuantityError err;//最终合成量不确定度检查结果
    //public symexpr calcexpr, uncexpr;
    public static CalcResult CheckRegression(UserInputLinearRegression input) {//一元线性回归
        var (b, a, r, b_unca, a_unca) = StaticMethods.LinearRegression(input.x, input.y);
        CalcResult result = new CalcResult();
        result.err = new QuantityError();
        bool flag = true;
        if(!input.b.AlmostEqual(b)) {
            flag = false;
            result.err.b.right = false;
            result.err.b.message = "一元线性回归y=bx+a系数b计算错误";
            result.err.b.latex = @"b=\frac{\overline{x}\overline{y}-\overline{xy}}{(\overline{x})^2-\overline{x^2}}";
        }
        if(!input.a.AlmostEqual(a)) {
            flag = false;
            result.err.a.right = false;
            result.err.a.message = "一元线性回归y=bx+a系数a计算错误";
            result.err.a.latex = @"a=\overline{y}-b\overline{x}";
        }
        if(!input.r.AlmostEqual(r)) {
            flag = false;
            result.err.r.right = false;
            result.err.r.message = "一元线性回归y=bx+a相关系数r计算错误";
            result.err.r.latex = @"\frac{\overline{xy}-\overline{x}\overline{y}}{\sqrt{(\overline{x^2}-(\overline{x})^2)(\overline{y^2}-(\overline{y})^2)}}";
        }
        if(!input.f_uncb.AlmostEqual(input.correct_uncb)) {
            flag = false;
            result.err.ub.right = false;
            result.err.ub.message = "b类不确定度计算错误";
            result.err.ub.latex = StaticMethods.GetUbExprLatex(input.ifa ? "a" : "b");
        }
        if(input.ifa) {
            if(!input.f_unca.AlmostEqual(a_unca)) {
                flag = false;
                result.err.ua.right = false;
                result.err.ua.message = "a类不确定度计算错误";
                result.err.ua.latex = @"u_a(a)=b\sqrt{\frac{\overline{x^2}}{n-2}(\frac{1}{r^2}-1)}";
            }
            if(!input.f_unc.AlmostEqual(StaticMethods.CalcUncertain(a_unca, input.correct_uncb))) {
                flag = false;
                result.err.unc.right = false;
                result.err.unc.message = "合成不确定度计算错误";
                result.err.unc.latex = StaticMethods.GetUncLatex("a");
            }
        }
        else {
            if(!input.f_unca.AlmostEqual(b_unca)) {
                flag = false;
                result.err.ua.right = false;
                result.err.ua.message = "a类不确定度计算错误";
                result.err.ua.latex = @"u_a(b)=b\sqrt{\frac{1}{n-2}(\frac{1}{r^2}-1)}";
            }
            if(!input.f_unca.AlmostEqual(StaticMethods.CalcUncertain(b_unca, input.correct_uncb))) {
                flag = false;
                result.err.unc.right = false;
                result.err.unc.message = "合成不确定度计算错误";
                result.err.unc.latex = StaticMethods.GetUncLatex("b");
            }
        }
        result.err.right = flag;
        result.err.Title = $"检查{input.name}的不确定度";
        result.status = flag ? "正确" : "错误";
        return result;
    }
    public static CalcResult CheckSuccessiveDifference(UserInputSuccessiveDifference input) {//逐差法
        double[] bk = new double[input.y_nplusi_minus_y_i.Length];
        double b = 0, uncb = 0;
        bool flag = true;
        CalcResult result = new CalcResult();
        result.err = new QuantityError();
        for(int i = 0;i < bk.Length;i++) {
            bk[i] = input.y_nplusi_minus_y_i[i] / input.x_nplusi_minus_x_i;
        }
        b = StaticMethods.Average(bk);
        for(int i = 0;i < bk.Length;i++) {
            uncb += ((bk[i] - b) * (bk[i] - b));
        }
        uncb = Math.Sqrt(uncb / (bk.Length * (bk.Length - 1)));
        if(!input.correct_b_uncb.AlmostEqual(input.user_b_uncb)) {
            flag = false;
            result.err.ub.right = false;
            result.err.ub.message = "b类不确定度计算错误";
            result.err.ub.latex = StaticMethods.GetUbExprLatex("b");
        }
        if(!input.user_b_unca.AlmostEqual(uncb)) {
            flag = false;
            result.err.ua.right = false;
            result.err.ua.message = "a类不确定度计算错误";
            result.err.ua.latex = @"u_a(b)=s(\overline{b})=\sqrt{\frac{(b_i-\overline{b})^2}{n(n-1)}}";
        }
        if(!input.user_aver_b.AlmostEqual(b)) {
            flag = false;
            result.err.average.right = false;
            result.err.average.message = "逐差法计算错误";
            result.err.average.latex = StaticMethods.GetAverageLatex(@"\Delta b");
        }
        if(!input.user_b_unc.AlmostEqual(StaticMethods.CalcUncertain(uncb, input.correct_b_uncb))) {
            flag = false;
            result.err.unc.right = false;
            result.err.unc.message = "合成不确定度计算错误";
            result.err.unc.latex = StaticMethods.GetUncLatex("b");
        }
        result.err.right = flag;
        result.status = flag ? "正确" : "错误";
        result.err.Title = $"检查{input.name}的不确定度";
        return result;
    }
    public static CalcResult CheckTable(UserInputTable input) {//一个的列表法
        string varname = input.varname;
        var userin = input.data;
        var (average, ua, unc) = userin.CalcUncertain();
        var result = new CalcResult();
        result.err = new QuantityError();
        bool flag = true;
        if(!userin.userua.AlmostEqual(ua)) {
            flag = false;
            result.err.ua.right = false;
            result.err.ua.message = "a类不确定度计算错误";
            result.err.ua.latex = StaticMethods.GetUaExprLatex(varname);
        }
        if(!userin.userub.AlmostEqual(userin.ub)) {
            flag = false;
            result.err.ub.right = false;
            result.err.ub.message = "b类不确定度计算错误";
            result.err.ub.latex = StaticMethods.GetUbExprLatex(varname);
        }
        if(!userin.userunc.AlmostEqual(unc)) {
            flag = false;
            result.err.unc.right = false;
            result.err.unc.message = "合成不确定度计算错误";
            result.err.unc.latex = StaticMethods.GetUncLatex(varname);
        }
        result.err.right = flag;
        result.err.Title = $"检查{input.varname}的不确定度";
        result.status = flag ? "正确" : "错误";
        return result;
    }
}
public class UserInputSuccessiveDifference {
    public string name;
    public double[] y_nplusi_minus_y_i;//n是这个数组的长度
    public double x_nplusi_minus_x_i;//x[n+i]-x[i]是固定值
    public double user_aver_b;//用户给的最终结果
    public double correct_b_uncb;//正确的b类不确定度
    public double user_b_unca;//用户给的最终结果的a类不确定度
    public double user_b_uncb;//用户给的最终结果的b类不确定度
    public double user_b_unc;//用户给的最终结果的合成不确定度
}
public class UserInputLinearRegression {
    public string name;
    public double[] x, y;//用户数据
    public double a, b, r, f_unca, f_uncb, f_unc, correct_uncb; //用户的a,b,r 和 a,b的不确定度
    public bool ifa;//true为要求a的不确定度 false为b的不确定度
}
public class UserInputTable {
    public string varname;
    public CalcVariable data;
}