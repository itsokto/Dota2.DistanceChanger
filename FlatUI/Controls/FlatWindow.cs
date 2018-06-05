#region

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#endregion

namespace FlatUI.Controls
{
    /// <summary>
    ///     Выполните шаги 1a или 1b, а затем 2, чтобы использовать этот настраиваемый элемент управления в файле XAML.
    ///     Шаг 1a. Использование настраиваемого элемента управления в файле XAML, существующем в текущем проекте.
    ///     Добавьте атрибут XmlNamespace к корневому элементу файла разметки, где он
    ///     должен использоваться:
    ///     xmlns:MyNamespace="clr-namespace:FlatUI"
    ///     Шаг 1b. Использование этого настраиваемого элемента управления в файле XAML, существующем в текущем проекте.
    ///     Добавьте атрибут XmlNamespace к корневому элементу файла разметки, где он
    ///     должен использоваться:
    ///     xmlns:MyNamespace="clr-namespace:FlatUI;assembly=FlatUI"
    ///     Потребуется также добавить ссылку на проект из проекта, в котором находится файл XAML
    ///     в данный проект и пересобрать во избежание ошибок компиляции:
    ///     Правой кнопкой мыши щелкните проект в обозревателе решений и выберите команду
    ///     "Добавить ссылку"->"Проекты"->[Выберите этот проект]
    ///     Шаг 2)
    ///     Продолжайте дальше и используйте элемент управления в файле XAML.
    ///     <MyNamespace:CustomControl1 />
    /// </summary>
    public class FlatWindow : Window
    {
        static FlatWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlatWindow),
                new FrameworkPropertyMetadata(typeof(FlatWindow)));
        }


        #region Click events

        protected void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion


        public override void OnApplyTemplate()
        {
            if (GetTemplateChild("closeButton") is Button closeButton)
                closeButton.Click += CloseClick;

            if (GetTemplateChild("header") is UIElement element)
                element.PreviewMouseDown += moveRectangle_PreviewMouseDown;

            base.OnApplyTemplate();
        }

        private void moveRectangle_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}