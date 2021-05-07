using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using Flurl.Http;
using System.Threading.Tasks;

public static class ServerAPI
{
    // public static Task<ValidateStatus> ValidateSymbol(string symbol)
    // {
    //     return Task.Run(() =>
    //     {
    //         var result = GetUrl("valsym")
    //         .AllowAnyHttpStatus()
    //         .PostJsonAsync(new { symbol })
    //         .ReceiveJson<CodeResponse>().Result;

<<<<<<< HEAD
            if (result.code == 0)
                return Pass();
            return Refuse("鏈煡閿欒");
        });
    }
=======
    //         if (result.code == 0)
    //             return Pass();
    //         return Refuse("未知错误");
    //     });
    // }
>>>>>>> 8ea9a5e28762f89436cab7fb79a04eae7b15628e

    // public static Task<ValidateStatus> ValidateExpression(string expression, List<QuantityModel> quantities)
    // {
    //     return Task.Run(() =>
    //     {
    //         var result = GetUrl("valexpr")
    //         .AllowAnyHttpStatus()
    //         .PostJsonAsync(new
    //         {
    //             expr = expression,
    //             quantities
    //         })
    //         .ReceiveJson<CodeResponse>().Result;

<<<<<<< HEAD
            if (result.code == 0)
                return Pass();
            return Refuse("鏈煡閿欒");
        });
    }
=======
    //         if (result.code == 0)
    //             return Pass();
    //         return Refuse("未知错误");
    //     });
    // }
>>>>>>> 8ea9a5e28762f89436cab7fb79a04eae7b15628e

    // private static string GetUrl(string parmas)
    // {
    //     return "http://localhost:" +
    //         $"{ProcessManager.Port}/{parmas}";
    // }

<<<<<<< HEAD
    private static ValidateStatus Pass()
    {
        return new ValidateStatus()
        {
            Passed = true,
            Message = "閫氳繃"
        };
    }
=======
    // private static ValidateStatus Pass()
    // {
    //     return new ValidateStatus()
    //     {
    //         Passed = true,
    //         Message = "通过"
    //     };
    // }
>>>>>>> 8ea9a5e28762f89436cab7fb79a04eae7b15628e

    // private static ValidateStatus Refuse(string message)
    // {
    //     return new ValidateStatus()
    //     {
    //         Passed = false,
    //         Message = message
    //     };
    // }
}
