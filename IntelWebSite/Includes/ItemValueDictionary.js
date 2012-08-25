//Load Current values for all
//the text boxes, radio buttons, checkboxes, select buttons,
//adding another text box will be automatically
//picked up by the code as the Jquery selectors are 
//used for each control type. 


function LoadCurrentValues(dictionary) {
    $("input:text").each(function () {
        AddElementToDictionary(dictionary, this.id, this.value);
    });
    //alert(this.value )});
    $("input:radio").each(function () {
        AddElementToDictionary(dictionary, this.id, this.checked);
    });
    $("input:checkbox").each(function () {
        AddElementToDictionary(dictionary, this.id, this.checked);

    });
    $("select").each(function () {
        AddElementToDictionary(dictionary, this.id, $("#" + this.id).val());
    }
    )
}


// to compare the initial form values and current form
//values and returns true if they are same or false if they are different.

function CompareDictionaries(InitialDictionary) {
    CurrentDictionary = new Array();

    //At any point while the application is runing
    //it will load the current values, for each control in the form. 
    LoadCurrentValues();
    // Initially check for the number of controls in the
    // Initial Dictionary and in the final dictionary.
    // If the number of controls don't match then
    // form is changed in such a way that some of the controls
    // are hidden or some of the controls loaded as part of the page actions.
    // In these cases, Form save dailogue should trigger

    if (InitialDictionary.length != CurrentDictionary.length) {
        return false;
    }
    //number of controls are same, next check if there
    //is any change in the values of each controls
    else {

        for (i = 0; i < InitialDictionary.length; i++) {
            if ((InitialDictionary[i].name == CurrentDictionary[i].name) &&
          (InitialDictionary[i].value != CurrentDictionary[i].value)) {
                return false;
            }
        }
        return true;
    }
}

/*--------------------CLIENT SIDE CODE-------------------
In the above code, LoadCurrentValues(dictionary) loads the snapshot of each control when the method is called, and the dictionary will have the loaded values. After the page DOM is rendered, the LoadCurrentValues(dictionary) method will be called to load the initial values of the controls. It will prepare a dictionary of IDs and values, with the initial values.

Similarly, LoadCurrentValues(InitialDictionary) will load the current values when the user leaves the form page, and prepares a dictionary of IDs and values, with the current values. The CompareDictionaries() method compares the initial dictionary and the current dictionary. If they are different, then it will return false, else it returns true.

The code below is the place where the LoadInitialValues() method will be called. This is the master page for both of the pages.
*/

$().ready(function () {
    //To capture form initial values
    InitialDictionary = new Array();
    LoadCurrentValues(InitialDictionary);
    $("a[id*=hl]").click(function () {
        if (!CompareDictionaries(InitialDictionary)) {
            return confirm('Form is modified, Do you want to continue');
        }
    }
        )
}
)