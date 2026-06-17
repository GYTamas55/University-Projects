using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GyorsulasEVA_AVA.ViewModels
{
    //Ősosztály -> INotifyPropertyChanged
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        //Ezt hívom ha változást akarok jelezni a view-nak
        public event PropertyChangedEventHandler? PropertyChanged;

        public ViewModelBase() { }
        //CallerMemberName automatikusan tudja mit kell frissiteni az OnPr.Ch. miatt
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
