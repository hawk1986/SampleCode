
// ajax get data
function GetJsonData(url, param, callback, errorCallback, sync) {
    if (sync == undefined) {
        sync = true;
    }
    $.ajax({
        type: 'GET',
        url: url,
        data: param,
        async: sync,
        dataType: 'json',
        success: callback,
        error: errorCallback
    });
}


// ajax get data
function GetHtmlData(url, postData, callback, errorCallback) {
    $.ajax({
        type: 'GET',
        url: url,
        data: postData,
        dataType: 'html',
        success: callback,
        error: errorCallback
    });
}

// ajax post data
function PostHtmlData(url, postData, callback, errorCallback) {
    $.ajax({
        type: 'POST',
        url: url,
        data: postData,
        dataType: 'html',
        success: callback,
        error: errorCallback
    });
}



function hideColumn(columns) {
    $.each(columns, function (k, v) {
        $table.bootstrapTable('hideColumn', v);
    });
}

/**時間日期相關function**/
//格式化日期時間格式
Date.prototype.format = function (format) {
    var date = {
        "M+": this.getMonth() + 1,
        "d+": this.getDate(),
        "h+": this.getHours(),
        "m+": this.getMinutes(),
        "s+": this.getSeconds(),
        "q+": Math.floor((this.getMonth() + 3) / 3),
        "S+": this.getMilliseconds()
    };
    if (/(y+)/i.test(format)) {
        format = format.replace(RegExp.$1, (this.getFullYear() + '').substr(4 - RegExp.$1.length));
    }
    for (var k in date) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1
                ? date[k] : ("00" + date[k]).substr(("" + date[k]).length));
        }
    }
    return format;
}
//時間差距計算
Date.prototype.dateDiff = function (interval, objDate) {
    var dtEnd = new Date(objDate);
    if (isNaN(dtEnd)) return undefined;
    switch (interval) {
        case "s": return parseInt((dtEnd - this) / 1000);  //秒
        case "n": return parseInt((dtEnd - this) / 60000);  //分
        case "h": return parseInt((dtEnd - this) / 3600000);  //時
        case "d": return parseInt((dtEnd - this) / 86400000);  //天
        case "w": return parseInt((dtEnd - this) / (86400000 * 7));  //週
        case "m": return (dtEnd.getMonth() + 1) + ((dtEnd.getFullYear() - this.getFullYear()) * 12) - (this.getMonth() + 1);  //月份
        case "y": return dtEnd.getFullYear() - this.getFullYear();  //天
    }
}
//取得月份的天數
function daysInMonth(iMonth, iYear) {
    return 32 - new Date(iYear, iMonth, 32).getDate();
}
//取得AddDayCount天後的日期
function GetDateStr(date, AddDayCount) {
    var dd = date;
    dd.setDate(dd.getDate() + AddDayCount);//獲取AddDayCount天後的日期
    var y = dd.getFullYear();
    var m = dd.getMonth() + 1;//獲取當前月份的日期
    var d = dd.getDate();
    if (m.toString().length == 1) m = '0' + m;
    if (d.toString().length == 1) d = '0' + d;
    return m + "/" + d;
}
//把日期加減number(interval)
function DateAdd(interval, number, date) {
    var newdate = new Date(date.format("yyyy/MM/dd hh:mm:ss"))
    switch (interval) {
        case "y ": {
            newdate.setFullYear(newdate.getFullYear() + number);
            return newdate;
        }
        case "q ": {
            newdate.setMonth(newdate.getMonth() + number * 3);
            return newdate;
        }
        case "m ": {
            newdate.setMonth(newdate.getMonth() + number);
            return date;
        }
        case "w ": {
            newdate.setDate(newdate.getDate() + number * 7);
            return newdate;
        }
        case "d ": {
            newdate.setDate(newdate.getDate() + number);
            return newdate;
        }
        case "h ": {
            newdate.setHours(newdate.getHours() + number);
            return newdate;
        }
        case "mm ": {
            newdate.setMinutes(newdate.getMinutes() + number);
            return newdate;
        }
        case "s ": {
            newdate.setSeconds(newdate.getSeconds() + number);
            return newdate;
        }
        default: {
            newdate.setDate(newdate.getDate() + number);
            return newdate;

        }
    }
}
//將日期轉為utc
function DateToUTC(date) {
    return new Date(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate(), date.getUTCHours(), date.getUTCMinutes(), date.getUTCSeconds());
}
//秒數轉時間格式(hh:mm:ss)
function formatSecond(secs) {
    var hr = Math.floor(secs / 3600);
    var min = Math.floor((secs - (hr * 3600)) / 60);
    var sec = parseInt(secs - (hr * 3600) - (min * 60));
    var h = "";
    if (hr > 0) h = paddingLeft(hr.toString(), 2) + ':';
    return h + paddingLeft(min.toString(), 2) + ':' + paddingLeft(sec.toString(), 2);
}
/**時間日期相關function**/

//年齡計算
function ages(str) {
    var r = str.match(/^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})$/);
    if (r == null) return false;
    var d = new Date(r[1], r[3] - 1, r[4]);
    if (d.getFullYear() == r[1] && (d.getMonth() + 1) == r[3] && d.getDate() == r[4]) {
        var Y = new Date().getFullYear();
        return (Y - r[1]);
    }
    return ("輸入的日期格式錯誤！");
}

//將表單資料Reset
function FormReset() {
    $('button[type="reset"]').click(function () {
        var form = $(this).parent('form');
        $(form).find('input').each(function (index, item) {
            var type = $(item).attr('type');
            switch (type) {
                case 'radio':
                    $(item).RemoveAttr('checked');
                    break;
                case 'checkbox':
                    $(item).RemoveAttr('checked');
                    break;
                default:
                    $(item).val('').text('');
            }
        });
        $(form).find('select').each(function (index, item) {
            $(item).find('option:eq(0)').prop('selected', true);
        });
        $('.selectpicker').selectpicker('refresh');
        $('#search_form_submit').click();
    });
}

//顯示遮罩，loading icon是否顯示
function popupDiv(type, loading) {
    if (type == 'show') {
        //添加並顯示遮罩   
        $('<div id="mask"></div>').addClass("mask")
            .appendTo("body")
            .fadeIn(200);
        if (loading == null)
            $(".loading").show();
    } else {
        $("#mask").remove();
        $(".loading").hide();
    }
}

//數字左側補0，length幾位數
function paddingLeft(str, lenght) {
    if (str.length >= lenght)
        return str;
    else
        return paddingLeft("0" + str, lenght);
}

//視窗右下方顯示Alarm訊息
function AlarmMessage(message, sec, alertType) {
    if (alertType == null)
        alertType = 'success';
    var template = '<div class="alert alert-{1} alert-dismissable alarmMessage"><button class="close" type="button">x</button>{0}</div>';
    var result = $(template.format(message, alertType));
    $("body").append(result);
    result.find(".close").click(function () {
        result.animate({ right: '-' + (result.width() + 55) + 'px' });
        setTimeout(function () { result.remove(); }, 1000);
    });
    result.animate({ right: '15px' });
    if (sec != null) {
        setTimeout(function () {
            result.animate({ right: '-' + (result.width() + 55) + 'px' });
            setTimeout(function () { result.remove(); }, 1000);
        }, sec);
    }
}

//顯示alert訊息
function AlertMessage(obj) {
    reposition($("#alarmModal"));
    $("#alarmModal .msg-alert").html(obj.msg);
    alarmObj = obj;
    switch (obj.type) {
        case 'alert':
            $("#alarmModal #close_btn").hide();
            $("#alarmModal #ok_btn").show();
            //系統訊息視窗關閉時呼叫
            $('#alarmModal').on('hidden.bs.modal', function () {
                if (typeof video == 'object' && (typeof play == 'boolean' && !play || typeof play != 'boolean'))
                    video.show();
                $('#myModal').unbind('hidden.bs.modal');
            });
            break;
        case 'confirm':
            $("#alarmModal #close_btn").show();
            $("#alarmModal #ok_btn").show();
            break;
    }
    if (typeof video == 'object')
        video.hide();
    $("#alarmModal").modal('show');
    if (obj.hide) {
        setTimeout(function () {
            $("#alarmModal").modal('hide');
        }, obj.timeoutSec);
    }
}
function alarmOK() {
    if (alarmObj.callback != null) {
        alarmObj.callback()
    }
}
function alarmClose() {
    if (alarmObj.err_callback != null) {
        alarmObj.err_callback();
    }
}

//打開Dialog對話視窗
function OpenDialog(obj) {
    reposition(obj.dialog);
    if (obj.video != null && typeof obj.video == 'object')
        obj.video.hide();
    obj.dialog.modal('show');
    if (obj.dialog.find(".modal-body").html() == "" || obj.update) {
        obj.dialog.find(".modal-body").html("<div class='modalLoading'><i class='fa fa-spinner fa-pulse fa-fw'></i></div>");
        setTimeout(function () {
            if (obj.postData != null)
                obj.postData.random = Math.random();
            else
                obj.postData = { random: Math.random() };
            var callback = function (data) {
                if (obj.iframe) {
                    obj.dialog.find(".modal-body").html('<iframe id="dialog_frame" scrolling="no" width="' + obj.iframeWidth + '" height="' + obj.iframeHeight + '" frameborder="0"></iframe>');
                    var ifrm = document.getElementById('dialog_frame');
                    ifrm = (ifrm.contentWindow) ? ifrm.contentWindow : (ifrm.contentDocument.document) ? ifrm.contentDocument.document : ifrm.contentDocument;
                    ifrm.document.open();
                    ifrm.document.write(data);
                    ifrm.document.close();
                    ifrm.load(function () {
                        if (obj.callfun != null)
                            obj.callfun();
                    })
                } else {
                    obj.dialog.find(".modal-body").html(data);
                    //去除連結虛線
                    RemovelinkDottedLine();
                    if (obj.callfun != null)
                        obj.callfun();
                }
            };
            var errorCallback = function (data, error) { };
            GetHtmlData(obj.url, obj.postData, callback, errorCallback);
        }, 500);
    } else {
        if (obj.clear != null)
            obj.clear();
        if (obj.callfun != null)
            obj.callfun();

    }
}

//將Dialog視窗垂直置中
function reposition(obj) {
    var modal = obj,
        dialog = modal.find('.modal-dialog');
    modal.css('display', 'block');
    var top = ($(window).height() - dialog.height()) / 2;
    dialog.css("margin-top", Math.max(0, top));

}

//判斷瀏覽器版本
function detectBrowser() {
    var isIE = navigator.userAgent.search("MSIE") > -1;
    var isIE7 = navigator.userAgent.search("MSIE 7") > -1;
    var isFirefox = navigator.userAgent.search("Firefox") > -1;
    var isOpera = navigator.userAgent.search("Opera") > -1;
    var isChrome = navigator.userAgent.search("Safari") > -1;//Google瀏覽器是用這核心
    var browser = 'IE';
    if (isIE7) {
        browser = 'IE7';
    }
    if (isIE) {
        browser = 'IE';
    }
    if (isFirefox) {
        browser = 'Firefox';
    }
    if (isOpera) {
        browser = 'Opera';
    }
    if (isChrome) {
        browser = 'Safari/Chrome';
    }
    return browser;
}

//去除<a>虛線
function RemovelinkDottedLine() {
    $("a").focus(function () { $(this).blur(); });
}

//num：四捨五入數值，len：保留小數位數
function GetRound(num, len) {
    return Math.round(num * Math.pow(10, len)) / Math.pow(10, len);
}

//D3 number(正整數)
function D3_Number(objID, start, end, duration, isFormat) {
    if (isFormat == null) isFormat = true;
    if (duration == null) duration = 2000;
    var show = d3.select(objID);
    show.transition()
        .duration(duration)
        .tween("number", function () {
            var i = d3.interpolateRound(start, end);
            return function (t) {
                this.textContent = (!isFormat) ? i(t) : d3.format(",")(i(t));
            };
        });

}

//D3 float(浮點數)
function D3_FloatName(obj, start, end, duration) {
    var show = d3.select(obj);
    show.transition()
        .duration(duration)
        .tween("text", function () {
            var i = d3.interpolate(start, end),
                prec = end.split("."),
                round = (prec.length > 1) ? 10 : 1;
            return function (t) {
                this.textContent = Math.round(i(t) * round) / round;
            };
        });
}

//計算map座標兩點距離
function GetDistance(lat1, lng1, lat2, lng2) {
    var dis = 0;
    var radLat1 = toRadians(lat1);
    var radLat2 = toRadians(lat2);
    var deltaLat = radLat1 - radLat2;
    var deltaLng = toRadians(lng1) - toRadians(lng2);
    var dis = 2 * Math.asin(Math.sqrt(Math.pow(Math.sin(deltaLat / 2), 2) + Math.cos(radLat1) * Math.cos(radLat2) * Math.pow(Math.sin(deltaLng / 2), 2)));
    return dis * 6378137 / 1000;

    function toRadians(d) { return d * Math.PI / 180; }
}

//設定cookie
function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

//取得cookie
function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

/**google map座標轉baidu map座標**/
var x_pi = 3.14159265358979324 * 3000.0 / 180.0;
var pi = 3.14159265358979324;
var a = 6378245.0;
var ee = 0.00669342162296594323;
function google_bd_encrypt(gg_lat, gg_lon) {

    var x = gg_lon, y = gg_lat;
    var z = Math.sqrt(x * x + y * y) + 0.00002 * Math.sin(y * x_pi);
    var theta = Math.atan2(y, x) + 0.000003 * Math.cos(x * x_pi);
    var bd_lon = z * Math.cos(theta) + 0.0065;
    var bd_lat = z * Math.sin(theta) + 0.006;
    return [bd_lon, bd_lat];
}

function wgs_gcj_encrypts(wgLat, wgLon) {
    if (outOfChina(wgLat, wgLon)) {
        point.setLat(wgLat);
        point.setLng(wgLon);
        return point;
    }
    var dLat = transformLat(wgLon - 105.0, wgLat - 35.0);
    var dLon = transformLon(wgLon - 105.0, wgLat - 35.0);
    var radLat = wgLat / 180.0 * pi;
    var magic = Math.sin(radLat);
    magic = 1 - ee * magic * magic;
    var sqrtMagic = Math.sqrt(magic);
    dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * pi);
    dLon = (dLon * 180.0) / (a / sqrtMagic * Math.cos(radLat) * pi);
    var lat = wgLat + dLat;
    var lon = wgLon + dLon;
    return [lon, lat];
}


function transform(wgLat, wgLon, latlng) {
    if (outOfChina(wgLat, wgLon)) {
        latlng[0] = wgLat;
        latlng[1] = wgLon;
        return;
    }
    var dLat = transformLat(wgLon - 105.0, wgLat - 35.0);
    var dLon = transformLon(wgLon - 105.0, wgLat - 35.0);
    var radLat = wgLat / 180.0 * pi;
    var magic = Math.sin(radLat);
    magic = 1 - ee * magic * magic;
    var sqrtMagic = Math.sqrt(magic);
    dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * pi);
    dLon = (dLon * 180.0) / (a / sqrtMagic * Math.cos(radLat) * pi);
    latlng[0] = wgLat + dLat;
    latlng[1] = wgLon + dLon;
}

function outOfChina(lat, lon) {
    if (lon < 72.004 || lon > 137.8347)
        return true;
    if (lat < 0.8293 || lat > 55.8271)
        return true;
    return false;
}

function transformLat(x, y) {
    var ret = -100.0 + 2.0 * x + 3.0 * y + 0.2 * y * y + 0.1 * x * y + 0.2 * Math.sqrt(Math.abs(x));
    ret += (20.0 * Math.sin(6.0 * x * pi) + 20.0 * Math.sin(2.0 * x * pi)) * 2.0 / 3.0;
    ret += (20.0 * Math.sin(y * pi) + 40.0 * Math.sin(y / 3.0 * pi)) * 2.0 / 3.0;
    ret += (160.0 * Math.sin(y / 12.0 * pi) + 320 * Math.sin(y * pi / 30.0)) * 2.0 / 3.0;
    return ret;
}

function transformLon(x, y) {
    var ret = 300.0 + x + 2.0 * y + 0.1 * x * x + 0.1 * x * y + 0.1 * Math.sqrt(Math.abs(x));
    ret += (20.0 * Math.sin(6.0 * x * pi) + 20.0 * Math.sin(2.0 * x * pi)) * 2.0 / 3.0;
    ret += (20.0 * Math.sin(x * pi) + 40.0 * Math.sin(x / 3.0 * pi)) * 2.0 / 3.0;
    ret += (150.0 * Math.sin(x / 12.0 * pi) + 300.0 * Math.sin(x / 30.0 * pi)) * 2.0 / 3.0;
    return ret;
}
/**google map座標轉baidu map座標**/

function Drag(event) {
    var dt = event.dataTransfer;
    if (dt.getData("text") == "javascript::;")
        dt.setData("text", event.currentTarget.id);
}

function DragChannel(event) {
    event.dataTransfer.setData("text", $(event.currentTarget).attr("ch"));
}

function AllowDrop(event) {
    event.preventDefault();
}

function DragScreen(FrameObj, ImgObj, contentObj, ratioObj, top, left, ratioSlider, mapModal, hasLine) {
    ratioSlider = ratioObj.bootstrapSlider({
        min: 20,
        max: 100,
    });
    ratioObj.bootstrapSlider().on('change', function (data) {
        ratio = data.value.newValue;
        ImgObj.css("zoom", ratio + '%');
        contentObj.css("zoom", ratio + '%');
    });

    FrameObj.mousedown(function (event) {
        $(this)
            .data('down', true)
            .data('x', event.clientX)
            .data('scrollLeft', this.scrollLeft)
            .data('y', event.clientY)
            .data('scrollTop', this.scrollTop);
        return false;
    }).mouseup(function (event) {
        $(this).data('down', false);
    }).mousemove(function (event) {
        if ($(this).data('down') == true) {
            this.scrollLeft = $(this).data('scrollLeft') + $(this).data('x') - event.clientX;
            this.scrollTop = $(this).data('scrollTop') + $(this).data('y') - event.clientY;
        }
    }).css({
        'overflow': "hidden",
        'cursor': '-moz-grab'
    });

    ImgObj.on('touchstart', function (e) {
        console.log(e);
        e.stopPropagation();
        e.preventDefault();

        if (e.originalEvent.targetTouches.length == 1) {
            var event = e.originalEvent.changedTouches[0];
            FrameObj
                .data('down', true)
                .data("pinch", false)
                .data('x', event.clientX)
                .data('scrollLeft', FrameObj.scrollLeft())
                .data('y', event.clientY)
                .data('scrollTop', FrameObj.scrollTop());
        } else {
            var point1 = e.originalEvent.touches[0];
            var point2 = e.originalEvent.touches[1];
            var xLen = Math.abs(point2.pageX - point1.pageX);
            var yLen = Math.abs(point2.pageY - point1.pageY);
            FrameObj
                .data('touchDistance', _getDistance(xLen, yLen))
                .data('changedY', point1.clientY)
                .data('changedX', point1.clientX);
        }
        return false;
    }).on('touchmove', function (e) {
        //$("#msg").append(printObject(e.originalEvent.targetTouches.length) + "<br>");
        //$("#msg").append(e.originalEvent.targetTouches.length + "<br>");
        if (e.originalEvent.targetTouches.length == 1) {
            var event = e.originalEvent.changedTouches[0];
            if (FrameObj.data('down') == true && FrameObj.data("pinch") == false) {
                FrameObj.scrollLeft(FrameObj.data('scrollLeft') + FrameObj.data('x') - event.clientX);
                FrameObj.scrollTop(FrameObj.data('scrollTop') + FrameObj.data('y') - event.clientY);
            }
        } else {
            var xLen = Math.abs(e.originalEvent.touches[0].pageX - e.originalEvent.touches[1].pageX);
            var yLen = Math.abs(e.originalEvent.touches[0].pageY - e.originalEvent.touches[1].pageY);
            var touchDistance = _getDistance(xLen, yLen);
            if (FrameObj.data('touchDistance')) {
                var pinchScale = touchDistance - FrameObj.data('touchDistance');
                var zoom = ratio;
                var size = false;
                if (pinchScale < 0) {
                    // moved down
                    if ((zoom - 2) >= ratioMin) {
                        zoom = zoom - 2;
                        ImgObj.css("zoom", zoom + '%');
                        contentObj.css("zoom", zoom + '%');
                        size = true;
                    }
                } else {
                    // moved up
                    if ((zoom + 2) <= ratioMax) {
                        zoom = zoom + 2;
                        ImgObj.css("zoom", zoom + '%');
                        contentObj.css("zoom", zoom + '%');
                        size = true;
                    }
                }
                if (size) {
                    ratioSlider.bootstrapSlider('setValue', zoom);
                    var tempX = FrameObj.data('changedX') - left + $(this).scrollLeft();
                    var tempY = FrameObj.data('changedY') - top + $(this).scrollTop();
                    //var obj = el.target.getBoundingClientRect();
                    FrameObj.scrollLeft(((tempX + FrameObj.scrollLeft()) * 100 / ratio * zoom / 100) - tempX);
                    FrameObj.scrollTop(((tempY + FrameObj.scrollTop()) * 100 / ratio * zoom / 100) - tempY);
                    ratio = zoom;
                }
            }
        }
    }).on('touchend', function (e) {
        FrameObj
            .data('down', false)
            .data('changedY', null)
            .data('changedX', null);
    });



    ImgObj.bind('mousewheel', function (e) {
        var o = e.target;
        var zoom = parseInt(o.style.zoom, 10) || 100;
        zoom += event.wheelDelta / 12;
        if (zoom >= ratioMin && zoom <= ratioMax) {
            if (zoom > 0) ImgObj.css("zoom", zoom + '%');
            contentObj.css("zoom", zoom + '%');
            if (detectBrowser() == "IE") {
                contentObj.each(function () {
                    $(this).css({ top: (parseInt($(this).attr("top")) * zoom / 100) + "px", left: (parseInt($(this).attr("left")) * zoom / 100) + "px" });
                });
            }
            ratioSlider.bootstrapSlider('setValue', zoom);
            if (hasLine) {
                $("#svgLine").css("zoom", zoom + '%');
                setLine($("#beaconData .beacon.active"), JSON.parse($("#beaconData .beacon.active").attr("data")));
            }
            var tempX = e.clientX - left + $(this).scrollLeft();
            var tempY = e.clientY - top + $(this).scrollTop();
            if (detectBrowser() == "IE") {
                console.log((e.offsetX * zoom / 100) + '--' + tempX);
                FrameObj.scrollLeft((e.offsetX * zoom / 100) - tempX);
                FrameObj.scrollTop((e.offsetY * zoom / 100) - tempY);
            } else {
                FrameObj.scrollLeft((e.offsetX * 100 / ratio * zoom / 100) - tempX);
                FrameObj.scrollTop((e.offsetY * 100 / ratio * zoom / 100) - tempY);
            }

            ratio = zoom;
        }
        return false;
    });

    if (mapModal == true) {
        $('#map-modal').on('shown.bs.modal', function () {
            if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
                ratio = 20;
            } else {
                ratio = Math.ceil($(".customer-position").width() / $("#OrgMap").width() * 100);
                if (ratio < ratioMin)
                    ratio = ratioMin;
            }
            ImgObj.css("zoom", ratio + "%");
            contentObj.css("zoom", ratio + "%");
            ratioSlider.bootstrapSlider('setValue', ratio);
            var pos = $("#map-modal #position").val().split(',');
            if (pos[0] == "0" || pos[1] == "0") {
                var top = parseInt(contentObj.css("top"));
                var left = parseInt(contentObj.css("left"));
                if (detectBrowser() == "IE") {
                    contentObj.css({ top: ($(".customer-position .position img").height() * ratio / 100 / 2) + "px", left: ($(".customer-position .position img").width() * ratio / 100 / 2) + "px" });
                } else {
                    contentObj.css({ top: ($(".customer-position .position img").height() / 2) + "px", left: ($(".customer-position .position img").width() / 2) + "px" });
                }
            } else {
                if (detectBrowser() == "IE") {
                    console.log(ratio);
                    contentObj.css({ top: (parseFloat(pos[1]) * ratio / 100) + "px", left: (parseFloat(pos[0]) * ratio / 100) + "px" });
                    contentObj.attr({ left: pos[0], top: pos[1] });
                } else {
                    contentObj.css({ top: pos[1] + "px", left: pos[0] + "px" });
                }
            }

        });
    }

    return ratioSlider;
}

function _getDistance(xLen, yLen) {
    return Math.sqrt(xLen * xLen + yLen * yLen);
}

function sortTable(rows, key, insertObj) {
    rows.sort(function (a, b) {
        if ($(a).find('td:eq(' + key + ')').text().toLowerCase() > $(b).find('td:eq(' + key + ')').text().toLowerCase())
            return 1;
        else if ($(a).find('td:eq(' + key + ')').text().toLowerCase() < $(b).find('td:eq(' + key + ')').text().toLowerCase())
            return -1;
        return 0;
    }).insertAfter(insertObj);
}

//Array.prototype.remove = function (text) {
//    var index = this.indexOf(text);
//    this.splice(index, 1);
//}
