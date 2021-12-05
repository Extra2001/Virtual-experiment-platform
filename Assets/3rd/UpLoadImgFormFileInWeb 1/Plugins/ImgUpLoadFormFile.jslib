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
        console.log(fileObj);
        var url = 'https://newbase.zhihuishu.com/upload/commonUploadFile'; //服务器上传地址

        var xhr = new XMLHttpRequest();
        xhr.withCredentials = true;


        // var container = document.querySelector('.preview');
        // var img = new Image();
        // img.src = URL.createObjectURL(fileObj);
        // var fr = new FileReader();
        // fr.readAsText(fileObj);
        // fr.readAsBinaryString(fileObj);
        fr.onload = function() { //文件读取成功回调
            var dataUrl = fileObj.name + "&";
            dataUrl += fr.result;  //result属性为data:URL格式,与读取方式有关
            console.log(dataUrl);
            if (ReceiveMsgGameObjectName != null && ReceiveMsgMethodName != null) {
                SendMessage(ReceiveMsgGameObjectName, ReceiveMsgMethodName, dataUrl);
            }
        };
	// 	img.onload = function() {
    //     //后缀名使用的时候需要判断一下,这里的r即为图片字符串数据
    //     var r = getImageBase64(img, 'png');
    //         if (ReceiveMsgGameObjectName != null && ReceiveMsgMethodName != null)
    //         SendMessage(ReceiveMsgGameObjectName, ReceiveMsgMethodName, r);
    // }
},
$uploadImage: function()
	{
    // if (document.getElementById("inputpdf") == null)
    // {
    //     var input = document.createElement("input");
    //     input.setAttribute('type', 'file');
    //     input.setAttribute('accept', 'application/pdf');
    //     input.setAttribute('id', 'inputpdf');
    //     input.setAttribute('hidden', 'true');
	// 	input.onchange = uploadImg;
    //     document.body.appendChild(input);
    // }
    // document.getElementById("inputpdf").click();
        console.log("go");
        if (document.getElementById("file-input") == null)
        {
            var input = document.createElement("input");
            input.setAttribute('type', 'file');
            input.setAttribute('accept', 'application/pdf');
            input.setAttribute('id', 'file-input');
            input.setAttribute('hidden', 'true');
            input.onchange = function () {
                var data = new FormData(),
                    url = 'https://newbase.zhihuishu.com/upload/commonUploadFile'; //服务器上传地址
                data.append('file', input.files[0]);

                var xhr = new XMLHttpRequest();
                xhr.withCredentials = true;

                xhr.addEventListener("readystatechange", function() {
                    if(this.readyState === 4) {
                        var j = JSON.parse(this.responseText);
                        var file_url = j['rt']['url'];
                        console.log("file_url = " + file_url);
                        if (ReceiveMsgGameObjectName != null && ReceiveMsgMethodName != null) {
                            SendMessage(ReceiveMsgGameObjectName, ReceiveMsgMethodName, file_url);
                        }
                    }
                });

                xhr.open("POST", url);
                xhr.send(data);
            }
            document.body.appendChild(input);
        }
        document.getElementById("file-input").click();
},
 GetImgFromFile : function(GameObjectName_, MethodName_)
        {
        var GameObjectName = Pointer_stringify(GameObjectName_);
        var MethodName = Pointer_stringify(MethodName_);
		ReceiveMsgGameObjectName=GameObjectName;
        ReceiveMsgMethodName=MethodName;
		uploadImage();
    }
};

autoAddDeps(ImgUpLoadFormFile, '$ReceiveMsgGameObjectName');
autoAddDeps(ImgUpLoadFormFile, '$ReceiveMsgMethodName');
autoAddDeps(ImgUpLoadFormFile, '$uploadImage');
autoAddDeps(ImgUpLoadFormFile, '$uploadImg');
autoAddDeps(ImgUpLoadFormFile, '$getImageBase64');
mergeInto(LibraryManager.library, ImgUpLoadFormFile);