using ScrollerIssue.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScrollerIssue.Controllers
{
    public class HomeController : Controller
    {
        PacificRepository pacificRepo;

        public HomeController()
        {
            pacificRepo = new EFPacificRepository();
        }

        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }

        // This is called dynamically from the data table.  Since "Scroller" is enabled,
        // it will request new data as the user scrolls through the table and dynamically
        // load it.  That way, it implements a sort of "infinite" scrolling and is scalable. 
        public ActionResult UnitListHandler(jQueryDataTableParamModel param)
        {
            var currUnitResults = pacificRepo.Units.OrderBy(p => p.c_number).Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = pacificRepo.Units.Count(),
                iTotalDisplayRecords = pacificRepo.Units.Count(),
                aaData = FormatUnitData(currUnitResults)
            },
            JsonRequestBehavior.AllowGet);
        }

        private ArrayList FormatUnitData(IEnumerable<Unit> currUnitResults)
        {
            ArrayList myA = new ArrayList();

            foreach (var u in currUnitResults)
            {
                var geoRes = pacificRepo.GeoLocations.FirstOrDefault(g => g.geo_location.Latitude == u.geo_location.Latitude && g.geo_location.Longitude == u.geo_location.Longitude);

                myA.Add(new string[]{
                    u.unit_id.ToString(),
                    u.c_number,
                    u.serial_number,
                    u.location_at_address,
                    u.ip_address,
                    "<a href=\"https://adaptrum.sharepoint.com/_layouts/15/start.aspx#/Shared%20Documents/Forms/AllItems.aspx?RootFolder=%2FShared%20Documents%2FJ1_BUILD_DOCS&FolderCTID=0x01200061C0D3105A92014DA4636DDDFC0577D4&View={435D0C25-0AD1-404D-8B07-9C2CE645102E}\">" + u.build_version + "</a>",
                    (geoRes==null) ? "Unknown" : geoRes.city + ", " + geoRes.state_abbrv,
                    u.location_at_address
                });
            }

            return myA;
        }

	}
}