using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gyorsulás.Model;
using Gyorsulás.Persistence;
using Moq;

namespace Gyorsulás.Test
{
    [TestClass]
    public class GameTest
    {
        private Game? _game;
        private MockFileManager? _mockFileManager;
        private Mock<Gyorsulás.Model.ITimer>? _mockTimer;

        [TestInitialize]
        public void Initialize()
        {
            _mockTimer = new Mock<Model.ITimer>(); //Mock timer has been added, with the package mock
            _game = new Game(_mockTimer.Object);
            _mockFileManager = new MockFileManager();
        }

        //ALL THE TEST FINALLY WORKING IT BETTER WORKS CAUSE IMMA DO A CURT COBAIN ON MYSELF
        [TestMethod]
        public void NewGame_ShouldResetValues()
        {
            // Arrange
            _game!.mainMotor._posX = 0.9;
            _game.mainMotor._fuel = 20;
            _game.score = 99;
            _game.elapsedTime = 5;

            // Act
            _game.Newgame();

            // Assert
            Assert.AreEqual(0.5, _game.mainMotor._posX, 0.001);
            Assert.AreEqual(0.8, _game.mainMotor._posY, 0.001);
            Assert.AreEqual(100, _game.mainMotor._fuel);
            Assert.AreEqual(0, _game.score);
            Assert.AreEqual(0, _game.elapsedTime);
            Assert.IsFalse(_game.isPaused);
            Assert.IsFalse(_game.isGameOver);
        }
        //Mokc timer test
        [TestMethod]
        public void Game_ShouldUpdate_WhenTimerTicks()
        {
            //Arrange
            double before = _game!.elapsedTime;

            //Act
            _mockTimer!.Raise(t => t.Tick += null!, this, EventArgs.Empty);
            
            //Assert
            Assert.IsTrue(_game.elapsedTime >= before);
        }

        [TestMethod]
        public void Pause_ShouldCallStopOnTimer()
        {
            //Arrange
            _mockTimer!.Setup(t => t.Stop());
                
            //Act
            _game!.Pause();
            
            //Assert
            _mockTimer.Verify(t => t.Stop(), Times.Once);
        }
    
        //UPDAZE
        [TestMethod]
        public void UpdateGame_ShouldIncreaseElapsedTime()
        {
            // Arrange
            double before = _game!.elapsedTime;
            System.Threading.Thread.Sleep(50); 

            // Act
            _game.UpdateGame();

            // Assert
            Assert.IsTrue(_game.elapsedTime > before);
        }

        [TestMethod]
        public void UpdateGame_ShouldMoveObjects()
        {
            // Arrange
            double oldFuelY = _game!.mainRoadFuel._posY;
            double oldMapY = _game.mainMap._posY;

            // Act
            _game.UpdateGame();

            // Assert
            Assert.IsTrue(_game.mainRoadFuel._posY > oldFuelY);
            Assert.IsTrue(_game.mainMap._posY > oldMapY);
        }

        [TestMethod]
        public void UpdateGame_ShouldConsumeFuel()
        {
            // Arrange
            double oldFuel = _game!.mainMotor._fuel;

            // Act
            _game.UpdateGame();

            // Assert
            Assert.IsTrue(_game.mainMotor._fuel < oldFuel);
        }

        //COLLISION
        [TestMethod]
        public void Collision_ShouldIncreaseScore_AndFuel()
        {
            // Arrange
            _game!.mainRoadFuel._posX = _game.mainMotor._posX;
            _game.mainRoadFuel._posY = _game.mainMotor._posY;
            double oldFuel = _game.mainMotor._fuel;
            int oldScore = _game.score;

            // Act
            _game.UpdateGame();

            // Assert
            Assert.IsTrue(_game.mainMotor._fuel >= oldFuel);
            Assert.AreEqual(oldScore + 1, _game.score);
        }

        //PERSISTENCE
        [TestMethod]
        public void Save_ShouldWriteAllData()
        {
            // Act
            _game!.Save(_mockFileManager!);

            // Assert
            Assert.IsTrue(_mockFileManager!.SaveCalled);
            Assert.IsTrue(_mockFileManager.LastSavedData.Contains("|"));
            Assert.IsTrue(_mockFileManager.LastSavedData.Contains("\n"));
        }

        [TestMethod]
        public void LoadGame_ShouldRestoreGameState()
        {
            // Arrange
            string saveData =
                "0,25|75\n" +    
                "0,10|-0,5\n" +  
                "-0,2\n" +       
                "5\n" +         
                "2,5\n" +       
                $"{DateTime.Now}\n" + 
                "Right\n";       

            _mockFileManager!.MockLoadData = saveData;

            // Act
            _game!.LoadGame(_mockFileManager);

            // Assert
            Assert.AreEqual(0.25, _game.mainMotor._posX, 0.1);
            Assert.AreEqual(75, _game.mainMotor._fuel, 0.1);
            Assert.AreEqual(0.10, _game.mainRoadFuel._posX, 0.1);
            Assert.AreEqual(-0.5, _game.mainRoadFuel._posY, 0.1);
            Assert.AreEqual(-0.2, _game.mainMap._posY, 0.1);
            Assert.AreEqual(5, _game.score);
            Assert.AreEqual(2.5, _game.elapsedTime, 0.1);
            Assert.IsTrue(_game.isPaused);
        }
    }

    //MOCK STUFF 
    //so it doesnt create a file
    internal class MockFileManager : IFileManager
    {
        public bool SaveCalled { get; private set; } = false;
        public string LastSavedData { get; private set; } = string.Empty;
        public string MockLoadData { get; set; } = string.Empty;

        public string Load()
        {
            return MockLoadData;
        }

        public void Save(string content)
        {
            SaveCalled = true;
            LastSavedData = content;
        }
    }
}
