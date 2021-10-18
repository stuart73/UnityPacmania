using UnityEngine;

namespace Pacmania.Cutscenes
{
    public class SandBoxSpinBlocks : MonoBehaviour
    {
        [SerializeField] private GameObject boxes = default;

        private const float boxOffsetIntervalSeconds = 1.0f / 8.0f;
        private void Start()
        {
            int i = 0;
            Animator[] boxAnimator = boxes.GetComponentsInChildren<Animator>();
            foreach (Animator animator in boxAnimator)
            {
                AnimatorClipInfo[] currentClipInfo;
                currentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
                string clipName = currentClipInfo[0].clip.name;

                // Play each box spin start time at 1/8 second intervals.
                animator.Play(clipName, 0, (boxOffsetIntervalSeconds * i++));
            }
        }
    }
}

