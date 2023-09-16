using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CanditiateManager : MonoBehaviour
{
	static readonly System.Random m_systemRandom = new();

	public List<GameObject> m_canditiateGroups;
	public GameObject m_midBossPrefab;
	public GameObject m_battlePrefab;
	public GameObject m_randomPrefab;
	public GameObject m_chestPrefab;

	readonly List<GameObject> m_selectedArea = new();

	// Start is called before the first frame update
	void Start()
	{
		int seed = m_systemRandom.Next();
		Random.InitState(seed);
		Debug.Log($"Seed: {seed}");

		// Choice areas from canditiate groups
		foreach (GameObject canditiateGroup in m_canditiateGroups)
		{
			Debug.Assert(canditiateGroup.transform.childCount > 0, "Can't find canditiates from groups.");

			int areaSelected = Random.Range(0, canditiateGroup.transform.childCount);

			Transform areaTransform = canditiateGroup.transform.GetChild(areaSelected);
			m_selectedArea.Add(areaTransform.gameObject);
		}

		/// ## BELOW IS TEMP! ## ///
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
	}

	public static void OnAreaClick(string areaName)
	{
		print(areaName + ": clicked");
	}
}
