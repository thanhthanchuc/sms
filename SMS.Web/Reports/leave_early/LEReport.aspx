<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LEReport.aspx.cs" Inherits="SMS.Web.Reports.leave_early.LEReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register TagPrefix="rsweb" Namespace="Microsoft.Reporting.WebForms" Assembly="Microsoft.ReportViewer.WebForms" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <table>
            <tr>
                <td>Từ ngày</td>
                <td><input type="date" /></td>
                <td></td>
                <td>Đến ngày</td>
                <td><input type="date" /></td>
                <td></td>
                <td>Phòng ban</td>
                <td><input type="text" /></td>
                <td></td>
                <td>Mã nhân viên</td>
                <td><input type="text" /></td>
                <td></td>
                <td><button class="btn btn-sm btn-primary" id="btn-search" onclick="handleSearch()" type="button"><i class="fas fa-search"></i>Tìm kiếm</button></td>
            </tr>
        </table>
        <br />
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <rsweb:ReportViewer ID="ReportViewer1" Width="1200" Height="1200" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" BackColor="" ClientIDMode="AutoID" HighlightBackgroundColor="" InternalBorderColor="204, 204, 204" InternalBorderStyle="Solid" InternalBorderWidth="1px" LinkActiveColor="" LinkActiveHoverColor="" LinkDisabledColor="" PrimaryButtonBackgroundColor="" PrimaryButtonForegroundColor="" PrimaryButtonHoverBackgroundColor="" PrimaryButtonHoverForegroundColor="" SecondaryButtonBackgroundColor="" SecondaryButtonForegroundColor="" SecondaryButtonHoverBackgroundColor="" SecondaryButtonHoverForegroundColor="" SplitterBackColor="" ToolbarDividerColor="" ToolbarForegroundColor="" ToolbarForegroundDisabledColor="" ToolbarHoverBackgroundColor="" ToolbarHoverForegroundColor="" ToolBarItemBorderColor="" ToolBarItemBorderStyle="Solid" ToolBarItemBorderWidth="1px" ToolBarItemHoverBackColor="" ToolBarItemPressedBorderColor="51, 102, 153" ToolBarItemPressedBorderStyle="Solid" ToolBarItemPressedBorderWidth="1px" ToolBarItemPressedHoverBackColor="153, 187, 226" style="margin-right: 0px">
                <LocalReport ReportPath="Reports\leave_early\ReportLE.rdlc">
                    <DataSources>
                        <rsweb:ReportDataSource Name="dsLE" DataSourceId="SqlDataSource1"></rsweb:ReportDataSource>
                    </DataSources>
                </LocalReport>
            </rsweb:ReportViewer>
            <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:DB_SMSConnectionString %>' SelectCommand="SELECT ID, EmpCode, FullName, Position, Team, Shift, Reason, Purpose, EstimatedDate, EstimatedTime, CreatedBy, CreatedDate, ApprovedBy, ApprovedStatus, ApproverRemark, ApprovedDate, Guard, GuardStatus, GuardRemark, GuardDate, ModifiedDate, ModifiedBy FROM Leave_Early"></asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
