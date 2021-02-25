using HT.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public abstract class IndicatorBase : UILogicResident
{
    public abstract void ShowIndicator(string key, string message);
}
