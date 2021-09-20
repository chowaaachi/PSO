using System;
using UnityEngine;

namespace Station
{
    public class StationParticle
    {
        const float POSITION_RANGE = 10f;

        const float DEFAULT_POSITION_RANGE = 10f;
        const float DEFAULT_VELOVITY_RANGE = 10f;

        const float INERTIA_COEFFICIENT       = 0.99f;
        const float PERSONAL_BEST_COEFFICIENT = 0.01f;
        const float GLOBAL_BEST_COEFFICIENT   = 0.01f;

        int       stationCount;

        Vector2[] positionSet;
        Vector2[] velocitySet;

        Vector2[] pBestPositionSet;
        float     pBestPositionScore = 0f;

        /// <summary>
        /// 粒子最良地点.
        /// </summary>
        public Vector2[] PBestPositionSet
        {
            get { return pBestPositionSet; }
        }
        /// <summary>
        /// 粒子最良地点スコア.
        /// </summary>
        public float PBestPositionScore
        {
            get { return pBestPositionScore; }
        }

        /// <summary>
        /// コンストラクタ.
        /// </summary>
        /// <param name="stationCount"></param>
        public StationParticle(int stationCount)
        {
            this.stationCount = stationCount;

            positionSet      = new Vector2[stationCount];
            velocitySet      = new Vector2[stationCount];
            pBestPositionSet = new Vector2[stationCount]; ;

            for(int ii = 0;ii < stationCount;ii++)
            {
                positionSet[ii] = new Vector2(UnityEngine.Random.Range(-DEFAULT_POSITION_RANGE, +DEFAULT_POSITION_RANGE),
                                              UnityEngine.Random.Range(-DEFAULT_POSITION_RANGE, +DEFAULT_POSITION_RANGE));
                velocitySet[ii] = new Vector2(UnityEngine.Random.Range(-DEFAULT_VELOVITY_RANGE, +DEFAULT_VELOVITY_RANGE),
                                              UnityEngine.Random.Range(-DEFAULT_VELOVITY_RANGE, +DEFAULT_VELOVITY_RANGE));
            }
        }

        /// <summary>
        /// 速度情報の更新.
        /// </summary>
        /// <param name="gBestPositionSet"></param>
        public void UpdateVelocity(Vector2[] gBestPositionSet)
        {
            float pBestCoefficient = PERSONAL_BEST_COEFFICIENT * UnityEngine.Random.Range(0.0f,1.0f);
            float gBestCoefficient = GLOBAL_BEST_COEFFICIENT   * UnityEngine.Random.Range(0.0f,1.0f);

            for (int ii = 0; ii < stationCount; ii++)
            {
                var position      = positionSet[ii];
                var pBestPosition = pBestPositionSet[ii];
                var gBestPosition = gBestPositionSet[ii];

                velocitySet[ii] = INERTIA_COEFFICIENT * velocitySet[ii]            +
                                  pBestCoefficient    * (pBestPosition - position) +
                                  gBestCoefficient    * (gBestPosition - position) ;
            }
        }

        /// <summary>
        /// 位置情報の更新.
        /// </summary>
        public void UpdatePosition()
        {
            for (int ii = 0; ii < stationCount; ii++)
            {
                positionSet[ii] += velocitySet[ii];

                // 範囲外に出ていたら反転する.
                while (true)
                {
                    if (     positionSet[ii].x > POSITION_RANGE)
                    {
                        positionSet[ii].x  = ( 2 * POSITION_RANGE) - positionSet[ii].x;
                        velocitySet[ii].x *= -1;
                    }
                    else if (positionSet[ii].x < -POSITION_RANGE)
                    {
                        positionSet[ii].x  = (-2 * POSITION_RANGE) - positionSet[ii].x;
                        velocitySet[ii].x *= -1;
                    }
                    else if (positionSet[ii].y > POSITION_RANGE)
                    {
                        positionSet[ii].y  = ( 2 * POSITION_RANGE) - positionSet[ii].y;
                        velocitySet[ii].y *= -1;
                    }
                    else if (positionSet[ii].y < -POSITION_RANGE)
                    {
                        positionSet[ii].y  = (-2 * POSITION_RANGE) - positionSet[ii].y;
                        velocitySet[ii].y *= -1;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 最良地点の更新.
        /// </summary>
        public void UpdateBestPosition()
        {
            float newScore = getScore(positionSet);

            if(newScore > pBestPositionScore)
            {
                pBestPositionScore = newScore;
                Array.Copy(positionSet, pBestPositionSet, stationCount);
            }
        }

        /// <summary>
        /// 評価関数.
        /// </summary>
        /// <param name="positionSet"></param>
        /// <returns></returns>
        float getScore(Vector2[] positionSet)
        {
            float shortestMagnitude = float.MaxValue;

            int count = positionSet.Length;
            for (int ii = 0; ii < count; ii++)
            {
                var positionA = positionSet[ii];

                for (int jj = ii + 1; jj < count; jj++)
                {
                    var positionB = positionSet[jj];

                    var magnitude = (positionA - positionB).sqrMagnitude;
                    if (magnitude < shortestMagnitude)
                    {
                        shortestMagnitude = magnitude;
                    }
                }
            }

            return shortestMagnitude;
        }
    }
}
