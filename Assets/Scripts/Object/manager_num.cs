using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.PackageManager;
using UnityEngine.UIElements;
using UnityEngine.UI.Extensions;

public class manager_num : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;
    public int accuracy_rating; // 小数点后多少位
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
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
            string s = num_int.ToString();
            for (int i = s.Length-1; i > -1; i--)
            {
                this.transform.Find((i+1).ToString()).gameObject.GetComponent<show_num>().ChangeTheNum((int)s[s.Length-i-1]-48);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
       Show_num(31);
       
    }
}
