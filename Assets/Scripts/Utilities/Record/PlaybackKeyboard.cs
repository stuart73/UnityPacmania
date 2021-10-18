using UnityEngine;
using Pacmania.InGame.Arenas;

namespace Pacmania.Utilities.Record
{
    public class PlaybackKeyboard : MonoBehaviour
    {
        private int count = 0;
        private int index = 0;
        private Arena arena;

        private void Awake()
        {
            arena = FindObjectOfType<Arena>();
        }

        private void Update()
        {
        }

        public RecordKeyboard.KeyboardSnapshot GetNextFixedUpdateSnapshot()
        {
            if (arena.DemoSteps == null)
            {
                return new RecordKeyboard.KeyboardSnapshot(0, new Vector2Int(0,0), false);
            }
            RecordKeyboard.KeyboardSnapshot next = arena.DemoSteps[index+1];

            if (count == next.count && index < arena.DemoSteps.Length - 1)
            {
                index++;
            }

            count++;
            return arena.DemoSteps[index];
        }
    }
}
