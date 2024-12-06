using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Params;
using Tests;
using Tests.Result;
using Tests.Testing;
using System.Windows.Controls;
using AlgoStructTester.UI;
using System.Windows;
using Tests.Algorithms;

namespace AlgoStructTester.Tab
{
    enum PathfindingUI
    {
        CountIteration
    }
    public class PathfindingControl
    {
        private DynamicTabItem dynamicTabItem;
        private PathfindingParams pathfindingParams;
        private PathfindingTester pathfindingTester;
        public ConcurrentQueue<PathfindingResult> pathfindingResults;

        public PathfindingControl(TabControl tabControl)
        {
            dynamicTabItem = new DynamicTabItem("PathfindingControl");
            tabControl.Items.Add(dynamicTabItem.Tab);
            pathfindingParams = new PathfindingParams();
            pathfindingTester = new PathfindingTester();
            pathfindingResults = new ConcurrentQueue<PathfindingResult>();
            CreateUI();
            TakeParams();
            SetResult();
        }

        public void CreateUI()
        {
            List<(Enum, UIElement)> list = new List<(Enum, UIElement)>();

            foreach (Enum i in Enum.GetValues(typeof(PathfindingAlgotithm)))
                list.Add((i, UIFactory.CreateCheckBox(i)));
            dynamicTabItem.AddColumn(PathfindingAlgotithm.BellmanFord.GetType().Name, list.ToArray());
            list.Clear();

            list.Add((PathfindingUI.CountIteration, 
                UIFactory.CreateTextBox(PathfindingUI.CountIteration.ToString(), "10", typeof(int))));
            dynamicTabItem.AddColumn("Length", list.ToArray());
            list.Clear();
        }

        public void TakeParams()
        {
            pathfindingParams = new PathfindingParams();

            foreach (PathfindingAlgotithm item in Enum.GetValues(typeof(PathfindingAlgotithm)))
            {
                CheckBox temp = (CheckBox)dynamicTabItem.GetUIElement(item.GetType().Name, item);
                if (temp.IsChecked == true)
                {
                    pathfindingParams.Algotithm.Add(item);
                }
            }

            TextBox t = (TextBox)dynamicTabItem.GetUIElement("Length", PathfindingUI.CountIteration);
            pathfindingParams.CountIteration = Convert.ToInt32(t.Text);

            foreach (var item in pathfindingParams.Algotithm)
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine(pathfindingParams.CountIteration);
        }

        public async void Tests()
        {
            TakeParams();
            await pathfindingTester.PathfindingTestRun(pathfindingParams);
        }

        public PathfindingResult TakeResult()
        {
            if (pathfindingResults.TryDequeue(out var result)) return result;
            return null;
        }


        private async void SetResult()
        {
            while (true)
            {
                PathfindingResult t = pathfindingTester.TakeResult();
                if (t != null)
                {
                    pathfindingResults.Enqueue(t);
                    await Task.Delay(3);
                }
                else
                    await Task.Delay(5000);
            }
        }
    }
}
