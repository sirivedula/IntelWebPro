<%@ Page Title="" Language="C#" MasterPageFile="~/IntelWeb.Master" AutoEventWireup="true" CodeBehind="wfIntelWebImport.aspx.cs" Inherits="IntelWebSite.wfIntelWebImport" %>
<%@ MasterType VirtualPath="~/IntelWeb.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#anhHome').removeClass("selected");
            $('#anhIImp').addClass("selected");
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="centered" style="font-family:Tahoma;color:#FFF;">
    <asp:Literal ID="ltrMessage" runat="server"></asp:Literal>
    <table>
        <tr><td class="ra">Choose Tier</td><td><select id="tiername" name="tiername">
                                    <asp:Literal ID="ltrTiers" runat="server"></asp:Literal></select></td></tr>
        <tr><td class="ra">Choose Excel File</td><td><input type="file" id="xlsFile" name="xlsFile" size="50"/></td></tr></table>
    <hr />
        <table><tr><td>(or) Paste Data From Excel into the following Text Area:</td></tr>
            <tr><td><textarea id="xlsTextArea" name="xlsTextArea" rows="15" cols="80"></textarea></td></tr>
            <tr><td><input type="submit" id="btnSubmit" value="Submit" /></td></tr>
        </table>
    </div>
    <hr />
    <div>
        <asp:Literal ID="ltrJobStatus" runat="server"></asp:Literal>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
</asp:Content>
