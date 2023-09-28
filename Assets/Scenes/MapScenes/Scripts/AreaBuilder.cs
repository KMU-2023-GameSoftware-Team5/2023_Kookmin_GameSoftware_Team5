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

		readonly List<Tuple<Image, Button>> m_areaTagets = new();
		readonly List<GameObject> m_canditiates = new();
		readonly List<AreaData> m_areaDatas = new();

		AreaPickStrategy m_areaPickStrategy = null;

		GameObject m_iconPrefab = null;
		Transform m_iconParent = null;

		public void AddAreaTarget(Image areaImage, Button areaButton) =>
			m_areaTagets.Add(Tuple.Create(areaImage, areaButton));

		public void AddCanditiateGroup(GameObject group)
		{
			foreach (Transform child in group.transform)
				m_canditiates.Add(child.gameObject);
		}

		public void AddAreaData(AreaData areaData) => m_areaDatas.Add(areaData);

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

		public void UseAreaPickStrategy(AreaPickStrategy strategy) => m_areaPickStrategy = strategy;

		public void UseIconParent(Transform parent) => m_iconParent = parent;

		/// <summary>
		/// Instantiate icons with AreaDatas and 
		/// </summary>
		/// <returns>  </returns>
		public List<GameObject> Build()
		{
			// 0. If any core object is null, return empty list
			if (m_iconPrefab == null || m_areaPickStrategy == null)
				return new List<GameObject>();

			// 1. Instantiate area icon
			foreach (AreaData areaData in m_areaDatas)
			{
				GameObject areaIcon = Instantiate(m_iconPrefab, m_iconParent);
				areaIcon.GetComponentInChildren<Image>().sprite = areaData.sprite;
				areaIcon.GetComponentInChildren<TMP_Text>().SetText(areaData.areaName);
			}

			// 2. Pick Area type with AreaPickStrategy and add to List
			List<GameObject> ret = new();

			foreach (var targetTuple in m_areaTagets)
			{
				int index = m_areaPickStrategy(m_areaDatas.Count);

				targetTuple.Item1.sprite = m_areaDatas[index].sprite;
				targetTuple.Item2.onClick.AddListener(() => m_areaDatas[index].onClick.Invoke());

				ret.Add(targetTuple.Item1.gameObject);
			}

			// 3. Return list
			return ret;
		}
	}
}
