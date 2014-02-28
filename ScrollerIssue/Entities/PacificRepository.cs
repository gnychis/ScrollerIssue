using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrollerIssue.Entities
{
    public interface PacificRepository
    {
        IQueryable<GeoLocation> GeoLocations { get; }
        void SaveCoordinate(GeoLocation g);
        void CacheLocation(Unit u);

        IQueryable<Unit> Units { get; }
        void SaveUnit(Unit u, bool preserve_data);
    }
}