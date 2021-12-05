using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WisdomTree.Xuxiaohao.Function;

public class LoadImgFromFileTest : MonoBehaviour
{
    public Button loadBtn;
    public Image image;
    // Start is called before the first frame update
    void Start()
    {
        loadBtn.onClick.AddListener(OnUploadCustomImgBtnClick);
    }


    private void OnUploadCustomImgBtnClick()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        ToolHelper.UITool.TextureTool.Texture2DGetterFormFile_Webgl.GetTexture2DFormFile_Webgl(this.gameObject, RecivePng);                
#endif
        ToolHelper.UITool.TextureTool.Texture2DGetterFormFile_WinPC.UpLodImage(GetUpLoadTexture2D);
    }
    public void GetUpLoadTexture2D(Texture2D texture)
    {
        if (texture == null)
        {
            return;
        }
        Sprite sprite = TextureToSprite(texture);
        image.sprite = sprite;
    }
    public void RecivePng(string str)
    {
        Texture2D texture2D = ToolHelper.UITool.TextureTool.Texture2DGetterFormFile_Webgl.WebglStringToTextur2D(str);

        GetUpLoadTexture2D(texture2D);
    }
    public static Sprite TextureToSprite(Texture2D t)
    {
        if (t == null)
        {
            return null;
        }
        return Sprite.Create(t, new Rect(Vector2.zero, new Vector2(t.width, t.height)), Vector2.zero);
    }
}
