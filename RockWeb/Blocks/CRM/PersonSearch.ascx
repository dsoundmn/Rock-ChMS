<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PersonSearch.ascx.cs" Inherits="RockWeb.Blocks.Crm.PersonSearch" %>

<Rock:Grid ID="gPeople" runat="server" EmptyDataText="No People Found">
    <Columns>
        <asp:BoundField DataField="Person.FullNameLastFirst" HeaderText="Person" />
        <asp:BoundField DataField="Spouse.FullNameLastFirst" HeaderText="Spouse" />
    </Columns>
</Rock:Grid>


