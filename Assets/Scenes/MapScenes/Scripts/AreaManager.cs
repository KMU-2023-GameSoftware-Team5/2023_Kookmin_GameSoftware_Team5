using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace GameMap
{
	public class AreaManager : MonoBehaviour
	{
		static readonly System.Random SystemRandom = new();

		[SerializeField] Sprite m_bossSprite;
		[SerializeField] Sprite m_midBossSprite;
		[SerializeField] Sprite m_battleSprite;
		[SerializeField] Sprite m_randomSprite;
		[SerializeField] Sprite m_chestSprite;

		[SerializeField] GameObject m_bossArea;
		[SerializeField] GameObject[] m_otherAreas;

		[SerializeField] int m_bossOpenMinimum = 5;
		[SerializeField] int m_areaOpenAtOnce = 3;
		bool m_bossRevealed = false;

		public int AreaVisitedCount
		{
			get;
			private set;
		} = 0;

		readonly List<GameObject> m_areaHided = new();

		void Start()
		{
			InitializeRandomSeed();

			Sprite[] t_sprites = { // THIS IS TEMP!
				m_midBossSprite,
				m_battleSprite, m_randomSprite,
				m_chestSprite
			};

			// Set areas
			foreach (GameObject area in m_otherAreas)
			{
				area.GetComponent<Button>().onClick.AddListener(() => AreaOnClick(area));
				area.SetActive(false);
				m_areaHided.Add(area);

				// THIS IS TEMP!
				int index = Random.Range(0, t_sprites.Length);
				SetAreaSprite(area, t_sprites[index]);
			}

			// Set start area
			int startAreaIndex = Random.Range(0, m_otherAreas.Length);
			m_otherAreas[startAreaIndex].SetActive(true);

			// Set boss area
			SetAreaSprite(m_bossArea, m_bossSprite);
			m_bossArea.GetComponent<Button>().onClick.AddListener(() => AreaOnClick(m_bossArea));
			m_bossArea.SetActive(false);
		}

		void AreaOnClick(GameObject callerObject)
		{
			if (++AreaVisitedCount >= m_bossOpenMinimum && !m_bossRevealed)
			{
				m_bossArea.SetActive(true);
				m_bossRevealed = true;
			}

			for (int i = 0; i < m_areaOpenAtOnce && m_areaHided.Count != 0; i++)
			{
				GameObject targetArea = m_areaHided[Random.Range(0, m_areaHided.Count)];

				m_areaHided.Remove(targetArea);
				targetArea.SetActive(true);
			}

			callerObject.SetActive(false);
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
