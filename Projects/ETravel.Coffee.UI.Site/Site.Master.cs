using System;
using System.Collections.Generic;
using System.Web.UI;
using ETravel.Coffee.UI.Site.Properties;

namespace ETravel.Coffee.UI.Site
{
	public partial class SiteMaster : MasterPage
	{
		protected internal Settings Settings { get; set; }
		protected internal IDictionary<string, string> Links { get; set; }

		public SiteMaster()
		{
			Settings = Settings.Default;

			Links = new Dictionary<string, string>
			{
				{ "Home", "Default.aspx" },
				{ "About", "About.aspx" }
			};
		}

		protected void Page_Load(object sender, EventArgs e)
		{

		}
	}
}
