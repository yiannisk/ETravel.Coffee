using System;
using ETravel.Coffee.Service;
using ServiceStack.MiniProfiler;

namespace ETravel.Coffee.Site
{
	public class Global : System.Web.HttpApplication
	{
		protected void Application_Start(object sender, EventArgs e)
		{
			CoffeeAppHost.Start();
		}

		protected void Application_BeginRequest(object sender, EventArgs e)
		{
			if (Properties.Settings.Default.UseProfiler)
			{
				Profiler.Start();
			}
		}

		protected void Application_EndRequest(object sender, EventArgs e)
		{
			if (Properties.Settings.Default.UseProfiler)
			{
				Profiler.Stop();
			}
		}
	}
}
