﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Map : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string inner = InitializeMap("33.693", "73.067");
        string message = GetInfoWindowString(ReasonTypes.Accident, SeverityType.Severe, "11:45:00PM", "1111");
        inner += CreateMarker("marker", "33.693", "73.067", message, "1", ReasonTypes.Accident, UserTypes.Admin);

        //message = GetInfoWindowString(ReasonTypes.Clear, SeverityType.Mild, "11:45:00PM", "1111");
        inner += CreateMarker("marker2", "34.693", "73.067", message, "2", ReasonTypes.Accident, UserTypes.Admin);

        //message = GetInfoWindowString(ReasonTypes.Unknown, SeverityType.Normal, "11:45:00PM", "1111");
        inner += CreateMarker("marker2", "33.693", "73.069", message, "3", ReasonTypes.Clear, UserTypes.User);

        inner += TapMap();
        string script = "function initialize() {" + inner + "}google.maps.event.addDomListener(window, 'load', initialize);";

        this.Page.Header.Controls.Add(
            new LiteralControl("<script>" + script + "</script>")); 
    }

    private string InitializeMap(string latitude, string longitude)
    {
        return Environment.NewLine + @"var mapOptions = {
                center: { lat: " + latitude + ", lng: " + longitude + @" },
                zoom: 14
                };
                var map = new google.maps.Map(document.getElementById('map-canvas'),
                    mapOptions);" + Environment.NewLine;
    }

    private string CreateMarker(string markerVariableName, string latitude, string longitude, string messageString, string innerLabel, int reasonType, int userType)
    {
        return _InitializeLatlangVariable("latlng" + markerVariableName, latitude, longitude) + _InitializeInfoWindow("infowindow" + markerVariableName, messageString) + _ShowMarker(markerVariableName, "latlng" + markerVariableName, innerLabel, reasonType, userType) + _SetMarkerClick(markerVariableName);
    }

    private string TapMap()
    {
        return Environment.NewLine + @"google.maps.event.addListener(map, 'click', function (e) {
                    var latLng = e.latLng;
                    alert(latLng);
                    document.getElementbyId
                });" + Environment.NewLine;
    }

    private string GetInfoWindowString(int reasonType, int severityType, string time, string postId)
    {
        string reason = "Unknown";
        string severity = "Mild";
        
        if (reasonType == ReasonTypes.Clear)
            reason = "Clear";

        else if (reasonType == ReasonTypes.Accident)
            reason = "Accident";
        
        else if (reasonType == ReasonTypes.Standstill)
            reason = "Standstill";
        
        else if (reasonType == ReasonTypes.Signal)
            reason = "Signal";

        if (severityType == SeverityType.Normal)
            severity = "Normal";

        else if (severityType == SeverityType.Severe)
            severity = "Severe";

        return "<div>" + "<p>Reason: " + reason + "</p><p>Severity: " + severity + "</p><p>Time: " + time + "</p><br /><a id=\"" + postId + "\">Report as spam</a></div>";
    }

    private string _InitializeLatlangVariable(string variableName, string Latitude, string Longitude)
    {
        return Environment.NewLine + "var " + variableName + " = new google.maps.LatLng(" + Latitude + ", " + Longitude + ");" + Environment.NewLine;
    }

    private string _InitializeInfoWindow(string variableName, string messageString)
    {
        return Environment.NewLine + "var " + variableName + @" = new google.maps.InfoWindow({
                    content: '" + messageString + @"'
                });" + Environment.NewLine;
    }

    private string _ShowMarker(string variableName, string latLngVariableName, string innerLabel, int type, int usertype)
    {
        string icon = "images/pinred.png";
        string position = "6, 40";

        if (type == ReasonTypes.Clear)
            icon = "images/pingreen.png";


        if (usertype == UserTypes.Admin)
        {
            icon = icon.Split('.')[0] + "police." + icon.Split('.')[1];
            position = "6, 25";
        }
            
        return Environment.NewLine + @"var " + variableName + @" = new MarkerWithLabel({
            position: " + latLngVariableName + @",
            map: map,
            labelContent: '" + innerLabel + @"',
            labelAnchor: new google.maps.Point(" + position + @"),
            icon: '" + icon + @"',
            labelClass: 'labels', // the CSS class for the label
            labelInBackground: false
        });" + Environment.NewLine;
    }

    private string _SetMarkerClick(string markerVariableName)
    {
        return Environment.NewLine + "google.maps.event.addListener(" + markerVariableName + @", 'click', function () {
            infowindow" + markerVariableName + ".open(map, " + markerVariableName + @");
        });" + Environment.NewLine;
    }

    
}