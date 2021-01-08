// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {

    var $body = $('body').first();

    var showDelay = 600;
    var hideDelay = 450;
    var animationDuration = 200;
    var timeout = null;

    $(document).on("mouseenter", ".pic-badge", function () {

        clearTimeout(timeout);

        var $cards = $(".hover-card", $body);

        setTimeout(function () {
            $cards.stop().remove();
        }, hideDelay);

        var $badge = $(this);

        $.ajax({
            url: "/api/user/" + $badge.attr("data-userId"),
            type: 'GET',
            dataType: "json",
            headers: {
                "Authorization": "Bearer " + $('#tokenField').val()
            },
            complete: function (response) {
                if (response.status == 200) {
                    var user = response.responseJSON;

                    console.log(user);

                    var roleDescription = "";

                    if (user.roles.length) {
                        roleDescription = user.roles[0].description;
                    }

                    var html = '\
                        <div class="hover-card card shadow-sm">\
                            <div class="row no-gutters flex-nowrap">\
                                <div class="col" style="width:100px">\
                                    <img class="card-img rounded-circle" alt="Avatar"\
                                         src="' + $badge.attr('src') + '">\
                                </div>\
                                <div class="col">\
                                    <div class="card-body p-0 pl-3">\
                                        <p class="card-title m-0">'+ user.firstName + ' ' + user.lastName + '</p>\
                                        <p class="card-text">\
                                            <small class="text-muted">' + roleDescription + '</small>\
                                        </p>\
                                        <p class="card-text m-0">\
                                            <small><span class="mr-1">Country:</span> <i class="flag ' + user.country.code + '"></i> ' + user.country.name + '</small>\
                                        </p>\
                                        <p class="card-text">\
                                            <small><span class="mr-1">Member since:</span> ' + new Date(user.dateJoined).getFullYear() + '</small>\
                                        </p>\
                                    </div >\
                                </div>\
                            </div>\
                        </div>';

                    var $div = $(html);
                    var pos = $badge.offset();

                    $div.css({
                        display: "none",
                        position: "absolute",
                        top: pos.top,
                        left: pos.left,
                        "box-sizing": "content-box"
                    });

                    $div.appendTo($body);

                    var w = $div.width() + 'px';
                    var h = $div.height() + 'px';
                    var $img = $(".card-img", $div);

                    $img.add($div).width($badge.width()).height($badge.height());

                    timeout = setTimeout(function () {
                        $div.css("display", "block");
                        $img.animate({ width: "100px", height: "100px" }, animationDuration);
                        $div.animate({ width: w, height: h, padding: ".7rem .5rem" }, animationDuration);
                    }, showDelay);
                }
            }
        });
    });

    var debounce = null;

    $(document).on("mouseleave", ".hover-card", function () {
        var self = this;
        clearTimeout(debounce);
        debounce = setTimeout(function (){
            $(self).stop().remove();
        }, hideDelay);
    });

    let $selCounter = $('#js-selection-counter');

    $(".js-table-check").each(function (i, table) {
        let $tbody = $('tbody', table);
        let $checkAll = $("thead input[type='checkbox']", table).first();

        function updateCounter(count) {
            $selCounter.text(count > 0 ? count : "");
        }

        $tbody.on('change', "input[type='checkbox']", function () {
            $(this).closest('tr').toggleClass("table-active", this.checked);

            let $checks = $("input[type='checkbox']", $tbody);
            let nChecked = $checks.filter(':checked').length;

            if (this.checked && nChecked == $checks.length) {
                $checkAll.prop({
                    indeterminate: false,
                    checked: true
                });
            }
            else if (nChecked == 0) {
                $checkAll.prop({
                    indeterminate: false,
                    checked: false
                });
            }
            else {
                $checkAll.prop({
                    indeterminate: true,
                    checked: false
                });
            }

            updateCounter(nChecked);
        });

        $checkAll.on('change', function () {
            let $checks = $("input[type='checkbox']", $tbody);

            $checks.prop('checked', this.checked);
            $('tr', $tbody).toggleClass("table-active", this.checked);

            let nChecked = $checks.filter(':checked').length;
            updateCounter(nChecked);
        });
    });
});
