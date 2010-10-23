using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace SergioPereira.Xml
{
	class Program
	{
		static void Main(string[] args)
		{
			string s = XmlBuilder.Build(Encoding.ASCII, xml =>
			{
				xml.Indentation = 4;
				xml.Formatting = System.Xml.Formatting.Indented;

				xml.Root(children =>
				{
					children.Comment("Children below...");

					for(int i = 1; i < 10; i++)
					{
						children.Element(child =>
						{
							child["age"] = i.ToString();
							child["referenceNumber"] = "ref-" + i;

							child.AppendText("child & content #" + i);
						});
					}
				});
			});

			XmlBuilder.Build("file.xml", Encoding.UTF8, xml =>
			{
				xml.Indentation = 4;
				xml.Formatting = System.Xml.Formatting.Indented;

				xml.Root(children =>
				{
					children.Comment("Children below...");

					for(int i = 1; i < 10; i++)
					{
						children.Element(child =>
						{
							child["age"] = i.ToString();
							child["referenceNumber"] = "ref-" + i;

							child.AppendText("child & content #" + i);
						});
					}
				});

			});

			Console.WriteLine(s);
			WithDOM();
			WithWriter();
			Console.ReadLine();
		}

		static void WithDOM()
		{
			XmlDocument xml = new XmlDocument();
			XmlElement root = xml.CreateElement("children");
			xml.AppendChild(root);

			XmlComment comment = xml.CreateComment("Children below...");
			root.AppendChild(comment);

			for(int i = 1; i < 10; i++)
			{
				XmlElement child = xml.CreateElement("child");
				child.SetAttribute("age", i.ToString());
				child.SetAttribute("referenceNumber", "ref-" + i);
				child.InnerText = "child & content #" + i;
				root.AppendChild(child);
			}

			Console.WriteLine(xml.OuterXml);

		}

		static void WithWriter()
		{
			StringWriter sw = new StringWriter();
			XmlTextWriter wr = new XmlTextWriter(sw);

			wr.WriteStartDocument();
			wr.WriteComment("Children below...");
			wr.WriteStartElement("children");

			for(int i = 1; i < 10; i++)
			{
				wr.WriteStartElement("child");
				wr.WriteAttributeString("age", i.ToString());
				wr.WriteAttributeString("referenceNumber", "ref-" + i);
				wr.WriteString("child & content #" + i);
				wr.WriteEndElement();
			}

			wr.WriteEndElement();
			wr.WriteEndDocument();


			wr.Flush();
			wr.Close();

			Console.WriteLine(sw.ToString());
		}
	}
}
