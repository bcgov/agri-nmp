//Tool tip customziation
var toolTipClickableInnerHtml = '<div class="tooltip" role="tooltip"><div class="title">Information<button type="button" class="close" id="tooltipCloseButton">×</button></div><div class="tooltip-arrow"></div><div class="tooltip-inner"></div></div>';

//Enables the Close button in the above to close the passed parent of the tooltip
function triggerToolTip(element) {
    var $toolTipElement = $(element);
    $toolTipElement.tooltip('show');
    var toolTipId = $toolTipElement.attr('aria-describedby');
    var $toolTip = $('#' + toolTipId).find('#tooltipCloseButton');
    $toolTip.click(function () {
        $toolTipElement.tooltip('hide');
    });

    //hide it when clicking anywhere else except the popup and the trigger
    $(document).on('click touch', function (event) {
        if (!$(event.target).parents().addBack().is($toolTipElement) && !$(event.target).parents().addBack().is($toolTip)) {
            $toolTipElement.tooltip('hide');
        }
    });

    // Stop propagation to prevent hiding "#tooltip" when clicking on it
    $toolTip.on('click touch', function (event) {
        event.stopPropagation();
    });
}

function RefreshNavigation(controller, currentAction) {
    $.ajax({
        type: "POST",
        url: "/" + controller + "/CallRefreshOfNavigation?currentAction=" + currentAction,
        data: param = "",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            $(result.target).load(result.url); //  Load data from the server and place the returned HTML into the matched element
        },
        error: ""
    });
}

//Used to refresh Navigation appearance in regards to Grayed Out font
function RefreshNextPreviousNavigation(controller, currentAction) {
    $.ajax({
        type: "POST",
        url: "/" + controller + "/CallRefreshOfNextPreviousNavigation?currentAction=" + currentAction,
        data: param = "",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            $(result.target).load(result.url); //  Load data from the server and place the returned HTML into the matched element
        },
        error: ""
    });
}