using System.Windows.Input; 
using GyorsulasEVA_WPF.ViewModel;
using System.Windows;
using Microsoft.Win32;

namespace GyorsulasEVA_WPF.View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += (s, e) => this.Focus();
        }
       
    }
}