<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LE_RDLC.aspx.cs" Inherits="SMS.Web.Reports.LE.LE_RDLC" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register TagPrefix="rsweb" Namespace="Microsoft.Reporting.WebForms" Assembly="Microsoft.ReportViewer.WebForms" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        #ReportViewer1_ctl13 {
            z-index: -1;
        }

        #Calendar1, #Calendar2 {
            z-index: 1;
        }

        #Button1 {
            width: 24px;
        }
    </style>
</head>
<body style="height: auto; width:100%">
    <br />
    <form id="form1" runat="server" style="text-align:center">
        <br />
        <div style="width: 100%; text-align:center; display: flex">
            <div>
                <asp:Panel ID="Panel1" runat="server">
                    <asp:Label ID="Label1" runat="server" Text="Từ ngày" Font-Size="Large"></asp:Label>
                    &nbsp;<asp:TextBox ID="TextBox1" runat="server" Width="120px" Font-Size="medium" onfocus="ShowCalendar1()"></asp:TextBox>
                    <asp:Calendar Style="z-index: 1" ID="Calendar1" runat="server" Font-Size="Large" OnSelectionChanged="Calendar1_SelectionChanged" BackColor="#CCCCCC"></asp:Calendar>
                    &nbsp;<asp:ImageButton ID="Button1" runat="server" BorderStyle="Solid" Height="16px" ImageUrl="~/Assets/img/logo.png" OnClick="Button1_Click" Width="16px" BorderWidth="1px" />
                </asp:Panel>
            </div>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <div>
                <asp:Panel ID="Panel2" runat="server">
                    <asp:Label ID="Label2" runat="server" Text="Đến ngày" Font-Size="Large"></asp:Label>
                    &nbsp;<asp:TextBox ID="TextBox2" runat="server" Width="120px" Font-Size="medium"></asp:TextBox>
                    <asp:Calendar ID="Calendar2" runat="server" Font-Size="Large" OnSelectionChanged="Calendar2_SelectionChanged" BackColor="Silver"></asp:Calendar>
                    &nbsp;<asp:ImageButton ID="ImageButton2" runat="server" BorderStyle="Solid" Height="16px" ImageUrl="~/Assets/img/logo.png" Width="16px" OnClick="ImageButton2_Click" BorderWidth="1px" />
                </asp:Panel>
            </div>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <div>
                <asp:Label ID="Label3" runat="server" Text="Phòng ban" Font-Size="Large"></asp:Label>
                &nbsp;<asp:TextBox ID="TextBox3" Width="120px" runat="server" Font-Size="medium"></asp:TextBox>
            </div>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <div>
                <asp:Label ID="Label4" runat="server" Text="Tên nhân viên" Font-Size="Large"></asp:Label>
                &nbsp;<asp:TextBox ID="TextBox4" Width="150px" runat="server" Font-Size="medium"></asp:TextBox>
            </div>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <div>
                <asp:Button ID="Button2" Width="80px" Height="25px" runat="server" Text="Tìm kiếm" BorderStyle="Solid" BorderWidth="1px" Font-Strikeout="False" OnClick="Button2_Click" />
            </div>
            <br />
            <br />
            <br />
            <br />
        </div>

        <div style="z-index: -1; height: auto; width:100%; overflow:auto">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <rsweb:reportviewer id="ReportViewer1" runat="server" backcolor="" clientidmode="AutoID" highlightbackgroundcolor="" internalbordercolor="204, 204, 204" internalborderstyle="Solid" internalborderwidth="1px" linkactivecolor="" linkactivehovercolor="" linkdisabledcolor="" primarybuttonbackgroundcolor="" primarybuttonforegroundcolor="" primarybuttonhoverbackgroundcolor="" primarybuttonhoverforegroundcolor="" secondarybuttonbackgroundcolor="" secondarybuttonforegroundcolor="" secondarybuttonhoverbackgroundcolor="" secondarybuttonhoverforegroundcolor="" splitterbackcolor="" toolbardividercolor="" toolbarforegroundcolor="" toolbarforegrounddisabledcolor="" toolbarhoverbackgroundcolor="" toolbarhoverforegroundcolor="" toolbaritembordercolor="" toolbaritemborderstyle="Solid" toolbaritemborderwidth="1px" toolbaritemhoverbackcolor="" toolbaritempressedbordercolor="51, 102, 153" toolbaritempressedborderstyle="Solid" toolbaritempressedborderwidth="1px" toolbaritempressedhoverbackcolor="153, 187, 226" width="100%" Height="794px">
                <LocalReport ReportPath="Reports\LE\ReportLE.rdlc">
                </LocalReport>
            </rsweb:reportviewer>
        </div>
    </form>
</body>
</html>
