using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Pacmania.Utilities.Record
{
    public class RecordKeyboard : MonoBehaviour
    {
        public class KeyboardSnapshot
        {
            public KeyboardSnapshot(int count, Vector2Int direction, bool jump)
            {
                this.count = count;
                this.direction = direction;
                this.jump = jump;
            }
            public int count;
            public Vector2Int direction;
            public bool jump;
        }

        private List<KeyboardSnapshot> steps = new List<KeyboardSnapshot>();

        private Vector2Int lastDirection = new Vector2Int(2, 2);
        private bool lastJump = false;
        private int count = 0;

        public void RecordFixedUpdate(Vector2Int direction, bool jump)
        {
            if (direction.x != lastDirection.x ||
                direction.y != lastDirection.y ||
                jump ||
                lastJump == true)
            {
                var snapshot = new KeyboardSnapshot(count, direction, jump);
                steps.Add(snapshot);
                lastDirection = direction;
                lastJump = jump;
            }
            count++;
        }

        public void Print()
        {
            var sb = new StringBuilder();
            foreach (var record in steps)
            {
                sb.Append("new RecordKeyboard.KeyboardSnapshot(");
                sb.Append(record.count.ToString());
                sb.Append(", ");
                sb.Append(record.direction.x.ToString());
                sb.Append(", ");
                sb.Append(record.direction.y.ToString());
                sb.Append(", ");
                sb.Append(record.jump.ToString().ToLower());
                sb.Append("),\r\n");
            }

            Debug.Log(sb.ToString()) ;

        }
    }
}
