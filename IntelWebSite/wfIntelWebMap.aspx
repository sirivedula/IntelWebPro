<%@ Page Title="" Language="C#" MasterPageFile="~/IntelWeb.Master" AutoEventWireup="true" CodeBehind="wfIntelWebMap.aspx.cs" Inherits="IntelWebSite.wfIntelWebMap" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="http://maps.google.com/maps?file=api&amp;v=2&amp;sensor=false&amp;key=ABQIAAAAbIHH-Xkal5qUSmlI0fZVahS26xkXOIoZMQBpLrZnbzPjD5fhYhQoFImi_S7GLSYwNfNfBxptHmyQ8Q" type="text/javascript"></script>
    
    
    <script type="text/javascript">
        var map = null;
        var geocoder = null;
        var jsMapCities;
        var jsFromDate = '04/01/2011';
        var jsToDate = '04/30/2011';
        
        $(document).ready(function() {
            $(".MidBlock,.MidCenDiv").css("width", "100%");
            $('#anhHome').removeClass("selected");
            $('#anhIMap').addClass("selected");
            $('#fromdate,#todate').mask('99/99/9999');
            $('#fromdate,#todate').datepicker({ showOn: 'button', buttonImage: 'Images/date.png', buttonImageOnly: true, changeYear: true, changeMonth: true });

            $('#fromdate').val(jsFromDate);
            $('#todate').val(jsToDate);

            /*
            var d = new Date();
            $('#fromdate').val(getMMddyyyy(d));
            d.setMonth(d.getMonth() + 1);
            $('#todate').val(getMMddyyyy(d));
            */
            mapInit();
        });

        function getMMddyyyy(dd) {
            return (dd.getMonth()+1) + "/" + dd.getDate() + "/" + dd.getFullYear();
        }
        
        function showDiv(id, siteCode) {
            $('#showup').dialog({ title: siteCode + ' - Panel Layer', height: 300, width: 500 });
            $('#showup').dialog('widget').position({ my: 'left top', at: 'left bottom', of: $(id)});
            $('#showup').html('<img src="Images/ajax5.gif" alt="loading..."/>');
            var params = {};
            params.tiercode = $('#tiercode').val();
            params.deptcode = $('#deptcode').val();
            params.sitecode = siteCode;
            params.fdate = $('#fromdate').val();
            params.tdate = $('#todate').val();
            $.get("wfPanelData.aspx", params, function(data) {
                $('#showup').html(data);
            });
            
        }

        function showDetails(site, panel, device) {
            $('#showDetailDiv').dialog({ title: site + " - " + panel, height: 300, width: 800 });
            $('#showDetailDiv').html('<img src="Images/ajax5.gif" alt="loading..."/>');
            var params = {};
            params.tiercode = $('#tiercode').val();
            params.deptcode = $('#deptcode').val();
            params.sitecode = site;
            params.panelcode = panel;
            params.devicecode = device;
            params.fdate = $('#fromdate').val();
            params.tdate = $('#todate').val();
            $.get("wfDeviceData.aspx", params, function(data) {
                $('#showDetailDiv').html(data);
            });
        }
        function submitRefresh() {
            // TODO::check date valids later
            $('form:first').submit();
        }
    </script>    
    
    <asp:Literal ID="ltrMapScript" runat="server"></asp:Literal>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="centered" style="font-family:Tahoma;white-space:nowrap; color:#FFF;">
        <table cellpadding="2">
            <tr>
                <td class="ra">Tier</td><td><select style="font-family:Tahoma;" id="tiercode" name="tiercode"><asp:Literal ID="ltrTiers" runat="server"></asp:Literal></select></td>
                <td class="ra">Department</td><td><select style="font-family:Tahoma;" id="deptcode" name="deptcode"><asp:Literal ID="ltrDepts" runat="server"></asp:Literal></select></td>
                <td class="ra">From Date</td><td><input type="text" style="font-family:Tahoma;" id="fromdate" name="fromdate" size="9" maxlength="10" /></td>
                <td class="ra">To Date</td><td><input type="text" style="font-family:Tahoma;" id="todate" name="todate" size="9" maxlength="10" /></td>
                <td><input type="button" id="btnView" value="Refresh Map" style="font-family:Tahoma;" onclick="submitRefresh();" /></td>
            </tr>
        </table>
    </div>
    <asp:Literal ID="ltrMessage" runat="server"></asp:Literal>

    <div id="map_canvas" style="width:100%;height:100%;">
    </div>
    
    <div id="showup" style="display:none;">
                This is test under panels display....
    </div>
    <div id="showDetailDiv" style="display:none;">
    </div>    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
</asp:Content>
