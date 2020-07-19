var _userGO = {
    init: function () {
        _userGO.registerEvent();
    },
    registerEvent: function () {
        $("#info-GO-create").click(function () {
            _userGO.getUserGO();
        });
    },
    getUserGO: function () {
        var empCode = $("#txtEmpCodeGO").val();
        $.ajax({
            url: '/User/GetUserByCode?empCode=' + empCode,
            type: "GET",
            dataType: 'json',
            success: function (reponse) {
                console.log(reponse.data);
                if (reponse.status) {
                    $('.txtFullNameGO').val(reponse.data.FullName);
                    $('.txtPositionGO').val(reponse.data.Position);
                    $('.txtTeamGO').val(reponse.data.Team);
                } else {
                    alert("Mã nhân viên không tồn tại");
                    $('.txtFullNameGO').val("");
                    $('.txtPositionLGO').val(reponse.data.Position);
                    $('.txtTeamGO').val(reponse.data.Team);
                    console.log(reponse.data.data);
                }
            },
            error: function () {
                $('.txtFullNameGO').val("Không tồn tại");
                $('.txtPositionGO').val("?");
                $('.txtTeamGO').val("?");
                console.log(reponse.data.data);
            }
        });
    }
}
_userGO.init();


var goOut = {
    init: function () {
        goOut.registerEvent();
    },
    registerEvent: function () {
        $(".btn-admin-approve-GO").off('click').on('click', function (e) {
            e.preventDefault();
            var row = this;
            var btn = $(this);
            var s_id = btn.attr('data-id');
            var remark = $('#approve_' + s_id).val();
            var data = { id: s_id, remark: remark }
            $.ajax({
                url: "/GoOut/ApproveForAdmin",
                data: data,
                dataType: "json",
                type: "POST",
                success: function (respone) {
                    if (respone.status == true) {
                        goOut.deleteRow(row);
                    }
                }
            })
        });
        $(".btn-admin-reject-GO").off('click').on('click', function (e) {
            e.preventDefault();
            var row = this;
            var btn = $(this);
            var s_id = btn.attr('data-id');
            var remark = $('#approve_' + s_id).val();
            var data = { id: s_id, remark: remark }
            $.ajax({
                url: "/GoOut/RejectForAdmin",
                data: data,
                dataType: "json",
                type: "POST",
                success: function (respone) {
                    if (respone.status == true) {
                        goOut.deleteRow(row);
                    }
                }
            })
        });
    },
    deleteRow: function (row) {
        $(row).parent().parent().remove();
    }
}
goOut.init();

$('#txtEmpCodeGO').keyup(function () {
    var count = $('#txtEmpCodeGO').val().length;
    if (count == 8) {
        _userGO.getUserGO();
    } else {
        $('.txtFullNameGO').val("");
        $('.txtPositionGO').val("");
        $('.txtTeamGO').val("");
    }
});

$('#estimated_date_out, #estimated_date_return, #date_from, #date_to').datepicker({
    dateFormat: 'dd/mm/yy'
});

$('#estimated_date_out, #estimated_date_return, #date_from, #date_to').datepicker().datepicker("setDate", new Date());

$('.timepicker, #estimated_time_out, #estimated_time_return').timepicker({
    timeFormat: 'h:mm p',
    interval: 5,
    startTime: '00:00',
    dynamic: false,
    dropdown: true,
    scrollbar: true
});

$(".btn-reload").off('click').on('click', function (e) {
    location.reload();
});

function handleRefId(leaveId) {
    $("#btn-confirm-delete-GO").attr("href", "/GoOut/Delete/" + leaveId)
}
function cancel(leaveId) {
    $("#btn-confirm-reject-GO").attr("href", "/GoOut/Cancel/" + leaveId);
}