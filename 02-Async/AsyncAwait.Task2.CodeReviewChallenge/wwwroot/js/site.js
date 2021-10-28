// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var site = site || {};
site.baseUrl = site.baseUrl || "";

$(document).ready(function (e) {

    // locate each partial section.
    // if it has a URL set, load the contents into the area.

    $(".partialContents").each(function (index, item) {
        var url = site.baseUrl + $(item).data("url") + "?visitedPagePath=" + $(item).data("path");
        if (url && url.length > 0) {
            $(item).load(url);
        }
    });
});