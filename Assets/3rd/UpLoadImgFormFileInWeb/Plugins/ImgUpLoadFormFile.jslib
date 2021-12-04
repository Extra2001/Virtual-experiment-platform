var ImgUpLoadFormFile = {
	$ReceiveMsgGameObjectName:"",
	$ReceiveMsgMethodName:"",
	$getImageBase64: function(img, ext)
{
    var canvas = document.createElement("canvas");
    canvas.width = img.width;
    canvas.height = img.height;
    var ctx = canvas.getContext("2d");
    ctx.drawImage(img, 0, 0, img.width, img.height);
    var dataURL = canvas.toDataURL("image/" + ext);
    canvas = null;
    return dataURL;
},
$uploadImg: function(e)
	{	
    var URL = window.URL || window.webkitURL || window.mozURL;
        var e = event || e;
    
		 var fileObj =  e.target.files[0];
        var container = document.querySelector('.preview');
        var img = new Image();
        img.src = URL.createObjectURL(fileObj);
		img.onload = function() {
        //后缀名使用的时候需要判断一下,这里的r即为图片字符串数据
        var r = getImageBase64(img, 'png');
            if (ReceiveMsgGameObjectName != null && ReceiveMsgMethodName != null)
            SendMessage(ReceiveMsgGameObjectName, ReceiveMsgMethodName, r);
    }
},
$uploadImage: function()
	{
    if (document.getElementById("inputimg") == null)
    {
        var input = document.createElement("input");
        input.setAttribute('type', 'file');
        input.setAttribute('accept', 'image/png');
        input.setAttribute('id', 'inputimg');
        input.setAttribute('hidden', 'true');
		input.onchange = uploadImg;
        document.body.appendChild(input);
    }
    document.getElementById("inputimg").click();
},
 GetImgFromFile : function(GameObjectName_, MethodName_)
        {
        var GameObjectName = Pointer_stringify(GameObjectName_);
        var MethodName = Pointer_stringify(MethodName_);
		ReceiveMsgGameObjectName=GameObjectName;
        ReceiveMsgMethodName=MethodName;
		uploadImage();
    }};

autoAddDeps(ImgUpLoadFormFile, '$ReceiveMsgGameObjectName');
autoAddDeps(ImgUpLoadFormFile, '$ReceiveMsgMethodName');
autoAddDeps(ImgUpLoadFormFile, '$uploadImage');
autoAddDeps(ImgUpLoadFormFile, '$uploadImg');
autoAddDeps(ImgUpLoadFormFile, '$getImageBase64');
mergeInto(LibraryManager.library, ImgUpLoadFormFile);