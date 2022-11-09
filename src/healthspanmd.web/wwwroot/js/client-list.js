
function viewProgress(e) {
    e.preventDefault();
    actionDataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var url = "/Analytics/Progress/" + actionDataItem.UserId;
    window.location.href = url;
}

function editUser(e) {
    e.preventDefault();
    actionDataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var url = "/Account/AdminDetail/" + actionDataItem.UserId;
    window.location.href = url;
}

function viewAssignments(e) {
    e.preventDefault();
    actionDataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var url = "/Account/ContentAssignments/" + actionDataItem.UserId;
    window.location.href = url;
}


var userIdToDeactivate = -1;
function confirmDeactivateUser(id) {
    userIdToDeactivate = id;
    $("#modalConfirmDeactivate").modal("show");
}

function deactivateUser() {
    var serviceUrl = "/Account/Deactivate/" + userIdToDeactivate;
    $.ajax({
        url: serviceUrl,
        type: "get",
        contentType: "application/json",
        success: function (response) {
            if (response.success) {
                $("#clientListContainer").html(response.html);
                KTMenu.createInstances();
                $("#modalConfirmDeactivate").modal("hide");
            } else {
                console.log(serviceUrl + " did not succeed");
            }
            //hideBusySpinner();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(serviceUrl + " failed");
            //hideBusySpinner();
        }
    });
}


var userIdToDelete = -1;
function confirmDeleteUser(id) {
    userIdToDelete = id;
    $("#modalConfirmDelete").modal("show");
}

function deleteUser() {
    var serviceUrl = "/Account/Delete/" + userIdToDelete;
    $.ajax({
        url: serviceUrl,
        type: "get",
        contentType: "application/json",
        success: function (response) {
            if (response.success) {
                $("#clientListContainer").html(response.html);
                KTMenu.createInstances();
                $("#modalConfirmDelete").modal("hide");
            } else {
                console.log(serviceUrl + " did not succeed");
            }
            //hideBusySpinner();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(serviceUrl + " failed");
            //hideBusySpinner();
        }
    });
}

function activateUser(userId) {
    var serviceUrl = "/Account/Activate/" + userId;
    $.ajax({
        url: serviceUrl,
        type: "get",
        contentType: "application/json",
        success: function (response) {
            if (response.success) {
                $("#clientListContainer").html(response.html);
                KTMenu.createInstances();
            } else {
                console.log(serviceUrl + " did not succeed");
            }
            //hideBusySpinner();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(serviceUrl + " failed");
            //hideBusySpinner();
        }
    });
}