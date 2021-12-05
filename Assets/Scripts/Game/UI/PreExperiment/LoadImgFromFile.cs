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

        /*Action<string> action = delegate (string s)
        {
            Debug.Log(s);
            control.GetComponent<show_return_message>().message = "上传成功";
            control.GetComponent<show_return_message>().can_show = true;
        };
        
        Communication.UploadReport(uuid, experimentId, courseId, 0, 60, "", action,
            new ExperimentReportModelBuilder("report", new ExperimentReportContentBuilder("report", new Files("report", url, 1, 1, 1))));*/

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
