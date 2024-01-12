/// <reference name="MicrosoftAjax.js" />

function ChangeDeleteBoxesState(condition)
{
    if (deleteBoxes == null)
        return;
    for (var i = 0; i < deleteBoxes.length; i++) {
        var checkbox = document.getElementById(deleteBoxes[i]);
        if (checkbox != null)
            checkbox.checked = condition;
    }
}

function CheckHeaderForChanges()
{
    if (deleteBoxes == null)
        return;
    for (var i = 1; i < deleteBoxes.length; i++) {
        var checkbox = document.getElementById(deleteBoxes[i]);
        if (!(checkbox != null && checkbox.checked)) {
            document.getElementById(deleteBoxes[0]).checked = false;
            return;
        }
    }
    document.getElementById(deleteBoxes[0]).checked = true;
}

function FormatMoney(obj)
{
    if (!obj)
        return;
    var m = Number.parseInvariant(obj.value);
    if (isNaN(m) || m == 0)
        return;
    obj.value = m.format("N");
}

function DisplayLoading() {
    DisplayFullScreen($('<img src="/images/loading.gif" alt="Loading..." />'));
}

function DisplayFullScreen(codeBlock) {
    $('#blackScreen').show();
    $('#divScreenData').show();

    if (codeBlock) {
        $('#divScreenData').html('');
    }

    if (codeBlock) {
        var jqueryObj = null;
        if (codeBlock instanceof jQuery) {
            jqueryObj = codeBlock;
        } else {
            jqueryObj = $(codeBlock);
        }
        $('#divScreenData').append(jqueryObj);

        if ($('img', $('#divScreenData')).length !== 0) {
            $('#divScreenData').css('visibility', 'hidden');
            $('img', $('#divScreenData')).bind('imgpreload', function() {
                $('#divScreenData').css('visibility', 'visible');
                var leftOff = ($(window).width() - $('#divScreenData').outerWidth(true)) / 2;
                if (leftOff < 0) {
                    leftOff = 0;
                }
                var topOff = ($(window).height() - $('#divScreenData').outerHeight(true)) / 2;
                if (topOff < 0) {
                    topOff = 0;
                }
                $('#divScreenData').css('top', topOff + 'px');
                $('#divScreenData').css('left', leftOff + 'px');
            });
        } else {
            var leftOff = ($(window).width() - $('#divScreenData').outerWidth(true)) / 2;
            if (leftOff < 0) {
                leftOff = 0;
            }
            var topOff = ($(window).height() - $('#divScreenData').outerHeight(true)) / 2;
            if (topOff < 0) {
                topOff = 0;
            }
            $('#divScreenData').css('top', topOff + 'px');
            $('#divScreenData').css('left', leftOff + 'px');
        }
    }
}

$(window).resize(function() {
    if ($('#divScreenData').css('display') !== 'none') {
        if ($('img', $('#divScreenData')).length !== 0) {
            $('#divScreenData').css('visibility', 'hidden');
            $('img', $('#divScreenData')).bind('imgpreload', function() {
                $('#divScreenData').css('visibility', 'visible');
                var leftOff = ($(window).width() - $('#divScreenData').outerWidth(true)) / 2;
                if (leftOff < 0) {
                    leftOff = 0;
                }
                var topOff = ($(window).height() - $('#divScreenData').outerHeight(true)) / 2;
                if (topOff < 0) {
                    topOff = 0;
                }
                $('#divScreenData').css('top', topOff + 'px');
                $('#divScreenData').css('left', leftOff + 'px');
            });
        } else {
            var leftOff = ($(window).width() - $('#divScreenData').outerWidth(true)) / 2;
            if (leftOff < 0) {
                leftOff = 0;
            }
            var topOff = ($(window).height() - $('#divScreenData').outerHeight(true)) / 2;
            if (topOff < 0) {
                topOff = 0;
            }
            $('#divScreenData').css('top', topOff + 'px');
            $('#divScreenData').css('left', leftOff + 'px');
        }
    }
});

function HideFullScreen() {
    $('#blackScreen').hide();
    $('#divScreenData').hide();
}