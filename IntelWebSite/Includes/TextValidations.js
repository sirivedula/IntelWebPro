function validateUSPhone( strValue ) {

  var objRegExp  = /^\([1-9]\d{2}\)\s?\d{3}\-\d{4}$/;

  //check for valid us phone with or without space between
  //area code
  return objRegExp.test(strValue);
}

function  validateNumeric( strValue ) {

  var objRegExp  =  /(^-?\d\d*\.\d*$)|(^-?\d\d*$)|(^-?\.\d\d*$)/;

  //check for numeric characters
  return objRegExp.test(strValue);
}

function validateInteger( strValue ) {

  var objRegExp  = /(^-?\d\d*$)/;

  //check for integer characters
  return objRegExp.test(strValue);
}

function validateNotEmpty( strValue ) {

   var strTemp = strValue;
   strTemp = trimAll(strTemp);
   if(strTemp.length > 0){
     return true;
   }
   return false;
}

function validateUSZip( strValue ) {

var objRegExp  = /(^\d{5}$)|(^\d{5}-\d{4}$)/;

  //check for valid US Zipcode
  return objRegExp.test(strValue);
}

function validateUSDate( strValue ) {

  var objRegExp = /^\d{1,2}(\-|\/|\.)\d{1,2}\1\d{4}$/

  //check to see if in correct format
  if(!objRegExp.test(strValue))
    return false; //doesn't match pattern, bad date
  else{
    var strSeparator = strValue.substring(2,3) //find date separator
    //separator is an integer; get next available character i.e 1/1/2008
    if (validateInteger(strSeparator)) {strSeparator = strValue.substring(3,4)} 
    //separator is an integer; get next available character i.e 1/11/2008
    if (validateInteger(strSeparator)) {strSeparator = strValue.substring(4,5)} 
    var arrayDate = strValue.split(strSeparator); //split date into month, day, year
    //create a lookup for months not equal to Feb.
    var arrayLookup = { '01' : 31, '03' : 31, '04' : 30, '05' : 31, '06' : 30, '07' : 31,
                        '08' : 31, '09' : 30,'10' : 31,'11' : 30,'12' : 31, '1' : 31, 
                        '3' : 31, '4' : 30, '5' : 31, '6' : 30, '7' : 31, '8' : 31, '9' : 30}
    var intDay = parseInt(arrayDate[1]);

    //check if month value and day value agree
    if(arrayLookup[arrayDate[0]] != null) {
      if(intDay <= arrayLookup[arrayDate[0]] && intDay != 0)
        return true; //found in lookup table, good date
    }

    //check for February (bugfix 20050322)
    var intMonth = parseInt(arrayDate[0]);
    if (intMonth == 2) { 
       var intYear = parseInt(arrayDate[2]);
       if( ((intYear % 4 == 0 && intDay <= 29) || (intYear % 4 != 0 && intDay <=28)) && intDay !=0)
          return true; //Feb. had valid number of days
       }
  }
  return false; //any other values, bad date
}

function validateValue( strValue, strMatchPattern ) {

var objRegExp = new RegExp( strMatchPattern);

 //check if string matches pattern
 return objRegExp.test(strValue);
}