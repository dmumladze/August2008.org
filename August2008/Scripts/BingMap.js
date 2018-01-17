
function Map() {
};

Map.instance = undefined;
Map.actionUrl = undefined;
Map.pinInfoBox = undefined;
Map.infoboxLayer = undefined;
Map.pinLayer = undefined;
Map.pinIconUrl = undefined;
Map.infoboxHtmlContent = undefined;
Map.parent = undefined;
Map.fullScreen = false;
Map.zoomLevel = 2;

Map.init = function (actionUrl, container, pinIconUrl, infoboxLegUrl, flover) {
    Map.infoboxHtmlContent =
        '<div class="infobox">' +
        '<a class="infobox-close" href="javascript:Map.hideInfobox()">' +
        'x' +
        '</a>' +
        '<div class="infobox-content">' +
        '{content}' +
        '</div>' +
        '</div>' +
        '<div class="infobox-pointer">' +
        '<img src="' + infoboxLegUrl + '"/>' +
        '</div>';

    $('<img id="map-flover" src="' + flover + '"/>').appendTo(document.body);
    //debugger;
    Map.actionUrl = actionUrl;
    Map.pinIconUrl = pinIconUrl;
    Map.infoboxLayer = new Microsoft.Maps.EntityCollection();
    Map.pinLayer = new Microsoft.Maps.EntityCollection();
    Map.instance = new Microsoft.Maps.Map(document.getElementById(container), {
        credentials: 'Ar3RVmfuiXbvTneoW5Jcj80k2UGTJjEWcxAW9s0JSo2FcQCp3UFbA4ED-0S9xhNr',
        showCopyright: false,
        enableSearchLogo: false,
        enableClickableLogo: false,
        mapTypeId: Microsoft.Maps.MapTypeId.aerial,
        zoom: 2,
        center: new Microsoft.Maps.Location(20, 7),
        showDashboard: false
    });
    Microsoft.Maps.Events.addThrottledHandler(Map.instance, 'viewchangeend', Map.viewChangeEndHandler, 25);
    Microsoft.Maps.Events.addHandler(Map.instance, "dblclick", Map.floverPopup);

    Map.getLocations();

    $('#btnResize').click(function (e) {
        //debugger;
        var bingMap = $('#bingMapContainer');
        var body = $(document.body);
        var win = $(window);
        var btnView = $(e.target);

        if (Map.fullScreen) {
            btnView.attr("class", "map-maximize");
            body.css({ overflow: "auto" });
            bingMap.detach();
            Map.parent.height(550);
            bingMap.css({ position: "relative", top: 0, width: "100%", height: 550, opacity: 0.97 });
            Map.parent.append(bingMap);
            Map.zoom(2, 20, 7);
            Map.fullScreen = false;
        }
        else {
            btnView.attr("class", "map-minimize");
            Map.parent = bingMap.parent();
            body.css({ overflow: "hidden" });

            var w = body.width();
            var h = body.height();
            var sh = win.scrollTop();

            var mw = bingMap.width();
            var mh = bingMap.height();
            var off = bingMap.offset();
            var ml = off.left;
            var mt = off.top;

            bingMap.detach();
            body.append(bingMap);
            bingMap.css({ position: "absolute", "left": 0, top: sh, height: h, width: w, opacity: 0.97 });
            Map.zoom(3, 20, 7);
            Map.fullScreen = true;
        }
        body.css({ "overflow-x": "hidden" });
    });
    $("#slider-vertical").slider({
        orientation: "vertical",
        range: "min",
        min: 2,
        max: 10,
        value: Map.zoomLevel,
        slide: function (event, ui) {
            Map.zoom(ui.value);
        }
    });
};

Map.viewChangeEndHandler = function (credentials) {
    //debugger;
    //Map.instance.getCredentials(makeMapRequest);
    Map.zoomLevel = Map.instance.getZoom();
    if (Map.zoomLevel > 7)
        Map.instance.setMapType(Microsoft.Maps.MapTypeId.road);
    else
        Map.instance.setMapType(Microsoft.Maps.MapTypeId.aerial);
    
    $("#slider-vertical").slider("value", Map.zoomLevel);
};

Map.getLocations = function () {
    $.getJSON(Map.actionUrl, null, function (data, status, xhr) {
        //debugger;
        Map.instance.entities.clear();
        Map.pinInfobox = new Microsoft.Maps.Infobox(new Microsoft.Maps.Location(0, 0), { visible: false });
        Map.pinInfobox.setMap(Map.instance);

        $.each(data, function (index, value) {
            //debugger;
            var pin = new Microsoft.Maps.Pushpin(Map.instance.getCenter(), { icon: Map.pinIconUrl });

            pin.setLocation(new Microsoft.Maps.Location(
                value.Latitude,
                value.Longitude));
            //debugger;
            pin.Title = value.City + ", " + value.State + " (" + value.Country + ")";
            pin.Description = "Donated: $" + value.TotalSum;
            Map.pinLayer.push(pin);

            Microsoft.Maps.Events.addHandler(pin, 'click', Map.displayInfobox);
        });
        //debugger;
        Map.instance.entities.push(Map.pinLayer);
        Map.instance.entities.push(Map.infoboxLayer);
        //Map.instance.layers.insert(Map.pinLayer);
        //Map.instance.layers.insert(Map.infoboxLayer);
        //Map.infoboxLayer.setMap(Map.instance);
    });
};

Map.zoom = function (zoom, centerLat, centerLong) {
    var options = Map.instance.getOptions();
    options.zoom = Map.zoomLevel;
    //debugger;
    if (centerLat && centerLong) {
        options.center = new Microsoft.Maps.Location(centerLat, centerLong);
    }
    Map.instance.setView(options);
}

Map.displayInfobox = function (e) {
    //debugger;
    var html = '<div class="infobox-title">' + e.target.Title + '</div><div class="infobox-desc">' + e.target.Description + '</div>';
    Map.pinInfobox.setOptions({
        visible: true,
        offset: new Microsoft.Maps.Point(-30, 10),
        htmlContent: Map.infoboxHtmlContent.replace('{content}', html)
    });
    Map.pinInfobox.setLocation(e.target.getLocation());
};

Map.hideInfobox = function (e) {
    Map.pinInfobox.setOptions({ visible: false });
};

Map.floverPopup = function (e) {
    var x = e.clientX || e.pageX;
    var y = e.clientY || e.pageY;
    var flover = $('#map-flover');
    flover.width(1).height(1).css({ left: x, top: y - 10 });
    setTimeout(function () {
        flover.addClass('map-flover-tran map-flover-close');
        setTimeout(function () {
            flover.removeClass('map-flover-tran map-flover-close').width(0).height(0);
        }, 500);
    }, 25);
};
