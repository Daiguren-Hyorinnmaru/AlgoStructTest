﻿using AlgoStructTester.Tab;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Tests;
using Tests.Algorithms;
using Tests.Factory;
using Tests.Graph;
using Tests.Params;
using Tests.Result;
using Tests.Testing;

namespace AlgoStructTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SortTester tester;
        PCConfig _PCConfig;
        DynamicTabItem dynamicTabItem;
        SortsControl sortsControl;
        PathfindingControl pathfindingControl;

        public MainWindow()
        {
            // Перевірка наявності рядка підключення
            string connectionName = "TestingSystemEntities";
            var connectionString = ConfigurationManager.ConnectionStrings[connectionName];

            if (connectionString != null)
            {
                Console.WriteLine($"Connection string '{connectionName}' found:");
                Console.WriteLine("Connection String: " + connectionString.ConnectionString);
            }
            else
            {
                Console.WriteLine($"Connection string '{connectionName}' not found.");
            }

            InitializeComponent();

            _PCConfig = new PCConfig();
            sortsControl = new SortsControl(MainTabControl);
            pathfindingControl = new PathfindingControl(MainTabControl);

            ResultManage resultManage = new ResultManage(sortsControl, pathfindingControl);

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

        private void Pathfinding_Click(object sender, RoutedEventArgs e)
        {
            pathfindingControl.Tests();
        }

        private void StartAll(object sender, RoutedEventArgs e)
        {
            pathfindingControl.Tests();
            sortsControl.Tests();
        }
    }
}
