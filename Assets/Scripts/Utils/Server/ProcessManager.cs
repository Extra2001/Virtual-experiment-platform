using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.Threading.Tasks;

public static class ProcessManager
{
    public static string FileName => @"C:\Users\jingx\PycharmProjects\testFlaskProject\venv\Scripts\python.exe";
    public static int Port { get; private set; }
    private static Process process = null;

    public static void StartService()
    {
        int port = GetPort();
        var startInfo = new ProcessStartInfo()
        {
            FileName = FileName,
            Arguments = $@"-u C:\Users\jingx\PycharmProjects\testFlaskProject\app.py {port}",
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardError = true,
            RedirectStandardOutput = true
        };
        (process = Process.Start(startInfo)).Exited += Process_Exited;
        UnityEngine.Debug.Log($"服务已运行在 http://localhost:{port}/");

    }

    private static void Process_Exited(object sender, System.EventArgs e)
    {
        UnityEngine.Debug.Log($"服务中途崩溃。");
        StartService();
    }

    public static void StopService()
    {
        if (process == null || process.HasExited)
            UnityEngine.Debug.Log($"服务在此前已停止。");
        else
        {
            process.Exited -= Process_Exited;
            process.Kill();
            UnityEngine.Debug.Log($"服务已停止。");
        }
    }

    private static int GetPort()
    {
        System.Random r = new System.Random();
        for (int i = 0; i < 10000; i++)
        {
            int ret = r.Next(4000, 20000);
            if (!PortInUse(ret))
                return ret;
        }
        return 0;
    }

    private static bool PortInUse(int port)
    {
        bool inUse = false;

        IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
        IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();//IP端口

        foreach (IPEndPoint endPoint in ipEndPoints)
        {
            if (endPoint.Port == port)
            {
                inUse = true;
                return inUse;
            }
        }
        ipEndPoints = ipProperties.GetActiveUdpListeners();//UDP端口
        foreach (IPEndPoint endPoint in ipEndPoints)
        {
            if (endPoint.Port == port)
            {
                inUse = true;
                return inUse;
            }
        }
        return inUse;
    }
}
