//Modal Window control

$(function () {
    var placeholderElement = $('#modal-placeholder');

    $('button[data-toggle="razor-page-modal"]').click(function (event) {
        //Hide Loading message
        $("#divSize").removeClass("modal-carousel");
        //Set the size of the Modal
        $("#divSize").addClass("modal-large");
        //retrieve the url from button
        var url = $(this).data('url');
        //the ajax GET request receives the modal body into memory into 'data'
        $.get(url).done(function (data) {
            //load it with modal and then we’re going to display it
            placeholderElement.html(data);
            //Show the modal
            placeholderElement.find('.modal').modal('show');
        });
    });

    // Attach click event handler to an element
    // which is located inside #modal-placeholder
    // and has data-save attribute equal to modal
    placeholderElement.on('click', '[data-save="razor-page-modal"]', function (event) {
        event.preventDefault();

        //Get the form loaded into modal
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var dataToSend = form.serialize();

        $.post(actionUrl, dataToSend).done(function (result) {
            //If successfull json is expected { success: bool, url: string relative page url }
            if (result.redirect) {
                //Form has no errors hide modal and redirect to URL passed
                placeholderElement.find('.modal').modal('hide');
                $(location).attr('href', result.redirect)
            }
            else {
                //Validation has failed reload form into modal with errors
                var newBody = $('.modal-body', result);
                placeholderElement.find('.modal-body').replaceWith(newBody);
            }
        });
    });
});

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

$(document).on("click",
    '[legacy_modal]',
    function (e) {
        $.ajaxSetup({ cache: false });
        $type = $(this).data('type'); // this works as of jQuery 1.4.3, otherwise $(this).attr('data-type');
        $('#myModalContent').load($type,
            function () {
                $('#myModal').modal({
                    /*backdrop: 'static',*/
                    //keyboard: true
                },
                    'show');
                bindLegacyForm(this);
            });
        return false;
    });

function bindLegacyForm(dialog) {
    $('form', dialog).submit(function () {
        $.ajax({
            cache: false,
            url: this.action,
            type: this.method,
            data: $(this).serialize()
        })
            .done(function (result) {
                if (result.success) {
                    $('#myModal').modal('hide');
                    //$(result.target).load(result.url); //  Load data from the server and place the returned HTML into the matched element
                    window.location.href = result.url;
                } else {
                    $('#myModalContent').html(result);
                    bindForm2(dialog);
                }
            });

        return false;
    });
}