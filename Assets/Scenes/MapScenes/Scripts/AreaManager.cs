using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameMap
{
    public class AreaManager : MonoBehaviour
    {
        static MapStageData MapData = new();

        [SerializeField] MobSetData[] m_mobSetDatas;
        [SerializeField] MobSetData m_bossMobSet;

        [SerializeField] GameObject m_iconGroupLayout;
        [SerializeField] GameObject m_iconPrefab;

        [SerializeField] AreaData[] m_areaData;
        [SerializeField] Transform m_canditiateGroup;

        [SerializeField] GameObject m_bossArea;
        [SerializeField] AreaData m_bossData;

        [SerializeField] GameObject m_nearbyStandardA;
        [SerializeField] GameObject m_nearbyStandardB;

        [SerializeField][Range(0, 50)] int m_bossOpenMinimum = 5;

        List<GameObject> m_areas;

        float NearbyDistanceStandard
        {
            get => (m_nearbyStandardA.transform.position -
                    m_nearbyStandardB.transform.position).magnitude;
        }

        void Start()
        {
            BuildAreas();

            m_bossArea.GetComponent<Button>().onClick.AddListener(() =>
            {
                var sceneParameter = SceneParamter.Instance();

                sceneParameter.EnemyReinforce = 0;
                sceneParameter.MapStage++;

                MapData = new();

                // THIS IS TEMP!
                SceneParamter.Instance().MobSet = m_bossMobSet;
                SceneManager.LoadScene("CombineScene");
            });

            // Add common onClick event to areas
            for (int i = 0; i < m_areas.Count; i++)
            {
                // Use this variable to resolve closure problem
                int t_index = i;

                m_areas[i].GetComponent<Button>().onClick.AddListener(() =>
                {
                    ChangeMobSetData();

                    MapData.AreaVisitCount++;
                    MapData.AreaIndex = t_index;

                    // THIS IS TEMP!
                    SceneParamter.Instance().EnemyReinforce = MapData.AreaVisitCount;
                    SceneManager.LoadScene("CombineScene");
                });
            }

            // Disable all areas
            m_bossArea.SetActive(false);
            foreach (var area in m_areas)
                area.SetActive(false);

            // Get index of start(now) area
            int startAreaIndex = MapData.NeedInit ?
                Random.Range(0, m_areas.Count) : MapData.AreaIndex;

            // Activate now and near areas
            ActivateNearAreas(m_areas[startAreaIndex]);
            m_areas[startAreaIndex].SetActive(true);
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

            if (MapData.NeedInit)
            {
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

            builder.Build();

            MapData.AreaDatas = builder.AreaData.ToArray();
            m_areas = builder.AreaResult;
        }

        void ChangeMobSetData()
        {
            // THIS IS TEMP!
            int index = MapData.AreaVisitCount % m_mobSetDatas.Length;
            SceneParamter.Instance().MobSet = m_mobSetDatas[index];
        }

        void ActivateNearAreas(GameObject target)
        {
            Vector3 areaPos = target.transform.position;

            foreach (GameObject other in m_areas)
            {
                if (target == other)
                    continue;

                Vector3 otherPos = other.transform.position;

                // Other area is out of range
                if ((areaPos - otherPos).magnitude >= NearbyDistanceStandard)
                    continue;

                other.SetActive(true);
            }

            Vector3 bossPos = m_bossArea.transform.position;

            // Other area is in range
            if (MapData.AreaVisitCount >= m_bossOpenMinimum &&
                (areaPos - bossPos).magnitude <= NearbyDistanceStandard)
                m_bossArea.SetActive(true);
        }
    }
}
