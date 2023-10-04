using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameMap
{
	public class AreaManager : MonoBehaviour
	{
		static readonly System.Random SystemRandom = new();

		[SerializeField] GameObject m_iconGroupLayout;
		[SerializeField] GameObject m_iconPrefab;

		[SerializeField] AreaData[] m_areaData;
		[SerializeField] GameObject m_canditiateGroup;

		[SerializeField] GameObject m_bossArea;
		[SerializeField] AreaData m_bossData;

		[SerializeField] GameObject m_nearbyStandardA;
		[SerializeField] GameObject m_nearbyStandardB;

		[SerializeField] [Range(0, 50)] int m_bossOpenMinimum = 5;
		int m_areaVisitCount = 0;

		List<GameObject> m_areas;

		float NearbyDistanceStandard
		{
			get => (m_nearbyStandardA.transform.position -
					m_nearbyStandardB.transform.position).magnitude;
		}

		void Start()
		{
			InitializeRandomSeed();
			BuildAreas();

			// Disable all areas and add onClick listeners to area
			foreach (GameObject area in m_areas)
			{
				area.SetActive(false);

				// Todo: Check this work on multiple button click
				area.GetComponent<Button>().onClick.AddListener(() =>
				{
					m_areaVisitCount++;
					DisableAllAreas(area);
					ActivateNearAreas(area);
				});
			}

			// Pick and show start area
			int startAreaIndex = Random.Range(0, m_areas.Count);
			m_areas[startAreaIndex].SetActive(true);
		}

		void InitializeRandomSeed()
		{
			int seed = SystemRandom.Next();

			Random.InitState(seed);
			Debug.Log($"Seed: {seed}");
		}

		void BuildAreas()
		{
			var builder = new AreaBuilder();

			if (!builder.TryUseIconPrefab(m_iconPrefab))
			{
				Debug.LogError("Icon prefab doesn't have chuld component Image or TMP_Text or both.");
				return;
			}

			builder.UseAreaPickStrategy(areaDataCount =>
			{
				// THIS IS TEMP!
				return Random.Range(0, areaDataCount);
			});

			builder.UseIconParent(m_iconGroupLayout.transform);

			if (!m_bossArea.TryGetComponent(out Button bossButton))
			{
				Debug.LogError($"Boss area does not have button component");
				return;
			}

			builder.UseBossTarget(bossButton, m_bossData);

			foreach (Transform canditiateTransfrom in m_canditiateGroup.transform)
			{
				if (!canditiateTransfrom.TryGetComponent(out Image image))
				{
					string name = canditiateTransfrom.name;
					Debug.LogWarning($"This area does not have image component: {name}");
					continue;
				}

				if (!canditiateTransfrom.TryGetComponent(out Button button))
				{
					string name = canditiateTransfrom.name;
					Debug.LogWarning($"This area does not have button component: {name}");
					continue;
				}

				builder.AddCanditiateTarget(image, button);
			}

			foreach (AreaData areaData in m_areaData)
				builder.AddAreaData(areaData);

			m_areas = builder.Build();
		}

		void ActivateNearAreas(GameObject target)
		{
			Vector3 areaPos = target.transform.position;

			foreach (GameObject other in m_areas)
			{
				if (target == other || (other == m_bossArea && m_areaVisitCount < m_bossOpenMinimum))
					continue;

				Vector3 otherPos = other.transform.position;

				// Other area is out of range
				if ((areaPos - otherPos).magnitude >= NearbyDistanceStandard)
					continue;

				other.SetActive(true);
			}
		}

		void DisableAllAreas(GameObject exclusive = null)
		{
			foreach (GameObject area in m_areas)
			{
				if (area == exclusive)
					continue;

				area.SetActive(false);
			}
		}
	}
}
