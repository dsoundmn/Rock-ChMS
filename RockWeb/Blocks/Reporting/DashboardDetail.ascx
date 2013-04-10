<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DashboardDetail.ascx.cs" Inherits="RockWeb.Blocks.Reporting.DashboardDetail" %>

<asp:UpdatePanel ID="upDashboardDetail" runat="server">
    <ContentTemplate>

        <asp:Panel ID="pnlDashboardGridBlockEditor" runat="server" Visible="false">
            <asp:HiddenField ID="hfDashboardGridBlockId" runat="server" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert alert-error" />
            <fieldset>
                <legend>
                    <asp:Literal ID="lActionTitleDashboardGridBlock" runat="server" />
                </legend>
                <div class="span6">
                    <Rock:NotificationBox ID="nbEditModeMessage" runat="server" NotificationBoxType="Info" />
                </div>
                <div class="row-fluid">
                    <div class="span12">
<%--                        <Rock:DataTextBox ID="tbValueName" runat="server" SourceTypeName="Rock.Model.DefinedValue, Rock" PropertyName="Name" />
                        <Rock:DataTextBox ID="tbValueDescription" runat="server" SourceTypeName="Rock.Model.DefinedValue, Rock" PropertyName="Description" TextMode="MultiLine" Rows="3" />--%>
                            <Rock:DataDropDownList ID="ddlGridBlockMetric" runat="server" SourceTypeName="Rock.Model.Dashboard, Rock" PropertyName="Choose a Metric" />
                            <Rock:DataTextBox ID="txtGridBlockDescription" runat="server" TextMode="MultiLine" Rows="4" SourceTypeName="Rock.Model.Dashboard, Rock" PropertyName="Description"></Rock:DataTextBox>
                            <Rock:DateTimePicker ID="dtpGridBlockStartDate" runat="server" SourceTypeName="Rock.Model.Dashboard, Rock" PropertyName="Start Date" />
                            <Rock:DateTimePicker ID="dtpGridBlockEndDate" runat="server" SourceTypeName="Rock.Model.Dashboard, Rock" PropertyName="End Date" />
                            <Rock:DataDropDownList ID="ddlGridBlockSize" runat="server" SourceTypeName="Rock.Model.Dashboard, Rock" PropertyName="Grid Block Size" />
                    </div>
                </div>
                <div class="attributes">
                    <asp:PlaceHolder ID="phDashboardGridBlockAttributes" runat="server" EnableViewState="false"></asp:PlaceHolder>
                </div>
            </fieldset>

            <div class="actions">
                <asp:LinkButton ID="btnSaveDashboardGridBlock" runat="server" Text="Save" CssClass="btn primary" OnClick="btnSaveDashboardGridBlock_Click" />
                <asp:LinkButton ID="btnCancelDashboardGridBlock" runat="server" Text="Cancel" CssClass="btn secondary" CausesValidation="false" OnClick="btnCancelDashboardGridBlock_Click" />
            </div>
        </asp:Panel>

    </ContentTemplate>
</asp:UpdatePanel>
