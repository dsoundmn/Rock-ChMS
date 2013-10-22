<%@ Control Language="C#" AutoEventWireup="false" CodeFile="HtmlContent.ascx.cs" Inherits="RockWeb.Blocks.Cms.HtmlContent" %>

<link href="../../Themes/RockChMS/Styles/theme.css" rel="stylesheet" visible="false" />

<asp:UpdatePanel runat="server" ID="upPanel">
    <ContentTemplate>

        <%-- View Panel --%>
        <asp:Panel ID="pnlView" runat="server">
            <asp:Literal ID="lPreText" runat="server" />
            <asp:Literal ID="lHtmlContent" runat="server" />
            <asp:Literal ID="lPostText" runat="server" />
        </asp:Panel>

        <%-- Edit Panel --%>
        <asp:Panel ID="pnlEdit" runat="server" Visible="false">
            <Rock:ModalDialog ID="mdEdit" runat="server" OnSaveClick="btnSave_Click" Title="Edit Html" PopupDragHandleControlID="edtHtml">
                <HeaderCustomContent>
                    <div class="pull-right">
                        <table class="table-condensed">
                            <tr>
                                <td>
                                    <asp:Literal ID="lCurrentVersion" runat="server" Text="Version: 00" />
                                </td>
                                <td>
                                    <asp:LinkButton ID="btnShowVersionGrid" runat="server" Text="History" OnClick="btnShowVersionGrid_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </HeaderCustomContent>
                <Content>
                    <fieldset>
                        <div class="float-right">
                            <Rock:RockCheckBox ID="chkApproved" runat="server" Text="Approve" />
                        </div>
                        <Rock:DateRangePicker ID="pDateRange" runat="server" Label="Display Date Range" />
                        

                        <Rock:HtmlEditor ID="edtHtml" runat="server" ResizeMaxWidth="720" />

                    </fieldset>

<<<<<<< Updated upstream
                <div class="">
                    <asp:CheckBox ID="cbOverwriteVersion" runat="server" TextAlign="Right" Text="don't save a new version" />
                </div>
            </div>
            <div class="modal-footer">
                
                <asp:LinkButton ID="lbCancel" runat="server" CssClass="btn btn-default" Text="Cancel" />
                <asp:LinkButton ID="lbOk" runat="server" cssclass="btn btn-primary" Text="Save" />
            </div>
        </div>
=======
                    <Rock:RockCheckBox ID="ckDontSaveNewVersion" runat="server" Text="don't save a new version" />
                    <asp:LinkButton ID="btnPreview" runat="server" Text="Preview" CssClass="btn btn-xs pull-right" CausesValidation="false" OnClick="btnPreview_Click" />
                </Content>
            </Rock:ModalDialog>
        </asp:Panel>
>>>>>>> Stashed changes


        <asp:Panel ID="pnlVersionHistory" runat="server" Visible="false">
        </asp:Panel>

        <%-- Preview Dialog --%>
        <asp:Panel ID="pnlPreview" runat="server" Visible="false">
            <Rock:ModalDialog ID="mdPreview" runat="server" Title="Preview Html">
                <Content>
                    <div class='scroll-container'>
                        <div class='scrollbar'>
                            <div class='track'>
                                <div class='thumb'>
                                    <div class='end'></div>
                                </div>
                            </div>
                        </div>
                        <div class='viewport'>
                            <div class='overview'>
                                <asp:Literal ID="lPreTextPreview" runat="server" />
                                <asp:Literal ID="lHtmlContentPreview" runat="server" />
                                <asp:Literal ID="lPostTextPreview" runat="server" />
                            </div>
                        </div>
                    </div>
                </Content>
            </Rock:ModalDialog>
            <script>
                // help the scroll-container size correctly when first loaded
                Sys.Application.add_load(function () {
                    $('.scroll-container').tinyscrollbar({ size: 150 });
                    $('.scroll-container').on('mouseenter', function () {
                        $('.scroll-container').tinyscrollbar_update('relative');
                    });
                });
            </script>
        </asp:Panel>


    </ContentTemplate>
</asp:UpdatePanel>



