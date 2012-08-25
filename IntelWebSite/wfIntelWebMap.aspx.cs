using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data;
using IntelWeb;

namespace IntelWebSite
{
    public partial class wfIntelWebMap : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {

            CurrentUser cuser = new CurrentUser();
            cuser.userName = "Balaji";
            cuser.Load();

            string formTier = Request.Form["tiercode"] ?? "";
            string formdept = Request.Form["deptcode"] ?? "";
            string formfdate = Request.Form["fromdate"] ?? "";
            string formtdate = Request.Form["todate"] ?? "";
            
            // Testing
            if (string.IsNullOrEmpty(formfdate)) formfdate = "04/01/2011";            
            if (string.IsNullOrEmpty(formtdate)) formtdate = "04/30/2011";

            /*
             * if (string.IsNullOrEmpty(formfdate)) formfdate = DateTime.Today.ToString("MM/dd/yyyy");
             * if (string.IsNullOrEmpry(formtdate)) formtdate = DateTime.Today.AddMonths(1).ToString("MM/dd/yyyy");
            */

            lkup_tier tier = new lkup_tier(cuser);
            List<IntelWebObject> tiers = tier.Load();
            this.ltrTiers.Text = "<option value=\"\"></option>" + string.Join("", tiers.Cast<lkup_tier>().Select(x=>"<option value=\"" + HttpUtility.HtmlEncode(x.tier_code) + "\" " + (formTier.Equals(x.tier_code)?"selected":"") + " >" + HttpUtility.HtmlEncode(x.tier_code) + "</option>").ToArray());

            lkup_department lkupDept = new lkup_department(cuser);
            List<IntelWebObject> lkupDepts = lkupDept.Load();
            this.ltrDepts.Text = "<option value=\"\"></option>" + string.Join("", lkupDepts.Cast<lkup_department>().Select(x => "<option value=\"" + HttpUtility.HtmlEncode(x.department_code) + "\" " + (formdept.Equals(x.department_code)?"selected":"") +  " >" + HttpUtility.HtmlEncode(x.department_code + " (" + x.department_code_description + ")") + "</option>").ToArray());

            tbl_alarm_time_details alarmDet = new tbl_alarm_time_details(cuser);
            List<IntelWebField> bparams = new List<IntelWebField>();
            IntelWebField tierFld = alarmDet.Field("tier_code");
            tierFld.fieldValue = formTier;
            IntelWebField deptFld = alarmDet.Field("department_code");
            deptFld.fieldValue = formdept;
            IntelWebField fdateFld = new IntelWebField("todate", OleDbType.DBDate, 10, 10, 0, DataRowVersion.Current, false, formfdate);
            IntelWebField tdateFld = new IntelWebField("todate", OleDbType.DBDate, 10, 10, 0, DataRowVersion.Current, false, formtdate);
            bparams.Add(tierFld);
            bparams.Add(deptFld);
            bparams.Add(fdateFld);
            bparams.Add(tdateFld);

            List<IntelWebObject> alaramDets = alarmDet.Load("tier_code=? and department_code=? and alarm_date between ? and ?", "site_code,panel_code,alarm_device_code", bparams);
            if (alaramDets.Count > 0)
            {
                var distSites = alaramDets.Select(x => x.Field("site_code").fieldValue ?? "").Distinct().ToList();
                /* Distinct Sites get Address City, State, Country */
                lkup_site lkupsite = new lkup_site(cuser);
                bparams.Clear();
                string qs = "";
                foreach (string site in distSites)
                {
                    qs += ("?,");
                    IntelWebField fld = new IntelWebField("site_code", OleDbType.VarChar, 50, 50, 0, DataRowVersion.Current, false, null);
                    fld.fieldValue = site;
                    bparams.Add(fld);
                }
                bparams.Add(IntelWebParameter.GetParam("deptcode", OleDbType.VarChar, 50, formdept));
                if (qs.Length > 0) qs = qs.Substring(0, qs.Length - 1);
                List<IntelWebObject> Sites = lkupsite.Load("(site_code in (" + qs + ") or department_code=?)and city is not null ", "city", bparams);
                Dictionary<string, string> distCities = new Dictionary<string, string>();
                foreach (lkup_site site in Sites)
                {
                    string mapcity = site.city + (((site.state_code ?? "").Length > 0) ? "," + site.state_code.Trim() : "");
                    string siteDets = site.site_code + "\t" + site.address1 + "\t" + site.address2 + "\t" + site.city + "\t" + site.country_code;
                    if (distCities.ContainsKey(mapcity))
                    {
                        distCities[mapcity] = distCities[mapcity] + "\n" + siteDets;
                    }
                    else
                    {
                        distCities.Add(mapcity, siteDets);
                    }
                }

                string jsCities = "[";
                foreach (string key in distCities.Keys)
                {
                    jsCities += "{\"address\":\"" + key + "\",\"sitesHtml\":\"";
                    string[] arySites = distCities[key].Split('\n');
                    jsCities += "<table style=\\\"width:95%;\\\"><tr>";
                    foreach (string st in arySites)
                    {
                        string[] arySiteAddr = st.Split('\t');
                        jsCities += "<td>";
                        if (arySiteAddr.Length > 0)
                        {
                            jsCities += "<div><img style=\\\"cursor:pointer;\\\" onclick=\\\"showDiv(this, '" + arySiteAddr[0] + "');\\\" src=\'Images/house.png\' alt=\'site\' /> " + JSUtil.EnquoteJS(HttpUtility.HtmlEncode(arySiteAddr[0])) + " </div>";
                        }
                        if(arySiteAddr.Length > 3)
                        {
                            jsCities += "<div style=\\\"border:1px solid #999;padding:2px;white-space:nowrap;\\\"><div>" + JSUtil.EnquoteJS(HttpUtility.HtmlEncode(arySiteAddr[1])) + "</div><div>" + JSUtil.EnquoteJS(HttpUtility.HtmlEncode(arySiteAddr[2])) + "</div><div>" + JSUtil.EnquoteJS(HttpUtility.HtmlEncode(arySiteAddr[3] + " " + arySiteAddr[4])) + "</div></div>";
                        }
                        jsCities += "</td>";
                        //<br />" + st + "<br />   Peak:50 <br />Non-Peak:20</td>";
                    }
                    jsCities += "</tr></table>\"},";
                }
                if (distCities.Count > 0) jsCities = jsCities.Substring(0, jsCities.Length - 1);
                jsCities += "];";

                /* Map Script STARTS */
                ltrMapScript.Text = @"<script type=""text/javascript""> 
                jsMapCities = " + jsCities + @"
                jsFromDate = '" + formfdate + @"';
                jsToDate = '" + formtdate + @"';
                function mapInit(){         
                    if (GBrowserIsCompatible()) {
                        var map = new GMap2(document.getElementById('map_canvas'));
			            map.addControl(new GLargeMapControl());
			            map.addControl(new GMapTypeControl());
			            map.addControl(new GOverviewMapControl());
			            map.enableScrollWheelZoom();
			            map.setCenter(new GLatLng(39, -93), 4);
                        map.setUIToDefault();
                        var bounds = new GLatLngBounds();
                        geocoder = new GClientGeocoder();

                        function createMarker(address, thtml, cnt){
			                // Creates the  customized marker icon
                            var blueIcon = new GIcon(G_DEFAULT_ICON);
                            if(cnt % 2 == 0) {
                                blueIcon.image = ""http://iweb.vasbal.com/images/grn-pushpin.png"";
                            } else if(cnt % 3 == 0) {
                                blueIcon.image = ""http://iweb.vasbal.com/images/red-pushpin.png"";
                            } else {
                                blueIcon.image = ""http://iweb.vasbal.com/images/ylw-pushpin.png"";
                            }
                            blueIcon.iconSize = new GSize(32, 32);

                            // Set up our GMarkerOptions object
                            var markerOptions = { icon:blueIcon };

                            if(geocoder){
                                geocoder.getLatLng(address, function(point) {
                                        if (point) {
                                            var marker = new GMarker(point, markerOptions); 
                                            map.addOverlay(marker);
                                            bounds.extend(marker.getPoint());
                                            var mhtml = thtml;
                                            var mklatlng = new GLatLng(marker.getPoint().lat(), marker.getPoint().lng());
                                            GEvent.addListener(marker, ""click"", function () { map.openInfoWindowHtml(mklatlng, '<div>'+mhtml+'</div>'); });
                                            GEvent.addListener(marker, ""mouseover"", function () { map.openInfoWindowHtml(mklatlng, '<div>'+mhtml+'</div>'); });
                                                                     
                                        }
                                });
                            }
                        }
    
                        map.clearOverlays();
                        var cnt = 0;
                        for(var x in jsMapCities){
                            createMarker(jsMapCities[x].address, jsMapCities[x].sitesHtml, cnt);
                            cnt++;
                        }
                    }
                }
                </script>";
                /* End Script */
            }
            else
            {
                ltrMessage.Text = @"<div class=""ui-widget"">
                <div class=""ui-state-error ui-corner-all"" style=""width:70%;margin-left:200px;margin-top: 5px; padding: 0 .7em;""> 
                <p style=""height:35px;""><span class=""ui-icon ui-icon-alert"" style=""float: left; margin-right: .3em;""></span> 
                <strong>Alert:</strong>There are no alarm details found between selected date period.</p></div></div>";

                ltrMapScript.Text = @"<script type=""text/javascript""> 
                function mapInit(){         
                    if (GBrowserIsCompatible()) {
                        var map = new GMap2(document.getElementById('map_canvas'));
			            map.addControl(new GLargeMapControl());
			            map.addControl(new GMapTypeControl());
			            map.addControl(new GOverviewMapControl());
			            map.enableScrollWheelZoom();
			            map.setCenter(new GLatLng(39, -93), 4);
                        map.setUIToDefault();
                    }
                }
                </script>";
            }
        }
    }
}
