using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Avalonia.Input;
using GyorsulasEVA_AVA.Model;
using GyorsulasEVA_AVA.Persistence;
using GyorsulasEVA_AVA.ViewModels;
using Avalonia.Controls;
using Avalonia;
using System.Diagnostics;


namespace GyorsulasEVA_AVA.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Properties and MainViewModel 
        //Privát game amit nem teszünk ki a viewnak
        private Game _mainGame;
        private IFileManager _fileManager;

        // Kommunikáció, view eléri
        public Game publicGameInfo => _mainGame;

        #region JAVÍTOTT PROPERTYCHANGE
        //Az OnPropertyChange-ekhez
        public int score_Prop => _mainGame.score;
        public double fuel_Prop => _mainGame.mainMotor._fuel;
        public double elapsedTime_Prop => _mainGame.elapsedTime;

        public double fuelY_Prop => _mainGame.mainRoadFuel._posY;
        public double fuelX_Prop => _mainGame.mainRoadFuel._posX;
        public double fuelWidth_Prop => _mainGame.mainRoadFuel._width;
        public double fuelHeight_Prop => _mainGame.mainRoadFuel._height;

        public double motorX_Prop => _mainGame.mainMotor._posX;
        public double motorY_Prop => _mainGame.mainMotor._posY;
        public double motorWidth_Prop => _mainGame.mainMotor._width;
        public double motorHeight_Prop => _mainGame.mainMotor._height;

        public double mapX_Prop => _mainGame.mainMap._posX;
        public double mapY_Prop => _mainGame.mainMap._posY;
        public double mapWidth_Prop => _mainGame.mainMap._width;
        public double mapHeight_Prop => _mainGame.mainMap._height;

        public bool isGameOver_Prop => _mainGame.isGameOver;
        public bool isGamePaused_Prop => _mainGame.isPaused;

        private void Maingame_GameUpdated(object? sender, EventArgs e)
        {
            
            OnPropertyChanged(nameof(fuelX_Prop));
            OnPropertyChanged(nameof(fuelY_Prop));

            OnPropertyChanged(nameof(motorX_Prop));
       
            OnPropertyChanged(nameof(mapY_Prop));

            OnPropertyChanged(nameof(isGameOver_Prop));
            OnPropertyChanged(nameof(isGamePaused_Prop));

            OnPropertyChanged(nameof(score_Prop));
            OnPropertyChanged(nameof(fuel_Prop));
            OnPropertyChanged(nameof(elapsedTime_Prop));

            OnPropertyChanged(nameof(motorWidth_Prop));
            OnPropertyChanged(nameof(motorHeight_Prop));
            OnPropertyChanged(nameof(fuelWidth_Prop));
            OnPropertyChanged(nameof(fuelHeight_Prop));
        }

        //Segedfv menuhoz
        private void RefreshMenuState()
        {
            //Pause valtozott
            OnPropertyChanged(nameof(isGamePaused_Prop));

            //Kattintható vizsgálat
            NewGameCommand.RaiseCanExecuteChanged();
            ExitGameCommand.RaiseCanExecuteChanged();
            SaveGameCommand.RaiseCanExecuteChanged();
            LoadGameCommand.RaiseCanExecuteChanged();
        }
        #endregion

        // Parancsok
        public DelegateCommand MenuCommand { get; set; }
        public DelegateCommand NewGameCommand { get; set; }
        public DelegateCommand ExitGameCommand { get; set; }
        public DelegateCommand SaveGameCommand { get; set; }
        public DelegateCommand LoadGameCommand { get; set; }
        public DelegateCommand KeyDownCommand { get; private set; }
        public DelegateCommand KeyUpCommand { get; private set; }

        //New events
        public event EventHandler? LoadGameRequested;
        public event EventHandler? SaveGameRequested;
        public MainViewModel(Game mainGame)
        {
            _mainGame = mainGame;
            _fileManager = new FileManager("initialSave.txt");

            // Menu 
            MenuCommand = new DelegateCommand(OnEsc);
            NewGameCommand = new DelegateCommand(OnNewGame, IsMenuOn);
            ExitGameCommand = new DelegateCommand(OnExitGame, IsMenuOn);
            SaveGameCommand = new DelegateCommand(OnSaveGame, IsMenuOn);
            LoadGameCommand = new DelegateCommand(OnLoadGame, IsMenuOn);
            //Fejbelovommagam
            KeyDownCommand = new DelegateCommand(OnKeyDown);
            KeyUpCommand = new DelegateCommand(OnKeyUp);
            //Update feliratkozás
            _mainGame.GameUpdated += Maingame_GameUpdated;

            OnNewGame(null);
        }
        #endregion

        

        

        // Irányítás
        public void SetDirection(WhereToMove direction)
        {
            _mainGame.mainDirection = direction;
        }
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
        private void OnLoadGame(object? obj)
        {
            LoadGameRequested?.Invoke(this, EventArgs.Empty);
        }
        //Tenyleges
        public async Task LoadGameFromPathAsync(string filePath)
        {
            IFileManager customFileManager = new FileManager(filePath);
            try
            {
                await _mainGame.LoadGameAsync(customFileManager);
                RefreshMenuState();
                // MessageBox helyett konzolra írunk vagy egy státusz property-t állítunk
                Debug.WriteLine("GAME LOADED");
            }
            catch (Exception e)
            {
                Debug.WriteLine("Failed to LOAD: " + e.Message);
            }
        }

        // Save
        //Gomb
        private void OnSaveGame(object? obj)
        {
           SaveGameRequested?.Invoke(this, EventArgs.Empty);
        }
        //Tenyleges
        public async Task SaveGameToPathAsync(string filePath)
        {
            IFileManager customFileManager = new FileManager(filePath);
            try
            {
                await _mainGame.SaveAsync(customFileManager);
                Debug.WriteLine("GAME SAVED");
            }
            catch (Exception e)
            {
                Debug.WriteLine("Failed to SAVE: " + e.Message);
            }
        }

        //Exit
        private void OnExitGame(object? obj)
        {
            Environment.Exit(0);
        }

        //Ujjatek
        private void OnNewGame(object? obj)
        {
            _mainGame.Newgame();
            //Ujjatek után menu nem kell
            RefreshMenuState();
        }



        #endregion
    }
}