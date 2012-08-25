
//---link for the below code http://www.invisalign.com/Doctor-Locator/Pages/Default.aspx?zip=77070&QuerySource=TR2

//---click event
var doclocZoomLevel = 11;
var doclocNumResults = 5;
doclocCenter = [30.0117, -95.5892];
doclocResults = [[30.0192, -95.4344, "Raymond McLendon", "Orthodontist", "1", "SUPER ELITE", "Clear Choice Orthodontics", "Houston", "TX", "77090", "www.clearchoiceortho.com", "(281)587-4900", "False", "True", "True", "105657", "False", "False"], [29.8816, -95.6454, "Carl Gullick", "Orthodontist", "2", "ELITE PREFERRED PROVIDER", "7171 Highway 6 N Ste 107", "Houston", "TX", "77095-2422", "www.gullicksmiles.com", "(281)859-6555", "False", "True", "True", "64266", "False", "False"], [30.0125, -95.516, "Robert Muirhead", "Orthodontist", "3", "PREMIER PREFERRED PROVIDER", "16116 Stuebner Airline Rd", "Spring", "TX", "77379-7327", "www.lonestarsmiles.com", "(281)376-5858", "False", "True", "True", "63705", "False", "False"], [29.9352, -95.5849, "Jim Watson", "Orthodontist", "4", "PREFERRED PROVIDER", "11111 Jones Road", "Houston", "TX", "77070", "www.orthodontics1.net", "(281)955-7612", "False", "False", "True", "63527", "False", "False"], [30.0117, -95.5892, "James Beaty", "General Dentist", "4", "PREFERRED PROVIDER", "22516 Highway 249", "Houston", "TX", "77070", "www.watersedgedentistry.com", "(281)251-9898", "False", "False", "True", "210331", "False", "False"]];
initialize();
//----------------------
var gmarkers = [];
var htmls = [];
var side_bar_html = "";

function getSearchAsArray() {
    var results = [];
    var input = unescape(location.search.substr(1));
    if (input) {
        var srchArray = input.split('&');
        var tempArray = [];
        for (var i = 0; i < srchArray.length; i++) {
            tempArray = srchArray[i].split("=");
            results[tempArray[0]] = tempArray[1];
        }
    }
    return results;
};
//------------
function initialize() {
    if (GBrowserIsCompatible()) {
        var map = new GMap2(document.getElementById("map_canvas"));
        var bounds = new GLatLngBounds();
        var segmentName;

        //map.setCenter(new GLatLng(doclocCenter[0], doclocCenter[1]),  doclocZoomLevel);
        map.setCenter(new GLatLng(0, 0), 0);
        map.enableContinuousZoom();
        map.enableScrollWheelZoom();
        map.addControl(new GMapTypeControl());
        map.addControl(new GLargeMapControl());
        // Creates a marker at the given point
        function createMarker(index) {




            function importanceOrder(marker, b) {
                return GOverlay.getZIndex(marker.getPoint().lat()) + marker.importance * 1000000;
            }
            var latlng = new GLatLng(doclocResults[index][0], doclocResults[index][1]);
            var latlng2 = new GLatLng(doclocResults[index][0], doclocResults[index][1]);

            var qs = getSearchAsArray();
            if (typeof qs.sort != "undefined" && qs.sort == "S5" && doclocResults[index][13] == "True") {
                var providerIcon = new GIcon();
                providerIcon.image = "/Doctor-Locator/PublishingImages/mapiconteen.png";
                providerIcon.iconSize = new GSize(37, 50);
                providerIcon.shadow = "/Doctor-Locator/PublishingImages/shadow.png";
                providerIcon.iconSize = new GSize(37, 50);
                providerIcon.shadowSize = new GSize(42, 50);
                providerIcon.iconAnchor = new GPoint(15.0, 50.0);
                providerIcon.infoWindowAnchor = new GPoint(15.0, 50.0);
                // Set up our GMarkerOptions object
                providermarkerOptions = { icon: providerIcon, zIndexProcess: importanceOrder };
                var marker = new GMarker(latlng, providermarkerOptions);
                if (doclocResults[index][4] == "3") {
                    segmentName = "Premier Preferred";
                    marker.importance = 2;
                }
                else if (doclocResults[index][4] == "2") {
                    segmentName = "Elite Preferred";
                    marker.importance = 3;
                }
                else if (doclocResults[index][4] == "1") {
                    segmentName = "Elite Preferred";
                    marker.importance = 3;
                }
                else if (doclocResults[index][4] == "4") {
                    segmentName = "Preferred Provider";
                    marker.importance = 1;
                }
                else {
                    segmentName = "";
                    marker.importance = 1;
                }
            }
            else if (doclocResults[index][4] == "3") {

                var premierIcon = new GIcon();
                premierIcon.image = "/Doctor-Locator/PublishingImages/mapiconpremiere.png";
                premierIcon.shadow = "/Doctor-Locator/PublishingImages/shadow.png";
                premierIcon.iconSize = new GSize(37, 50);
                premierIcon.shadowSize = new GSize(42, 50);
                premierIcon.iconAnchor = new GPoint(15.0, 50.0);
                premierIcon.infoWindowAnchor = new GPoint(15.0, 50.0);
                // Set up our GMarkerOptions object
                premiermarkerOptions = { icon: premierIcon, zIndexProcess: importanceOrder };
                var marker = new GMarker(latlng, premiermarkerOptions);
                segmentName = "Premier Preferred";
                marker.importance = 2;
            }
            else if (doclocResults[index][4] == "2") {
                var eliteIcon = new GIcon();
                eliteIcon.image = "/Doctor-Locator/PublishingImages/mapiconelite.png";
                eliteIcon.iconSize = new GSize(37, 50);
                eliteIcon.shadow = "/Doctor-Locator/PublishingImages/shadow.png";
                eliteIcon.iconSize = new GSize(37, 50);
                eliteIcon.shadowSize = new GSize(42, 55);
                eliteIcon.iconAnchor = new GPoint(15.0, 50.0);
                eliteIcon.infoWindowAnchor = new GPoint(15.0, 50.0);
                // Set up our GMarkerOptions object
                elitemarkerOptions = { icon: eliteIcon, zIndexProcess: importanceOrder };
                var marker = new GMarker(latlng, elitemarkerOptions);
                segmentName = "Elite Preferred";
                marker.importance = 3;
            }
            else if (doclocResults[index][4] == "1") {
                var eliteIcon = new GIcon();
                eliteIcon.image = "/Doctor-Locator/PublishingImages/mapiconelite.png";
                eliteIcon.iconSize = new GSize(37, 50);
                eliteIcon.shadow = "/Doctor-Locator/PublishingImages/shadow.png";
                eliteIcon.iconSize = new GSize(37, 50);
                eliteIcon.shadowSize = new GSize(42, 50);
                eliteIcon.iconAnchor = new GPoint(15.0, 50.0);
                eliteIcon.infoWindowAnchor = new GPoint(15.0, 50.0);
                // Set up our GMarkerOptions object
                elitemarkerOptions = { icon: eliteIcon, zIndexProcess: importanceOrder };
                var marker = new GMarker(latlng, elitemarkerOptions);
                segmentName = "Elite Preferred";
                marker.importance = 3;
            }
            else if (doclocResults[index][13] == "True") {
                var providerIcon = new GIcon();
                providerIcon.image = "/Doctor-Locator/PublishingImages/mapiconteen.png";
                providerIcon.iconSize = new GSize(37, 50);
                providerIcon.shadow = "/Doctor-Locator/PublishingImages/shadow.png";
                providerIcon.iconSize = new GSize(37, 50);
                providerIcon.shadowSize = new GSize(42, 50);
                providerIcon.iconAnchor = new GPoint(15.0, 50.0);
                providerIcon.infoWindowAnchor = new GPoint(15.0, 50.0);
                // Set up our GMarkerOptions object
                providermarkerOptions = { icon: providerIcon, zIndexProcess: importanceOrder };
                var marker = new GMarker(latlng, providermarkerOptions);
                if (doclocResults[index][4] == "3") {
                    segmentName = "Premier Preferred";
                    marker.importance = 2;
                }
                else if (doclocResults[index][4] == "2") {
                    segmentName = "Elite Preferred";
                    marker.importance = 3;
                }
                else if (doclocResults[index][4] == "1") {
                    segmentName = "Elite Preferred";
                    marker.importance = 3;
                }
                else if (doclocResults[index][4] == "4") {
                    segmentName = "Preferred Provider";
                    marker.importance = 1;
                }
                else {
                    segmentName = "";
                    marker.importance = 1;
                }

            }
            else if (doclocResults[index][4] == "4") {
                var providerIcon = new GIcon();
                providerIcon.image = "/Doctor-Locator/PublishingImages/mapiconpreferred.png";
                providerIcon.iconSize = new GSize(37, 50);
                providerIcon.shadow = "/Doctor-Locator/PublishingImages/shadow.png";
                providerIcon.iconSize = new GSize(37, 50);
                providerIcon.shadowSize = new GSize(42, 50);
                providerIcon.iconAnchor = new GPoint(15.0, 50.0);
                providerIcon.infoWindowAnchor = new GPoint(15.0, 50.0);
                // Set up our GMarkerOptions object
                providermarkerOptions = { icon: providerIcon, zIndexProcess: importanceOrder };
                var marker = new GMarker(latlng, providermarkerOptions);
                segmentName = "Preferred Provider";
                marker.importance = 1;
            }
            else {
                var providerIcon = new GIcon();
                providerIcon.image = "/Doctor-Locator/PublishingImages/mapiconprovider.png";
                providerIcon.iconSize = new GSize(37, 50);
                providerIcon.shadow = "/Doctor-Locator/PublishingImages/shadow.png";
                providerIcon.iconSize = new GSize(37, 50);
                providerIcon.shadowSize = new GSize(42, 50);
                providerIcon.iconAnchor = new GPoint(15.0, 50.0);
                providerIcon.infoWindowAnchor = new GPoint(15.0, 50.0);
                // Set up our GMarkerOptions object
                providermarkerOptions = { icon: providerIcon, zIndexProcess: importanceOrder };
                var marker = new GMarker(latlng, providermarkerOptions);
                segmentName = "";
                marker.importance = 1;
            }

            bounds.extend(marker.getPoint());
            marker.value = index + 1;
            var myHtml = "<table class='mapPopup' width='185'> <tr> <td> <h2>" + doclocResults[index][2] + "</h2> <span class='location'>" + doclocResults[index][3] + "</span> <span class='specialty'>" + segmentName + "</span>";

            if (doclocResults[index][17] == "True") {
                myHtml = myHtml + "<span class='prefdoc'>Clinical Advisory Board Member</span>";
            }
            if (doclocResults[index][16] == "True") {
                myHtml = myHtml + "<span class='prefdoc'>Align Faculty</span>";
            }

            myHtml = myHtml + "<br/><span class='location'>" + doclocResults[index][6] + "," + " <br /> " + doclocResults[index][7] + ", " + doclocResults[index][8] + ", " + doclocResults[index][9] + " <br /> " + doclocResults[index][11] + "<br /></span><br />";
            if (doclocResults[index][13] == "True") {
                myHtml = myHtml + "<a href='/Invisalign-For-Adults-And-Teens/Pages/Teens.aspx' target='_blank'><span class='teen'>Invisalign Teen</span></a>";
            }
            if (doclocResults[index][10].length > 0) {
                myHtml = myHtml + "<a href='/Doctor-Locator/Pages/Track.aspx?src=main&type=2&rurl=" + doclocResults[index][10] + "&ref=" + doclocResults[index][15] + "' target='_blank'>&gt;&gt;" + doclocResults[index][10] + "</a>";
            }
            myHtml = myHtml + "<a href='http://maps.google.com/maps?daddr=" + doclocResults[index][6] + "," + doclocResults[index][7] + "," + doclocResults[index][8] + "," + doclocResults[index][9] + "' target='_blank'> &gt;&gt;Get Directions</a>";
            myHtml = myHtml + "</td></tr></table>";
            htmls[index] = myHtml;

            function addHtml(myHtml) {
                map.openInfoWindowHtml(latlng2, myHtml);
            }
            GEvent.addListener(marker, "click", function () { map.openInfoWindowHtml(latlng2, myHtml); });

            GEvent.addListener(marker, "mouseover", function () { map.openInfoWindowHtml(latlng2, myHtml); });

            // save the info we need to use later 

            gmarkers[index] = marker;
            //side_bar_html += '<a href="javascript:myclick(' + index + ')">' + doclocResults[index][2] + '</a><br>';
            return marker;
        }
        map.clearOverlays();
        //for (var i = 0; i < doclocNumResults; i++)
        for (var i = doclocNumResults - 1; i >= 0; i--) {
            map.addOverlay(createMarker(i));
        }
        map.setZoom(map.getBoundsZoomLevel(bounds));
        map.setCenter(bounds.getCenter());
        //document.getElementById("map_links").innerHTML = side_bar_html;                                                
    }
}
// This function picks up the click and opens the corresponding info window
function myclick(i) {
    gmarkers[i].openInfoWindowHtml(htmls[i]);
    window.scrollTo(document.getElementById("map_canvas").offsetLeft, document.getElementById("map_canvas").offsetTop);
}