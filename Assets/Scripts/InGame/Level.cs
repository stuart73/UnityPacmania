using System.Collections.Generic;
using UnityEngine;
using Pacmania.Utilities.Random;
using Pacmania.Utilities.StateMachines;
using Pacmania.InGame.Characters;
using Pacmania.InGame.Characters.Pacman;
using Pacmania.InGame.Characters.Ghost;
using Pacmania.InGame.UI;
using Pacmania.GameManagement;
using Pacmania.InGame.LevelStates;
using Pacmania.InGame.Arenas;
using Pacmania.InGame.Pickups;
using Pacmania.InGame.ScoreSprites;

namespace Pacmania.InGame
{
    public class Level : MonoBehaviour
    {
        [SerializeField] [Range(0, 50)] private int levelNumber = default;
        public int LevelNumber
        {
            get { return levelNumber; }
        }
        public Arena Arena { get; private set; }
        public Hud Hud { get; private set; }
        public PacmanController Pacman { get; private set; }
        public FrightenSiren FrightenSiren { get; private set; }
        public CharacterManager CharacterManager { get; private set; }
        public GhostManager GhostManager { get; private set; }
        public ScoreSpawner ScoreSpawner { get; private set; }

        public Audio.AudioManager AudioManager { get; private set; }
        public SeedUniformRandomNumberStream RandomStream { get; private set; }

        private StateMachine fsm;

        private void Awake()
        {
            Arena = FindObjectOfType<Arena>();
            Hud = FindObjectOfType<Hud>();
            Pacman = FindObjectOfType<PacmanController>();
            FrightenSiren = FindObjectOfType<FrightenSiren>();
            AudioManager = FindObjectOfType<Audio.AudioManager>();
            GhostManager = FindObjectOfType<GhostManager>();
            ScoreSpawner = FindObjectOfType<ScoreSpawner>();
            CharacterManager = new CharacterManager();
            RandomStream = new SeedUniformRandomNumberStream(1);

            if (Arena == null)
            {
                Debug.LogError("No arena found when starting level", this);
            }
            if (Hud == null)
            {
                Debug.LogError("No Hud found when starting level", this);
            }
            if (Pacman == null)
            {
                Debug.LogError("No pacman found when starting level", this);
            }
            if (FrightenSiren == null)
            {
                Debug.LogError("No FrightenSiren found when starting level", this);
            }
            if (AudioManager == null)
            {
                Debug.LogError("No AudioManager found when starting level", this);
            }
            if (GhostManager == null)
            {
                Debug.LogError("No GhostManager found when starting level", this);
            }
            if (GhostManager == null)
            {
                Debug.LogError("No ScoreSpawner found when starting level", this);
            }

            // CurrentLevel should already be set. But just in case we jumped straight into
            // this level from the unity editor, then make sure it is set here.
            Game.Instance.CurrentSession.CurrentLevel = levelNumber;
        }

        private void Start()
        {
            if (Pacman != null)
            {
                // Reguster with things the level needs to know about.
                PacmanCollision pacmanCollision = Pacman.GetComponent<PacmanCollision>();
                pacmanCollision.Dying += Pacman_Dying;
                pacmanCollision.EatenPellete += Pacman_EatenPellete;
                FindObjectOfType<ScoreSpawner>().Spawned += Score_Spawned;
            }

            Hud.RedrawFruit();
        }

        private void Score_Spawned()
        {
            // This check should not be needed but just in case.
            if (fsm.CurrrentState.GetType() == typeof(PlayingState))
            {
                fsm.SetState(typeof(EatenPauseState));
            }
        }

        private void GeneranteFSM()
        {
            List<BaseState> states = new List<BaseState>();
            states.Add(new GetReadyState());
            states.Add(new EatenPauseState());
            states.Add(new PlayingState());
            states.Add(new DyingState());
            states.Add(new WinState());
            states.Add(new GameOverState());
            fsm = new StateMachine(gameObject, states);
        }

        private void FixedUpdate()
        {
            if (fsm == null)
            {
                GeneranteFSM();
            }
            fsm.Update();
        }

        private void Pacman_EatenPellete()
        {
            bool containsPellete = Arena.ContainsPickup<Pellet>();
            bool containsPowerPellete = Arena.ContainsPickup<PowerPellet>();

            if (containsPellete == false && containsPowerPellete == false)
            {
                fsm.SetState(typeof(WinState));
            }
        }

        private void Pacman_Dying()
        {
            fsm.SetState(typeof(DyingState));
        }

        // Cheat method
        public void WinLevel()
        {
            fsm.SetState(typeof(WinState));
        }
    }
}

