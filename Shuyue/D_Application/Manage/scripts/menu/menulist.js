managebase.data = {};
managebase.data.initPage = function () {
    managebase.data.loadPage();
    //编辑
    $(".smContent").on("click", ".smEdit", function (e) {
        e.preventDefault();
        var id = $(this).closest("td").data("Id");
        managebase.data.edit(id);
    });
    //新增子菜单
    $(".smContent").on("click", ".smAdd", function (e) {
        e.preventDefault();
        var id = $(this).closest("td").data("Id");
        managebase.data.add(id);
    });
    //删除菜单
    $(".smContent").on("click", ".smDel", function (e) {
        e.preventDefault();
        var id = $(this).closest("td").data("Id");
        managebase.data.delete(id);
    });
}
managebase.data.add = function (id) {
    window.location.href = "/menu/edit?parentId=" + id;
}
managebase.data.edit = function (id) {
    window.location.href = "/menu/edit/" + id;
}
managebase.data.delete = function (id) {
    $.dialog.confirm('你确定要删除这个菜单吗？', function () {
        $.post("/menu/delete", { id: id }, function (d) {
            if (d && d.Code == 0) {
                showTip('删除成功', 3, 0.8);
                managebase.data.refreshTable();
            }
            else {
                showTip('删除失败', 3, 0.8);
            }
        })
    }, function () {
    });
}
managebase.data.refreshTable = function () {
    managebase.data.listTable.fnReloadAjax('/menu/search');
};
managebase.data.loadPage = function () {
    managebase.data.listTable = $("#dataTable").smDataTable({
        "bServerSide": true,
        "sAjaxSource": "/menu/search",
        "sDom": "Rlfrtip",
        "iDisplayLength": 10,
        "bFilter": false,
        "bPaginate": true,
        "bLengthChange": false,
        "sPaginationType": "full_numbers",
        "fnServerParams": function (aoData) {
            aoData.push({ "name": "parentId", "value": $("#parentId").val() });
        },
        "aaSorting": [],
        aoColumns: [
            {
                "mData": "Id",
                "sClass": "center",
                "sWidth": "10%",
            },
            {
                "mData": "Title",
                "sClass": "center",
                "sWidth": "20%",
                "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                    $(nTd).html("<a href=\"/menu/index?parentId=" + oData.Id + "\">" + sData + "</a>");
                }
            },
            {
                "mData": "Description",
                "sClass": "center",
                "sWidth": "20%"
            },
            {
                "mData": "SortValue",
                "sClass": "center",
                "sWidth": "20%"
            },
            {
                "mData": "State",
                "sClass": "center",
                "sWidth": "20%",
                "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                    $(nTd).html(sData == true ? "<a href=\"javascript:;\" class=\"iconf icon-on1 kg\" data-id=\"" + oData.Id + "\"></a>"
                        : "<a href=\"javascript:;\" class=\"iconf icon-off1 kg\" data-id=\"" + oData.Id + "\"></a>");
                }
            },
            {
                "sWidth": "10%",
                "mData": "Id",
                "sClass": "center",
                "bSortable": false,
                "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                    $(nTd).data("Id", sData);
                    $(nTd).html('<div class="tr-icon"><a class="smAdd" href="#"><i class="iconf icon-x-tianjia"></i><div>添加子菜单</div></a><a class="smEdit" href="#"><i class="iconf icon-x-edit1"></i><div>修改</div></a><a class="smDel" href="#"><i class="iconf icon-x-del-01"></i><div>删除</div></a></div>');
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
                url: '/menu/editstate/',
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