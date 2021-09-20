using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Station
{
    public class StationManager : MonoBehaviour
    {
        const int PARTICLE_COUNT = 1000;
        const int MAX_ITERATION  = 10000;

        [SerializeField]
        Station[] stationList;

        int iterationCount = 0;

        List<StationParticle> stationParticleList = new List<StationParticle>(PARTICLE_COUNT);

        Vector2[] gBestPositionSet;
        float     gBestPositionScore = 0.0f;

        /// <summary>
        /// Reset.
        /// ※SerializeFieldの初期化.
        /// </summary>
        void Reset()
        {
            stationList = GetComponentsInChildren<Station>();
        }

        /// <summary>
        /// Start.
        /// </summary>
        void Start()
        {
            int stationCount = stationList.Length;
            gBestPositionSet = new Vector2[stationCount];

            for (int ii = 0; ii < PARTICLE_COUNT; ii++)
            {
                var stationParticle = new StationParticle(stationCount);
                stationParticle.UpdateBestPosition();

                stationParticleList.Add(stationParticle);
            }

            updateGBestPosition();
        }

        /// <summary>
        /// Update.
        /// </summary>
        void Update()
        {
            iterationCount++;
            if (iterationCount == MAX_ITERATION)
            {
                Debug.Log("Finish");
            }

            if (iterationCount >= MAX_ITERATION)
            {
                return;
            }

            updateStationParticles();
            updateGBestPosition();
        }

        /// <summary>
        /// 各粒子の更新.
        /// </summary>
        void updateStationParticles()
        {
            stationParticleList.ForEach(stationParticle =>
            {
                stationParticle.UpdateVelocity(gBestPositionSet);
            });

            Parallel.ForEach(stationParticleList, stationParticle =>
            {
                stationParticle.UpdatePosition();
                stationParticle.UpdateBestPosition();
            });
        }

        /// <summary>
        /// 粒子群最適解の更新.
        /// </summary>
        void updateGBestPosition()
        {
            stationParticleList.ForEach(stationParticle =>
            {
                if (stationParticle.PBestPositionScore > gBestPositionScore)
                {
                    gBestPositionScore = stationParticle.PBestPositionScore;
                    Array.Copy(stationParticle.PBestPositionSet, gBestPositionSet, stationList.Length);

                    Debug.Log("GBest Updated!!!!!!!!!!!!!!!!!");
                    Debug.Log("Iteration: "  + iterationCount.ToString());
                    Debug.Log("gBestScore: " + gBestPositionScore.ToString());

                    displayStations(gBestPositionSet);
                }
            });
        }

        /// <summary>
        /// 解の表示.
        /// </summary>
        /// <param name="stationPositionSet"></param>
        void displayStations(Vector2[] stationPositionSet)
        {
            for (int ii = 0; ii < stationList.Length; ii++)
            {
                var station         = stationList[ii];
                var stationPosition = gBestPositionSet[ii];

                station.transform.position = stationPosition;
            }
        }
    }
}