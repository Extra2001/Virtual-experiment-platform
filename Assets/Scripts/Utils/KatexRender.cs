using HT.Framework;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class KatexRender : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;
    
    [SerializeField]
    private PowerUI.Manager manager;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            Render("2 \\leq 0");
        });
    }

    public void Render(string latex)
    {
        //var sr = new StreamReader(Path.Combine(Application.streamingAssetsPath, "Katex", "Test.html"));

        //var doc = new PowerUI.HtmlDocument();

        //doc.LoadHtml(sr.ReadToEnd());
        print(manager.document.JavascriptEngine.Engine.Execute("JSON.stringify(document)").GetCompletionValue());
        print(manager.document.JavascriptEngine.Engine.GetValue("renderLatex"));
        manager.document.JavascriptEngine.Run("renderLatex", manager.document.window, latex);
        //manager.Document = doc;
        //doc.JavascriptEngine.AddCode("katex");
        //manager.UpdateZoom(false);
        ////doc.JavascriptEngine.Run("renderLatex", doc.window, latex);
        //sr.Close();
    }
}
