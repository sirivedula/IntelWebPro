// Return JQuery id for the supplied client control id
function GetJQId(myid) {
    return '#' + myid.replace(/(:|\.)/g, '\\$1');
};

// Addning two input values and return total value for the supplied parameters
function Add(input1, input2) {
    var total = 0.0;
    var val1 = parseFloat(input1);
    var val2 = parseFloat(input2);

    if (!isNaN(val1)) {
        total += val1;
    }

    if (!isNaN(val2)) {
        total += val2;
    }

    return total.toFixed(2);
};

function AddArrayValues(arrayValues, hiddenControl) {
    var total = 0.0;
    
    jQuery.each(arrayValues, function (index, inputValue) {
        //alert("key is " + index + ", value is " + inputValue);
        if (!isNaN(inputValue)) {
            if (inputValue != "") {
                total += parseFloat(inputValue);
            }
        }
    });

    //alert("Toal is " + total);
    if (total != 0) {
        $(hiddenControl).val(total);//--assign value to hidden field
        return total;
    }
    else {
        return "";
    }
    //return total.toFixed(2); -- Number format (eg 8.00)
};


//--Display 'Invalid!' error message if entered value is NonNumeric
function validateTextBox(ControlId, InputValue) {
    if (isNaN(InputValue)) {
        $(ControlId).val("Invalid!");
        $(ControlId).addClass("validateInputTextBox");
    }
    else {
        $(ControlId).removeClass("validateInputTextBox");
    }

}

//--Compare the supplied two values and return 'true' if vlaues are same else 'false' and add the erroe message for the supplied 'errorConrolId'
function IsMatching(inputVal1, inputVal2, errorControlId1, errorControlId2, errorMessage) {

    errorMessage = "(Not matching Total Enrollment!)"
    inputVal1 = jQuery.trim(inputVal1);
    inputVal2 = jQuery.trim(inputVal2);

    if (inputVal1 != inputVal2) {
        $(errorControlId1).html($(errorControlId1).html() + "<span class='errorMessageStyle'>" + errorMessage + "</span>");
        $(errorControlId2).html($(errorControlId2).html() + "<span class='errorMessageStyle'>" + errorMessage + "</span>");
        return false;
    }
    else {
        var str = $(errorControlId1).html();
        var charPosition = str.indexOf('(');
        str = str.substring(0, charPosition)
        if (charPosition != -1) {
            $(errorControlId1).html(str);
            $(errorControlId2).html(str);
        }
        return true;
    }
}