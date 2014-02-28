using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Data.Entity.Spatial;

namespace ScrollerIssue.Entities
{
    /* Hardware unit.
    * A class to track all of our hardware units.
    * @unit_id:  Arbitrary unique unit ID (with regards to our database and internal system).
    * @c_number:  The Adaptrum internal C-Number associated with the unit.
    * @serial_number:  The more global serial number across all of the units.
    * @build_version:  The build version for the specific unit.
    * @log_file_name:  An associated log file by its name.
    * @geo_latitude:  The geographical latitude of the unit (currently).
    * @geo_longitude:  The geographical longitude of the unit (currently).
    * @deployment_status:  The current deployment status of the unit (enumerable).
    * @short_description:  Any short description of the unit, including a nickname.
    * @notes:  A given set of notes related to the unit.
    * @contact_id:  An associated contact with the unit, referencing the Contacts data.
    * @reg_contact_id:  An associated contact that has registration of the unit.
    * @network_id:  A network that the unit belongs to (arbitrary ID internal to our system and Networks data).
    */
    public class Unit
    {
        [Key]
        public int unit_id { get; set; }
        public string c_number { get; set; }
        public string serial_number { get; set; }
        public string ip_address { get; set; }
        public string build_version { get; set; }
        public string log_file_name { get; set; }
        public DbGeography geo_location { get; set; }
        public string location_at_address { get; set; }
        public int deployment_status { get; set; }
        public string short_description { get; set; }
        public string notes { get; set; }
        public int contact_id { get; set; }
        public int reg_contact_id { get; set; }
        public int network_id { get; set; }

        public Unit()
        {

        }

        public void cacheGeoCoordinate()
        {

        }
    }
}