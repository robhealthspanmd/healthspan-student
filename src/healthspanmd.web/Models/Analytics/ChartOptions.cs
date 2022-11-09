using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace healthspanmd.web.Models.Analytics
{
    public class ChartOptions
    {
        [JsonProperty("chart")]
        public ChartOptions_Chart Chart { get; set; }

        [JsonProperty("dataLabels")]
        public ChartOptions_DataLabels DataLabels { get; set; }

        [JsonProperty("colors")]
        public string[] Colors { get; set; }


        [JsonProperty("markers")]
        public ChartOptions_Markers Markers { get; set; }

        [JsonProperty("series")]
        public ICollection<ChartOptions_Series> Series { get; set; }

        [JsonProperty("xaxis")]
        public ChartOptions_XAxis XAxis { get; set; }

        [JsonProperty("stroke")]
        public ChartOptions_Stroke Stroke { get; set; }

        [JsonProperty("annotations")]
        public ChartOptions_Annotations Annotations { get; set; }
    }

    public class ChartOptions_Chart
    {
        [JsonProperty("type")]
        public string ChartType { get; set; }

        [JsonProperty("toolbar")]
        public ChartOptions_Chart_Toolbar Toolbar { get; set; }
    }

    public class PlainJsonStringConverter : Newtonsoft.Json.JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.Value;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue((string)value);
        }
    }

    public class ChartOptions_DataLabels
    {
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("formatter")]
        [Newtonsoft.Json.JsonConverter(typeof(PlainJsonStringConverter))]
        public string Formatter { get; set; }
    }

    public class ChartOptions_Chart_Toolbar
    {
        [JsonProperty("show")]
        public bool Show { get; set; }
    }

    public class ChartOptions_Markers
    {
        [JsonProperty("size")]
        public int Size { get; set; }
    }

    public class ChartOptions_Series
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("data")]
        public ChartOptions_SeriesData[] Data { get; set; }


    }

    public class ChartOptions_SeriesData
    {
        [JsonProperty("x")]
        public double X { get; set; }

        [JsonProperty("y")]
        public double? Y { get; set; }
    }

    public class ChartOptions_XAxis
    {
        //[JsonProperty("categories")]
        //public string[] Categories { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class ChartOptions_Stroke
    {
        [JsonProperty("curve")]
        public string Curve { get; set; }
    }

    public class ChartOptions_Annotations
    {
        [JsonProperty("yaxis")]
        public ChartOptions_Annotation_YAxis[] YAxis { get; set; }
       
    }

    public class ChartOptions_Annotation_YAxis
    {
        [JsonProperty("y")]
        public double Y { get; set; }

        [JsonProperty("y2")]
        public double Y2 { get; set; }

        [JsonProperty("borderColor")]
        public string BorderColor { get; set; }

        [JsonProperty("fillColor")]
        public string FillColor { get; set; }

        [JsonProperty("label")]
        public ChartOptions_Label Label { get; set; }

        [JsonProperty("opacity")]
        public double Opacity { get; set; }

    }

    public class ChartOptions_Label
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("borderColor")]
        public string BorderColor { get; set; }

        [JsonProperty("style")]
        public ChartOptions_Style Style { get; set; }
    }

    public class ChartOptions_Style
    {
        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("background")]
        public string Background { get; set; }
    }

}
