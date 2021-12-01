mergeInto(LibraryManager.library, {
    Render: function (latex, callbackMono, callbackFuncName) {
        var latex1 = Pointer_stringify(latex);
        var callbackMono1 = Pointer_stringify(callbackMono);
        var callbackFuncName1 = Pointer_stringify(callbackFuncName);
        getImageOfLatex(latex1).then(function (res) {
            gameInstance.SendMessage(callbackMono1, callbackFuncName1, res.png);
        }).catch(function () {
            gameInstance.SendMessage(callbackMono1, callbackFuncName1, "渲染错误");
        })
    },
});