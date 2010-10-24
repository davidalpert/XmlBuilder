using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SergioPereira.Xml;
using Should;
using Xunit;
using System.Xml;
using System.IO;

namespace SergioPereira.Xml
{
    public class XmlBuilder_Facts
    {
        [Fact]
        public void Can_Build_to_a_string()
        {
            string s = XmlBuilder.Build(Encoding.ASCII, xml =>
            {
                xml.Indentation = 4;
                xml.Formatting = System.Xml.Formatting.Indented;

				xml.Root( children =>
				{
					children.Comment( "Children below..." );

					for ( int i = 1; i < 10; i++ )
					{
						children.Element( child =>
						{
							child["age"] = i.ToString();
							child["referenceNumber"] = "ref-" + i;

							child.AppendText( "child & content #" + i );
						} );
					}
				} );
			} );
			s.ShouldEqual(
@"<?xml version=""1.0"" encoding=""us-ascii""?>
<children>
    <!--Children below...-->
    <child age=""1"" referenceNumber=""ref-1"">child &amp; content #1</child>
    <child age=""2"" referenceNumber=""ref-2"">child &amp; content #2</child>
    <child age=""3"" referenceNumber=""ref-3"">child &amp; content #3</child>
    <child age=""4"" referenceNumber=""ref-4"">child &amp; content #4</child>
    <child age=""5"" referenceNumber=""ref-5"">child &amp; content #5</child>
    <child age=""6"" referenceNumber=""ref-6"">child &amp; content #6</child>
    <child age=""7"" referenceNumber=""ref-7"">child &amp; content #7</child>
    <child age=""8"" referenceNumber=""ref-8"">child &amp; content #8</child>
    <child age=""9"" referenceNumber=""ref-9"">child &amp; content #9</child>
</children>" );

		}

		[Fact]
		public void Can_Build_to_a_file()
		{
			XmlBuilder.Build( "file.xml", Encoding.UTF8, xml =>
			{
				xml.Indentation = 4;
				xml.Formatting = System.Xml.Formatting.Indented;

				xml.Root( children =>
				{
					children.Comment( "Children below..." );

					for ( int i = 1; i < 10; i++ )
					{
						children.Element( child =>
						{
							child["age"] = i.ToString();
							child["referenceNumber"] = "ref-" + i;

							child.AppendText( "child & content #" + i );
						} );
					}
				} );
			} );

			File.Exists( "file.xml" ).ShouldBeTrue();
			File.ReadAllText( "file.xml" ).ShouldEqual(
@"<?xml version=""1.0"" encoding=""utf-8""?>
<children>
    <!--Children below...-->
    <child age=""1"" referenceNumber=""ref-1"">child &amp; content #1</child>
    <child age=""2"" referenceNumber=""ref-2"">child &amp; content #2</child>
    <child age=""3"" referenceNumber=""ref-3"">child &amp; content #3</child>
    <child age=""4"" referenceNumber=""ref-4"">child &amp; content #4</child>
    <child age=""5"" referenceNumber=""ref-5"">child &amp; content #5</child>
    <child age=""6"" referenceNumber=""ref-6"">child &amp; content #6</child>
    <child age=""7"" referenceNumber=""ref-7"">child &amp; content #7</child>
    <child age=""8"" referenceNumber=""ref-8"">child &amp; content #8</child>
    <child age=""9"" referenceNumber=""ref-9"">child &amp; content #9</child>
</children>" );
		}

		[Fact]
		public void Build_is_equivalent_to_DOM()
		{
			var expected = WithDOM();
			var actual = XmlBuilder.Build( xml =>
			{
				xml.Formatting = System.Xml.Formatting.None;

				xml.Root( children =>
				{
					children.Comment( "Children below..." );

					for ( int i = 1; i < 10; i++ )
					{
						children.Element( child =>
						{
							child["age"] = i.ToString();
							child["referenceNumber"] = "ref-" + i;

							child.AppendText( "child & content #" + i );
						} );
					}
				} );
			} );

			actual.Substring( 1 ).ShouldEqual( expected );
		}

		private string WithDOM()
		{
			XmlDocument xml = new XmlDocument();
			XmlDeclaration declaration = xml.CreateXmlDeclaration( "1.0", "utf-8", String.Empty );
			xml.AppendChild( declaration );

			XmlElement root = xml.CreateElement( "children" );
			xml.AppendChild( root );

			XmlComment comment = xml.CreateComment( "Children below..." );
			root.AppendChild( comment );

			for ( int i = 1; i < 10; i++ )
			{
				XmlElement child = xml.CreateElement( "child" );
				child.SetAttribute( "age", i.ToString() );
				child.SetAttribute( "referenceNumber", "ref-" + i );
				child.InnerText = "child & content #" + i;
				root.AppendChild( child );
			}

			return xml.OuterXml;
		}

		[Fact]
		public void Build_is_equivalent_to_XmlTextWriter()
		{
			var expected = WithWriter();
			var actual = XmlBuilder.Build( Encoding.UTF8, xml =>
			{
				xml.Formatting = System.Xml.Formatting.None;

				xml.Root( children =>
				{
					children.Comment( "Children below..." );

					for ( int i = 1; i < 10; i++ )
					{
						children.Element( child =>
						{
							child["age"] = i.ToString();
							child["referenceNumber"] = "ref-" + i;

							child.AppendText( "child & content #" + i );
						} );
					}
				} );
			} );

			actual.Substring( 1 ).ShouldEqual( expected );
		}

		private string WithWriter()
		{
			StringWriter sw = new StringWriterWithEncoding( Encoding.UTF8 );
			XmlTextWriter wr = new XmlTextWriter( sw );

			wr.WriteStartDocument();
			wr.WriteStartElement( "children" );
			wr.WriteComment( "Children below..." );

			for ( int i = 1; i < 10; i++ )
			{
				wr.WriteStartElement( "child" );
				wr.WriteAttributeString( "age", i.ToString() );
				wr.WriteAttributeString( "referenceNumber", "ref-" + i );
				wr.WriteString( "child & content #" + i );
				wr.WriteEndElement();
			}

			wr.WriteEndElement();
			wr.WriteEndDocument();


			wr.Flush();
			wr.Close();

			return sw.ToString();
		}

		[Fact]
		public void Can_append_objects_as_text()
		{
			string s = XmlBuilder.Build( Encoding.ASCII, xml =>
			{
				xml.Indentation = 4;
				xml.Formatting = System.Xml.Formatting.Indented;
				xml.Root( children =>
				{
					children.Element( e =>
					{
						e.AppendText( DateTime.Parse( "Mon 31 May 2011" ) );
					} );
				} );
			} );

    }
			s.ShouldEqual(
@"<?xml version=""1.0"" encoding=""us-ascii""?>
<children>
    <e></e>
</children>" );
		}
	}

        [Fact]
        public void Can_repeat_elements()
        {
            var actual = XmlBuilder.Build(Encoding.UTF8, xml =>
            {
                xml.Formatting = System.Xml.Formatting.None;

                xml.Root(children =>
                {
                    children.Comment("Children below...");

                    children.Element(9, (child,i) =>
                    {
                        child["age"] = i.ToString();
                        child["referenceNumber"] = "ref-" + i;

                        child.AppendText("child & content #" + i);
                    });
                });
            });
            actual.Substring(1).ShouldEqual(WithWriter());
        }
    }
}
