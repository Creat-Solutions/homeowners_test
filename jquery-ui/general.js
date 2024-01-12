function cambiaTxt(selected_help) {
    var i = 0;
    var objarray = new Array(0);

    // First lets loop through all the help divs and store them in an array
    while (document.getElementById("help" + i)) {
        objarray.push(document.getElementById("help" + i));
        i++;
    }
    //Now loop through our array of helps and turn them all off, except for the one we want on.
    for (var y = 0; y < objarray.length; y++) {
        if (objarray[y].id == selected_help) {
            objarray[y].style.display = "block";
        } else {
            objarray[y].style.display = "none";
        }
    }
}

var isNav;

function checkKey1(e) {
    var keyChar;

    if (isNav == 0) {
        if (e.which < 48 || e.which > 57) e.RETURNVALUE = false;
    } else {
        if ((e.keyCode < 48 || e.keyCode > 57) && e.keyCode != 46 && e.keyCode != 45) e.returnValue = false;
    }
}


function ValidChar(e) {
    var RegX = "[0-9a-zA-Z\-/,\s]+";
    var regex = new RegExp(RegX);

    alert(e.value);

    if (!regex.test(e.value)) {
        alert('Please enter a valid data.');
        return false;
    }
}

function IsNumeric(sText, obj) {
    var ValidChars = "0123456789";
    var IsNumber = true;
    var Char;
    var sVAL

    Char = sText.charAt(0);

    if (Char == "." && obj.value.indexOf('.') > -1)
        IsNumber = false;
    else {
        if (ValidChars.indexOf(Char) == -1) {
            IsNumber = false;
        }
    }
    return IsNumber;
}

function cargar() {
    parent.top.Cierra();
}
