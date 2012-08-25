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
    public partial class SaveGrid : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int counter = 0;
            string objectType = Request.Form.Get("grid" + counter.ToString() + "_objectId");
            List<string> saveErrors = new List<string>();
            var GridReturnRecordInfo = new StringBuilder();
            if (!String.IsNullOrEmpty(objectType))
            {
                CurrentUser cuser = new CurrentUser();
                cuser.userName = "Balaji";
                cuser.Load();

                WebGridController gridController = WebGridController.ControllerFromName(objectType, Request.Form, null, cuser);
                var myGrid = gridController.grid;
                string submittedName = Request.Form.Get("grid" + counter.ToString() + "_objectName");
                myGrid.gridName = submittedName; //change grid name to the value submitted - this supports multiple aliased gris - such as in Onboarding New Hire
                GridReturnRecordInfo.Append("{name:'" + JSUtil.EnquoteJS(myGrid.gridName) + "',records:");
                myGrid.LoadData();
                if (myGrid.loadErrors != null && myGrid.loadErrors.Count != 0)
                {
                    foreach (ApplicationException ae in myGrid.loadErrors)
                    {
                        saveErrors.Add(ae.Message);
                    }
                }

                myGrid.UpdateData();
                if (!string.IsNullOrEmpty(myGrid.updateDataErrors))
                {
                    Response.Write("{ok:false,errorMessage:'The save operation was cancelled because there were problems with the data that you submitted. " + JSUtil.EnquoteJS("\n" + myGrid.updateDataErrors) + "'}");
                    Response.End();
                }
                var myData = myGrid.data;
                var removeObjs = new List<IntelWebObject>();
                if (myData != null)
                {
                    foreach (IntelWebObject obj in myData)
                    {
                        if (obj.isDirty)
                        {
                            if (!myGrid.deleteQueue.Contains(obj))
                            { //don't save items marked for deletion
                                if (!obj.save())
                                {
                                    saveErrors.Add(obj.saveErrorText);
                                    if (obj.isNew)
                                    {
                                        removeObjs.Add(obj);

                                    }
                                    else
                                    {
                                        obj.revertChanges();
                                    }
                                }
                            }
                        }
                    }
                    foreach (IntelWebObject removeIt in removeObjs)
                    {
                        myGrid.data.Remove(removeIt);
                    }
                }
                GridReturnRecordInfo.Append(myGrid.dataJS());
                GridReturnRecordInfo.Append(",deletedKeys:[");
                var deleteBuilder = new StringBuilder();
                foreach (IntelWebObject apObj in myGrid.deleteQueue)
                {
                    var dKey = JSUtil.SerializeJSProperty(apObj.primaryKey);

                    if (!apObj.delete())
                    {
                        saveErrors.Add(apObj.saveErrorText);
                    }
                    else
                    {
                        deleteBuilder.Append(dKey);
                        deleteBuilder.Append(",");
                    }
                }
                if (deleteBuilder.Length > 0)
                {
                    deleteBuilder.Remove(deleteBuilder.Length - 1, 1);
                }
                GridReturnRecordInfo.Append(deleteBuilder.ToString());
                GridReturnRecordInfo.Append("]}");
                GridReturnRecordInfo.Append(",");
                counter++;
                objectType = Request.Form.Get("grid" + counter.ToString() + "_objectId");
            }
            else
            {
                saveErrors.Add("Object Id is null");
            }
            if (GridReturnRecordInfo.Length > 0) { GridReturnRecordInfo.Remove(GridReturnRecordInfo.Length - 1, 1); }
            string output = "{ok:[paramOK],errorMessage:'[paramErrorMessage]'"; //action:[paramAction] - is implemented in the gridBase.js 7/22/2010
            if (saveErrors.Count.Equals(0))
            {
                output = output.Replace("[paramOK]", "true");
                output = output.Replace("[paramErrorMessage]", "");

            }
            else
            {
                output = output.Replace("[paramOK]", "false");
                var sb = new StringBuilder();
                foreach (string em in saveErrors)
                {
                    sb.Append(JSUtil.EnquoteJS(em + "\n"));

                }
                output = output.Replace("[paramErrorMessage]", sb.ToString());
            }
            output += ",gridInfos:[" + GridReturnRecordInfo.ToString() + "]";
            output += "}";
            Response.Write(output);

        }
    }
}
