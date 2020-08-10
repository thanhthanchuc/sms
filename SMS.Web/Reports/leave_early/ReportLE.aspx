<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportLE.aspx.cs" Inherits="SMS.Web.Reports.leave_early.ReportLE" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        #ReportViewer1_ctl13 {
            z-index:-1;
        }
    </style>
</head>
<body style="height: 807px">
    <form id="form1" runat="server">
        <div style="width: 100%; display: flex">
            <div>
                <asp:Panel ID="Panel1" runat="server">
                    <asp:Label ID="Label1" runat="server" Text="Từ ngày" Font-Size="Large"></asp:Label>
                    &nbsp;<asp:TextBox ID="TextBox1" runat="server" Width="120px" Font-Size="Large"></asp:TextBox>
                    <asp:Calendar style="z-index:1" ID="Calendar1" runat="server" Font-Size="Large" OnSelectionChanged="Calendar1_SelectionChanged" BackColor="#CCCCCC"></asp:Calendar>
                    &nbsp;<asp:ImageButton ID="Button1" runat="server" BorderStyle="Solid" Height="16px" ImageUrl="~/Assets/img/logo.png" OnClick="Button1_Click" Width="16px" BorderWidth="1px" />
                </asp:Panel>
            </div>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <div>
                <asp:Panel ID="Panel2" runat="server">
                    <asp:Label ID="Label2" runat="server" Text="Đến ngày" Font-Size="Large"></asp:Label>
                    &nbsp;<asp:TextBox ID="TextBox2" runat="server" Width="120px" Font-Size="Large"></asp:TextBox>
                    <asp:Calendar ID="Calendar2" runat="server" Font-Size="Large" OnSelectionChanged="Calendar2_SelectionChanged" BackColor="Silver"></asp:Calendar>
                    &nbsp;<asp:ImageButton ID="ImageButton2" runat="server" BorderStyle="Solid" Height="16px" ImageUrl="~/Assets/img/logo.png" Width="16px" OnClick="ImageButton2_Click" BorderWidth="1px" />
                </asp:Panel>
            </div>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <div>
                <asp:Label ID="Label3" runat="server" Text="Phòng ban" Font-Size="Large"></asp:Label>
                &nbsp;<asp:TextBox ID="TextBox3" Width="120px" runat="server" Font-Size="Large"></asp:TextBox>
            </div>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <div>
                <asp:Label ID="Label4" runat="server" Text="Tên nhân viên" Font-Size="Large"></asp:Label>
                &nbsp;<asp:TextBox ID="TextBox4" Width="150px" runat="server" Font-Size="Large"></asp:TextBox>
            </div>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <div>
                <asp:Button ID="Button2" Width="80px" Height="25px" runat="server" Text="Tìm kiếm" BorderStyle="Solid" BorderWidth="1px" Font-Strikeout="False" OnClick="Button2_Click" />
            </div>
            <br />
            <br />
            <br />
        </div>
        <div style="z-index:-1">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" BackColor="" ClientIDMode="AutoID" HighlightBackgroundColor="" InternalBorderColor="204, 204, 204" InternalBorderStyle="Solid" InternalBorderWidth="1px" LinkActiveColor="" LinkActiveHoverColor="" LinkDisabledColor="" PrimaryButtonBackgroundColor="" PrimaryButtonForegroundColor="" PrimaryButtonHoverBackgroundColor="" PrimaryButtonHoverForegroundColor="" SecondaryButtonBackgroundColor="" SecondaryButtonForegroundColor="" SecondaryButtonHoverBackgroundColor="" SecondaryButtonHoverForegroundColor="" SplitterBackColor="" ToolbarDividerColor="" ToolbarForegroundColor="" ToolbarForegroundDisabledColor="" ToolbarHoverBackgroundColor="" ToolbarHoverForegroundColor="" ToolBarItemBorderColor="" ToolBarItemBorderStyle="Solid" ToolBarItemBorderWidth="1px" ToolBarItemHoverBackColor="" ToolBarItemPressedBorderColor="51, 102, 153" ToolBarItemPressedBorderStyle="Solid" ToolBarItemPressedBorderWidth="1px" ToolBarItemPressedHoverBackColor="153, 187, 226" Width="1051px">
                <LocalReport ReportPath="~/Reports/leave_early/ReportLE.rdlc">
                </LocalReport>
            </rsweb:ReportViewer>
        </div>

    </form>
</body>
</html>
