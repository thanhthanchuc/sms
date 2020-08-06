var _userLE = {
    init: function () {
        _userLE.registerEvent();
    },
    registerEvent: function () {
        $("#info-LE-create").click(function () {
            _userLE.getUserLE();
        });
    },
    getUserLE: function () {
        var empCode = $("#txtEmpCodeLE").val();
        $.ajax({
            url: '/User/GetUserByCode?empCode=' + empCode,
            type: "GET",
            dataType: 'json',
            success: function (reponse) {
                console.log(reponse.data);
                if (reponse.status) {
                    $('#txtFullNameLE').val(reponse.data.FullName);
                    $('#txtPositionLE').val(reponse.data.Position);
                    $('#txtTeamLE').val(reponse.data.Team.Name);
                    //console.log(`/Image/${ empCode }.png`)
                    $("#img-profile").attr("src", `/Image/${empCode}.jpg`);
                } else {
                    alert("Mã nhân viên không tồn tại");
                    $('#txtFullNameLE').val("");
                    $('#txtPositionLE').val(reponse.data.Position);
                    $('#txtTeamLE').val(reponse.data.Team.Name);
                    $("#img-profile").attr("src", `~/Assets/img/the-world.jpg`);
                    console.log(reponse.data.data);
                }
            },
            error: function () {
                $('#txtFullNameLE').val("Không tồn tại");
                $('#txtPositionLE').val("?");
                $('#txtTeamLE').val("?");
                $("#img-profile").attr("src", `~/Assets/img/the-world.jpg`);
                console.log(reponse.data.data);
            }
        });
    }
}
_userLE.init();

var leaveEarly = {
    init: function () {
        leaveEarly.registerEvent();
    },
    registerEvent: function () {
        $(".btn-admin-approve-LE").off('click').on('click', function (e) {
            e.preventDefault();
            var row = this;
            var btn = $(this);
            var s_id = btn.attr('data-id');
            var remark = $('#approve_' + s_id).val();
            var data = { id: s_id, remark: remark }
            $.ajax({
                url: "/LeaveEarly/ApproveForAdmin",
                data: data,
                dataType: "json",
                type: "POST",
                success: function (respone) {
                    if (respone.status == true) {
                        leaveEarly.deleteRow(row);
                    }
                }
            })
        });
        $(".btn-admin-reject-LE").off('click').on('click', function (e) {
            e.preventDefault();
            var row = this;
            var btn = $(this);
            var s_id = btn.attr('data-id');
            var remark = $('#approve_' + s_id).val();
            var data = { id: s_id, remark: remark }
            $.ajax({
                url: "/LeaveEarly/RejectForAdmin",
                data: data,
                dataType: "json",
                type: "POST",
                success: function (respone) {
                    if (respone.status == true) {
                        leaveEarly.deleteRow(row);
                    }
                }
            })
        });
    },
    deleteRow: function (row) {
        $(row).parent().parent().remove();
    }
}
leaveEarly.init();

$('#txtEmpCodeLE').keyup(function () {
    var count = $('#txtEmpCodeLE').val().length;
    if (count == 8) {
        _userLE.getUserLE();
    } else {
        $('#txtFullNameLE').val("");
        $('#txtPositionLE').val("");
        $('#txtTeamLE').val("");
    }
});

$('#estimated_date, #date_from, #date_to').datepicker({
    dateFormat: 'dd/mm/yy'
});

//$('.timepicker, #estimated_time').timepicker({
//    timeFormat: 'h:mm p',
//    interval: 5,
//    startTime: '00:00',
//    dynamic: false,
//    dropdown: true,
//    scrollbar: true
//});


$(".btn-reload").off('click').on('click', function (e) {
    location.reload();
});

function handleRefId(leaveId) {
    $("#btn-confirm-delete-LE").attr("href", "/LeaveEarly/Delete/" + leaveId)
}
function cancel(leaveId) {
    $("#btn-confirm-reject-LE").attr("href", "/LeaveEarly/Cancel/" + leaveId)
}