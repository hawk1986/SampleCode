// message template
var template = '<div class="alert alert-dismissible alert-{0}" role="alert">{1}</div>';

// message param
var message = '';

// display message
function displayMessage() {
    if (message !== '') {
        var temp = JSON.parse(message);
        $('#msg_div').html(template.format(temp.Style, temp.Message));
        $("#msg_div .alert").alert();
        $('#messageModal').modal('show');
    }
}

//上傳檔案，預覽圖片
function getObjectURL(file) {
    var url = null;
    if (window.createObjectURL !== undefined) { // basic
        url = window.createObjectURL(file);
    } else if (window.URL !== undefined) { // mozilla(firefox)
        url = window.URL.createObjectURL(file);
    } else if (window.webkitURL !== undefined) { // webkit or chrome
        url = window.webkitURL.createObjectURL(file);
    }
    return url;
}