/*
Copyright 2012 Chris Moschini, Brass Nine Design

This code is licensed under the LGPL or MIT license, whichever you prefer.
*/

using System;


namespace Brass9.TimeZone
{
	/// <summary>
	/// A DateTime object in UTC, and a TimeZoneInfo object representing what timezone it's meant to target.
	/// </summary>
	public class UtcTimeZone
	{
		/// <summary>
		/// The time stored, represented in UTC.
		/// </summary>
		public DateTime Utc { get; set; }

		/// <summary>
		/// The timezone this time originated from.
		/// </summary>
		public TimeZoneInfo TimeZone { get; set; }


		protected UtcTimeZone(DateTime utc, TimeZoneInfo timeZone)
		{
			if (utc.Kind == DateTimeKind.Local)
				throw new ArgumentException("DateTime utc must be .Kind == DateTimeKind.Utc, not Local.", "utc");
			
			Utc = utc;
			TimeZone = timeZone;
		}

		public static UtcTimeZone FromUtc(DateTime utc, TimeZoneInfo timeZone)
		{
			if (utc.Kind == DateTimeKind.Local)
				throw new ArgumentException("DateTime utc must be .Kind == DateTimeKind.Utc, not Local.", "utc");

			return new UtcTimeZone(utc, timeZone);
		}
		public static UtcTimeZone FromUtcAndTimeZoneId(DateTime utc, TimeZoneId timeZoneId)
		{
			if (utc.Kind == DateTimeKind.Local)
				throw new ArgumentException("DateTime utc must be .Kind == DateTimeKind.Utc, not Local.", "utc");

			return FromUtc(utc, TimeZoneIdMap.Current.FindSystemTimeZoneById(timeZoneId));
		}
		public static UtcTimeZone FromUtcAndShort(DateTime utc, string timezoneShortname)
		{
			if (utc.Kind == DateTimeKind.Local)
				throw new ArgumentException("DateTime utc must be .Kind == DateTimeKind.Utc, not Local.", "utc");

			return FromUtc(utc, TimeZoneShortNameMap.Current.TimeZoneForShortName(timezoneShortname));
		}

		public static UtcTimeZone FromLocal(DateTime local, TimeZoneInfo timeZone)
		{
			if (local.Kind == DateTimeKind.Utc)
				throw new ArgumentException("DateTime utc must be .Kind == DateTimeKind.Local, not Utc.", "local");

			var u = new UtcTimeZone(TimeZoneInfo.ConvertTimeToUtc(local, timeZone), timeZone);
			return u;
		}
		public static UtcTimeZone FromLocalAndTimeZoneId(DateTime local, TimeZoneId timeZoneId)
		{
			if (local.Kind == DateTimeKind.Utc)
				throw new ArgumentException("DateTime utc must be .Kind == DateTimeKind.Local, not Utc.", "local");

			return FromLocal(local, TimeZoneIdMap.Current.FindSystemTimeZoneById(timeZoneId));
		}
		public static UtcTimeZone FromLocalAndShort(DateTime local, string timezoneShortname)
		{
			if (local.Kind == DateTimeKind.Utc)
				throw new ArgumentException("DateTime utc must be .Kind == DateTimeKind.Local, not Utc.", "local");

			return FromLocal(local, TimeZoneShortNameMap.Current.TimeZoneForShortName(timezoneShortname));
		}



		/// <summary>
		/// Gets a DateTime object mapped to the current timezone.
		/// </summary>
		public DateTime Local
		{
			get
			{
				return TimeZoneInfo.ConvertTimeFromUtc(Utc, this.TimeZone);
			}
		}

		/// <summary>
		/// Whether this timezone is in daylight savings according to the TimeZoneInfo data.
		/// 
		/// Note that weirdness with the TimeZoneInfo object means this will sometimes return true for timezones that have
		/// no Daylight Savings rules; if you're trying to determine if a timezone has a DST version, it may be simpler to
		/// check whether it has a DST shortname mapped.
		/// </summary>
		public bool IsDaylightSavings
		{
			get
			{
				return this.TimeZone.IsDaylightSavingTime(Utc);
			}
		}

		/// <summary>
		/// The short name for the timezone this date/timezone corresponds to, like PST, PDT, HKT.
		/// </summary>
		public string TimeZoneShortName
		{
			get
			{
				return TimeZoneShortNameMap.Current.ShortNameForTime(Utc, TimeZone);
			}
		}



		/// <summary>
		/// Switches this object to another timezone.
		/// </summary>
		/// <param name="shortTz"></param>
		public void SwitchTimeZone(string shortTz)
		{
			SwitchTimeZone(TimeZoneShortNameMap.Current.TimeZoneForShortName(shortTz));
		}
		public void SwitchTimeZone(TimeZoneId timeZoneId)
		{
			SwitchTimeZone(TimeZoneIdMap.Current.FindSystemTimeZoneById(timeZoneId));
		}
		public void SwitchTimeZone(TimeZoneInfo timeZoneInfo)
		{
			TimeZone = timeZoneInfo;
		}

		/// <summary>
		/// Gets a DateTime object for this time in the requested timezone.
		/// </summary>
		/// <param name="shortTz"></param>
		public void ToLocal(string shortTz)
		{
			ToLocal(TimeZoneShortNameMap.Current.TimeZoneForShortName(shortTz));
		}
		public void ToLocal(TimeZoneId timeZoneId)
		{
			ToLocal(TimeZoneIdMap.Current.FindSystemTimeZoneById(timeZoneId));
		}
		public void ToLocal(TimeZoneInfo timeZoneInfo)
		{
			TimeZoneInfo.ConvertTimeFromUtc(Utc, timeZoneInfo);
		}
	}
}
