namespace Galaga.Model
{
    /// <summary>
    ///     EnemyMissile class.
    /// </summary>
    public class EnemyMissile : GameObject
    {
        #region Data members

        private const int SpeedXDirection = 0;
        private const int SpeedYDirection = 12;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="EnemyMissile" /> class.
        /// </summary>
        public EnemyMissile()
        {
            Sprite = new View.Sprites.EnemyMissile();
            SetSpeed(SpeedXDirection, SpeedYDirection);
        }

        #endregion
    }
}