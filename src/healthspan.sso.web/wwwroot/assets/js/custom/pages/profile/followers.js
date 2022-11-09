"use strict";

// Class definition
var KTProfileFollowers = function () {
    // init variables
    var showMoreButton = document.getElementById('kt_followers_show_more_button');
    var showMoreCards = document.getElementById('kt_followers_show_more_cards');

    // Private functions
    var handleShowMore = function () {
        // Show more click
        showMoreButton.addEventListener('click', function (e) {
            showMoreButton.setAttribute('data-kt-indicator', 'on');

            // Disable button to avoid multiple click 
            showMoreButton.disabled = true;

            setTimeout(function() {
                // Hide loading indication
                showMoreButton.removeAttribute('data-kt-indicator');

                // Enable button
				showMoreButton.disabled = false;

                // Hide button
                showMoreButton.classList.add('d-none');

                // Show card
                showMoreCards.classList.remove('d-none');

                // Scroll to card
                KTUtil.scrollTo(showMoreCards, 200);
            }, 2000);
        });
    }

    // Follow button
    var initUserFollowButton = function() {

        // Dismiss handler
        KTUtil.on(document.body,  '[data-kt-user-follow="true"]', 'click', function(e) {
            e.preventDefault();
            var follow = this;
                   
            // Show indicator
            follow.setAttribute('data-kt-indicator', 'on');
            
            // Disable button to avoid multiple click 
            follow.disabled = true;

            // Check button state
            if (follow.classList.contains("btn-primary")) {
                setTimeout(function() {
                    follow.removeAttribute('data-kt-indicator');
                    follow.classList.remove("btn-primary");
                    follow.classList.add("btn-light");
                    follow.querySelector(".svg-icon").classList.add("d-none");
                    follow.querySelector(".indicator-label").innerHTML = 'Follow';
                    follow.disabled = false;
                }, 1500);   
            } else {
                setTimeout(function() {
                    follow.removeAttribute('data-kt-indicator');
                    follow.classList.add("btn-primary");
                    follow.classList.remove("btn-light");
                    follow.querySelector(".svg-icon").classList.remove("d-none");
                    follow.querySelector(".indicator-label").innerHTML = 'Following';
                    follow.disabled = false;
                }, 1000);   
            } 
        });
    }

    // Public methods
    return {
        init: function () {
            handleShowMore();
            initUserFollowButton();
        }
    }
}();


// On document ready
KTUtil.onDOMContentLoaded(function() {
    KTProfileFollowers.init();
});