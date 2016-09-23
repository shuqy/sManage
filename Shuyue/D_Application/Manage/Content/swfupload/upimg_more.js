function setStatus(cen, status) {
    $("#" + cen).css({ display: "block" });
    $("#" + cen).html(status);
}

function fileQueued(file) {
    try {
        var progress = new FileProgress(this.customSettings.progressBar, this.customSettings.progressStatus);
        setStatus(this.customSettings.progressStatus, "请稍后...");
        $("#" + this.customSettings.progressWrapper).css({ display: "block" });
    } catch (ex) {
        this.debug(ex);
    }
}

function fileQueueError(file, errorCode, message) {
    try {
        if (errorCode === SWFUpload.QUEUE_ERROR.QUEUE_LIMIT_EXCEEDED) {
            alert(message == 0 ? "您已达到上传限制！" : "您最多能选择" + (message > 1 ? "上传" + message + "张图片！" : "1张图片！"));
            return;
        }
        switch (errorCode) {
            case SWFUpload.QUEUE_ERROR.FILE_EXCEEDS_SIZE_LIMIT:
                alert("文件大小请不要超过5M")
                //setStatus("文件大小请不要超过2M");
                //this.debug("Error Code: 文件超过限制, File name: " + file.name + ", File size: " + file.size + ", Message: " + message);
                break;
            case SWFUpload.QUEUE_ERROR.ZERO_BYTE_FILE:
                alert("不能上传0字节的文件")
                //setStatus("不能上传0字节的文件");
                //this.debug("Error Code: 0字节的文件, File name: " + file.name + ", File size: " + file.size + ", Message: " + message);
                break;
            case SWFUpload.QUEUE_ERROR.INVALID_FILETYPE:
                alert("不支持该类型上传");
                //setStatus("不支持该类型上传");
                //this.debug("Error Code: 不支持的文件类型, File name: " + file.name + ", File size: " + file.size + ", Message: " + message);
                break;
            default:
                if (file !== null) {
                    alert("Unhandled Error");
                    //setStatus("Unhandled Error");
                }
                //this.debug("Error Code: " + errorCode + ", File name: " + file.name + ", File size: " + file.size + ", Message: " + message);
                break;
        }
    } catch (ex) {
        this.debug(ex);
    }
}

function fileDialogComplete(numFilesSelected, numFilesQueued) {
    try {
        this.startUpload();
    } catch (ex) {
        this.debug(ex);
    }
}

function uploadStart(file) {
    try {
        setStatus(this.customSettings.progressStatus, "正在上传...");
    } catch (ex) {

    }
    return true;
}

function uploadProgress(file, bytesLoaded, bytesTotal) {
    try {
        var percent = Math.ceil((bytesLoaded / bytesTotal) * 100);
        var progress = new FileProgress(this.customSettings.progressBar, this.customSettings.progressStatus);
        progress.setProgress(percent);
        setStatus(this.customSettings.progressStatus, "已上传" + percent + "%...");
    } catch (ex) {
        this.debug(ex);
    }
}

function uploadSuccess(file, serverData) {

    try {
        //alert("ok");
        setStatus(this.customSettings.progressStatus, "完成");
        var ssurl = serverData.toLowerCase();
        $("#" + this.customSettings.imgId).attr("src", ssurl);
        $("#" + this.customSettings.inputId).val(ssurl);
        $("#" + this.customSettings.progressWrapper).css({ display: "none" });
        $("#" + this.customSettings.progressStatus).html('');
        $("#" + this.customSettings.progressStatus).css({ display: "none" });
    } catch (ex) {
        this.debug(ex);
    }
}

function uploadError(file, errorCode, message) {
    try {
        switch (errorCode) {
            case SWFUpload.UPLOAD_ERROR.HTTP_ERROR:
                setStatus(this.customSettings.progressStatus, "Upload Error: " + message);
                //this.debug("Error Code: HTTP Error, File name: " + file.name + ", Message: " + message);
                break;
            case SWFUpload.UPLOAD_ERROR.UPLOAD_FAILED:
                alert(this.customSettings.progressStatus, "上传失败!");
                //setStatus(this.customSettings.progressStatus,"上传失败!");
                //this.debug("Error Code: Upload Failed, File name: " + file.name + ", File size: " + file.size + ", Message: " + message);
                break;
            case SWFUpload.UPLOAD_ERROR.IO_ERROR:
                alert("上传失败 Server (IO) Error");
                //setStatus("上传失败 Server (IO) Error");
                //this.debug("Error Code: IO Error, File name: " + file.name + ", Message: " + message);
                break;
            case SWFUpload.UPLOAD_ERROR.SECURITY_ERROR:
                alert(this.customSettings.progressStatus, "上传失败 Security Error");
                //setStatus(this.customSettings.progressStatus,"上传失败 Security Error");
                //this.debug("Error Code: Security Error, File name: " + file.name + ", Message: " + message);
                break;
            case SWFUpload.UPLOAD_ERROR.UPLOAD_LIMIT_EXCEEDED:
                alert(this.customSettings.progressStatus, "已到达上传上限！");
                //setStatus(this.customSettings.progressStatus,"已到达上传上限！");
                //this.debug("Error Code: Upload Limit Exceeded, File name: " + file.name + ", File size: " + file.size + ", Message: " + message);
                break;
            case SWFUpload.UPLOAD_ERROR.FILE_VALIDATION_FAILED:
                alert(this.customSettings.progressStatus, "文件验证失败！");
                //setStatus(this.customSettings.progressStatus,"文件验证失败！");
                //this.debug("Error Code: File Validation Failed, File name: " + file.name + ", File size: " + file.size + ", Message: " + message);
                break;
            case SWFUpload.UPLOAD_ERROR.FILE_CANCELLED:
                setStatus(this.customSettings.progressStatus, "Cancelled");
                break;
            case SWFUpload.UPLOAD_ERROR.UPLOAD_STOPPED:
                setStatus(this.customSettings.progressStatus, "Stopped");
                break;
            default:
                setStatus(this.customSettings.progressStatus, "Unhandled Error: " + errorCode);
                this.debug("Error Code: " + errorCode + ", File name: " + file.name + ", File size: " + file.size + ", Message: " + message);
                break;
        }
    } catch (ex) {
        this.debug(ex);
    }
}

function uploadComplete(file) {
   // alert(0);
}

function queueComplete(numFilesUploaded) { }

function FileProgress(barid, statusid) {
    this.progressBarID = "#" + barid;
    this.progressStatusID = "#" + statusid;
    this.reset();
    this.setTimer(null);
}

FileProgress.prototype.setTimer = function (timer) {
    this["FP_TIMER"] = timer;
};
FileProgress.prototype.getTimer = function (timer) {
    return this["FP_TIMER"] || null;
};
FileProgress.prototype.reset = function () {
    $(this.progressBarID).width(0);
};
FileProgress.prototype.setProgress = function (percentage) {
    $(this.progressBarID).width(percentage + "%");
};
FileProgress.prototype.setComplete = function () {
    $(this.progressBarID).width(0);
};
FileProgress.prototype.setError = function () {
    $(this.progressBarID).width(0);
};
FileProgress.prototype.setStatus = function (status) {
    $(this.progressStatusID).html(status);
};

/**/
var ihtml = $('.xc-photo').html();
var ss = '';
var sel;
var curNum = $('.xc-photo').length || 0;
var imgAount = 50;

function uploadSuccess_2(file, serverData) {
    try {
        curNum++;
        if (curNum > imgAount) {
            curNum = imgAount;
            showTip('最多可上传' + imgAount + '张！');
            setTimeout(function () { $('#progressStatus').hide(); }, 100);
            return false;
        }
        var liclass = $('.xc-photo li').attr('class');
        setStatus(this.customSettings.progressStatus, "完成");
        //console.log(this.customSettings.warpDiv);
        var ssurl = serverData.toLowerCase();
        var oLi = '<li class="' + liclass + '"><i class="img-exit">x</i><img src="' + ssurl + '"><input type="hidden" name="imghh" value="' + ssurl + '"></li>';
        $("#" + this.customSettings.warpDiv).append(oLi);
        $("#" + this.customSettings.progressWrapper).css({ display: "none" });
        $("#" + this.customSettings.progressStatus).html('');
        $("#" + this.customSettings.progressStatus).css({ display: "none" });
        $('#temp').remove();
        ss += ssurl + ',';
    } catch (ex) {
        this.debug(ex);
    }
}
function uploadComplete_2(file) {
    try {
        if (this.getStats().files_queued > 0) {
            this.startUpload();
        } else {
            setStatus("完成");
            $("#progressWrapper").css({ display: "none" });
            $("#progressStatus").html('');
            $("#progressStatus").css({ display: "none" });
           // $("#" + this.customSettings.inputId).val(ss.substr(0, ss.length - 1));
            //var dataImg = [];
            //$("input[name='imghh']").each(function () {
            //    dataImg.push($(this).val());
            //})
            sel = $("#" + this.customSettings.warpDiv);
            var arr= [];
            $("#" + this.customSettings.inputId).val(getImgVal(sel)); 
        }
    } catch (ex) {
        this.debug(ex);
    }

}

function getImgVal() {
    var dataImg = [];
     $("input[name='imghh']").each(function () {
        dataImg.push($(this).val());
    });
    return dataImg.join(',');
}
$('body').on('click', '.img-exit', function () {
    $(this).parent().remove();
    //$(this).parent().parent().next().next().next().val(getImgVal());
    $('#titval').val(getImgVal(sel));
    // if ((--curNum) == 1) {
    //     $('.xc-photo').html(ihtml);
    // }
});
