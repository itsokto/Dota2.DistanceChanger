#region

using System.Windows;
using System.Windows.Controls;

#endregion

namespace FlatUI.Controls
{
    /// <summary>
    ///     Выполните шаги 1a или 1b, а затем 2, чтобы использовать этот пользовательский элемент управления в файле XAML.
    ///     Шаг 1a. Использование пользовательского элемента управления в файле XAML, существующем в текущем проекте.
    ///     Добавьте атрибут XmlNamespace в корневой элемент файла разметки, где он
    ///     будет использоваться:
    ///     xmlns:MyNamespace="clr-namespace:FlatUI.Controls"
    ///     Шаг 1б. Использование пользовательского элемента управления в файле XAML, существующем в другом проекте.
    ///     Добавьте атрибут XmlNamespace в корневой элемент файла разметки, где он
    ///     будет использоваться:
    ///     xmlns:MyNamespace="clr-namespace:FlatUI.Controls;assembly=FlatUI.Controls"
    ///     Потребуется также добавить ссылку из проекта, в котором находится файл XAML,
    ///     на данный проект и пересобрать во избежание ошибок компиляции:
    ///     Щелкните правой кнопкой мыши нужный проект в обозревателе решений и выберите
    ///     "Добавить ссылку"->"Проекты"->[Поиск и выбор проекта]
    ///     Шаг 2)
    ///     Теперь можно использовать элемент управления в файле XAML.
    ///     <MyNamespace:FlatTextBox />
    /// </summary>
    public class FlatTextBox : TextBox
    {
        // Using a DependencyProperty as the backing store for Hint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HintProperty =
            DependencyProperty.Register("Hint", typeof(string), typeof(FlatTextBox), new FrameworkPropertyMetadata("",
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        static FlatTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlatTextBox),
                new FrameworkPropertyMetadata(typeof(FlatTextBox)));
        }

        public string Hint
        {
            get => (string) GetValue(HintProperty);
            set => SetValue(HintProperty, value);
        }
    }
}