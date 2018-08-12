var flightRoutes =

 {
     "type": "FeatureCollection",

     "features": [

        {
            "properties":{"name": "Route4", "abbrev": "RDU", "from": "Raleigh", "to": "Chennai", "departure": "RDU", "arrival": "MAA", "Route1_Stop1": "ATL", "Route1_Stop2": "DXB"}, "type": "Feature", "geometry": { "type": "LineString", "coordinates": [[80.163775973154515, 12.982530166915435], [55.35407691722429, 25.252565593818161]] }

        },

        {
            "properties":{"name": "Route5", "abbrev": "RDU", "from": "Raleigh", "to": "Chennai", "departure": "RDU", "arrival": "MAA", "Route1_Stop1": "ATL", "Route1_Stop2": "DXB"}, "type": "Feature", "geometry": { "type": "LineString", "coordinates": [[55.35407691722429, 25.252565593818161], [-84.425397433604729, 33.640529080735213]] }

        },
        {
            "properties":{"name": "Route6", "abbrev": "RDU", "from": "Raleigh", "to": "Chennai", "departure": "RDU", "arrival": "MAA", "Route1_Stop1": "ATL", "Route1_Stop2": "DXB"}, "type": "Feature", "geometry": { "type": "LineString", "coordinates": [[-84.425397433604729, 33.640529080735213], [-78.791381400675078, 35.875232345225498]] }

        },

        {
            "properties":{"name": "Route7", "abbrev": "RDU", "from": "Raleigh", "to": "Chennai", "departure": "RDU", "arrival": "MAA", "Route1_Stop1": "JFK", "Route1_Stop2": "DXB"}, "type": "Feature", "geometry": { "type": "LineString", "coordinates": [[80.163775973154515, 12.982530166915435], [55.35407691722429, 25.252565593818161]] }

        },

        {
            "properties":{"name": "Route8", "abbrev": "RDU", "from": "Raleigh", "to": "Chennai", "departure": "RDU", "arrival": "MAA", "Route1_Stop1": "JFK", "Route1_Stop2": "DXB"}, "type": "Feature", "geometry": { "type": "LineString", "coordinates": [[55.35407691722429, 25.252565593818161], [-73.786326860929549, 40.645959558408144]] }

        },

        {
            "properties":{"name": "Route9", "abbrev": "RDU", "from": "Raleigh", "to": "Chennai", "departure": "RDU", "arrival": "MAA", "Route1_Stop1": "JFK", "Route1_Stop2": "DXB"}, "type": "Feature", "geometry": { "type": "LineString", "coordinates": [[-73.786326860929549, 40.645959558408144], [-78.791381400675078, 35.875232345225498]] }

        },

        {
            "properties":{"name": "Route10", "abbrev": "MAA", "from": "Chennai", "to": "Raleigh", "departure": "MAA", "arrival": "RDU", "Route1_Stop1": "FRA", "Route1_Stop2": "IAD"}, "type": "Feature", "geometry": { "type": "LineString", "coordinates": [[80.163775973154515, 12.982530166915435], [8.571822869076076, 50.05067708952074]] }

        },
        {
            "properties":{"name": "Route11", "abbrev": "MAA", "from": "Chennai", "to": "Raleigh", "departure": "MAA", "arrival": "RDU", "Route1_Stop1": "FRA", "Route1_Stop2": "IAD"}, "type": "Feature", "geometry": { "type": "LineString", "coordinates": [[8.571822869076076, 50.05067708952074], [-77.447792576920619, 38.95277403795302]] }

        },
        {
            "properties": { "name": "Route12", "abbrev": "MAA", "from": "Chennai", "to": "Raleigh", "departure": "MAA", "arrival": "RDU", "Route1_Stop1": "FRA", "Route1_Stop2": "IAD" }, "type": "Feature", "geometry": { "type": "LineString", "coordinates": [ [-77.447792576920619, 38.95277403795302], [-78.791381400675078, 35.875232345225498]] }

        }

     ]
 }

var flightRoutes_data =
[
        { "name": "Route4", "abbrev": "RDU", "from": "Raleigh", "to": "Chennai", "departure": "RDU", "arrival": "MAA", "Route1_Stop1": "ATL", "Route1_Stop2": "DXB" },
        { "name": "Route5", "abbrev": "RDU", "from": "Raleigh", "to": "Chennai", "departure": "RDU", "arrival": "MAA", "Route1_Stop1": "ATL", "Route1_Stop2": "DXB" },
        { "name": "Route6", "abbrev": "RDU", "from": "Raleigh", "to": "Chennai", "departure": "RDU", "arrival": "MAA", "Route1_Stop1": "ATL", "Route1_Stop2": "DXB" },
        { "name": "Route7", "abbrev": "RDU", "from": "Raleigh", "to": "Chennai", "departure": "RDU", "arrival": "MAA", "Route1_Stop1": "JFK", "Route1_Stop2": "DXB" },
        { "name": "Route8", "abbrev": "RDU", "from": "Raleigh", "to": "Chennai", "departure": "RDU", "arrival": "MAA", "Route1_Stop1": "JFK", "Route1_Stop2": "DXB" },
        { "name": "Route9", "abbrev": "RDU", "from": "Raleigh", "to": "Chennai", "departure": "RDU", "arrival": "MAA", "Route1_Stop1": "JFK", "Route1_Stop2": "DXB" },
        { "name": "Route10", "abbrev": "MAA", "from": "Chennai", "to": "Raleigh", "departure": "MAA", "arrival": "RDU", "Route1_Stop1": "FRA", "Route1_Stop2": "IAD" },
        { "name": "Route11", "abbrev": "MAA", "from": "Chennai", "to": "Raleigh", "departure": "MAA", "arrival": "RDU", "Route1_Stop1": "FRA", "Route1_Stop2": "IAD" },
        { "name": "Route12", "abbrev": "MAA", "from": "Chennai", "to": "Raleigh", "departure": "MAA", "arrival": "RDU", "Route1_Stop1": "FRA", "Route1_Stop2": "IAD" }

     ]

var intermediatestops1 =

 {
     "type": "FeatureCollection",

     "features": [
         
        {
            "properties": { "name": "Dulles Int'l Airport", "from": "Raleigh", "to": "Chennai", "Intermediate": "Washington", "abbrev": "IAD", "departure": "RDU", "arrival": "MAA", "Route1_Stop1": "IAD", "Route1_Stop2": "FRA" }, "type": "Feature", "geometry": { "type": "Point", "coordinates": [-77.447792576920619, 38.95277403795302] }

        },
        {
            "properties": { "name": "Frankfurt Int'l Airport", "from": "Chennai", "to": "Raleigh", "Intermediate": "Frankfurt", "abbrev": "FRA", "departure": "MAA", "arrival": "RDU", "Route1_Stop1": "FRA", "Route1_Stop2": "IAD" }, "type": "Feature", "geometry": { "type": "Point", "coordinates": [8.571822869076076, 50.05067708952074] }

        },

        {
            "properties": { "name": "Atlanta Int'l Airport", "from": "Raleigh", "to": "Chennai", "Intermediate": "Atlanta", "abbrev": "ATL", "departure": "RDU", "arrival": "MAA", "Route1_Stop1": "ATL", "Route1_Stop2": "DXB" }, "type": "Feature", "geometry": { "type": "Point", "coordinates": [-84.425397433604729, 33.640529080735213] }

        },

        {
            "properties": { "name": "John F Kennedy Int'l Airport", "from": "Raleigh", "to": "Chennai", "Intermediate": "Newyork", "abbrev": "JFK", "departure": "RDU", "arrival": "MAA", "Route1_Stop1": "JFK", "Route1_Stop2": "DXB" }, "type": "Feature", "geometry": { "type": "Point", "coordinates": [-73.786326860929549, 40.645959558408144] }

        }

     ]
 }

var intermediatestops2 =

 {
     "type": "FeatureCollection",

     "features": [

        {
            "properties": { "name": "Dubai Int'l Airport", "from": "Chennai", "to": "Raleigh", "Intermediate": "Dubai", "abbrev": "DXB", "departure": "MAA", "arrival": "RDU", "Route1_Stop1": "DXB", "Route1_Stop2": "JFK", "Route2_Stop1": "DXB", "Route2_Stop2": "ATL" }, "type": "Feature", "geometry": { "type": "Point", "coordinates": [55.35407691722429, 25.252565593818161] }

        }

     ]
 }

var intermediatestops1_data =
 [
        {"name": "Dulles Int'l Airport", "from": "Raleigh", "to": "Chennai", "Intermediate": "Washington", "abbrev": "IAD", "departure": "RDU", "arrival": "MAA", "Route1_Stop1": "IAD", "Route1_Stop2": "FRA" },
        {"name": "Frankfurt Int'l Airport", "from": "Chennai", "to": "Raleigh", "Intermediate": "Frankfurt", "abbrev": "FRA", "departure": "MAA", "arrival": "RDU", "Route1_Stop1": "FRA", "Route1_Stop2": "IAD" },
        { "name": "Atlanta Int'l Airport", "from": "Raleigh", "to": "Chennai", "Intermediate": "Atlanta", "abbrev": "ATL", "departure": "RDU", "arrival": "MAA", "Route1_Stop1": "ATL", "Route1_Stop2": "DXB" },
        { "name": "John F Kennedy Int'l Airport", "from": "Raleigh", "to": "Chennai", "Intermediate": "Newyork", "abbrev": "JFK", "departure": "RDU", "arrival": "MAA", "Route1_Stop1": "JFK", "Route1_Stop2": "DXB" }
 ]

var intermediatestops2_data =
 [
        { "name": "Dubai Int'l Airport", "from": "Chennai", "to": "Raleigh", "Intermediate": "Dubai", "abbrev": "DXB", "departure": "MAA", "arrival": "RDU", "Route1_Stop1": "DXB", "Route1_Stop2": "JFK", "Route2_Stop1": "DXB", "Route2_Stop2": "ATL" }
 ]

var airports =

 {
     "type": "FeatureCollection",

     "features": [

        {
            "properties": { "name": "Durham Int'l Airport", "from": "Raleigh", "to": "Chennai", "abbrev": "RDU", "departure": "RDU", "arrival": "MAA", "Route1_Stop1": "IAD", "Route1_Stop2": "FRA", "Route2_Stop1": "JFK", "Route2_Stop2": "DXB", "Route3_Stop1": "ATL", "Route3_Stop2": "DXB" }, "type": "Feature", "geometry": { "type": "Point", "coordinates": [-78.791381400675078, 35.875232345225498] }

        },
        {
            "properties": {"name": "Chennai Int' Airport", "from": "Chennai", "to": "Raleigh", "abbrev": "MAA", "departure": "MAA", "arrival": "RDU", "Route1_Stop1": "FRA", "Route1_Stop2": "IAD", "Route2_Stop1": "DXB", "Route2_Stop2": "JFK", "Route3_Stop1": "DXB", "Route3_Stop2": "ATL"}, "type": "Feature", "geometry": { "type": "Point", "coordinates": [80.163775973154515, 12.982530166915435] }

        }
     ]
 }

var airports_data=
    [
   { "name": "Durham Int'l Airport", "from": "Raleigh", "to": "Chennai", "abbrev": "RDU", "departure": "RDU", "arrival": "MAA", "Route1_Stop1": "IAD", "Route1_Stop2": "FRA", "Route2_Stop1": "JFK", "Route2_Stop2": "DXB", "Route3_Stop1": "ATL", "Route3_Stop2": "DXB"},
   { "name": "Chennai Int' Airport", "from": "Chennai", "to": "Raleigh", "abbrev": "MAA", "departure": "MAA", "arrival": "RDU", "Route1_Stop1": "FRA", "Route1_Stop2": "IAD", "Route2_Stop1": "DXB", "Route2_Stop2": "JFK", "Route3_Stop1": "DXB", "Route3_Stop2": "ATL"}
]