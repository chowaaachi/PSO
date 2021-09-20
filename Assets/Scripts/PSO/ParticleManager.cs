using System;
using System.Collections.Generic;
using UnityEngine;

namespace PSO
{
    public class ParticleManager : MonoBehaviour
    {
        public static ParticleManager Instance = null;

        [SerializeField, Header("DOTween実行間隔"), Range(0.0f, 5.0f)]
        public float DOTWEEN_DURATION = 1.0f;

        [SerializeField, Header("慣性係数"), Range(0.0f, 1.0f)]
        public float INERTIA_COEFFICIENT = 0.9f;
        [SerializeField, Header("粒子最適解の参照係数"), Range(0.0f, 1.0f)]
        public float PERSONAL_BEST_COEFFICIENT = 0.6f;
        [SerializeField, Header("粒子群最適解の参照係数"), Range(0.0f, 1.0f)]
        public float GLOBAL_BEST_COEFFICIENT = 0.3f;

        [SerializeField, Header("粒子群")]
        List<Particle> particles = new List<Particle>();

        [SerializeField, Header("最良地点")]
        public Transform target = default;

        [SerializeField, Header("粒子群最適地点")]
        public Transform GlobalBestPositionTransform = default;

        [NonSerialized]
        public float     GlobalBestPositionScore = 0f;

        [NonSerialized]
        public float     IterationTimeCount = 0.0f;
        [NonSerialized]
        public int       IterationCount      = 0;

        [ContextMenu("GetParticles")]
        public void GetParticles()
        {
            particles = new List<Particle>(transform.GetComponentsInChildren<Particle>());
        }

        void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }

            Instance = this;
        }

        void Start()
        {
            particles.ForEach(particle => particle.Init());
        }


        void Update()
        {
            IterationTimeCount += Time.deltaTime;
            if (IterationTimeCount >= DOTWEEN_DURATION)
            {
                particles.ForEach(particle => particle.startNextIteration());
                IterationTimeCount = 0.0f;
                IterationCount++;
            }
        }

        /// <summary>
        /// 座標の評価を取得.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static float GetPositionScore(Vector3 position)
        {
            return 1.0f / (Instance.target.localPosition - position).sqrMagnitude;
        }

        /// <summary>
        /// 粒子群最適解の更新.
        /// </summary>
        /// <param name="newBestPosition"></param>
        /// <param name="newBestPositionScore"></param>
        public static void UpdateGlobalBestPosition(Vector3 newBestPosition, float newBestPositionScore)
        {
            if (newBestPositionScore > Instance.GlobalBestPositionScore)
            {
                Instance.GlobalBestPositionTransform.position = newBestPosition;
                Instance.GlobalBestPositionScore     = newBestPositionScore;

                Debug.Log("Updated!");
                Debug.Log(newBestPosition);
            }
        }
    }
}