var AdvertisementDetailsController = function (followingService) {
    var followButton;

    var init = function () {
        $(".js-toggle-follow").click(toggleFollowing);
    };

    var toggleFollowing = function (e) {
        followButton = $(e.target);
        var followeeId = followButton.attr("data-user-id");

        if (followButton.hasClass("btn-primary"))
            followingService.createFollowing(followeeId, done, fail);
        else
            followingService.deleteFollowing(followeeId, done, fail);

    };

    var done = function () {
        var text = (followButton.text() == "Obserwuj") ? "Obserwowany" : "Obserwuj";
        followButton.toggleClass("btn-info").toggleClass("btn-primary").text(text);
    };

    var fail = function () {
        alert("Something failed");
    };

    return {
        init: init
    }

}(FollowingService);