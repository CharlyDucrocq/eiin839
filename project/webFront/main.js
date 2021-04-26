var mousePositionControl = new ol.control.MousePosition({
    coordinateFormat: ol.coordinate.createStringXY(8),
    projection: 'EPSG:4326',
    // comment the following two lines to have the mouse position
    // be placed within the map.
    className: 'custom-mouse-position',
    target: document.getElementById('mouse-position'),
    undefinedHTML: '&nbsp;',
});


var map = new ol.Map({
    controls: ol.control.defaults().extend([mousePositionControl]),
    layers: [
        new ol.layer.Tile({
            source: new ol.source.OSM(),
        })],
    target: 'map',
    view: new ol.View({
        center: [0, 0],
        zoom: 2,
    }),
});

var currentUpTargetId = "start";
function selectCurrentUpTarget(id) {
    currentUpTargetId = id;
}

var layers = {};

var mapDiv = document.getElementById("map");
mapDiv.onclick = function () {
    var pos = document.getElementById('mouse-position').firstChild.innerHTML.split(',');
    var lng = pos[0];
    var lat = pos[1];
    console.log('try to write on ' + currentUpTargetId + "Lat and " + currentUpTargetId + "Lng");
    document.getElementById(currentUpTargetId + "Lat").value = lat;
    document.getElementById(currentUpTargetId + "Lng").value = lng;
};

function addIcon(iconName, coordinate) {

    function createStyle(src) {
        return new ol.style.Style({
            image: new ol.style.Icon({
                anchor: [0.5, 0.96],
                src: src,
                crossOrigin: null,
            }),
        });
    }

    var iconFeature = new ol.Feature(new ol.geom.Point(coordinate));
    iconFeature.set('style', createStyle('data/' + iconName + '.png'));
    var vectorSource = new ol.source.Vector({ features: [iconFeature] });
    var iconLayer = new ol.layer.Vector({
        style: function (feature) {
            return feature.get('style');
        },
        source: vectorSource,
    })

    map.addLayer(iconLayer);
    return iconLayer;
}

map.on('click', function (evt) {
    var coordinate = evt.coordinate;

    if (layers[currentUpTargetId]) {
        map.removeLayer(layers[currentUpTargetId]);
    }
    layers[currentUpTargetId] = addIcon(currentUpTargetId + '_icon', coordinate);
    map.render();
});

// ----------------------------REQUEST PART---------------------------------------

function sendRequest() {
    var targetUrl = "http://localhost:8733/Design_Time_Addresses/MyBicycleRedirectionService/RedirectionService/rest/way" +
                    "?startLng=" + document.getElementById("startLng").value+
                    "&startLat=" + document.getElementById("startLat").value +
                    "&endLng="   + document.getElementById("endLng").value +
                    "&endLat="   + document.getElementById("endLat").value ;
    var requestType = "GET";

    var caller = new XMLHttpRequest();
    caller.open(requestType, targetUrl, true);
    // The header set below limits the elements we are OK to retrieve from the server.
    caller.setRequestHeader("Accept", "application/json");
    // onload shall contain the function that will be called when the call is finished.
    caller.onload = getResponse;
    document.getElementById("responseBox").innerHTML = "Loading ...";
    caller.send();
}

var interPointIconsLayers = [];
function getResponse() {
    //reset previous points
    interPointIconsLayers.forEach(layer => {
        map.removeLayer(layer);
    });
    interPointIconsLayers = [];

    // Let's parse the response:
    var response = JSON.parse(this.responseText).GetWayToGoSimpleResult;
    console.log(response);

    // Create an array containing the GPS positions you want to draw
    var coords = [];
    var i = 0;
    response.forEach(edge => {
        if (i != 0) {
            var newLayer = addIcon("parking_icon", ol.proj.fromLonLat(edge.coordinates[0]));
            interPointIconsLayers.push(newLayer);
        }
        coords = coords.concat(edge.coordinates);
        i++;
    });
    var lineString = new ol.geom.LineString(coords);

    // Transform to EPSG:3857
    lineString.transform('EPSG:4326', 'EPSG:3857');

    // Create the feature
    var feature = new ol.Feature({
        geometry: lineString,
        name: 'Line'
    });

    // Configure the style of the line
    var lineStyle = new ol.style.Style({
        stroke: new ol.style.Stroke({
            color: '#ffcc33',
            width: 10
        })
    });

    var source = new ol.source.Vector({
        features: [feature]
    });

    var vector = new ol.layer.Vector({
        source: source,
        style: [lineStyle]
    });

    var traceName = "trace";
    if (layers[traceName]) {
        map.removeLayer(layers[traceName]);
    }
    layers[traceName] = vector
    map.addLayer(vector);
    document.getElementById("responseBox").innerHTML = "Done";
}