<%@ Page Title="" Language="C#" MasterPageFile="~/IntelWeb.Master" AutoEventWireup="true" CodeBehind="wfDepartment.aspx.cs" Inherits="IntelWebSite.wfDepartment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function editDept(gridname, _id) {
            var myGrid = webGrid.findGrid(gridname);
            if (myGrid != null) {
                var myRecord = webGrid.findFirst(myGrid.data, function(obj) { return obj._id == _id });
                webGrid.fillUIForm(gridname, _id);
            }
        }

        function delDept(gridname, _id) {
            var myGrid = webGrid.findGrid(gridname);
            if (myGrid != null) {
                var myRecord = webGrid.findFirst(myGrid.data, function(obj) { return obj._id == _id });
                if (myRecord != null) {
                    if (confirm("Are you sure you want to delete '" + myRecord.fields.department_code + "'?")) {
                        webGrid.deleteRecord(gridname, _id);
                        webGrid.submit(gridname, function(result) {
                            webGrid.mergeGridInfos(result.gridInfos);
                            webGrid.drawAllRows(gridname);
                        });
                    }
                }
            }
        }        
        
        function saveDept() {
            if (webGrid.saveUIForm('divDeptEntry')) {
                webGrid.submit("divDeptEntry", function() {
                });
            }
        }
        
        function addNew() {
            var n = webGrid.add("deptGrid");
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
    <div style="display:none;" id="divDeptEntry">
        <table>
            <tr><td class="ra">Department Code</td><td><input type="text" id="department_code" name="department_code" /></td><td class="ra">Code Description</td><td><input type="text" id="department_code_description" name="department_code_description" /></td></tr>
            <tr><td colspan="6" style="text-align:center;text-decoration:underline;">Contact Person</td></tr>
            <tr><td class="ra">First Name</td><td><input type="text" id="first_name" name="first_name" /></td><td class="ra">Last Name</td><td><input type="text" id="last_name" name="last_name" /></td></tr>
            <tr><td class="ra">Address Line 1</td><td><input type="text" id="address1" name="address1" /></td><td class="ra">Address Line 2</td><td><input type="text" id="address2" name="address2" /></td></tr>
            <tr><td class="ra">City</td><td><input type="text" id="city" name="city" /></td><td class="ra">State</td><td><input type="text" id="state_code" name="state_code" size="9" /></td><td class="ra">Zip</td><td><input type="text" id="zip_code" name="zip_code" size="9" /></td></tr>
            <tr><td class="ra">Cell Phone</td><td><input type="text" id="mobile_number" name="mobile_number" /></td><td class="ra">Home Phone</td><td><input type="text" id="land_number" name="land_number" size="9" /></td><td class="ra">Fax</td><td><input type="text" id="fax_number" name="fax_number" /></td></tr>
            <tr><td class="ra">E Mail</td><td><input type="text" id="email_id" name="email_id" /></td></tr>
            <tr><td colspan="2"><input type="button" value="Save" id="btnSave" onclick="saveDept()" /></td></tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
</asp:Content>
