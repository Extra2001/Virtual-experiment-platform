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

    //         if (result.code == 0)
    //             return Pass();
    //         return Refuse("δ֪����");
    //     });
    // }

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

    //         if (result.code == 0)
    //             return Pass();
    //         return Refuse("δ֪����");
    //     });
    // }

    // private static string GetUrl(string parmas)
    // {
    //     return "http://localhost:" +
    //         $"{ProcessManager.Port}/{parmas}";
    // }

    // private static ValidateStatus Pass()
    // {
    //     return new ValidateStatus()
    //     {
    //         Passed = true,
    //         Message = "ͨ��"
    //     };
    // }

    // private static ValidateStatus Refuse(string message)
    // {
    //     return new ValidateStatus()
    //     {
    //         Passed = false,
    //         Message = message
    //     };
    // }
}
