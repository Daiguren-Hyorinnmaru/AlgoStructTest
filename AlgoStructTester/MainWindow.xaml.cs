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
using Tests;
using Tests.Algorithms;
using Tests.Factory;
using Tests.Params;

namespace AlgoStructTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SortTester tester;
        public MainWindow()
        {
            tester = new SortTester();
            InitializeComponent();

        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            SortParams sortParams = new SortParams
            {
                Collections = new List<CollectionType> { CollectionType.List/*, CollectionType.Array*/ },
                DataTypes = new List<DataType> { DataType.String },
                Algorithms = new List<SortsAlgorithms> { SortsAlgorithms.QuickSort },
                LengthStart = 1000,
                LengthEnd = 10000,
                Step = 1000
            };
            DataParams dataParams = new DataParams();
            tester.SortTestRun(sortParams, dataParams);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
