using Newtonsoft.Json;
using SharedLibrary.Http;
using SharedLibrary.Model.FuncModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Helper
{
    internal class WeatherHelper
    {
        public static async Task<WeatherModel> GetWeatherResult(string cityCode)
        {

            string response = HttpApi.HttpGet("http://www.nmc.cn/rest/weather?stationid=" + $"{cityCode}");
            var jsonSetting = new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Include };
            jsonSetting.MissingMemberHandling = MissingMemberHandling.Ignore;
            jsonSetting.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
            jsonSetting.NullValueHandling = NullValueHandling.Ignore;
            jsonSetting.DefaultValueHandling = DefaultValueHandling.Include;
            jsonSetting.ObjectCreationHandling = ObjectCreationHandling.Auto;
            jsonSetting.TypeNameHandling = TypeNameHandling.Auto;
            WeatherJsonModel w = JsonConvert.DeserializeObject<WeatherJsonModel>(response, jsonSetting);

            var currentTime = "无数据";
            var rain = "无数据";
            var realFeelst = "无数据";
            var currentTemp = "无数据";
            var relativeHumidity = "无数据";
            var airQuality = "无数据";
            var weather = "无数据";
            var wind = new Windy() { WindDirect = "未知", WindSpeed = "未知" };
            try
            {
                if (w.data.real.publish_time != null)
                {
                    currentTime = w.data.real.publish_time;
                }
                if (w.data.real.weather.rain.ToString() != null)
                {
                    rain = w.data.real.weather.rain.ToString();
                }
                if (w.data.real.weather.feelst.ToString() != null)
                {
                    realFeelst = w.data.real.weather.feelst.ToString();
                }
                if (w.data.real.weather.temperature.ToString() != null)
                {
                    currentTemp = w.data.real.weather.temperature.ToString();
                }
                if (w.data.real.weather.humidity.ToString() != null)
                {
                    relativeHumidity = w.data.real.weather.humidity.ToString();
                }
                if (w.data.air != null)
                {
                    airQuality = w.data.air.aqi.ToString();
                }
                if (w.data.predict.detail[0] != null)
                {

                    weather = WeatherChoose(w.data.predict.detail[0].day.weather.info, w.data.predict.detail[0].night.weather.info);
                }
                if (w.data.real.wind.direct != null && w.data.real.wind.power != null)
                {
                    wind = new Windy() { WindDirect = w.data.real.wind.direct, WindSpeed = w.data.real.wind.power };
                }






            }
            catch (Exception e)
            {

            }
            var result = new WeatherModel
            {
                CurrentTime = currentTime,
                Rain = rain,
                RealFeelst = realFeelst,
                CurrentTemp = currentTemp,
                RelativeHumidity = relativeHumidity,
                AirQuality = airQuality,
                Weather = weather,
                Wind = wind
            };
            return result;
        }
        public static string AirQuality(string AQI)
        {
            var aq = "";
            if (!AQI.Contains("无数据") && AQI != (""))
            {
                int aqi = int.Parse(AQI);

                if (aqi <= 50 && aqi >= 0)
                {
                    aq = "优";
                }
                else if (aqi <= 100 && aqi >= 51)
                {
                    aq = "良";
                }
                else if (aqi <= 150 && aqi >= 101)
                {
                    aq = "轻度污染";
                }
                else if (aqi <= 200 && aqi >= 151)
                {
                    aq = "中度污染";
                }
                else if (aqi <= 300 && aqi >= 200)
                {
                    aq = "重度污染";
                }
                else if (aqi > 300)
                {
                    aq = "严重污染";
                }
            }
            else
            {
                aq = "无数据";
            }


            return aq;
        }

        private static string WeatherChoose(string day, string night)
        {
            string weather = "";

            if (day.Contains(night))
            {
                weather = day;
            }
            else
            {
                weather = day + "转" + night;
            }

            return weather;
        }
    }
}
