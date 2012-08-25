using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using IntelWeb;

namespace IntelWebSite
{
    public partial class wfSite : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            CurrentUser cuser = new CurrentUser();
            cuser.userName = "Balaji";
            cuser.Load();
            WebGridController siteGC = WebGridController.ControllerFromName("lkup_site", Request.Form, this.Form.Controls, cuser);
            lkup_department lkupDept = new lkup_department(cuser);
            List<IntelWebObject> lkupDepts = lkupDept.Load();
            this.ltrDepts.Text = string.Join("", lkupDepts.Cast<lkup_department>().Select(x=>"<option value=\"" + HttpUtility.HtmlEncode(x.department_code) + "\">" + HttpUtility.HtmlEncode(x.department_code + " (" + x.department_code_description + ")") + "</option>").ToArray());

            lkup_site lkupSite = new lkup_site(cuser);
            List<IntelWebObject> lkupSites = lkupSite.Load("","department_code,site_code");
            siteGC.grid.data = lkupSites;

            siteGC.grid.uiFormShowJS = @"function(){
            $('#divSiteEntry').dialog({width:750,modal:true,title:'Site/Building Info',close:function(){webGrid.removeNewItems()}});$('#site_code').focus();}";
            siteGC.grid.afterSubmitJS = @"function(result,grid){
                                              if(result.ok && result.gridInfos){
                                                webGrid.mergeGridInfos(result.gridInfos);
                                                webGrid.drawFilteredRows(grid,function(record){
                                                  if((typeof record.fields.markedforremoval != undefined) && !record.fields.markedforremoval){
                                                    return true;
                                                  }
                                                return false;
                                                });
                                               	if($('#divSiteEntry').is(':visible')){
                                                    try{
													    $('#divSiteEntry').dialog('close');
                                                    }
                                                    catch(ex){}
												}
                                              }
                                            }";

            siteGC.grid.uiFormName = "theform";
            var gtable = new WebGridTable();
            gtable.UIHeaderText = "Site / Building Information";
            gtable.tableId = "tblFieldsId";
            gtable.Grid = siteGC.grid;
            var myButtonFunction = new Func<IntelWebObject, object>((obj) =>
            {
                return @"<a href=""javascript:void(0);"" title=""Edit Site"" onclick=""editSite([gridName],[recordIndex])"" ><img alt=""Edit"" title=""Edit Site"" src=""Images/table_edit.png""></a>&nbsp;<a href=""javascript:void(0);"" title=""Delete Site"" onclick=""delSite([gridName],[recordIndex])"" ><img title=""Delete Site"" src=""Images/delete.png"" alt=""delete""></a>";
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
                    jsFormatFunction = @"return '<a href=""javascript:void(0);"" title=""Edit Site"" onclick=""editSite([gridName],[recordIndex])"" ><img alt=""Edit"" title=""Edit Site"" src=""Images/table_edit.png""></a>&nbsp;<a href=""javascript:void(0);"" title=""Delete Site"" onclick=""delSite([gridName],[recordIndex])"" ><img title=""Delete Site"" src=""Images/delete.png"" alt=""delete""></a> '"
                }
            });

            ltrGrid.Text = "<div class=\"centered\">" + gtable.tableUI() + "</div>";
            ltrGridScript.Text = "<script type=\"text/javascript\">" + siteGC.grid.gridScript() + "</script>\n" + gtable.tableScript();

        }
    }
}
