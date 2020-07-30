var _userBO = {
    init: function () {
        _userBO.registerEvent();
    },
    registerEvent: function () {
        $("#info-BI-create").click(function () {
            _userBO.getUserBO();
        });
    },
    getUserBO: function () {
        var empCode = $("#txtEmpCodeBO").val();
        $.ajax({
            url: '/User/GetUserByCode?empCode=' + empCode,
            type: "GET",
            dataType: 'json',
            success: function (reponse) {
                console.log(reponse.data);
                if (reponse.status) {
                    $('#txtFullNameBO').val(reponse.data.FullName);
                    $('#txtPositionBO').val(reponse.data.Position);
                    $('#txtTeamBO').val(reponse.data.Team.Name);
                } else {
                    alert("Mã nhân viên không tồn tại");
                    $('#txtFullNameBO').val("");
                    $('#txtPositionBO').val(reponse.data.Position);
                    $('#txtTeamBO').val(reponse.data.Team.Name);
                    console.log(reponse.data.data);
                }
            },
            error: function () {
                $('#txtFullNameBO').val("Không tồn tại");
                $('#txtPositionBO').val("?");
                $('#txtTeamBO').val("?");
                console.log(reponse.data.data);
            }
        });
    }
}
_userBO.init();

//Create
$('#register').on("click", (event) => {
    event.preventDefault();
    var bringOutObject = {};
    bringOutObject.EmpCode = $('#txtEmpCodeBO').val();
    bringOutObject.Reason = $('#reason').val();
    bringOutObject.FullName = $('#txtFullNameBO').val();
    bringOutObject.Position = $('#txtPositionBO').val();
    bringOutObject.EstimatedDate = $('#estimated_date').val();
    bringOutObject.EstimatedTime = $('#estimated_time').val();
    bringOutObject.Team = $('#txtTeamBO').val();
    bringOutObject.Bring_Out_Items = [];

    $(".row-items").each((index, item) => {
        var bringOutItem = {};
        bringOutItem.Item = $(item).find('input[class*=item]').val();
        bringOutItem.Serial = $(item).find('input[class*=serial]').val();
        bringOutItem.Quantity = $(item).find('input[class*=quantity]').val();
        bringOutItem.Unit = $(item).find('input[class*=unit]').val();
        bringOutItem.AssetType = $(item).find('select[class*=assetType]').val();
        bringOutItem.IsReturn = $(item).find('select[class*=isReturn]').val();
        bringOutItem.ReturnDate = $(item).find('input[class*=returnDate]').val();
        bringOutItem.ReturnTime = $(item).find('input[class*=returnTime]').val();

        bringOutObject.Bring_Out_Items.push(bringOutItem);
    })

    console.log(bringOutObject);
    $.confirm({
        title: "Thông báo",
        content: function () {
            var self = this;
            return $.ajax({
                url: "/BringOut/Create",
                method: "POST",
                data: bringOutObject,
                success: function (mess) {
                    if (mess === "Success") {
                        window.location.href = "/BringOut/History"
                    }
                    else {
                        self.setContent(mess);
                    }
                }
            })
        }
    })
})