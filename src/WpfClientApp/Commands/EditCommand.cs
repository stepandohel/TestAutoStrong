using System.Windows.Input;

namespace WpfClientApp
{
    public class EditCommand
    {
        public EditCommand()
        {
            Edit = new RoutedCommand("Edit", typeof(MainWindow));
        }
        public static RoutedCommand Edit { get; set; } = new RoutedCommand("Edit", typeof(MainWindow));

    }
}
