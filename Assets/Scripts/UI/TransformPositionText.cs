using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(Text))]
    class TransformPositionText : MonoBehaviour
    {
        const string textFormat = "x: {0}, y: {1}, z: {2}";

        [SerializeField]
        Transform targetTransform;
        [SerializeField]
        Text      text;

        void Reset()
        {
            text = GetComponent<Text>();
        }

        void Update()
        {
            text.text = string.Format(textFormat,
                                      targetTransform.position.x,
                                      targetTransform.position.y,
                                      targetTransform.position.z);
        }
    }
}
