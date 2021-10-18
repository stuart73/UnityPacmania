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

namespace Pacmania.InGame
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private int levelNumber = default;
        public int LevelNumber
        {
            get { return levelNumber; }
        }

        public Arena Arena { get; private set; }  
        public Hud Hud { get; private set; }
        public PacmanController Pacman { get; private set; }
        public ScatterChaseTimer ScatterChaseTimer { get; private set; }
        public CharacterManager CharacterManager { get; private set; }
        public GhostManager GhostManager { get; private set; }
        public StateMachine FSM { get; private set; }
        public Audio.AudioManager AudioManager { get; private set; }
        public SeedUniformRandomNumberStream RandomStream { get; private set; } = new SeedUniformRandomNumberStream(1);
    
        private void Awake()
        {
            Arena = FindObjectOfType<Arena>();
            Hud = FindObjectOfType<Hud>();
            Pacman = FindObjectOfType<PacmanController>();
            ScatterChaseTimer = FindObjectOfType<ScatterChaseTimer>();
            AudioManager = FindObjectOfType<Audio.AudioManager>();
            GhostManager = FindObjectOfType<GhostManager>();
            CharacterManager = new CharacterManager();
          
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
            if (ScatterChaseTimer == null)
            {
                Debug.LogError("No ScatterChaseTimer found when starting level", this);
            }
            if (AudioManager == null)
            {
                Debug.LogError("No AudioManager found when starting level", this);
            }
            if (GhostManager == null)
            {
                Debug.LogError("No GhostManager found when starting level", this);
            }

            // CurrentLevel should already be set. But just in case we jumped straight into
            // this level from the unity editor, then make sure it is set here.
            Game.Instance.CurrentSession.CurrentLevel = levelNumber;
        }

        private void Start()
        {
            if (Pacman != null)
            {
                PacmanCollision pacmanCollision = Pacman.GetComponent<PacmanCollision>();
                pacmanCollision.Dying += Pacman_Dying;
                pacmanCollision.EatenGhost += Pacman_EatenGhost;
                pacmanCollision.EatenPellete += Pacman_EatenPellete;
            }

            Hud.RedrawFruit();
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
            FSM = new StateMachine(gameObject, states);
        }

        private void FixedUpdate()
        {
            if (FSM == null)
            {
                GeneranteFSM();
            }
            FSM.Update();
        }

        private void Pacman_EatenGhost()
        {
            FSM.SetState(typeof(EatenPauseState));
        }

        private void Pacman_EatenPellete()
        {
            bool containsPellete = Arena.ContainsPickup<Pellet>();
            bool containsPowerPellete = Arena.ContainsPickup<PowerPellet>();

            if (containsPellete ==false && containsPowerPellete ==false)
            {
                FSM.SetState(typeof(WinState));
            }
        }

        private void Pacman_Dying()
        {
            FSM.SetState(typeof(DyingState));
        }     
    }
}

