﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Attributes.ascx.cs" Inherits="RockWeb.Blocks.Administration.Attributes" %>

<asp:UpdatePanel ID="upPanel" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlList" runat="server">

            <Rock:GridFilter ID="rFilter" runat="server" OnDisplayFilterValue="rFilter_DisplayFilterValue">
                <Rock:EntityTypePicker ID="ddlEntityType" runat="server" Label="Entity Type" IncludeGlobalOption="true" AutoPostBack="true" OnSelectedIndexChanged="ddlEntityType_SelectedIndexChanged" />
                <Rock:CategoryPicker ID="cpCategoriesFilter" runat="server" Label="Categories" AllowMultiSelect="true" />
            </Rock:GridFilter>
            <Rock:Grid ID="rGrid" runat="server" AllowSorting="true" RowItemText="setting" DescriptionField="Description" OnRowSelected="rGrid_Edit">
                <Columns>
                    <asp:BoundField 
                        DataField="Id" 
                        HeaderText="Id" 
                        SortExpression="EntityType.FriendlyName" 
                        ItemStyle-Wrap="false" />
                    <asp:TemplateField ItemStyle-Wrap="false">
                        <HeaderTemplate>Qualifier</HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="lEntityQualifier" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-Wrap="false" />
                    <asp:TemplateField ItemStyle-Wrap="false">
                        <HeaderTemplate>Categories</HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="lCategories" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="FieldType" HeaderText="Type" />
                    <Rock:BoolField DataField="IsMultiValue" HeaderText="Multi-Value" SortExpression="IsMultiValue" />
                    <Rock:BoolField DataField="IsRequired" HeaderText="Required" SortExpression="IsRequired" />
                    <asp:TemplateField>
                        <HeaderTemplate>Default Value</HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="lDefaultValue" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>Value</HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="lValue" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <Rock:EditValueField OnClick="rGrid_EditValue" />
                    <Rock:SecurityField TitleField="Name" />
                    <Rock:DeleteField OnClick="rGrid_Delete" />
                </Columns>
            </Rock:Grid>

        </asp:Panel>

        <asp:Panel ID="pnlDetails" runat="server" Visible="false" CssClass="panel panel-default">
            <div class="panel-body">

                <div class="banner"><h1><asp:Literal ID="lAttributeTitle" runat="server"></asp:Literal></h1></div>

                <asp:ValidationSummary ID="valSummaryTop" runat="server" HeaderText="Please Correct the Following" CssClass="alert alert-danger" />

                <Rock:EntityTypePicker ID="ddlAttrEntityType" runat="server" Label="Entity Type" IncludeGlobalOption="true" Required="true" />
                <Rock:RockTextBox ID="tbAttrQualifierField" runat="server" Label="Qualifier Field" />
                <Rock:RockTextBox ID="tbAttrQualifierValue" runat="server" Label="Qualifier Value" />

                <Rock:AttributeEditor ID="edtAttribute" runat="server" OnSaveClick="btnSave_Click" OnCancelClick="btnCancel_Click" ValidationGroup="Attribute" />

            </div>
        </asp:Panel>

        <Rock:ModalDialog ID="modalDetails" runat="server" Title="Attribute" ValidationGroup="AttributeValue" >
            <Content>
                <asp:HiddenField ID="hfIdValues" runat="server" />
                <asp:ValidationSummary ID="ValidationSummaryValue" runat="server" HeaderText="Please Correct the Following" CssClass="alert alert-danger" ValidationGroup="AttributeValue"  />
                <fieldset id="fsEditControl" runat="server"/>
            </Content>
        </Rock:ModalDialog>

        <Rock:NotificationBox ID="nbMessage" runat="server" Title="Error" NotificationBoxType="Danger" Visible="false" />

    </ContentTemplate>
</asp:UpdatePanel>
