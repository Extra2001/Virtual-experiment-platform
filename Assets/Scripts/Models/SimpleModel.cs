using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

public class SimpleModel
{
    public string Title = "提示";

    public string Message = "";

    public string CancelText = "取消";

    public string ConfirmText = "确认";

    public UnityAction CancelAction { get; set; }

    public UnityAction ConfirmAction { get; set; }

    public bool ShowCancel { get; set; } = true;
}