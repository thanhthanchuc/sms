$(document).ready(function () {
    $(".btn-print-LE").click(function () {
        ReportManager.GenerateReport();
    });
});

var ReportManager = {
    GenerateReport: function () {
        var jsonParam = "";
        var serviceUrl = "/LeaveEarly/GetReport";
        ReportManager.GetReport(serviceUrl, jsonParam, onFailed);

        function onFailed(error) {
            alert(error);
        }
    },

    GetReport: function (serviceUrl, jsonParams, errorCallback) {
        jQuery.ajax({
            url: serviceUrl,
            async: false,
            type: "POST",
            data: "{" + jsonParams + "}",
            contentType: "application/json; charset=utf-8",
            success: function () {
                window.open('../Reports/early_leave/ReportViewer.aspx', '_newtab');
            },
            error: errorCallback
        });
    }
};

var ReportHelper = {

}