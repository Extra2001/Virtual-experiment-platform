mergeInto(LibraryManager.library, {

    _SendHtml: function (key, param) {
      UnitySetDataFromWeb(Pointer_stringify(key), Pointer_stringify(param));
    },
  });
  