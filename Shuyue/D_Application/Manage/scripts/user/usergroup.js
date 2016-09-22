
managebase.data = {};
managebase.data.initPage = function () {
    managebase.data.loadPage();
    //编辑
    $(".smContent").on("click", ".smEdit", function (e) {
        e.preventDefault();
        var id = $(this).closest("td").data("Id");
        managebase.data.edit(id);
    });
    //设置权限
    $(".smContent").on("click", ".smConfig", function (e) {
        e.preventDefault();
        var id = $(this).closest("td").data("Id");
        managebase.data.config(id);
    });
    //点击新增角色，弹出新增界面
    $("#nCreateRoleBtn").on("click", function (e) {
        e.preventDefault();
        window.location.href = "/UserGroup/Edit";
    });
}
managebase.data.edit = function (id) {
    window.location.href = "/UserGroup/Edit/" + id;
}
managebase.data.config = function (id) {
    window.location.href = "/UserGroup/SetMenu/" + id;
}
managebase.data.loadPage = function () {
    managebase.data.listTable = $("#dataTable").smDataTable({
        "bServerSide": true,
        "sAjaxSource": "/UserGroup/Search",
        "sDom": "Rlfrtip",
        "iDisplayLength": 10,
        "bFilter": false,
        "bPaginate": true,
        "bLengthChange": false,
        "sPaginationType": "full_numbers",
        "fnServerParams": function (aoData) {
        },
        "aaSorting": [],
        aoColumns: [
            {
                "mData": "Title",
                "sClass": "center",
                "sWidth": "20%"
            },
            {
                "mData": "Description",
                "sClass": "center",
                "sWidth": "30%"
            },
            {
                "mData": "Code",
                "sClass": "center",
                "sWidth": "20%"
            },
            {
                "mData": "SortValue",
                "sClass": "center",
                "sWidth": "10%"
            },
            {
                "mData": "State",
                "sClass": "center",
                "sWidth": "10%",
                "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                    $(nTd).html(sData == true ? "<a href=\"javascript:;\" class=\"iconf icon-on1 kg\" data-id=\"" + oData.Id + "\"></a>"
                        : "<a href=\"javascript:;\" class=\"iconf icon-off1 kg\" data-id=\"" + oData.Id + "\"></a>");
                }
            },
            {
                "sWidth": "10%",
                "mData": "Id",
                "sName": "Id",
                "sClass": "center",
                "bSortable": false,
                "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                    $(nTd).data("Id", sData);
                    $(nTd).html('<div class="tr-icon">'
                        + '<a class="smEdit" href="#"><i class="iconf icon-x-edit1"></i><div>修改</div></a>'
                       + '<a class="smConfig" href="#"><i class="iconf icon-x-shezhi"></i><div>设置权限</div></a>'
                       + '<a class="smDel" href="#"><i class="iconf icon-x-del-01"></i><div>删除</div></a></div>');
                }
            }
        ]
    });
}
managebase.data.initPage();

$(function () {
    /*开关 on off*/
    var flag = true;
    $('#dataTable').delegate('a.iconf', 'click', function (e) {
        if (flag) {
            e.preventDefault();
            flag = false;
            var _id = $(this).data('id');
            if (_id == 0) {
                showTip("操作失败！", 0, 3);
                flag = true;
                return;
            }
            var obj = $(this);
            managebase.common.smAjax({
                url: '/UserGroup/EditState/',
                data: { id: _id },
                success: function (data) {
                    flag = true;
                    if (data.Code == 0) {
                        if (obj.hasClass('icon-on1')) {
                            obj.removeClass('icon-on1').addClass('icon-off1');
                        } else {
                            obj.removeClass('icon-off1').addClass('icon-on1');
                        }
                    } else {
                        showTip("操作失败！", 0, 3);
                    }
                }
            });
        }
    });

})