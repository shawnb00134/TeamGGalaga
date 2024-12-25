using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Galaga.View;
using Galaga.View.Sprites;

namespace Galaga.Model
{
    /// <summary>
    ///     Manages the Galaga game play.
    /// </summary>
    public class GameManager
    {
        #region Data members

        private const int TickTimer = 50;
        private const int ShipRemovalDelay = 4500;
        private const int LevelCap = 3;

        private readonly Canvas canvas;
        private readonly GameCanvas gameCanvas;
        private readonly PlayerManager playerManager;
        private readonly MissileManager missileManager;
        private readonly SoundManager soundManager;
        private readonly Random random;

        private readonly DispatcherTimer timer;
        private int tickCounter;

        private int score;
        private int currentLevel;
        private bool specialShipHasSpawned;

        private IList<EnemyShip> enemyShips;
        private readonly IList<GameObject> listOfShips;
        private readonly IList<GameObject> missiles;
        private readonly Physics physics;
        private readonly EnemyManager enemyManager;

        private TextBox nameInputBox;
        private Button submitScoreButton;
        private ListView highScoreListView;
        private ComboBox sortingOptions;
        private Rectangle overlayBackground;
        private Grid popupContainer;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GameManager" /> class.
        /// </summary>
        public GameManager(Canvas canvas, GameCanvas gameCanvas)
        {
            this.canvas = canvas ?? throw new ArgumentNullException(nameof(canvas));
            this.gameCanvas = gameCanvas ?? throw new ArgumentNullException(nameof(gameCanvas));
            this.soundManager = new SoundManager();

            this.playerManager = new PlayerManager(this.canvas);
            this.missileManager = new MissileManager();
            this.enemyShips = new List<EnemyShip>();
            this.listOfShips = new List<GameObject>();
            this.missiles = new List<GameObject>();
            this.enemyManager = new EnemyManager(this.canvas);
            this.physics = new Physics();
            this.random = new Random();
            this.canvas = canvas;

            this.tickCounter = 0;
            this.score = 0;
            this.currentLevel = 1;
            this.specialShipHasSpawned = false;

            this.timer = new DispatcherTimer();
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, TickTimer);
            this.timer.Tick += this.timer_Tick;

            this.initializeGame();
            this.timer.Start();
            this.updateScore(this.score);

            this.initializeHighScoreUi();
        }

        #endregion

        #region Methods

        private void timer_Tick(object sender, object e)
        {
            if (this.gameCanvas.IsMovingLeft())
            {
                this.playerManager.MovePlayerLeft();
            }

            if (this.gameCanvas.IsMovingRight())
            {
                this.playerManager.MovePlayerRight();
            }

            this.enemyManager.MoveEnemyShips(this.enemyShips, this.tickCounter);
            this.enemyManager.MoveBonusShip();
            this.enemyFireMissiles();
            this.moveMissiles();
            this.tickCounter++;

            this.missileManager.UpdateDelayTick();
            this.enemyManager.SwapSpritesAnimation(this.enemyShips);
            this.checkForMissileOutOfBounds();
            this.checkForCollisions();

            if (this.getRandomNumber() % 101 == 1 && this.specialShipHasSpawned == false)
            {
                this.enemyShips.Add(this.enemyManager.CreateSpecialShip());
                this.listOfShips.Add(this.enemyShips.Last());
                this.soundManager.PlayBonusShipCreation();
                this.specialShipHasSpawned = true;
            }

            this.checkWhenToRemoveSpecialShip();
        }

        private int getRandomNumber()
        {
            return this.random.Next(0, 1000001);
        }

        private void initializeGame()
        {
            this.playerManager.CreateAndPlacePlayer(this.listOfShips);
            this.initializeLevel();
        }

        private void initializeLevel()
        {
            this.updatePlayerLives();
            this.createEnemyShips();
            this.specialShipHasSpawned = false;
        }

        private void createEnemyShips()
        {
            this.enemyShips = this.enemyManager.CreateAndPlaceEnemyShip(this.currentLevel);

            foreach (var enemyShip in this.enemyShips)
            {
                this.listOfShips.Add(enemyShip);
            }
        }

        /// <summary>
        ///     Moves the player left.
        /// </summary>
        public void MovePlayerLeft()
        {
            this.playerManager.MovePlayerLeft();
        }

        /// <summary>
        ///     Moves the player right.
        /// </summary>
        public void MovePlayerRight()
        {
            this.playerManager.MovePlayerRight();
        }

        /// <summary>
        ///     Fires the missile.
        /// </summary>
        public void FireMissile()
        {
            this.missiles.Add(this.missileManager.FireMissile(this.playerManager.GetPlayer(), this.canvas));
        }

        private void moveMissiles()
        {
            this.missileManager.MoveMissiles(this.missiles, this.canvas);
        }

        private void enemyFireMissiles()
        {
            this.missiles.Add(this.missileManager.FireEnemyMissiles(this.enemyShips, this.playerManager.GetPlayer(), this.canvas));
        }

        private void checkForCollisions()
        {
            var objectsToRemove = this.physics.CheckCollisions(this.listOfShips, this.missiles);
            this.removeObjectsFromCanvas(objectsToRemove);
        }

        private void checkForMissileOutOfBounds()
        {
            var objectsToRemove = new List<GameObject>();

            foreach (var missile in this.missiles)
            {
                if (this.physics.CheckMissileBoundary(missile, this.canvas))
                {
                    objectsToRemove.Add(missile);
                }
            }

            this.removeObjectsFromCanvas(objectsToRemove);
        }

        private void removeObjectsFromCanvas(IList<GameObject> objectsToRemove)
        {
            foreach (var obj in objectsToRemove)
            {
                switch (obj)
                {
                    case Missile _:
                        this.missileManager.TriggerNuke(obj, this.canvas);
                        this.missileManager.CheckForPlayerMissile(obj);
                        this.missiles.Remove(obj);
                        this.canvas.Children.Remove(obj.Sprite);
                        break;
                    case Player _:
                        this.destroyPlayerLife(obj);
                        break;
                    case EnemyShip enemyShip:
                        this.removeEnemyShip(enemyShip);
                        break;
                }

                this.checkForEndGame();
            }
        }

        private void checkWhenToRemoveSpecialShip()
        {
            if (this.enemyManager.CheckBounceCounter())
            {
                foreach (var enemyShip in this.enemyShips)
                {
                    if (enemyShip.Sprite is EnemySpecialSprite)
                    {
                        this.listOfShips.Remove(enemyShip);
                        this.enemyShips.Remove(enemyShip);
                        this.canvas.Children.Remove(enemyShip.Sprite);
                        break;
                    }
                }
            }
        }

        private void destroyPlayerLife(GameObject ship)
        {
            this.playerManager.CheckPlayerLives(ship, this.listOfShips);
            this.enemyManager.PlayExplosion(ship);
            this.missileManager.ResetPlayerLimits();
            this.updatePlayerLives();
            this.soundManager.PlayPlayerDestroyed();
        }

        private void removeEnemyShip(EnemyShip enemyShip)
        {
            if (enemyShip.Sprite is EnemySpecialSprite)
            {
                this.destroySpecialEnemy();
                this.updatePlayerLives();
            }

            this.enemyShips.Remove(enemyShip);
            this.listOfShips.Remove(enemyShip);
            this.canvas.Children.Remove(enemyShip.Sprite);
            this.enemyManager.PlayExplosion(enemyShip);
            this.soundManager.PlayEnemyDestroyed();

            this.updateScore(enemyShip.ScoreValue);
        }

        private void destroySpecialEnemy()
        {
            if (this.currentLevel == 1)
            {
                this.playerManager.AddPlayerLife();
            }

            if (this.currentLevel == 2)
            {
                this.playerManager.AddPlayerLife();
                this.missileManager.PowerUpPlayer();
            }

            if (this.currentLevel == 3)
            {
                this.missileManager.EnableNuke();
            }
        }

        private void updateScore(int scoreValue)
        {
            this.score += scoreValue;
            this.gameCanvas.UpdateScoreBoard("Score: " + this.score);
        }

        private void checkForEndGame()
        {
            if (!this.listOfShips.Any(ship => ship is Player))
            {
                this.timer.Stop();
                this.gameCanvas.DisplayYouLoseText();
                this.handleGameOver();
            }

            if (!this.enemyShips.Any())
            {
                if (this.checkForLevel())
                {
                    this.currentLevel++;
                    this.specialShipHasSpawned = false;
                    this.initializeLevel();
                }
                else
                {
                    this.timer.Stop();
                    this.gameCanvas.DisplayYouWinText();
                    this.handleGameOver();
                }
            }
        }

        private void handleGameOver()
        {
            this.LoadAndDisplayHighScores();

            var highScores = Score.LoadHighScores();

            if (highScores.Count < 10 || this.score > highScores.Min(s => s.PlayerScore))
            {
                this.nameInputBox.Visibility = Visibility.Visible;
                this.submitScoreButton.Visibility = Visibility.Visible;
            }
            else
            {
                this.nameInputBox.Visibility = Visibility.Collapsed;
                this.submitScoreButton.Visibility = Visibility.Collapsed;
            }
        }

        private bool checkForLevel()
        {
            return this.currentLevel < LevelCap;
        }

        private void updatePlayerLives()
        {
            this.gameCanvas.UpdatePlayerLivesBoard("Lives: " + this.playerManager.GetPlayerLivesCount());
        }

        private void initializeHighScoreUi()
        {
            this.overlayBackground = new Rectangle
            {
                Fill = new SolidColorBrush(Color.FromArgb(150, 0, 0, 0)),
                Width = this.canvas.Width,
                Height = this.canvas.Height,
                Visibility = Visibility.Collapsed
            };
            Canvas.SetZIndex(this.overlayBackground, 0);
            this.canvas.Children.Add(this.overlayBackground);

            this.popupContainer = new Grid
            {
                Width = 500,
                Height = 400,
                Background = new SolidColorBrush(Colors.Black),
                BorderBrush = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(2),
                Visibility = Visibility.Collapsed
            };
            Canvas.SetLeft(this.popupContainer, (this.canvas.Width - this.popupContainer.Width) / 2);
            Canvas.SetTop(this.popupContainer, (this.canvas.Height - this.popupContainer.Height) / 2);
            Canvas.SetZIndex(this.popupContainer, 1);

            var containerPanel = new StackPanel();

            var scoreboardLabel = new TextBlock
            {
                Text = "Scoreboard",
                FontSize = 24,
                Foreground = new SolidColorBrush(Colors.White),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            containerPanel.Children.Add(scoreboardLabel);

            this.sortingOptions = new ComboBox
            {
                Width = 300,
                Visibility = Visibility.Visible,
                ItemsSource = new List<string>
                {
                    "Sort by Score/Name/Level",
                    "Sort by Name/Score/Level",
                    "Sort by Level/Score/Name"
                },
                SelectedIndex = 0
            };
            this.sortingOptions.SelectionChanged += this.SortingOptions_SelectionChanged;
            containerPanel.Children.Add(this.sortingOptions);

            this.highScoreListView = new ListView
            {
                Width = 400,
                Height = 300,
                Visibility = Visibility.Visible
            };
            containerPanel.Children.Add(this.highScoreListView);

            var inputPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            this.nameInputBox = new TextBox
            {
                Width = 200,
                PlaceholderText = "Enter your name",
                Visibility = Visibility.Collapsed
            };
            inputPanel.Children.Add(this.nameInputBox);

            this.submitScoreButton = new Button
            {
                Content = "Submit",
                Width = 100,
                Visibility = Visibility.Collapsed
            };
            this.submitScoreButton.Click += this.SubmitScoreButton_Click;
            inputPanel.Children.Add(this.submitScoreButton);

            containerPanel.Children.Add(inputPanel);

            this.popupContainer.Children.Add(containerPanel);
            this.canvas.Children.Add(this.popupContainer);
        }

        private void SubmitScoreButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(this.nameInputBox.Text))
                {
                    var dialog = new ContentDialog
                    {
                        Title = "Invalid Name",
                        Content = "Please enter a valid name.",
                        CloseButtonText = "OK"
                    };
                    _ = dialog.ShowAsync();
                    return;
                }

                Score.AddNewScore(this.nameInputBox.Text.Trim(), this.score, this.currentLevel);

                this.LoadAndDisplayHighScores();

                this.nameInputBox.Visibility = Visibility.Collapsed;
                this.submitScoreButton.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                var errorDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = $"An unexpected error occurred:\n{ex.Message}",
                    CloseButtonText = "OK"
                };
                _ = errorDialog.ShowAsync();
            }
        }

        /// <summary>
        ///     Loads and displays the high scores.
        /// </summary>
        /// <param name="sortBy"></param>
        public void LoadAndDisplayHighScores(string sortBy = "Sort by Score/Name/Level")
        {
            var highScores = Score.LoadHighScores();

            switch (sortBy)
            {
                case "Sort by Score/Name/Level":
                    highScores = highScores
                        .OrderByDescending(s => s.PlayerScore)
                        .ThenBy(s => s.PlayerName)
                        .ThenByDescending(s => s.LevelCompleted)
                        .ToList();
                    break;
                case "Sort by Name/Score/Level":
                    highScores = highScores
                        .OrderBy(s => s.PlayerName)
                        .ThenByDescending(s => s.PlayerScore)
                        .ThenByDescending(s => s.LevelCompleted)
                        .ToList();
                    break;
                case "Sort by Level/Score/Name":
                    highScores = highScores
                        .OrderByDescending(s => s.LevelCompleted)
                        .ThenByDescending(s => s.PlayerScore)
                        .ThenBy(s => s.PlayerName)
                        .ToList();
                    break;
            }

            this.highScoreListView.ItemsSource =
                highScores.Select(s => $"{s.PlayerName} - {s.PlayerScore} - Level {s.LevelCompleted}");
            this.overlayBackground.Visibility = Visibility.Visible;
            this.popupContainer.Visibility = Visibility.Visible;
        }

        private void SortingOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.sortingOptions.SelectedItem is string selectedSort)
            {
                this.LoadAndDisplayHighScores(selectedSort);
            }
        }

        /// <summary>
        ///     Fires the Nuke for the Wow Factor
        /// </summary>
        public async void FireNuke()
        {
            if (this.currentLevel == LevelCap && this.missileManager.NukeEnabled)
            {
                this.missiles.Add(this.missileManager.FireNuke(this.playerManager.GetPlayer(), this.canvas));
                this.missileManager.NukeEnabled = false;

                await Task.Delay(ShipRemovalDelay);

                this.removeAllEnemySprites();
            }
        }

        private void removeAllEnemySprites()
        {
            var removalList = new List<GameObject>();
            foreach (var ship in this.enemyShips)
            {
                removalList.Add(ship);
            }

            foreach (var missile in this.missiles)
            {
                removalList.Add(missile);
            }

            this.removeObjectsFromCanvas(removalList);
        }

        #endregion
    }
}