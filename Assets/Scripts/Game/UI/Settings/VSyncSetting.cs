/************************************************************************************
    作者：荆煦添
    描述：调整垂直同步
*************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

class VSyncSetting : MonoBehaviour
{
    public Toggle toggle;

    private void Start()
    {
        toggle.onValueChanged.AddListener(x => QualitySettings.vSyncCount = x ? 1 : 0);
        toggle.SetIsOnWithoutNotify(QualitySettings.vSyncCount > 0);
    }
}
