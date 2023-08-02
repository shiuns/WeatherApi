using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatgpt
{
    public class Parameter
    {
        public string ParameterName { get; set; }
        public string ParameterValue { get; set; }
        public string parameterUnit { get; set; }

    }

    public class Time
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public Parameter Parameter { get; set; }
    }

    public class WeatherElement
    {
        public string ElementName { get; set; }
        public List<Time> Time { get; set; }
    }

    public class Location
    {
        public string LocationName { get; set; }
        public List<WeatherElement> WeatherElement { get; set; }
    }

    public class records
    {
        public string DatasetDescription { get; set; }
        public List<Location> Location { get; set; }

        public override string ToString()
        {
            StringBuilder weatherall = new StringBuilder();
            foreach (var item in Location)
            {
                if (item != null && Location.Count > 0)
                {
                    var LocationName = item.LocationName;
                    var ParameterName = item.WeatherElement[0].Time[0].Parameter.ParameterName;
                    var MinT = item.WeatherElement[2].Time[0].Parameter.ParameterName;
                    var MaxT = item.WeatherElement[4].Time[0].Parameter.ParameterName;
                    var PoP = item.WeatherElement[1].Time[0].Parameter.ParameterName;

                    weatherall.AppendLine($"區域:{LocationName} 天氣:{ParameterName} 溫度:{MinT}℃~{MaxT}℃ 降雨機率:{PoP}%");

                }
            }
            return weatherall.ToString();
        }
    }
}
