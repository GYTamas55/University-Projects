using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GyorsulasEVA_WPF.Model;
using GyorsulasEVA_WPF.Persistence;
using GyorsulasEVA_WPF.ViewModel;
using System.Windows.Input;
using System.Windows;

namespace GyorsulasEVA_WPF.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Properties and MainViewModel 
        //Privát game amit nem teszünk ki a viewnak
        private Game _mainGame;

        //Köztes propertyk
        //map
        public double MapWidth => _mainGame.mainMap._width;
        public double MapHeight => _mainGame.mainMap._height;
        public double MapX => _mainGame.mainMap._posX;
        public double MapY => _mainGame.mainMap._posY;
        //Fuel
        public double FuelWidth => _mainGame.mainRoadFuel._width;
        public double FuelHeight => _mainGame.mainRoadFuel._height;
        public double FuelX => _mainGame.mainRoadFuel._posX;
        public double FuelY => _mainGame.mainRoadFuel._posY;
        //Motor
        public double MotorWidth => _mainGame.mainMotor._width;
        public double MotorHeight => _mainGame.mainMotor._height;
        public double MotorX => _mainGame.mainMotor._posX;
        public double MotorY => _mainGame.mainMotor._posY;
        public double MotorFuel => _mainGame.mainMotor._fuel;
        //Többi
        public int Score => _mainGame.score;
        public double ElapsedTime => _mainGame.elapsedTime;
        public bool IsPaused => _mainGame.isPaused;

        // Parancsok
        public DelegateCommand MenuCommand { get; set; }
        public DelegateCommand NewGameCommand { get; set; }
        public DelegateCommand ExitGameCommand { get; set; }
        public DelegateCommand KeyDownCommand { get; private set; }
        public DelegateCommand KeyUpCommand { get; private set; }


        public event EventHandler? LoadGameRequested;
        public event EventHandler? SaveGameRequested;
        public DelegateCommand LoadGameCommand { get; private set; }
        public DelegateCommand SaveGameCommand { get; private set; }
        public MainViewModel(Game mainGame)
        {
            _mainGame = mainGame;

            // Menu 
            LoadGameCommand = new DelegateCommand(OnLoadGameRequested);
            SaveGameCommand = new DelegateCommand(OnSaveGameRequested);


            MenuCommand = new DelegateCommand(OnEsc);
            NewGameCommand = new DelegateCommand(OnNewGame, IsMenuOn);
            ExitGameCommand = new DelegateCommand(OnExitGame, IsMenuOn);
            //Fejbelovommagam
            KeyDownCommand = new DelegateCommand(OnKeyDown);
            KeyUpCommand = new DelegateCommand(OnKeyUp);
            //Update feliratkozás
            _mainGame.GameUpdated += Maingame_GameUpdated;

            OnNewGame(null);
        }
        #endregion
        private void OnLoadGameRequested(object? obj)
        {
            LoadGameRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OnSaveGameRequested(object? obj)
        {
            SaveGameRequested?.Invoke(this, EventArgs.Empty);
        }

        private void Maingame_GameUpdated(object? sender, EventArgs e)
        {
            //Mindent is
            OnPropertyChanged(string.Empty);
        }
        

        // Irányítás
        private void OnKeyDown(object? param)
        {
            var e = param as KeyEventArgs;
            if (e == null) return;

            if (e.Key == Key.Left)
            {
                _mainGame.mainDirection = WhereToMove.Left;
            }
            else if (e.Key == Key.Right)
            {
                _mainGame.mainDirection= WhereToMove.Right;
            }
            else if (e.Key == Key.Escape) 
            {
                OnEsc(null);
            } 
        }

        private void OnKeyUp(object? param)
        {
            var e = param as KeyEventArgs;
            if (e == null) return;

            if (e.Key == Key.Left || e.Key == Key.Right)
            {
                _mainGame.mainDirection = WhereToMove.Stay;
            }
        }
        #region Delegate commandokhoz

        // ESC lenyomása
        private void OnEsc(object? obj)
        {
            //Már le volt nyomva
            if (_mainGame.isPaused)
            {
                _mainGame.Resume();
            }
            //Először lenyomott
            else
            {
                _mainGame.Pause();
            }
            RefreshMenuState();
        }

        //CanExecute a menuhöz
        private bool IsMenuOn(object? obj)
        {
            return _mainGame.isPaused;
        }

        //Load
        //Gomb
        //ÁTKERÜLT
        //Tenyleges
        public async Task LoadGameFromPathAsync(string filePath)
        {
            IFileManager fm = new FileManager(filePath);
            try
            {
                await _mainGame.LoadGameAsync(fm);
                RefreshMenuState();
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.Message);
            }
        }

        // Save
        //Gomb
        //ÁTKERÜLT
        //Tenyleges
        public async Task SaveGameToPathAsync(string filePath)
        {
            IFileManager fm = new FileManager(filePath);
            try
            {
                await _mainGame.SaveAsync(fm);
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.Message);
            }
        }

        //Exit
        private void OnExitGame(object? obj)
        {
            Application.Current.Shutdown();
        }

        //Ujjatek
        private void OnNewGame(object? obj)
        {
            _mainGame.Newgame();
            //Ujjatek után menu nem kell
            RefreshMenuState();
        }

        //Segedfv menuhoz
        private void RefreshMenuState()
        {
            //Pause valtozott
            OnPropertyChanged(string.Empty);

            //Kattintható vizsgálat
            NewGameCommand.RaiseCanExecuteChanged();
            ExitGameCommand.RaiseCanExecuteChanged();
        }

        #endregion
    }
}