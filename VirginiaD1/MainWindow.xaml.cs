using System;
using System.Collections.Generic;
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
using CleansingsD1;

namespace VirginiaD1
{


    public partial class MainWindow : Window
    {


        public MainWindow()
        {


            InitializeComponent();


        }


        void restore(object sender, RoutedEventArgs e)
        {


            lazlow(necro.lazarus());


        }


        void delette(object sender, RoutedEventArgs e)
        {


            execute();


        }


        public void execute()
        {


            contentgrid.Children.Clear();


            ScrollViewer SierraVictor = new ScrollViewer()
            {


                Width = contentgrid.Width,
                VerticalAlignment = VerticalAlignment.Stretch


            };


            contentgrid.Children.Add(SierraVictor);


            Grid packsetcontainer = new Grid()
            {


                Width = SierraVictor.Width,
                Height = SierraVictor.Height,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                

            };


            List<packinfo> AllPackages = new List<packinfo>();

            int i = 0;

            AllPackages = prepforpurge.EfXML(prepforpurge.EfPMfU());


            foreach(packinfo package in AllPackages)
            {


                if(package.SafeToPurge == true && (!package.DisplayName.Contains("Framework") && !package.DisplayName.Contains("Runtime")))
                {


                    Grid packset = new Grid()
                    {


                        Width = 500,
                        Height = 180,
                        Background = new SolidColorBrush(Color.FromRgb(35,34,43)),
                        Margin = new Thickness(20, i * 200 + 20, 0, 50),
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Left,


                    };


                    TextBlock TangoBravo = new TextBlock()
                    {


                        Text = package.DisplayName,
                        Margin = new Thickness(160, 10, 0, 0),
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        FontSize = 18,
                        FontFamily = new FontFamily("Microsoft YaHei UI Light")


                    };


                    packset.Children.Add(TangoBravo);

                    BitmapImage BravoIndia = new BitmapImage();
                    BravoIndia.BeginInit();
                    BravoIndia.UriSource = new Uri(package.LogoPath, UriKind.Absolute);
                    BravoIndia.EndInit();


                    Image India = new Image()
                    {


                        Source = BravoIndia,
                        Width = 150,
                        Height = 150,
                        Margin = new Thickness(15, 10, 10, 10),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center


                    };


                    packset.Children.Add(India);


                    Button Bravo = new Button()
                    {


                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Bottom,
                        Width = 150,
                        Height = 50,
                        Margin = new Thickness(160, 10, 0, 10),
                        Content = "Delete Package",
                        Background = new SolidColorBrush(Color.FromRgb(124, 122, 122)),
                        FontSize = 18,
                        FontFamily = new FontFamily("Microsoft YaHei UI Light")


                    };
                    Bravo.Click += delegate { necro.bury(package); execute(); };


                    packset.Children.Add(Bravo);

                    //Button exe = new Button() { Content = "DILID DIS", Width = 100, Height = 50 };
                    //exe.Click += delegate { necro.bury(package); execute(); };

                    //packset.Children.Add(exe);

                    packsetcontainer.Children.Add(packset);

                    i++;


                }


            }


            SierraVictor.Content = packsetcontainer;


        }


        public void lazlow(List<packinfo> AllPackages)
        {


            contentgrid.Children.Clear();

            int i = 0;


            if(AllPackages.Count() > 0)
            {


                ScrollViewer SierraVictor = new ScrollViewer()
                {


                    Width = contentgrid.Width,
                    VerticalAlignment = VerticalAlignment.Stretch


                };


                contentgrid.Children.Add(SierraVictor);


                Grid packsetcontainer = new Grid()
                {


                    Width = SierraVictor.Width,
                    Height = SierraVictor.Height,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,


                };


                foreach (packinfo package in AllPackages)
                {


                    Grid packset = new Grid()
                    {


                        Width = 500,
                        Height = 100,
                        Background = new SolidColorBrush(Color.FromRgb(35, 34, 43)),
                        Margin = new Thickness(20, i * 120 + 20, 0, 50),
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Left,


                    };


                    packsetcontainer.Children.Add(packset);


                    TextBlock TangoBravo = new TextBlock()
                    {


                        Text = package.DisplayName,
                        Margin = new Thickness(10, 0, 0, 0),
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        FontSize = 18,
                        FontFamily = new FontFamily("Microsoft YaHei UI Light")


                    };


                    packset.Children.Add(TangoBravo);


                    Button Bravo = new Button()
                    {


                        HorizontalAlignment = HorizontalAlignment.Right,
                        VerticalAlignment = VerticalAlignment.Center,
                        Width = 150,
                        Height = 50,
                        Margin = new Thickness(0, 0, 10, 0),
                        Content = "Restore Package",
                        FontSize = 18,
                        Background = new SolidColorBrush(Color.FromRgb(124,122,122)),
                        FontFamily = new FontFamily("Microsoft YaHei UI Light")


                    };
                    Bravo.Click += delegate { necro.finalLazarus(package.FullName, package.PackageFamilyName); lazlow(necro.lazarus()); };


                    packset.Children.Add(Bravo);


                    i++;


                }


                SierraVictor.Content = packsetcontainer;


            }
            else
            {


                contentgrid.Children.Add(new TextBlock() { Text = "asasasasasasasasasasasasasasasasassasasassa" });


            }


        }


    }


}
