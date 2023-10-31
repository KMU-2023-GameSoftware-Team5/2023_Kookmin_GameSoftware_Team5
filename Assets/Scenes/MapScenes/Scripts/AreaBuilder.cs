using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMap
{
    public class AreaBuilder : UnityEngine.Object
    {
        /// <summary>
        /// A strategy for selecting the type of a particular area.
        /// </summary>
        /// <param name="areaDataCount"> Count of added AreaData </param>
        /// <returns> Return index of AreaData array to apply </returns>
        public delegate int AreaPickStrategy(int areaDataCount);

        readonly List<Tuple<Image, Button>> m_canditiateTargets = new();
        readonly List<AreaData> m_areaDatas = new();

        Button m_bossButton;
        AreaData m_bossData;

        AreaPickStrategy m_areaPickStrategy = null;

        GameObject m_iconPrefab;
        Transform m_iconParent;

        public readonly List<GameObject> AreaResult = new();
        public readonly List<AreaData> AreaData = new();

        public void AddCanditiateTarget(Image areaImage, Button areaButton) =>
            m_canditiateTargets.Add(Tuple.Create(areaImage, areaButton));

        public void AddAreaData(AreaData areaData)
            => m_areaDatas.Add(areaData);

        public void UseAreaDatas(List<AreaData> areaDatas) =>
            areaDatas.ForEach(x => AreaData.Add(x));

        /// <summary>
        /// Try to use icon prefab
        /// </summary>
        /// <returns> If iconPrefab have Image and TMP-text, return true.
        /// Else return false. </returns>
        public bool TryUseIconPrefab(GameObject iconPrefab)
        {
            if (iconPrefab.GetComponentInChildren<Image>() == null ||
                iconPrefab.GetComponentInChildren<TMP_Text>() == null)
                return false;

            m_iconPrefab = iconPrefab;
            return true;
        }

        public void UseAreaPickStrategy(AreaPickStrategy strategy)
            => m_areaPickStrategy = strategy;

        public void UseIconParent(Transform parent)
            => m_iconParent = parent;

        public void UseBossTarget(Button bossButton, AreaData bossData)
        {
            m_bossButton = bossButton;
            m_bossData = bossData;
        }

        /// <summary>
        /// Instantiate icons with AreaDatas and pick each areas type.
        /// </summary>
        public void Build()
        {
            // 0. If any core object is null, return
            if (m_iconPrefab == null || m_areaPickStrategy is null ||
                m_bossButton == null || m_bossData == null)
                return;

            // 1. Instantiate area icon
            foreach (AreaData areaData in m_areaDatas)
            {
                GameObject areaIcon = Instantiate(m_iconPrefab, m_iconParent);
                areaIcon.GetComponentInChildren<Image>().sprite = areaData.sprite;
                areaIcon.GetComponentInChildren<TMP_Text>().SetText(areaData.areaName);
            }

            // 2-1. Remove boss data from m_areaDatas if exist
            m_areaDatas.Remove(m_bossData);

            // 2-2. Pick Area type with AreaPickStrategy and add to List
            for (int i = 0; i < m_canditiateTargets.Count; i++)
            {
                var targetTuple = m_canditiateTargets[i];

                AreaData areaData;

                if (m_areaPickStrategy is null)
                    areaData = m_areaDatas[i % m_areaDatas.Count];
                else
                    areaData = m_areaDatas[m_areaPickStrategy(m_areaDatas.Count)];

                targetTuple.Item1.sprite = areaData.sprite;
                targetTuple.Item2.onClick.AddListener(areaData.onClick.Invoke);

                AreaResult.Add(targetTuple.Item1.gameObject);
                AreaData.Add(areaData);
            }

            // 3. Set boss button
            m_bossButton.onClick.AddListener(m_bossData.onClick.Invoke);
        }
    }
}
