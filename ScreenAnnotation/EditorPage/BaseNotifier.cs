using System.ComponentModel;

namespace ScreenAnnotation.EditorPage
{
    public class BaseNotifier : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
