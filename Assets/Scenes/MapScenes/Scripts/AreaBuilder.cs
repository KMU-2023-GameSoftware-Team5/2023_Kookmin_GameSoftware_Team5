using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMap
{
    class NumberGenerator
    {
        int number = 0;

        public int Next(int _) =>
            number++;
    }

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
        readonly List<AreaData> m_onlyIcon = new();

        Button m_bossButton;
        AreaData m_bossData;

        AreaPickStrategy m_areaPickStrategy = (new NumberGenerator()).Next;

        GameObject m_iconPrefab;
        Transform m_iconParent;

        public readonly List<GameObject> AreaResult = new();
        public readonly List<AreaData> AreaData = new();

        public void AddCanditiateTarget(Image areaImage, Button areaButton) =>
            m_canditiateTargets.Add(Tuple.Create(areaImage, areaButton));

        public void AddOnlyIconAreaData(AreaData areaData) =>
            m_onlyIcon.Add(areaData);

        public void AddAreaData(AreaData areaData) =>
            m_areaDatas.Add(areaData);

        public void AddAreaDatas(AreaData[] areaDatas) =>
            m_areaDatas.AddRange(areaDatas);

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

        /// <summary>
        /// Use strategy to pick area data. Default is in order
        /// </summary>
        public void UseAreaPickStrategy(AreaPickStrategy strategy) =>
            m_areaPickStrategy = strategy;

        public void UseIconParent(Transform parent) =>
            m_iconParent = parent;

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

            // 1-1. Remove boss data from m_areaDatas if exist
            m_areaDatas.Remove(m_bossData);

            // 1-2. Instantiate boss area icon first
            GameObject bossIcon = Instantiate(m_iconPrefab, m_iconParent);
            bossIcon.GetComponentInChildren<Image>().sprite = m_bossData.sprite;
            bossIcon.GetComponentInChildren<TMP_Text>().SetText(m_bossData.areaName);

            // 1-3. Instantiate area icon from distincted m_areaDatas + m_onlyIcon
            foreach (AreaData areaData in m_areaDatas.Concat(m_onlyIcon).Distinct())
            {
                GameObject areaIcon = Instantiate(m_iconPrefab, m_iconParent);
                areaIcon.GetComponentInChildren<Image>().sprite = areaData.sprite;
                areaIcon.GetComponentInChildren<TMP_Text>().SetText(areaData.areaName);
            }

            // 2. Pick Area type with AreaPickStrategy and add to List
            foreach (var targetTuple in m_canditiateTargets)
            {
                AreaData areaData = m_areaDatas[m_areaPickStrategy(m_areaDatas.Count)];
                GameObject gameObject = targetTuple.Item1.gameObject;

                targetTuple.Item1.sprite = areaData.sprite;
                targetTuple.Item2.onClick.AddListener(areaData.onClick.Invoke);

                AreaResult.Add(gameObject);
                AreaData.Add(areaData);
            }

            // 3. Set boss button onClick
            m_bossButton.onClick.AddListener(m_bossData.onClick.Invoke);
        }
    }
}
