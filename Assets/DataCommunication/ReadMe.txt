测试示例：
void Update()
{
    if (Input.GetKeyDown(KeyCode.Alpha1))
    {
        Communication.Init();
    }
    else if (Input.GetKeyDown(KeyCode.Alpha2))
    {
        Communication.Test();
    }
}

结构示例：
public static void Test()
{
    UploadReport(22, 2000000005, 100, "", str => { },
        new ExperimentReportModelBuilder("实验目的",
            new ExperimentReportContentBuilder("实验目的", "目的")
        ),
        new ExperimentReportModelBuilder("实验内容",
            new ExperimentReportContentBuilder("内容1", new Texture2D(10, 10)),
            new ExperimentReportContentBuilder("内容2", new Texture2D[] { new Texture2D(10, 10), new Texture2D(10, 10) }, ExperimentReportContentBuilder.ImageType.Static)
        )
        );
}