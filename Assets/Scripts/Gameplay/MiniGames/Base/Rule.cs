namespace Gameplay.MiniGames.Base
{
    public abstract class Rule
    {
        public bool Performed { get; private set; }

        public abstract bool CheckPerformance();
    }
}