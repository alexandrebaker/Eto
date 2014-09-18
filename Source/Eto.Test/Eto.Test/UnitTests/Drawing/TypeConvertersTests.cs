using Eto.Drawing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Eto.Test.UnitTests.Drawing
{
	[TestFixture, Category(TestUtils.NoTestPlatformCategory)]
	public class TypeConvertersTests
	{
		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			// initialize test generator if running through IDE or nunit-gui
			TestUtils.Initialize();
		}

		[Test]
		public void TestColorConverter()
		{
			TestConversion(Color.FromArgb(255, 255, 255), new ColorConverter());
		}

		[Test]
		public void TestPaddingConverter()
		{
			TestConversion(new Padding(1), new PaddingConverter());
		}
			
		[Test]
		public void TestPointConverter()
		{
			TestConversion(new Point(1, 2), new PointConverter());
		}
			
		[Test]
		public void TestPointFConverter()
		{
			TestConversion(new PointF(1.1f, 2.2f), new PointFConverter());
		}

		[Test]
		public void TestRectangleConverter()
		{
			TestConversion(Rectangle.FromSides(1,1,2,2), new RectangleConverter());
		}

		[Test]
		public void TestRectangleFConverter()
		{
			TestConversion(RectangleF.FromSides(1.1f,1.2f,2.1f,2.2f), new RectangleFConverter());
		}
			
		[Test]
		public void TestSizeConverter()
		{
			TestConversion(new Size(1, 2), new SizeConverter());
		}

		[Test]
		public void TestSizeFConverter()
		{
			TestConversion(new SizeF(1.1f, 2.2f), new SizeFConverter());
		}

		public void TestConversion<ObjType,ConvType>(ObjType sourceObj, ConvType converter) where ConvType:System.ComponentModel.TypeConverter
		{
			string toConvert = (string)converter.ConvertTo(sourceObj, typeof(string));
			var destObj = converter.ConvertFrom(null, CultureInfo.CreateSpecificCulture("fr-CA"), toConvert);
			Assert.AreEqual(sourceObj, destObj);

			destObj = converter.ConvertFrom(null, CultureInfo.CreateSpecificCulture("en-US"), toConvert);
			Assert.AreEqual(sourceObj, destObj);

			destObj = converter.ConvertFrom(null, CultureInfo.CurrentCulture, toConvert);
			Assert.AreEqual(sourceObj, destObj);

			destObj = converter.ConvertFrom(null, CultureInfo.InvariantCulture, toConvert);
			Assert.AreEqual(sourceObj, destObj);
		}
	}
}
