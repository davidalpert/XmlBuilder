using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SergioPereira.Xml
{
	/// <summary>
	/// Creates an xml element
	/// </summary>
	public class XmlElementBuilder : XmlBuilderBase
	{
		internal XmlElementBuilder( string localName, XmlTextWriter writer )
			: base( writer )
		{
			Name = localName;
		}

		public string Name { get; protected set; }

		public void AppendText( string text )
		{
			Writer.WriteString( text );
		}

		public void AppendText( object o )
		{
			if ( o == null )
				return;

			Writer.WriteString( o.ToString() );
		}
	}
}
