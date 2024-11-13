using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Input;
using AlgoStructTester.UI;

namespace AlgoStructTester
{
    public class DynamicTabItem
    {
        public TabItem Tab { get; private set; }
        private Grid _grid; // Зберігаємо посилання на Grid для подальшого використання
        public Dictionary<string, StackPanel> _columns;
        public Dictionary<StackPanel, Dictionary<Enum, UIElement>> _panels;

        // Конструктор приймає заголовок і кількість стовпців у таблиці
        public DynamicTabItem(string headerText)
        {
            // Створюємо нову вкладку
            Tab = new TabItem
            {
                Header = headerText
            };

            // Створюємо таблицю (Grid)
            _grid = new Grid();
            _columns = new Dictionary<string, StackPanel>();
            _panels = new Dictionary<StackPanel, Dictionary<Enum, UIElement>>();
            // Додаємо таблицю як вміст вкладки
            Tab.Content = _grid;
        }

        public void AddColumn(string Header, params (Enum, UIElement)[] Columns)
        {
            _columns.Add(Header, UIFactory.CreateColumn(_grid));
            _panels.Add(_columns[Header], UIFactory.AddElementsToColumn(_columns[Header], Columns));
        }

        public UIElement GetUIElement(string Header, Enum _enum)
        {
            return _panels[_columns[Header]][_enum];
        }
    }
}
