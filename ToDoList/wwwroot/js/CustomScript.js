function confirmDelete(uniqueId, isDeleteClicked) {
    var deleteSpan = 'deleteSpan_' + uniqueId;
    var confirmDeleteSpan = 'confirmDeleteSpan_' + uniqueId;

    if (isDeleteClicked) {
        $('#' + deleteSpan).hide();
        $('#' + confirmDeleteSpan).show();
    } else {
        $('#' + deleteSpan).show();
        $('#' + confirmDeleteSpan).hide();
    }
}

function confirmColsed(uniqueId, isColsedClicked) {
    var colsedSpan = 'colsedSpan_' + uniqueId;
    var confirmColsedSpan = 'confirmColsedSpan_' + uniqueId;

    if (isColsedClicked) {
        $('#' + colsedSpan).hide();
        $('#' + confirmColsedSpan).show();
    } else {
        $('#' + colsedSpan).show();
        $('#' + confirmColsedSpan).hide();
    }

   
}
