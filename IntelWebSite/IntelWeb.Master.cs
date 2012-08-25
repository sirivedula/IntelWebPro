using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IntelWebSite
{
    public partial class IntelWeb : System.Web.UI.MasterPage
    {
        public bool UseMultiPartForm { get; set; }
        public string FormType = "application/x-www-form-urlencoded";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (UseMultiPartForm)
            {
                this.theForm.Enctype = "multipart/form-data";
            }
        }
    }
}
