using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.PackageManager;

public class manager_num : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;
    public int accuracy_rating; // 小数点后多少位
    /// </summary>
    GameObject f1;
    GameObject f2;
    GameObject f3;
    GameObject f4;
    GameObject f5;
    GameObject f6;
    GameObject f7;
    // Start is called before the first frame update
    void Start()
    {
        f1 = this.transform.Find("1").gameObject;
        f2 = this.transform.Find("1").gameObject;
        f3 = this.transform.Find("1").gameObject;
        f4 = this.transform.Find("1").gameObject;
        f5 = this.transform.Find("1").gameObject;
        f6 = this.transform.Find("1").gameObject;
        f7 = this.transform.Find("1").gameObject;
    }
    public void Show_num(double num)
    {
        for (int i = 0; i < accuracy_rating; i++)
        {
            num *= 10;
        }
        int num_int=(int) num;
        if (num_int > 9999999)
        {
            //
        }
        else
        {
            //string s = ToString(num_int)
        }
    }
    // Update is called once per frame
    void Update()
    {
        //f1.GetComponent<show_num>().ChangeTheNum(1);
    }
}
