using UnityEngine;
using DG.Tweening;

namespace PSO
{
    public class Particle : MonoBehaviour
    {
        const float defaultPositionRange = 100f;
        const float defaultVelocityRange = 100f;

        Transform cachedTransform = default;

        [SerializeField, Header("���̓����n�_")]
        Vector3 nextArrivalPosition = default;
        [SerializeField, Header("���ݑ��x")]
        Vector3 currentVelocity     = default;

        [SerializeField, Header("���q�œK�n�_")]
        Vector3 personalBestPosition    = default;
        [SerializeField, Header("���q�œK�n�_�X�R�A")]
        float personalBestPositionScore = 0f;

        /// <summary>
        /// Awake.
        /// ���A�N�Z�X�񐔂������̂�Transform�̃L���b�V����.
        /// </summary>
        void Awake()
        {
            cachedTransform = transform;
        }

        /// <summary>
        /// ������.
        /// �������l�A�������x�̃����_���ݒ�.
        /// </summary>
        public void Init()
        {
            cachedTransform.position = new Vector3(Random.Range(-defaultPositionRange, +defaultPositionRange),
                                                   Random.Range(-defaultPositionRange, +defaultPositionRange),
                                                   Random.Range(-defaultPositionRange, +defaultPositionRange));

            currentVelocity          = new Vector3(Random.Range(-defaultVelocityRange, +defaultVelocityRange),
                                                   Random.Range(-defaultVelocityRange, +defaultVelocityRange),
                                                   Random.Range(-defaultVelocityRange, +defaultVelocityRange));
        }

        /// <summary>
        /// ���̃C�e���[�g���J�n.
        /// </summary>
        public void startNextIteration()
        {
            updateVelocity();
            updateArrivalPosition();
            startDOMove();
        }

        /// <summary>
        /// ���x�̍X�V.
        /// </summary>
        void updateVelocity()
        {
            var globalBestPosition = ParticleManager.Instance.GlobalBestPositionTransform.position;

            currentVelocity = (ParticleManager.Instance.INERTIA_COEFFICIENT       * currentVelocity)                                                              +
                              (ParticleManager.Instance.PERSONAL_BEST_COEFFICIENT * Random.Range(0.0f, 1.0f) * (personalBestPosition - cachedTransform.position)) +
                              (ParticleManager.Instance.GLOBAL_BEST_COEFFICIENT   * Random.Range(0.0f, 1.0f) * (globalBestPosition   - cachedTransform.position)) ;
        }

        /// <summary>
        /// ���̓����n�_�̍X�V.
        /// </summary>
        void updateArrivalPosition()
        {
            nextArrivalPosition = cachedTransform.position + currentVelocity;
        }

        void startDOMove()
        {
            cachedTransform.DOMove(nextArrivalPosition, ParticleManager.Instance.DOTWEEN_DURATION).OnComplete( afterDOMove );
        }

        void afterDOMove()
        {
            updateBestPosition();

            outputLog();
        }

        /// <summary>
        /// �œK���̍X�V.
        /// </summary>
        void updateBestPosition()
        {
            float currentPositionScore = ParticleManager.GetPositionScore(cachedTransform.position);
            if (  currentPositionScore > personalBestPositionScore)
            {
                personalBestPosition      = cachedTransform.position;
                personalBestPositionScore = currentPositionScore;

                ParticleManager.UpdateGlobalBestPosition(personalBestPosition, personalBestPositionScore);
            }
        }

        /// <summary>
        /// ���O�̏o��.
        /// </summary>
        void outputLog()
        {
            Debug.Log(gameObject.name);

            Debug.Log(nextArrivalPosition);
            Debug.Log(currentVelocity);

            Debug.Log(personalBestPosition);
            Debug.Log(personalBestPositionScore);
        }
    }
}