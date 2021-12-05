mergeInto(LibraryManager.library, {
    _SendHtml: function(key, param) {
        UnitySetDataFromWeb(Pointer_stringify(key), Pointer_stringify(param));
    },
    _SetTips: function(tips) {
        SetExplanation(Pointer_stringify(tips))
    }
});