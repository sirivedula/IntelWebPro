<%@ Page Title="" Language="C#" MasterPageFile="~/IntelWeb.Master" AutoEventWireup="true" CodeBehind="wfSite.aspx.cs" Inherits="IntelWebSite.wfSite" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function editSite(gridname, _id) {
            var myGrid = webGrid.findGrid(gridname);
            if (myGrid != null) {
                var myRecord = webGrid.findFirst(myGrid.data, function(obj) { return obj._id == _id });
                webGrid.fillUIForm(gridname, _id);
            }
        }

        function delSite(gridname, _id) {
            var myGrid = webGrid.findGrid(gridname);
            if (myGrid != null) {
                var myRecord = webGrid.findFirst(myGrid.data, function(obj) { return obj._id == _id });
                if (myRecord != null) {
                    if (confirm("Are you sure you want to delete '" + myRecord.fields.site_code + "'?")) {
                        webGrid.deleteRecord(gridname, _id);
                        webGrid.submit(gridname, function(result) {
                            webGrid.mergeGridInfos(result.gridInfos);
                            webGrid.drawAllRows(gridname);
                        });
                    }
                }
            }
        }        
        
        function saveSite() {
            if (webGrid.saveUIForm('divSiteEntry')) {
                webGrid.submit("divSiteEntry", function(data) {
                });
            }
        }
        
        function addNew() {
            var n = webGrid.add("siteGrid");
            webGrid.fillUIForm(n.grid, n);
        }
        
        $(document).ready(function() {
            $('#anhHome').removeClass("selected");
            $('#anhMast').addClass("selected");
            $('#fax_number,#mobile_number,#land_number').mask('(999) 999-9999');
        });
    </script>
    
    <asp:Literal ID="ltrGridScript" runat="server"></asp:Literal>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Literal ID="ltrGrid" runat="server"></asp:Literal>
    
	<div style="display:none;" id="divSiteEntry">
		<table>
			<tr><td class="ra">Dept Code</td><td><select id="department_code" name="department_code"><asp:Literal ID="ltrDepts" runat="server"></asp:Literal></select></td><td class="ra">Site Code</td><td><input type="text" id="site_code" name="site_code" /></td><td class="ra">Code Description</td><td><input type="text" id="site_code_description" name="site_code_description" /></td></tr>
			<tr><td class="ra">Building Name</td><td><input type="text" id="building_name" name="building_name" /></td><td class="ra">Facility Contact</td><td><input type="text" id="facility_contact" name="facility_contact" /></td><td class="ra">Path</td><td><input type="text" id="path" name="path" /></td></tr>
            <tr><td class="ra">Address Line 1</td><td><input type="text" id="address1" name="address1" /></td><td class="ra">Address Line 2</td><td><input type="text" id="address2" name="address2" /></td></tr>
            <tr><td class="ra">City</td><td><input type="text" id="city" name="city" /></td><td class="ra">State</td><td><input type="text" id="state_code" name="state_code" /></td><td class="ra">Zip</td><td><input type="text" id="zip_code" name="zip_code" size="9" /></td></tr>
            <tr><td class="ra">Cell Phone</td><td><input type="text" id="mobile_number" name="mobile_number" /></td><td class="ra">Home Phone</td><td><input type="text" id="land_number" name="land_number" /></td><td class="ra">Fax</td><td><input type="text" id="fax_number" name="fax_number" /></td></tr>
            <tr><td class="ra">Country</td><td><input type="text" name="country_code" id="country_code" /></td><td class="ra">E Mail</td><td><input type="text" id="email_id" name="email_id" /></td></tr>
            <tr><td colspan="6"><input type="button" value="Save" id="btnSave" onclick="saveSite()" /></td></tr>			
		</table>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
</asp:Content>
