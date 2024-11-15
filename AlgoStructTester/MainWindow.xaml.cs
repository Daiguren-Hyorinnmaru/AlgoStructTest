using AlgoStructTester.Tab;
using DataBase;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Tests;
using Tests.Algorithms;
using Tests.Factory;
using Tests.Params;
using Tests.Result;

namespace AlgoStructTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TestingSystemEntities appDbContext;
        SortTester tester;
        PCConfig _PCConfig;
        DynamicTabItem dynamicTabItem;
        SortsControl sortsControl;

        public MainWindow()
        {
            InitializeComponent();

            appDbContext = new TestingSystemEntities();

            _PCConfig = new PCConfig();
            sortsControl = new SortsControl(MainTabControl);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SwithTheme(object sender, RoutedEventArgs e)
        {
            // Перевіряємо, яка тема наразі активна, та перемикаємось на іншу
            var currentTheme = Application.Current.Resources.MergedDictionaries.Count > 0
                ? Application.Current.Resources.MergedDictionaries[0].Source.ToString()
                : string.Empty;

            ResourceDictionary newTheme;

            if (currentTheme.Contains("style/DarkTheme.xaml"))
            {
                // Якщо поточна тема темна, перемикаємось на світлу
                newTheme = new ResourceDictionary
                {
                    Source = new Uri("style/WhiteTheme.xaml", UriKind.Relative)
                };
            }
            else
            {
                // Якщо поточна тема світла або жодна не активна, перемикаємось на темну
                newTheme = new ResourceDictionary
                {
                    Source = new Uri("style/DarkTheme.xaml", UriKind.Relative)
                };
            }

            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(newTheme);
        }

        private void Sort_Click(object sender, RoutedEventArgs e)
        {
            sortsControl.Tests();
        }
    }
}
