using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IntelWeb;

namespace IntelWebSite
{
    public partial class wfDepartment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            CurrentUser cuser = new CurrentUser();
            cuser.userName = "Balaji";
            cuser.Load();
            WebGridController deptGC = WebGridController.ControllerFromName("lkup_department", Request.Form, this.Form.Controls, cuser);
            lkup_department lkupDept = new lkup_department(cuser);
            List<IntelWebObject> lkupDepts = lkupDept.Load();
            deptGC.grid.data = lkupDepts;

            deptGC.grid.uiFormShowJS = @"function(){
            $('#divDeptEntry').dialog({width:700,modal:true,title:'Department',close:function(){webGrid.removeNewItems()}});$('#department_code').focus();}";
            deptGC.grid.afterSubmitJS = @"function(result,grid){
                                              if(result.ok && result.gridInfos){
                                                webGrid.mergeGridInfos(result.gridInfos);
                                                webGrid.drawFilteredRows(grid,function(record){
                                                  if((typeof record.fields.markedforremoval != undefined) && !record.fields.markedforremoval){
                                                    return true;
                                                  }
                                                return false;
                                                });
                                               	if($('#divDeptEntry').is(':visible')){
                                                    try{
													    $('#divDeptEntry').dialog('close');
                                                    }
                                                    catch(ex){}
												}
                                              }
                                            }";

            deptGC.grid.uiFormName = "theform";
            var gtable = new WebGridTable();
            gtable.UIHeaderText = "Department Information";
            gtable.tableId = "tblFieldsId";
            gtable.Grid = deptGC.grid;
            var myButtonFunction = new Func<IntelWebObject, object>((obj) =>
            {
                return @"<a href=""javascript:void(0);"" title=""Edit Deparment"" onclick=""editDept([gridName],[recordIndex])"" ><img alt=""Edit"" title=""Edit Department"" src=""Images/table_edit.png""></a>&nbsp;<a href=""javascript:void(0);"" title=""Delete Department"" onclick=""delDept([gridName],[recordIndex])"" ><img title=""Delete Department"" src=""Images/delete.png"" alt=""delete""></a>";
            });

            gtable.extraColumns.Add(new WebGridTable.extraColumn()
            {
                fieldTemplate = new DisplayField()
                {
                    submitField = false
                    ,
                    fieldName = "EditButtons"
                    ,
                    formatFunction = myButtonFunction
                    ,
                    isHeaderField = true
                    ,
                    jsFormatFunction = @"return '<a href=""javascript:void(0);"" title=""Edit Deparment"" onclick=""editDept([gridName],[recordIndex])"" ><img alt=""Edit"" title=""Edit Department"" src=""Images/table_edit.png""></a>&nbsp;<a href=""javascript:void(0);"" title=""Delete Department"" onclick=""delDept([gridName],[recordIndex])"" ><img title=""Delete Department"" src=""Images/delete.png"" alt=""delete""></a> '"
                }
            });

            ltrGrid.Text = "<div class=\"centered\">" + gtable.tableUI() + "</div>";
            ltrGridScript.Text = "<script type=\"text/javascript\">" + deptGC.grid.gridScript() + "</script>\n" + gtable.tableScript();
        }
    }
}
