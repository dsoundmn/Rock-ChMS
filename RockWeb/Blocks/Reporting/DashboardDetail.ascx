<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DashboardDetail.ascx.cs" Inherits="RockWeb.Blocks.Reporting.DashboardDetail" %>

<asp:UpdatePanel ID="upDashboardDetail" runat="server">
    <ContentTemplate>

        <asp:Panel ID="pnlDashboardGridBlockEditor" runat="server" Visible="false">
            <asp:HiddenField ID="hfDashboardGridBlockId" runat="server" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert alert-error" />
            <Rock:NotificationBox ID="nbEditModeMessage" runat="server" NotificationBoxType="Info" />
            <fieldset>
                <legend>
                    <asp:Literal ID="lActionTitleDashboardGridBlock" runat="server" />
                </legend>
                <div class="row-fluid">
                    <div class="span12">
                        <Rock:DataDropDownList ID="ddlGridBlockMetric" runat="server" SourceTypeName="Rock.Model.Dashboard, Rock" PropertyName="Metric" LabelText="Choose a Metric" />
                        <Rock:DataTextBox ID="txtGridBlockDescription" runat="server" TextMode="MultiLine" Rows="4" SourceTypeName="Rock.Model.Dashboard, Rock" PropertyName="Description" LabelText="Description"></Rock:DataTextBox>
                        <Rock:DateTimePicker ID="dtpGridBlockStartDate" runat="server" SourceTypeName="Rock.Model.Dashboard, Rock" PropertyName="StartDate" LabelText="Start Date" />
                        <Rock:DateTimePicker ID="dtpGridBlockEndDate" runat="server" SourceTypeName="Rock.Model.Dashboard, Rock" PropertyName="EndDate" LabelText="End Date" />
                        <Rock:DataDropDownList ID="ddlGridBlockSize" runat="server" SourceTypeName="Rock.Model.Dashboard, Rock" PropertyName="Size" LabelText="Grid Block Size" />
                    </div>
                </div>
            </fieldset>

            <div class="actions">
                <asp:LinkButton ID="btnSaveDashboardGridBlock" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSaveDashboardGridBlock_Click" />
                <asp:LinkButton ID="btnCancelDashboardGridBlock" runat="server" Text="Cancel" CssClass="btn" CausesValidation="false" OnClick="btnCancelDashboardGridBlock_Click" />
            </div>
        </asp:Panel>

    </ContentTemplate>
</asp:UpdatePanel>
