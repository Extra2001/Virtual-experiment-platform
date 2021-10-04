using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using HT.Framework;

public class SaveOpenRecord
{
    public static void ExportRecord(Record record)
    {
        var path = GetSaveFilePath();
        if (string.IsNullOrEmpty(path)) return;
        var info = RecordManager.recordInfos.Where(x => x.id == record.info.id).FirstOrDefault();
        if (info != null) record.info = info;
        var data = JsonConvert.SerializeObject(record);
        var encoded = new SymmetricMethod().Encrypto(data);
        File.WriteAllText(path, encoded);
        UIAPI.Instance.ShowTips($"已将存档保存至{path}，发送该文件给好友并导入程序。");
    }

    public static void ImportRecord()
    {
        var path = GetOpenFilePath();
        if (string.IsNullOrEmpty(path)) return;
        
        try
        {
            string encoded = File.ReadAllText(path);
            var data = new SymmetricMethod().Decrypto(encoded);
            var info = RecordManager.Import(data);
            Main.m_Event.Throw<RecordUpdateEventHandler>();
            UIAPI.Instance.ShowTips($"导入成功。存档名称为{info.title}。点击加载按钮加载该存档。");
        }
        catch
        {
            UIAPI.Instance.ShowTips("导入时发生错误，请检查存档文件是否完好。");
        }
    }

    public static string GetOpenFilePath()
    {
        OpenFileDlg pth = new OpenFileDlg();
        pth.structSize = System.Runtime.InteropServices.Marshal.SizeOf(pth);
        pth.filter = "REC (*.rec)\0*.rec\0\0";
        pth.file = new string(new char[256]);
        pth.maxFile = pth.file.Length;
        pth.fileTitle = new string(new char[64]);
        pth.maxFileTitle = pth.fileTitle.Length;
        pth.initialDir = Application.dataPath;
        pth.title = "选择要导入的存档";
        pth.defExt = "rec";
        pth.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;
        if (OpenFileDialog.GetOpenFileName(pth))
        {
            string filepath = pth.file;
            return filepath;
        }
        return null;
    }

    public static string GetSaveFilePath()
    {
        SaveFileDlg pth = new SaveFileDlg();
        pth.structSize = System.Runtime.InteropServices.Marshal.SizeOf(pth);
        pth.filter = "REC (*.rec)\0*.rec\0\0";
        pth.file = new string(new char[256]);
        pth.maxFile = pth.file.Length;
        pth.fileTitle = new string(new char[64]);
        pth.maxFileTitle = pth.fileTitle.Length;
        pth.title = "选择存档保存的位置";
        pth.defExt = "rec";
        pth.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008 | 0x00000002;
        if (SaveFileDialog.GetSaveFileName(pth))
        {
            string filepath = pth.file;
            return filepath;
        }
        return null;
    }
}

public class SymmetricMethod
{
    private SymmetricAlgorithm mobjCryptoService;
    private string Key;
    /// <summary>   
    /// 对称加密类的构造函数   
    /// </summary>   
    public SymmetricMethod()
    {
        mobjCryptoService = new RijndaelManaged();
        Key = "Guz(%&hj7x89H$yuBI0456FtmaT5&fvHUFCy76*h%(HilJ$lhj!y6&(*jkP87jH7";
    }
    /// <summary>   
    /// 获得密钥   
    /// </summary>   
    /// <returns>密钥</returns>   
    private byte[] GetLegalKey()
    {
        string sTemp = Key;
        mobjCryptoService.GenerateKey();
        byte[] bytTemp = mobjCryptoService.Key;
        int KeyLength = bytTemp.Length;
        if (sTemp.Length > KeyLength)
            sTemp = sTemp.Substring(0, KeyLength);
        else if (sTemp.Length < KeyLength)
            sTemp = sTemp.PadRight(KeyLength, ' ');
        return ASCIIEncoding.ASCII.GetBytes(sTemp);
    }
    /// <summary>   
    /// 获得初始向量IV   
    /// </summary>   
    /// <returns>初试向量IV</returns>   
    private byte[] GetLegalIV()
    {
        string sTemp = "E4ghj*Ghg7!rNIfb&95GUY86GfghUb#er57HBh(u%g6HJ($jhWk7&!hg4ui%$hjk";
        mobjCryptoService.GenerateIV();
        byte[] bytTemp = mobjCryptoService.IV;
        int IVLength = bytTemp.Length;
        if (sTemp.Length > IVLength)
            sTemp = sTemp.Substring(0, IVLength);
        else if (sTemp.Length < IVLength)
            sTemp = sTemp.PadRight(IVLength, ' ');
        return ASCIIEncoding.ASCII.GetBytes(sTemp);
    }
    /// <summary>   
    /// 加密方法   
    /// </summary>   
    /// <param name="Source">待加密的串</param>   
    /// <returns>经过加密的串</returns>   
    public string Encrypto(string Source)
    {
        byte[] bytIn = UTF8Encoding.UTF8.GetBytes(Source);
        MemoryStream ms = new MemoryStream();
        mobjCryptoService.Key = GetLegalKey();
        mobjCryptoService.IV = GetLegalIV();
        ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();
        CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
        cs.Write(bytIn, 0, bytIn.Length);
        cs.FlushFinalBlock();
        ms.Close();
        byte[] bytOut = ms.ToArray();
        return Convert.ToBase64String(bytOut);
    }
    /// <summary>   
    /// 解密方法   
    /// </summary>   
    /// <param name="Source">待解密的串</param>   
    /// <returns>经过解密的串</returns>   
    public string Decrypto(string Source)
    {
        byte[] bytIn = Convert.FromBase64String(Source);
        MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
        mobjCryptoService.Key = GetLegalKey();
        mobjCryptoService.IV = GetLegalIV();
        ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();
        CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
        StreamReader sr = new StreamReader(cs);
        return sr.ReadToEnd();
    }
}