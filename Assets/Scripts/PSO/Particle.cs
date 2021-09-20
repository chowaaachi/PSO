using UnityEngine;
using DG.Tweening;

namespace PSO
{
    public class Particle : MonoBehaviour
    {
        const float defaultPositionRange = 100f;
        const float defaultVelocityRange = 100f;

        Transform cachedTransform = default;

        [SerializeField, Header("次の到着地点")]
        Vector3 nextArrivalPosition = default;
        [SerializeField, Header("現在速度")]
        Vector3 currentVelocity     = default;

        [SerializeField, Header("粒子最適地点")]
        Vector3 personalBestPosition    = default;
        [SerializeField, Header("粒子最適地点スコア")]
        float personalBestPositionScore = 0f;

        /// <summary>
        /// Awake.
        /// ※アクセス回数が多いのでTransformのキャッシュ化.
        /// </summary>
        void Awake()
        {
            cachedTransform = transform;
        }

        /// <summary>
        /// 初期化.
        /// ※初期値、初期速度のランダム設定.
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
        /// 次のイテレートを開始.
        /// </summary>
        public void startNextIteration()
        {
            updateVelocity();
            updateArrivalPosition();
            startDOMove();
        }

        /// <summary>
        /// 速度の更新.
        /// </summary>
        void updateVelocity()
        {
            var globalBestPosition = ParticleManager.Instance.GlobalBestPositionTransform.position;

            currentVelocity = (ParticleManager.Instance.INERTIA_COEFFICIENT       * currentVelocity)                                                              +
                              (ParticleManager.Instance.PERSONAL_BEST_COEFFICIENT * Random.Range(0.0f, 1.0f) * (personalBestPosition - cachedTransform.position)) +
                              (ParticleManager.Instance.GLOBAL_BEST_COEFFICIENT   * Random.Range(0.0f, 1.0f) * (globalBestPosition   - cachedTransform.position)) ;
        }

        /// <summary>
        /// 次の到着地点の更新.
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
        /// 最適解の更新.
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
        /// ログの出力.
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