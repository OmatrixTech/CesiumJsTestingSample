using CefSharp;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Xml;
using System.Xml.Linq;


namespace WPF.CesiumJs.Samples
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields declaration
        private string kmlString; private string kmlString2; private string kmlString3; 
        List<string> dynamicKmlStrings = new List<string>();
        ObservableCollection<KMLItems> kmlItemsObjColl = null;
        #endregion

        #region KML
        private void SetKML(string kmlFileName)
        {
            // Create a FlowDocument to hold the formatted KML with colored text.
            FlowDocument flowDocument = new FlowDocument();
            // Load the KML file as an XML document
            XmlDocument kmlDocument = new XmlDocument();
            if (!string.IsNullOrEmpty(kmlFileName))
            {
                if ("FirstLocationPoints.kml" == kmlFileName)
                {
                    kmlDocument.Load("FirstLocationPoints.kml");
                }
                if ("SecondLocationPoints.kml" == kmlFileName)
                {
                    kmlDocument.Load("SecondLocationPoints.kml");
                }
                if ("ThirdLocationPoints.kml" == kmlFileName)
                {
                    kmlDocument.Load("ThirdLocationPoints.kml");
                }
                FormatKmlXml(kmlDocument.DocumentElement, flowDocument.Blocks);
                kmlRichTextBox.Document = flowDocument;
            }

        }
        private void FormatKmlXml(XmlNode node, BlockCollection blocks)
        {
            if (node.NodeType == XmlNodeType.Element)
            {
                // Create a Paragraph for the element name with a different color.
                Paragraph elementNameParagraph = new Paragraph();

                // Create a Run for the element name and set its foreground color.
                Run elementNameRun = new Run("<" + node.Name.ToUpper().Trim() + ">")
                {
                    Foreground = System.Windows.Media.Brushes.Blue
                };

                // Add the Run to the Paragraph.
                elementNameParagraph.Inlines.Add(elementNameRun);

                // Add the Paragraph to the blocks collection.
                blocks.Add(elementNameParagraph);

                // Process child nodes recursively.
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    FormatKmlXml(childNode, blocks);
                }

                // Create a Paragraph for the closing tag with a different color.
                Paragraph closingTagParagraph = new Paragraph();

                // Create a Run for the closing tag and set its foreground color.
                Run closingTagRun = new Run("</" + node.Name.ToUpper().Trim() + ">")
                {
                    Foreground = System.Windows.Media.Brushes.Blue
                };

                // Add the Run to the Paragraph.
                closingTagParagraph.Inlines.Add(closingTagRun);

                // Add the Paragraph to the blocks collection.
                blocks.Add(closingTagParagraph);
            }
            else if (node.NodeType == XmlNodeType.Text)
            {
                // Create a Paragraph for the text content with a different color.
                Paragraph textParagraph = new Paragraph();
                // Create a Run for the text content and set its foreground color.
                Run textRun = new Run(node.Value.ToLower().Trim())
                {
                    Foreground = System.Windows.Media.Brushes.Black
                };
                // Add the Run to the Paragraph.
                textParagraph.Inlines.Add(textRun);
                // Add the Paragraph to the blocks collection.
                blocks.Add(textParagraph);
            }
        }


        #endregion
        public MainWindow()
        {
            InitializeComponent();
            kmlItemsObjColl = new ObservableCollection<KMLItems>();
            string htmlFileContent = File.ReadAllText("CesiumJsMapIntegration.html");
            ChromiumWebBrowser.LoadHtml(htmlFileContent);
            kmlString = File.ReadAllText("FirstLocationPoints.kml");
            kmlString2 = File.ReadAllText("SecondLocationPoints.kml");
            kmlString3 = File.ReadAllText("ThirdLocationPoints.kml");
            kmlItemsObjColl.Clear();
            kmlItemsObjColl.Add(new KMLItems
            {
                Key = "#KMLDOC1",
                KmlDoc = kmlString
            });
            kmlItemsObjColl.Add(new KMLItems
            {
                Key = "#KMLDOC2",
                KmlDoc = kmlString2
            });
            kmlItemsObjColl.Add(new KMLItems
            {
                Key = "#KMLDOC3",
                KmlDoc = kmlString3
            });
        }
        private void RemoveKML1_Click(object sender, RoutedEventArgs e)
        {
            string kmlDockey = "#KMLDOC1";
            string jsFunctionCall = $"UnloadKML('{kmlDockey}')";
            ChromiumWebBrowser.ExecuteScriptAsync(jsFunctionCall);
        }

        private void RemoveKML2_Click(object sender, RoutedEventArgs e)
        {
            string kmlDockey = "#KMLDOC2";
            string jsFunctionCall = $"UnloadKML('{kmlDockey}')";
            ChromiumWebBrowser.ExecuteScriptAsync(jsFunctionCall);
        }

        private void RemoveKML3_Click(object sender, RoutedEventArgs e)
        {
            string kmlDockey = "#KMLDOC3";
            string jsFunctionCall = $"UnloadKML('{kmlDockey}')";
            ChromiumWebBrowser.ExecuteScriptAsync(jsFunctionCall);
        }

        private void LoadKML1_Click(object sender, RoutedEventArgs e)
        {
            SetKML("FirstLocationPoints.kml");
            kmlString = File.ReadAllText("FirstLocationPoints.kml");
            string docId = "#KMLDOC1";
            string escapedKmlData = EscapeForJavaScript(kmlString);
            string script = $"ReceiveKMLData('{escapedKmlData}','{docId}');";
            ChromiumWebBrowser.ExecuteScriptAsync(script);
        }

        private void LoadKML2_Click(object sender, RoutedEventArgs e)
        {
            SetKML("SecondLocationPoints.kml");
            kmlString2 = File.ReadAllText("SecondLocationPoints.kml");
            string docId = "#KMLDOC2";
            string escapedKmlData = EscapeForJavaScript(kmlString2);
            string script = $"ReceiveKMLData('{escapedKmlData}','{docId}');";
            ChromiumWebBrowser.ExecuteScriptAsync(script);
        }

        private void LoadKML3_Click(object sender, RoutedEventArgs e)
        {
            SetKML("ThirdLocationPoints.kml");
            kmlString3 = File.ReadAllText("ThirdLocationPoints.kml");
            string docId = "#KMLDOC3";
            string escapedKmlData = EscapeForJavaScript(kmlString3);
            string script = $"ReceiveKMLData('{escapedKmlData}','{docId}');";
            ChromiumWebBrowser.ExecuteScriptAsync(script);
        }

        private void UnLoadAllKMLDoc_Click(object sender, RoutedEventArgs e)
        {
            string jsFunctionCall = "RemoveAllDatasource()";
            ChromiumWebBrowser.ExecuteScriptAsync(jsFunctionCall);
        }

        private void Location1_Click(object sender, RoutedEventArgs e)
        {
            string jsFunctionCall = "AddDynamicLocationPoint('Home', 77.117358, 28.657817)";
            ChromiumWebBrowser.ExecuteScriptAsync(jsFunctionCall);
        }

        private void Location2_Click(object sender, RoutedEventArgs e)
        {
            string jsFunctionCall = "AddDynamicLocationBillBoard('Office',77.389342, 28.626617,1)";
            ChromiumWebBrowser.ExecuteScriptAsync(jsFunctionCall);
        }

        private void Location3_Click(object sender, RoutedEventArgs e)
        {
            string jsFunctionCall = "AddDynamicLocationBillBoard('China', 115.683111, 41.494847,2)";
            ChromiumWebBrowser.ExecuteScriptAsync(jsFunctionCall);
        }

        private void Location4_Click(object sender, RoutedEventArgs e)
        {
            string jsFunctionCall = "AddDynamicLocationBillBoard('Russia', 84.485961, 53.307267,3)";
            ChromiumWebBrowser.ExecuteScriptAsync(jsFunctionCall);
        }

        private void Location5_Click(object sender, RoutedEventArgs e)
        {
            string jsFunctionCall = "AddDynamicLocationPoint('Africa', 60.242958,21.165911)";
            ChromiumWebBrowser.ExecuteScriptAsync(jsFunctionCall);
        }
        private void Line_1_5_Click(object sender, RoutedEventArgs e)
        {
            string jsFunctionCall = "AddLinesBetweenTwoLocation( 77.117358, 28.657817,60.242958,21.165911,0)";
            ChromiumWebBrowser.ExecuteScriptAsync(jsFunctionCall);
        }

        private void Line_2_3_Click(object sender, RoutedEventArgs e)
        {
            string jsFunctionCall = "AddLinesBetweenTwoLocation(77.389342, 28.626617,115.683111, 41.494847,1)";
            ChromiumWebBrowser.ExecuteScriptAsync(jsFunctionCall);
        }

        private void Line_1_4_Click(object sender, RoutedEventArgs e)
        {
            string jsFunctionCall = "AddLinesBetweenTwoLocation(77.117358, 28.657817,84.485961, 53.307267,2)";
            ChromiumWebBrowser.ExecuteScriptAsync(jsFunctionCall);
        }

        private void Line_1_2_Click(object sender, RoutedEventArgs e)
        {
            string jsFunctionCall = "AddLinesBetweenTwoLocation(77.117358, 28.657817,77.389342, 28.626617,3)";
            ChromiumWebBrowser.ExecuteScriptAsync(jsFunctionCall);
        }

        private void ControlCamera_Click(object sender, RoutedEventArgs e)
        {
            string jsFunctionCall = "HandleCameraPositionInMap( 115.683111, 41.494847,15000, 20.0,-35.0,0.0)";
            ChromiumWebBrowser.ExecuteScriptAsync(jsFunctionCall);
        }

        private void LoadAllKMLDoc_Click(object sender, RoutedEventArgs e)
        {
            foreach (KMLItems kMLItem in kmlItemsObjColl)
            {
                string docId = (string)kMLItem.Key;
                string escapedKmlData = EscapeForJavaScript(kMLItem.KmlDoc);
                string script = $"ReceiveKMLData('{escapedKmlData}','{docId}');";
                ChromiumWebBrowser.ExecuteScriptAsync(script);
            }
        }

        private string EscapeForJavaScript(string input)
        {
            // Replace characters that need escaping with their JavaScript escape sequences
            string escaped = input.Replace("\\", "\\\\")
                                 .Replace("'", "\\'")
                                 .Replace("\"", "\\\"")
                                 .Replace("\r", "\\r")
                                 .Replace("\n", "\\n")
                                 .Replace("\t", "\\t");

            return escaped;
        }

        private void TopToolBar_Click(object sender, RoutedEventArgs e)
        {
            string jsFunctionCall = "toggleCesiumTopToolBar()";
            ChromiumWebBrowser.ExecuteScriptAsync(jsFunctionCall);
        }

        private void FullScreen_Click(object sender, RoutedEventArgs e)
        {
            string jsFunctionCall = "toggleFullscreenContainer()";
            ChromiumWebBrowser.ExecuteScriptAsync(jsFunctionCall);
        }

    }
}
