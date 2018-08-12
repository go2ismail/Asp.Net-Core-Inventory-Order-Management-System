

updateNode = false;
var diagram;

 

function diagramSelectionChange(args) {

    if (args.state == "changed") {

        disableProperties();
        if (args.changeType && args.changeType != "remove") {
            $(".cols-prop-area").css("display", "block");
            if (args.element) {
                if (args.element.type === "bpmn")
                    enableProperties(args.element)
            }
        }
        else {
            $(".cols-prop-area").css("display", "none");
        }
    }
}
function disableProperties() {
     
    var ctrls = ["basic_shape", "BPMNEvents", "BPMNTriggers", "BPMNGateways", "BPMNDataObjects", "BPMNActivity",
    "BPMNLoops", "BPMNTasks", "BPMNSubProcess", "BPMNBoundary", "BPMNFlows",
    "CompensationShape", "TaskCall", "AddHoc", "BPMNCollection"];
    for (var i in ctrls)
        $("#" + ctrls[i]).data("ejDropDownList").disable();

    var ids = ["basic_shape_prop", "BPMNEvents_prop", "BPMNTriggers_prop", "BPMNGateways_prop", "BPMNDataObjects_prop",
   "BPMNActivity_prop", "BPMNLoops_prop", "BPMNTasks_prop", "BPMNSubProcess_prop", "BPMNBoundary_prop",
   "BPMNFlows_prop", "CompensationShape_prop", "TaskCall_prop", "AddHoc_prop", "BPMNCollection_prop"];
    for (var i in ids)
        $("#" + ids[i]).css("display", "none");
}
function matchItem(id, value) {
    var control = $('#' + id).data("ejDropDownList")
    $("#" + id + "_prop").css("display", "");
    var items = control.popupListItems;
    value = value.toLowerCase();
    for (var i = 0; i < items.length; i++) {
        var item1 = items[i].text.toLowerCase()
        if (item1 === value) {
            $('#' + id).data("ejDropDownList").selectItemByText(items[i].text);
        }
    }
}
function enableProperties(node) {

    updateNode = false;
    if (node) {
        shape = node.shape;
        switch (shape) {
            case "gateway":
                $('#BPMNGateways').data("ejDropDownList").enable();
                matchItem("BPMNGateways", node.gateway);
                break;
            case "dataobject":

                $('#BPMNDataObjects').data("ejDropDownList").enable();
                if (node.data && node.data.type) matchItem("BPMNDataObjects", node.data.type);

                $('#BPMNCollection').data("ejDropDownList").enable();
                matchItem("BPMNCollection", node.data.collection ? "Collection" : "None");
                break;
            case "activity":
                $('#BPMNLoops').data("ejDropDownList").enable();
                if (node.activity == "task")
                    matchItem("BPMNLoops", node.task.loop);
                else
                    matchItem("BPMNLoops", node.subProcess.loop);

                $('#CompensationShape').data("ejDropDownList").enable();
                if (node.activity == "task")
                    node.task.compensation ? matchItem("CompensationShape", "Compensation") : matchItem("CompensationShape", "None");
                else
                    node.subProcess.compensation ? matchItem("CompensationShape", "Compensation") : matchItem("CompensationShape", "None");

                $('#BPMNActivity').data("ejDropDownList").enable();
                matchItem("BPMNActivity", node.activity === "Task" ? "task" : "subprocess");

                matchItem("BPMNLoops", node.subProcess.loop);
                if (node.activity == "task") {
                    $('#BPMNTasks').data("ejDropDownList").enable();
                    matchItem("BPMNTasks", node.task.type);

                    $('#TaskCall').data("ejDropDownList").enable();
                    matchItem("TaskCall", node.task.call ? "Call" : "None");
                }
                else {
                    $('#AddHoc').data("ejDropDownList").enable();
                    matchItem("AddHoc", node.subProcess.adhoc ? "Ad-Hoc" : "None");


                    $('#BPMNBoundary').data("ejDropDownList").enable();
                    matchItem("BPMNBoundary", node.subProcess.boundary);

                    $('#BPMNSubProcess').data("ejDropDownList").enable();
                    matchItem("BPMNSubProcess", node.subProcess.type);

                    if (node.subProcess.type == "event") {
                        $('#BPMNEvents').data("ejDropDownList").enable();
                        matchItem("BPMNEvents", node.subProcess.event);
                        $('#BPMNTriggers').data("ejDropDownList").enable();
                        matchItem("BPMNTriggers", node.subProcess.trigger);
                    }
                }
                break;
            case "event":
                $('#BPMNEvents').data("ejDropDownList").enable();
                matchItem("BPMNEvents", node.event);
                $('#BPMNTriggers').data("ejDropDownList").enable();
                matchItem("BPMNTriggers", node.trigger);
                break;
        }
    }
    updateNode = true;
}
function dropDownChanged(args, ad, da) {
    if (updateNode) {
        var diagram = $("#diagram").ejDiagram("instance");
        var menuselect = args.model.cssClass;
        var node = diagram.selectionList[0];
        var options = {}, loop;
        if (node && node.type === "bpmn") {
            options.task = {};
            options.subProcess = {};
            options.data = {};
            switch (menuselect) {
                case "BPMNTriggers":
                    if (node.shape == "activity") options.subProcess.trigger = args.text.toLowerCase();
                    else options.trigger = args.text.toLowerCase();
                    break;
                case "BPMNEvents":
                    var obj;
                    obj = ej.datavisualization.Diagram.BPMNEvents[args.text];
                    if (node.shape == "activity") options.subProcess.event = obj;
                    else options.event = obj;
                    break;
                case "BPMNGateways":
                    options.gateway = ej.datavisualization.Diagram.BPMNGateways[args.text];
                    break;
                case "BPMNDataObjects":
                    if (args.text === "Input") options.data.type = "input";
                    else if (args.text === "Output") options.data.type = "output";
                    else options.data.type = "none";
                    break;
                case "BPMNCollection":
                    options.data.collection = (args.text === "Collection") ? true : false;
                    break;
                case "BPMNLoops":
                    loop = ej.datavisualization.Diagram.BPMNLoops[args.text];
                    if (node.activity == "task") options.task.loop = loop;
                    else options.subProcess.loop = loop;
                    break;
                case "BPMNTasks":
                    options.task.type = ej.datavisualization.Diagram.BPMNTasks[args.text];
                    break;
                case "BPMNSubProcess":
                    options.subProcess.events = [];
                    if (args.text === "Default") options.subProcess.type = args.text.charAt(0).toUpperCase();
                    else if (args.text === "Event") options.subProcess.type = args.text.charAt(0).toUpperCase();
                    else if (args.text === "Transaction") {
                        options.subProcess.type = args.text.charAt(0).toUpperCase();
                        options.subProcess.events = [{ event: "intermediate", trigger: "cancel", offset: { x: 0.25, y: 1 } }, { event: "intermediate", trigger: "error", offset: { x: 0.75, y: 1 } }];
                    }
                    break;
                case "CompensationShape":
                    if (node.activity == "task") options.task.compensation = (args.text === "Compensation") ? true : false;
                    else options.subProcess.compensation = (args.text === "Compensation") ? true : false;
                    break;
                case "AddHoc":
                    options.subProcess.adhoc = (args.text === "Ad-Hoc") ? true : false;
                    break;
                case "TaskCall":
                    options.task.call = (args.text === "Call") ? true : false;
                    break;
                case "BPMNBoundary":
                    options.subProcess.boundary = ej.datavisualization.Diagram.BPMNBoundary[args.text];
                    break;
                case "BPMNActivity":
                    options.activity = (args.text === "Task") ? "task" : "subprocess";
                    break;
                default:
                    return;
            }
            if (node.segments) diagram.updateConnector(node.name, { segments: node.segments, });
            else diagram.updateNode(node.name, options);
        }
    }
}
