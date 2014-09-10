using System;
using System.IO;
using System.Reflection;
using System.Xaml;

namespace Eto.Serialization.Xaml
{

	/// <summary>
	/// Methods to help load/save Eto objects to/from xaml
	/// </summary>
	public static class XamlReader
	{

		/// <summary>
		/// Loads the specified type from a xaml of the same name
		/// </summary>
		/// <remarks>
		/// If your class name is MyNamespace.MyType, then this will attempt to load MyNamespace.MyType.xaml
		/// for the xaml definition in the same assembly.
		/// 
		/// If you want to specify a different xaml, use <see cref="Load{T}(Stream)"/>
		/// </remarks>
		/// <typeparam name="T">Type of object to load from xaml</typeparam>
		/// <returns>A new instance of the specified type with the contents loaded from xaml</returns>
		public static T Load<T> ()
			where T: Widget, new()
		{
			var type = typeof(T);
			var stream = type.Assembly.GetManifestResourceStream (type.FullName + ".xaml");
			return Load<T> (stream, null);
		}
		
		/// <summary>
		/// Loads the specified type from the specified xaml stream
		/// </summary>
		/// <remarks>
		/// If your class name is MyNamespace.MyType, then this will attempt to load MyNamespace.MyType.xaml
		/// for the xaml definition in the same assembly.
		/// </remarks>
		/// <typeparam name="T">Type of object to load from the specified xaml</typeparam>
		/// <param name="stream">Xaml content to load (e.g. from resources)</param>
		/// <returns>A new instance of the specified type with the contents loaded from the xaml stream</returns>
		public static T Load<T> (Stream stream)
			where T: Widget, new()
		{
			return Load<T> (stream, null);
		}

		/// <summary>
		/// Loads the specified instance with xaml of the same name
		/// </summary>
		/// <remarks>
		/// If your class name is MyNamespace.MyType, then this will attempt to load MyNamespace.MyType.xaml
		/// for the xaml definition in the same assembly.
		/// 
		/// If you want to specify a different xaml, use <see cref="Load{T}(Stream, T)"/>
		/// </remarks>
		/// <typeparam name="T">Type of object to load from the specified xaml</typeparam>
		/// <param name="instance">Instance to use as the starting object</param>
		/// <returns>A new or existing instance of the specified type with the contents loaded from the xaml stream</returns>
		public static T Load<T> (T instance)
			where T: Widget
		{
			var type = typeof(T);
			var stream = type.Assembly.GetManifestResourceStream (type.FullName + ".xaml");
			return Load<T> (stream, instance);
		}

		/// <summary>
		/// Loads the specified instance with xaml of the same name
		/// </summary>
		/// <remarks>
		/// If your class name is MyNamespace.MyType, then this will attempt to load MyNamespace.MyType.xaml
		/// for the xaml definition in the same assembly.
		/// 
		/// If you want to specify a different xaml, use <see cref="Load{T}(Stream, T)"/>
		/// </remarks>
		/// <typeparam name="T">Type of object to load from the specified xaml</typeparam>
		/// <param name="instance">Instance to use as the starting object</param>
		/// <returns>A new or existing instance of the specified type with the contents loaded from the xaml stream</returns>
		public static T Load<T> (T instance, AfterEndInitHandlerDelegate afterEndInitHandlerDelegate)
			where T: Widget
		{
			var type = typeof(T);
			var stream = type.Assembly.GetManifestResourceStream (type.FullName + ".xaml");
			return Load<T> (stream, instance, afterEndInitHandlerDelegate);
		}

		public delegate void AfterEndInitHandlerDelegate (ref object instance);

		/// <summary>
		/// Loads the specified type from the specified xaml stream
		/// </summary>
		/// <typeparam name="T">Type of object to load from the specified xaml</typeparam>
		/// <param name="stream">Xaml content to load (e.g. from resources)</param>
		/// <param name="instance">Instance to use as the starting object</param>
		/// <returns>A new or existing instance of the specified type with the contents loaded from the xaml stream</returns>
		public static T Load<T> (Stream stream, T instance, AfterEndInitHandlerDelegate afterEndInitHandlerDelegate = null)
			where T : Widget
		{
			var type = typeof(T);
			var context = new EtoXamlSchemaContext (new Assembly[] { typeof(XamlReader).Assembly });
			var reader = new XamlXmlReader (stream, context);
			var writerSettings = new XamlObjectWriterSettings {
				RootObjectInstance = instance
			};

			writerSettings.AfterBeginInitHandler += delegate (object sender, XamlObjectEventArgs e) {
				if (afterEndInitHandlerDelegate != null) {
					var obj = e.Instance;
					afterEndInitHandlerDelegate (ref obj);
				}
			};
			writerSettings.AfterPropertiesHandler += delegate (object sender, XamlObjectEventArgs e) {
				var obj = e.Instance as Widget;
				if (obj != null && !string.IsNullOrEmpty (obj.ID)) {
					var property = type.GetProperty (obj.ID, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
					if (property != null)
						property.SetValue (instance, obj, null);
					else {
						var field = type.GetField (obj.ID, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
						if (field != null)
							field.SetValue (instance, obj);
					}
				}
			};

			var writer = new XamlObjectWriter (context, writerSettings);
			XamlServices.Transform (reader, writer);
			return writer.Result as T;
		}
	}
}