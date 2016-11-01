$(function () {
    $.fn.smAjaxSubmit = function (param) {

        var md = $(this);
        var form = $(this).is("form") ? $(this) : $(this).find("form");
        var method = form.attr("method") || "POST";
        var url = form.attr('action');
        var success = param.success || $.noop;
        var d = param.data || {};
        var complete = param.complete || $.noop;

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

        return md;
    }
})