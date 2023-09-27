using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameMap
{
	[System.Serializable]
	public class LimitStandard
	{
		public GameObject a;
		public GameObject b;

		public float LimitDistance
		{
			get => (a.transform.position - b.transform.position).magnitude;
		}
	}

	public class AreaManager : MonoBehaviour
	{
		static readonly System.Random SystemRandom = new();

		[SerializeField] AreaData[] m_areaData;

		[SerializeField] LayoutGroup m_mapIconLayout;
		[SerializeField] GameObject m_mapIconPrefab;

		[SerializeField] GameObject m_areaGroup;
		[SerializeField] GameObject m_bossArea;

		[SerializeField] LimitStandard m_maximumDistanceBetween;
		[SerializeField] int m_bossOpenMinimum = 5;
		int m_areaVisitCount = 0;

		public int AreaVisitedCount
		{
			get;
			private set;
		} = 0;

		readonly List<GameObject> m_otherAreas = new();

		static Vector3 GetWorldCenterPositionOfRectObject(GameObject target)
		{
			Vector3[] temp = new Vector3[4];

			target.GetComponent<RectTransform>().GetWorldCorners(temp);

			return (temp[0] + temp[2]) / 2;
		}

		void Start()
		{
			InitializeRandomSeed();
			SetMapAreas();
			SetMapIcons();
		}

		void SetMapIcons()
		{
			// Set icons with m_areaData
			foreach (var areaData in m_areaData)
			{
				GameObject mapIcon = Instantiate(m_mapIconPrefab, m_mapIconLayout.transform);
				areaData.SetIcon(mapIcon);
			}
		}

		void SetMapAreas()
		{
			// Add all other areas to list
			foreach (Transform areaTransfrom in m_areaGroup.transform)
				m_otherAreas.Add(areaTransfrom.gameObject);

			// Set area
			// THIS IS TEMP!
			foreach (GameObject area in m_otherAreas)
			{
				int index = Random.Range(0, m_areaData.Length);

				// Set area
				area.GetComponent<Button>().onClick.AddListener(() => AreaOnClick(area));
				m_areaData[index].SetArea(area);
			}

			// Hide all areas
			HideAllAreas();

			// Pick and show start area
			int startAreaIndex = Random.Range(0, m_otherAreas.Count);
			m_otherAreas[startAreaIndex].SetActive(true);
		}

		// THIS IS TEMP!
		public void AreaOnClick(GameObject area)
		{
			m_areaVisitCount++;
			HideAllAreas();
			RevealNearAreas(area);
		}

		void HideAllAreas()
		{
			foreach (var area in m_otherAreas)
				area.SetActive(false);
			m_bossArea.SetActive(false);
		}

		void RevealNearAreas(GameObject area)
		{
			Vector3 areaPos = GetWorldCenterPositionOfRectObject(area);

			foreach (var target in m_otherAreas)
			{
				if (area == target)
					continue;

				Vector3 targetPos = GetWorldCenterPositionOfRectObject(target);

				if ((areaPos - targetPos).magnitude >= m_maximumDistanceBetween.LimitDistance)
					continue;

				target.SetActive(true);
			}

			if (m_areaVisitCount >= m_bossOpenMinimum)
			{
				Vector3 bossPos = GetWorldCenterPositionOfRectObject(m_bossArea);

				if ((areaPos - bossPos).magnitude < m_maximumDistanceBetween.LimitDistance)
					m_bossArea.SetActive(true);
			}
		}

		void InitializeRandomSeed()
		{
			int seed = SystemRandom.Next();

			Random.InitState(seed);
			Debug.Log($"Seed: {seed}");
		}

		void SetAreaSprite(GameObject areaObject, Sprite sprite)
		{
			Image image = areaObject.GetComponent<Image>();
			image.sprite = sprite;
		}
	}
}
