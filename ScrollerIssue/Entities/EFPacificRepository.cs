using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace ScrollerIssue.Entities
{
    public class EFPacificRepository : PacificRepository
    {
        private EFDbContext context = new EFDbContext();

        public IQueryable<Unit> Units { get { return context.Units; } }
        public IQueryable<GeoLocation> GeoLocations { get { return context.GeoLocations; } }

        public void SaveUnit(Unit u, bool preserve_metadata)
        {
            if (u.unit_id == 0)
            {
                context.Units.Add(u);
            }
            else
            {
                if (preserve_metadata == true)
                {
                    var unitInDb = context.Units.Single(a => a.unit_id == u.unit_id);
                    u.geo_location = unitInDb.geo_location;
                    u.notes = unitInDb.notes;
                    u.deployment_status = unitInDb.deployment_status;
                    u.short_description = unitInDb.short_description;
                    u.notes = unitInDb.notes;
                    u.contact_id = unitInDb.contact_id;
                    u.reg_contact_id = unitInDb.reg_contact_id;
                    u.network_id = unitInDb.network_id;
                }

                context.Entry(u).State = System.Data.Entity.EntityState.Modified;
            }

            context.SaveChanges();
        }

        public void SaveCoordinate(GeoLocation g)
        {
            // Check to see if we have a geocoordinate for the unit
            GeoLocation cachedCoord = context.GeoLocations.FirstOrDefault(g2 => (g2.geo_location.Latitude == g.geo_location.Latitude) && (g2.geo_location.Longitude == g.geo_location.Longitude));

            if (cachedCoord == null)
            {
                context.GeoLocations.Add(g);
            }
            else
            {
                context.Entry(g).State = System.Data.Entity.EntityState.Modified;
            }

            context.SaveChanges();
        }

        /* Cache geolocation of the unit.
         * If a entry does not exist in our cache for the given geolocation, get one and store it.
         * @u: The unit to check the location of based on the geo coordinates.
         */
        public void CacheLocation(Unit u)
        {
            if (u.geo_location == null)
                return;

            // Use the Google geolocation information from the coordinates
            JObject googleAddressInfo = CoordinatesToJSON((double)u.geo_location.Latitude, (double)u.geo_location.Longitude);

            // Translate to our version of the coordinate
            GeoLocation gc = new GeoLocation(u.geo_location, googleAddressInfo);

            this.SaveCoordinate(gc);
        }

        static public JObject CoordinatesToJSON(double longitude, double latitude)
        {
            // Query the Google service to get location information
            WebRequest request = WebRequest.Create("http://maps.googleapis.com/maps/api/geocode/json?latlng=" + longitude + "," + latitude + "&sensor=true");
            WebResponse response = request.GetResponse();
            Stream data = response.GetResponseStream();
            string jsonResponse = String.Empty;
            using (StreamReader sr = new StreamReader(data))
            {
                jsonResponse = sr.ReadToEnd();
            }

            return JObject.Parse(jsonResponse);
        }
    }
}