using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Reservation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            //We need to read meeting room data from an XML file "meeting room data.xml"
            string[] rooms = new string[4];
            string file = Process.GetCurrentProcess().MainModule.FileName;

            //Because file includes also the name of the executable, we need to cut it out
            char[] del = new char[] { 'R', 'e', 's', 'e', 'r', 'v', 'a', 't', 'i', 'o', 'n', '.', 'e', 'x', 'e' };
            file = file.TrimEnd(del);

            //Then we catenate the name "meeting room data.xml" at the end of path
            //and open file and read its content
            string data = File.ReadAllText(file + "meeting room data.xml");

            //Let's check the xml data
            var doc = XElement.Parse(data, LoadOptions.None);
            tulos = from nimet in doc.Elements("meeting_rooms")
                    from items in nimet.Elements("item")
                    select items;
        }
    }
}
