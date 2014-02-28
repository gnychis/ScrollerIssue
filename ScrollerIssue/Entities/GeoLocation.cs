using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace ScrollerIssue.Entities
{
    public class GeoLocation
    {
        [Key]
        public int geo_id { get; set; }
        public DbGeography geo_location { get; set; }
        public int valid { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string state_abbrv { get; set; }
        public string state { get; set; }
        public string county { get; set; }
        public string country_abbrv { get; set; }
        public string country { get; set; }
        public int zipcode { get; set; }

        public GeoLocation()
        {

        }

        public GeoLocation(DbGeography geoL, JToken address_components)
        {
            geo_location = geoL;
            setAddressComponents(address_components);
        }

        public GeoLocation(DbGeography geoL, JObject googleResponse)
        {
            geo_location = geoL;

            if ((string)googleResponse.SelectToken("status") != "OK")
            {
                valid = 0;
                return;
            }

            // Initialize a few values
            valid = 1;
            street = "";

            // Get the first result and parse through it
            setAddressComponents(googleResponse["results"][0]["address_components"]);
        }

        private void setAddressComponents(JToken address_components)
        {
            foreach (var x in address_components)
            {

                // This is the street number
                if (x["types"].Children().Values<string>().Contains("street_number"))
                {
                    street = (string)x["long_name"] + " " + street;
                }

                // This is the name of the the street, post-fixed to the street number
                if (x["types"].Children().Values<string>().Contains("route"))
                {
                    street = street + (string)x["long_name"];
                }

                // Locality stores the name of the city (e.g., San Jose)
                if (x["types"].Children().Values<string>().Contains("locality"))
                {
                    city = (string)x["long_name"];
                }

                // Find the state, stored in "administrative_area_level_1" in the response
                if (x["types"].Children().Values<string>().Contains("administrative_area_level_1"))
                {
                    state_abbrv = (string)x["short_name"];
                    state = (string)x["long_name"];
                }

                // Store the county (NOT country), e.g., Santa Clara
                if (x["types"].Children().Values<string>().Contains("administrative_area_level_2"))
                {
                    county = (string)x["long_name"];
                }

                // Store the country, both the long and short version of it (e.g., United States vs. US)
                if (x["types"].Children().Values<string>().Contains("country"))
                {
                    country_abbrv = (string)x["short_name"];
                    if (country_abbrv == "US")
                        country_abbrv = "USA";
                    country = (string)x["long_name"];
                }

                // Finally, store the zipcode of the area
                if (x["types"].Children().Values<string>().Contains("postal_code"))
                {
                    zipcode = (int)x["long_name"];
                }
            }
        }
    }
}