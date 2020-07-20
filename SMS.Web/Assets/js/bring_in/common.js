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
        var empCode = $("#txtEmpCodeBI").val();
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

$(".btn-reload").off('click').on('click', function (e) {
    location.reload();
});

//Create
$('#register').on("click", (event) => {
    event.preventDefault();
    var bringInObject = {};
    bringInObject.EmpCode = $('#txtEmpCodeBI').val();
    bringInObject.Reason = $('#reason').val();
    bringInObject.FullName = $('#txtFullNameBI').val();
    bringInObject.Position = $('#txtPositionBI').val();
    bringInObject.EstimatedDate = $('#estimated_date').val();
    bringInObject.EstimatedTime = $('#estimated_time').val();
    bringInObject.Team = $('#txtTeamBI').val();
    bringInObject.Bring_In_Items = [];

    $(".row-items").each((index, item) => {
        var bringInItem = {};
        bringInItem.Item = $(item).find('input[class*=item]').val();
        bringInItem.Serial = $(item).find('input[class*=serial]').val();
        bringInItem.Quantity = $(item).find('input[class*=quantity]').val();
        bringInItem.Unit = $(item).find('input[class*=unit]').val();
        bringInItem.AssetType = $(item).find('select[class*=assetType]').val();
        bringInItem.IsReturn = $(item).find('select[class*=isReturn]').val();
        bringInItem.ReturnDate = $(item).find('input[class*=returnDate]').val();
        bringInItem.ReturnTime = $(item).find('input[class*=returnTime]').val();

        if (bringInItem.Item && bringInItem.Unit && bringInItem.Quantity)
            bringInObject.Bring_In_Items.push(bringInItem);
    })

    console.log(bringInObject);
    $.confirm({
        title: "Thông báo",
        content: function () {
            var self = this;
            return $.ajax({
                url: "/BringIn/Create",
                method: "POST",
                data: bringInObject,
                success: function (mess) {
                    if (mess === "Success") {
                        window.location.href = "/BringIn/History"
                    }
                    else {
                        self.setContent(mess);
                    }
                }
            })
        }
    })
})