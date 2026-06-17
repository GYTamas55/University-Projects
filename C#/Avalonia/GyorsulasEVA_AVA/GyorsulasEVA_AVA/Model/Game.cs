using GyorsulasEVA_AVA.Persistence;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace GyorsulasEVA_AVA.Model
{
    public class Game
    {
        #region Properties
        //Contains all the properties of 'Game'
        //GameData and such

        
        public Motor mainMotor { get; set; }
        public RoadFuel mainRoadFuel { get; set; }
        public Map mainMap { get; set; }
        public WhereToMove mainDirection { get; set; } = WhereToMove.Stay;
        public ITimer mainTimer { get; set; } = new FinalTimer(16);
        public double elapsedTime { get; set; } = 0; //For accelaration 
        public DateTime lastUpdate { get; set; } = DateTime.Now; //For moving

        public int score { get; set; } = 0;
        public bool isGameOver { get; set; } = false;
        public bool isPaused { get; set; } = false;

        #endregion

        #region EVENTS
        public event EventHandler? GameUpdated;

        private void OnGameUpdated()
        {
            GameUpdated?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        public Game(ITimer? timer = null)
        {
            mainMotor = new Motor(0.5, 100);
            mainRoadFuel = new RoadFuel(-1);
            mainMap = new Map(-1);

            mainTimer = timer ?? new FinalTimer(16);
            mainTimer.Tick += (s, e) => UpdateGame();

            mainRoadFuel.FuelCollected += WhenFuelCollected;
        }

        public void Newgame()
        {
            
            mainMotor._posX = 0.5;
            mainMotor._posY = 0.8;
            mainMotor._fuel = 100;

            mainRoadFuel._posX = 0.1;
            mainRoadFuel._posY = -1;

            mainMap._posY = -1;

            score = 0;
            elapsedTime = 0;
            isPaused = false;
            isGameOver = false;

            this.Pause();
            this.Resume();

        }

        public void NewGame(Motor newMotor, RoadFuel newRoadFuel, Map newMap, int newScore, double newElapsedTime) //For load
        {
            mainMotor = newMotor;
            mainRoadFuel = newRoadFuel;
            mainMap = newMap;

            score = newScore;
            elapsedTime = newElapsedTime;
            isPaused = false;
            isGameOver = false;

            this.Pause();
            
        }


        public void UpdateGame() //Manages Time and Speed
        {
            if (isPaused || isGameOver)
            {
                return;
            }

            //Time stuff
            DateTime now = DateTime.Now;
            double deltaTime = (now - lastUpdate).TotalSeconds;
            lastUpdate = now;
            elapsedTime += deltaTime;

            //Speed stuff
            double initialSpeed = 0.02; //Randomly picked initially
            double acceleration = 0.001; //Randomly picked initially 
            double actualSpeed = initialSpeed + acceleration * elapsedTime;
            //Gamestate stuff
            mainRoadFuel.Move(actualSpeed);
            mainMap.Move(actualSpeed);
            mainMotor.Move(mainDirection);
            mainMotor.ReduceTankAmount(0.01 * elapsedTime);
            if (CheckOverlap(mainMotor, mainRoadFuel))
            {
                mainRoadFuel.OnFuelCollected();
            }
            OnGameUpdated();
            isGameOver = mainMotor._fuel <= 0;

        }
        private bool CheckOverlap(Motor m, RoadFuel r)
        {
            return !(m._posX + m._width < r._posX || //Motor is on the left of the fuel
                     m._posX > r._posX + r._width || //Motor is on the right of the fuel
                     m._posY + m._height < r._posY || //Motor is under the fuel
                     m._posY > r._posY + r._height); //Motor is above the fuel
        }

        private void WhenFuelCollected(object? sender, CollectEventArgs e)
        {
            mainMotor.CalcTankAmount(e.FuelAdd);
            score += 1;
        }

        public void Pause()
        {
            if (!isPaused)
            {
                isPaused = true;
                mainTimer.Stop();
            }
        }

        public void Resume()
        {
            if (isPaused)
            {
                isPaused = false;
                lastUpdate = DateTime.Now;
                mainTimer.Start();
            }
        }

        //Saves in the following order:
        //Motor (x, fuel)
        //RoadFuel (x,y)
        //Map (y)
        //Score
        //ElapsedTime
        //lastUpdate
        //Direction
        public async Task SaveAsync(IFileManager fileManager)
        {
            
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{this.mainMotor._posX}|{this.mainMotor._fuel}");
            sb.AppendLine($"{this.mainRoadFuel._posX}|{this.mainRoadFuel._posY}");
            sb.AppendLine($"{this.mainMap._posY}");
            sb.AppendLine($"{this.score}");
            sb.AppendLine($"{this.elapsedTime}");
            sb.AppendLine($"{this.lastUpdate}");
            sb.AppendLine($"{this.mainDirection}");

            await fileManager.SaveAsync(sb.ToString());
        }

        public async Task LoadGameAsync(IFileManager fileManager)
        {
            //All data
            //Először megvárjuk még megkapjuk az adatokat
            string waitedContent = await fileManager.LoadAsync();
            //Aztán kezdünk csak dolgozni vele
            string[] savedDataRows = waitedContent.Split('\n'); 
            //Motor
            string[] rawData = savedDataRows[0].Split('|');
            Motor newMotor = new Motor(double.Parse(rawData[0]), double.Parse(rawData[1]));
            //RoadFuel
            rawData = savedDataRows[1].Split('|');
            RoadFuel newRoadFuel = new RoadFuel(double.Parse(rawData[0]), double.Parse(rawData[1]));
            //Map
            Map newMap = new Map(double.Parse(savedDataRows[2]));
            
            this.NewGame(newMotor,  newRoadFuel, newMap, int.Parse(savedDataRows[3]), double.Parse(savedDataRows[4]));

           mainRoadFuel.FuelCollected -= WhenFuelCollected;
           mainRoadFuel.FuelCollected += WhenFuelCollected;

            isPaused = true;
            
        }

    }


}

