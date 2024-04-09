using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfClientApp.Modeles
{
    public class ItemVM : INotifyPropertyChanged
    {
        public string Text { get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }

        private ImageSource _bitmapImage { get; set; }
        public ImageSource BitmapImage
        {
            get
            {
                return _bitmapImage;
            }
            set
            {
                _bitmapImage = value;
                OnPropertyChanged("BitmapImage");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
