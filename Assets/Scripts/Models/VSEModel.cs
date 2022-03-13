using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSEAccessTokenGET
{
    public int code { get; set; }
    public string access_token { get; set; }
    public string un { get; set; }
    public string dis { get; set; }
    public string role { get; set; }
    public string classid { get; set; }
}

public class VSEAccessTokenREFRESH
{
    public int code { get; set; }
    public string access_token { get; set; }
}

public class VSEReportUpload
{
    public string username;
    public string title;
    public int status;
    public int score;
    public long startTime;
    public long endTime;
    public int timeUsed;
    public string appid;
    public string originId;
    public string previewurl;
    public List<VSEReportStep> steps;
}

public class VSEReportStep
{
    public int seq;
    public string title;
    public long startTime;
    public long endTime;
    public int timeUsed;
    public int expectTime;
    public int maxScore;
    public int score;
    public int repeatCount;
    public string evaluation;
    public string scoringModel;
    public string remarks;
}