using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace GameMap
{
    public class MapStageData
    {
        public AreaData[] AreaDatas;
        public int AreaVisitCount;
        public int AreaIndex = -1;
        public int MarketDisableTurn;

        public string fromJson(JObject json)
        {
            List<AreaData> temp = new();
            foreach (JObject t_areaData in (JArray)json["area_data"])
                temp.Add(AreaData.GetAreaDataByName((string)t_areaData));

            AreaVisitCount = (int)json["area_visit_count"];
            AreaIndex = (int)json["area_index"];
            MarketDisableTurn = (int)json["market_disable_turn"];

            return string.Empty;
        }

        public JObject toJson()
        {
            JObject ret = new();

            JArray temp = new();

            if (AreaDatas != null)
                foreach (AreaData areaData in AreaDatas)
                    temp.Add(areaData.areaName);
            ret["area_data"] = temp;

            ret["area_visit_count"] = AreaVisitCount;
            ret["area_index"] = AreaIndex;
            ret["market_disable_turn"] = MarketDisableTurn;

            return ret;
        }
    }
}
