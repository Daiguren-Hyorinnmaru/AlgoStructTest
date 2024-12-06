using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;

namespace AlgoStructTester.UI
{
    internal static class UIFactory
    {
        public static StackPanel CreateColumn(Grid _grid)
        {
            ScrollViewer CreateScrollViewer(int columnIndex)
            {
                // Створюємо ScrollViewer для прокрутки
                ScrollViewer scrollViewer = new ScrollViewer
                {
                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto, // Автоматично відображати вертикальну смугу прокрутки
                    HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled // Вимкнути горизонтальну смугу прокрутки
                };

                return scrollViewer; // Повертаємо ScrollViewer з вмістом
            }
            StackPanel CreateStackPanel()
            {
                StackPanel stackPanel = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                };

                return stackPanel;
            }

            int newColumnIndex = _grid.ColumnDefinitions.Count; // Індекс нового стовпця (в кінець)

            _grid.ColumnDefinitions.Add(new ColumnDefinition());

            ScrollViewer scrollViewer = CreateScrollViewer(newColumnIndex + 1);
            StackPanel stackPanel = CreateStackPanel();
            scrollViewer.Content = stackPanel;

            Grid.SetColumn(scrollViewer, newColumnIndex);
            _grid.Children.Add(scrollViewer);
            return stackPanel;
        }

        public static Dictionary<Enum, UIElement> AddElementsToColumn(StackPanel panel, params (Enum, UIElement)[] Columns)
        {
            Dictionary<Enum, UIElement> dictionary = new Dictionary<Enum, UIElement>();
            foreach (var column in Columns)
            {
                UIElement uIElement = column.Item2;
                if (column.Item2 is GroupBox groupBox)
                {
                    uIElement = (TextBox)groupBox.Content;
                }
                dictionary.Add(column.Item1, uIElement);
                panel.Children.Add(column.Item2);
            }
            return dictionary;
        }

        // Метод для створення CheckBox
        public static CheckBox CreateCheckBox(object content)
        {
            return new CheckBox
            {
                Content = content,
                IsChecked = true,
            };
        }

        // Метод для створення TextBox з обмеженнями
        public static GroupBox CreateTextBox(string header, string content, Type typeRestriction)
        {
            // Метод для обробки текстового вводу
            static void HandleTextInput(object sender, TextCompositionEventArgs e, Type typeRestriction)
            {
                // Визначаємо регулярний вираз на основі типу обмеження
                Regex regex = typeRestriction == typeof(int) ? new Regex("^[0-9]*$") : new Regex("^[0-9]*(\\.[0-9]*)?$");

                // Дозволяємо введення, якщо регулярний вираз відповідає
                e.Handled = !regex.IsMatch(((TextBox)sender).Text + e.Text);
            }

            // Метод для обробки натискань клавіш
            static void HandleKeyDown(object sender, KeyEventArgs e)
            {
                // Дозволяємо натискання клавіші "Backspace" та "Delete"
                if (e.Key == Key.Back || e.Key == Key.Delete)
                {
                    e.Handled = false;
                }
            }

            TextBox textBox = new TextBox
            {
                Text = content,
                Margin = new Thickness(5)
            };

            GroupBox groupBox = new GroupBox
            {
                Content = textBox,
                Header = header
            };

            // Налаштування обробника подій для обмеження введення
            textBox.PreviewTextInput += (sender, e) => HandleTextInput(sender, e, typeRestriction);
            textBox.PreviewKeyDown += (sender, e) => HandleKeyDown(sender, e);

            return groupBox;
        }
    }
}
