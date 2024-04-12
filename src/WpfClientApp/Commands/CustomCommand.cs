using System.Windows.Input;

namespace WpfClientApp
{
    public class CustomCommand
    {
        public static RoutedCommand Edit { get; set; } = new RoutedCommand("Edit", typeof(MainWindow));
        public static RoutedCommand Delete { get; set;} = new RoutedCommand("Delete", typeof(MainWindow));

    }
}
