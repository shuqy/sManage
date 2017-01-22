managebase.data = {};
managebase.data.initPage = function () {
    managebase.data.loadPage();
    //更新数据
    $(".smContent").on("click", ".smUpdate", function (e) {
        e.preventDefault();
        var id = $(this).closest("td").data("code");
        managebase.data.update(id);
    });
}
managebase.data.update = function (code) {
    $.dialog.confirm('确定更新？', function () {
        $.post("/Prophesy/UpSingleStockAllHistory", { code: code }, function (d) {
            if (d && d.Code == 0) {
                showTip(d.Message, 3, 0.8);
                managebase.data.refreshTable();
            }
            else {
                showTip(d.Message, 3, 0.8);
            }
        })
    }, function () {
    });
}
managebase.data.refreshTable = function () {
    managebase.data.listTable.fnReloadAjax('/Prophesy/GetStockList');
};
managebase.data.loadPage = function () {
    managebase.data.listTable = $("#dataTable").smDataTable({
        "bServerSide": true,
        "sAjaxSource": "/Prophesy/GetStockList",
        "sDom": "Rlfrtip",
        "iDisplayLength": 20,
        "bFilter": false,
        "bPaginate": true,
        "bLengthChange": false,
        "sPaginationType": "full_numbers",
        "fnServerParams": function (aoData) {
        },
        "aaSorting": [],
        aoColumns: [
            {
                "mData": "FullCode",
                "sClass": "center",
                "sWidth": "10%"
            },
            {
                "mData": "StockName",
                "sClass": "center",
                "sWidth": "10%"
            },
            {
                "mData": "TotalMarketValue",
                "sClass": "center",
                "sWidth": "10%"
            },
            {
                "mData": "CirculationMarketValue",
                "sClass": "center",
                "sWidth": "10%"
            },
            {
                "mData": "MainCount",
                "sClass": "center",
                "sWidth": "10%"
            },
            {
                "mData": "Earning",
                "sClass": "center",
                "sWidth": "10%"
            },
            {
                "mData": "PERatio",
                "sClass": "center",
                "sWidth": "10%"
            },
            {
                "mData": "IndustryName",
                "sClass": "center",
                "sWidth": "10%"
            },
            {
                "sWidth": "10%",
                "mData": "StockCode",
                "sClass": "center",
                "bSortable": false,
                "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                    console.log((oData["IsCreatedTable"] == 1))
                    $(nTd).data("code", sData);
                    $(nTd).html('<div class="tr-icon"><a href="/Prophesy/StockAnalysis?stockCode=' + sData + '"><i class="iconf icon-x-ser"></i><div>查看</div></a>' +
                        '<a class="smUpdate" href="#"><i class="iconf ' + ((oData["IsCreatedTable"] == 1) ? "icon-s-guanzhu" : "icon-x-collect") + '"></i><div>更新数据</div></a></div>');
                }
            }
        ]
    });
}

$(function () {
    managebase.data.initPage();
})