using AlgoStructTester.UI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Tests;
using Tests.Algorithms;
using Tests.Factory;
using Tests.Params;
using Tests.Result;

namespace AlgoStructTester.Tab
{
    enum sortUI
    {
        LengthStart,
        LengthEnd,
        Step
    }

    public class SortsControl
    {
        private DynamicTabItem dynamicTabItem;
        private SortParams sortParams;
        private SortTester sortTester;
        private ConcurrentQueue<SortResult> sortResults;

        public SortsControl(TabControl tabControl)
        {
            dynamicTabItem = new DynamicTabItem("SortsControl");
            tabControl.Items.Add(dynamicTabItem.Tab);
            sortParams = new SortParams();
            sortTester = new SortTester();
            sortResults = new ConcurrentQueue<SortResult>();
            CreateUI();
            TakeResult();
        }

        public void CreateUI()
        {
            List<(Enum, UIElement)> list = new List<(Enum, UIElement)>();

            foreach (Enum i in Enum.GetValues(typeof(SortsAlgorithms)))
                list.Add((i, UIFactory.CreateCheckBox(i)));
            dynamicTabItem.AddColumn(SortsAlgorithms.QuickSort.GetType().Name, list.ToArray());
            list.Clear();

            foreach (Enum i in Enum.GetValues(typeof(CollectionType)))
                list.Add((i, UIFactory.CreateCheckBox(i)));
            dynamicTabItem.AddColumn(CollectionType.Array.GetType().Name, list.ToArray());
            list.Clear();

            foreach (Enum i in Enum.GetValues(typeof(DataType)))
                list.Add((i, UIFactory.CreateCheckBox(i)));
            dynamicTabItem.AddColumn(DataType.Double.GetType().Name, list.ToArray());
            list.Clear();

            list.Add((sortUI.LengthStart, UIFactory.CreateTextBox(sortUI.LengthStart.ToString(), "10", typeof(int))));
            list.Add((sortUI.LengthEnd, UIFactory.CreateTextBox(sortUI.LengthEnd.ToString(), "100", typeof(int))));
            list.Add((sortUI.Step, UIFactory.CreateTextBox(sortUI.Step.ToString(), "10", typeof(int))));
            dynamicTabItem.AddColumn("Length", list.ToArray());
            list.Clear();
        }
        public void TakeParams()
        {
            sortParams = new SortParams();

            foreach (SortsAlgorithms item in Enum.GetValues(typeof(SortsAlgorithms)))
            {
                CheckBox temp = (CheckBox)dynamicTabItem.GetUIElement(item.GetType().Name, item);
                if (temp.IsChecked == true)
                {
                    sortParams.Algorithms.Add(item);
                }
            }

            foreach (CollectionType item in Enum.GetValues(typeof(CollectionType)))
            {
                CheckBox temp = (CheckBox)dynamicTabItem.GetUIElement(item.GetType().Name, item);
                if (temp.IsChecked == true)
                {
                    sortParams.Collections.Add(item);
                }
            }

            foreach (DataType item in Enum.GetValues(typeof(DataType)))
            {
                CheckBox temp = (CheckBox)dynamicTabItem.GetUIElement(item.GetType().Name, item);
                if (temp.IsChecked == true)
                {
                    sortParams.DataTypes.Add(item);
                }
            }

            TextBox t = (TextBox)dynamicTabItem.GetUIElement("Length", sortUI.LengthStart);
            sortParams.LengthStart = Convert.ToInt32(t.Text);

            t = (TextBox)dynamicTabItem.GetUIElement("Length", sortUI.LengthEnd);
            sortParams.LengthEnd = Convert.ToInt32(t.Text);

            t = (TextBox)dynamicTabItem.GetUIElement("Length", sortUI.Step);
            sortParams.Step = Convert.ToInt32(t.Text);
        }

        public void Tests()
        {
            TakeParams();
            sortTester.SortTestRun(sortParams, new DataParams());
        }
        private async void TakeResult()
        {
            while (true)
            {
                SortResult t = sortTester.TakeResult();
                if (t != null)
                {
                    sortResults.Enqueue(t);
                    await Task.Delay(3);
                }
                else
                    await Task.Delay(5000);
            }
        }
    }
}