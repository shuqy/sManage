$(function () {

});

//顶部提示语
function ShowTooltips(message) {
    $(".js_tooltips").html("").html(message);
    $('.js_tooltips').show();
    setTimeout(function () {
        $('.js_tooltips').hide();
    }, 3000);
}

//ajax 请求时 loading
function Loading(info) {
    var htmlString = "<div class=\"weui_mask_transparent\"></div><div class=\"weui_toast\">";
    htmlString += "<div class=\"weui_loading\"><div class=\"weui_loading_leaf weui_loading_leaf_0\"></div>";
    htmlString += "<div class=\"weui_loading_leaf weui_loading_leaf_1\"></div><div class=\"weui_loading_leaf weui_loading_leaf_2\"></div>";
    htmlString += "<div class=\"weui_loading_leaf weui_loading_leaf_3\"></div><div class=\"weui_loading_leaf weui_loading_leaf_4\"></div>";
    htmlString += " <div class=\"weui_loading_leaf weui_loading_leaf_5\"></div><div class=\"weui_loading_leaf weui_loading_leaf_6\"></div>";
    htmlString += "<div class=\"weui_loading_leaf weui_loading_leaf_7\"></div><div class=\"weui_loading_leaf weui_loading_leaf_8\"></div>";
    htmlString += "<div class=\"weui_loading_leaf weui_loading_leaf_9\"></div><div class=\"weui_loading_leaf weui_loading_leaf_10\"></div>";
    htmlString += "<div class=\"weui_loading_leaf weui_loading_leaf_11\"></div></div><p class=\"weui_toast_content\">" + info + "</p></div>";
    $("#loadingToast").html("").html(htmlString);
    $("#loadingToast").show();
}

//弹出提示框
function ShowToast(msg) {
    var htmlString="<div class=\"weui_mask_transparent\"></div>";
    htmlString += "<div class=\"weui_toast\"><i class=\"weui_icon_toast\"></i>";
    htmlString += "<p class=\"weui_toast_content\">" + msg + "</p></div>";
    $("#toast").html("").html(htmlString);
    $("#toast").show();
    setTimeout(function () {
        $('#toast').hide();
    }, 2000);
}
//获取地址栏参数值
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}