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

        private Database mDatabase = new Database();
        private int mRow1, mRow2, mColumn;
        private bool mAvailable;
        private List<DockPanel> mRows = new List<DockPanel>();

        public MainWindow()
        {
            InitializeComponent();
            string[] rooms = new string[4];
            string[] seats = new string[4];
            string[] addresses = new string[4];

            //We need to read meeting room data from an XML file "meeting room data.xml"
            string file = Process.GetCurrentProcess().MainModule.FileName;

            //Because file includes also the name of the executable, we need to cut it out
            string del = "Reservation.exe";
            file = file.TrimEnd(del.ToCharArray());

            //Then we catenate the name "meeting room data.xml" at the end of path
            //and open file and read its content
            string data = File.ReadAllText(file + "meeting room data.xml");

            //Let's check the xml data. Room names:
            var doc = XElement.Parse(data, LoadOptions.None);
            var result = from item in doc.Elements("meeting_room")
                        select item.Attribute("name");
            int j = 0;
            foreach (var name in result)
            {
                rooms[j] = name.Value;
                j++;
            }

            //Numbers of seats in rooms:
            result = from item in doc.Elements("meeting_room")
                         select item.Attribute("seats");
            j = 0;
            foreach (var name in result)
            {
                seats[j] = name.Value;
                j++;
            }

            //Addresses of rooms:
            result = from item in doc.Elements("meeting_room")
                         select item.Attribute("location");
            j = 0;
            foreach (var name in result)
            {
                addresses[j] = name.Value;
                j++;
            }

            //Fill the names, sizes and addresses of rooms to grid
            TextBlock txt = new TextBlock();
            txt.Text = rooms[0] + "\n" + seats[0] + "\n" + addresses[0];
            grid.Children.Add(txt);

            txt = new TextBlock();
            txt.Text = rooms[1] + "\n" + seats[1] + "\n" + addresses[1];
            Grid.SetRow(txt, 0);
            Grid.SetColumn(txt, 1);
            grid.Children.Add(txt);

            txt = new TextBlock();
            txt.Text = rooms[2] + "\n" + seats[2] + "\n" + addresses[2];
            Grid.SetRow(txt, 0);
            Grid.SetColumn(txt, 2);
            grid.Children.Add(txt);

            txt = new TextBlock();
            txt.Text = rooms[3] + "\n" + seats[3] + "\n" + addresses[3];
            Grid.SetRow(txt, 0);
            Grid.SetColumn(txt, 3);
            grid.Children.Add(txt);

            //Let's fill a green color to all the other cells
            for (int ii=1; ii<25; ii++)
            {
                for (int jj=0; jj<4; jj++)
                {

                    //A cell needs a panel which can be colored
                    DockPanel panel = new DockPanel();
                    panel.Background = Brushes.Green;
                    panel.MouseDown += new MouseButtonEventHandler(PanelMouseDown);
                    panel.MouseMove += new MouseEventHandler(PanelMouseMove);
                    panel.MouseUp += new MouseButtonEventHandler(PanelMouseUp);
                    Grid.SetRow(panel, ii);
                    Grid.SetColumn(panel, jj);
                    grid.Children.Add(panel);

                    //A panel needs a time stamp
                    txt = new TextBlock();
                    int start = ii - 1;
                    int stop = ii;
                    txt.Text = start.ToString() + ".00 - " + stop.ToString() + ".00";
                    panel.Children.Add(txt);
                }
            }
        }

        //This method starts handling room reservation made by a mouse
        private void PanelMouseDown(Object sender, MouseButtonEventArgs args)
        {

            if (sender != null)
            {

                //We need to check if the room is available at the selected time
                mRow1= mRow2 = Grid.GetRow((UIElement)sender);
                mColumn = Grid.GetColumn((UIElement)sender);
                if (mDatabase.IsFree(ref mRow1, ref mColumn))
                {

                    //Room is available.
                    mAvailable = true;
                    DockPanel panel = sender as DockPanel;
                    panel.Background = Brushes.Yellow;
                    mRows.Add(panel);
                }
                else
                {
                    mAvailable = false;
                }
            }
        }

        //This method continues multiple hour reservation.
        private void PanelMouseMove(Object sender, MouseEventArgs args)
        {
            int row = Grid.GetRow((UIElement)sender);
            int column = Grid.GetColumn((UIElement)sender);
            if (sender != null && args.LeftButton == MouseButtonState.Pressed && row != mRow2)
            {
                
                //We need to check if the room is available at the selected time
                if (mDatabase.IsFree(ref row, ref mColumn))
                {
                    mRow2 = row;
                    DockPanel panel = sender as DockPanel;
                    panel.Background = Brushes.Yellow;
                    mRows.Add(panel);
                }
                else
                {
                    mAvailable = false;
                }
            }
        }

        //This method completes the room reservation.
        private void PanelMouseUp(Object sender, MouseButtonEventArgs args)
        {
            if (sender!=null)
            {
                if (mAvailable)
                {

                    //Room is available. Let's ask the name of subscriber:
                    Input input = new Input();
                    if (input.ShowDialog() == true)
                    {

                        //If user gives his/her name, the application makes reservation
                        string name = input.Answer;
                        mDatabase.Reserve(name, ref mRow1, ref mRow2, ref mColumn);

                        //All the reserved hours must be red colored.
                        for (int i=0; i<mRows.Count; i++)
                        {
                            mRows[i].Background = Brushes.Red;
                        }
                        mRows.Clear();
                    }
                }
            }
        }
    }
}
