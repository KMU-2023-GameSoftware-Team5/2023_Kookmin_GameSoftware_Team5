using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CanditiateManager : MonoBehaviour
{
	static readonly System.Random m_systemRandom = new();

	public GameObject m_midBossPrefab;
	public GameObject m_battlePrefab;
	public GameObject m_randomPrefab;
	public GameObject m_chestPrefab;

	public GameObject m_startArea;
	public GameObject temp;
	public float m_areaDistMax = 130.36f;

	// Start is called before the first frame update
	void Start()
	{
		print((m_startArea.transform.position - temp.transform.position).magnitude);

		int seed = m_systemRandom.Next();

		Random.InitState(seed);
		Debug.Log($"Seed: {seed}");

		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_startArea.transform.position, m_areaDistMax);

		print(colliders.Length);

		m_startArea.SetActive(true);

		foreach (var temp in colliders)
		{
			temp.gameObject.SetActive(true);
		}

		/// ## BELOW IS TEMP! ## ///
		/*
		GameObject[] prefabs = {
			m_midBossPrefab,
			m_battlePrefab,
			m_randomPrefab,
			m_chestPrefab
		};

		foreach (GameObject area in m_selectedArea)
		{
			int rand = Random.Range(0, 4);
			GameObject selectedPrefab = prefabs[rand];
			var setting = new ConvertToPrefabInstanceSettings();

			PrefabUtility.ConvertToPrefabInstance(area, selectedPrefab, setting, InteractionMode.AutomatedAction);
		}
		*/
	}

	// THIS IS TEMP!
	public static void OnAreaClick(string areaName)
	{
		print(areaName + ": clicked");
	}
}
