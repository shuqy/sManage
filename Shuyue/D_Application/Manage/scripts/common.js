function SetCookie(name, value) {
    var exp = new Date();
    exp.setTime(exp.getTime() + 24 * 3600000);
    document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString() + ";path=/";
}
function GetCookie(name) {
    var arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));
    if (arr != null) {
        return unescape(arr[2]);
    } else {
        return "";
    }
}

//当天时间

var today = new Date();
var todayStr = today.getFullYear() + "-" + (today.getMonth() + 1) + "-" + today.getDate();
var todayLongStr = todayStr + ' ' + today.getHours() + ':' + today.getMinutes();
//延迟显示提示
var Timer = null;
//1、aControl 移入层[$('ul.warp-icon li')]  2、dom显示的层[ul,div..]  3、showTime显示所用时间 
//4、hideTime隐藏所用时间  5、delayTime延迟所用时间[建议不大于1000] 根据需求而定
$(document).on('mousemove', '.tr-icon a', function () {
    //console.log('测试common');
    //var This = this;
    $(this).find('div').show();
    // Timer = setTimeout(function () { $(This).find(dom).show(showTime) }, delayTime);
})
$(document).on('mouseout', '.tr-icon a', function () {
    $(this).find('div').hide();
});
function DelayToggle(aControl, dom, showTime, hideTime, delayTime) {
    //$.isNumeric(showTime) && showTime > 0 ? showTime = showTime : showTime = 0;
    //$.isNumeric(hideTime) && hideTime > 0 ? hideTime = hideTime : hideTime = 0;
    //$.isNumeric(delayTime) && delayTime > 0 ? delayTime = delayTime : delayTime = 0;
    $(document).on('mousemove', '.tr-icon a', function () {
        var This = this;
        Timer = setTimeout(function () { $(This).find(dom).show(showTime) }, delayTime);
    })
    clearTimeout(Timer);
    $(document).on('mouseout', '.tr-icon a', function () {
        $(this).find(dom).hide(hideTime);
    });
    aControl.hover(function () {
        var This = this;
        console.log(This);
        Timer = setTimeout(function () { $(This).find(dom).show(showTime) }, delayTime);
        //alert('测试');
    }, function () {
        clearTimeout(Timer);
        $(this).find(dom).hide(hideTime);
    });
}

function aa(v) {
    var json = null;
    var regex = 'none';
    var alertext1 = '';
    var alertext2 = '';
    var arrJson = null;
    var arrName = null;
    v.focus(function () {
        if ($(this).attr('flag') != undefined)
            $(this).attr('flag', '0');
    }).blur(function () {
        $('.table.ui_dialog').css({ width: '130px' })
        json = $(this).attr('valid');
        var size = $(this).attr('valid-size');
        var arrI = json.split(' ');
        var jsonI = null;
        var temp1 = /\S/;
        var numMax, numMin;

        for (var key in regb) {
            if (key == json) {
                arrJson = regb[key];
            }
            for (var j in arrJson) {
                if (j == 'regex')
                    regex = arrJson[j];
                if (j == 'alertext1')
                    alertext1 = arrJson[j];
                if (j == 'alertext2')
                    alertext2 = arrJson[j];
            }
        }
        //必填项且为空
        if (!temp1.test($(this).val()) && $(this).attr('flag') != undefined) {
            showTip(alertext1, 2, 1.5);
            $(this).attr('flag', '0');
            return;
        }
        //size 属性
        if (size != undefined) {
            //console.log(size.indexOf('sizeMax') >= 0);
            if (size.indexOf('sizeR') >= 0) {
                numMax = size.split(',')[1].split(']')[0].trim();
                numMin = size.split(',')[0].split('[')[1].trim();
                if ($(this).val().trim().length < numMin) {
                    showTip('不能小于' + numMin + '个字符！', 3, 1.5);
                    if ($(this).attr('flag') != undefined) {
                        $(this).attr('flag', '1');
                        return;
                    } else {
                        return;
                    }
                }
                if ($(this).val().trim().length > numMax) {
                    //console.log(size.indexOf('sizeMax') >= 0);
                    showTip('不能超过' + numMax + '个字符！', 3, 1.5);
                    if ($(this).attr('flag') != undefined) {
                        $(this).attr('flag', '1');
                        return;
                    } else {
                        return;
                    }
                }
            } else if (size.indexOf('sizeMin') >= 0) {
                // console.log(size)
                numMin = size.split('[')[1].split(']')[0].trim();
                if ($(this).val().trim().length < numMin) {
                    showTip('不能小于' + numMin + '个字符！', 3, 1.5);
                    if ($(this).attr('flag') != undefined) {
                        $(this).attr('flag', '1');
                        return;
                    } else {
                        return;
                    }
                }
            } else if (size.indexOf('sizeMax') >= 0) {
                numMax = size.split('[')[1].split(']')[0].trim();
                if ($(this).val().trim().length > numMax) {
                    showTip('不能超过' + numMax + '个字符！', 3, 1.5);
                    if ($(this).attr('flag') != undefined) {
                        $(this).attr('flag', '1');
                        return;
                    } else {
                        return;
                    }
                }
            } else {
                return;
            }
        }

        //reg 正则
        if (json != '') {
            console.log(regex.test($(this).val().trim()));
            if (regex.test($(this).val().trim())) {
                if ($(this).attr('flag') != undefined) {
                    $(this).attr('flag', '1');
                    return;
                } else {
                    // showTip(alertext1, 2, 1.5);
                    return;
                }
            } else {
                showTip(alertext2, 2, 1.5);
                return;
            }
        }
    })

}


aa($('[valid]'));

//下拉按钮
function SelectBtn() {
    $('.selectbtn').bind('mouseover', function () {
        $(this).next().show();
    })
    $('.select-btn').bind('mouseover', function () {
        $(this).find('.btn').addClass('active');
        $(this).find('ul').show();
    })
    $('.select-btn').bind('mouseout', function () {
        $(this).find('.btn').removeClass('active');
        $(this).find('ul').hide();
    })
}

$.extend({
    vToggleInput: function (vForm) {
        vForm.find(':text').focus(function () {
            if (!this.initValue) {
                this.initValue = this.value;
            }
            if (this.value === this.initValue) {
                this.value = '';
                $(this).css({ color: '#666' });
            }
        }).blur(function () {
            if (this.value === '' || this.value === null) {
                this.value = this.initValue;
                $(this).css({ color: '#999' });
            }
        });
    }
})


//全选插件
// $.extend({

//     checkedAllToggle: function (aControl, boxName) {
//         //全选(1、点击element[div.class #id]  2、checkbox名称name)
//         aControl.toggle(function () {
//             $('input[name=' + boxName + ']').attr('checked', 'checked');
//             $(this).html('取消全选');
//         }, function () {
//             $('input[name=' + boxName + ']').removeAttr('checked');
//             $(this).html('全选');
//         })
//     }
// });
function CheckedAllToggle(aControl, boxName) {
    aControl.on('click',function(){
        if( $('input[name=' + boxName + ']').prop('checked')){
            $('input[name=' + boxName + ']').prop('checked', false);
            //$(this).val('取消全选');
        }else{
            $('input[name=' + boxName + ']').prop('checked', true);
            //$(this).val('全选');
        }
    })

}


$(function () {
    //下拉按钮
    SelectBtn();

    //侧边菜单伸缩
    $('.leftnav ol').find('li.active').parent().show().siblings('span').addClass('active');
    $('.leftnav span').click(function () {
        $('.leftnav ol').not($(this).next()).slideUp(500);
        $('.leftnav span').not($(this)).removeClass('active');
        $(this).toggleClass('active');
        $(this).next().slideToggle(500);
    })
    $(".leftnav ol").find("li").click(function () {
        $(".leftnav ol").find("li").removeClass('active');
        $(this).addClass('active');
    })

    //侧边栏伸缩
    $(".nav_btn").click(function () {
        if ($(this).hasClass("off")) {
            $(this).removeClass("off");
            $('.leftnav').animate({ marginLeft: '0px' }, "100");
            $('.rightwarp').animate({ marginLeft: '200px' }, "100");
            SetCookie("leftZd", "n");
        } else {
            $(this).addClass("off");
            $('.leftnav').animate({ marginLeft: '-200px' }, "100");
            $('.rightwarp').animate({ marginLeft: '10px' }, "100");
            SetCookie("leftZd", "y")

        }
    })
    if (GetCookie("leftZd") == "y") {
        $(".nav_btn").addClass("off");
        $('.leftnav').animate({ marginLeft: '-200px' }, "100");
        $('.rightwarp').css({ marginLeft: '10px' });
    } else {
        $(".nav_btn").removeClass("off");
        $('.leftnav').animate({ marginLeft: '0px' }, "100");
        $('.rightwarp').css({ marginLeft: '200px' });
    }
    //图标提示
    //DelayToggle($('ul.warp-icon li'), 'div', 100, 100, 400);
    //DelayToggle($('.tr-icon a'), 'div', 100, 100, 400);
    //DelayToggle($('ul.tip-icon li'), 'div', 100, 100, 400);
    //用户弹窗
    DelayToggle($('ul.userul li'), 'ul', 500, 500, 400);
    //全选
    CheckedAllToggle($('#check'), 'ischecked');
    //$.checkedAllToggle($('#check'), 'ischecked');
    //选择
    $(".btn-del").click(function () {
        var str = "";
        $('[name=ischecked]:checkbox:checked').each(function () {
            str += $(this).val() + ",";
        })
        if (str.length > 0) {
            //alert(10);
        } else {
            showTip("请选择要操作的项目", 4);
        }
    })
    //关闭
    $('.icon-exit').click(function () {
        $(this).parent().hide("slow");
    })
})

//正则表达式
var regb = {
    'phone'://手机号
    {
        'regex': /^(13[0-9]|14[5|7]|15[0|1|2|3|5|6|7|8|9]|18[0|1|2|3|5|6|7|8|9])\d{8}$/,
        'alertext1': '手机号为必填项不能为空！',
        'alertext2': '手机号码格式错误！'
    },
    'qq'://QQ号
    {
        'regex': '/[1-9][0-9]{4,}/',
        'alertext1': 'qq号码为必填项不能为空！',
        'alertext2': 'qq号码填写不正确！'
    },
    'name'://
    {
        'regex': '/^[a-zA-Z][a-zA-Z0-9_]{4,15}$/',
        'alertext1': '姓名为必填项不能为空',
        'alertext2': '姓名填写不正确！'
    },
    'password': //长度在6~18之间，只能包含字母、数字和下划线
    {
        'regex': '/^[a-zA-Z]w{5,17}$/',
        'alertext1': '密码为必填项不能为空',
        'alertext2': '密码格式不正确！'
    },
    'sfz'://身份证号(15位、18位数字)
    {
        'regex': /^\d{15}|\d{18}$/,
        'alertext1': '身份证为必填项不能为空',
        'alertext2': '身份证格式不正确！'
    },
    'email'://email
    {
        'regex': /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/,
        'alertext1': '邮箱为必填项不能为空',
        'alertext2': '邮箱格式不正确！'
    },
    'chinese'://汉字
    {
        //'regex': '/^[\u4E00-\u9FA5]+$/',
        'regex': /^[\u4e00-\u9fa5]{0,}$/,
        'alertext1': '必填项，请输入汉字',
        'alertext2': '请输入汉字！'
    },
    'numd'://正整数【0-9】
    {
        'regex': /^[1-9]d*$/,
        'alertext1': '必填项，请输入1-9正整数',
        'alertext2': '请输入1-9正整数！'
    },
    'numf'://负整数
    {
        'regex': /^-[1-9]d*$/,
        'alertext1': '必填项，请输入负整数',
        'alertext2': '请输入负整数！'
    },
    'url'://url(网站地址)
    {
        'regex': '/^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$/',
        'alertext1': '必填项，请输入正确的url',
        'alertext2': 'url格式不正确！'
    },
    'strNoTeshu'://不允许输入特殊符号
    {
        'regex': /^[\u4E00-\u9FA5A-Za-z0-9]+$/,
        'alertext1': '必填项，请从新输入',
        'alertext2': '输入中不能包含特殊符号，请从新输入！'
    },
    'strYEnlish'://只能输入字符串
    {
        'regex': /[A-Za-z]+$/,
        'alertext1': '必填项，请输入字符串!',
        'alertext2': '只能输入字符串，请从新输入！'
    },
    'IP'://IP
    {
        'regex': /\d+\.\d+\.\d+\.\d+/,
        'alertext1': '必填项，请输入IP！',
        'alertext2': '只能输入字符串，请从新输入！'
    },
    'youxiang'://邮箱
    {
        'regex': /\d+\.\d+\.\d+\.\d+/,
        'alertext1': '必填项，请输入IP！',
        'alertext2': '只能输入字符串，请从新输入！'
    },
}

//动画隐藏
function AnimationHide(animation, selector) {
    switch (animation) {
        case 'fadeIn':
            selector.fadeOut();
            break;
        case 'show':
            selector.hide();
            break;
        default:
            selector.fadeIn();
            break;
    }
}
//动画显示
function AnimationShow(animation, selector) {
    switch (animation) {
        case 'fadeIn':
            selector.fadeIn();
            break;
        case 'show':
            selector.show();
            break;
        default:
            selector.fadeIn();
            break;
    }
}
//定位
function Dir(dir, selector) {
    var _left, _top, _bottom, _right;
    if (dir == 'center') {
        _left = ($(document).width() - selector.width()) / 2 + 'px';
        _top = ($(window).height() - selector.height()) / 2 + 'px';
        selector.css({ 'left': _left, 'top': _top });
    } else if (dir == 'centerTop') {
        _left = ($(document).width() - selector.width()) / 2 + 'px';
        _top = 0 + 'px';;
        selector.css({ 'left': _left, 'top': _top })
    } else if (dir == 'centerBottom') {
        _left = ($(document).width() - selector.width()) / 2 + 'px';
        _bottom = 0 + 'px';
        selector.css({ 'left': left, 'bottom': _bottom })
    } else if (dir == 'centerTop20') {
        _left = ($(document).width() - selector.width()) / 2 + 'px';
        _top = 20 + 'px';
        selector.css({ 'left': _left, 'top': _top })
    } else if (dir == 'centerBottom20') {
        _left = ($(document).width() - selector.width()) / 2 + 'px';
        _bottom = 20 + 'px';
        selector.css({ 'left': _left, 'bottom': _bottom })
    } else if (dir == 'left') {
        _top = 0 + 'px';
        _left = 0 + 'px';
        selector.css({ 'left': _left, 'top': top });
    } else if (dir == 'leftCenter') {
        _left = 0 + 'px';
        _top = ($(document).height() - selector.height()) / 2 + 'px';
        selector.css({ 'left': _left, 'top': _top });
    } else {
        _left = ($(document).width() - selector.width()) / 2 + 'px';
        _top = ($(document).height() - selector.height()) / 2 + 'px';
        selector.css({ 'left': _left, 'top': _top });
    }
}
/*
 vs --- 练手msg 1.00
 ypf 2015-11-10
 提示框
 */
function Msg() {
    this.settings = {
        dir: 'center',
        type: 'default',
        animation: 'fadeIn',
        txt: '提示',
        dalay: 3000
    };
}
; (function ($) {
    Msg.prototype.init = function (opt, callback) {
        $.extend(this.settings, opt);
        this.create(callback);
        this.die();
    }
    Msg.prototype.create = function (callback) {
        $(document.body).prepend('<div id="msg"  class=""><div class="msg-icon"><i class="iconf "></i></div><div class="msg-txt">' + this.settings.txt + '</div></div>');
        //背景颜色和图标字体
        switch (this.settings.type) {
            case 'default':
                $('#msg').attr('class', 'defalut');
                $('#msg .iconf').attr('class', 'iconf icon-x-tixing1');
                break;
            case 'err':
                $('#msg').attr('class', 'err');
                $('#msg .iconf').attr('class', 'iconf icon-x-shanchu');
                break;
            case 'alert':
                $('#msg').attr('class', 'alert');
                $('#msg .iconf').attr('class', 'iconf icon-x-alert');
                break;
            case 'pass':
                $('#msg').attr('class', 'pass');
                $('#msg .iconf').attr('class', 'iconf icon-x-tongguo');
                break;
            default:
                $('#msg').attr('class', 'defalut');
                $('#msg .iconf').attr('class', 'iconf icon-x-tixing1');
                break;
        }
        //定位
        Dir(this.settings.dir, $('#msg'));
        //显示
        AnimationShow(this.settings.animation, $('#msg'));
        //回调函数
        if (typeof (callback) == 'undefined') {
            return false;
        } else {
            callback();
        }
    }

    Msg.prototype.die = function () {
        var timer1, timer2;
        var _this = this;
        clearTimeout(timer1);
        clearTimeout(timer2);
        timer1 = setTimeout(function () {
            AnimationHide(_this.settings.animation, $('#msg'));
            timer2 = setTimeout(function () { $('#msg').remove() }, 1000);
        }, this.settings.dalay);
    }
})(jQuery);

/**
 * [confirmDialog 下架弹窗]
 * @param  {[确认下架]} fnOK [回调函数]
 * @return {[type]}      [description]
 */
function confirmDialog(titxt, rightxt, exitxt, fnOK) {

    var str = '<div class="m-cover"></div><div class="dialog1">' +
        '<h4>' + titxt + '</h4>' +
        '<div class="dialog1-footer">' +
        '<button class="btn ccc jsok">' + rightxt + '</button>' +
        '<button class="btn jsexit">' + exitxt + '</button>' +
        '</div>' +
        '</div>';
    if (!$('.dialog1').hasClass('dialog1')) {
        $(document.body).append(str);
    }
    $('.dialog1').css({ 'marginTop': -$('.dialog1').height() / 2 })
    $('.dialog1').show();
    $('.m-cover').show();
    $(document.body).on('click', '.jsexit', function () {
        $('.dialog1').remove();
        $('.m-cover').remove();
    })
    $(".jsok").unbind().click(function () {
        fnOK();
        $('.dialog1').remove();
        $('.m-cover').remove();
    })
}

/**
 * [新提示]---原来弹窗和提示是一个函数当同时出现会有问题
 * @param  {[type]} $ [text：文字 timer：显示时间 icon：图标  callback：回调函数w]
 * @return {[type]}   [description]
 */
; (function ($) {
    $.moduleTip = function (option) {
        var opt = $.extend({ 'text': '提示', 'timer': 2000, 'icon': '1' }, option);
        if (!$('.m-tip').hasClass('m-tip')) {
            var str = '<div class="m-tip">' +
                '<div class="m-tip-table">' +
                '<img  class="m-tip-cell" src=' + iconSelect(opt.icon) + ' />' +
                '<div class="m-tip-cell">' + opt.text + '</div>' +
                '</div>' +
                '</div>';
            $(document.body).append(str);
        } else {
            return false;
        }
        $('.m-tip').fadeIn(1000);
        var dir = function () {
            $('.m-tip').css({
                'marginLeft': -$('.m-tip').outerWidth() / 2,
                'marginTop': -$('.m-tip').outerHeight() / 2
            });
        }
        var someClose = function () {
            opt.timer = opt.timer < 2000 ? 2000 : opt.timer;
            setTimeout(function () {
                setTimeout(function () {
                    $('.m-tip').remove();
                }, 1000)
                $('.m-tip').fadeOut();
            }, opt.timer)
        }
        var fire = function () {
            if (typeof (opt.callback) == 'function') {
                //var _callback = opt.callback;
               setTimeout(opt.callback,opt.timer);
            }
        }
        dir();
        someClose();
        fire();
    }
})(jQuery);
function iconSelect(st) {
    var icont;
    if (st == 0) {
        icont = "http://img.pccoo.cn/website/js/lhgdialog/skins/icons/fail.png";//错误
    } else if (st == 1) {
        icont = "http://img.pccoo.cn/website/js/lhgdialog/skins/icons/succ.png";//正确
    } else if (st == 2) {
        icont = "http://img.pccoo.cn/website/js/lhgdialog/skins/icons/hits.png";//警告
    } else if (st == 3) {
        icont = "http://img.pccoo.cn/website/js/lhgdialog/skins/icons/i.png";//提示   
    } else {
        icont = "http://img.pccoo.cn/website/js/lhgdialog/skins/icons/i.png";//提示   
    }
    return icont;
}
/**
 * 对话框
 * @param  {[type]} $ [description]
 * @return {[type]}   [description]
 */
;(function($){
    $.moduleConfirm = function(option){
        var opt = $.extend({ 'title': '对话框', 'content':'','oktxt': '确认', 'exitxt': '取消'},
            option);
        if (!$('.m-tip').hasClass('m-confirm')) {
            var str = '<div class="m-warp"><div class="m-cover1"></div><div class="m-confirm">' +
                '<h4>' + opt.title + '</h4>' +
                '<p>' + opt.content + '</p>'+
                '<div class="m-confirm-footer">' +
                '<button class="btn btn-lan jsok">' + opt.oktxt + '</button>' +
                '<button class="btn jsexit">' + opt.exitxt + '</button>' +
                '</div>' +
                '</div>'+
                '</div>';
        }else{
            return false;
        }
        $(document.body).append(str);
        $('.m-confirm').css({'marginTop':-$('.m-confirm').height()/2});
        $('.jsexit').click(function(){
            $('.m-warp').remove();
        })

        $('.jsok').click(function () {


            if(typeof(opt.callback)  == 'function'){
                var _a = opt.callback;
                _a();
            }
            $('.m-warp').remove();
        })
    }
})(jQuery);