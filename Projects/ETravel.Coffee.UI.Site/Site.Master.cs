using System;
using System.Web.UI;
using ETravel.Coffee.UI.Site.Properties;

namespace ETravel.Coffee.UI.Site
{
	public partial class SiteMaster : MasterPage
	{
		protected internal Settings Settings { get; set; }

		public SiteMaster()
		{
			Settings = Settings.Default;
		}

		protected void Page_Load(object sender, EventArgs e)
		{

		}
	}
}
