using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;

namespace SergioPereira.Xml
{
	/// <summary>
	/// Represents a generic xml content builder
	/// </summary>
	public abstract class XmlBuilderBase
	{
		protected XmlBuilderBase( XmlTextWriter writer )
		{
			Writer = writer;
		}

		internal XmlTextWriter Writer { get; set; }
		private bool _contentAdded = false;
		private bool _tagStarted = false;

		/// <summary>
		/// Insert comments
		/// </summary>
		/// <param name="comment"></param>
		public virtual void Comment( string comment )
		{
			Writer.WriteComment( comment );
			_contentAdded = true;
		}

		/// <summary>
		/// Starts a new child element
		/// </summary>
		/// <param name="build">logic that builds the element</param>
		public virtual void Element( Action<XmlElementBuilder> build )
		{
			string name = build.Method.GetParameters()[0].Name;
			Element( name, new Dictionary<string, string>(), build );
		}

		/// <summary>
		/// Starts a new child element
		/// </summary>
		/// <param name="localName">local name of the element</param>
		/// <param name="build">logic that builds the element</param>
		public virtual void Element( string localName, Action<XmlElementBuilder> build )
		{
			Element( localName, new Dictionary<string, string>(), build );
		}

		/// <summary>
		/// Starts a new child element
		/// </summary>
		/// <param name="localName">local name of the element</param>
		/// <param name="attributes">list of attributes for the element</param>
		/// <param name="build">logic that builds the element</param>
		public virtual void Element( string localName, IDictionary<string, string> attributes, Action<XmlElementBuilder> build )
		{
			XmlElementBuilder child = new XmlElementBuilder( localName, Writer );

			Writer.WriteStartElement( localName );
			child._tagStarted = true;

			foreach ( var att in attributes )
				child[att.Key] = att.Value;

			build( child );// <-- element content is generated here
			Writer.WriteEndElement();
			_contentAdded = true;
		}

		Dictionary<string, string> _attributes = new Dictionary<string, string>();

		/// <summary>
		/// Gets or sets an attribute value on the element
		/// </summary>
		/// <param name="attributeName">Name of the attribute</param>
		/// <returns>The value of the attribute</returns>
		public string this[string attributeName]
		{
			get
			{
				if ( _attributes.ContainsKey( attributeName ) )
					return _attributes[attributeName];
				return null;
			}

			set
			{
				if ( _contentAdded )
					throw new InvalidOperationException( "Cannot add attributes after content has been added to the element." );

				_attributes[attributeName] = value;

				if ( _tagStarted )
					Writer.WriteAttributeString( attributeName, value );
			}
		}
	}
}
