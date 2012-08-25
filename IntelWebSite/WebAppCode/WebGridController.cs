using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Globalization;
using System.Data;
using IntelWeb;
using IntelWeb.IntelWebObjects;

namespace IntelWebSite
{
    public class WebGridController
    {
        private WebGrid _grid;
        public WebGrid grid
        {
            get { return _grid; }
            set { _grid = value; }
        }

        public WebGridController()
        {

        }

        public static WebGridController ControllerFromName(string name, NameValueCollection form, ControlCollection controls, CurrentUser user)
        {
            WebGridController result;
            switch (name)
            {
                case "lkup_department":
                    result = new DeptGrid(form, controls, user);
                    return result;
                case "lkup_site":
                    result = new SiteGrid(form, controls, user);
                    return result;
                default:
                    break;
            }

            return null;
        }
    }

    public class DeptGrid : WebGridController
    {
        public DeptGrid(NameValueCollection form, ControlCollection controls, CurrentUser user)
        {
            grid = new WebGrid("lkup_department", user, "deptGrid");
            grid.objectId = "lkup_department";
            grid.form = form;
            grid.controls = controls;
            grid.fields.Add(new DisplayField(grid, "department_code", "Dept Code", null, true, "department_code"));
            grid.fields.Add(new DisplayField(grid, "department_code_description", "Dept Description", null, true, "department_code_description"));
            grid.fields.Add(new DisplayField(grid, "first_name", "Contact Name", null, true, "first_name"));
            grid.fields.Add(new DisplayField(grid, "last_name", "Last Name", null, false, "last_name"));

        }
    }

    public class SiteGrid : WebGridController
    {
        public SiteGrid(NameValueCollection form, ControlCollection controls, CurrentUser user)
        {
            grid = new WebGrid("lkup_site", user, "siteGrid");
            grid.objectId = "lkup_site";
            grid.form = form;
            grid.controls = controls;
            grid.fields.Add(new DisplayField(grid, "department_code", "Dept Code", null, true, "department_code"));
            grid.fields.Add(new DisplayField(grid, "site_code", "Site Code", null, true, "site_code"));
            grid.fields.Add(new DisplayField(grid, "site_code_description", "Site Description", null, true, "site_code_description"));
            grid.fields.Add(new DisplayField(grid, "building_name", "Building Name", null, true, "building_name"));
            grid.fields.Add(new DisplayField(grid, "facility_contact", "Facility Contact", null, true, "facility_contact"));
            grid.fields.Add(new DisplayField(grid, "address1", "Address1", null, false, "address1"));
            grid.fields.Add(new DisplayField(grid, "address2", "Address2", null, false, "address2"));
            grid.fields.Add(new DisplayField(grid, "city", "City", null, true, "city"));
            grid.fields.Add(new DisplayField(grid, "state_code", "State", null, true, "state_code"));
            grid.fields.Add(new DisplayField(grid, "zip_code", "Zip", null, false, "zip_code"));
            grid.fields.Add(new DisplayField(grid, "country_code", "Country", null, true, "country_code"));
            grid.fields.Add(new DisplayField(grid, "mobile_number", "Cell", null, false, "mobile_number"));
            grid.fields.Add(new DisplayField(grid, "land_number", "Home", null, false, "land_number"));
            grid.fields.Add(new DisplayField(grid, "fax_number", "Fax", null, false, "fax_number"));
            grid.fields.Add(new DisplayField(grid, "email_id", "EMail", null, false, "email_id"));

        }
    }


    public class GridDefUtil
    {
        public static Control getControl(ControlCollection cc, string controlName)
        {
            foreach (Control c in cc)
            {
                if (c.ID != null && c.ID.Equals(controlName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return c;
                }

            }
            return null;
        }
    }

}
