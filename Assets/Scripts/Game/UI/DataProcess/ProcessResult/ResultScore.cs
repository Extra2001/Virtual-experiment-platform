using System;

[Serializable]
public class ResultScore
{
    public int MeasureQuantityError { get; set; } = 0;//����ֱ�Ӳ��������µĴ�����
    public int ComplexQuantityError { get; set; } = 0;//�������պϳ������µĴ�����
    public int DataRecordError { get; set; } = 0; //��¼���ݹ��̷��µĴ�����

    public double CalcScore()
    {
        double ans = 0;
        ans += f(MeasureQuantityError) * 2 + f(ComplexQuantityError) * 2 + f(DataRecordError);

        return ans;
    }
    
    double f(int n)   // ����������õ��ķ�����һ��
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
