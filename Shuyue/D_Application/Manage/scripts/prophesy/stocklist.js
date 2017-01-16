managebase.data = {};
managebase.data.initPage = function () {
    managebase.data.loadPage();
}
managebase.data.loadPage = function () {
    managebase.data.listTable = $("#dataTable").smDataTable({
        "bServerSide": true,
        "sAjaxSource": "/Prophesy/GetStockList",
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
                "mData": "Id",
                "sClass": "center",
                "bSortable": false,
                "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                    $(nTd).data("Id", sData);
                    $(nTd).html('<div class="tr-icon"><a class="smEdit" href="#"><i class="iconf icon-x-ser"></i><div>查看</div></a></div>');
                }
            }
        ]
    });
}

$(function () {
    managebase.data.initPage();
})