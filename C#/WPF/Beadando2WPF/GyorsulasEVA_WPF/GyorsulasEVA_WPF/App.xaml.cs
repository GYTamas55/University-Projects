using System.Windows;
using GyorsulasEVA_WPF.View;
using GyorsulasEVA_WPF.ViewModel;
using GyorsulasEVA_WPF.Model;
using Microsoft.Win32;

namespace GyorsulasEVA_WPF
{
    public partial class App : Application
    {
        private MainViewModel? viewModel;
        public App()
        {
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //Model init
            Game game = new Game();

            //ViewModel init és a model átadása 
            viewModel = new MainViewModel(game);

            viewModel.LoadGameRequested += OnLoadGame;
            viewModel.SaveGameRequested += OnSaveGame;
            //View init
            MainWindow window = new MainWindow();

            //A window a viewModellel dolgozzon
            window.DataContext = viewModel;

            //Mutatás
            window.Show();
        }
        private async void OnLoadGame(object? sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                
                if (viewModel == null) { return; }
                try
                {
                    await viewModel.LoadGameFromPathAsync(openFileDialog.FileName);
                    MessageBox.Show("GAME LOADED");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to LOAD game " + ex.Message);
                }
            }
        }

        private async void OnSaveGame(object? sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                
                if (viewModel == null) { return; }
                try
                {
                    await viewModel.SaveGameToPathAsync(saveFileDialog.FileName);
                    MessageBox.Show("GAME SAVED");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to SAVE game " + ex.Message);
                }
            }
        }
    }
}