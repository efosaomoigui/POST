using System.Collections.Generic;

namespace GIGLS.Core.DTO.Partnership
{
    public class Distance
    {
        public string text { get; set; }
        public int value { get; set; }
    }

    public class Duration
    {
        public string text { get; set; }
        public int value { get; set; }
    }

    public class Element
    {
        public Distance distance { get; set; }
        public Duration duration { get; set; }
        public string status { get; set; }
    }

    public class Row
    {
        public List<Element> elements { get; set; }
    }

    public class RootObject
    {
        public List<string> destination_addresses { get; set; }
        public List<string> origin_addresses { get; set; }
        public List<Row> rows { get; set; }
        public string status { get; set; }
    }

    public class GeoCodeAddressResponse
    {
        public List<results> results { get; set; }
        public string status { get; set; }
    }

    
    public class results
    {
        public List<address_components> address_components { get; set; }
        public string formatted_address { get; set; }
        public geometry geometry { get; set; }
        public string place_id { get; set; }
    }

    public class address_components
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public List<string> types { get; set; }
    }

    public class geometry
    {
        public location location { get; set; }

    }

    public class location
    {
        public double lat { get; set; }
        public double lng { get; set; }

    }





}