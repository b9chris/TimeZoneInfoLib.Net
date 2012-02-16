/*
Copyright 2012 Chris Moschini, Brass Nine Design

This code is licensed under the LGPL or MIT license, whichever you prefer.
*/

using System;
using System.Collections.Generic;


namespace Brass9
{
	/// <summary>
	/// Apply to Enum values to map custom strings to them
	/// </summary>
	public class StringValueAttribute : Attribute
	{
		protected string value;

		public StringValueAttribute(string value)
		{
			this.value = value;
		}

		public string Value
		{
			get
			{
				return value;
			}
		}


		protected static string getStringValue(object enumValue)
		{
			var type = enumValue.GetType();
			var fieldInfo = type.GetField(enumValue.ToString());
			var stringValueAttributes = (StringValueAttribute[])fieldInfo.GetCustomAttributes(typeof(StringValueAttribute), false);
			if (stringValueAttributes.Length == 0)
				throw new ArgumentException("Enum value " + type.Name + "." + enumValue.ToString() + " does not have StringValueAttribute applied.");

			return stringValueAttributes[0].value;
		}

		/// <summary>
		/// Call this to map custom strings to enum values and back
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="enumToString"></param>
		/// <param name="stringToEnum"></param>
		public static void Map<T>(Dictionary<T, string> enumToString, Dictionary<string, T> stringToEnum)
		{
			Type enumType = typeof(T);
			if (!enumType.IsEnum)
				throw new Exception("Type must be Enum.");

			var enumValues = (T[])Enum.GetValues(enumType);
			foreach (var enumVal in enumValues)
			{
				string stringVal = getStringValue(enumVal);
				enumToString.Add(enumVal, stringVal);
				stringToEnum.Add(stringVal, enumVal);
			}
		}
	}
}
