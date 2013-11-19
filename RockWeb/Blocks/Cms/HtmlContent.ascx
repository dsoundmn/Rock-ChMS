<%@ Control Language="C#" AutoEventWireup="false" CodeFile="HtmlContent.ascx.cs" Inherits="RockWeb.Blocks.Cms.HtmlContent" %>

<link href="../../Themes/RockChMS/Styles/theme.css" rel="stylesheet" visible="false" />

<asp:UpdatePanel runat="server" ID="upPanel" ChildrenAsTriggers="false" UpdateMode="Conditional">
    <ContentTemplate>
        <%-- View Panel --%>

        <asp:Panel ID="pnlView" runat="server">
            <asp:Literal ID="lPreText" runat="server" />
            <asp:Literal ID="lHtmlContent" runat="server" />
            <asp:Literal ID="lPostText" runat="server" />
        </asp:Panel>

        <%-- Edit Panel --%>

        <Rock:ModalDialog ID="mdEdit" runat="server" OnSaveClick="btnSave_Click" Title="Edit Html" PopupDragHandleControlID="edtHtml">
            <Content>
                <asp:UpdatePanel runat="server" ID="upEdit">
                    <ContentTemplate>

                        <asp:Panel ID="pnlEdit" runat="server" Visible="false">
                            <div class="row">
                                <div class="col-md-7">
                                    <Rock:DateRangePicker ID="pDateRange" runat="server" Label="Display Date Range" />
                                </div>

                                <div class="col-md-5">
                                    <Rock:RockDropDownList runat="server" ID="ddlVersions" Label="Version" OnSelectedIndexChanged="ddlVersions_SelectedIndexChanged" />
                                </div>
                            </div>


                            <Rock:HtmlEditor ID="edtHtml" runat="server" ResizeMaxWidth="720" Height="400" AutoGrowOnStartup="false" />
                            <Rock:RockCheckBox ID="ckDontSaveNewVersion" runat="server" Text="don't save a new version" />
                            <Rock:RockCheckBox ID="chkApproved" runat="server" Text="Approve" />

                        </asp:Panel>

                    </ContentTemplate>
                </asp:UpdatePanel>




            </Content>
        </Rock:ModalDialog>






    </ContentTemplate>
</asp:UpdatePanel>



