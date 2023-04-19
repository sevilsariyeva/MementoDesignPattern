using Screenshot_Memento_.ViewModels;
using Screenshot_Memento_.Views.UserControls;
using System;
using System.Collections.Generic;
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
using static System.Net.Mime.MediaTypeNames;
using Application = System.Windows.Application;
using Path = System.IO.Path;

namespace Screenshot_Memento_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var viewmodel = new MainViewModel();
            App.MyGrid = myGrid;
            var homeUCViewModel = new HomeUCViewModel();
            var homeUC = new HomeUC();
            homeUC.DataContext = homeUCViewModel;

            App.MyGrid.Children.Add(homeUC);
            this.DataContext = viewmodel;
        }
    }
}
