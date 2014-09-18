using System;
using System.ComponentModel;
using System.Globalization;

namespace Eto.Drawing
{
	/// <summary>
	/// Converter for the <see cref="Padding"/> class
	/// </summary>
	/// <copyright>(c) 2014 by Curtis Wensley</copyright>
	/// <license type="BSD-3">See LICENSE for full terms</license>
	public class PaddingConverter : TypeConverter
	{
		/// <summary>
		/// Determines if the specified <paramref name="sourceType"/> can be converted to a <see cref="Padding"/> object
		/// </summary>
		/// <param name="context">Conversion context</param>
		/// <param name="sourceType">Type to convert from</param>
		/// <returns>True if this converter can convert from the specified type, false otherwise</returns>
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		/// <summary>
		/// Converts the specified value to a <see cref="Padding"/> object
		/// </summary>
		/// <param name="context">Conversion context</param>
		/// <param name="culture">Culture to perform the conversion for</param>
		/// <param name="value">Value to convert</param>
		/// <returns>A new instance of the <see cref="Padding"/> object with the value represented by <paramref name="value"/></returns>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string text = value as string;
			if (text != null)
			{
				var parts = text.Split(CultureInfo.InvariantCulture.TextInfo.ListSeparator.ToCharArray());
				var converter = new Int32Converter();
				switch (parts.Length)
				{
					case 1:
						return new Padding(
							(int)converter.ConvertFromString(context, CultureInfo.InvariantCulture, parts[0])
						);
					case 2:
						return new Padding(
							(int)converter.ConvertFromString(context, CultureInfo.InvariantCulture, parts[0]),
							(int)converter.ConvertFromString(context, CultureInfo.InvariantCulture, parts[1])
						);
					case 4:
						return new Padding(
							(int)converter.ConvertFromString(context, CultureInfo.InvariantCulture, parts[0]),
							(int)converter.ConvertFromString(context, CultureInfo.InvariantCulture, parts[1]),
							(int)converter.ConvertFromString(context, CultureInfo.InvariantCulture, parts[2]),
							(int)converter.ConvertFromString(context, CultureInfo.InvariantCulture, parts[3])
						);
					default:
						throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Cannot parse value '{0}'. Should be in the form of 'all', 'horizontal,vertical', or 'left, top, right, bottom'", text));
				}
					
			}
			return base.ConvertFrom(context, CultureInfo.InvariantCulture, value);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {  
			if (destinationType == typeof(string)) {
				return ((Padding)value).Left.ToString(CultureInfo.InvariantCulture) + "," + ((Padding)value).Top.ToString(CultureInfo.InvariantCulture)
					+ "," + ((Padding)value).Right.ToString(CultureInfo.InvariantCulture) + "," + ((Padding)value).Bottom.ToString(CultureInfo.InvariantCulture);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}

