function GetAllStudents() {

    var ajaxtarget = "http://" + document.location.host + "/api/students";

    $.ajax({
        url: ajaxtarget, success: function (result) {
            $("#demo").append(result);
        }
    });
}

function GetHostInfo() {
    console.log("Output;");
    console.log(location.hostname);
    console.log(document.domain);
    alert(document.location.host)

    console.log("document.URL : " + document.URL);
    console.log("document.location.href : " + document.location.href);
    console.log("document.location.origin : " + document.location.origin);
    console.log("document.location.hostname : " + document.location.hostname);
    console.log("document.location.host : " + document.location.host);
    console.log("document.location.pathname : " + document.location.pathname);
}