jQuery().ready(function () {

    let room = null;
    let movieId = jQuery("input[name='movieId']").val();
    let selectedScreening = null;
    const selectedSeats = [];

    jQuery("#booking-ticket").steps({
        headerTag: "h3",
        bodyTag: "section",
        enableFinishButton: false,
        onStepChanging: function (e, currentIndex, newIndex) {
            switch (currentIndex) {
                case 0:
                    if (!selectedScreening) {
                        swal('Must choose a screening', '', 'error');
                        return false;
                    }
                    jQuery.getJSON(`${baseUrl}/ajax/getAvailableSeats?screeningId=${selectedScreening.data('id')}`)
                        .done(function (data) {

                            let rowIndex = 0;
                            let containerSeats = jQuery('#seat-list');
                            containerSeats.html('');
                            for (var seats of data) {
                                let row = `<div class="d-flex align-items-center justify-content-center">`;
                                let ch = String.fromCharCode(rowIndex + 65);
                                row += `<div class="seat-list__heading p-3 font-weight-bold">${ch}</div>`;
                                for (var seat of seats) {
                                    let bgColor = "bg-primary";
                                    if (!seat.available) {
                                        bgColor = "bg-danger";
                                    }
                                    else {
                                        if (seat.type === 'VIP') {
                                            bgColor = "bg-success";
                                        }
                                        else if (seat.type === 'NONE') {
                                            bgColor = "bg-secondary";
                                        }
                                    }
                                    row += `<div class="seat-list__item text-white p-3 m-1 rounded ${bgColor}" data-type="${seat.type}" data-id="${seat.id}">${ch}${seat.col}</div>`
                                }
                                row += '<div>';
                                containerSeats.append(row);
                                rowIndex++;
                            }

                            jQuery('#seat-list .seat-list__item').click(function () {
                                if (jQuery(this).hasClass('bg-danger') || jQuery(this).hasClass('bg-secondary')) return;
                                jQuery(this).toggleClass('seat-list__item--selected');
                            });

                        });
                    return true;
                case 1:
                    if (newIndex < 1) return true;
                    selectedSeats.length = 0;
                    jQuery('#seat-list .seat-list__item--selected').each(function () {
                        let item = jQuery(this);
                        selectedSeats.push({ text: item.text(), id: item.data('id'), type: item.data('type') });
                    });
                    if (selectedSeats.length === 0) {
                        swal('Must choose a seat', '', 'error');
                        return false;
                    }
                    let totalElem = jQuery('#price-total');
                    let container = jQuery('#selected-seat-list');
                    container.html('');
                    let price = selectedScreening.data('price');
                    let total = 0;
                    for (let seat of selectedSeats) {
                        let seatPrice = seat.type === 'VIP' ? price * 1.1 : price;
                        total += seatPrice;
                        container.append(`<div class="col-12 col-sm-6 col-md-4 my-1">
                                <div class="rounded text-white p-2 ${seat.type === 'VIP' ? 'bg-success' : 'bg-primary'}">
                                    ${seat.text}<br/>
                                    ${seat.type}<br/>
                                    ${seatPrice.toPrecision(3)}$
                                </div>
                            </div>`);
                    }
                    totalElem.text('Total: ' + total.toPrecision(3) + '$');
                    return true;
                default: return true;
            }
        }
    });


    jQuery(".calendar-container").calendar({
        date: new Date(),
        onClickDate: (date) => {
            jQuery(".calendar-container").updateCalendarOptions({date})
        }
    });

    jQuery("select[name='roomId']").change(function () {
        let option = jQuery(this).find("option:selected");
        let i = option.val();
        if (!Number.isNaN(i) && i > 0) {
            room = {
                id: parseInt(i),
                name: option.text(),
                cinema: option.parent().attr("label"),
            }
        }
    });

    jQuery("#btnSearchScreening").click(function () {
        if (room === null) {
            swal('Choose a room', '', 'error');
            return;
        }
        var container = jQuery("#screening-list");
        container.html('');
        let screeningDate = jQuery(".calendar-container").calendar().getSelectedDate();
        jQuery.getJSON(`${baseUrl}/ajax/getScreenings?movieId=${movieId}&roomId=${room.id}&screeningDate=${moment(screeningDate).format("MM/DD/YYYY")}`)
            .done(function (data) {
                if (data.length === 0) {
                    container.append("<div class='text-center'>Screening data not found. Try another day</div>")
                    return;
                }
                for (let item of data) {
                    container.append(`<div class="col-12 col-sm-6 col-md-4 mt-3">
                            <div class="p-2 border rounded screening-list__item" data-id="${item.id}" data-price="${item.price}">
                                    <span class="badge badge-secondary">${item.format}</span>
                                    <h3>${item.price}$</h3>
                                    ${moment(item.screeningStart).format('HH:mm dddd MM/DD/YYYY')}
                            </div></div>
                        `);
                }
                jQuery("#screening-list .screening-list__item").click(function () {
                    if (selectedScreening) {
                        selectedScreening.removeClass("border-primary");
                    }
                    selectedScreening = jQuery(this);
                    jQuery(this).addClass("border-primary");
                });
            });
    });

    $("#form-booking").submit(function (e) {
        e.preventDefault();
        if (selectedScreening == null || selectedSeats.length === 0) return;
        let formData = $('#form-booking').serializeArray();
        let reservation = {};
        for (let item of formData) {
            reservation[item.name] = item.value;
        }
        reservation["ScreeningId"] = selectedScreening.data('id');
        reservation["Seats"] = selectedSeats.map(i => i.id);
        reservation["CardDate"] = moment(reservation["CardDate"]).format("MM/DD/YYYY");

        swal('Waiting for booking...', '', 'info');
        jQuery.ajax({
            url: `${baseUrl}/ajax/booking`,
            data: JSON.stringify(reservation),
            headers: {
                "Content-type": "application/json"
            },
            method: "post"
        }).done(function (res) {
            if (res.error) {
                swal(res.error, '', 'error');
            }
            else {
                swal('Saved!', '', 'success');
                window.location.href = baseUrl;
            }
        });
    });

    let carousel = jQuery("#movie-carousel");
    if (carousel.length > 0) {
        carousel.slick({
            slidesToShow: 5,
            centerMode: true,
            adaptiveHeight: true,
            infinite: true,
            autoplay: true,
            responsive: [
                {
                    breakpoint: 1024,
                    settings: {
                        centerMode: true,
                        adaptiveHeight: true,
                        slidesToShow: 3,
                        infinite: true,
                        autoplay: true,
                    }
                },
                {
                    breakpoint: 480,
                    settings: {
                        slidesToShow: 3,
                        infinite: true,
                        autoplay: true,
                        centerMode: false
                    }
                },
                {
                    breakpoint: 350,
                    settings: {
                        slidesToShow: 2,
                        infinite: true,
                        autoplay: true,
                        centerMode: false
                    }
                }
            ]
        });
    }

    jQuery("input[name='CardNumber']").inputmask("9999 9999 9999 9999");
    jQuery("input[name='Cvc']").inputmask("999");
    jQuery("input[name='Phone']").inputmask("9{10,15}");

    jQuery('.starrr.starrr-readonly').each(function () {
        jQuery(this).starrr({
            readOnly: true,
            rating: jQuery(this).data('value')
        });
    });

    jQuery('.starrr').starrr();

    jQuery('.starrr').on('starrr:change', function (e, value) {
        jQuery(this).next().val(value);
    });

    jQuery('.moment-time').each(function () {
        jQuery(this).text(moment(jQuery(this).data('value')).fromNow());
    });

    jQuery('.form-rate').submit(function (e) {
        e.preventDefault();
        let form = jQuery(this);
        let rate = {};
        for (let item of jQuery(this).serializeArray()) {
            rate[item.name] = item.value;
        }

        swal('Waiting for submit...', '', 'info');

        jQuery.ajax({
            url: `${baseUrl}/ajax/rate`,
            method: 'post',
            data: JSON.stringify(rate),
            headers: {
                "Content-type": "application/json"
            },
        }).done(function (res) {
            if (res.error) {
                swal(res.error, '', 'error');
            }
            else {
                swal('Rate saved. Thanks!', '', 'success');
                form.parent().html("You have rated");
            }
        });
    });
});