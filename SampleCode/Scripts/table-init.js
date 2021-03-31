// table object
var $table = null;
// init object
var initObjOptions;

// bootstrap table parameter
var BToptions = {
    classes: 'table table-hover table-condensed',
    method: 'post',
    pagination: true,
    reorderableColumns: true,
    striped: true,
    sidePagination: 'server',
    sortOrder: 'desc',
    showColumns: true,
    showRefresh: true,
    clickToSelect: true,
    resizable: true
};

// single data event
function singleRecordEvent(btnId, url) {
    $('#' + btnId).click(function () {
        if (!$(this).hasClass('disabled')) {
            var data = $('#eventsTable').bootstrapTable('getSelections');
            var ids = $.map(data, function (item) {
                return item.ID;
            });
            if (ids.length === 1) {
                location.href = url + '/' + ids[0];
            }
        }
    });
}
function singleRecordParameterEvent(btnId, url, parameter) {
    $('#' + btnId).click(function () {
        if (!$(this).hasClass('disabled')) {
            var data = $('#eventsTable').bootstrapTable('getSelections');
            var ids = $.map(data, function (item) {
                return item.ID;
            });
            if (ids.length === 1) {
                location.href = url + '/' + ids[0] + '?' + parameter;
            }
        }
    });
}

// batch data process
function batchRecordEvent(btnId, url) {
    $('#' + btnId).click(function () {
        if (!$(this).hasClass('disabled')) {
            var data = $('#eventsTable').bootstrapTable('getSelections');
            var ids = $.map(data, function (item) {
                return item.ID;
            });
            if (ids.length > 0) {
                window.location = url + '?' + $.param({ id: ids }, true);
            }
        }
    });
}
function batchRecordParameterEvent(btnId, url, parameter) {
    $('#' + btnId).click(function () {
        if (!$(this).hasClass('disabled')) {
            var data = $('#eventsTable').bootstrapTable('getSelections');
            var ids = $.map(data, function (item) {
                return item.ID;
            });
            if (ids.length > 0) {
                window.location = url + '?' + $.param({ id: ids }, true) + '&' + parameter;
            }
        }
    });
}
function batchDataProcess(options) {
    $('#' + options.btnId).click(function () {
        if (!$(this).hasClass('disabled') && confirm(options.confirmMessage)) {
            var data = $('#eventsTable').bootstrapTable('getSelections');
            var ids = $.map(data, function (item) {
                return item.ID;
            });
            if (ids.length > 0) {
                $.ajax({
                    type: 'POST',
                    url: options.url,
                    data: { id: ids },
                    dataType: 'json',
                    success: function (data) {
                        if (data) {
                            message = '{ "Style": "success", "Message": "{0}" }'.format(options.successMessage);
                            displayMessage();
                            $('#eventsTable').bootstrapTable('refresh');
                            resetButton();
                        }
                        else {
                            message = '{ "Style": "danger", "Message": "{0}" }'.format(options.errorMessage);
                            displayMessage();
                        }
                    },
                    error: function (data, error) {
                        message = '{ "Style": "danger", "Message": "{0}" }'.format(options.systemErrorMessage);
                        displayMessage();
                    }
                });
            }
        }
    });
}

// 下載二進制的檔案
var exportFilename = 'export.pdf';
function downloadBlob(data, textStatus, xhr) {
    var a = document.createElement('a');
    var url = window.URL.createObjectURL(data);
    a.href = url;
    a.download = exportFilename;
    document.body.append(a);
    a.click();
    a.remove();
    window.URL.revokeObjectURL(url);
}

// 初始化一般新增刪除修改查詢 event
function InitBasicEvent(initOptions, initQuery) {
    initObjOptions = initOptions;
    // get column definition
    var columns = null;
    $.ajax({
        type: 'GET',
        url: initOptions.getColumnUrl,
        data: { "functionName": initOptions.functionName },
        async: false,
        dataType: 'json',
        success: function (data) { columns = [data]; },
        error: function (data, error) { }
    });

    // 合併設定
    if (initOptions !== undefined) {
        BToptions = $.extend({ columns: columns, onReorderColumn: initOptions.onReorderColumn, onColumnSwitch: initOptions.onColumnSwitch }, BToptions);
    }

    // init bootstrap table
    $table = $('#eventsTable').bootstrapTable(BToptions);

    // display load error message
    $table.on('load-error.bs.table', function (e, status, res) {
        message = '{ "Style": "danger", "Message": "' + res.responseText + '" }';
        displayMessage();
    });

    // table row selected event
    $table.on('check.bs.table uncheck.bs.table check-all.bs.table uncheck-all.bs.table', function () {
        resetButton();
        var rows = $('#eventsTable').bootstrapTable('getSelections');
        settingButton(rows.length);
    });

    // search event
    $('#search_form_submit').click(function () {
        var search_param = $('#search_form').serialize();
        $table.bootstrapTable('refresh', { url: initOptions.pagingUrl + "?" + search_param });
        resetButton();
        return false;
    });

    // 依權限顯示 function button
    // 檢視
    if (initOptions.detailsBtn) {
        // edit event
        singleRecordEvent('details_btn', initOptions.detailsUrl);
    }
    // 編輯
    if (initOptions.editBtn) {
        // edit event
        singleRecordEvent('edit_btn', initOptions.editUrl);
    }
    // 刪除
    if (initOptions.deleteBtn) {
        // delete event
        var options = {
            btnId: 'delete_btn',
            url: initOptions.deleteUrl,
            confirmMessage: initOptions.confirmMessage,
            successMessage: initOptions.successMessage,
            errorMessage: initOptions.errorMessage,
            systemErrorMessage: initOptions.systemErrorMessage
        };
        batchDataProcess(options)
    }

    if (initQuery === undefined || initQuery === true) {
        $('#search_form_submit').click();
    }
}

// reset button
function resetButton() {
    disableButton('details_btn');
    disableButton('edit_btn');
    disableButton('delete_btn');
    if (typeof selectItemZero === 'function') {
        selectItemZero();
    }
}

// setting button
function settingButton(count) {
    if (count === 0) {
        disableButton('details_btn');
        disableButton('edit_btn');
        disableButton('delete_btn');
        if (typeof selectItemZero === 'function') {
            selectItemZero();
        }
    }
    else if (count === 1) {
        enableButton('details_btn');
        enableButton('edit_btn');
        enableButton('delete_btn');
        if (typeof selectItemOne === 'function') {
            selectItemOne();
        }
    }
    else if (count > 1) {
        disableButton('details_btn');
        disableButton('edit_btn');
        enableButton('delete_btn');
        if (typeof selectItemMulti === 'function') {
            selectItemMulti();
        }
    }
}

// enable button
function enableButton(id) {
    $('#' + id).removeClass('disabled');
}

// disable button
function disableButton(id) {
    $('#' + id).removeClass('disabled').addClass('disabled');
}

//列表的編輯按鈕
function EditFormatter(value, row, index) {
    if (initObjOptions.editBtn) {
        return '<a href="' + initObjOptions.editUrl + '/' + value + '" class="btn btn-success"><i class="fa fa-pencil"></i> 編輯</a>';
    }
    return '';
}

//列表的刪除按鈕
function DeleteFormatter(value, row, index) {
    if (initObjOptions.deleteBtn) {
        return '<button name="single_delete_btn" class="btn btn-danger" onclick="singleDeleteEvent(this)" value="' + value + '"><i class="fa fa-times" aria-hidden="true"></i> 刪除</button>';
    }
    return '';
}

//列表的編輯及刪除按鈕
function SingleEventFormatter(value, row, index) {
    var result = '';
    if (initObjOptions.editBtn) {
        result += '<a href="' + initObjOptions.editUrl + '/' + value + '" class="btn btn-success"><i class="fa fa-pencil" aria-hidden="true"></i> 編輯</a> ';
    }
    if (initObjOptions.deleteBtn) {
        result += '<button name="single_delete_btn" class="btn btn-danger" onclick="singleDeleteEvent(this)" value="' + value + '"><i class="fa fa-times" aria-hidden="true"></i> 刪除</button>';
    }
    return result;
}

//語系顯示
function CultureFormatter(value, row, index) {
    if (null !== value) {
        switch (value) {
            case 1:
                return '繁體中文';
            case 2:
                return '英文';
        }
    }
}

//是否顯示
function BitFormatter(value, row, index) {
    if (null !== value) {
        switch (value) {
            case true:
                return '<font color="green">是</font>';
            case false:
                return '<font color="red">否</font>';
        }
    }
}

//操作記錄顯示
function OperatorFormatter(value, row, index) {
    if (null !== value) {
        switch (value) {
            case "C":
                return '新增';
            case "R":
                return '查詢';
            case "U":
                return '修改';
            case "D":
                return '刪除';
        }
    }
}

//操作結果顯示
function OperatorResultFormatter(value, row, index) {
    if (null !== value) {
        switch (value) {
            case "S":
                return '成功';
            case "F":
                return '失敗';
        }
    }
}

//附件檔案下載顯示
function FileAttachedFormatter(value, row, index) {
    if (value === true) {
        return '<a href="/api/DownloadArchive/get/' + row.ID + '"><i class="fa fa-paperclip"></i></a>';
    }
    return '';
}

//標題置頂的顯示
function TitleStickyTopFormatter(value, row, index) {
    if (row.IsStickyTop === true) {
        return '<i class="fa fa-star"></i> ' + value;
    }
    return value;
}

//置頂的顯示
function StickyTopFormatter(value, row, index) {
    if (value === true) {
        return '<i class="fa fa-star"></i>';
    }
    return '';
}

// 將JSON日期格式，轉為日期格式(yyyy/MM/dd)
function JsonDateFormatter(value, row, index) {
    if (null !== value) {
        var re = /-?\d+/;
        var m = re.exec(value);
        var d = new Date(parseInt(m[0]));
        year = d.getFullYear();
        month = d.getMonth();
        day = d.getDate();
        month = month < 9 ? '0' + (month + 1) : (month + 1);
        day = day < 10 ? '0' + day : day;
        date = String.format('{0}/{1}/{2}', year, month, day);

        return date;
    }
}

// 將JSON日期格式，轉為台灣日期格式(yyyy/MM/dd)
function JsonTaiwanDateFormatter(value, row, index) {
    if (null !== value) {
        var re = /-?\d+/;
        var m = re.exec(value);
        var d = new Date(parseInt(m[0]));
        year = d.getFullYear() - 1911;
        month = d.getMonth();
        day = d.getDate();
        year = year < 100 ? '0' + year : year;
        month = month < 9 ? '0' + (month + 1) : (month + 1);
        day = day < 10 ? '0' + day : day;
        date = String.format('{0}/{1}/{2}', year, month, day);

        return date;
    }
}

// 將json日期時間格式，轉為日期時間格式(yyyy/MM/dd hh:mm:ss)
function JsonTimeFormatter(value, row, index) {
    if (null !== value) {
        var re = /-?\d+/;
        var m = re.exec(value);
        var d = new Date(parseInt(m[0]));
        year = d.getFullYear();
        month = d.getMonth();
        day = d.getDate();
        month = month < 9 ? '0' + (month + 1) : (month + 1);
        day = day < 10 ? '0' + day : day;
        s = String.format('{0}/{1}/{2}', year, month, day);
        var h = d.getHours().toString();
        h = h.length > 1 ? h : '0' + h;
        var mm = d.getMinutes().toString();
        mm = mm.length > 1 ? mm : '0' + mm;
        var ss = d.getSeconds().toString();
        ss = ss.length > 1 ? ss : '0' + ss;

        return s + ' ' + h + ':' + mm + ':' + ss;
    }
}