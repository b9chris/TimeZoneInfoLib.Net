using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Brass9.TimeZone;


namespace TimeZoneInfoLibExample.Controllers
{
	public class HomeController : Controller
	{
		public ViewResult Index()
		{
			return View();
		}

		[HttpPost]
		public JsonResult TimeZoneDropDownList()
		{
			List<string> timezones = new List<string>();
			var tzIds = TimeZoneIdMap.Current.GetKnownTimeZoneIds();

			var tzs = tzIds.Select(t => new
			{
				Value = TimeZoneShortNameMap.Current.StandardNameForTimeZoneId(t),
				Text = generalShortName(t)
			});

			return Json(tzs);
		}

		[HttpPost]
		protected string generalShortName(TimeZoneId tzId)
		{
			string standard = TimeZoneShortNameMap.Current.StandardNameForTimeZoneId(tzId);
			string daylight = TimeZoneShortNameMap.Current.DaylightNameForTimeZoneId(tzId);

			string general = standard + (daylight != null ? "/" + daylight : "");

			return general;
		}

		[HttpPost]
		public JsonResult ConvertDateTime(string d, string startTz, string destTz)
		{
			DateTime dateTime;
			if (!DateTime.TryParse(d, out dateTime))
				return Json(null);

			var utcTz = DateTimeAndZone.FromLocalAndShort(dateTime, startTz);
			utcTz.SwitchTimeZone(destTz);
			var dateString = utcTz.Local.ToString("M/d/yyyy h:mmtt").ToLower() + " " + utcTz.TimeZoneShortName;
			return Json(dateString);
		}

		[HttpPost]
		public JsonResult ByStateAndCountry(string state, string country)
		{
			var tzId = TimeZoneByStateAndCountry.Current.GetTimeZoneId(state, country);
			if (tzId == TimeZoneId.None)
				return Json("Not found");

			string general = generalShortName(tzId);
			return Json(general);
		}

		[HttpPost]
		public JsonResult DateTimeByStateAndCountry(string d, string state2, string country2, string state3, string country3)
		{
			DateTime dateTime;
			if (!DateTime.TryParse(d, out dateTime))
				return Json("Not a valid date/time");

			var startTz = TimeZoneByStateAndCountry.Current.GetTimeZoneInfo(state2, country2, TimeZoneId.UTC);
			var destTz = TimeZoneByStateAndCountry.Current.GetTimeZoneInfo(state3, country3, TimeZoneId.UTC);

			var utcTz = DateTimeAndZone.FromLocal(dateTime, startTz);
			utcTz.SwitchTimeZone(destTz);
			var dateString = utcTz.Local.ToString("M/d/yyyy h:mmtt").ToLower() + " " + utcTz.TimeZoneShortName;

			if (startTz.Id == "UTC")
				dateString += " (start country not found)";

			if (destTz.Id == "UTC")
				dateString += " (dest country not found)";

			return Json(dateString);
		}
	}
}
