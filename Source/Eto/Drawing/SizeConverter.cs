using System;
using System.ComponentModel;
using System.Globalization;

namespace Eto.Drawing
{
	/// <summary>
	/// Converter for the <see cref="Size"/> class
	/// </summary>
	/// <remarks>
	/// Allows for conversion from a string to a <see cref="Size"/>.
	/// </remarks>
	/// <copyright>(c) 2014 by Curtis Wensley</copyright>
	/// <license type="BSD-3">See LICENSE for full terms</license>
	public class SizeConverter : TypeConverter
	{
		/// <summary>
		/// Determines if this converter can convert from the specified <paramref name="sourceType"/>
		/// </summary>
		/// <param name="context">Conversion context</param>
		/// <param name="sourceType">Type to convert from</param>
		/// <returns>True if this converter can convert from the specified type, false otherwise</returns>
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		/// <summary>
		/// Converts the specified value to a <see cref="Size"/>
		/// </summary>
		/// <param name="context">Conversion context</param>
		/// <param name="culture">Culture to perform the conversion</param>
		/// <param name="value">Value to convert</param>
		/// <returns>A new instance of a <see cref="Size"/> converted from the specified <paramref name="value"/></returns>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			var text = value as string;
			if (text != null)
			{
				var parts = text.Split(CultureInfo.InvariantCulture.TextInfo.ListSeparator.ToCharArray());
				if (parts.Length != 2)
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Cannot parse value '{0}' as size.  Should be in the form of 'width,height'", text));

				var converter = new Int32Converter();
				return new Size(
					(int)converter.ConvertFromString(context, CultureInfo.InvariantCulture, parts[0]),
					(int)converter.ConvertFromString(context, CultureInfo.InvariantCulture, parts[1])
				);
			}
			return base.ConvertFrom(context, CultureInfo.InvariantCulture, value);
		}

		/// <summary>
		/// Converts the given value object to the specified type, using the specified context and culture information.
		/// </summary>
		/// <returns>The to.</returns>
		/// <param name="context">Context.</param>
		/// <param name="culture">Culture.</param>
		/// <param name="value">Value.</param>
		/// <param name="destinationType">Destination type.</param>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{  
			if (destinationType == typeof(string))
			{
				return ((Size)value).Width.ToString(CultureInfo.InvariantCulture) + "," + ((Size)value).Height.ToString(CultureInfo.InvariantCulture);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}

