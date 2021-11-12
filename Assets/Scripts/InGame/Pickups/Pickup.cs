using UnityEngine;
using Pacmania.GameManagement;

namespace Pacmania.InGame.Pickups
{
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class Pickup : MonoBehaviour
    {
        [SerializeField] protected int score;

        protected Level level;
        private void Awake()
        {
            level = FindObjectOfType<Level>();
        }

        public int Score
        {
            get { return score; }
        }

        public virtual void OnPickedUp()
        {
            Game.Instance.CurrentSession.AddScore(level, score);
        }

    }
}
