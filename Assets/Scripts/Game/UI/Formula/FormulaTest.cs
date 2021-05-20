using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Newtonsoft.Json;

public class FormulaTest : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            //foreach (var item in FormulaController.Instances)
            //{
            //    Log.Info(JsonConvert.SerializeObject(item.Value.Serialize()));
            //    try
            //    {
            //        Log.Info($"{item.Value.Expression} = {item.Value.ExpressionExecuted}");
            //    }
            //    catch
            //    {
            //        Log.Info("公式未输入完整。");
            //    }
            //}
            var str = JsonConvert.SerializeObject(FormulaController.Instances.First().Value.Serialize());
            Log.Info(str);
            FormulaController.Instances.Last().Value.LoadFormula(JsonConvert.DeserializeObject<List<FormulaNode>>(str));

            // 存储：
            Storage.CommonStorage.SetStorage("yourkey", FormulaController.Instances.First().Value.Serialize());

            // 取用：
            FormulaController.Instances.Last().Value.LoadFormula(Storage.CommonStorage.GetStorage<List<FormulaNode>>("yourkey"));
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
