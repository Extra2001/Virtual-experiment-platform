using HT.Framework;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using WisdomTree.Common.Function;
using WisdomTree.Xuxiaohao.Function;
using System.Linq;
using System.IO;

public class LoadImgFromFile : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;

    public string uuid;
    public int experimentId;
    public int courseId;


    public void OnUploadCustomImgBtnClick()
    {
        Debug.Log("button down");
#if UNITY_WEBGL && !UNITY_EDITOR
               //  UpLoadImgFormFileInWeb.GetImgFromFile(this.name, "RecivePng");
        ToolHelper.UITool.TextureTool.Texture2DGetterFormFile_Webgl.GetTexture2DFormFile_Webgl(this.gameObject, RecivePng);    
                // ToolHelper.UITool.TextureTool.Texture2DGetterFormFile_Webgl.GetTexture2DFormFile_Webgl(this.name, "RecivePng");              
#endif
        Debug.Log("end if");
        //ToolHelper.UITool.TextureTool.Texture2DGetterFormFile_WinPC.UpLodImage(GetUpLoadTexture2D);
    }
    public void GetUpLoadTexture2D(Texture2D texture)
    {
        Debug.Log("GetUpLoadTexture2D begin");
        Debug.Log(texture);
        if (texture == null)
        {
            return;
        }
        Sprite sprite = TextureToSprite(texture);
        //image.sprite = sprite;
    }
    public void RecivePng(string url)
    {
        //Texture2D texture2D = ToolHelper.UITool.TextureTool.Texture2DGetterFormFile_Webgl.WebglStringToTextur2D(str);

        //GetUpLoadTexture2D(texture2D);
        /*
        Action<string> action = delegate (string s)
        {
            text.GetComponent<Text>().text = s;
        };
        Communication.UploadFile(arr, "try", action);
        */
        /*
        string filename = str.Split('&')[0];
        string content = str.Substring(filename.Length + 1);
        byte[] b = Encoding.ASCII.GetBytes(content.ToCharArray());
        Debug.Log(b);
        */

        Action<string> action = delegate (string s)
        {
            UIAPI.Instance.ShowModel(new SimpleModel()
            {
                ShowCancel = false,
                Title = "提示",
                Message = "实验数据上传成功"
            });
        };

        Record record = RecordManager.GetRecord(GetComponent<RecordCell>().recordId, x => 
        {
            /*
            需要填充的的数据：1.定义的每个物理量及其的名称、符号、测量工具、数据处理方法
            2.合成公式的字符串表达式
            3.Record.score的三种错误数量
            4.获取实验分数使用Record.score。CalcScore
            */

            //似乎可以使用注释掉的内容进行通讯，但要想办法把uuid传进来
            List<ExperimentReportModelBuilder> models = new List<ExperimentReportModelBuilder>();
            List<ExperimentReportContentBuilder> contents = new List<ExperimentReportContentBuilder>();
            var hh = new string[] { "直接计算", "逐差法", "一元线性回归", "图示法" };
            contents.Add(new ExperimentReportContentBuilder("实验设计", $"实验共测量{x.quantities.Count}个物理量，分别有{string.Join("、", x.quantities.Select(x => x.Name + "(" + x.Symbol + ")" + "，使用" + x.InstrumentType.CreateInstrumentInstance().Name + "测量，并使用" + hh[x.processMethod] + "进行数据处理；"))}"));
            contents.Add(new ExperimentReportContentBuilder("数据处理", string.Join("\n", x.quantities.Select(y =>
            $"物理量：{y.Name}，代号：{y.Symbol}，处理方法：{hh[y.processMethod]}。处理结果：平均值：{?}"))));
            contents.Add(new ExperimentReportContentBuilder("附加材料", new List<Files>().ToArray())); /*这里换成已打开的文件*/
            models.Add(new ExperimentReportModelBuilder("实验报告", contents.ToArray()));
            Communication.UploadReport(GameManager.Instance.startTime, DateTime.Now, x.score, $"本次实验共有{x.}次xxx错误，{x.}次xxx错误。。。", models.ToArray(), new Step[0]);//这里也可以添加很多实验步骤。
            if (url != null)
            {
                
                //Communication.UploadReport(uuid, experimentId, courseId, 0, 60, "", action, new ExperimentReportModelBuilder("report", new ExperimentReportContentBuilder("report", new Files("report", url, 1, 1, 1))));
            }
            else
            {
                //Communication.UploadReport(uuid, experimentId, courseId, 0, 60, "", action, new ExperimentReportModelBuilder("report", new ExperimentReportContentBuilder("report", new Files("report", url, 1, 1, 1))));
            }


        });


    }



    public static Sprite TextureToSprite(Texture2D t)
    {
        if (t == null)
        {
            return null;
        }
        return Sprite.Create(t, new Rect(Vector2.zero, new Vector2(t.width, t.height)), Vector2.zero);
    }

    public void uploadPDF()
    {
        //Communication.UploadFile();
    }
}
