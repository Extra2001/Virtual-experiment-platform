using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using System.IO;
using UnityEngine.UI;

public class ImportModel : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OpenFile);
    }

    public void OpenFile()
    {
        OpenFileDlg pth = new OpenFileDlg();
        pth.structSize = System.Runtime.InteropServices.Marshal.SizeOf(pth);
        pth.filter = "OBJ (*.obj)\0*.obj\0\0";
        pth.file = new string(new char[256]);
        pth.maxFile = pth.file.Length;
        pth.fileTitle = new string(new char[64]);
        pth.maxFileTitle = pth.fileTitle.Length;
        pth.initialDir = Application.dataPath;
        pth.title = "选择要导入的模型";
        pth.defExt = "obj";
        pth.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;
        if (OpenFileDialog.GetOpenFileName(pth))
        {
            string filepath = pth.file;
            if (!Directory.Exists(Path.Combine(Application.persistentDataPath, "objects")))
                Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "objects"));
            File.Copy(filepath, Path.Combine(Application.persistentDataPath, "objects", Path.GetFileName(filepath)), true);
            GameManager.Instance.objectsModels.Add(new ObjectsModel()
            {
                id = 0,
                Name = Path.GetFileNameWithoutExtension(filepath),
                DetailMessage = "导入的3D物体",
                Integrated = false,
                PreviewImage = $"{Application.streamingAssetsPath}/PreviewImages/cubic.png",
                ResourcePath = Path.Combine(Application.persistentDataPath, "objects", Path.GetFileName(filepath))
            });
        }
    }
}
