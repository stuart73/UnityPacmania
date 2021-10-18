
namespace Pacmania.GameManagement
{
    public abstract class GameSession
    {
        public int Lives { get; set; } = 3;
        public bool InfiniteLives { get; set; } = false;

        public int CurrentLevel { get; set; } = 1;
        public int StartedGameOnLevel { get; set; } = 1;
        public int Score { get; set; } = 0;
        public abstract void StartNextScene();
        public abstract void CompletedLevel();
        public virtual void AddScore(int amount) { }

        public abstract void GameOver();
    }
}
