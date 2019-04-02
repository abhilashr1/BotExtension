// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Jquery DOM binds
(function () {
    $("#mstestbtn").click(function (e) {
        e.preventDefault();
        var appid = $("#msappId").html();
        var password = $("#mspasswd").html();

        // TODO: Ask for the endpoint manually
        $.ajax(
            {
                url: "/BotExtension/api/key/microsoft",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({ appId: appid, password: password, url: window.location.origin }),
                success: function (result) {
                    if (result.statusCode === 200) {
                        $("#msresult").html("<b>Valid!</b>");
                    }
                    else {
                        $("#msresult").html("<b>Invalid!</b><br><div>" + result.reasonPhrase+"</div>");
                    }
            }
            }); 
    });

    // Hide LUIS fields
    $("#luistextbox").hide();
    $("#luismsgbutton").hide();

    $("#luistestbtn").click(function (e) {
        e.preventDefault();
        $("#luistextbox").toggle();
        $("#luismsgbutton").toggle();
    });

    $("#luismsgbutton").click(function (e) {
        e.preventDefault();
        var appid = $("#luisappId").html();
        var password = $("#luiskey").html();
        var Hostname = $("#luishostname").html();
        var query = $("#luistextbox").val();

        // TODO: Ask for the endpoint manually
        $.ajax(
            {
                url: "/BotExtension/api/key/luis",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({ LuisAppId: appid, LuisKey: password, Hostname: Hostname, Query: query }),
                success: function (data, textStatus, jqXHR) {
                    if (textStatus === "success") {
                        $("#luisresult").html(data);
                    } else 
                        $("#luisresult").html("<b>Invalid!</b><br><div>" + textStatus +"</div>");
                }
            });
    });

    // Hide QNA fields
    $("#qnatextbox").hide();
    $("#qnamsgbutton").hide();

    $("#qnatestbtn").click(function (e) {
        e.preventDefault();
        $("#qnatextbox").toggle();
        $("#qnamsgbutton").toggle();
    });

    $("#qnamsgbutton").click(function (e) {
        e.preventDefault();
        var appid = $("#qnaappId").html();
        var password = $("#qnakey").html();
        var Hostname = $("#qnahostname").html();
        var query = $("#qnatextbox").val();

        // TODO: Ask for the endpoint manually
        $.ajax(
            {
                url: "/BotExtension/api/key/qna",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({ KbId: appid, Key: password, Hostname: Hostname, Query: query }),
                success: function (data, textStatus, jqXHR) {
                    if (textStatus === "success") {
                        $("#qnaresult").html(data);
                    } else
                        $("#qnaresult").html("<b>Invalid!</b><br><div>" + textStatus + "</div>");
                }
            });
    });
}).call(this);