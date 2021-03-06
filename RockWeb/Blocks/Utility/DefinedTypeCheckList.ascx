﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DefinedTypeCheckList.ascx.cs" Inherits="RockWeb.Blocks.Utility.DefinedTypeCheckList" %>

<asp:UpdatePanel ID="upSettings" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlContent" runat="server" CssClass="defined-type-checklist">
            <asp:Repeater ID="rptrValues" runat="server">
                <ItemTemplate>
                    <asp:Panel ID="pnlValue" runat="server">
                        <asp:HiddenField ID="hfValue" runat="server" Value='<%# Eval("Id") %>' />
                        <Rock:RockCheckBox ID="cbValue" runat="server" AutoPostBack="true" />
                    </asp:Panel>
                </ItemTemplate>
            </asp:Repeater>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
