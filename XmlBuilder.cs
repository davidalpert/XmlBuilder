using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

namespace SergioPereira.Xml
{
	/// <summary>
	/// Wraps the creation of XML documents
	/// </summary>
	public class XmlBuilder
	{
		private XmlBuilder()
		{
		}

		/// <summary>
		/// Starts an xml document
		/// </summary>
		/// <param name="build">logic to build the xml document</param>
		/// <returns>A string with the xml document</returns>
		public static string Build(Action<XmlDocumentBuilder> build)
		{
			return Build(Encoding.UTF8, build);
		}

		/// <summary>
		/// Starts an xml document
		/// with the appropriate text encoding
		/// </summary>
		/// <param name="encoding">the desired text encoding</param>
		/// <param name="build">logic to build the xml document</param>
		/// <returns>A string with the xml document</returns>
		public static string Build(Encoding encoding, Action<XmlDocumentBuilder> build)
		{
			MemoryStream mem = new MemoryStream();
			Build(mem, encoding, build);
			byte[] buffer = new byte[mem.Length];
			mem.Position = 0;
			mem.Read(buffer, 0, buffer.Length);

			return encoding.GetString(buffer);
		}

		/// <summary>
		/// Starts an xml document file
		/// </summary>
		/// <param name="fileName">path to the output file</param>
		/// <param name="build">logic to build the xml document</param>
		/// <returns></returns>
		public static XmlDocumentBuilder Build(string fileName, Action<XmlDocumentBuilder> build)
		{
			return Build(fileName, Encoding.UTF8, build);

		}

		/// <summary>
		/// Starts an xml document file
		/// with the appropriate text encoding
		/// </summary>
		/// <param name="fileName">path to the output file</param>
		/// <param name="encoding">the desired text encoding</param>
		/// <param name="build">logic to build the xml document</param>
		/// <returns>A document builder</returns>
		public static XmlDocumentBuilder Build(string fileName, Encoding encoding, Action<XmlDocumentBuilder> build)
		{
			using(FileStream f = new FileStream(fileName, FileMode.Create))
			{
				return Build(f, encoding, build);
			}
		}

		/// <summary>
		/// Starts an xml document stream
		/// </summary>
		/// <param name="stream">the output stream that will receive the xml text</param>
		/// <param name="build">logic to build the xml document</param>
		/// <returns>A document builder</returns>
		public static XmlDocumentBuilder Build(Stream stream, Action<XmlDocumentBuilder> build)
		{
			return Build(stream, Encoding.UTF8, build);
		}

		/// <summary>
		/// Starts an xml document stream
		/// with the appropriate encoding
		/// </summary>
		/// <param name="stream">the output stream that will receive the xml text</param>
		/// <param name="encoding">the desired text encoding</param>
		/// <param name="build">logic to build the xml document</param>
		/// <returns>A document builder</returns>
		public static XmlDocumentBuilder Build(Stream stream, Encoding encoding, Action<XmlDocumentBuilder> build)
		{
			XmlDocumentBuilder builder = new XmlDocumentBuilder(new XmlTextWriter(stream, encoding));

			builder.Writer.WriteStartDocument();
			build(builder); //<-- document content is generated here
			builder.Writer.WriteEndDocument();
			builder.Writer.Flush();

			return builder;
		}
		
	}
}
