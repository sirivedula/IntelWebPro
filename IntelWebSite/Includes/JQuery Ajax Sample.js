function locationchange() {
    var location = $("#IncidentLocationTypeDropDown option:selected").attr('text');
    if (location.indexOf("Select") == -1) {
        var params = new Object();
        params.locationtype = location;
        $.ajax({
            type: "POST",
            url: "IncidentFormControlAjax.asmx/getfacilities",
            data:$.toJSON(params) ,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                $('#FacilityDropDownList > option').remove();
                $('#InitiatingStaffDropDownList > option').remove();
                $('#YouthDropDownList > option').remove();
                var facilities = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                for (var i = 0; i < facilities.length; i++)
                    $('#FacilityDropDownList').append($("<option></option>").val(facilities[i]['V'].toString()).html(facilities[i]['T']));                
            },
            failure: function (msg) {
                alert(msg);
            }
        });
    }
}
$(document).ready(function () {
    $('.datetimepicker').datetimepicker();
});
 
function staffchange() {
    $("#OtherStaffDiv").empty();
    $("#InitiatingStaffDropDownList > option").not(':selected').each(function () {
        if ($(this).attr('value') != "-1") {
            var chkbox = "<input type='checkbox'" + "value = '" + $(this).attr('value') + "'>" + $(this).attr('text') + "<br/>";            
            $("#OtherStaffDiv").append(chkbox);
        }    
    });    
}
function youthchange() {
    $("#OtherYouthDiv").empty();
    $("#YouthDropDownList > option").not(':selected').each(function () {
        if ($(this).attr('value') != "-1") {
            var chkbox = "<input type='checkbox'" + "value = '" + $(this).attr('value') + "'>" + $(this).attr('text') + "<br/>";
            $("#OtherYouthDiv").append(chkbox);
        }
    });
}
function facilitychange() {
    var facility = $("#FacilityDropDownList option:selected").attr('text') + ';' + $("#FacilityDropDownList option:selected").attr('value');
    if (facility.indexOf("Select") == -1) {
        var params = new Object();
        params.facility = facility;
        $.ajax({
            type: "POST",
            url: "IncidentFormControlAjax.asmx/getstaff",
            data: $.toJSON(params),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                $('#InitiatingStaffDropDownList > option').remove();
                var staff = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                for (var i = 0; i < staff.length; i++) {
                    $('#InitiatingStaffDropDownList').append($("<option></option>").val(staff[i]['V']).html(staff[i]['T']));
                }
            },
            failure: function (msg) {
                alert(msg);
            }
        });
        $.ajax({
            type: "POST",
            url: "IncidentFormControlAjax.asmx/getyouth",
            data: $.toJSON(params),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                $('#YouthDropDownList > option').remove();
                var youth = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                for (var i = 0; i < youth.length; i++) {
                    $('#YouthDropDownList').append($("<option></option>").val(youth[i]['V']).html(youth[i]['T']));
                }
            },
            failure: function (msg) {
                alert(msg);
            }
        });
    }
}








using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using IncidentReporting;
using IncidentReporting.BO;
using Newtonsoft.Json;
using System.Collections;
using IncidentReporting.BLL;
using IncidentReporting.DAL;
namespace LearnJqueryAjax
{
    /// <summary>
    /// Summary description for IncidentFormControlAjax
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class IncidentFormControlAjax : System.Web.Services.WebService
    {
 
        private class customclass
        {
            private object k;
            private object v;
 
            public object T
            {
                get { return k; }
                set { k = value; }
            }
 
            public object V
            {
                get { return v; }
                set { v = value; }
            }
 
            public customclass(object k1,object v1)
            {
                k = k1; v = v1;
            }
        }
 
        private class customclasslist:List<customclass>
        {
            public customclasslist()
            {
 
            }
        }
        [WebMethod]
        public string getfacilities(string locationtype)
        {    
            DataTable dt = Common.getFacilities(locationtype);
            customclasslist clist = new customclasslist();
            foreach (DataRow dr in dt.Rows)
            {   
                customclass c = new customclass(dr[1],dr[2].ToString() + ';' + dr[0]);
                clist.Add(c);
            }
            clist.Insert(0,new customclass("< Select a Value >", "-1"));
            string jsonResult = JsonConvert.SerializeObject(clist);
            return jsonResult;
        }
        [WebMethod]
        public string getstaff(string facility)
        {
            String sfacility = facility.Substring(0, facility.IndexOf(';'));
            StaffList s = CommonManager.getStaff(sfacility);
            customclasslist clist = new customclasslist();
            foreach (Staff ys in s)
                clist.Add(new customclass(ys.Name, ys.NTID));
            clist.Insert(0, new customclass("< Select a Value >", "-1"));
            string jsonResult = JsonConvert.SerializeObject(clist);
            return jsonResult;
        }
        [WebMethod]
        public string getyouth(string facility)
        {
            String system = facility.Substring(facility.IndexOf(';') + 1, facility.LastIndexOf(';') - facility.IndexOf(';') - 1);
            String sfacility = facility.Substring(0, facility.IndexOf(';') );
            YouthList y = Common.getYouth(sfacility, system);
            customclasslist clist = new customclasslist();
            if (system == "Jori")
                foreach (Youth ys in y)
                    clist.Add(new customclass(ys.Name,ys.YouthId));
            else
                foreach (Youth ys in y)
                    clist.Add(new customclass(ys.Name,ys.DetentionId));            
            clist.Insert(0, new customclass("< Select a Value >", "-1"));
            string jsonResult = JsonConvert.SerializeObject(clist);
            return jsonResult;
        }
        [WebMethod]
        public string getdata(int startindex, int pagesize)
        {
            int totalIncidents = 0;
            IncidentList li = IncidentsDB.GetList("-1","-1","-1",DateTime.Parse("03/15/2001"),DateTime.Parse("03/15/2012"), startindex, pagesize,"IncidentId", ref totalIncidents);
            customclasslist clist = new customclasslist();
            foreach (Incident i in li)
            {
                customclass c = new customclass(i.Id, i.InitiatingStaff.Name);
                clist.Add(c);
            }
            string jsonResult = JsonConvert.SerializeObject(clist);
            return jsonResult;
        }
    }
}

