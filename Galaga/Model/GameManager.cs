using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Galaga.View;
using Galaga.View.Sprites;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;

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
        private TextBlock scoreboardLabel;
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
            soundManager = new SoundManager();

            playerManager = new PlayerManager(this.canvas);
            missileManager = new MissileManager();
            enemyShips = new List<EnemyShip>();
            listOfShips = new List<GameObject>();
            missiles = new List<GameObject>();
            enemyManager = new EnemyManager(this.canvas);
            physics = new Physics();
            random = new Random();
            this.canvas = canvas;

            tickCounter = 0;
            score = 0;
            currentLevel = 1;
            specialShipHasSpawned = false;

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, TickTimer);
            timer.Tick += timer_Tick;

            initializeGame();
            timer.Start();
            updateScore(score);

            InitializeHighScoreUI();
        }

        #endregion
        
        #region Methods

        private void timer_Tick(object sender, object e)
        {
            if (gameCanvas.IsMovingLeft()) playerManager.MovePlayerLeft();

            if (gameCanvas.IsMovingRight()) playerManager.MovePlayerRight();

            enemyManager.MoveEnemyShips(enemyShips, tickCounter);
            enemyManager.MoveBonusShip();
            enemyFireMissiles();
            moveMissiles();
            tickCounter++;

            missileManager.UpdateDelayTick();
            enemyManager.SwapSpritesAnimation(enemyShips);
            checkForMissileOutOfBounds();
            checkForCollisions();

            if (getRandomNumber() % 101 == 1 && specialShipHasSpawned == false)
            {
                enemyShips.Add(enemyManager.CreateSpecialShip());
                listOfShips.Add(enemyShips.Last());
                soundManager.playBonusShipCreation();
                specialShipHasSpawned = true;
            }

            checkWhenToRemoveSpecialShip();
        }

        private int getRandomNumber()
        {
            return random.Next(0, 1000001);
        }

        private void initializeGame()
        {
            playerManager.CreateAndPlacePlayer(listOfShips);
            initializeLevel();
        }

        private void initializeLevel()
        {
            updatePlayerLives();
            createEnemyShips();
            this.specialShipHasSpawned = false;
        }

        private void createEnemyShips()
        {
            enemyShips = enemyManager.CreateAndPlaceEnemyShip(currentLevel);

            foreach (var enemyShip in enemyShips) listOfShips.Add(enemyShip);
        }

        /// <summary>
        ///     Moves the player left.
        /// </summary>
        public void MovePlayerLeft()
        {
            playerManager.MovePlayerLeft();
        }

        /// <summary>
        ///     Moves the player right.
        /// </summary>
        public void MovePlayerRight()
        {
            playerManager.MovePlayerRight();
        }

        /// <summary>
        ///     Fires the missile.
        /// </summary>
        public void FireMissile()
        {
            missiles.Add(missileManager.FireMissile(playerManager.GetPlayer(), canvas));
        }

        private void moveMissiles()
        {
            missileManager.MoveMissiles(missiles, canvas);
        }

        private void enemyFireMissiles()
        {
            missiles.Add(missileManager.FireEnemyMissiles(enemyShips, playerManager.GetPlayer(), canvas));
        }

        private void checkForCollisions()
        {
            var objectsToRemove = physics.CheckCollisions(listOfShips, missiles);
            removeObjectsFromCanvas(objectsToRemove);
        }

        private void checkForMissileOutOfBounds()
        {
            var objectsToRemove = new List<GameObject>();

            foreach (var missile in missiles)
                if (physics.CheckMissileBoundary(missile, canvas))
                    objectsToRemove.Add(missile);

            removeObjectsFromCanvas(objectsToRemove);
        }

        private void removeObjectsFromCanvas(IList<GameObject> objectsToRemove)
        {
            foreach (var obj in objectsToRemove)
            {
                switch (obj)
                {
                    case Missile _:
                        missileManager.TriggerNuke(obj, canvas);
                        missileManager.checkForPlayerMissile(obj);
                        missiles.Remove(obj);
                        canvas.Children.Remove(obj.Sprite);
                        break;
                    case Player _:
                        destroyPlayerLife(obj);
                        break;
                    case EnemyShip enemyShip:
                        removeEnemyShip(enemyShip);
                        break;
                }

                checkForEndGame();
            }
        }

        private void checkWhenToRemoveSpecialShip()
        {
            if (enemyManager.checkBounceCounter())
                foreach (var enemyShip in enemyShips)
                    if (enemyShip.Sprite is EnemySpecialSprite)
                    {
                        listOfShips.Remove(enemyShip);
                        enemyShips.Remove(enemyShip);
                        canvas.Children.Remove(enemyShip.Sprite);
                        break;
                    }
        }

        private void destroyPlayerLife(GameObject ship)
        {
            playerManager.CheckPlayerLives(ship, listOfShips);
            enemyManager.playExplosion(ship);
            missileManager.ResetPlayerLimits();
            updatePlayerLives();
            soundManager.playPlayerDestroyed();
        }

        private void removeEnemyShip(EnemyShip enemyShip)
        {
            if (enemyShip.Sprite is EnemySpecialSprite)
            {
                destroySpecialEnemy();
                updatePlayerLives();
            }

            enemyShips.Remove(enemyShip);
            listOfShips.Remove(enemyShip);
            canvas.Children.Remove(enemyShip.Sprite);
            enemyManager.playExplosion(enemyShip);
            soundManager.playEnemyDestroyed();

            updateScore(enemyShip.ScoreValue);
        }

        private void destroySpecialEnemy()
        {
            if (currentLevel == 1) playerManager.AddPlayerLife();

            if (currentLevel == 2)
            {
                playerManager.AddPlayerLife();
                missileManager.PowerUpPlayer();
            }

            if (currentLevel == 3) missileManager.EnableNuke();
        }

        private void updateScore(int scoreValue)
        {
            score += scoreValue;
            gameCanvas.updateScoreBoard("Score: " + score);
        }

        private void checkForEndGame()
        {
            if (!listOfShips.Any(ship => ship is Player))
            {
                timer.Stop();
                gameCanvas.DisplayYouLoseText();
                HandleGameOver();
            }

            if (!enemyShips.Any())
            {
                if (checkForLevel())
                {
                    currentLevel++;
                    specialShipHasSpawned = false;
                    initializeLevel();
                }
                else
                {
                    timer.Stop();
                    gameCanvas.DisplayYouWinText();
                    HandleGameOver();
                }
            }
        }

        private void HandleGameOver()
        {
            LoadAndDisplayHighScores();

            var highScores = Score.LoadHighScores();

            if (highScores.Count < 10 || score > highScores.Min(s => s.PlayerScore))
            {
                nameInputBox.Visibility = Visibility.Visible;
                submitScoreButton.Visibility = Visibility.Visible;
            }
            else
            {
                nameInputBox.Visibility = Visibility.Collapsed;
                submitScoreButton.Visibility = Visibility.Collapsed;
            }
        }

        private bool checkForLevel()
        {
            return currentLevel < LevelCap;
        }

        private void updatePlayerLives()
        {
            gameCanvas.updatePlayerLivesBoard("Lives: " + playerManager.GetPlayerLivesCount());
        }

        private void InitializeHighScoreUI()
        {
            overlayBackground = new Rectangle
            {
                Fill = new SolidColorBrush(Windows.UI.Color.FromArgb(150, 0, 0, 0)),
                Width = canvas.Width,
                Height = canvas.Height,
                Visibility = Visibility.Collapsed
            };
            Canvas.SetZIndex(overlayBackground, 0);
            canvas.Children.Add(overlayBackground);

            popupContainer = new Grid
            {
                Width = 500,
                Height = 400,
                Background = new SolidColorBrush(Windows.UI.Colors.Black),
                BorderBrush = new SolidColorBrush(Windows.UI.Colors.White),
                BorderThickness = new Thickness(2),
                Visibility = Visibility.Collapsed
            };
            Canvas.SetLeft(popupContainer, (canvas.Width - popupContainer.Width) / 2);
            Canvas.SetTop(popupContainer, (canvas.Height - popupContainer.Height) / 2);
            Canvas.SetZIndex(popupContainer, 1);

            var containerPanel = new StackPanel();

            var scoreboardLabel = new TextBlock
            {
                Text = "Scoreboard",
                FontSize = 24,
                Foreground = new SolidColorBrush(Windows.UI.Colors.White),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            containerPanel.Children.Add(scoreboardLabel);

            sortingOptions = new ComboBox
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
            sortingOptions.SelectionChanged += SortingOptions_SelectionChanged;
            containerPanel.Children.Add(sortingOptions);

            highScoreListView = new ListView
            {
                Width = 400,
                Height = 300,
                Visibility = Visibility.Visible
            };
            containerPanel.Children.Add(highScoreListView);

            var inputPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            nameInputBox = new TextBox
            {
                Width = 200,
                PlaceholderText = "Enter your name",
                Visibility = Visibility.Collapsed
            };
            inputPanel.Children.Add(nameInputBox);

            submitScoreButton = new Button
            {
                Content = "Submit",
                Width = 100,
                Visibility = Visibility.Collapsed
            };
            submitScoreButton.Click += SubmitScoreButton_Click;
            inputPanel.Children.Add(submitScoreButton);

            containerPanel.Children.Add(inputPanel);

            popupContainer.Children.Add(containerPanel);
            canvas.Children.Add(popupContainer);
        }

        private void SubmitScoreButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nameInputBox.Text))
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

                Score.AddNewScore(nameInputBox.Text.Trim(), score, currentLevel);

                LoadAndDisplayHighScores();

                nameInputBox.Visibility = Visibility.Collapsed;
                submitScoreButton.Visibility = Visibility.Collapsed;
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

        private void LoadAndDisplayHighScores(string sortBy = "Sort by Score/Name/Level")
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

            highScoreListView.ItemsSource = highScores.Select(s => $"{s.PlayerName} - {s.PlayerScore} - Level {s.LevelCompleted}");
            overlayBackground.Visibility = Visibility.Visible;
            popupContainer.Visibility = Visibility.Visible;
        }

        private void SortingOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sortingOptions.SelectedItem is string selectedSort)
            {
                LoadAndDisplayHighScores(selectedSort);
            }
        }

        /// <summary>
        ///     Fires the Nuke for the Wow Factor
        /// </summary>
        public async void FireNuke()
        {
            missileManager.EnableNuke();
            if (currentLevel == LevelCap && missileManager.NukeEnabled)
            {
                missiles.Add(missileManager.FireNuke(playerManager.GetPlayer(), canvas));
                missileManager.NukeEnabled = false;

                await Task.Delay(ShipRemovalDelay);

                removeAllEnemySprites();
            }
        }

        private void removeAllEnemySprites()
        {
            var removalList = new List<GameObject>();
            foreach (var ship in enemyShips) removalList.Add(ship);
            foreach (var missile in missiles) removalList.Add(missile);
            removeObjectsFromCanvas(removalList);
        }

        #endregion
    }
}