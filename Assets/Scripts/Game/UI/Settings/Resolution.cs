/************************************************************************************
    作者：荆煦添
    描述：调整分辨率
*************************************************************************************/
using HT.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

public class Resolution : HTBehaviour
{
    private List<CustomSettings.Resolution> resolutions = new List<CustomSettings.Resolution>();
    private Dropdown dropdown;
    void Start()
    {
        AddDefault();
        dropdown = GetComponent<Dropdown>();
        dropdown.ClearOptions();
        dropdown.AddOptions(resolutions.Select(x => $"{x.width}x{x.height} {x.refreshRate}").ToList());
        dropdown.value = 0;
        dropdown.onValueChanged.AddListener(value =>
        {
            var oldh = Screen.currentResolution;
            if (resolutions[value].width != oldh.width || resolutions[value].height != oldh.height)
            {
                Screen.SetResolution(resolutions[value].width, resolutions[value].height, true);
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                var task = Task.Delay(15000, tokenSource.Token);
                task.ContinueWith(_ =>
                {
                    Screen.SetResolution(oldh.width, oldh.height, true);
                    dropdown.SetValueWithoutNotify(resolutions.FindIndex(x => x.width == oldh.width && x.height == oldh.height));
                });
                UIAPI.Instance.ShowModel(new ModelDialogModel
                {
                    Message = new BindableString("显示分辨率已更改，是否确认？"),
                    ConfirmAction = () =>
                    {
                        tokenSource.Cancel();
                    },
                    CancelAction = () =>
                    {
                        tokenSource.Cancel();
                        Screen.SetResolution(oldh.width, oldh.height, true);
                        dropdown.SetValueWithoutNotify(resolutions.FindIndex(x => x.width == oldh.width && x.height == oldh.height));
                    }
                });
            }
        });
    }

    private void AddDefault()
    {
        resolutions.Clear();
        resolutions.Add(new CustomSettings.Resolution()
        {
            width = Screen.currentResolution.width,
            height = Screen.currentResolution.height,
            refreshRate = Screen.currentResolution.refreshRate
        });
        foreach (var item in Screen.resolutions)
        {
            Add(item.width, item.width, item.refreshRate);
        }
        for (int i = 1; i < resolutions.Count; i++)
            if (resolutions[i].width == resolutions[0].width &&
                    resolutions[i].height == resolutions[0].height &&
                    resolutions[i].refreshRate == resolutions[0].refreshRate)
                resolutions.RemoveAt(i);
    }

    private void Add(int width, int height, int refe)
    {
        resolutions.Add(new CustomSettings.Resolution()
        {
            width = width,
            height = height,
            refreshRate = refe
        });
    }
}
