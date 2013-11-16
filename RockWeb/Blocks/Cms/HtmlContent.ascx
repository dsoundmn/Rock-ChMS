<%@ Control Language="C#" AutoEventWireup="false" CodeFile="HtmlContent.ascx.cs" Inherits="RockWeb.Blocks.Cms.HtmlContent" %>

<<<<<<< HEAD
<link href="../../Themes/RockChMS/Styles/theme.css" rel="stylesheet" visible="false" />
=======
<script type="text/javascript">
    Sys.Application.add_load(function () {
        Rock.controls.htmlContentEditor.initialize({
            blockId: <%= CurrentBlock.Id %>,
            behaviorId: '<%= mpeContent.BehaviorID %>',
            hasBeenModified: <%= HtmlContentModified.ToString().ToLower() %>,
            versionId: '<%= hfVersion.ClientID %>',
            startDateId: '<%= tbStartDate.ClientID %>',
            expireDateId: '<%= tbExpireDate.ClientID %>',
            ckEditorId: '<%= htmlContent.ClientID %>',
            approvalId: '<%= cbApprove.ClientID %>'
        });
    });
</script>
<asp:UpdatePanel runat="server" class="html-content-block">
<ContentTemplate>
>>>>>>> refs/heads/develop

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
                        

<<<<<<< HEAD
                        <Rock:HtmlEditor ID="edtHtml" runat="server" ResizeMaxWidth="720" />

                    </fieldset>
=======
        <div id="html-content-versions-<%=CurrentBlock.Id %>" style="display:none">
            <div class="modal-body">
                <Rock:Grid ID="rGrid" runat="server" AllowPaging="false" >
                    <Columns>
                        <asp:TemplateField SortExpression="Version" HeaderText="Version">
                            <ItemTemplate>
                                <a data-html-id='<%# Eval("Id") %>' class="html-content-show-version-<%=CurrentBlock.Id %>" href="#">Version <%# Eval("Version") %></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ModifiedDateTime" HeaderText="Modified" SortExpression="ModifiedDateTime" />
                        <asp:BoundField DataField="ModifiedByPerson" HeaderText="By" SortExpression="ModifiedByPerson" />
                        <Rock:BoolField DataField="Approved" HeaderText="Approved" SortExpression="Approved" />
                        <asp:BoundField DataField="ApprovedByPerson" HeaderText="By" SortExpression="ApprovedByPerson" />
                        <asp:BoundField DataField="StartDateTime" DataFormatString="MM/dd/yy" HeaderText="Start" SortExpression="StartDateTime" />
                        <asp:BoundField DataField="ExpireDateTime" DataFormatString="MM/dd/yy" HeaderText="Expire" SortExpression="ExpireDateTime" />
                    </Columns>
                </Rock:Grid>
            </div>
            <div class="modal-footer">
                <button id="html-content-versions-cancel-<%=CurrentBlock.Id %>" class="btn btn-link">Cancel</button>
            </div>
        </div>

        <div id="html-content-edit-<%=CurrentBlock.Id %>">
            <asp:panel ID="pnlVersioningHeader" runat="server" class="htmlcontent-edit-header">
                <asp:HiddenField ID="hfVersion" runat="server" />
                Start: <asp:TextBox ID="tbStartDate" runat="server" CssClass="date-picker"></asp:TextBox>
                Expire: <asp:TextBox ID="tbExpireDate" runat="server" CssClass="date-picker"></asp:TextBox>
                <div class="html-content-approve"><asp:CheckBox ID="cbApprove" runat="server" TextAlign="Right" Text="Approve" /></div>
            </asp:panel>
            <div class="modal-body">

                <Rock:HtmlEditor ID="htmlContent" runat="server" Visible="false" />
>>>>>>> refs/heads/develop

<<<<<<< Updated upstream
                <div class="">
                    <asp:CheckBox ID="cbOverwriteVersion" runat="server" TextAlign="Right" Text="don't save a new version" />
                </div>
            </div>
            <div class="modal-footer">
                
                <asp:LinkButton ID="lbCancel" runat="server" CssClass="btn btn-link" Text="Cancel" />
                <asp:LinkButton ID="lbOk" runat="server" CssClass="btn btn-primary" Text="Save" />
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



