using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameMap
{
	[System.Serializable]
	public class LimitDistanceStandard
	{
		public GameObject a;
		public GameObject b;

		public float Value
		{
			get => (a.transform.position - b.transform.position).magnitude;
		}
	}

	public class AreaManager : MonoBehaviour
	{
		static readonly System.Random SystemRandom = new();

		[SerializeField] AreaData[] m_areaData;

		[SerializeField] GameObject m_iconGroupLayout;
		[SerializeField] GameObject m_iconPrefab;

		[SerializeField] GameObject m_canditiateGroup;
		[SerializeField] GameObject m_bossArea;

		[SerializeField] LimitDistanceStandard m_maxDistanceGroup;
		[SerializeField] int m_bossOpenMinimum = 5;

		List<GameObject> m_areas = null;

		static Vector3 GetWorldCenterPosition(GameObject target)
		{
			Vector3[] temp = new Vector3[4];

			target.GetComponent<RectTransform>().GetWorldCorners(temp);

			return (temp[0] + temp[2]) / 2;
		}

		void Start()
		{
			InitializeRandomSeed();
			BuildAreas();

			// Disable all areas and add onClick to areas
			foreach (GameObject area in m_areas)
			{
				area.SetActive(false);

				var button = area.GetComponent<Button>();

				button.onClick.AddListener(() =>
				{
					Vector3 areaPos = GetWorldCenterPosition(area);

					foreach (GameObject other in m_areas)
					{
						if (area == other)
							continue;

						Vector3 otherPos = GetWorldCenterPosition(other);

						// Other area is out of range
						if ((areaPos - otherPos).magnitude >= m_maxDistanceGroup.Value)
							continue;

						other.SetActive(true);
					}
				});
			}

			// Pick and show start area
			int startAreaIndex = Random.Range(0, m_areas.Count);
			m_areas[startAreaIndex].SetActive(true);
		}

		void BuildAreas()
		{
			var builder = new AreaBuilder();

			if (!builder.TryUseIconPrefab(m_iconPrefab))
			{
				Debug.LogError("Icon prefab doesn't have component Image or TMP_Text or both.");
				return;
			}

			builder.UseAreaPickStrategy(areaDataCount =>
			{
				// THIS IS TEMP!
				return Random.Range(0, areaDataCount);
			});

			builder.UseIconParent(m_iconGroupLayout.transform);

			foreach (Transform canditiateTransfrom in m_canditiateGroup.transform)
			{
				if (!canditiateTransfrom.TryGetComponent(out Image image))
				{
					string name = canditiateTransfrom.name;
					Debug.LogError($"This area does not have image component: {name}");
					continue;
				}

				if (!canditiateTransfrom.TryGetComponent(out Button button))
				{
					string name = canditiateTransfrom.name;
					Debug.LogError($"This area does not have button component: {name}");
					continue;
				}

				builder.AddAreaTarget(image, button);
			}

			foreach (AreaData areaData in m_areaData)
				builder.AddAreaData(areaData);

			m_areas = builder.Build();
		}

		void InitializeRandomSeed()
		{
			int seed = SystemRandom.Next();

			Random.InitState(seed);
			Debug.Log($"Seed: {seed}");
		}
	}
}
