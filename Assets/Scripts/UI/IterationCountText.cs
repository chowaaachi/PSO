using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(Text))]
    class IterationCountText : MonoBehaviour
    {
        [SerializeField]
        Text      text;

        void Reset()
        {
            text = GetComponent<Text>();
        }

        void Update()
        {
            if(PSO.ParticleManager.Instance == null)
            {
                return;
            }

            text.text = PSO.ParticleManager.Instance.IterationCount.ToString();
        }
    }
}
