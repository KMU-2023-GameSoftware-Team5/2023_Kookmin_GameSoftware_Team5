using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameMap
{
    public class AreaManager : MonoBehaviour
    {
        public static MapStageData MapData;

        [SerializeField] MobSetData[] m_mobSetDatas;
        [SerializeField] MobSetData m_bossMobSet;

        [SerializeField] GameObject m_iconGroupLayout;
        [SerializeField] GameObject m_iconPrefab;

        [SerializeField] AreaData[] m_areaData;
        [SerializeField] Transform m_canditiateGroup;

        [SerializeField] GameObject m_bossArea;
        [SerializeField] AreaData m_bossData;
        [SerializeField] AreaData m_nowArea;

        [SerializeField] GameObject m_nearbyStandardA;
        [SerializeField] GameObject m_nearbyStandardB;

        [SerializeField][Range(0, 50)] int m_bossOpenMinimum = 5;
        [SerializeField][Range(0, 10)] int m_marketTurnLimit = 3;

        List<GameObject> m_areas;

        float NearbyDistanceStandard
        {
            get => (m_nearbyStandardA.transform.position -
                    m_nearbyStandardB.transform.position).magnitude;
        }

        void Start()
        {
            BuildAreas();

            // MapData must not to be null at this point
            if (MapData is null)
            {
                Debug.LogError("MapData is null!");
                return;
            }

            m_bossArea.GetComponent<Button>().onClick.AddListener(() =>
            {
                MapData = null;
                BattleAreaOnClick(true);
            });

            // Add common onClick event to areas
            for (int i = 0; i < m_areas.Count; i++)
            {
                // Use this variable to resolve closure problem
                string t_areaName = MapData.AreaDatas[i].areaName;
                int t_marketTurnLimit = m_marketTurnLimit;
                int t_index = i;

                m_areas[i].GetComponent<Button>().onClick.AddListener(() =>
                {
                    MapData.AreaVisitCount++;
                    MapData.AreaIndex = t_index;

                    switch (t_areaName)
                    {
                        // case "Random":
                        //     SceneManager.LoadScene("EventScene");
                        //     break;
                        case "Market":
                            MapData.MarketDisableTurn = t_marketTurnLimit;
                            Debug.Log($"what? {t_marketTurnLimit}");
                            SceneManager.LoadScene("ShopTestScene");
                            break;
                        case "Battle":
                            BattleAreaOnClick();
                            break;
                    }
                });
            }
            /*** TMP FIX BY JS_LEE ***/
            // Remove market's onclick if turn has not yet passed after using the market
            if (MapData.MarketDisableTurn > 0)
            {
                for (int i = 0; i < m_areas.Count; i++)
                {
                    if (MapData.AreaDatas[i].areaName != "Market")
                        continue;
                    Image areaImage = m_areas[i].GetComponent<Image>();
                    Button areaButton = m_areas[i].GetComponent<Button>();
                    areaImage.color = new Vector4(0,0,0,0); // just Make store icon transparent
                    // areaImage.sprite = m_defaultAreaSprite;
                    areaButton.onClick.RemoveAllListeners();

                    /*
                        // Use this variable to resolve closure problem
                        int t_index = i;
                        areaButton.onClick.AddListener(() =>
                        {
                            MapData.AreaVisitCount++;
                            MapData.AreaIndex = t_index;
                            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                        });
                    */
                }
                MapData.MarketDisableTurn--;
            }
            /*** TMP FIX BY JS_LEE ***/


            // Disable all areas
            m_bossArea.SetActive(false);
            foreach (var area in m_areas)
                area.SetActive(false);

            // Activate now and near areas
            GameObject nowArea = m_areas[MapData.AreaIndex];

            ActivateNearAreas(nowArea);
            nowArea.SetActive(true);

            // Remove now area's onclick
            nowArea.GetComponent<Button>().onClick.RemoveAllListeners();

            // Change now area's icon
            nowArea.GetComponent<Image>().sprite = m_nowArea.sprite;
        }

        void BuildAreas()
        {
            var builder = new AreaBuilder();

            if (!builder.TryUseIconPrefab(m_iconPrefab))
            {
                Debug.LogError("Icon prefab doesn't have chuld component Image or TMP_Text or both.");
                return;
            }

            builder.UseIconParent(m_iconGroupLayout.transform);

            if (!m_bossArea.TryGetComponent(out Button bossButton))
            {
                Debug.LogError($"Boss area does not have button component");
                return;
            }

            builder.UseBossTarget(bossButton, m_bossData);

            foreach (Transform canditiateTransfrom in m_canditiateGroup)
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

            if (MapData is null)
            {
                MapData = new();

                foreach (AreaData areaData in m_areaData)
                    builder.AddAreaData(areaData);

                builder.UseAreaPickStrategy(areaDataCount =>
                {
                    // THIS IS TEMP!
                    return Random.Range(0, areaDataCount);
                });
            }
            else
                builder.AddAreaDatas(MapData.AreaDatas);

            builder.AddOnlyIconAreaData(m_nowArea);
            builder.Build();

            MapData.AreaDatas = builder.AreaData.ToArray();
            m_areas = builder.AreaResult;

            if (MapData.AreaIndex == -1)
                MapData.AreaIndex = Random.Range(0, m_areas.Count);
        }

        void BattleAreaOnClick(bool isBoss = false)
        {
            var sceneParameter = SceneParamter.Instance();

            sceneParameter.IsBoss = isBoss;
            sceneParameter.EnemyReinforce = isBoss ? 0 : MapData.AreaVisitCount;
            sceneParameter.MapStage += isBoss ? 1 : 0;

            sceneParameter.MobSet = isBoss ? m_bossMobSet :
                m_mobSetDatas[sceneParameter.EnemyReinforce % m_mobSetDatas.Length];

            SceneManager.LoadScene("CombineScene");
        }

        void ActivateNearAreas(GameObject target)
        {
            Vector3 areaPos = target.transform.position;

            for (int i = 0; i < m_areas.Count; i++)
            {
                GameObject other = m_areas[i];

                if (target == other)
                    continue;

                Vector3 otherPos = other.transform.position;

                // Other area is out of range
                if ((areaPos - otherPos).magnitude >= NearbyDistanceStandard)
                    continue;

                other.SetActive(true);
            }

            Vector3 bossPos = m_bossArea.transform.position;

            // Boss area is in range
            if (MapData.AreaVisitCount >= m_bossOpenMinimum &&
                (areaPos - bossPos).magnitude <= NearbyDistanceStandard)
                m_bossArea.SetActive(true);
        }
    }
}
