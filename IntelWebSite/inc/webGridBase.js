if (!webGrid) {
    var webGrid = function() {
        var imageFolder = 'Images/';
        var grids = [];
        var formName = 'webdataform';
        var debugAjax = false;
        var $currentRow;
        return {
            findFirst: function(data, predicate) {
                var dmax = data.length;
                for (var counter = 0; counter < dmax; counter++) {
                    if (predicate(data[counter])) {
                        return data[counter];
                    }
                }
                return null;
            }
        , findAll: function(data, predicate) {
            var dmax = data.length;
            var results = [];
            for (var counter = 0; counter < dmax; counter++) {
                if (predicate(data[counter])) {
                    results.push(data[counter]);
                }
            }
            return results;
        }
        , formName: formName
        , grids: grids
        , $currentRow: $currentRow
        , debugAjax: debugAjax
        , imageFolder: imageFolder
        , warn: function(message) {
            alert(message);
        }
        , generateTemporaryKey: function() {
            var key = "";
            for (var i = 1; i <= 16; i++) {
                var n = Math.floor(Math.random() * 36.0).toString(36);
                key += n;
            }
            return key
        }
        , removeNewItems: function() {
            var gmax = webGrid.grids.length;
            for (counter = 0; counter < gmax; counter++) {
                var myGrid = webGrid.grids[counter];
                var rmax = myGrid.data.length;
                for (var rcount = rmax - 1; rcount >= 0; rcount--) {
                    var record = myGrid.data[rcount];
                    if (record.isNew) {
                        webGrid.remove(myGrid, record);
                    }
                }
            }
        }
        , removeGrid: function(_grid) {
            var myGrid = webGrid.findGrid(_grid);
            var gmax = webGrid.grids.length;
            for (var counter = gmax - 1; counter >= 0; counter--) {
                if (webGrid.grids[counter] === myGrid) {
                    var rest = webGrid.grids.slice(counter + 1 || webGrid.grids.length);
                    webGrid.grids.length = counter;
                    webGrid.grids.push.apply(webGrid.grids, rest);
                }
            }

        }
        , remove: function(_grid, _record) {
            var myGrid = webGrid.findGrid(_grid);
            if (myGrid == null) {
                webGrid.warn("Data for the grid " + _grid + " could not be located. Unable to remove the record.");
                return false;
            }
            var myRecord = (typeof _record == 'object') ? myRecord = _record : webGrid.findFirst(myGrid.data, function(obj) { return obj._id == _record });
            if (myRecord == null) {
                myRecord = webGrid.findFirst(myGrid.data, function(obj) { return obj._pk == _record }); //attempt to find by pk
                if (myRecord == null) {
                    webGrid.warn("The record " + _record + " could not be found. Unable to remove the row.");
                    return false;
                }
            }
            var rmax = myGrid.data.length;
            if (myGrid.tableId != '') {
                var $tbl = $('#' + myGrid.tableId);
            }
            for (var counter = rmax - 1; counter >= 0; counter--) {
                var drecord = myGrid.data[counter];
                if (drecord === myRecord) {
                    var rowId = myGrid.name + '_row_' + myRecord._id;
                    if (myGrid.tableId != '') { $tbl.find('#' + rowId).remove(); }
                    var rest = myGrid.data.slice(counter + 1 || myGrid.data.length);
                    myGrid.data.length = counter;
                    myGrid.data.push.apply(myGrid.data, rest);
                }
            }
            webGrid.reId(myGrid);
        }
        , findGrid: function(_grid) {
            return (typeof _grid == 'object') ? _grid : webGrid.findFirst(webGrid.grids, function(obj) { return obj.name == _grid; });
        }
        , findRecord: function(_grid, _record) {
            var myGrid = webGrid.findGrid(_grid);
            var myRecord = (typeof _record == 'object') ? _record : webGrid.findFirst(myGrid.data, function(obj) { return obj._id == _record });
            if (myRecord == null) {
                //attempt to find by pk
                myRecord = webGrid.findFirst(myGrid.data, function(obj) { return obj._pk == _record });
            }
            return myRecord;
        }
        , reId: function(_grid) {
            var myGrid = webGrid.findGrid(_grid);
            if (myGrid == null) {
                webGrid.warn("Data for the grid " + _grid + " could not be located. Unable to Re-Id the records.");
                return false;
            }
            var rmax = myGrid.data.length;
            for (var counter = 0; counter < rmax; counter++) {
                var record = myGrid.data[counter];
                if (record._id != counter) {
                    record._id = counter;
                    webGrid.drawRow(myGrid, record);
                }
            }
        }
        , deleteRecord: function(_grid, _record) {
            var myGrid = webGrid.findGrid(_grid);
            if (myGrid == null) {
                webGrid.warn("Data for the grid " + _grid + " could not be located. Unable to update the value of " + fieldName + " to " + value);
                return false;
            }
            var myRecord = webGrid.findRecord(myGrid, _record);
            if (myRecord == null) {
                webGrid.warn("The record " + _record + " could not be found. Unable to delete the record");
                return false;
            }
            myRecord.isDeleted = true;
            myRecord.dirty = true;
            myRecord.grid = myGrid;
            return myRecord;
        }
        , add: function(_grid) { //returns the added record with defaults applied
            var myGrid = webGrid.findGrid(_grid);
            if (myGrid == null) {
                webGrid.warn("Data for the grid " + _grid + " could not be located. Unable to add record.");
                return undefined;
            }
            var record = {};
            record.isNew = true;
            record.dirty = true;
            record._pk = '';
            record._temppk = webGrid.generateTemporaryKey();
            record.fields = [];
            record.grid = myGrid;
            var fmax = myGrid.fieldDefs.length
            for (var fc = 0; fc < fmax; fc++) {
                var field = myGrid.fieldDefs[fc];
                if (!field.isCalculatedField) {
                    var fieldname = field.name;
                    if (field.defaultValue != undefined) {
                        record.fields[fieldname] = field.defaultValue;
                    }
                    else {
                        record.fields[fieldname] = ''
                    }
                }
            }
            record._id = myGrid.data.length;
            if (myGrid.onAdd && typeof myGrid.onAdd == 'function') {
                myGrid.onAdd(record);
            }
            myGrid.data.push(record);
            webGrid.drawRow(myGrid.name, record._id);
            return record;
        }
        , saveUIForm: function(_uiFormName) {
            var problem = false;
            $("#" + _uiFormName).find(":input").not(':button').each(function(index) {
                var $this = $(this);
                var recordInfo = $this.data('recordInfo');
                if (recordInfo) {
                    problem = problem || !webGrid.saveUIRecordField($this, recordInfo);
                }
            })
            return !problem;
        }
        , saveUIField: function(fld) {
            //saves a field that has recordinfo data in it
            var $fld = $(fld);
            var recordInfo = $fld.data('recordInfo');
            if (webGrid.saveUIRecordField($fld, recordInfo)) {
                webGrid.drawRow(recordInfo.grid, recordInfo.record)
            }
        }
        , saveUIRecordField: function($this, recordInfo) {
            var problem = false;
            var fieldVal = $this.val();
            if ($this.is(':checkbox')) {
                fieldVal = $this[0].checked;
            }
            if (!webGrid.change(recordInfo.grid, recordInfo.record, recordInfo.fieldDef.name, fieldVal)) {
                problem = true;
                $this.addClass("errfield");
            }
            else {
                $this.removeClass("errfield");
            }
            return !problem;
        }
        , fillUIForm: function(_grid, _record, onupdateFunction) {
            var myGrid = webGrid.findGrid(_grid);

            if (myGrid == null) {

                webGrid.warn("Data for the grid " + _grid + " could not be located. Unable to fill ui form.");
                return false;
            }
            var myRecord = webGrid.findRecord(myGrid, _record);
            if (myRecord == null) {
                webGrid.warn("The record number " + _record + " could not be found. Unable to draw the row.");
                return false;
            }
            var fieldMax = myGrid.fieldDefs.length;
            myGrid.currentUIRecord = myRecord;
            for (var counter = 0; counter < fieldMax; counter++) {
                var fieldDef = myGrid.fieldDefs[counter];
                if (fieldDef.formFieldName && fieldDef.formFieldName != '') {
                    var displayValue;
                    if (fieldDef.formatter) {
                        displayValue = fieldDef.formatter(myRecord);
                    }
                    else {
                        if (myRecord.fields[fieldDef.name] === null) {
                            displayValue = '';
                        }
                        else {
                            displayValue = myRecord.fields[fieldDef.name];
                        }
                    }
                    var $fld = $('#' + fieldDef.formFieldName)
                    if ($fld.is(':checkbox')) {
                        displayValue = myRecord.fields[fieldDef.name]; //remove formatting for checkboxes
                        if (!displayValue) {
                            $fld.removeAttr('checked');
                        }
                        else {
                            $fld.attr('checked', 'checked');
                        }

                    }
                    else {
                        $fld.val(displayValue);
                    }
                    $fld.data('recordInfo', { 'grid': myGrid, 'record': myRecord, 'fieldDef': fieldDef });

                    if (typeof onupdateFunction == 'function') {
                        $fld.bind(($fld.is(':checkbox') ? 'click' : 'change'), function() {
                            onupdateFunction(this);
                        });
                    }
                }
            }
            if (myGrid.showUIForm) {

                myGrid.showUIForm(myGrid, myRecord);

            }
        }

        , drawFilteredRows: function(_grid, recordPredicate) {
            var myGrid = webGrid.findGrid(_grid);
            if (myGrid == null) {
                webGrid.warn("Data for the grid " + _grid + " could not be located. Unable to draw the filtered rows.");
                return false;
            }
            var rmax = myGrid.data.length;
            for (var counter = 0; counter < rmax; counter++) {
                if (recordPredicate(myGrid.data[counter])) {
                    webGrid.drawRow(myGrid, myGrid.data[counter]);
                }
                else {
                    webGrid.hideRow(myGrid, myGrid.data[counter]);
                }
            }
        }
        , drawAllRows: function(_grid) {
            var myGrid = webGrid.findGrid(_grid);
            if (myGrid == null) {
                webGrid.warn("Data for the grid " + _grid + " could not be located. Unable to draw all rows.");
                return false;
            }
            var rmax = myGrid.data.length;
            for (var counter = 0; counter < rmax; counter++) {
                webGrid.drawRow(myGrid, myGrid.data[counter]);
            }
        }
        , $table: function(_grid) {
            var myGrid = webGrid.findGrid(_grid);
            if (myGrid == null) {
                return null;
            }
            return $('#' + myGrid.tableId);
        }
        , hideRow: function(_grid, _record) {
            var myGrid = webGrid.findGrid(_grid);
            if (myGrid == null) {
                webGrid.warn("Data for the grid " + _grid + " could not be located. Unable to hide row.");
                return false;
            }
            var myRecord = webGrid.findRecord(myGrid, _record);
            if (myRecord == null) {
                webGrid.warn("The record number " + _record + " could not be found. Unable to draw the row.");
                return false;
            }
            var $tbl = $('#' + myGrid.tableId);
            if ($tbl.length == 0) {
                webGrid.warn("The UI table '" + myGrid.tableId + "' could not be found.  Unable to draw the row");
                return false;
            }
            var rowId = myGrid.name + '_row_' + myRecord._id;
            var $row = $tbl.find('#' + rowId).hide();

        }
        , removeAllUIRows: function(_grid) {
            var myGrid = webGrid.findGrid(_grid);
            if (myGrid == null) {
                webGrid.warn("Data for the grid " + _grid + " could not be located. Unable to hide row.");
                return false;
            }
            var $tbl = $('#' + myGrid.tableId);
            if ($tbl.length == 0) {
                webGrid.warn("The UI table '" + myGrid.tableId + "' could not be found.  Unable to draw the row");
                return false;
            }
            var rmax = myGrid.data.length;
            for (var counter = 0; counter < rmax; counter++) {
                var myRecord = myGrid.data[counter];
                var rowId = myGrid.name + '_row_' + myRecord._id;
                var $row = $tbl.find('#' + rowId);
                $row.remove()
            }
        }
        , drawRow: function(_grid, _record, options) { //grid can be a gridName or object, record can be a record _id or record object
            //options => {removeRow:false,tableId:'',columnPredicate:function(columnInfo){return true;},extraColumns([{formatter,name,isHeaderField,atBeginning}])}
            var myGrid = webGrid.findGrid(_grid);
            if (myGrid == null) {
                webGrid.warn("Data for the grid " + _grid + " could not be located. Unable to draw row.");
                return false;
            }
            var myRecord = webGrid.findRecord(myGrid, _record);
            if (myRecord == null) {
                webGrid.warn("The record number " + _record + " could not be found. Unable to draw the row.");
                return false;
            }

            if (typeof options == 'undefined') {
                var options = {};
            }
            var destinationTableId = options.tableId || myGrid.tableId;
            if (destinationTableId == '') {
                return false;
            }
            var $tbl = $('#' + destinationTableId);
            if ($tbl.length == 0) {
                webGrid.warn("The UI table '" + destinationTableId + "' could not be found.  Unable to draw the row");
                return false;
            }
            //if the options specifies an alternate tableId then append it to the rowId to keep unique Ids
            var rowId = myGrid.name + (options.tableId ? options.tableId : '') + '_row_' + myRecord._id;
            var $row = $tbl.find('#' + rowId).show();
            if (options && options.removeRow) {
                $row.remove()
                return null;
            }
            if ($row.length == 0) {
                //add the row
                var $tbody = $tbl.find('tbody').last()
                $tbody.append('<tr id="' + rowId + '"></tr>');
                $row = $tbl.find('#' + rowId);
                if ($tbody.find('tr:visible').length % 2 != 0) {
                    $row.addClass('gridodd');
                }
                if (myRecord.isDeleted) {
                    $row.addClass('deleted');
                }
                else if (myRecord.isNew) {
                    $row.addClass('new');
                }
            }
            var tdCounter = -1;
            var fieldDefMax = myGrid.fieldDefs.length;
            var extraColumnMaker = function(myGrid, myRecord, options, $row, atBeginning, tdCounter) { //returns tdcounter

                var extraColumns = myGrid.extraColumns;
                if (options.extraColumns) {
                    extraColumns = extraColumns.concat(options.extraColumns);
                }
                var extraFieldMax = extraColumns.length;

                for (var counter = 0; counter < extraFieldMax; counter++) {
                    var extra = extraColumns[counter];
                    if ((!options.columnPredicate) || options.columnPredicate(extra)) {
                        var fieldValue = webGrid.replaceGridAndRecordParams(extra.formatter(myRecord), myGrid.name, myRecord._id, myRecord._pk);
                        if (extra.isHeaderField && extra.atBeginning == atBeginning) {
                            tdCounter++;
                            var $td = $row.find('td:eq(' + tdCounter + ')');
                            if ($td.length == 0) {
                                tdCounter++;
                                $('<td />').html(fieldValue).appendTo($row);
                            }
                            else {
                                $td.html(fieldValue);
                            }
                        }
                    }
                }
                return tdCounter;
            }
            tdCounter = extraColumnMaker(myGrid, myRecord, options, $row, true, tdCounter);
            for (var counter = 0; counter < fieldDefMax; counter++) {
                var fieldDef = myGrid.fieldDefs[counter];
                if ((!options.columnPredicate) || options.columnPredicate(fieldDef)) {
                    if (fieldDef.isHeaderField) {
                        var fieldValue;
                        tdCounter++;
                        if (fieldDef.formatter != null) {
                            fieldValue = fieldDef.formatter(myRecord);
                        }
                        else {
                            fieldValue = myRecord.fields[fieldDef.name];
                        }
                        var $td = $row.find('td:eq(' + tdCounter + ')');
                        if (fieldValue == null) {
                            fieldValue = '';
                        }
                        if ($td.length == 0) {
                            $('<td />').text(fieldValue).appendTo($row);
                        }
                        else {
                            $td.text(fieldValue);
                        }

                    }
                }
            }
            tdCounter = extraColumnMaker(myGrid, myRecord, options, $row, false, tdCounter);
            return $row;
        }
        , replaceGridAndRecordParams: function(subject, gridname, recordindex, recordpk) {
            return subject.replace(/\[gridName\]/img, "'" + webGrid.escapeJS(gridname) + "'").replace(/\[recordIndex\]/img, recordindex).replace(/\[recordPK\]/img, "'" + webGrid.escapeJS(recordpk) + "'");
        }
        , mergeGridInfos: function(gridInfos) {
            var giMax = gridInfos.length;
            for (var counter = 0; counter < giMax; counter++) {
                var gridInfo = gridInfos[counter];
                var myGrid = webGrid.findFirst(webGrid.grids, function(obj) { return obj.name == gridInfo.name; })
                if (myGrid == null) {
                    webGrid.warn("Data for the grid " + _grid + " could not be located. Unable to merge the results.");
                    break;
                }
                webGrid.merge(myGrid, gridInfo.records);
                var deleteCount = gridInfo.deletedKeys.length;
                for (var dCount = 0; dCount < deleteCount; dCount++) {
                    //find and remove the deleted item
                    webGrid.remove(myGrid, gridInfo.deletedKeys[dCount]);
                }
            }
        }
        , merge: function(_grid, records) {
            var myGrid = webGrid.findGrid(_grid);
            if (myGrid == null) {
                webGrid.warn("Data for the grid " + _grid + " could not be located. Unable to update the value of " + fieldName + " to " + value);
                return false;
            }
            var rmax = records.length;
            var fmax = myGrid.fieldDefs.length;
            for (var counter = 0; counter < rmax; counter++) {
                record = records[counter];
                record.grid = myGrid;
                var mergeWithRecord = webGrid.findFirst(myGrid.data, function(obj) { return obj._pk == record._pk; });
                if (mergeWithRecord != null) {
                    record._id = mergeWithRecord._id;
                    record.dirty = false;
                    myGrid.data[record._id] = record;
                }
                else {
                    //check to see if this is a new record
                    if (record._temppk) {

                        mergeWithRecord = webGrid.findFirst(myGrid.data, function(obj) { return obj._temppk && obj._temppk == record._temppk; });
                        if (record._pk && record._pk != '') {
                            record._temppk = undefined;
                        }
                    }

                    if (mergeWithRecord != null) {
                        record._id = mergeWithRecord._id;
                        record._isNew = false;
                        record.dirty = false;
                        myGrid.data[record._id] = record;
                    }
                    else {
                        record._id = myGrid.data.length;
                        record.dirty = false;
                        myGrid.data.push(record);
                    }
                }
            }
        }
        , change: function(_grid, _record, fieldName, value) {
            var myGrid = webGrid.findGrid(_grid);
            if (myGrid == null) {
                webGrid.warn("Data for the grid " + _grid + " could not be located. Unable to update the value of " + fieldName + " to " + value);
                return false;
            }
            var myRecord = webGrid.findRecord(myGrid, _record);
            if (myRecord == null) {
                webGrid.warn("The record number " + _record + " could not be found. Unable to update the value of " + fieldName + " to " + value);
                return false;
            }
            var fieldDef = webGrid.findFirst(myGrid.fieldDefs, function(obj) { return obj.name == fieldName; });
            if (fieldDef == null) {
                webGrid.warn("The field definition for '" + fieldName + "' could not be found.  Unable to update the value of " + fieldName + " to " + value);
            }
            var validationError = '';
            var previousValue = myRecord.fields[fieldName];
            myRecord.fields[fieldName] = value; //need to set the field value pre-validation, this gets changed back immediately following
            if (fieldDef.validation != undefined && fieldDef.validation != null) {
                validationError = fieldDef.validation(myRecord.fields);
            }
            if (validationError != null && validationError != '') {
                webGrid.warn("The value '" + value + "' is not a valid value for '" + fieldDef.displayName + "'. " + validationError);
                return false;
            }
            myRecord.fields[fieldName] = previousValue; //change to pre-validation value
            if (!fieldDef.allowBlank && (value == null || value === '')) {
                webGrid.warn("The value for '" + fieldDef.displayName + "' may not be blank. ");
                return false;
            }
            if (value != null && fieldDef.maxLength > 0 && value.length > fieldDef.maxLength) {
                webGrid.warn("The '" + fieldDef.displayName + "' may be a maximum of '" + fieldDef.maxLength + "' characters long, the value '" + value + "' is '" + value.length + "' characters long. ");
                return false;
            }
            //store original value
            if (myRecord.originalValues == undefined) {
                myRecord.originalValues = {};
            }
            if (myRecord.originalValues[fieldName] == undefined) {
                myRecord.originalValues[fieldName] = myRecord.fields[fieldName];
            }
            if (!myRecord.dirty) { myRecord.dirty = false; }
            myRecord.dirty = myRecord.dirty || (myRecord.originalValues[fieldName] != value);
            myRecord.fields[fieldName] = value;

            return true;

        }
        , changedRecords: function(gridPredicate) {
            var candidateGrids = [];
            if (typeof gridPredicate == 'function') {
                candidateGrids = webGrid.findAll(webGrid.grids, gridPredicate);
            }
            else {
                candidateGrids = webGrid.grids;
            }
            var result = [];
            var cmax = candidateGrids.length;
            for (var gridcounter = 0; gridcounter < cmax; gridcounter++) {
                var myGrid = candidateGrids[gridcounter];
                for (var recCounter = 0; recCounter < myGrid.data.length; recCounter++) {
                    var myRecord = myGrid.data[recCounter];
                    if (myRecord.dirty) {
                        myRecord.grid = myGrid;
                        result.push(myRecord);
                    }
                }
            }
            return result;
        }
        , gridsFromPredicate: function(gridPredicate) {

            if (typeof gridPredicate == 'function') {
                return webGrid.findAll(webGrid.grids, gridPredicate);
            }
            else {
                return webGrid.grids;
            }
        }
        , submit: function(gridPredicate, callBackFunction, preAfterSubmitCallBackFunction) {
            var candidateGrids = webGrid.gridsFromPredicate(gridPredicate);
            var submitGridUrls = [];
            var candidateMax = candidateGrids.length;
            for (var counter = 0; counter < candidateMax; counter++) { //produce list of unique URLS to submit to
                var tGrid = candidateGrids[counter];
                if (tGrid.ajaxURL && tGrid.ajaxURL != '' && $.inArray(tGrid.ajaxURL, submitGridUrls) == -1) {
                    submitGridUrls.push(tGrid.ajaxURL);
                }
            }
            var sMax = submitGridUrls.length;
            for (var counter = 0; counter < sMax; counter++) {
                var url = submitGridUrls[counter];
                var submitPredicate = function(myGrid) { return (myGrid.ajaxURL && myGrid.ajaxURL === url); }
                webGrid.prepareSubmission(submitPredicate);
                var gridsToSubmit = webGrid.findAll(candidateGrids, submitPredicate);
                //combine ajax submission values for these grids
                var submitParams = {};
                var gridToSubmitMax = gridsToSubmit.length;
                for (var gcount = 0; gcount < gridToSubmitMax; gcount++) {
                    var curGrid = gridsToSubmit[gcount]
                    $.extend(submitParams, curGrid.ajaxValues);
                    if (curGrid.extraAjaxParams) {
                        $.extend(submitParams, curGrid.extraAjaxParams());
                    }
                }
                if (webGrid.extraAjaxParams) {
                    $.extend(submitParams, webGrid.extraAjaxParams());
                }
                if (webGrid.debugAjax) {
                    var debug = '{';
                    for (var x in submitParams) {
                        debug += x + ":'" + submitParams[x] + "', ";
                    }
                    debug += "}";
                    if ($('#debugArea').length == 0) {
                        $('form:last').after($('<div id="debugArea"></div>'));
                    }
                    $('#debugArea').html($('#debugArea').html() + '<div style="background-color:#EEE6FF;">URL: ' + url + '<br />' + debug + '</div>');
                }
                var saveId = "save_" + new Date().getTime();
                if ($('#' + saveId).length == 0) {
                    $('form:first').after($('<div style="display:none;" id="' + saveId + '"><img src="' + webGrid.imageFolder + 'ajax5.gif" /></div>'));
                }
                var dPosition = "center";
                if (window.parentwindow) {
                    var pwindow = window.parentwindow();
                    if (pwindow) {
                        dPosition = [($(window).width() / 2) - 70, $(pwindow).scrollTop()];
                    }
                }
                if ((typeof webGrid.lockDelay != undefined) && webGrid.lockDelay == 0) {
                    $('#' + saveId).dialog({ modal: true, title: "Saving", position: dPosition })
                }
                else {
                    webGrid[saveId] = setTimeout(function() {
                        $('#' + saveId).dialog({ modal: true, title: "Saving", position: dPosition })
                    }, webGrid.lockDelay || 500); //the save wait dialog is only displayed if the save takes more than 500ms
                }
                $.post(url, submitParams, function(data) {
                    if (webGrid.debugAjax) {
                        $('#debugArea').html($('#debugArea').html() + '<div style="background-color:#E7FEEE;">Response: ' + data + '</div>');
                    }
                    clearTimeout(webGrid[saveId]);
                    setTimeout(function() {
                        try {
                            $('#' + saveId).dialog('destroy').remove();
                        } catch (ex) { }
                    }, 250);
                    var result;
                    try {
                        eval('result = ' + data);
                    }
                    catch (ex) {
                        webGrid.warn("There was an error");
                        var $ediv = $('#gridDisplayError');
                        if ($ediv.length == 0) {
                            $('div:last').append('<div id="gridDisplayError"></div>');
                            $ediv = $('#gridDisplayError');
                        }
                        $ediv.html("<div class=\"err\"><h2>There was an error parsing the save result.</h2>" + webGrid.escapeHTML(data) + "</div>");
                        $ediv.dialog({ title: 'Error', height: 600, width: $('body').innerWidth(), position: [0, 0], modal: false });
                        return false;
                    }
                    if (!result.ok && result.errorMessage) {
                        webGrid.warn(result.errorMessage);
                    }
                    if (result.action && typeof result.action == 'function') {
                        result.action(result);
                    }
                    if (typeof preAfterSubmitCallBackFunction == 'function') {
                        preAfterSubmitCallBackFunction(result);
                    }
                    for (var gcount = 0; gcount < gridToSubmitMax; gcount++) {
                        var pGrid = gridsToSubmit[gcount];
                        if (pGrid) {
                            pGrid.submitting = false;
                            if (pGrid.afterSubmit && typeof pGrid.afterSubmit == 'function') {
                                pGrid.afterSubmit(result, pGrid);
                            }
                        }
                    }
                    if (typeof callBackFunction == 'function') {
                        callBackFunction(result);
                    }
                })
            }
        }
        , setSubmissionValue: function(oGrid, name, subvalue) {
            var value = subvalue;
            if (value && (typeof value.getFullYear == 'function')) {
                // value = value.toLocaleString();
                value = gridFormatters.toUSDateTime(value);
            }
            if (oGrid.ajaxURL && oGrid.ajaxURL != '') {
                if (!oGrid.ajaxValues) {
                    oGrid.ajaxValues = {};
                }

                if (value == null) {
                    oGrid.ajaxValues[name] = '';
                }
                else {
                    oGrid.ajaxValues[name] = value;
                }
            }

            else {
                var $field = $('#' + name);
                if ($field.length == 0) {
                    $('#' + webGrid.formName).append($('<input type="hidden" />').attr('id', name).attr('name', name));
                    $field = $('#' + name);
                }

                if (value == null) {
                    $field.val('');
                }
                else {
                    $field.val(value);
                }
            }
        }
        , prepareSubmission: function(gridPredicate) {
            var candidateGrids = webGrid.gridsFromPredicate(gridPredicate);
            var candidateMax = candidateGrids.length;
            for (var ccounter = 0; ccounter < candidateMax; ccounter++) {
                candidateGrids[ccounter].ajaxValues = {};
            }
            var changed = webGrid.changedRecords(gridPredicate);
            var changedCountsByGrid = {};
            //reset all ajax values
            var gmax = webGrid.grids.length;
            for (var counter = 0; counter < changed.length; counter++) {
                var rec = changed[counter];
                if (changedCountsByGrid[rec.grid.name] == undefined) {
                    changedCountsByGrid[rec.grid.name] = 0;
                }
                var submissionIndex = changedCountsByGrid[rec.grid.name];
                changedCountsByGrid[rec.grid.name] += 1;

                if (rec.isNew) {
                    webGrid.setSubmissionValue(rec.grid, rec.grid.name + "_isNew_" + submissionIndex, true);
                    webGrid.setSubmissionValue(rec.grid, rec.grid.name + "_temppk_" + submissionIndex, rec._temppk);
                }
                else {
                    webGrid.setSubmissionValue(rec.grid, rec.grid.name + "_pk_" + submissionIndex, rec._pk);
                }
                if (rec.isDeleted) {
                    webGrid.setSubmissionValue(rec.grid, rec.grid.name + "_isDeleted_" + submissionIndex, true);
                }
                for (var field in rec.fields) {
                    var submitName = rec.grid.name + "_r" + submissionIndex + "_" + field;
                    var myField = rec.fields[field];
                    var fieldDef = webGrid.findFirst(rec.grid.fieldDefs, function(obj) { return obj.name == field; });
                    if (fieldDef.submitField) {
                        webGrid.setSubmissionValue(rec.grid, submitName, myField);
                    }
                }
            }
            var counter = -1;
            for (var changedGridName in changedCountsByGrid) {
                counter++;
                var myGrid = webGrid.findFirst(webGrid.grids, function(obj) { return obj.name == changedGridName; });
                webGrid.setSubmissionValue(myGrid, "grid" + counter + "_objectId", myGrid.objectId);
                webGrid.setSubmissionValue(myGrid, "grid" + counter + "_objectName", myGrid.name);
                webGrid.setSubmissionValue(myGrid, changedGridName + "_recordCount", changedCountsByGrid[changedGridName]);
            }
        }
        , escapeJS: function(s) {
            if (s == undefined) {
                return '';
            }
            var text = "";
            for (var i = 0, len = s.length; i < len; i++) {
                var ch = s.charAt(i);
                switch (ch) {
                    case '\b':
                        text += "\\b";
                        break;
                    case '\n':
                        text += "\\n";
                        break;
                    case '\t':
                        text += "\\t";
                        break;
                    case '\f':
                        text += "\\f";
                        break;
                    case '\r':
                        text += "\\r";
                        break;
                    case '\"':
                        text += "\\u0022";
                        break;
                    case '\'':
                        text += "\\u0027";
                        break;
                    case '\\':
                        text += "\\\\";
                        break;
                    default:
                        text += ch;
                }
            }
            return text;
        }
        , escapeHTML: function(str) {
            var div = document.createElement('div');
            var text = document.createTextNode(str);
            div.appendChild(text);
            return div.innerHTML;
        }
        , currentRowRecord: function(_grid) {
            var myGrid = webGrid.findGrid(_grid);
            var curRow;
            if (myGrid == null) {
                curRow = webGrid.$currentRow;
            }
            else {
                curRow = myGrid.$currentRow;
            }
            if (!curRow) {
                return null;
            }
            var aTemp = curRow.attr('id').split('_');
            if (aTemp.length > 0) {
                var tempId = aTemp[aTemp.length - 1];
                return webGrid.findFirst(myGrid.data, function(obj) { return obj._id == tempId })
            }
            return null;
        }
        , positionPointers: function($row) {
            if ($row && $row.length && $row.length == 1) {
                var aTemp = $row.attr('id').split('_');
                if (aTemp.length > 0) {
                    var gridName = aTemp[0];
                    var myGrid = webGrid.findGrid(gridName);
                    if (myGrid) {
                        myGrid.$currentRow = $row;
                    }
                }
                webGrid.$currentRow = $row;

                try {
                    var top;
                    var left;
                    var width;
                    top = $row.children('td:visible:first').position()['top'];
                    left = $row.children('td:visible:first').position()['left'] - 16;
                    width = $row.children('td:visible:last').position()['left'] + $row.children('td:visible:last').width() + 3;
                    $('#curpointer').show();
                    $('#curpointerR').show();
                    $('#curpointer').css('left', left + 'px').css('top', top + 'px');
                    $('#curpointerR').css('left', width + 'px').css('top', top + 'px');
                }
                catch (ex) {
                    $('#curpointer').hide()
                    $('#curpointerR').hide()
                }
            }
        }
}//
        } ();
    $().ready(function() {
        $('#' + webGrid.formName).submit(function() {
            webGrid.prepareSubmission();

        });

        $(document).ajaxError(function(e, xhr, settings, exception) {

            alert('There was an error processing your request: ' + (exception || xhr.responseText) + '\n' + settings.url);
        });
    });

    }

if (!gridFormatters) {
    var gridFormatters = function() {
        return {
            toUSDate:
		function(mydate) {
		    if (mydate == '' || mydate == null) {
		        return '';
		    }
		    var td = this.toJSDate(mydate);

		    return ("0" + (td.getMonth() + 1).toString()).slice(-2) + '/' + ("0" + td.getDate().toString()).slice(-2) + '/' + td.getFullYear().toString()
		}
    , toUSDateTime:
        function(mydate) {
            if (mydate == '' || mydate == null) {
                return '';
            }
            var td = this.toJSDate(mydate);
            return ("0" + (td.getMonth() + 1).toString()).slice(-2) + '/' + ("0" + td.getDate().toString()).slice(-2) + '/' + td.getFullYear().toString() + ' ' + td.getHours().toString() + ':' + ("0" + (td.getMinutes()).toString()).slice(-2) + ':' +  ("0" + (td.getSeconds()).toString()).slice(-2);
        }
	, toJSDate:
		function(mydate) {
		    if (mydate && mydate.getMonth) {
		        return mydate; //already a js date
		    }
		    var y, m, d
		    var a = mydate.split('/');
		    //input is mm/dd/yyyy
		    if (a.length >= 3) {
		        y = a[2];
		        m = a[0];
		        d = a[1];
		    }
		    else {
		        //input is yyyymmdd
		        y = mydate.substr(0, 4);
		        m = mydate.substr(4, 2);
		        d = mydate.substr(6, 2);
		    }
		    return new Date(y, Number(m) - 1, d);
		}

        }


    } ()
}
