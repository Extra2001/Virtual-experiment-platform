using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.Threading.Tasks;

public class ProcessManager
{
    public string FileName { get => ""; }
    private Process process = null;

    public void StartService()
    {
        process = Process.Start(FileName, GetPort().ToString());
        process.Exited += Process_Exited;
    }

    private void Process_Exited(object sender, System.EventArgs e)
    {
        StartService();
    }

    public void StopService()
    {
        process.Exited -= Process_Exited;
        process.Kill();
    }

    private int GetPort()
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

    private bool PortInUse(int port)
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
