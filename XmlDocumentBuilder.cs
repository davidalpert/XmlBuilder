using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace SergioPereira.Xml
{
	/// <summary>
	/// Creates an xml document
	/// </summary>
	public class XmlDocumentBuilder : XmlBuilderBase
	{

		internal XmlDocumentBuilder(XmlTextWriter writer)
			: base(writer)
		{
			OutputStream = writer.BaseStream;
		}

		private bool _elementAdded = false;

		public Stream OutputStream { get; private set; }

		/// <summary>
		/// Adds the root element of the xml document
		/// </summary>
		/// <param name="build"></param>
		public void Root(Action<XmlElementBuilder> build)
		{
			Element(build);
		}

		/// <summary>
		/// Adds the root element of the xml document
		/// </summary>
		/// <param name="name"></param>
		/// <param name="build"></param>
		public void Root(string name, Action<XmlElementBuilder> build)
		{
			Element(name, build);
		}

		public override void Element(string name, Action<XmlElementBuilder> build)
		{
			if(_elementAdded)
				throw new InvalidOperationException("Document can only have one top level element.");
			
			base.Element(name, build);
			_elementAdded = true;
		}

		#region Properties delegated to the XmlTextWriter

		public Formatting Formatting { get { return Writer.Formatting; } set { Writer.Formatting = value; } }
		public int Indentation { get { return Writer.Indentation; } set { Writer.Indentation = value; } }
		public char IndentChar { get { return Writer.IndentChar; } set { Writer.IndentChar = value; } }
		public char QuoteChar { get { return Writer.QuoteChar; } set { Writer.QuoteChar = value; } }

		#endregion

	}
}
