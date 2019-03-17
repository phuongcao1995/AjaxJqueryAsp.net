/// <reference path="jquery.validate.js" />
/// <reference path="bootstrap.js" />
/// <reference path="jquery.validate.unobtrusive.js" />
/// <reference path="notify.min.js" />
/// <reference path="jquery-3.3.1.min.js" />

$(function () {
    ActiveJqueryDataTable();
    WaitAjax();
});
function ShowImagePreview(imageUpLoader, imagePreview) {
    if (imageUpLoader.files && imageUpLoader.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(imagePreview).attr("src", e.target.result);
        };
        reader.readAsDataURL(imageUpLoader.files[0]);
    }
}
function AjaxJQueryPost(form) {
    $.validator.unobtrusive.parse(form);

    if ($(form).valid()) {
        var ajaxConfig = {
            type: "POST",
            url: form.action,
            data: new FormData(form),
            success: function (response) {
                if (response.success){
                    $("#viewAll").html(response.html);
                    refreshAddNewTab($(form).attr("data-resetUrl"), true);
                    $.notify(response.message, "success");
                    if (typeof (ActiveJqueryDataTable) !== "undefined" && $.isFunction(ActiveJqueryDataTable)){
                        ActiveJqueryDataTable();
                    }
                } else {
                    $.notify(response.message, "error");
                }
               
            }
        };
        if ($(form).attr("enctype") === "multipart/form-data") {
            ajaxConfig["contentType"] = false;
            ajaxConfig["processData"] = false;

        }
        $.ajax(ajaxConfig);
    }
    return false;

}
function refreshAddNewTab(resetUrl, showViewTab) {
    $.ajax({
        type: "GET",
        url: resetUrl,
        success: function (response) {
            $("#addNew").html(response);
            $("ul.nav.nav-tabs a:eq(1)").html("AddNew");
            if (showViewTab) {
                $("ul.nav.nav-tabs a:eq(0)").tab("show");

            }
      }
    });
}
function Edit(url) {
    $.ajax({
        type: "GET",
        url: url,
        success: function (response) {
            $("#addNew").html(response);
            $("ul.nav.nav-tabs a:eq(1)").html("Edit");
            $("ul.nav.nav-tabs a:eq(1)").tab("show");

        }
    });
}

function Delete(url) {
    if (confirm('Are you sure to to delete this record?')) {
        $.ajax({
            type: "Post",
            url: url,
            success: function (response) {
                if (response.success) {
                    $("#viewAll").html(response.html);
                    $("ul.nav.nav-tabs a:eq(1)").html("AddNew");
                    $.notify(response.message, "success");
                } else {
                    $.notify(response.message, "error");
                }
            }
        });
    }

}
function ActiveJqueryDataTable() {
    $("#employee-table").DataTable();
}
function WaitAjax() {
    $("#loaderbody").addClass('hide');
    $(document).bind('ajaxStart', function () {
        $("#loaderbody").removeClass('hide');
    }).bind('ajaxStop', function () {
        $("#loaderbody").addClass('hide');
    });
}