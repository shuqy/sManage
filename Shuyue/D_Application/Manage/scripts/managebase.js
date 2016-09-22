﻿managebase = {};
managebase.common = {};

$.fn.dataTableExt.oApi.fnReloadAjax = function (oSettings, sNewSource, fnCallback, bStandingRedraw, iStart) {
    if (typeof sNewSource != 'undefined' && sNewSource != null) {
        oSettings.sAjaxSource = sNewSource;
    }

    // Server-side processing should just call fnDraw
    if (oSettings.oFeatures.bServerSide) {
        if (iStart != null) {
            oSettings._iDisplayStart = 0; //lin.su 修改查询还保留上次的开始数据行_iDisplayStart ！=0
        }
        this.fnDraw();
        return;
    }
    this.oApi._fnProcessingDisplay(oSettings, true);
    var that = this;
    var iStart = oSettings._iDisplayStart;
    var aData = [];

    this.oApi._fnServerParams(oSettings, aData);

    oSettings.fnServerData.call(oSettings.oInstance, oSettings.sAjaxSource, aData, function (json) {
        /* Clear the old information from the table */
        that.oApi._fnClearTable(oSettings);

        /* Got the data - add it to the table */
        var aData = (oSettings.sAjaxDataProp !== "") ?
            that.oApi._fnGetObjectDataFn(oSettings.sAjaxDataProp)(json) : json;

        for (var i = 0 ; i < aData.length ; i++) {
            that.oApi._fnAddData(oSettings, aData[i]);
        }

        oSettings.aiDisplay = oSettings.aiDisplayMaster.slice();

        if (typeof bStandingRedraw != 'undefined' && bStandingRedraw === true) {
            oSettings._iDisplayStart = iStart;
            that.fnDraw(false);
        }
        else {
            that.fnDraw();
        }

        that.oApi._fnProcessingDisplay(oSettings, false);

        /* Callback user function - for event handlers etc */
        if (typeof fnCallback == 'function' && fnCallback != null) {
            fnCallback(oSettings);
        }
    }, oSettings);
};

managebase.errorHandle = function (status, message) {
    if (status >= 500) {
        alert("系统错误！\r\n\r\n1.请尝试重新登录系统。\r\n\t2.请稍候再试。\r\n\t3.请联系管理员。\r\n\r\n错误详情: \r\n\r\n" + message);
    }
    else if (status > 400) {
        alert("连接错误！\r\n \r\n\t1. 请检查您的网路是否可用。 \r\n\r\n2.请尝试重新登录系统。 ")
    }
}

$.fn.smDatetimepicker = function () {

    return this.each(function () {
        var myControl = {
            create: function (tp_inst, obj, unit, val, min, max, step) {
                $('<input class="ui-timepicker-input" value="' + val + '" style="width:50%">')
                    .appendTo(obj)
                    .spinner({
                        min: min,
                        max: max,
                        step: step,
                        change: function (e, ui) { // key events
                            // don't call if api was used and not key press
                            if (e.originalEvent !== undefined)
                                tp_inst._onTimeChange();
                            tp_inst._onSelectHandler();
                        },
                        spin: function (e, ui) { // spin events
                            tp_inst.control.value(tp_inst, obj, unit, ui.value);
                            tp_inst._onTimeChange();
                            tp_inst._onSelectHandler();
                        }
                    });
                return obj;
            },
            options: function (tp_inst, obj, unit, opts, val) {
                if (typeof (opts) == 'string' && val !== undefined)
                    return obj.find('.ui-timepicker-input').spinner(opts, val);
                return obj.find('.ui-timepicker-input').spinner(opts);
            },
            value: function (tp_inst, obj, unit, val) {
                if (val !== undefined)
                    return obj.find('.ui-timepicker-input').spinner('value', val);
                return obj.find('.ui-timepicker-input').spinner('value');
            }
        };

        $(this).datetimepicker({
            changeMonth: true,
            changeYear: true,
            controlType: myControl
        });
    });
}

$.fn.smAjaxModal = function (param) {
    var p = param || {};
    var url = param.url || "";
    var data = param.data || "";
    var onLoad = param.onLoad || $.noop;
    var onShown = param.onShown || $.noop;
    var onHidden = param.onHidden || $.noop;
    var autowidth = param.width || 0;
    var autoheight = param.height || 0;
    var modal = $(this);
    url = url + "?rnd=" + Math.random();
    var zindex = modal.attr("data-zindex");

    modal.find("iframe").attr("src", "")
    modal.find("iframe").remove()
    modal.css("z-index", zindex);
    modal.empty().off("shown hidden")
        .html("加载中")
        .draggable({ "delay": 100 })
        .load(url, data, function (response, status, xhr) {
            if (status == "error") {
                //   var msg = "Sorry but there was an error: ";
                //  alert(msg + xhr.status + " " + xhr.statusText);
                managebase.errorHandle(xhr.status, xhr.statusText);
                modal.modal("hide");
            }
            else {
                modal.draggable("option", "handle", ".move");
                modal.find("form").smValidationEngine();
                if (autowidth > 0 || autowidth > 0) {
                    var top = ($(window).height() - autoheight * 1.5) / 2;
                    var left = ($(window).width() - autowidth) / 2;
                    modal.css({
                        "height": autoheight,
                        "max-height": autoheight,
                        "min-height": autoheight,
                        "width": autowidth,
                        "min-width": autowidth,
                        "top": top + "px",
                        "left": left + "px"
                    });
                }
                else {
                    if (modal.is(".big")) {
                        var h = $(window).height() - 40;
                        var left = $(window).width() < 1400 ? ($(window).width() * 0.05) / 2 : ($(window).width() - 1400) / 2;
                        var w = $(window).width() < 1400 ? ($(window).width() * 0.95) : 1400;
                        modal.css({
                            "height": h,
                            "max-height": h,
                            "width": w,
                            "top": "20px",
                            "left": left + "px"
                        });

                        var bd = modal.find(".modal-body");
                        var h = modal.height();
                        if (modal.find(".modal-footer").length > 0) {
                            bd.height(h - 90);
                        } else if (modal.find(".modal-footer-220").length > 0) {
                            bd.height(h - 250);
                        } else {
                            bd.height(h - 60);
                        }
                    }
                    else if (modal.is(".middle")) {
                        var h = $(window).height() - 80;
                        var left = $(window).width() < 1400 ? ($(window).width() * 0.12) / 2 : ($(window).width() - 1400) / 2;
                        var w = $(window).width() < 1400 ? ($(window).width() * 0.88) : 1400;
                        modal.css({
                            "height": h,
                            "max-height": h,
                            "width": w,
                            "top": "20px",
                            "left": left + "px"
                        });

                        var bd = modal.find(".modal-body");
                        var h = modal.height();
                        if (modal.find(".modal-footer").length > 0) {
                            bd.height(h - 70);
                        } else if (modal.find(".modal-footer-220").length > 0) {
                            bd.height(h - 250);
                        } else {
                            bd.height(h - 50);
                        }
                    }
                    else if (modal.is(".normal")) {
                        var h = $(window).height() - 80;
                        var left = $(window).width() < 1400 ? ($(window).width() * 0.14) / 2 : ($(window).width() - 1400) / 2;
                        var w = $(window).width() < 1400 ? ($(window).width() * 0.86) : 1400;
                        modal.css({
                            "height": h,
                            "max-height": h,
                            "width": w,
                            "top": "20px",
                            "left": left + "px"
                        });

                        var bd = modal.find(".modal-body");
                        var h = modal.height();
                        if (modal.find(".modal-footer").length > 0) {
                            bd.height(h - 90);
                        } else if (modal.find(".modal-footer-220").length > 0) {
                            bd.height(h - 250);
                        } else {
                            bd.height(h - 90);
                        }
                    } else if (modal.is(".mini")) {
                        var h = $(window).height() - 150;
                        var left = $(window).width() < 1400 ? ($(window).width() * 0.2) / 2 : ($(window).width() - 1400) / 2;
                        var w = $(window).width() < 1400 ? ($(window).width() * 0.8) : 1400;
                        modal.css({
                            "height": h,
                            "max-height": h,
                            "width": w,
                            "top": "20px",
                            "left": left + "px"
                        });

                        var bd = modal.find(".modal-body");
                        var h = modal.height();
                        if (modal.find(".modal-footer").length > 0) {
                            bd.height(h - 90);
                        } else if (modal.find(".modal-footer-220").length > 0) {
                            bd.height(h - 250);
                        } else {
                            bd.height(h - 110);
                        }
                    } else {
                        var h = $(window).height() - 140;
                        var left = $(window).width() < 1400 ? ($(window).width() * 0.74) / 2 : ($(window).width() - 1400) / 2;
                        modal.css({
                            "height": h,
                            "max-height": h,
                            "top": "20px",
                            "left": left + "px"
                        });

                        var bd = modal.find(".modal-body");
                        var h = modal.height();
                        if (modal.find(".modal-footer").length > 0) {
                            bd.height(h - 50);
                        } else {
                            bd.height(h - 40);
                        }
                    }
                }

                onLoad();
            }
        })
        .modal({ backdrop: 'static', zindex: zindex })
        .on("shown", onShown)
        .on("hidden", onHidden)
        .modal("show");


}

$.fn.smAInnerAjaxDialog = function (param) {
    var p = param || {};
    var dialog = $(this);
    var url = param.url || "";
    var size = param.size || "normal";
    var data = param.data || "";
    var onLoad = param.onLoad || $.noop;
    var onShown = param.onShown || $.noop;
    var onHidden = param.onHidden || $.noop;

    var h = 800;
    var w = 600;
    if (param.appendTo) {
        h = param.appendTo.innerHeight();
        w = param.appendTo.innerWidth();
    }
    param.width = w - 60;
    param.height = h - 40;

    param.close = function () {
        onHidden();
        dialog.empty();
    };
    param.resizable = false;
    url = url + "?rnd=" + Math.random();
    dialog
   .empty()
   .html("加载中")
   .load(url, data, function (response, status, xhr) {
       if (status == "error") {
           managebase.errorHandle(xhr.status, xhr.statusText);
           dialog.dialog("hide");
       }
       else {

           dialog.find("form").smValidationEngine();

           onLoad();
       }
   })
   .dialog(param);

    if (param.appendTo) {
        dialog.closest(".ui-dialog").prependTo(param.appendTo).css({ left: "30px", top: "20px" });
    }

    return dialog;
}

$.fn.smAjaxDialog = function (param) {
    var p = param || {};
    var url = param.url || "";
    var size = param.size || "normal";
    var data = param.data || "";
    var onLoad = param.onLoad || $.noop;
    var onShown = param.onShown || $.noop;
    var onHidden = param.onHidden || $.noop;

    var dialog = $(this);
    var h = 800;
    var w = 600;
    if (size == "big") {
        h = $(window).height() - 40;
        w = $(window).width() * 0.96;
    }
    else if (size == "middle") {
        h = $(window).height() - 60;
        w = $(window).width() * 0.86;
    }
    else if (size == "normal") {
        var h = $(window).height() - 60;
        w = $(window).width() * 0.76;
    }
    if (w > 1400) {
        w = 1400;
    }
    param.width = w;
    param.height = h;
    // param.modal = true;
    param.top = "30px";
    param.open = function () { $("body").css({ "overflow": "hidden" }); };
    param.close = function () {

        $("body").removeAttr("style");
        dialog.dialog("hide").empty();

    };
    // param.draggable = false;
    param.resizable = false;
    // param.maxWidth = 1400;
    url = url + "?rnd=" + Math.random();
    dialog
   .empty()
   .html("加载中")
   .load(url, data, function (response, status, xhr) {
       if (status == "error") {
           managebase.errorHandle(xhr.status, xhr.statusText);
           dialog.dialog("hide");
       }
       else {
           dialog.find("form").smValidationEngine();
           onLoad();
       }
   })
   .dialog(param);

    if (!($(this).parent().has("#parent").length > 0)) {
        var parentdiv = $('<div></div>');
        parentdiv.attr('id', 'parent');
        parentdiv.addClass('ui-dialog-buttonpane ui-widget-content ui-helper-clearfix');
        parentdiv.css("backgroundColor", "#F5F5F5");
        parentdiv.css("borderWidth", "0");
        var childdiv = $('<div></div>');
        childdiv.attr('id', 'child');
        childdiv.addClass('ui-dialog-buttonset');
        childdiv.appendTo(parentdiv);
        //var top = $(".ui-dialog").offset().top;
        //top = top - 18;
        //$(".ui-dialog").css("top", top+"px");
        var top = $(this).parent().offset().top;
        top = top - 18;
        $(this).parent().css("top", top + "px");
        parentdiv.appendTo($(this).parent());
    }

    dialog.draggable("option", "handle", ".move");

    return dialog;
}

$.fn.smShowLoading = function () {
    return this.each(function () {
        var loader = $(this).find(".loading");
        if (loader.length == 0) {
            loader = $('<div class="loading"><img src="/Images/loading.gif" /></div>').appendTo($(this));
        }
        var w = loader.parent().width();
        var h = loader.parent().height();
        loader.css({
            width: w,
            height: h
        }).show();
        if ($(this).css("position") == "static") {
            loader.css({ "position": "relative", "top": -h });
        }
        loader.click(function (e) {
            e.preventDefault();
            e.stopPropagation();
        })
    });
}

$.fn.smHideLoading = function () {
    return this.each(function () {
        $(this).find(".loading").hide();
    });
}

$.fn.initEditor = function (param) {
    return this.each(function () {
        var p = param || {};
        p = $.extend({
            tools: 'full',
            width: "100%",
            height: "120px",
            upImgUrl: "/upload.aspx",
            upImgExt: "jpg,jpeg,gif,png"
        }, p);
        $(this).xheditor(p);
    });
}

managebase.common.smAjax = function (param) {

    var data = param.data || {};
    var method = param.method || "POST";
    var dataType = param.dataType || {};
    var url = param.url;
    var success = param.success || $.noop;
    var error = param.error || null;
    var complete = param.complete || $.noop;

    $.ajax({
        cache: false,
        url: url,
        type: method,
        data: data,
        success: success,
        error: function (jqXhr, textStatus, xhr) {

            if (error) {
                error(jqXhr, textStatus, xhr);
            } else {
                managebase.errorHandle(jqXhr.status, xhr.statusText);
            }

        },
        complete: function () {
            complete();
        }
    });

}

$.fn.smAjaxSubmit = function (param) {

    var md = $(this);
    var form = $(this).is("form") ? $(this) : $(this).find("form");
    var method = form.attr("method") || "POST";
    var url = form.attr('action');
    var success = param.success || $.noop;
    var d = param.data || {};
    var complete = param.complete || $.noop;

    if (form.validationEngine("validate")) {
        var d = $.param(d);
        if (d) {
            d = "&" + d;
        }
        var data = form.serialize() + d;
        //md.smShowLoading();
        $.ajax({
            url: url,
            type: method,
            data: data,
            success: success,
            error: function (jqXhr, textStatus, xhr) {
                managebase.errorHandle(jqXhr.status, xhr.statusText);
            },
            complete: function () {
                //md.smHideLoading();
                complete();
            }
        });
    }

    return md;
}

$.fn.smLoad = function (param) {
    var url = param.url;
    if (!url) { alert("Url is required."); }
    var data = param.data || {};
    var success = param.success || $.noop;
    var obj = $(this);
    obj.smShowLoading();
    $.ajax({
        cache: false,
        method: "POST",
        url: url,
        data: data,
        success: function (d) {
            obj.empty().html(d);
            //$(d).appendTo();
            success(d);
        },
        error: function (jqXhr, textStatus, xhr) {
            managebase.errorHandle(jqXhr.status, xhr.statusText);
            obj.smHideLoading();
        },
        complete: function () {
            obj.smHideLoading();
        }
    });
}


$.fn.values = function () {

    if (this.length == 0) return;

    var result = new Array();

    this.each(function () {
        result.push($(this).val());
    });

    return result.join(',');
}

$.fn.inputTip = function () {
    return this.each(function () {
        var c = $(this);
        if (c.val() == "") {
            c.val(c.attr("value2"));
        }
        c.focus(function () {
            if ($(this).attr("value2") == $(this).attr("value")) {
                $(this).val("");
            }
        })
        .blur(function () {
            if ($(this).attr("value") == "") {
                $(this).val($(this).attr("value2"));
            }
        });
    });
}

$.fn.tVal = function () {
    if ($(this).attr("value2") == $(this).attr("value")) {
        return "";
    } else {
        return $(this).val();
    }
}

$.fn.smValidationEngine = function (param) {
    var p = { promptPosition: "bottomRight" };
    p = $.extend(p, param);
    return this.validationEngine(p);
}

$.fn.fnSelectedNodeId = function () {
    var node = $(this).jstree("get_selected");
    if (node.is("li")) {
        return node.attr("data-id");
    }
    return null;
}

$.fn.smDataTable = function (param) {
    param = param || {};
    var fnRowClick = param.fnRowClick || $.noop;
    var fnRowDbClick = param.fnRowDbClick || $.noop;
    var dParam = {
        "bSort": false,
        "iDisplayLength": 10,
        "aLengthMenu": [[10, 20, 30, 50, 100, -1], [10, 20, 30, 50, 100, "全部"]],
        "bAutoWidth": false,
        "bPaginate": false,
        "bFilter": false,
        "bProcessing": true,
        "sServerMethod": "POST",
        "oLanguage": {
            "sProcessing": "<img src='/images/loading.gif' />",
            "sLengthMenu": "显示 _MENU_ 项结果",
            "sZeroRecords": "没有匹配结果",
            "sInfo": " 共计 _TOTAL_ 条记录", //显示第 _START_ 至 _END_ 项结果，共 _TOTAL_ 条
            "sInfoEmpty": "显示第 0 至 0 项结果，共 0 项",
            "sInfoFiltered": "(由 _MAX_ 项结果过滤)",
            "sInfoPostFix": "",
            "sSearch": "",
            "sUrl": "",
            "oPaginate": {
                "sFirst": "首页",
                "sPrevious": "«",
                "sNext": "»",
                "sLast": "尾页"
            }
        }
    };
    param = $.extend(dParam, param);
    var dt = $(this).dataTable(param);

    // regiest click event
    $(dt).find("tbody tr").on("click", function (e) {
        if ($(this).hasClass('row_selected')) {
            $(this).removeClass('row_selected');
            fnRowClick($(this));
        }
        else {
            $(dt).find('tr.row_selected').removeClass('row_selected');
            $(this).addClass('row_selected');
            fnRowClick($(this));
        }
    });

    // regiest dbclick event
    $(dt).find("tbody tr").on("dblclick", function (e) {
        $(dt).find('tr.row_selected').removeClass('row_selected');
        $(this).addClass('row_selected');
        fnRowDbClick($(this));
    });

    // regiest click checkAll
    $(dt).find("input.checkAll").click(function (e) {
        e.stopPropagation();
        $(dt).find("input.rowChk").prop("checked", $(this).is(":checked"));
    });

    return dt;
}

// Get the TR nodes from DataTables
managebase.common.fnRenderBoolCell = function (nTd, key, data, url, success) {
    var html = "";
    if (data == "True" || data == "true" || data == true) {
        html = '<a href="#" title="点击改变为否" ><span class="label label-success">是</span> </a>'
    } else {
        html = '<a href="#" title="点击改变为是" ><span class="label">否</span> </a>'
    }
    var sucFunc = success || $.noop;
    $(html)
        .appendTo($(nTd).empty())
        .click(function (e) {
            e.preventDefault();
            e.stopPropagation();

            $.ajax({
                cache: false,
                url: url,
                data: "Id=" + key + "&state=" + data,
                dataType: "json",
                type: "POST",
                success: function (d) {

                    if (d != null && !d.flag) {
                        return;
                    }

                    nData = (data == "True" || data == "true" || data == true) ? "False" : "True";
                    managebase.common.fnRenderBoolCell(nTd, key, nData, url);
                    sucFunc();
                }
            });
        });
}

$.fn.smUpload = function (para) {
    return $(this).each(function () {
        var p = para || {};
        p = $.extend(p, {
            'swf': '/Content/Uploadify/uploadify.swf',
            'uploader': '/Media/upload',
            'width': "100px",
            'height': '20px',
            'buttonText': '上传文件',
            'fileSizeLimit': '0'
        });
        $(this).uploadify(p);
    });
}
$.fn.smUploadCCOO = function (para) {
    return $(this).each(function () {
        var p = para || {};
        p = $.extend({
            'swf': '/Content/Uploadify/uploadify.swf',
            'uploader': 'http://up1.pccoo.cn/upload.ashx?filesrc=winccoo&mode=s&mw=500&mmode=w&sh=225&sw=300',
            'width': "100px",
            'height': '20px',
            'buttonText': '上传文件',
            'fileSizeLimit': '0'
        }, p);
        $(this).uploadify(p);
    });
}

$.fn.smDatepicker = function (para) {
    var para = para || {};
    var datepicker_CurrentInput;
    para = $.extend(para, {
        changeMonth: true,
        changeYear: true,
        showOtherMonths: true,
        selectOtherMonths: true,
        showButtonPanel: true,
        closeText: "清除",
        beforeShow: function (input, inst) { datepicker_CurrentInput = input; }
    });
    // 绑定“Done”按钮的click事件，触发的时候，清空文本框的值  
    $(".ui-datepicker-close").on("click", function () {
        datepicker_CurrentInput.value = "";
    });
    return this.each(function () {
        $(this).datepicker(para);
    });
};

managebase.common.getDate = function (v) {
    var y = parseInt(v / 10000);
    var m = parseInt((v % 10000) / 100);
    var d = parseInt(v % 100);
    var date = new Date(y, m, d);
    return date.getDay();
};

$.fn.initIndustryControl = function (obj) {
    var p = $(this);
    p.on("change", "select.clienttrade", function () {
        var parentId = $(this).val();
        var clientTradeId2 = p.find("select.clienttrade2");
        if (parentId) {
            $.ajax(
            {
                url: "/dataList/Industry",
                type: "POST",
                data: { parentId: parentId },
                success: function (d) {
                    clientTradeId2.empty().append("<option value=\"\">请选择</option>");
                    if (d != null) {
                        for (var i = 0; i < d.length; i++) {
                            clientTradeId2.append("<option value=\"" + d[i].Id + "\">" + d[i].Text + "</option>");
                        }
                    }
                }
            });
        } else {
            clientTradeId2.empty().append("<option value=\"\">选上级</option>");
        }
    })
}
$.fn.IndustryValue = function () {
    var industry2 = $(this).find("select.clienttrade2").val();

    if (industry2) {
        return industry2;
    }

    var industry = $(this).find("select.clienttrade").val();

    return industry;
}
$.fn.initAreaControl = function (obj) {
    var p = $(this);
    p.on("change", "select.area", function () {
        var parentId = $(this).val();
        var area2 = p.find("select.area2");

        if (parentId) {
            $.ajax(
               {
                   url: "/dataList/Area",
                   type: "POST",
                   data: { parentId: parentId },
                   success: function (d) {
                       area2.empty().append("<option value=\"\">请选择</option>");
                       if (d != null) {
                           for (var i = 0; i < d.length; i++) {
                               area2.append("<option value=\"" + d[i].Id + "\">" + d[i].Text + "</option>");
                           }
                       }
                   }
               });

        } else {

            area2.empty().append("<option value=\"\">选上级</option>");
        }
    })

    p.on("change", "select.area2", function () {
        var parentId = $(this).val();
        var area3 = p.find("select.area3");
        area3.empty();
        if (parentId) {
            $.ajax(
               {
                   url: "/dataList/Area",
                   type: "POST",
                   data: { parentId: parentId },
                   success: function (d) {
                       area3.empty().append("<option value=\"\">请选择</option>");
                       if (d != null) {
                           for (var i = 0; i < d.length; i++) {
                               area3.append("<option value=\"" + d[i].Id + "\">" + d[i].Text + "</option>");
                           }
                       }
                   }
               });

        } else {

            area3.empty().append("<option value=\"\">选上级</option>");
        }
    })
}

$.fn.areaValue = function () {

    var area3 = $(this).find("select.area3").val();

    if (area3) {
        return area3;
    }

    var area2 = $(this).find("select.area2").val();

    if (area2) {
        return area2;
    }

    var area = $(this).find("select.area").val();

    return area;
}

$.fn.dataTableExt.afnSortData['dom-a'] = function (oSettings, iColumn) {
    var aData = [];
    $('td:eq(' + iColumn + ') a', oSettings.oApi._fnGetTrNodes(oSettings)).each(function () {
        aData.push(this.text);
    });
    return aData;
}

$.extend($.fn.dataTableExt.oSort, {
    "chinese-string-asc": function (s1, s2) {
        return s1.localeCompare(s2);
    },
    "chinese-string-desc": function (s1, s2) {
        return s2.localeCompare(s1);
    }
});

$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

/************************************************************************************
***功能描述：实现jquery datatable 输入页码自动翻页功能  
***参数：jqtableID jquery datatable 的ID
***调用示例：GetJqueryTableGo("clientId") 必须在页面加载完就注册这个时间一般在$(function(){})
***时间作者： 2014-12-21 14:48:57 lin.su
************************************************************************************/
function GetJqueryTableGo(jqtableID) {
    if (jqtableID) {
        $("#btntab_" + jqtableID).click(function () {
            var oTable = $('#' + jqtableID).dataTable();

            if (oTable) {
                var oSettings = oTable.fnSettings();
                var pagindex = Math.ceil((oSettings.fnRecordsDisplay()) / oSettings._iDisplayLength);
                var pno = $("#pnot_" + jqtableID).val();
                if (!isNaN(pno)) {
                    if (pagindex >= pno) {
                        oTable.fnPageChange(pno - 1);
                    }
                    else {
                        alert("请输入小于" + pagindex + "的整数！");
                    }

                }
                else {
                    alert("请核对输入页码是否为数字！");
                }
            }
        });
    }
}

//前后台频道导航固定对应一级
managebase.common.AppNavOne = function (valone, value) {
    managebase.common.smAjax({
        url: "/Common/NavOne/",
        success: function (d) {
            if (d != null) {
                valone.html("");
                if (d.length > 0) {
                    var oLi = "";
                    for (var i = 0; i < d.length; i++) {
                        if (d[i].ID == value)
                            oLi += '<option value="' + d[i].ID + '" selected>' + d[i].Title + '</option>';
                        else
                            oLi += '<option value="' + d[i].ID + '">' + d[i].Title + '</option>';
                    }
                    valone.append(oLi);
                }
            }
        }
    });
}

//前后台频道导航固定对应二级
managebase.common.AppNavTwo = function (id, valtwo, value) {
    if (id > 0) {
        managebase.common.smAjax({
            url: "/Common/NavTwo/",
            data: { id: id },
            success: function (d) {
                if (d != null) {
                    valtwo.html("");
                    if (d.length > 0) {
                        var oLi = "";
                        for (var i = 0; i < d.length; i++) {
                            if (d[i].ID == value)
                                oLi += '<option value="' + d[i].ID + '" selected>' + d[i].Title + '</option>';
                            else
                                oLi += '<option value="' + d[i].ID + '">' + d[i].Title + '</option>';
                        }
                        valtwo.append(oLi);
                        valtwo.show();
                    }
                    else {
                        valtwo.html("");
                        valtwo.hide();
                    }
                }
            }
        });
    }
    else {
        valtwo.hide();
    }
}

managebase.common.UserPass = function (id) {
    $.get("/User/ChangePassword/" + id, function (data) {
        $.dialog({
            title: "用户修改密码",
            width: '500px',
            height: '200px',
            lock: true,
            content: data
        });
    });
}