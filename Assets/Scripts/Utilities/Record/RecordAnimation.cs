#if UNITY_EDITOR
using UnityEngine;
using UnityEditor.Animations;

namespace Pacmania.Utilities.Record
{
    public class RecordAnimation : MonoBehaviour
    {
        [SerializeField] private AnimationClip clip = default;
        private GameObjectRecorder recorder;

        private void Start()
        {
            // Create recorder and record the script GameObject.
            recorder = new GameObjectRecorder(gameObject);

            // Bind all the Transforms on the GameObject and all its children.
            recorder.BindComponentsOfType<Transform>(gameObject, true);
        }

        private void LateUpdate()
        {
            if (clip == null)
                return;

            // Take a snapshot and record all the bindings values for this frame.
            recorder.TakeSnapshot(Time.deltaTime);
        }

        private void OnDisable()
        {
            if (clip == null)
                return;

            if (recorder.isRecording)
            {
                // Save the recorded session to the clip.
                recorder.SaveToClip(clip);
            }
        }
    }
}
#endif