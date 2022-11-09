var videoContentItems = [];
//$(document).ready(function () {
//    window.addEventListener("resize", function () {
//        onWindowResize()
//    });
//    initVideoPlayers();
//});

function initVideo() {
    console.log("initVideo()");
    window.addEventListener("resize", function () {
        onWindowResize()
    });
    initVideoPlayers();
    
}

function initVideoPlayers() {

    var tag = document.createElement('script');

    tag.src = "https://www.youtube.com/iframe_api";
    var firstScriptTag = document.getElementsByTagName('script')[0];
    firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);
}

function onWindowResize() {
    var iframes = $(".content_item_video");
    var dheight = (document.getElementById("cardContainer").offsetWidth - 40) * .5625;
    var dwidth = document.getElementById("cardContainer").offsetWidth - 40;
    iframes.each(function (i) {
        $(this).width(dwidth);
        $(this).height(dheight);
    });
}

function onYouTubeIframeAPIReady() {
    console.log("------------  onYouTubeIframeAPIReady  ---------------");
    var dheight = (document.getElementById("cardContainer").offsetWidth - 40) * .5625;
    var dwidth = document.getElementById("cardContainer").offsetWidth - 40;

    for (i = 0; i < videoContentItems.length; i++) {
        var contentItem = videoContentItems[i];
        var playerId = "ytplayer_" + contentItem.Id;
        console.log("playerId = " + playerId);
        var player = new YT.Player(playerId, {
            height: dheight,
            width: dwidth,
            videoId: contentItem.Url,
            playerVars: {
                'playsinline': 1,
                'modestbranding': 1,
                'controls': 1,
                'rel': 0
            },
            events: {
                'onReady': onPlayerReady,
                'onStateChange': onYouTubePlayerStateChange
		    }
		});
    }

    //@foreach(var member in Model.ContentCard.ContentCardMembers.Where(m => m.ContentItem.ItemType == healthspanmd.core.CQRS.Content.ContentItemType.Video)) {
    //    var contentItem = member.ContentItem;

    //    <text>
    //        var playerId = "ytplayer_@contentItem.ContentItemId";
    //        var player = new YT.Player(playerId, {
    //            height: dheight,
    //        width: dwidth,
    //        videoId: "@contentItem.Url",
    //        playerVars: {
    //            'playsinline': 1,
    //        'modestbranding': 1,
    //        'controls': 1,
    //        'rel': 0
    //                    },
    //        events: {
    //            'onReady': onPlayerReady,
    //        'onStateChange': onYouTubePlayerStateChange
			 //           }
		  //          });

    //    </text>
    //}
}

function onPlayerReady(event) {
    console.log(" ---- onPlayerReady ---- ");
}

function onYouTubePlayerStateChange(event) {
    var playerId = event.target.i.id;
    var contentItemId = playerId.replace("ytplayer_", "");
    if (event.data == 0) {
        // the video has ended, 
        // notify the app that the user has completed the video
        var serviceUrl = "/Content/MarkCardAsCompleted/" + contentCardId + "/" + contentItemId;

        $.ajax({
            url: serviceUrl,
            type: "get",
            success: function (response) {
                if (response.success) {
                    console.log("Video marked as completed");
                } else {
                    console.log(serviceUrl + " did not succeed");
                }
                hideBusySpinner();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(serviceUrl + " failed");
                hideBusySpinner();
            }
        });
    }
}