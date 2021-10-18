using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Pacmania.GameManagement;
using UnityEditor;

namespace Pacmania.InGame.Characters.Ghost
{
    public class ScatterChaseTimer : MonoBehaviour
    {
        public enum currentGhostAction { scatter, chase };

        [Serializable]
        public struct ScatterChasePair
        {
            public int scatterSeconds;
            public int chaseSeconds;
        }

        [SerializeField] private List<ScatterChasePair> periods = new List<ScatterChasePair>();

        public currentGhostAction CurrentAction { get; private set; } = currentGhostAction.scatter;
        public bool Paused { get; set; } = false;
        private ScatterChasePair currentPeriod;
        private int currentCounter = 0;
        private int periodIndex = 0;

        private void Awake()
        {
            if (periods.Count > 0)
            {
                currentPeriod = periods[periodIndex];
                currentCounter = currentPeriod.scatterSeconds * Game.FramesPerSecond;
            }
            else
            {
                Debug.LogError("No scatter/chase periods set for level.");
            }
        }
        private void FixedUpdate()
        {
            if (Paused == true || periods.Count <= 0)
            {
                return;
            }

            if (currentCounter > 0)
            {
                currentCounter--;
            }

            if (currentCounter <= 0)
            {
                // If scattering go into chase mode for same period.
                if (CurrentAction == currentGhostAction.scatter)
                {
                    CurrentAction = currentGhostAction.chase;
                    currentCounter = currentPeriod.chaseSeconds * Game.FramesPerSecond;
                }
                else
                {
                    // Move to scatter state in the next period. If this is the last period then stay with it.
                    if (periodIndex < periods.Count - 1)
                    {
                        periodIndex++;
                    }

                    currentPeriod = periods[periodIndex];
                    CurrentAction = currentGhostAction.scatter;
                    currentCounter = currentPeriod.scatterSeconds * Game.FramesPerSecond;
                }
            }
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            float secondsCounter = ((float)currentCounter) / Game.FramesPerSecond;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(periodIndex);
            stringBuilder.Append(" ,");
            stringBuilder.Append(CurrentAction.ToString());
            stringBuilder.Append(" ,");
            stringBuilder.Append(secondsCounter.ToString("0.00"));

            Handles.Label(transform.position, stringBuilder.ToString());
#endif
        }
    }
}
