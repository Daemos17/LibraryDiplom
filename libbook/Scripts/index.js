var videoCapture;
$(document).ready(function () {
    videoCapture = document.getElementById('capturevideo');
    const constraints = {

        video: {
            width: 1000,
            height: 1000,
            facingMode: "environment"
        },
    };
});
const constraints = {

    video: {
        width:  1000 ,
        height: 1000,
        facingMode: "environment"
    },
};

$(document).on('click', '#btnActivateCamera', function () {
    if (navigator.mediaDevices && navigator.mediaDevices.getUserMedia) {
        // access video stream from webcam
        navigator.mediaDevices.getUserMedia({ video: { facingMode:"environment" } }).then(function (stream) {
            // on success, stream it in video tag 
            window.localStream = stream;
            videoCapture.srcObject = stream;
            videoCapture.play();
            activateCamera();
        }).catch(e => {
            // on failure/error, alert message. 
            alert("Please Allow: Use Your Camera!");
        });
    }
});
$(document).on('click', '#btnDeactivateCamera', function () {
    // stop video streaming if any
    localStream.getTracks().forEach(function (track) {
        if (track.readyState == 'live' && track.kind === 'video') {
            track.stop();
            deactivateCamera();
        }
    });
});

$(document).on('click', '#btnCapture', function (data_uri) {
    document.getElementById('capturecanvas').baseURI();

    base64String = data_uri.replace("data:", "")
        .replace(/^.+,/, "");
    data_uri = base64String;
    // downloadImage(today, data_uri);
    submitForm(data_uri);
});
function activateCamera() {
    $("#btnActivateCamera").addClass("d-none");
    $("#btnDeactivateCamera").removeClass("d-none");
    $("#capturevideo").removeClass("d-none");
    $("#btnCapture").removeClass("d-none");
    $("#capturecanvas").removeClass("d-none");
}
function deactivateCamera() {
    $("#btnDeactivateCamera").addClass("d-none");
    $("#btnActivateCamera").removeClass("d-none");
    $("#capturevideo").addClass("d-none");
    $("#btnCapture").addClass("d-none");
    $("#capturecanvas").addClass("d-none");
}


 
function submitForm(data_uri) {

    var model = data_uri;



    jQuery.ajax({
        type: "POST",
        url: "@Url.Action(\"Capture\",\"Camera\")",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ filePath: model }),
        success: function (response) {
            if (response) {
                window.location.href = response.redirectToUrl;
            }
        },
        failure: function (response) {
            alert(errMsg);
        }
    });

}
downloadImage = function (name, datauri) {
    var a = document.createElement('a');
    a.setAttribute('download', name);
    a.setAttribute('href', datauri);
    a.click();
}