var ServerProd = window.location.origin;
var jsondata;
function ConectServer(TypeMetodo, Metodo, Params) {
    jsondata = null;
    if (TypeMetodo == "POST") {
        if (Params == null) {
            $.post(ServerProd + Metodo, function (data) {
                console.log(data);
                jsondata = data;
                return;
            });
        }
        else {
            $.post(ServerProd + Metodo, Params, function (data) {
                console.log(data);
                jsondata = data;
                return;
            });
        }
    } else {
        if (Params == null) {
            $.getJSON(ServerProd + Metodo, function (data) {
                console.log(data);
                jsondata = data;
                return;
            });
        }
        else {
            $.getJSON(ServerProd + Metodo, Params, function (data) {
                console.log(data);
                jsondata = data;
                return;
            });
        }
    }
}


function setLoading(loadingMessage, width) {
    if (typeof width == "undefined")
        width = "0%";

    var loadingElement = $('#loading-container');
    if (loadingElement.length == 0) {
        if (loadingMessage == undefined || loadingMessage == '') {
            return;
        }
        loadingElement = $('<div id="loading-container" style="display:none" >' +
            '<div class="modal-backdrop in"></div>' +
            '<div class="loading" style=" position: fixed; width: 100%; top: 40%; text-align:center; z-index: 99999">' +
            '<div style="width: 50%; display: inline-block">' +
            '<div class="progress">' +
              '<div class="progress-bar progress-bar-default progress-bar-striped active" role="progressbar" aria-valuenow="45" aria-valuemin="0" aria-valuemax="100" style="width: ' + width + '">' +
                '<span class="sr-only"></span>' +
              '</div></div>' +
            '<h3 id="loading-container-message" style="color: white"> ' + loadingMessage + '</h3>' +
            '</div></div></div>');
        loadingElement.appendTo('body');
        loadingElement.show('fast');
    }
    else {
        if (loadingMessage == undefined || loadingMessage == '') {
            loadingElement.hide('fast', function () { $(this).remove(); });
        } else {
            $('#loading-container-message').text(loadingMessage);
            loadingElement.show();
        }
    }
}