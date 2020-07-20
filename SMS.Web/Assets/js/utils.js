function handleAPI(url, data, reload = false) {
    $.ajax({
        url,
        method: "POST",
        data,
        success: () => {
            if (reload)
                window.location.reload()
            else
                window.location.href = "/Guard/Queue"
        },
        error: () => {
            alert("Lỗi hệ thống!")
        }
    })
}


var _userBI = {
    init: function () {
        _userBI.registerEvent();
    },
    registerEvent: function () {
        $("#info-BI-create").click(function () {
            _userBI.getUserBI();
        });
    },
    getUserBI: function () {
        var empCode = $("#txtEmployeeID").val();
        $.ajax({
            url: '/User/GetUserByCode?empCode=' + empCode,
            type: "GET",
            dataType: 'json',
            success: function (reponse) {
                console.log(reponse.data);
                if (reponse.status) {
                    $('#txtFullNameBI').val(reponse.data.FullName);
                    $('#txtPositionBI').val(reponse.data.Position);
                    $('#txtTeamBI').val(reponse.data.Team);
                } else {
                    alert("Mã nhân viên không tồn tại");
                    $('#txtFullNameBI').val("");
                    $('#txtPositionBI').val(reponse.data.Position);
                    $('#txtTeamBI').val(reponse.data.Team);
                    console.log(reponse.data.data);
                }
            },
            error: function () {
                $('#txtFullNameBI').val("Không tồn tại");
                $('#txtPositionBI').val("?");
                $('#txtTeamBI').val("?");
                console.log(reponse.data.data);
            }
        });
    }
}
_userBI.init();

$('#txtEmployeeID').keyup(function () {
    var count = $('#txtEmployeeID').val().length;
    if (count == 1 || count == 8) {
        _userBI.getUserBI();
    } else {
        $('#txtFullNameBI').val("");
        $('#txtPositionBI').val("");
        $('#txtTeamBI').val("");
    }
});

$(".btn-reload").off('click').on('click', function (e) {
    location.reload();
});