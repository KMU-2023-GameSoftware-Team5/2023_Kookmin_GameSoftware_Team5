using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace GameMap
{
    public class MapStageData
    {
        public bool NeedInit = true;

        public AreaData[] AreaDatas;
        public int AreaVisitCount;
        public int AreaIndex;

        public string fromJson(JObject json)
        {
            List<AreaData> temp = new();
            foreach (JObject t_areaData in (JArray)json["area_data"])
                temp.Add(AreaData.GetAreaDataByName((string)t_areaData));

            NeedInit = (bool)json["need_init"];
            AreaVisitCount = (int)json["area_visit_count"];
            AreaIndex = (int)json["area_index"];

            return string.Empty;
        }

        public JObject toJson()
        {
            JObject ret = new();

            JArray temp = new();
            foreach (AreaData areaData in AreaDatas)
                temp.Add(areaData.areaName);
            ret["area_data"] = temp;

            ret["need_init"] = NeedInit;
            ret["area_visit_count"] = AreaVisitCount;
            ret["area_index"] = AreaIndex;

            return ret;
        }
    }
}
