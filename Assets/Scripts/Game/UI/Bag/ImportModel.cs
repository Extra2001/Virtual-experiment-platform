/************************************************************************************
    作者：荆煦添
    描述：导入自定义模型的处理程序
*************************************************************************************/
using HT.Framework;
using UnityEngine;
using Common;
using System.IO;
using System;

public class ImportModel
{
    /// <summary>
    /// 打开模型文件并导入
    /// </summary>
    public static bool OpenFile()
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
            var fileName = Guid.NewGuid().ToString() + ".obj";
            File.Copy(filepath, Path.Combine(Application.persistentDataPath, "objects", fileName), true);
            GameManager.Instance.objectsModels.Insert(0, new ObjectsModel()
            {
                id = 0,
                Name = Path.GetFileNameWithoutExtension(filepath),
                DetailMessage = "导入的3D物体",
                Integrated = false,
                PreviewImage = $"{Application.streamingAssetsPath}/PreviewImages/cubic.png",
                ResourcePath = Path.Combine(Application.persistentDataPath, "objects", fileName)
            });
            return true;
        }
        return false;
    }

    public static void DeleteModel(ObjectsModel model)
    {
        if (File.Exists(model.ResourcePath))
            File.Delete(model.ResourcePath);
        GameManager.Instance.objectsModels.Remove(model);
    }
}
