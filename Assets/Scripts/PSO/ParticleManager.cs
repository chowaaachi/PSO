using System;
using System.Collections.Generic;
using UnityEngine;

namespace PSO
{
    public class ParticleManager : MonoBehaviour
    {
        public static ParticleManager Instance = null;

        [SerializeField, Header("DOTween���s�Ԋu"), Range(0.0f, 5.0f)]
        public float DOTWEEN_DURATION = 1.0f;

        [SerializeField, Header("�����W��"), Range(0.0f, 1.0f)]
        public float INERTIA_COEFFICIENT = 0.9f;
        [SerializeField, Header("���q�œK���̎Q�ƌW��"), Range(0.0f, 1.0f)]
        public float PERSONAL_BEST_COEFFICIENT = 0.6f;
        [SerializeField, Header("���q�Q�œK���̎Q�ƌW��"), Range(0.0f, 1.0f)]
        public float GLOBAL_BEST_COEFFICIENT = 0.3f;

        [SerializeField, Header("���q�Q")]
        List<Particle> particles = new List<Particle>();

        [SerializeField, Header("�ŗǒn�_")]
        public Transform target = default;

        [SerializeField, Header("���q�Q�œK�n�_")]
        public Vector3 GlobalBestPosition      = default;
        [SerializeField, Header("���q�Q�œK�n�_�X�R�A")]
        public float   GlobalBestPositionScore = 0f;

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

        float timeCount = 0.0f;

        void Update()
        {
            timeCount += Time.deltaTime;
            if (timeCount >= DOTWEEN_DURATION)
            {
                particles.ForEach(particle => particle.startNextIteration());
                timeCount = 0.0f;
            }
        }

        /// <summary>
        /// ���W�̕]�����擾.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static float GetPositionScore(Vector3 position)
        {
            return 1.0f / (Instance.target.localPosition - position).sqrMagnitude;
        }

        /// <summary>
        /// ���q�Q�œK���̍X�V.
        /// </summary>
        /// <param name="newBestPosition"></param>
        /// <param name="newBestPositionScore"></param>
        public static void UpdateGlobalBestPosition(Vector3 newBestPosition, float newBestPositionScore)
        {
            if (newBestPositionScore > Instance.GlobalBestPositionScore)
            {
                Instance.GlobalBestPosition      = newBestPosition;
                Instance.GlobalBestPositionScore = newBestPositionScore;

                Debug.Log("Updated!");
                Debug.Log(newBestPosition);
            }
        }
    }
}