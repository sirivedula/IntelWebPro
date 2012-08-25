<%@ Page Title="" Language="C#" MasterPageFile="~/IntelWeb.Master" AutoEventWireup="true" CodeBehind="TestMap.aspx.cs" Inherits="IntelWebSite.TestMap" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="http://maps.google.com/maps?file=api&amp;v=2&amp;key=ABQIAAAAbn9zEUri_Q6n2HykSMea0BTvZHZuNK8bHHy4Wqgc-UttDWoLPBTe4ydgypgvQO7nrgyWSa685ziYeQ"
            type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function() {
            initialize();
        });
        function initialize() {
            if (GBrowserIsCompatible()) {
                var map = new GMap2(document.getElementById("gsMap"));
                map.setCenter(new GLatLng(37.4419, -122.1419), 13);
                map.setUIToDefault();

                // Create a base icon for all of our markers that specifies the
                // shadow, icon dimensions, etc.
                var baseIcon = new GIcon(G_DEFAULT_ICON);
                baseIcon.shadow = "http://www.google.com/mapfiles/shadow50.png";
                baseIcon.iconSize = new GSize(20, 34);
                baseIcon.shadowSize = new GSize(37, 34);
                baseIcon.iconAnchor = new GPoint(9, 34);
                baseIcon.infoWindowAnchor = new GPoint(9, 2);

                // Creates a marker whose info window displays the letter corresponding
                // to the given index.
                function createMarker(point, index) {
                    // Create a lettered icon for this point using our icon class
                    var letter = String.fromCharCode("A".charCodeAt(0) + index);
                    var letteredIcon = new GIcon(baseIcon);
                    letteredIcon.image = "http://www.google.com/mapfiles/marker" + letter + ".png";

                    // Set up our GMarkerOptions object
                    markerOptions = { icon: letteredIcon };
                    var marker = new GMarker(point, markerOptions);

                    GEvent.addListener(marker, "click", function() {
                        $('#showup').hide();
                        marker.openInfoWindowHtml('<div style="cursor:pointer;" onclick="showDiv(this);">Marker <b>' + letter + '</b></div>');
                    });
                    return marker;
                }

                // Add 10 markers to the map at random locations
                var bounds = map.getBounds();
                var southWest = bounds.getSouthWest();
                var northEast = bounds.getNorthEast();
                var lngSpan = northEast.lng() - southWest.lng();
                var latSpan = northEast.lat() - southWest.lat();
                for (var i = 0; i < 10; i++) {
                    var latlng = new GLatLng(southWest.lat() + latSpan * Math.random(),
            southWest.lng() + lngSpan * Math.random());
                    map.addOverlay(createMarker(latlng, i));
                }
            }
        }
        
        function showDiv(id) {
            var pos = $(id).offset();
            var xleft = pos.left + 'px';
            var xtop = pos.top + "px"; 
            $('#showup').css({
                position: 'absolute',
                zIndex: 9999,
                left: xleft,
                top: xtop
            });
            $('#showup').show();    
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="gsMap" style="width:100%;height:1024px;">
    </div>
    <div id="showup" style="display:none;position:absolute;">
        <span style="font-size:20px;font-family:Tahoma;border:2px solid #990000;background-color:Aqua;height:500px;width:800px;">Hi This is test poup on the map<br />Panel Device and all<br /> Can be displayed</span>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
</asp:Content>
