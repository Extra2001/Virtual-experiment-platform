using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ResultScore
{
    int MeasureQuantityError { get; set; } = 0;
    int ComplexQuantityError { get; set; } = 0;
    int DataRecordError { get; set; } = 0;
}
