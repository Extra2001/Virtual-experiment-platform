using System;

[Serializable]
public class ResultScore
{
    public int MeasureQuantityError { get; set; } = 0;//处理直接测量量犯下的错误数
    public int ComplexQuantityError { get; set; } = 0;//处理最终合成量犯下的错误数
    public int DataRecordError { get; set; } = 0; //记录数据过程犯下的错误数

    public double CalcScore()
    {
        double ans = 0;
        ans += f(MeasureQuantityError) * 2 + f(ComplexQuantityError) * 2 + f(DataRecordError);

        return ans;
    }
    
    double f(int n)   // 将错误个数得到的分数归一化
    {
        double ans;
        if (n == 0)
        {
            ans = 1;
        }
        else if (n <= 10)
        {
            ans = 1 - 0.1 * n;
        }
        else
        {
            ans = 0;
        }
        return ans;
    }
}
