<%@ Page Title="" Language="C#" MasterPageFile="~/IntelWeb.Master" AutoEventWireup="true" CodeBehind="IntelWebMap.aspx.cs" Inherits="IntelWebSite.IntelWebMap" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
            <div style="background-color:White; display:none;">
                Latitude :<input type="text" id="txtLat" style="width:150px;" />
                &nbsp;&nbsp;Longitude:<input type="text" id="txtLng" style="width:150px;" />
            </div>
            <div style="background-color:White;">
                <table class="mapPopup" cellpadding="0" cellspacing="0" style="width:1050px; font-size:13px;" >
                    <tr style="height:5px;">
                        <td colspan="13"></td>
                    </tr>
                    <tr style="height:20px;">
                        <td align="right" style="width:100px;">
                            Department&nbsp;:&nbsp;
                        </td>
                        <td align="left" style="width:170px;">
                            <asp:DropDownList runat="server" ID="ddlDepartment" Width="160px" Height="25px"  CssClass ="listboxFontSize" >
                                    </asp:DropDownList>
                        </td>
                        <td style="width:10px;">
                        </td>
                        <td align="right" style="width:80px;">
                            Tier&nbsp;:&nbsp;
                        </td>
                        <td align="left" style="width:150px;">
                            <asp:DropDownList runat="server" ID="ddlTier" Width="150px" Height="25px"  CssClass ="listboxFontSize" >
                                    </asp:DropDownList>
                        </td>
                        <td style="width:10px;">
                        </td>
                        <td align="right" style="width:80px;">
                            From &nbsp;:&nbsp;
                        </td>
                        <td align="left" style="width:150px;">
                             <asp:TextBox ID="txtCreatedDateFrom" runat="server" Width="70px" ></asp:TextBox>
                            <asp:ImageButton ID="imgCreatedDateFrom" runat="server" ImageUrl="~/Images/calendar.png"  CausesValidation="false" Width="16px" Height="16px" CssClass="calenderImage" />                
                        </td>
                        <td style="width:10px;">
                        </td>
                        <td align="right" style="width:80px;">
                            To&nbsp;:&nbsp;
                        </td>
                        <td align="left" style="width:150px;">
                            <asp:TextBox ID="txtCreatedDateTo" runat="server" Width="70px"></asp:TextBox>
                            <asp:ImageButton ID="imgCreatedDateTo" runat="server" ImageUrl="~/Images/calendar.png" CausesValidation="false" Width="16px" Height="16px" CssClass="calenderImage"/>                            
                        </td>
                        <td style="width:10px;">
                        </td>
                        <td style="width:80px;">
                            <input type="button" id="btnViewStatus" value="View Status" style="width:80px;" onclick="popupSiteStatus();" />
                        </td>
                    </tr>
                </table>
            </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
</asp:Content>
