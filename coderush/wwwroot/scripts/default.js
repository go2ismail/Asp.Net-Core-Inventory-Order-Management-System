var currentSamplepage = undefined, SamplesList = null, editcontrolpath = null, selthemecolor = null, cssheight = null, popupObj, resMenu_obj = null;
var loadedScripts = [];
var isPopupclosed = false, isContentMinimized = false;
window.theme = window.location.hash == "" ? "bootstrap" : window.location.hash.split("/")[1].replace("dark", "");
var absoluteUrl = window.location.toString().replace(window.location.hash, "").replace("default.htm", ""), angular;
window.themecolor = ""; window.themestyle = ""; window.themevarient = "", removeGroup = false; var isFullPageLoad = true;
isPopupOpened = false;
var culture_switch = [{ id: "1", locale: "en-US" }, { id: "2", locale: "de-DE" }, { id: "3", locale: "es-ES" }, { id: "4", locale: "fr-FR" }, { id: "5", locale: "zh-CN" }];
window.isMac = function () {
    return (/(ipad|iphone|ipod touch)/i).test(navigator.userAgent.toLowerCase()) && !(/trident|windows phone/i.test(navigator.userAgent.toLowerCase()));
}
_isAndroid = function () {
    return ((/android/i.test(navigator.userAgent.toLowerCase())) && !this.isWindows());
}
isWindows = function () {
    return (/trident|windows phone/i.test(navigator.userAgent.toLowerCase()));
}
isDevice = ej.isDevice() && ej.isTouchDevice();
window.baseurl = "//js.syncfusion.com/ejServices/";
window.biurl = "//bi.syncfusion.com/";
initControls = function () {
    $("#culture_switcher").ejDropDownList({ enablePersistence: true, height: "27px", width: "83px", value: "en-US", dataSource: culture_switch, fields: { id: "id", text: "locale", value: "locale" }, select: "culture_switcher", showRoundedCorner: true });
    $.ajax({
        url: 'scripts/cultures/ej.culture.' + $("#culture_switcher").val() + '.min.js',
        async: true
    });
    $.ajax({
        url: 'scripts/l10n/ej.localetexts.' + $("#culture_switcher").val() + '.min.js',
        async: false
    });
    ej.setCulture($("#culture_switcher").val());
    $(".nav_btn").ejButton({
        size: "mini",
        cssClass: "metroblue",
        contentType: "imageonly",
    });
    $("#newsamplewindow").ejButton("option", "prefixIcon", "e-icon newwindow");
    $("#prev_btn").ejButton({
        prefixIcon: "e-icon e-rarrowleft-2x",
        click: function (evt, args) {
            var index = $("#samplesDiv .SB-hashcollection").index($("#samplesDiv .highlighttextbg").parent());
            if (index > 0) {
                var hashBang = $($("#samplesDiv .SB-hashcollection")[index - 1]).attr("hashbang");
                queryChange(hashBang);
            }
        }
    });
    $("#next_btn").ejButton({
        prefixIcon: "e-icon e-rarrowright-2x",
        click: function (evt, args) {
            var index = $("#samplesDiv .SB-hashcollection").index($("#samplesDiv .highlighttextbg").parent());
            if (index < $("#samplesDiv .SB-hashcollection").length - 1) {
                var hashBang = $($("#samplesDiv .SB-hashcollection")[index + 1]).attr("hashbang")
                queryChange(hashBang);
            }
        }
    });
    $(".control-panel").ejWaitingPopup({ template: $("#sbwaitingTemplate") });
    popupObj = $(".control-panel").ejWaitingPopup("instance");
    popupObj.maindiv.addClass("sbloadingpopup");
    $("#sourceTab").ejTab({ cssClass: "metroblue", enableTabScroll: false });
    $("<span>").attr("id", "edit-wrapper").appendTo("#sourceTab ul");
    $("<button>").attr("id", "EditWindow").attr('title', 'Edit in JS Playground').appendTo($("#edit-wrapper"));
    $("<button>").attr("id", "copyclipboard").attr('title', 'Copy').appendTo($("#edit-wrapper"));
    $("<span id='copy-alert' class='hideEl'>Copied to Clipboard</span>").appendTo($("#edit-wrapper"));
    $("#copy-alert").bind('oanimationend animationend webkitAnimationEnd', function () {
        $("#copy-alert").removeClass("elementToFadeIn elementToFadeOut");
    });
    $("#EditWindow").ejButton({
        size: "small",
        cssClass: "copyedit e-flat",
        text: "Edit",
        contentType: "textandimage",
        prefixIcon: "e-icon newsrcwindow"
    });
    $("#copyclipboard").ejButton({
        size: "small",
        cssClass: "copyedit copycode e-flat",
        text: "Copy",
        contentType: "textandimage",
        prefixIcon: "e-icon  copycodeicon"
    });
}
$(function () {
    initControls();
    function iOSversion() {
        if (/iP(hone|od|ad)/.test(navigator.platform)) {
            var v = (navigator.appVersion).match(/OS (\d+)_(\d+)_?(\d+)?/);
            return [parseInt(v[1], 10), parseInt(v[2], 10), parseInt(v[3] || 0, 10)];
        }
    }
    iosVer = iOSversion();
    iosVersion = 84;
    isIosVersion8_4 = ej.isDevice() && iosVer != undefined && iosVersion == "" + iosVer[0] + iosVer[1];
    window.isTransitionSupported = "transition" in document.body.style;
    var sbModel = new ViewModel(), isloadLeft = false;
    if (window.isMac())
        window.baseurl = "//js.syncfusion.com/ejServices/";
    else
        window.baseurl = window.baseurl;


    $("body").addClass(window.theme);
    if (isDevice || window.innerWidth < 480) $("body").addClass("mobile");
    bindUnbindDocClickEvents(true);
    $(".samplebutton .anchorclass").ejButton({ showRoundedCorner: false });
    $(".codebutton .anchorclass").ejButton({ contentType: "imageOnly", prefixIcon: "e-icon codeimg", showRoundedCorner: false });
    window.Path && Path.map("#!/:theme/:control(/:category1)(/:category2)(/:category3)").to(function () {
        var control = "", categories = [], currentSample, theme = "", prePath = "", curPath = Path.routes.current.split("/");
        if (Path.routes.previous)
            prePath = Path.routes.previous.split("/")[1];
        for (var prop in this.params) {
            if (prop === "theme") {
                window.theme = this.params[prop];
                continue;
            }
            else if (prop === "control") {
                control = this.params[prop];
                continue;
            }
            categories.push(this.params[prop]);
        }
        if (window.theme.indexOf("dark") != -1 || window.theme.indexOf("high-contrast") != -1) {
            $(document.body).addClass("darktheme");
            window.theme = window.theme.replace("dark", "").replace("gradient", "");
        }
        if (window.theme.indexOf("gradient") != -1)
            window.theme = window.theme.replace("gradient", "");
        currentSample = categories.pop();
        try {
            currentSample = decodeURIComponent(currentSample)
        }
        catch (e) {
            currentSample = encodeURIComponent(currentSample);
            currentSample = decodeURIComponent(currentSample);
        }
        if (control !== "" && isFullPageLoad) {
            loadSamplePage(control, currentSample, categories);
            updateSBTheme();
        }
        else if (control !== "") preLoadSample(control, currentSample, categories);
        if (!curPath[1])
            location.reload();
        else if ((prePath === curPath[1]) && curPath.length <= 3)
            editItem("currentheader", "back");
        if (isPopupclosed)
            hideEJ2PopUp();
        else {
            //refresh Theme menu, control list, container
            var popupHt = $("#ej2popup").height(), themeMenu, contHt;
            themeMenu = window.innerWidth < 981 ? $("#res_themebutton ul") : $("#themebutton ul")
            if (!isContentMinimized) {
                themeMenu.height(themeMenu.height() - popupHt);
                contHt = $(".scrollit").height();
                $(".scrollit").height(contHt - popupHt);
                isContentMinimized = true;
            }
        }
    });
    $.views.converters("ensureURL", function (val) {
        return val.toLowerCase();
    });
    $.views.converters("Upperstring", function (val) {
        return val.toUpperCase();
    });
    $.views.converters("RemoveWhiteSpace", function (val) {
        return val.replace(/ /g, "");
    });
    var updateSBTheme = function () {
        $("body").removeClass('azure saffron high-contrast-01 high-contrast-02 material office-365 bootstrap azuredark gradientlime lime limedark saffrondark bootstrap').addClass(window.theme);
    };

    var preLoadSample = function (control, currentSample, categories, sample) {
        if (window.innerWidth < 981)
            $(".accordion-panel").css("left", "-250px");
        popupObj.show();
        var sample = sample || findSample(control, currentSample, categories), name = sample, parentId = null;
        while (name.samples) {
            if (name.samples.length) {
                parentId = parseInt(name.id) ? name.id : null;
                window.sample = name = $.extend(true, [], name.samples[0]);
                currentSample = name.querystring;
            } else
                break;
        }

        self.removeSelectedItemcss(control, name.id);
        if (control == "angularsupport" || control == "knockout") var sampleurl = currentSample + "/" + control + ".html";
        else var sampleurl = control + "/" + currentSample + ".html";
        self.loadSamplePage(sampleurl, control, currentSample, parentId, name.id, sample.deps, sample.depscss);
        $(".sample_wrapper").show();
        $('.control_ref').removeClass('hidepage');
        $('#ref_document').attr('href', sample.ugurl ? sample.ugurl : '//help.syncfusion.com/js');
        $('#ref_forums').attr('href', sample.forumurl ? sample.forumurl : '//www.syncfusion.com/forums/javascript');
        $('#ref_kb').attr('href', sample.kburl ? sample.kburl : '//www.syncfusion.com/kb/javascript');
        var curr;
        if (curr = ($("#samplesDiv").children("#" + control + "__sb").find("div[querystring=" + currentSample + "]"))) {
            curr.children('span.anchor').addClass("itemselected");
            curr.addClass("highlighttextbg selecteddashboard");
        }
        if (!isDevice && $(".highlighttextbg.selecteddashboard").length == 1 && $(".highlighttextbg.selecteddashboard")[0].offsetTop >= 0) {
            var scrollercontrol = $("#scrollcontainer").ejScroller("instance");
            scrollercontrol.model.scrollTop = $(".highlighttextbg.selecteddashboard")[0].offsetTop;
            scrollercontrol.refresh();
        }
        isFullPageLoad = true;
    };

    var loadSamplePage = function (control, currentSample, categories) {
        if (isDevice || window.innerWidth < 480) $("body").addClass("mobile");
        if (ej.isNullOrUndefined(control)) {
            $("#samplesDiv").css("display", "none");
            $(".samples").css("display", "none");
            $("sameplefile").css("visibility", "hidden")
            $("#samplefile").attr('src', 'about:blank');
            self.loadSBMainPage(null);
            bindUnbindDocClickEvents(true);
            document.title = "Essential Studio for JavaScript";
            return;
        }
        if (currentSample.indexOf('?') === 0)
            currentSample = "";
        var sample = findSample(control, currentSample, categories), isExist = (sample.name && sample.samples.length);
        if (isloadLeft || Number(name.id) <= 1) {
            if (isExist) {
                self.loadLeftTab(control + "__sb", currentSample);
            }
            else
                self.loadErrorPage();
        }
        if (isExist)
            preLoadSample(control, currentSample, categories, sample);
    };

    var findSample = function (control, currentSample, categories) {
        var sample = ej.Query().using(ej.DataManager(SamplesList)).where("id", "==", control, true), current = sample, res;

        for (var k = 0; k < categories.length; k++) {
            current.hierarchy((current = ej.Query("samples").where("url", "==", categories[k], true)));
        }

        if (currentSample)
            current.hierarchy(ej.Query("samples").where("url", "==", currentSample, true));

        res = sample.executeLocal();

        if (res.length) res = res[0];
        return res;
    };
    $("#Res-prodnav").on("click", function (e) {
        if ($(window).width() < 900) {
            $(".product-naviation").toggleClass("hideIt");
            $(".product-naviation").css("margin-left", ($(window).width() - 170));
        }
        isPopupOpened = isPopupOpened ? isPopupOpened : !$(".product-naviation").hasClass("hideIt");
        samplelisthide();
        bindUnbindDocClickEvents(isPopupOpened);
        e.stopPropagation();
    });
    $("#scrollcontainer").on("click", ".secondlevelload, .anchorclass", function (e) {
        if ($(e.currentTarget).hasClass("samples")) isFullPageLoad = false;
        var hashBang = $(e.currentTarget).attr("hashbang");
        popupObj.hide();
        if (window.sample && !(window.location.hash.indexOf(window.sample.name.toLowerCase()) > -1)) {
            $('#samplefile').css('visibility', 'hidden');
            $('#sourceTab').css('visibility', 'hidden');
            $("#edit-wrapper span.e-icon").css('visibility', 'hidden');
        }
        if (hashBang) {
            queryChange(hashBang);
            if (!isDevice && $(e.target).closest(".anchorclass.mainlevel").length == 1)
                setTimeout(function () { $("#scrollcontainer").ejScroller({ scrollTop: 0 }); }, 100);
        }
        var viewportWidth = $(window).width();
    });
    $("#slideMenu").ejButton({
        size: "mini",
        width: "36px",
        height: "36px",
        cssClass: "metroblue",
        contentType: "imageonly",
        prefixIcon: "e-icon slidepanel-nav",
        showRoundedCorner: true
    });
    var browser = navigator.userAgent.match(/(msie) ([\w.]+)/i);
    $("#themebutton").ejMenu({
        fields: { dataSource: menuData },
        openOnClick: true,
        width: 62,
        cssClass: "e-custom-theme",
        click: "themebtnClick",
        isResponsive: false
    });
    Menu_obj = $("#themebutton").data("ejMenu");
    resizeMenu = function () {
        var resheader, menu = window.innerWidth < 981 ? $("#res_themebutton ul") : $("#themebutton ul");
        menu.css("overflow-y", "scroll");
        var height_1 = (window.innerHeight || document.documentElement.clientHeight) - $(".header").outerHeight();
        height_1 > 520 ? menu.height(height_1 - 10) : menu.height(height_1 - $(".res_header").outerHeight() - 10);
        Menu_obj._closeAll();
        resMenu_obj._closeAll();
    }

    $("#res_themebutton").ejMenu({
        fields: { dataSource: menuData },
        openOnClick: true,
        width: 35,
        cssClass: "e-custom-theme",
        click: "themebtnClick",
        isResponsive: false,
        showRoundedCorner: true,
    });
    resMenu_obj = $("#res_themebutton").data("ejMenu");
    //$(".sample_wrapper:visible").css("min-height", window.innerHeight - $(".header").height() - 10 + "px");
    resizeMenu();
    $("#res_themebutton >li >a > .e-icon:first").addClass("bootstrap");
    $("#themebutton >li >a > .e-icon:first").addClass("bootstrap");
    if (!ej.isNullOrUndefined(browser) && browser[1].toLowerCase() == "msie" && browser[2] == 8) {
        $(".res_header .e-custom-theme").addClass('e-hide');
        $(".e-popup-first").css("font-size", "14px");
        $(".e-popup-second").css("font-size", "14px");
    }
    $("#buybutton").ejButton({
        size: "normal",
        width: "45px",
        height: "55px",
        cssClass: "metroblue",
        contentType: "imageonly",
        prefixIcon: "e-icon e-uiLight e-icon-handup"
    });

    $("#themestudio").ejButton({
        size: "normal",
        width: 165,
        height: 30,
        cssClass: "themestudio",
        showRoundedCorner: true,
        prefixIcon: "e-icon themestudio-logo",
        contentType: "textandimage",
    });

    $("#ej2button").ejButton({
        size: "normal",
        width: 150,
        height: 30,
        cssClass: "ej2button",
        showRoundedCorner: true,
        prefixIcon: "e-icon ej2button-logo",
        contentType: "textandimage",
    });

    $("#learnmore").ejButton({
        size: "normal",
        width: 100,
        height: 28,
        cssClass: "learnmore",
        showRoundedCorner: true,
        contentType: "text",
        click: function (evt, args) {
            hideEJ2PopUp();
        }
    });

    $("#popupclose").ejButton({
        size: "normal",
        width: 30,
        height: 30,
        cssClass: "popupclose",
        showRoundedCorner: true,
        prefixIcon: "e-icon ej2popupclose",
        contentType: "imageonly",
        click: function (evt, args) {
            hideEJ2PopUp();
        }
    });

    function hideEJ2PopUp() {
        var isAlreadyClosed = false;
        if ($("#ej2popup").is(":hidden"))
            isAlreadyClosed = true;
        isPopupclosed = true;
        $("#popupclose").hide();
        $("#ej2popup").slideUp("slow", function () {
            $("#ej2popup").hide();
        });
        if (!isAlreadyClosed) {
            //refresh Theme menu, control list, container
            var popupHt = $("#ej2popup").height(), themeMenu, scrollHt, scrollObj = $("#scrollcontainer").data("ejScroller");
            themeMenu = window.innerWidth < 981 ? $("#res_themebutton ul") : $("#themebutton ul")
            if (scrollObj) {
                scrollObj.option("height", scrollObj.model.height + popupHt);
                scrollObj.refresh();
            }
            else {
                scrollHt = $("#scrollcontainer").height();
                $("#scrollcontainer").height(scrollHt + popupHt)
            }
            themeMenu.height(themeMenu.height() + popupHt);
            isContentMinimized = false;
            contHt = $(".scrollit").height();
            $(".scrollit").height(contHt + popupHt);
        }
    }
    $(".product-naviation div").on("click", function (e) {
        var navText = e.target.textContent || e.target.innerText;
        viewdemo(navText);
        $(".productlogo").removeClass().addClass("productlogo");
        $(".productlogo").addClass(navText.toLowerCase() + "logo");
    });
    $("#sbtooltipbox").ejDialog({ height: "86px", width: "176px", enableResize: false, showOnInit: false, showHeader: false, cssClass: "metroblue", allowKeyboardNavigation: false });
    $("#themeDialog").ejDialog({ height: "160px", enableResize: false, showOnInit: false, showHeader: false, cssClass: "metroblue" });
    themeButtonSelect();
    var eleID = (window.themestyle == "" ? "flat" : window.themestyle) + window.themecolor + (window.themevarient != "light" ? window.themevarient : "");
    var menuObj = $("#themebutton").ejMenu('instance');
    var hiddenMenuObj = $("#themebutton").ejMenu('instance');
    jQuery.each(menuObj.element.find('li.e-list'), function (i, val) {
        if ($(val).attr('id') === eleID) $(val).addClass('e-active');
    });
    jQuery.each(hiddenMenuObj.element.find('li.e-list'), function (i, val) {
        if ($(val).attr('id') === eleID) $(val).addClass('e-active');
    });
    var metroradio = $("#JobSearch").data("ejMenu");
    var firstlevelsamples = [];

    var isContentPageLoaded = null;
    var index = 0;

    function editItem(id, back) {
        var divid = id;
        self.removeSelectedItemcss();
        url = window.location.pathname;
        $("#subsamplesDiv").hide().css("left", "250px");
        //After the sample page load footer will be loaded.
        $("#footer").hide();
        $(".cols-iframe,#sourceTab, .sample_wrapper").hide();
        queryChange(undefined, { "key": "" });
        loadSamplePage();
        setTimeout(function () {
            if (!isDevice) {
                refreshScroller();
                $("#scrollcontainer").ejScroller({ scrollTop: 0 });
            }
            else
                $("#scrollcontainer").css({ "height": window.innerHeight - $("#scrollcontainer").offset().top, "overflow-y": "auto" });
            if (isTransitionSupported) $(".accordion-panel").css("left", "0px");
            else $(".accordion-panel").animate({ "left": "0px" }, 350);
            $("#footer").show();
        }, 0);
    }
    function ViewModel() {
        this.Controls = SamplesList;
        this.controlname = "";
        this.controlName = null;
        this.sampleName = null;

        self.editSubItem = function (id, back) {
            var divid = id;
            removeSelectedItemcss();
            $("#subsamplesDiv").html('');
            $("#" + divid).css("margin-top", "0px");
            $("#subsamplesDiv").hide().css("left", "250px");
            $("#samplesDiv").css("left", "0px").show();
            $("#" + divid).show().css("visibility", "visible");
            $("#" + divid + "_back").addClass("dashboarddiv");
            $("#" + divid).children(".subsamples").find("a >div").removeClass("dashboardhover");
            $("#samplesDiv").find("#" + divid).css("left", "-250px");
            $("#samplesDiv").find("#" + divid).children(".subsamples").show();
            $("#samplesDiv").find("#" + divid).animate({ left: '0px' }, 200);
            var samplename = null, controlname = null;
            //var scroller = $("#scrollcontainer").data("ejScroller");
            //scroller.setModel({ cssClass: "metroblue" });
            //scroller.refresh();
            window.location.hash = window.location.hash.replace(/(#!\/[^\/]+)\/.+/, "$1");
            window.location.hash = window.location.hash + "/" + divid;
            refreshScroller();
        };

        self.loadLeftTab = function (divid) {
            if ($("#" + divid).prev().length > 0) {
                $("#" + divid).css("margin-top", "0px");
            }
            $("#accordion2").css("left", "-250px");
            $("#accordion2").prev().css("display", "none");
            $("#accordion2").css("display", "none");
            $("#subsamplesDiv").css("display", "none");
            $(".samples").hide();
            $("#samplesDiv").children("#" + divid).css("display", "block");
            $("#samplesDiv").children("#" + divid).children(".subsamples").show();
            $("#samplesDiv").children("#" + divid).children(".subsamples").find("#subControls").hide();
            $("#" + divid + "_back").css("display", "block");
            $("#" + divid + "_header").css("display", "block");
            $("#samplesDiv").css("display", "block");
            $("#samplesDiv").css("margin-top", "0px");
            $("#samplesDiv").animate({ left: '0px' }, 0);
            $('html, body').animate({
                scrollTop: 0
            }, 0);
            //code for hiding subheaders
            if ($("#currentheader").length > 0)
                $("#currentheader").remove();
            var element = $("#" + divid + "_back").clone();
            $(element).appendTo("#dashboardheader").attr("id", "currentheader");
            $("#sbdashboard").hide();
            if ($("#dashboardheader .current_control").length > 0)
                $("#dashboardheader .current_control").remove();
            $($("#" + divid + "_header").clone()).insertAfter("#currentheader").addClass("current_control").attr("id", "current_control");
            $("#" + divid + "_header").hide();
            $("#" + divid + "_back").hide();
            refreshScroller();
        };


        self.findSample = function (ctrlname, samplename, subchild, currentsampleid) {
            var query = ej.Query().using(ej.DataManager(SamplesList))
                .where("id", "==", ctrlname), curr = query, res;

            if (subchild)
                query.hierarchy(
                    curr = ej.Query("samples")
                        .where("id", "==", subchild));

            curr.hierarchy(
                ej.Query("samples")
                    .where("id", "==", currentsampleid)
            );

            res = query.executeLocal();

            if (res.length) res = res[0];
            return res;

        },
        self.getSampleContent = function (url, control) {
            var content, urlSplt;
            ej.widget.destroyAll($("#samplefile .e-js"));
            $("#samplefile").empty();
            window.sample.name = window.sample.name.replace(" ", "")
            if (control == "unobtrusive") {
                if (!$("#iframecontent").length) {
                    iframe = document.createElement("iframe");
                    $(iframe).attr("id", "iframecontent").appendTo("#samplefile");
                }
                if ((window.theme.indexOf("high-contrast") > -1 || window.themevarient.indexOf("light") > -1) && window.themestyle !== "gradient")
                    url = url
                else if (window.themestyle === "gradient") {
                    urlSplt = url.split("theme=");
                    url = window.themevarient.indexOf("light") > -1 ? urlSplt[0] + "theme=" + window.themestyle + urlSplt[1] : urlSplt[0] + "theme=" + window.themestyle + urlSplt[1] + window.themevarient;
                }
                else
                    url = url + window.themevarient;
                $("#iframecontent").attr("src", url);
                iframe.onload = function () {
                    newheight = (!ej.isNullOrUndefined(window.currentSamplepage) && window.currentSamplepage.indexOf("adaptive") < 0) ? 530 : $(document.getElementById("iframecontent").contentWindow.document.body).height();
                    $("#iframecontent").height(newheight + 20);
                    popupObj.hide();
                }
                var splitbutton = $(".cols-sample-area").find(".e-splitbutton.e-js");
                if (splitbutton.length) {
                    for (var i = 0; i < splitbutton.length; i++) {
                        var splitObj = $(splitbutton[i]).ejSplitButton("instance");
                        splitObj._destroy();
                        splitObj._init();
                    }
                }
            }
            else {
                $.ajax({
                    url: absoluteUrl + editcontrolpath,
                    dataType: "html",
                    cache: true,
                    success: function (str, status, xhr) {
                        try {
                            xhr.onreadystatechange = null;
                            xhr.abort();
                        } catch (e) {
                        }
                        if (sample.cultureSwitcher != "disable") {
                            $("#cul_switch").css("visibility", "visible");
                            ej.setCulture($("#culture_switcher").val());
                        }
                        else {
                            $("#cul_switch").css("visibility", "hidden");
                            ej.setCulture("en-US");
                        }

                        var title = str.match(/<(title)>[\s\S]*?<\/\1>/gi);
                        if (!ej.isNullOrUndefined(title)) {
                            title = title[0].replace('<title>', '').replace('</title>', '');
                            document.title = title;
                        }
                        else
                            document.title = "Essential Studio for JavaScript";
                        str = str.match(/<(body)>[\s\S]*?<\/\1>/gi);
                        str = str[0].replace('<body>', '').replace('</body>', '');
                        str = str.replace(new RegExp("../content/images", 'g'), "content/images");
                        $("#samplefile").html(str);
                        setTimeout(function () {
                            if (window.theme == "material" || window.theme == "office-365" || window.theme == "bootstrap") {
                                var slider = $(".cols-sample-area").find(".e-slider.e-js");
                                if (slider.length) {
                                    for (var i = 0; i < slider.length; i++) {
                                        var sliderObj = $(slider[i]).data("ejSlider");
                                        if (sliderObj.model.showScale && !sliderObj.model.showButtons) {
                                            sliderObj._destroyScale();
                                            sliderObj._renderScale(true);
                                        }
                                        if (sliderObj.model.showButtons) {
                                            sliderObj.option("showButtons", false);
                                            sliderObj.option("showButtons", true);
                                        }
                                    }
                                }
                            }
                        }, 500);
                        if ($(".cols-sample-area").next('div').length == 0)
                            $(".cols-sample-area").css("width", "100%");
                        if (status == "success")
                            popupObj.hide();
                    },
                    error: function (status) {
                        self.loadErrorPage();
                    },
                })
            }
        },

        self.loadSamplePage = function (url, ctrlname, samplename, subchild, currentsampleid) {
            currentSamplepage = url;
            if ($("#auto").data("ejAutocomplete"))
                $("#auto").ejAutocomplete("clearText");
            if (!$('body').hasClass('fixedlayout')) $('body').addClass('fixedlayout');
            var sample = self.findSample(ctrlname, samplename, subchild, currentsampleid), sampleTitle = "", cssDeps, scriptDeps;
            while (sample) {
                if (sampleTitle)
                    sampleTitle += " / ";
                sampleTitle += sample.name;
                cssDeps = sample.cssDeps;
                scriptDeps = sample.scriptDeps;
                sample = sample.samples && sample.samples[0];
            }

            var names = sampleTitle.split("/"), _samplename = names.pop();
            sampleTitle = names.join("/") + "/ ";
            $("#sampleheadingtext #controlname").html(sampleTitle);
            $("#sampleheadingtext #sbtxt").html(_samplename);
            self.setVisibility("productpage", "cols-iframe");
            $(".res_header .producttitle").hide();
            $(".res_header #sampleheadingtext").show();
            //Waiting popoup template
            isIosVersion8_4 ? "" : popupObj.show();
            //$(".sourcecodetab").hide();
            if (window.theme != "flat")
                url = url + "?theme=" + window.theme;
            editcontrolpath = url;
            if (!ej.isNullOrUndefined(cssDeps))
                for (var dep = 0 ; dep < cssDeps.length; dep++) {
                    var href = "content/" + cssDeps[dep] + ".css";
                    jQuery.ajax({
                        url: href,
                        cache: true,
                        success: function () {
                            $('<link rel="stylesheet" type="text/css" href="' + href + '" />').appendTo("head");
                        }
                    })
                }
            if (!ej.isNullOrUndefined(scriptDeps)) {
                var tempCount = 0;
                $.each(scriptDeps, function (index, val) {
                    var isExists = $.inArray(val, loadedScripts);
                    if (isExists > -1) {
                        tempCount++;
                        if (scriptDeps.length == tempCount)
                            getSampleContent(editcontrolpath, ctrlname);
                        return;
                    }
                    temp = index;
                    jQuery.ajax({
                        url: "scripts/" + val + ".js",
                        dataType: "script",
                        cache: true,
                        success: function (data) {
                            loadedScripts.push(val);
                        },
                        complete: function (xhr, status) {
                            if (status == "success")
                                tempCount++;
                            if (status == "success" && scriptDeps.length == tempCount)
                                getSampleContent(editcontrolpath, ctrlname);
                        }
                    })
                })
            }
            else
                getSampleContent(editcontrolpath, ctrlname);
            index = 0;
            if (currentSamplepage != undefined && !$("#sbdashboard").is(":visible"))
                self.loadSourceCodeTab(currentSamplepage);
            $('#samplefile').css('visibility', 'visible');
            $('#sourceTab').css('visibility', 'visible');
            $("#edit-wrapper span.e-icon").css('visibility', 'visible');
            $("#newsamplewindow").attr('href', currentSamplepage);
        };



        self.loadSBMainPage = function (divid, isErr) {
            $(".samplesection").hide();
            $('.control_ref').addClass('hidepage');
            if (divid != null) {
                $("#" + divid).css("visibility", "hidden");
                $("#" + divid + "_back").css("display", "none");
            }
            //code for removing the firstlevelback and secondlevelback header.
            $("#sbdashboard").show();
            if ($("#dashboardheader .firstlevelback"))
                $("#dashboardheader .firstlevelback").remove();

            if ($("#dashboardheader .current_control"))
                $("#dashboardheader .current_control").remove();
            self.loadSBPage(divid, isErr);
        };
        self.loadSBPage = function (divid, isErr) {
            $("#accordion2").prev().css("display", "block");
            $("#accordion2>a>.dashboardhover").removeClass("dashboardhover");
            $(".sourcecodetab").hide();
            if (divid != null)
                $("#" + divid).hide();
            $("#accordion2").show();
            $("#samplesDiv").css("left", "250px");
            $("#accordion2").animate({ left: 0 }, 200, function () {
                self.setVisibility((isErr ? "productpage" : "cols-iframe"), (isErr ? "cols-iframe" : "productpage"));
                $(".res_header #sampleheadingtext").hide();
                $(".res_header .producttitle").show();
            });
            $('html, body').animate({
                scrollTop: 0
            }, 0);
        };

        self.loadErrorPage = function () {
            $("#samplesDiv").css("display", "none");
            $(".samples").css("display", "none");
            loadSBMainPage(null, true);
            document.title = "Essential Studio for JavaScript";
            $("#sourceTab").hide();
            $(".sample_wrapper").show();
            $("#samplefile").html("<div class='row'><div class='col-md-10' style='font-size:25px; font-weight:500; color: rgb(236, 24, 24);'>Page not found</div></div><br><div style='font-size:15px; margin-left: 5px'><div class='row'>The page you are looking for is not found</div><div class='row' style='margin-top:2px'>" + window.location.href + "</div></div>");
            if (!isDevice) {
                refreshScroller();
                $("#scrollcontainer").ejScroller({ scrollTop: 0 });
            }
            popupObj.hide();
        };

        var count = 0;
        self.loadSourceCodeTab = function (url) {
            window.htmlEditor = [];
            $.ajax({
                cache: true,
                url: url,
                dataType: "html",
                success: function (data) {
                    var items = $("#sourceTab .e-item");
                    tabObj = $("#sourceTab").data("ejTab");
                    for (var t = $("#sourceTab .e-item").length; t > 0 ; t--) {
                        tabObj.removeItem(t);
                    }
                    if ($("#htmlpage").length == 0) {
                        var divTag = ej.buildTag("div#htmlpage");
                        $("#sourceTab").append(divTag);
                        tabObj.addItem("#htmlpage", window.sample.name, 0);
                        tabObj.setModel({ selectedItemIndex: 0 });
                    }
                    else {
                        $($("#sourceTab .e-item a")[0]).text(window.sample.name)
                    }
                    $("#htmlpage").empty();
                    $("#sourceTab").show();
                    window.htmlEditor.push(CodeMirror.fromTextArea($("#htmlpage").val(data)[0], {
                        lineNumbers: false,
                        readOnly: true,
                        mode: "text/html"
                    }));
                    $("#htmlpage").next($(".CodeMirror")).appendTo("#htmlpage");
                    //count++;
                    $(".sourcecodetab").show();
                    $(".CodeMirror").each(function (i, obj) {
                        if (i > 0)
                            $(obj).remove();
                    });
                    $("#htmlpage").show();
                    $("#sourceTab .CodeMirror").find('textarea').attr('readonly', 'readonly');
                    $("<div>").insertAfter($("#sourceTab")).attr("id", "clipboard");
                    $("<textarea>").attr("style", "display:none").attr("id", "copytextarea").appendTo($("#clipboard"));
                    if (window.sample.additionalTabs) {
                        setTimeout(function () { self.ensureAdditionalTab() }, 0);
                    }
                    initiateCopyHandler();
                }
            });
        };
        self.ensureAdditionalTab = function () {
            if (window.sample.additionalTabs.length > 0) {
                for (var i = 0; i < window.sample.additionalTabs.length; i++) {
                    var tabid = window.sample.additionalTabs[i].displayName.replace(" ", "");
                    $("#sourceTab").ejTab("addItem", "#" + window.sample.additionalTabs[i].displayName, window.sample.additionalTabs[i].displayName, 3, "", tabid);
                    var dataval = null, path = codeMirrorModes(window.sample.additionalTabs[i].filePath) == "text/x-csharp" ? absoluteUrl + "sourceCodehandler.ashx" : window.sample.additionalTabs[i].filePath;
                    if (path == absoluteUrl + "sourceCodehandler.ashx")
                        dataval = { 'url': window.sample.additionalTabs[i].filePath };
                    $.ajax({
                        url: path,
                        data: dataval,
                        cache: true,
                        success: function (data) {
                            var dataurl = $(this)[0].url, content = data;
                            dataurl = decodeURIComponent(dataurl).replace("sourceCodehandler.ashx?url=", "");
                            for (var tab = 0; tab < window.sample.additionalTabs.length; tab++) {
                                if (dataurl.indexOf(window.sample.additionalTabs[tab].filePath) > -1) {
                                    tabid = window.sample.additionalTabs[tab].displayName.replace(" ", "");
                                    window.htmlEditor.push(CodeMirror.fromTextArea($("#" + tabid).val(content)[0], {
                                        lineNumbers: false,
                                        readOnly: true,
                                        mode: codeMirrorModes(window.sample.additionalTabs[tab].filePath)
                                    }));
                                    break;
                                }
                            }
                            $("#" + tabid).parent().children(".CodeMirror").appendTo("#" + tabid);
                            $("#sourceTab .CodeMirror").find('textarea').attr('readonly', 'readonly');
                            initiateCopyHandler();
                        }
                    });
                }
            }
        };

        self.setVisibility = function (oldpage, newpage) {
            $("." + newpage).show();
            $("." + oldpage).hide();
        };
        this.getCssClass = function (item) {
            var value = Number(item.childcount);

            if (value >= 1) {
                return 'arrow';
            }
            return;
        };
        this.getCssVisibility = function (item) {
            var value = Number(item.id);

            if (value != 1) {
                return 'hideParent';
            }
            return 'dashboard';
        };
        self.removeSelectedItemcss = function (ctrlname, controlid) {
            var obj = $(".itemselected");
            obj.each(function () {
                $(obj).removeClass('itemselected');
                $(obj).parent().removeClass('highlighttextbg');
            });
            $(".selecteddashboard").removeClass("selecteddashboard");
        };
    }
    var Mainlist = [];

    $(GroupingList).each(function () {
        Mainlist[this.toString()] = [];
    });

    var samplesdetails = SamplesList;
    $(samplesdetails).each(function (index1, sampleslist) {
        if (sampleslist) {
            var smpls = {}, secsamples = [];
            smpls["id"] = sampleslist.id;
            smpls["name"] = sampleslist.name;
            smpls["type"] = sampleslist.type;
            smpls["url"] = SamplesList[index1]["url"] = sampleslist.querystring.split(" ").join("");
            smpls["publish"] = sampleslist.publish;
            smpls["isResponsive"] = sampleslist.isResponsive;
            $(sampleslist.samples).each(function (ind, subsampleslist) {
                if (subsampleslist) {
                    var subsmpls = {};
                    subsmpls["id"] = subsampleslist.id;
                    subsmpls["name"] = subsampleslist.name;
                    subsmpls["querystring"] = subsampleslist.querystring;
                    subsmpls["childcount"] = subsampleslist.childcount;
                    subsmpls["type"] = subsampleslist.type;
                    if (subsampleslist.publish)
                        subsmpls["publish"] = subsampleslist.publish;
                    else
                        subsmpls["publish"] = smpls["publish"];
                    if (subsampleslist.isResponsive)
                        subsmpls["isResponsive"] = subsampleslist.isResponsive;
                    else
                        subsmpls["isResponsive"] = smpls["isResponsive"];
                    subsmpls["url"] = SamplesList[index1].samples[ind]["url"] = subsampleslist.name.split(" ").join("");

                    if (subsampleslist.childcount == 1)
                        subsmpls["arrowclass"] = "arrow";
                    else
                        subsmpls["arrowclass"] = "";

                    $(subsampleslist.samples).each(function (j, thirdlist) {
                        if (thirdlist)
                            SamplesList[index1].samples[ind].samples[j]["url"] = thirdlist.name.split(" ").join("");
                    });
                    subsmpls["samples"] = subsampleslist.samples;
                    if (subsmpls["publish"] != 'online')
                        secsamples.push(subsmpls);
                }
            });
            smpls["samples"] = secsamples;
            var temp = this;
            $(GroupingList).each(function () {
                if (temp["Group"] == this.toString()) {
                    smpls.hashbang = smpls.id;
                    smpls.id = smpls.id + "__sb";
                    Mainlist[this.toString()].push(smpls);
                    return false;
                }
            });
        }
    });
    window.jsonline = ($.trim(window.location.host.toString()) == "js.syncfusion.com");
    $("#accordion2").html($("#accordionGroupTmpl").render(GroupingList));
    $(GroupingList).each(function () {
        var content = $("#accordionTmpl").render(Mainlist[this], { online: window.jsonline });
        if ($.trim(content) != "") {
            $(".SB-group." + this.toString().replace(/ /g, "")).html(content);
            $(Mainlist[this.toString()]).each(function () {
                firstlevelsamples.push(this);
            });
        }
        else
            $(".SB-group." + this.toString().replace(/ /g, "")).hide().prev().hide();
    });
    $("#samplesDiv").html($("#accordionTmplchild").render(firstlevelsamples, { online: window.jsonline }));
    if (!isDevice) $("#scrollcontainer").ejScroller({ buttonSize: 0, scrollerSize: 9 });
    else $("#scrollcontainer").css({ "height": window.innerHeight - $("#scrollcontainer").offset().top, "overflow-y": "auto" });

    $("#scrollcontainer .SB-groupIt").click(function (e) {
        if ($(this).hasClass("downArrow")) {
            $(this).next().hide(300, function () { refreshScroller() });
            $(this).removeClass("downArrow");
            $(this).addClass("rightArrow");
        }
        else {
            $(this).next().show(300, function () { refreshScroller() });
            $(this).removeClass("rightArrow");
            $(this).addClass("downArrow");
        }
        e.stopPropagation();
    });
    RefreshPoductNav();
    refreshScroller();
    refreshContentWindow();
    if (isPopupclosed)
        hideEJ2PopUp();
    else {
        //refresh Theme menu, control list, container
        var popupHt = $("#ej2popup").height(), themeMenu, contHt;
        themeMenu = window.innerWidth < 981 ? $("#res_themebutton ul") : $("#themebutton ul")
        if (!isContentMinimized) {
            themeMenu.height(themeMenu.height() - popupHt);
            contHt = $(".scrollit").height();
            $(".scrollit").height(contHt - popupHt);
            isContentMinimized = true;
        }
    }
    function refreshScroller() {
        if (!isDevice) {
            //scroller resize refresh
            $(".accordion-panel.cols-fixed-sidebar").height((($(window).height() - $(".sbheader").outerHeight())) + "px ;");
            var controlheight = $("#scrollcontainer").ejScroller("instance");
            var popupHt = $("#ej2popup").is(":visible") ? $("#ej2popup").height() : 0;
            var Minheight = ($(window).height()) - ($(".header").outerHeight() + $("#dashboardheader").outerHeight() + $(".accordion-panel .search").outerHeight() + popupHt);
            controlheight.model.height = Minheight;
            controlheight.refresh();
            $("#scrollcontainer .e-vhandle").addClass("e-box");
        }
    }
    function refreshContentWindow() {
        //resizing for content fluid width
        $(".scrollit").height($(window).height() - $(".sbheader").outerHeight() - 15);
        //$(".scrollit .row").css({ "min-height": $(window).height() - $(".sbheader").outerHeight() - $("#footer").outerHeight() - 15 });
    }

    $('.accordion-panel').on('click', 'div.firstlevelback', function (evt, args) {
        $(".e-waitpopup-pane").each(function (index, element) {
            if (!$(element).hasClass("sbloadingpopup"))
                $(element).remove();
        })
        editItem(evt.currentTarget.id, 'back');
    });
    function RefreshPoductNav() {
        $(".header").removeClass("Mobile").removeClass("Desktop");
        if ($(window).width() > 899)
            $(".header").addClass("Desktop");
        else
            $(".header").addClass("Mobile");
    }
    $(".dashboard").mouseover(function () {
        if (!$(".vHandle").is(":visible")) {
            if (window.themevarient.indexOf("dark") != -1 && !$(this).hasClass("dark-highlighttextbg"))
                $(this).addClass("dark-dashboardhover");
            else
                $(this).addClass("dashboardhover");
        }
    });
    $(".slidemenuicon").click(function (args) {
        if (!isPopupOpened) bindUnbindDocClickEvents(true);
        isPopupOpened = true;
        resMenu_obj._closeAll();
        if ($(".product-naviation").is(":visible")) $(".product-naviation").addClass("hideIt");
        if (isTransitionSupported) $('.accordion-panel').css("left", "0px");
        else $(".accordion-panel").animate({ "left": "0px" }, 350);
        $('.control-panel.cols-content-fluid').removeClass('accordionHide').removeClass('center-flow');
        move = 0;
        if (isDevice)
            $("#scrollcontainer").css({ "height": window.innerHeight - $("#scrollcontainer").offset().top, "overflow-y": "auto" });
        else
            refreshScroller();
        if (!removeGroup) {
            $(".accordion >.SB-group").each(function (i, item) {
                if (item.children.length == $(item).children(".irresponsive").length)
                    $(item.previousElementSibling).addClass("irresponsive");

            });
            removeGroup = true;
        }
        args.stopPropagation();
    });
    $("#samplefile").on(function (e) {
        $(document.getElementById("samplefile").contentWindow.document).click(function (e) {
            samplelisthide();
            if ($(".product-naviation").is(":visible")) $(".product-naviation").addClass("hideIt");
            if ($("#res_themebutton ul").is(":visible") || $("#themebutton ul").is(":visible")) {
                var resMenu_obj = window.innerWidth > 981 ? $("#themebutton").data("ejMenu") : $("#res_themebutton").data("ejMenu");
                resMenu_obj._closeAll();
            }
        });
    });

    $(".dashboard").click(function () {
        $(this).find(".anchor").addClass("itemselected");
        $(".dark-dashboard").hasClass("dark-highlighttextbg");
        $(".dark-dashboard").removeClass("dark-highlighttextbg");
        $(this).find(".itemselected").parent().addClass("highlighttextbg");

    });

    $(".dashboard").mouseout(function () {
        if ($(this).hasClass("dashboardhover"))
            $(this).removeClass("dashboardhover");
        else if ($(this).hasClass("dark-dashboardhover"))
            $(this).removeClass("dark-dashboardhover");
    });
    var collection = SamplesList;
    var subdata = [], i, j, k;

    for (i = 0; i < collection.length; i++) {
        if (!collection[i]) continue;
        if ((collection[i].publish == "online" && window.jsonline) || collection[i].publish != "online") {
            for (j = 0; j < collection[i].samples.length; j++) {
                if (!collection[i].samples[j]) continue;
                var properties = {}, len;
                var controlName = collection[i].id;
                if ((collection[i].samples[j].publish == "online" && window.jsonline) || collection[i].samples[j].publish != "online") {
                    if (!collection[i].samples[j].samples) {
                        properties["id"] = controlName + "/" + collection[i].samples[j].url;
                        properties["control"] = collection[i].name;
                        properties["text"] = collection[i].samples[j].name;
                        properties["data"] = collection[i].name + " " + collection[i].samples[j].name;
                        properties["isResponsive"] = collection[i].samples[j].isResponsive;
                        if (collection[i].isResponsive == "false") {
                            properties["isResponsive"] = "false";
                        }
                        subdata.push(properties);
                    }
                }
                if (collection[i].samples[j].childcount != "0") {
                    if (/msie 8.0/.test(navigator.userAgent.toLowerCase()))
                        len = collection[i].samples[j].samples.length - 1;
                    else
                        len = collection[i].samples[j].samples.length;
                    if (collection[i].samples[j].name == "Selection")
                        var s = true;
                    if ((collection[i].samples[j].publish == "online" && window.jsonline) || collection[i].samples[j].publish != "online") {
                        for (k = 0; k < len; k++) {
                            if (!collection[i].samples[j].samples[k]) continue;
                            var subprops = {};
                            var subcontrolName = collection[i].samples[j].url;
                            if ((collection[i].samples[j].samples[k].publish == "online" && window.jsonline) || collection[i].samples[j].samples[k].publish != "online") {
                                subprops["id"] = controlName + "/" + subcontrolName + "/" + collection[i].samples[j].samples[k].url;
                                subprops["control"] = collection[i].name;
                                subprops["text"] = collection[i].samples[j].name + "/" + collection[i].samples[j].samples[k].name;
                                subprops["data"] = collection[i].name + " " + collection[i].samples[j].name + "/" + collection[i].samples[j].samples[k].name;
                                subprops["isResponsive"] = collection[i].samples[j].isResponsive;
                                subdata.push(subprops);
                            }
                        }
                    }
                }
            }
        }
    }
    function navigationList() {
        $('#auto').ejAutocomplete({
            watermarkText: "Search here",
            showPopupButton: false,
            filterType: "Contains",
            dataSource: subdata,
            fields: { key: "id", text: "data" },
            width: "210px",
            popupHeight: "180px",
            template: '<div class="control_name ">${control}</div> <div class="control_samplename">${text}</div>',
            select: "onSelectSearchItem",
        });

        if ($(window).width() < 979) {
            searchObj = $("#auto").data("ejAutocomplete");
            OldData = searchObj.option("dataSource");
            var newData = $.grep(OldData, function (sample) {
                return sample.isResponsive != "false";
            });
            searchObj.option("dataSource", newData);
        }

        var autocompletScroller = $("#auto").data("ejAutocomplete").scrollerObj;
        autocompletScroller.model.buttonSize = 0;
        autocompletScroller.model.scrollerSize = 8;

        $(window).bind("resize", function (e) {
            var viewportWidth = $(window).width(), popupHt = $("#ej2popup").is(":visible") ? $("#ej2popup").height() : 0;
            viewportWidth < 981 ? $(".res_header").addClass("dashboardhover") : $(".res_header").removeClass("dashboardhover");
            refreshScroller();
            refreshContentWindow();
            $(".scrollit").height($(".scrollit").height() - popupHt);
            RefreshPoductNav();
            if ($("#sbtooltipbox").ejDialog("isOpened")) $("#sbtooltipbox").ejDialog("close");
            if (isDevice) $("#scrollcontainer").css({ "height": window.innerHeight - $("#scrollcontainer").offset().top, "overflow-y": "auto" });
            isDevice || window.innerWidth < 480 ? $("body").addClass("mobile") : $("body").removeClass("mobile")
            if (window.innerWidth >= 480)
                $("#copyclipboard .e-btntxt").text("Copy"); $("#newcodewindow .e-btntxt").text("New");
            if(!isDevice)
				samplelisthide();
            setTimeout(function () { resizeMenu() }, 0);
            popupObj && popupObj.refresh();
        });
        $(window).on("orientationchange", function () {
            setTimeout(function () {
                $("#scrollcontainer").css({ "height": window.innerHeight - $("#scrollcontainer").offset().top, "overflow-y": "auto" });
                resizeMenu();
            }, _isAndroid() ? 250 : 0)
        });
        if (Path.match(window.location.hash)) {
            $("#sbtooltipbox").ejDialog("close");
            isloadLeft = true;
            // $(".cols-iframe").show();
            showTooltipbox();
            var urlread = window.location.toString().toLowerCase();
            if (urlread.indexOf("reportviewer") > 0) {
                reCreateReportViewer();
            }
        }
        //var heightval = (window.innerHeight || $(window).height()) - ($(".header").outerHeight() + 5 + $(".search").outerHeight());
        //if(heightval<minScrollerHeight)
        //    heightval = minScrollerHeight;
        //else
        //    heightval = heightval - $("#footer").outerHeight();
        //var scroller = $("#scrollcontainer").width(253).data("ejScroller");
        //scroller.setModel({ height: heightval, width: 0, cssClass: "metroblue" });
        //scroller.refresh();
    }
    if (isIosVersion8_4) setTimeout(function () { navigationList(); }, 1000);
    else navigationList();

    $(".editcode").click(function () {
        $("#sbeditcodedialog").ejDialog(
                {
                    enableModal: true,
                    showOnInit: false,
                    allowDraggable: false,
                    width: "98%",
                    height: "95%",
                    cssClass: "metroblue",
                    enableResize: false,
                    content: "iframe",
                    contentUrl: "editcode.html?" + editcontrolpath,
                    title: "LIVE EDIT",
                    close: "onClose"
                });
        $("#sbeditcodedialog").show();
        $("#sbeditcodedialog").ejDialog("open");
        $('html, body').animate({
            scrollTop: 0
        }, 0);
        $("#sbeditcodedialog iframe").bind("load", function () {
            $('.liveeditctrls').insertAfter($("#sbeditcodedialog_wrapper .e-dialog-title")).addClass("showliveeditctrls");
            $("#sbeditcodedialog_wrapper .liveeditctrls #cssarea").ejCheckBox({ cssClass: "metroblue", "change": "oncssareaShowHide" });
            $("#sbeditcodedialog .e-iframe").contents().find("#apply").insertAfter($('.liveeditctrls'));
        });
    });
    Path.listen();

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
    ej.support.stableSort = false;
    if (window.innerWidth < 981 || isDevice) samplelisthide();
    $('body').css('visibility', 'visible');
});

function bindUnbindDocClickEvents(isOpened) {
    if (!isOpened) $(document).unbind("click");
    else
        $(document).bind("click", function (evt) {
            if ($("#sbtooltipbox").ejDialog("isOpened"))
                $("#sbtooltipbox").ejDialog("close");
            $(".product-naviation").addClass("hideIt");
            if (evt.target.nodeName != "INPUT" && !$(evt.target).hasClass("sbheading") && !$(evt.target).parents(".accordion-panel").length)
                samplelisthide();
            var resMenu_obj = window.innerWidth > 981 ? $("#themebutton").data("ejMenu") : $("#res_themebutton").data("ejMenu");
            resMenu_obj._closeAll();
            if (!$(evt.target).hasClass("sbheading") && evt.target.nodeName != "INPUT")
                bindUnbindDocClickEvents(false);
            else
                isPopupOpened = true;
            isPopupOpened = false;
            evt.stopPropagation();
        });
}
function culture_switcher(args) {
    $("#htmljssamplebrowser").ejWaitingPopup({ id: "htmljssamplebrowser", showOnInit: true, template: $("#waiting_temp") });
    location.reload();
}
function oncssareaShowHide(args) {
    if (args.isChecked) {
        if ($($("#ejcssarea .e-innerspan")[1]).hasClass("e-chk-inactive"))
            $($("#ejcssarea .e-innerspan")[1]).addClass("e-chk-active")

        $($("#sbeditcodedialog .e-iframe").contents().find("#spliter2 .v-pane")[1]).show();
        $("#sbeditcodedialog .e-iframe").contents().find("#spliter2 .v-bar").show();
        var cssheight = $($("#sbeditcodedialog .e-iframe").contents().find("#spliter1 .v-pane")[0]).css("height");
        $($("#sbeditcodedialog .e-iframe").contents().find("#spliter2 .v-pane")[0]).css("height", cssheight);
    }
    else {
        $($("#sbeditcodedialog .e-iframe").contents().find("#spliter2 .v-pane")[1]).hide();
        $("#sbeditcodedialog .e-iframe").contents().find("#spliter2 .v-bar").hide();
        cssheight = $($("#sbeditcodedialog .e-iframe").contents().find("#spliter2 .v-pane")[0]).css("height");
        $($("#sbeditcodedialog .e-iframe").contents().find("#spliter2 .v-pane")[0]).css("height", "100%");
    }
}
function preparetheme() {
    var themename = "";
    if (window.location.hash.indexOf('dark') != -1 && window.themevarient != 'light')
        window.themevarient = "dark";
    if (window.location.hash.indexOf('gradient') != -1 && window.themestyle != 'flat')
        window.themestyle = "gradient";
    if (window.themevarient == "dark") {
        if (window.themestyle == "gradient")
            themename = window.themestyle + window.themecolor + window.themevarient;
        else
            themename = (window.themecolor != "") ? window.themecolor + window.themevarient : window.themestyle + window.themevarient;

    }
    else {
        if (window.themestyle == "gradient")
            themename = window.themestyle + window.themecolor;
        else
            themename = (window.themecolor != "") ? window.themecolor : window.themestyle;
    }
    if (window.theme.indexOf("bootstrap") != -1) {
        themename = window.theme;
        window.themestyle = "flat";
        window.themevarient = "light";
    }
    if (window.theme.indexOf("high-contrast-01") != -1) {
        themename = window.theme;
        window.themestyle = "flat";
        window.themevarient = "dark";
    }
    if (window.theme.indexOf("high-contrast-02") != -1) {
        themename = window.theme;
        window.themestyle = "flat";
        window.themevarient = "dark";
    }
    if (window.theme.indexOf("material") != -1) {
        themename = window.theme;
        window.themestyle = "flat";
        window.themevarient = "light";
    }
    if (window.theme.indexOf("office-365") != -1) {
        themename = window.theme;
        window.themestyle = "flat";
        window.themevarient = "light";
    }
    window.theme = themename;
}

function updateTheme(selcolor) {
    preparetheme();
    replacebg(window.themevarient == "dark");
    replaceTheme();
}
var themes = {
    "flat": "content/ejthemes/default-theme/",
    "flatdark": "content/ejthemes/flat-azure-dark/",
    "azure": "content/ejthemes/default-theme/",
    "azuredark": "content/ejthemes/flat-azure-dark/",
    "lime": "content/ejthemes/flat-lime/",
    "limedark": "content/ejthemes/flat-lime-dark/",
    "saffron": "content/ejthemes/flat-saffron/",
    "saffrondark": "content/ejthemes/flat-saffron-dark/",
    "gradient": "content/ejthemes/gradient-azure/",
    "gradientdark": "content/ejthemes/gradient-azure-dark/",
    "gradientazure": "content/ejthemes/gradient-azure/",
    "gradientazuredark": "content/ejthemes/gradient-azure-dark/",
    "gradientlime": "content/ejthemes/gradient-lime/",
    "gradientlimedark": "content/ejthemes/gradient-lime-dark/",
    "gradientsaffron": "content/ejthemes/gradient-saffron/",
    "gradientsaffrondark": "content/ejthemes/gradient-saffron-dark/",
    "bootstrap": "content/ejthemes/bootstrap-theme/",
    "high-contrast-01": "content/ejthemes/high-contrast-01/",
    "high-contrast-02": "content/ejthemes/high-contrast-02/",
    "material": "content/ejthemes/material/",
    "office-365": "content/ejthemes/office-365/"
};
function updateLinkTag() {
    var links = $(document.head || document.getElementsByTagName('head')[0]).find("link");
    for (var i = 0; i < links.length; i++) {
        if (links[i].href.indexOf("ej.web.all.min.css") != -1){
            links[i].href = themes[theme] + "ej.web.all.min.css";
		} else if(links[i].href.indexOf("ej.reportdesigner.min.css") != -1) {
            links[i].href = themes[theme] + "ej.reportdesigner.min.css";	
		}
    }
    theme == "material" || theme == "office-365" ? document.styleSheets[1].disabled = true : document.styleSheets[1].disabled = false;
}
function replaceTheme() {
    if ((window.theme.indexOf("lime") != -1) && window.themecolor != 'azure' && window.themecolor != "saffron")
        window.themecolor = 'lime';
    else if ((window.theme.indexOf('saffron') != -1) && window.themecolor != 'azure' && window.themecolor != 'lime')
        window.themecolor = 'saffron';
    else if ((window.theme.indexOf('high-contrast-01') != -1))
        window.themecolor = 'high-contrast-01';
    else if ((window.theme.indexOf('high-contrast-02') != -1))
        window.themecolor = 'high-contrast-02';
    else if ((window.theme.indexOf('material') != -1))
        window.themecolor = 'material';
    else if ((window.theme.indexOf('office-365') != -1))
        window.themecolor = 'office-365';
    else if (window.theme.indexOf('azure') != -1)
        window.themecolor = "azure";
    else if (window.theme.indexOf('bootstrap') != -1)
        window.themecolor = "bootstrap";
    var selcolor = (window.themecolor == "") ? "bootstrap" : window.themecolor;
    $("body").attr("class", selcolor);
    updateLinkTag();
    if (window.currentSamplepage) {
        var querystring = "";
        if (window.theme != "flat") {
            querystring = "?theme=" + window.theme;
        }
        $("#iframecontent").attr('src', currentSamplepage + querystring);


        $("#newsamplewindow").attr('href', currentSamplepage);
    }
    if (window.location.hash === "")
        window.location.hash = "#!/" + window.theme;
    window.location.hash = window.location.hash.replace(/(#!\/[^\/]+)/, "!/" + window.theme);
}
function replacebg(enable) {
    if (enable)
        $("body").addClass("darktheme");
    else
        $("body").removeClass("darktheme");
}
function onSelectSearchItem(args) {
    var samples = (args.key).split("/");
    queryChange(undefined, args);
    $(".samples").hide();
    self.loadLeftTab(samples[0] + "__sb", samples[1]);
    this.element.val("");
}
function themebtnClick(args) {
    if (!isPopupOpened) bindUnbindDocClickEvents(true);
    isPopupOpened = $(args.element).hasClass("e-haschild");
    if (isPopupOpened) samplelisthide();
    if (args.event) args.event.stopPropagation();
    var color;
    if (args.ID == 1) {
        var $btnelement = $(window).width() <= 979 ? $("#res_themebutton") : $("#themebutton");
        var pos = $btnelement.offset();
        var left = $btnelement.width() - $(".e-custom-theme .e-horizontal .e-list>ul").width();
        $(".e-custom-theme .e-horizontal .e-list>ul").css('left', left);
    } else {
        this.element.find('li.e-active').removeClass('e-active');
        var _themename = (args.text).toLowerCase();
        if (_themename.indexOf("gradient") !== -1) window.themestyle = "gradient";
        else window.themestyle = "flat";
        if (_themename.indexOf("dark") !== -1) window.themevarient = "dark";
        else window.themevarient = "light";
        if (_themename.indexOf("azure") !== -1) window.themecolor = window.theme = "azure";
        else if (_themename.indexOf("lime") !== -1) window.themecolor = window.theme = "lime";
        else if (_themename.indexOf("saffron") !== -1) window.themecolor = window.theme = "saffron";
        else if (_themename.indexOf("office-365") !== -1) window.themecolor = window.theme = "office-365";
        else if (_themename.indexOf("material") !== -1) window.themecolor = window.theme = "material";
        else if (_themename.indexOf("high-contrast-02") !== -1) window.themecolor = window.theme = "high-contrast-02";
        else if (_themename.indexOf("high-contrast-01") !== -1) window.themecolor = window.theme = "high-contrast-01";
        else if (_themename.indexOf("bootstrap") !== -1) window.themecolor = window.theme = "bootstrap";
        color = window.themecolor;
        updateTheme(color);
        if (color === "material" || color === "office-365")
            location.reload();
        else {
            var urlread = window.location.toString().toLowerCase();
            if (urlread.indexOf("pivotgrid") > -1 && typeof loadPivotGridFrameTheme != 'undefined')
                loadPivotGridFrameTheme();
        }
        $(args.element).addClass("e-active");

    }
}
function samplelisthide() {
    if ($(window).width() < 981) {
        if (window.isTransitionSupported) $('.accordion-panel').css({ left: '-250px' });
        else $(".accordion-panel").animate({ "left": "-250px" }, 350);
        $('.control-panel.cols-content-fluid').addClass('center-flow');
    }
    else {
        if (window.isTransitionSupported) $('.accordion-panel').css({ left: '0px' });
        else $(".accordion-panel").animate({ "left": "0px" }, 350);
    }
}
function showTooltipbox() {
    var $btnelement = $(window).width() <= 979 ? $("#res_themebutton") : $("#themebutton");
    var pos = $btnelement.offset();
    var left = pos.left - $("#sbtooltipbox").width() + 50;
    if (left > $(".samplecontainerparent").width())
        left = (pos.left + ($btnelement.width() / 2)) - $("#sbtooltipbox_wrapper").width();
    $("#sbtooltipbox").ejDialog("option", "position", { X: left + ($(window).width() <= 979 ? 0 : 10), Y: pos.top + $btnelement.height() + 7 });
    $("#sbtooltipbox").ejDialog("open");
    setTimeout(function () {
        $("#sbtooltipbox").ejDialog("close");
    }, 3000);
}

function reCreateReportViewer() {
    if (window.theme == "material" || window.theme == "office-365") {
        var urlread = window.location.toString().toLowerCase();
        var timeSpan = urlread.indexOf("productlist") > 0 || urlread.indexOf("defaultfunctionalities") > 0 ? 3000 : 1000;
        setTimeout(function () {
            var viewerContainer = $('#container');
            if (viewerContainer.length > 0) {
                var ejReportViewer = viewerContainer.data('ejReportViewer');
                ejReportViewer._destroy();
                ejReportViewer._init();
            }
        }, timeSpan);
    }
}

function themeButtonSelect() {
    if ((window.theme.indexOf("lime") != -1) || (window.location.hash.indexOf('lime') != -1)) window.themecolor = 'lime';
    else if ((window.theme.indexOf("saffron") != -1) || (window.location.hash.indexOf('saffron') != -1)) window.themecolor = 'saffron';
    else if ((window.theme.indexOf("azure") != -1) || (window.location.hash.indexOf('azure') != -1)) window.themecolor = 'azure';
    else if ((window.theme.indexOf("high-contrast-01") != -1) || (window.location.hash.indexOf('high-contrast-01') != -1)) window.themecolor = 'high-contrast-01';
    else if ((window.theme.indexOf("high-contrast-02") != -1) || (window.location.hash.indexOf('high-contrast-02') != -1)) window.themecolor = 'high-contrast-02';
    else if ((window.theme.indexOf("material") != -1) || (window.location.hash.indexOf('material') != -1)) window.themecolor = 'material';
    else if ((window.theme.indexOf("office-365") != -1) || (window.location.hash.indexOf('office-365') != -1)) window.themecolor = 'office-365';
    else window.themecolor = 'bootstrap';
    if ((window.location.hash.indexOf("gradient") !== -1)) window.themestyle = "gradient";
    else window.themestyle = "flat";
    if ((window.location.hash.indexOf("dark") !== -1)) window.themevarient = "dark";
    else window.themevarient = "light";
    theme = (themestyle == "flat" ? "" : themestyle) + themecolor + (themevarient == "light" ? "" : themevarient);
    updateLinkTag();
}
function queryChange(hashBang, args) {
    var themestyle = (window.themestyle === "flat") ? "" : window.themestyle;
    var themevarient = (window.themevarient === "light" || window.theme.indexOf("contrast") > -1) ? "" : window.themevarient;
    if (!ej.isNullOrUndefined(hashBang))
        window.location.hash = hashBang.replace("flat", themestyle + window.theme + themevarient);
    else
        window.location.hash = "#!/" + themestyle + window.theme + themevarient + "/" + args.key;
}
function adjustpopupposition(args) {
    var offset = $("#selectControls_dropdown").offset();
    var height = $("#selectControls_wrapper").height();
    $("#selectControls_popup").css("top", (offset.top + height + 14) + "px");
    var left = $("#selectControls_popup").width() + offset.left;
    if (left > $(".content-container-fluid").width())
        left = (offset.left + $("#selectControls_dropdown").width()) - $("#selectControls_popup").width() - 12;
    $("#selectControls_popup").css("left", left + "px");
}
function viewdemo(product) {
    product = product.toLowerCase();
    if (window.jsonline) { if (product == "desktop") return; else window.location.href = "//js.syncfusion.com/demos/mobile/"; }
    else
    {
        if (location.protocol == "file:") {
            if (product == "desktop") return;
            else window.open("../../ionicapp/www/index.html", "_blank");
        }
        else if (location.toString().indexOf("sfjavascriptsamplebrowser") != -1) {
            if (product == "desktop") return;
            else window.open("//localhost:" + window.location.port + "/sfionicjssamplebrowser", "_blank");
        }
        else {
            var links = $(document.head || document.getElementsByTagName('head')[0]).find("link");
            var serverconfig = links[0].href.substr(0, links[0].href.indexOf("content") - 1);

            var text = 'StartDevServer.ashx', prot = window.location.protocol, hostname = window.location.host;
            if (product == "desktop") return; else product = "mobile";
            var path = "ionic app";
            $.ajax({
                async: false,
                cache: true,
                url: serverconfig + "/" + text + "?product=ionic" + "&path=" + path,
                success: function (data) {
                    if (product == "mobile") window.open(data + "/index.html?platform=js", "_blank");
                }
            });
        }
    }
}


function initiateCopyHandler() {
    try {
        var client = new ZeroClipboard($('#copyclipboard'));
        var time;
        client.on('ready', function (event) {
            client.on('copy', function (event) {
                event.clipboardData.setData('text/plain', copycontent());
                $("#copy-alert").removeClass("hideEl elementToFadeIn elementToFadeOut");
                $("#copy-alert").addClass("elementToFadeIn")
                setTimeout(function () {
                    $("#copy-alert").addClass("elementToFadeOut hideEl");
                }, 1000)
            });
        });
        client.on('error', function (event) {
            console.log('ZeroClipboard error of type "' + event.name + '": ' + event.message);
            ZeroClipboard.destroy();
        });

        function copycontent(e) {
            var headCont, content = "\n", scriptCont, childCont = "\n", i, j;
            if ($("#copytextarea").val() == "" || $("#copytextarea").val() == null) {
                $("#copytextarea").val("");
                $("#sourceTab >div:visible").each(function () {
                    contentLen = $(this).text();
                    headCont = contentLen.slice(2, contentLen.length).replace("Copy", "").split(">");
                    for (i = 0; i < headCont.length; i++) {
                        if (headCont[i].indexOf("$(function") > -1) {
                            scriptCont = headCont[i].replace(/\s+/g, ' ');
                            for (j = 0; j < scriptCont.length; j++) {
                                (scriptCont[j] === "}" && scriptCont[j + 1] != '"') && (content += "\n");
                                if ((scriptCont[j] === "{" && scriptCont[j - 1] != '"') || (scriptCont[j] === "}" && scriptCont[j + 1] != '"' && scriptCont[j + 1] != ",") || scriptCont[j] === "," || scriptCont[j] === ";") {
                                    content += scriptCont[j] + "\n";
                                    (scriptCont[j] === "{" || scriptCont[j] === ",") && (content += "\t");
                                }
                                else
                                    content += scriptCont[j];
                                (j === scriptCont.length - 1) && (content += ">\n");
                            }
                        }
                        else
                            if (i != headCont.length - 1)
                                content += headCont[i] + ">\n";
                    }
                    $("#copytextarea").val($("#copytextarea").val() + content);
                });
            }
            return $("#copytextarea").val();
        }

    }
    catch (e) { }
}

function codeMirrorModes(url) {
    var extn = url.substr(url.lastIndexOf(".") + 1, url.length).toLowerCase();
    switch (extn) {
        case "html":
        case "xml": return "text/html"; break;
        case "css":
        case "less": return "text/css"; break;
        case "js":
        case "ts": return "javascript"; break;
        case "ashx":
        case "cs": return "text/x-csharp"; break;
        default: return "text/html";

    }
}
$(document).on('dragover', function (event) {
    event.preventDefault();
    event.dataTransfer.dropEffect = 'none';
    return false;
}, false);
$(document).on('drop', function (event) {
    event.dataTransfer.dropEffect = 'none';
    event.preventDefault();
    return false;
}, false);

window.menuData = [{ id: 1, text: "", parentId: null, spriteCssClass: "e-icon" },
                    { id: "flatazure", text: "Flat-Azure", parentId: 1, spriteCssClass: "SB-theme SB-flat-azure" },
                    { id: "flatazuredark", text: "Flat-Azure-Dark", parentId: 1, spriteCssClass: "SB-theme SB-flat-azure-dark" },
                    { id: "flatlime", text: "Flat-Lime", parentId: 1, spriteCssClass: "SB-theme SB-flat-lime" },
                    { id: "flatlimedark", text: "Flat-Lime-Dark", parentId: 1, spriteCssClass: "SB-theme SB-flat-lime-dark" },
                    { id: "flatsaffron", text: "Flat-Saffron", parentId: 1, spriteCssClass: "SB-theme SB-flat-saffron" },
                    { id: "flatsaffrondark", text: "Flat-Saffron-Dark", parentId: 1, spriteCssClass: "SB-theme SB-flat-saffron-dark" },
                    { id: "gradientazure", text: "Gradient-Azure", parentId: 1, spriteCssClass: "SB-theme SB-gradient-azure" },
                    { id: "gradientazuredark", text: "Gradient-Azure-Dark", parentId: 1, spriteCssClass: "SB-theme SB-gradient-azure-dark" },
                    { id: "gradientlime", text: "Gradient-Lime", parentId: 1, spriteCssClass: "SB-theme SB-gradient-lime" },
                    { id: "gradientlimedark", text: "Gradient-Lime-Dark", parentId: 1, spriteCssClass: "SB-theme SB-gradient-lime-dark" },
                    { id: "gradientsaffron", text: "Gradient-Saffron", parentId: 1, spriteCssClass: "SB-theme SB-gradient-saffron" },
                    { id: "gradientsaffrondark", text: "Gradient-Saffron-Dark", parentId: 1, spriteCssClass: "SB-theme SB-gradient-saffron-dark" },
                    { id: "flatbootstrap", text: "Bootstrap", parentId: 1, spriteCssClass: "SB-theme SB-bootstrap" },
					{ id: "highcontrast01", text: "High-Contrast-01", parentId: 1, spriteCssClass: "SB-theme SB-high-contrast-01" },
					{ id: "highcontrast02", text: "High-Contrast-02", parentId: 1, spriteCssClass: "SB-theme SB-high-contrast-02" },
					{ id: "material", text: "Material", parentId: 1, spriteCssClass: "SB-theme SB-material" },
					{ id: "office365", text: "Office-365", parentId: 1, spriteCssClass: "SB-theme SB-office365" }

];