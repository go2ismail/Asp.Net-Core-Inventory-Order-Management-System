function diagramFitToPage(id, preventScaling) {
    if (id && window) {
        if (ej.isMobile() && ej.isDevice()) {
            var diagram = $("#" + id).ejDiagram("instance");
            diagram.fitToPage("width", "content");
            if (!preventScaling) {
                var viewPort = ej.datavisualization.Diagram.ScrollUtil._viewPort(diagram, true);
                var bounds = diagram._getDigramBounds("content");
                var scale = { x: viewPort.width / bounds.width, y: viewPort.height / bounds.height };
                $("#" + id).ejDiagram({ height: $("#" + id).height() * Math.min(scale.x, scale.y) });
                if (window.location.hostname) {
                    var iframe = top.document.getElementById('samplefile');
                    if (iframe) iframe.style.minHeight = $("#" + id).height() + "px";
                }
            }
        }
    }
}