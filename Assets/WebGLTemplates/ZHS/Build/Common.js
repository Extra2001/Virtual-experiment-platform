var u2;
var url="https://studyservice.zhihuishu.com/api/stuExperiment/submitExperimentResult";
var timeUrl="https://studyservice.zhihuishu.com/api/stuExperiment/systemTime";
//var url="studyservice.zhihuishu.com/api/videoquestion/mytest";
var id= -1;
var startTime=-1000000000000;
var endTime=-1000000000000;
function UnityGetDataFromWeb(identifier)
{									
}
	function UnitySetDataFromWeb(identifier, data)
	{
		if(identifier == "eval")
		{
			eval(data);
		}

		if(identifier == 'getQueryVariable')
		{
			WebSetDataToUnity('getQueryVariable', data + '=' + getQueryVariable(data))
		}

		if(identifier == 'getCookie')
		{
			WebSetDataToUnity('getCookie', data + '=' + getUUID())
		}
		
		if (identifier == 'getToken')
		{
            WebSetDataToUnity('getToken', data + '=' + getToken())
        }

		if (identifier == 'getAudio')
		{
            WebSetDataToUnity('getAudio', "audio" + '=' + data)
        }

		if("startRecording"==identifier)
		{
			startRecording();
			return;
		}
		if("stopRecording"==identifier)
		{
			stopRecording();
			return;
		}
		if("playRecording"==identifier)
		{
			playRecording();
			return;
		}
		if("stopPlay"==identifier)
		{
			stopPlay();
			return;
		}
		if("pausePlay"==identifier)
		{
			pausePlay();
			return;
		}
		if("continuePlay"==identifier)
		{
			continuePlay();
			return;
		}
		if("uploadRecording"==identifier)
		{
			uploadRecording();
			return;
		}
		
		if(identifier =="getExperimentId")
		{
			WebSetDataToUnity('getExperimentId', data + '=' + GetMeta("experimentId"))
		}
		if(identifier =="getCourseId")
		{
			WebSetDataToUnity('getCourseId', data + '=' + GetMeta("courseId"))
		}
		if(identifier =="getAppId")
		{
			WebSetDataToUnity('getAppId', data + '=' + GetMeta("appId"))
		}
		if(identifier =="getSecret")
		{
			WebSetDataToUnity('getSecret', data + '=' + GetMeta("secret"))
		}
		if (identifier == 'getTicket')
		{
            WebSetDataToUnity('getTicket', data + '=' + getTicket())
        }
		if (identifier == 'getUrls')
		{
            readJson('Build/urls.json', getUrls);
        }
	}
	function WebSetDataToUnity(identifier, data) 
	{
		gameInstance.SendMessage('_Communication', 'Receive', identifier + '_' + data);
	}
	function GetMeta(metaName)
	{
		var metas = document.getElementsByTagName("meta");
		for (let i = 0; i < metas.length; i++) {
			if (metas[i].getAttribute('name') == metaName) {
				return metas[i].getAttribute('content');
			}
		}
		
		return '';
    }
	function getQueryVariable(variable)
	{
		var query = window.location.search.substring(1);
		var vars = query.split("&");
		for (var i=0;i<vars.length;i++) {
			var pair = vars[i].split("=");
			if(pair[0] == variable){return pair[1];}
		}
		return(-1);
	}
	function getToken()
	{
		var query = window.location.search.substring(1);
		var vars = query.split("token=");
		return vars[1];
	}
	function getTicket()
	{
		var query = window.location.search.substring(1);
		if(query.indexOf("ticket=") == -1) return "";
		var vars = query.split("ticket=");
		return vars[1];
	}
	
	function readJson(path, loadFunc)
	{
		var url = path;
		var request = new XMLHttpRequest();
		request.open("get", url);
		request.send(null);
		request.onload = function () {
			if (request.status == 200) {
				loadFunc(request.responseText);
			}
			else
			{
				loadFunc("");
			}
		}
	}
	
	function getUrls(json)
	{
		WebSetDataToUnity('getUrls', 'urls=' + json);
	}
	
	function getUUID()
	{
		var info = getQueryVariable("info");

		if(info != -1)
		{
			var json = JSON.parse(decodeURIComponent(info));
			console.log(json);
			return json['uuid'];
		}

		return getCASLOGC("uuid");
	}

	function getCASLOGC(key)
	{
		var r =  unescape(getCookie("CASLOGC"));
		var json = JSON.parse(r);
		if (json[key] == null) return "";
		return json[key];
	}

	function getCookie(name)
	{
		var strcookie = document.cookie;
		var arrcookie = strcookie.split("; ");
		for (var i = 0; i < arrcookie.length; i++)
		{
			var arr = arrcookie[i].split("=");
			if (arr[0] == name)
			{
				return arr[1];
			}
		}
		return "{}";
	}
//代码调用这个方法
    function uploadImage(){
        if(document.getElementById("inputimg")==null){
            var input = document.createElement("input");
            input.setAttribute('type', 'file');
            input.setAttribute('accept', 'image/png');
            input.setAttribute('id', 'inputimg');
            input.setAttribute('hidden', 'true');
            input.setAttribute('onchange', 'uploadImg(event,this)');
            document.body.appendChild(input);
        }
        document.getElementById("inputimg").click();
    }

    function uploadImg(e,dom) {
        var URL = window.URL || window.webkitURL || window.mozURL
        var e = event || e
        var fileObj =
            dom instanceof HTMLElement
            ? dom.files[0]
            : $(dom)[0].files[0]
        console.log(e)
        console.log(dom)
        var container = document.querySelector('.preview')
        var img = new Image()
        img.src = URL.createObjectURL(fileObj)
        console.log(img);
        img.onload = function() {
            //后缀名使用的时候需要判断一下,这里的r即为图片字符串数据
            var r = getImageBase64(img,'png')
            console.log(r);
        }
    }
    
    function getImageBase64(img, ext) {
        var canvas = document.createElement("canvas");
        canvas.width = img.width;
        canvas.height = img.height;
        var ctx = canvas.getContext("2d");
        ctx.drawImage(img, 0, 0, img.width, img.height);
        var dataURL = canvas.toDataURL("image/" + ext);
        canvas = null;
        return dataURL;
    }


var audio;
var recorder;   
  
function startRecording() {  
	HZRecorder.get(
		function (rec) {  
			WebSetDataToUnity('microphoneState', "state=1");
			recorder = rec;  
			recorder.start(); 
		},
		function () {  
			WebSetDataToUnity('microphoneState', "state=0");
		}
	);  
};

function stopRecording(callback){  
    recorder.stop();
};  
  
function playRecording(){  
	if(recorder == null) return;
    recorder.play(audio); 
	recorder.doCallback(function(time)
	{
		console.log(time);
		WebSetDataToUnity('playAudio', 'audio=' + time.toString());
	});
}; 

function stopPlay(){  
	if(recorder == null) return;
    recorder.stop(); 
}; 

function pausePlay(){  
	if(recorder == null) return;
    recorder.pausePlay(); 
}; 

function continuePlay(){  
	if(recorder == null) return;
    recorder.continuePlay(); 
}; 

function uploadRecording()
{
	recorder.upload("https://newbase.zhihuishu.com/upload/commonUploadFile", function(type, e)
	{
		if(type == "ok")
		{
			var jsonStr = JSON.parse(e.target.response);
			console.log(JSON.stringify(jsonStr.rt));
			UnitySetDataFromWeb("getAudio", JSON.stringify(jsonStr.rt));
		}
	});
}

(function (window) {
    //兼容
    window.URL = window.URL || window.webkitURL;
    navigator.getUserMedia = navigator.getUserMedia || navigator.webkitGetUserMedia || navigator.mozGetUserMedia || navigator.msGetUserMedia;

    var HZRecorder = function (stream, config) {
		
		audio = document.createElement('audio') //生成一个audio元素 
		audio.autoplay = "autoplay";
 		document.body.appendChild(audio); //把它添加到页面中
		
        config = config || {};
        config.sampleBits = config.sampleBits || 8;      //采样数位 8, 16
        config.sampleRate = config.sampleRate || (44100 / 6);   //采样率(1/6 44100)

        var context = new (window.webkitAudioContext || window.AudioContext)(); // 创建一个音频环境对象
        var audioInput = context.createMediaStreamSource(stream); // 来关联可能来自本地计算机麦克风或其他来源的音频流MediaStream
        var createScript = context.createScriptProcessor || context.createJavaScriptNode; // 创建一个可以通过JavaScript直接处理音频的ScriptProcessorNode. 
        var recorder = createScript.apply(context, [4096, 1, 1]); // 第二个和第三个参数指的是输入和输出都是双声道。 

        var audioData = {
             size: 0          //录音文件长度
            , buffer: []     //录音缓存 
			, bufferLenght: 0    //录音缓存 上次 已处理的 的长度 
            , inputSampleRate: context.sampleRate    //输入采样率
            , inputSampleBits: 16       //输入采样数位 8, 16
            , outputSampleRate: config.sampleRate    //输出采样率
            , oututSampleBits: config.sampleBits       //输出采样数位 8, 16
            , input: function (data) {
                this.buffer.push(new Float32Array(data));
                this.size += data.length;
            }
            , compress: function (Data) { //合并压缩
			    var  _buffer=(Data=="")?this.buffer:Data;
                //合并
                var data = new Float32Array(this.size);
                var offset = 0;
                for (var i = 0; i < _buffer.length; i++) {
                    data.set(_buffer[i], offset);
                    offset += _buffer[i].length;
                }
                //压缩
                var compression = parseInt(this.inputSampleRate / this.outputSampleRate);
                var length = data.length / compression;
                var result = new Float32Array(length);
                var index = 0, j = 0;
                while (index < length) {
                    result[index] = data[j];
                    j += compression;
                    index++;
                }
                return result;
            }
            , encodeWAV: function (Data) {
                var sampleRate = Math.min(this.inputSampleRate, this.outputSampleRate);
                var sampleBits = Math.min(this.inputSampleBits, this.oututSampleBits);
                var bytes = this.compress(Data);
                var dataLength = bytes.length * (sampleBits / 8);
                var buffer = new ArrayBuffer(44 + dataLength);
                var data = new DataView(buffer);

                var channelCount = 1;//单声道
                var offset = 0;

                var writeString = function (str) {
                    for (var i = 0; i < str.length; i++) {
                        data.setUint8(offset + i, str.charCodeAt(i));
                    }
                }

                // 资源交换文件标识符 
                writeString('RIFF'); offset += 4;
                // 下个地址开始到文件尾总字节数,即文件大小-8 
                data.setUint32(offset, 36 + dataLength, true); offset += 4;
                // WAV文件标志
                writeString('WAVE'); offset += 4;
                // 波形格式标志 
                writeString('fmt '); offset += 4;
                // 过滤字节,一般为 0x10 = 16 
                data.setUint32(offset, 16, true); offset += 4;
                // 格式类别 (PCM形式采样数据) 
                data.setUint16(offset, 1, true); offset += 2;
                // 通道数 
                data.setUint16(offset, channelCount, true); offset += 2;
                // 采样率,每秒样本数,表示每个通道的播放速度 
                data.setUint32(offset, sampleRate, true); offset += 4;
                // 波形数据传输率 (每秒平均字节数) 单声道×每秒数据位数×每样本数据位/8 
                data.setUint32(offset, channelCount * sampleRate * (sampleBits / 8), true); offset += 4;
                // 快数据调整数 采样一次占用字节数 单声道×每样本的数据位数/8 
                data.setUint16(offset, channelCount * (sampleBits / 8), true); offset += 2;
                // 每样本数据位数 
                data.setUint16(offset, sampleBits, true); offset += 2;
                // 数据标识符 
                writeString('data'); offset += 4;
                // 采样数据总数,即数据总大小-44 
                data.setUint32(offset, dataLength, true); offset += 4;
                // 写入采样数据 
                if (sampleBits === 8) {
                    for (var i = 0; i < bytes.length; i++, offset++) {
                        var s = Math.max(-1, Math.min(1, bytes[i]));
                        var val = s < 0 ? s * 0x8000 : s * 0x7FFF;
                        val = parseInt(255 / (65535 / (val + 32768)));
                        data.setInt8(offset, val, true);
                    }
                } else {
                    for (var i = 0; i < bytes.length; i++, offset += 2) {
                        var s = Math.max(-1, Math.min(1, bytes[i]));
                        data.setInt16(offset, s < 0 ? s * 0x8000 : s * 0x7FFF, true);
                    }
                }

                return new Blob([data], { type: 'audio/wav' });
            }
        };
		
		this.getSize = function()
		{
			return audioData.size;
		}
		
        //开始录音
        this.start = function () {
            audioInput.connect(recorder);
            recorder.connect(context.destination);// 连接到输出源
        }

        //停止
        this.stop = function () {
			audioData.bufferLenght=0;
            recorder.disconnect();
        }

        //获取全部音频文件
        this.getBlob = function () {
            this.stop();
            return audioData.encodeWAV("");
        }
		//获取分段音频数据
        this.getSplitBlob = function () {
			var Data=audioData.buffer.slice(audioData.bufferLenght);//截取剩余录音
			audioData.bufferLenght=audioData.buffer.length;//赋值 已处理的 录音长
			var url=window.URL.createObjectURL(audioData.encodeWAV(Data));
            return url;
        }

        //回放
        this.play = function (audio) {
            audio.src = window.URL.createObjectURL(this.getBlob());
        }
		
		this.pausePlay = function()
		{
			audio.pause();
		}
		
		this.continuePlay = function()
		{
			audio.play();
		}
		
		this.getLengh = function()
		{
			return audio.length;
		}
		
		this.doCallback = function(callback)
		{
			audio.oncanplay=function(){
				callback(audio.duration);
				audio.oncanplay = null;
			}
		}
		
        //上传
        this.upload = function (url, callback) {
            var fd = new FormData();
            fd.append("file", this.getBlob(), "audio.mp3");
            var xhr = new XMLHttpRequest();
            if (callback) {
                xhr.upload.addEventListener("progress", function (e) {
                    callback('uploading', e);
                }, false);
                xhr.addEventListener("load", function (e) {
                    callback('ok', e);
                }, false);
                xhr.addEventListener("error", function (e) {
                    callback('error', e);
                }, false);
                xhr.addEventListener("abort", function (e) {
                    callback('cancel', e);
                }, false);
            }
            xhr.open("POST", url);
            xhr.send(fd);
        }
		
        //音频采集
        recorder.onaudioprocess = function (e) {
            audioData.input(e.inputBuffer.getChannelData(0));
            //record(e.inputBuffer.getChannelData(0));
        }
		
		this.getArray = function()
		{
			//将Blob 对象转换成 ArrayBuffer
			var reader = new FileReader();
			//reader.readAsArrayBuffer(blob_exch,'utf-16');
			reader.readAsArrayBuffer(this.getBlob(),'utf-8');
				reader.onload = function (e) {
				var abf=reader.result;
				var typedArray = new Uint8Array(abf);
				console.log(typedArray); 
				var str_vt = arry2str(typedArray);//arrybuffer to string 
				console.log(str_vt);
			}
		}
		
		function arry2str(Arry2str)
		{// string with ','
			return Arry2str.join(',');//arry2str
		}
    };
	
    //抛出异常
    HZRecorder.throwError = function (message) {
        alert(message);
        throw new function () { this.toString = function () { return message; } }
    }
    //是否支持录音
    HZRecorder.canRecording = (navigator.getUserMedia != null);
    //获取录音机
    HZRecorder.get = function (callback, callbackError, config) {
        if (callback) {
            if (navigator.getUserMedia) {
                navigator.getUserMedia(
                    { audio: true } //只启用音频
                    , function (stream) {
                        var rec = new HZRecorder(stream, config);
                        callback(rec);
                    }
                    , function (error) {
						callbackError();
                        switch (error.code || error.name) {
                            case 'PERMISSION_DENIED':
                            case 'PermissionDeniedError':
                                HZRecorder.throwError('用户拒绝提供信息。');
                                break;
                            case 'NOT_SUPPORTED_ERROR':
                            case 'NotSupportedError':
                                HZRecorder.throwError('浏览器不支持硬件设备。');
                                break;
                            case 'MANDATORY_UNSATISFIED_ERROR':
                            case 'MandatoryUnsatisfiedError':
                                HZRecorder.throwError('无法发现指定的硬件设备。');
                                break;
                            default:
                                HZRecorder.throwError('无法打开麦克风。异常信息:' + (error.code || error.name));
                                break;
                        }
                    });
            } else {
				callbackError();
                HZRecorder.throwErr('当前浏览器不支持录音功能。'); return;
            }
        }
    }

    window.HZRecorder = HZRecorder;

})(window);