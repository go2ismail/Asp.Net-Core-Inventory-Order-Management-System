// For Properties Panel

var themes = {
	"flat": "../content/ejthemes/default-theme/ej.theme.min.css",
    "flatdark": "../content/ejthemes/flat-azure-dark/ej.theme.min.css",
    "azure": "../content/ejthemes/default-theme/ej.theme.min.css",
    "azuredark": "../content/ejthemes/flat-azure-dark/ej.theme.min.css",
    "lime": "../content/ejthemes/flat-lime/ej.theme.min.css",
    "limedark": "../content/ejthemes/flat-lime-dark/ej.theme.min.css",
    "saffron": "../content/ejthemes/flat-saffron/ej.theme.min.css",
    "saffrondark": "../content/ejthemes/flat-saffron-dark/ej.theme.min.css",
	"gradient": "../content/ejthemes/gradient-azure/ej.theme.min.css",
    "gradientdark": "../content/ejthemes/gradient-azure-dark/ej.theme.min.css",
    "gradientazure": "../content/ejthemes/gradient-azure/ej.theme.min.css",
    "gradientazuredark": "../content/ejthemes/gradient-azure-dark/ej.theme.min.css",
    "gradientlime": "../content/ejthemes/gradient-lime/ej.theme.min.css",
    "gradientlimedark": "../content/ejthemes/gradient-lime-dark/ej.theme.min.css",
    "gradientsaffron": "../content/ejthemes/gradient-saffron/ej.theme.min.css",
    "gradientsaffrondark": "../content/ejthemes/gradient-saffron-dark/ej.theme.min.css",
    "bootstrap": "../content/ejthemes/bootstrap-theme/ej.theme.min.css",
	"high-contrast-01":"../content/ejthemes/high-contrast-01/ej.theme.min.css",
	"high-contrast-02": "../content/ejthemes/high-contrast-02/ej.theme.min.css",
	"material": "../content/ejthemes/material/ej.theme.min.css",
	"office-365": "../content/ejthemes/office-365/ej.theme.min.css"	
};

var core = {
    "bootstrap": "../content/ejthemes/ej.widgets.core.bootstrap.min.css",
    "material": "../content/ejthemes/ej.widgets.core.material.min.css",
	"office-365": "../content/ejthemes/ej.widgets.core.office-365.min.css"	
};
window.isMac = function () {
    return (/(ipad|iphone|ipod touch)/i).test(navigator.userAgent.toLowerCase()) && !(/trident|windows phone/i.test(navigator.userAgent.toLowerCase()));
}
window.baseurl = "//js.syncfusion.com/ejServices/";


(function ($, ej, undefined) {

    // Example plugin creation code
    // sfSample is the plugin name 
    // "ej.Sample" is "namespace.className" will hold functions and properties

	if (window.isMac())
		window.baseurl="//js.syncfusion.com/ejServices/";
	else 
		window.baseurl=window.baseurl;
	
    ej.widget("ejPropertiesPanel", "ej.PropertiesPanel", {
         _rootCSS: "e-prop",
        // default model
        element: null,
        model: null,
        header:"Properties",
        defaults: {
            delayRender: true
        },
        // constructor function
        _init: function () {
            if (ej.browserInfo().name == "msie" && this.model.delayRender) {
                var proxy = this;
                setTimeout(function () { proxy._initialize(); }, 0);
            }
			else this._initialize();
        },
        _initialize: function () {
            this._sfPropertiesPanel = this.element.wrap('<div class="cols-prop-area" />');
            this._sfPropertyTitlebar = ej.buildTag("div.heading").prependTo(this.element);
            ej.buildTag("span", this.header).appendTo(this._sfPropertyTitlebar);          
            this.element.find(".heading").next().addClass("content");
            if (QueryString["theme"] == "bootstrap") $(".e-prop").addClass("e-bootstrap");
        },
      
        hide: function () {
            var panel = this._sfPropertiesPanel;
            this._setSize();
            this._setPosition();
            panel.css({ display: "none" });
            return this;
        },
        _setModel: function () {
        },
        _setPosition: function () {
            if ($(document).find(".samplesection").length > 0) {
                this._sfPropertiesPanel.appendTo(".samplesection");
            }
            //var x = ($(this._sfPropertiesPanel).prev().width()+25);
            //this._sfPropertiesPanel.css({ left: x });            
        },
        // all events bound using this._on will be unbind automatically
        _destroy: function () {

        }
    });

})(jQuery, Syncfusion);

// For convert widgets
$(function () {
    convertWidgets();	
    if ($(".cols-sample-area").next('div').length==0)
        $(".cols-sample-area").css("width","100%");
});

var QueryString = function () {
    // This function is anonymous, is executed immediately and 
    // the return value is assigned to QueryString!
    var query_string = {};
    var query = window.location.search.substring(1);
    var vars = query.split("&");
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split("=");
        // If first entry with this name
        if (typeof query_string[pair[0]] === "undefined") {
            query_string[pair[0]] = pair[1];
            // If second entry with this name
        } else if (typeof query_string[pair[0]] === "string") {
            var arr = [query_string[pair[0]], pair[1]];
            query_string[pair[0]] = arr;
            // If third or later entry with this name
        } else {
            query_string[pair[0]].push(pair[1]);
        }
    }
    return query_string;
}();

window.updateThemess = function (theme) {
    if (theme) {
        var links = $(document.head || document.getElementsByTagName('head')[0]).find("link");
        for (var i = 0; i < links.length; i++) {
            if (links[i].href.indexOf("ej.web.all.min.css") != -1) {
                if ((theme == "bootstrap")||(theme == "material")||(theme == "office-365")) {
                    var corefile = document.createElement("link");
                    corefile.setAttribute("rel", "stylesheet");
                    corefile.setAttribute("type", "text/css");
                    corefile.setAttribute("href", core[theme]);
                    $(corefile).insertAfter(links[i]);
                }
                var fileref=document.createElement("link");
                fileref.setAttribute("rel", "stylesheet");
                fileref.setAttribute("type", "text/css");
                fileref.setAttribute("href", themes[theme]);
                ((theme == "bootstrap")||(theme == "material")||(theme =="office-365"))? $(fileref).insertAfter(corefile) : $(fileref).insertAfter(links[i]);				
                window.setTimeout(function () {
                    if ($(".cols-sample-area").find(".e-rte.e-js").length) $(".cols-sample-area").find(".e-rte.e-js").data("ejRTE")._setIframeHeight();
                    if ($(".cols-sample-area").find(".e-fileexplorer.e-js").length) $(".cols-sample-area").find(".e-fileexplorer.e-js").data("ejFileExplorer").adjustSize();
                },1500);
                break;
            }
        }
        $(function () {
            if ((theme.indexOf("dark") != -1)||(theme.indexOf("high-contrast")!=-1))
                document.body.className = "darktheme";          
        });
    }
};


updateThemess(QueryString["theme"]);
function loadTheme(sender) {
	if (!ej.util.isNullOrUndefined(window.orientation) && sender && this.pluginName == "ejChart") {    	//to modify chart properties for mobile view
		var model = sender.model,
		seriesLength = model.series.length;
		$(".cols-sample-area").css("padding", "0px");
		model.title.enableTrim = true;
		model.elementSpacing = 5;
		model.legend.visible = false;
		model.size.height = null;
		model.size.width = null;
		for (var i=0; i< seriesLength; i++){
			if (!model.series[i].marker)
				model.series[i].marker = {};
            if (!model.series[i].marker.size)
				model.series[i].marker.size = {};							
			model.series[i].marker.size.width = 6;
			model.series[i].marker.size.height = 6;
		}
		model.primaryXAxis.labelIntersectAction = "rotate45";
		if (model.primaryXAxis.title)
		     model.primaryXAxis.title.text = "";
		model.primaryXAxis.edgeLabelPlacement = "hide";
		model.primaryYAxis.labelIntersectAction = "rotate45";
		if (model.primaryYAxis.title)
		    model.primaryYAxis.title.text = "";
		model.primaryYAxis.edgeLabelPlacement = "hide";			
		if (model.axes) {
			for (var j=0; j< model.axes.length; j++){
				model.axes[j].labelIntersectAction = "rotate45";
				if (model.axes[j].title)
				   model.axes[j].title.text = "";
				model.axes[j].edgeLabelPlacement = "hide";
			}
		}
	}
	
    var theme = window.themestyle + window.themecolor + window.themevarient;
    if (theme) {
        switch (theme) {           
            case "flatdark":
            case "flatazuredark":
            case "flatlimedark":
            case "flatsaffrondark":
                theme = "flatdark";
                break;            
            case "gradientlight":
            case "gradientazurelight":
            case "gradientlimelight":
            case "gradientsaffronlight":
                theme = "gradientlight";
                break;
            case "gradientdark":            
            case "gradientazuredark":            
            case "gradientlimedark":            
            case "gradientsaffrondark":
                theme = "gradientdark";
                break;
            case "flatbootstraplight":
                theme = "bootstrap";
                break;
			case "flathigh-contrast-01dark":
			case "flathigh-contrast-02dark":
			    theme = "highcontrast01";
				break;
			case "flatmateriallight":
			case "flatoffice-365light":
			    theme = "material";
				break;
				
            default:
                theme = "flatlight";
                break;
        }
        sender.model.theme = theme;
    }

}

function loadOlapChartTheme(sender) {
    loadTheme(sender);
    if (sender.model.commonSeriesOptions.type == "pie" || sender.model.commonSeriesOptions.type == "pyramid" || sender.model.commonSeriesOptions.type == "funnel")
        sender.model.commonSeriesOptions.marker.dataLabel.font.color = sender.model.theme.indexOf("dark") >= 0 ? 'white' : 'black';
}

//Load Bullet theme
function loadBulletTheme(sender) {
    var bulletTheme = window.themestyle + window.themecolor + window.themevarient;
    if (bulletTheme) {
        switch (bulletTheme) {
            case "flatdark":
            case "flatazuredark":
            case "flatlimedark":
            case "flatsaffrondark":
            case "gradientdark":
            case "gradientazuredark":
            case "gradientlimedark":
            case "gradientsaffrondark":
			case "flathigh-contrast-01dark":
			case "flathigh-contrast-02dark":
                theme = "flatdark";
                break;
		    case "flatoffice-365light":
			case "flatmateriallight":
                theme = "material";
                break;
            default:
                theme = "flatlight";
                break;
        }
        sender.model.theme = theme;
    }
}

function loadSunBurstTheme(sender) {
    var SunBurstTheme = window.themestyle + window.themecolor + window.themevarient;
    SunBurstTheme = SunBurstTheme.toString();
    if (SunBurstTheme.indexOf("dark") > -1 || SunBurstTheme.indexOf("contrast") > -1)  
		 SunBurstTheme = "flatdark";
	else
	    SunBurstTheme = "flatlight";        
    sender.model.theme = SunBurstTheme;  
	
}


//Load Gauge Theme
function loadGaugeTheme(sender) {
    
    var theme1 = window.themestyle + window.themecolor + window.themevarient;
    if (theme1) {
        switch (theme1) {
            case "flatdark":
            case "flatazuredark":
            case "flatlimedark":
            case "flatsaffrondark":
            case "gradientdark":
            case "gradientazuredark":
            case "gradientlimedark":
            case "gradientsaffrondark":
			case "flathigh-contrast-01dark":
			case "flathigh-contrast-02dark":
                theme = "flatdark";
                break;           
            default:
                theme = "flatlight";
                break;
        }
        sender.model.theme = theme;
        updateGuageTheme(theme1,this._id);
    }
}

function updateGuageTheme(themestyle,id) {
    var clsname = "";
    if ($(".e-circulargauge").length > 0) {
        clsname = "e-circulargauge";
    }
    else if ($(".e-lineargauge").length > 0) {
        clsname = "e-lineargauge";
    }
    else if ($(".e-digitalgauge").length > 0) {
        clsname = "e-digitalgauge";
    }

    clsname && updateThemeforGauge(clsname, themestyle,id);
}
function updateThemeforGauge(clsname, themestyle, gaugeid) {   
    var themecolor = themestyle.indexOf("dark") == -1 ? "light" : "dark";
    var skin = "metro" + themecolor;
    switch (clsname) {
        case "e-circulargauge":           
            replaceframesforGauge(gaugeid, themecolor, themestyle, "circular");
            break;
        case "e-lineargauge":           
            replaceframesforGauge(gaugeid, themecolor, themestyle, "linear");
            break;
        case "e-digitalgauge":           
            replaceframesforGauge(gaugeid, themecolor, themestyle, "digital");
            break;
    }
}

function replaceframesforGauge(gaugeid, themecolor, themestyle, frameclass) {
    var lGauge, 
        oirentation,
	    framestyle;
    if (themestyle.indexOf("gradient") != -1) {
	    if (($(".e-lineargauge").length > 0)){
	      lGauge = $(".e-lineargauge").ejLinearGauge('instance');
	      oirentation = lGauge.model.orientation.toLowerCase();
	      framestyle = themecolor == "light" ? (oirentation == "vertical") ? "lightgradient" : "lightgradient1" : (oirentation == "vertical" ) ? "darkgradient" : "darkgradient1";
	    }
	    else if(($(".e-circulargauge").length > 0)){
            CGauge = $(".e-circulargauge").ejCircularGauge('instance');
			frameType = CGauge.model.frame.frameType;
			framestyle = themecolor == "light" ? (frameType == "fullcircle") ? "lightgradient" : "lightgradientsemi" : "darkgradient" ;			  
		}
		else
			framestyle = themecolor == "light" ? "lightgradient" : "darkgradient";
		
        if ($("#" + gaugeid).parent().hasClass(frameclass + "lightgradient"))
            $("#" + gaugeid).parent().removeClass(frameclass + "lightgradient");
	    else if ($("#" + gaugeid).parent().hasClass(frameclass + "lightgradient1"))
           $("#" + gaugeid).parent().removeClass(frameclass + "lightgradient1");
        else if ($("#" + gaugeid).parent().hasClass(frameclass + "darkgradient"))
            $("#" + gaugeid).parent().removeClass(frameclass + "darkgradient");
		else if ($("#" + gaugeid).parent().hasClass(frameclass + "darkgradient1"))
            $("#" + gaugeid).parent().removeClass(frameclass + "darkgradient1");
        frameclass = gaugeid == "thermoLinear" ? "thermo" : frameclass;
        $("#" + gaugeid).parent().addClass(frameclass + framestyle);
    } 
    else
        $("#" + gaugeid).parent()[0].className = "";
}

function convertWidgets() {
    // declaration
     $(".cols-prop-area .e-btn").ejButton();
    $(".cols-prop-area .e-togglebtn").ejToggleButton();
    $(".cols-prop-area .e-chkbox").ejCheckBox();
    $(".cols-prop-area .e-radiobutton").ejRadioButton();
    $(".cols-prop-area .e-ddl").ejDropDownList({ watermarkText: "Select" });
}

// Event Trace popup 

function adjustpopupposition(args) {
    var offset = $("#selectControls_dropdown").offset();
    var height = $("#selectControls_wrapper").height();
    $("#selectControls_popup_list_wrapper").css("top", (offset.top + height + 14) + "px");
    var left = (offset.left + $("#selectControls_dropdown").width()) - $("#selectControls_popup_list_wrapper").width() - 12;
    $("#selectControls_popup_list_wrapper").css("left", left + "px");
}

// For event tracer
jQuery.addEventLog = function (eventLog) {

    var divEvtTrace = null;
    if (!$(".sf-event-panel").length > 0) {
        divEvtTrace = ej.buildTag("div.divEventTrace sf-event-panel");
    }
    else {
        divEvtTrace = $(".sf-event-panel");
    }
    $("#EventLog").append(divEvtTrace);
    var eventHtml = "<span class='eventLog'>" + eventLog + "</span><hr>";
    $(eventHtml).prependTo($("div.divEventTrace")).slideDown(300, "easeOutQuad");
    if ($('input[name=themevarient]:checked').length>0 && $('input[name=themevarient]:checked')[0].id == "darktext")
        $(".eventtracesection hr").addClass("dark-hr");
    else
        $(".eventtracesection hr").removeClass("dark-hr");
};

jQuery.clearEventLog = function () {
    $("div.divEventTrace").html("");
}