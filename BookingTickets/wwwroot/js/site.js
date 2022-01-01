jQuery().ready(function () {

    let room = null;
    let movieId = jQuery("input[name='movieId']").val();
    let screeningDate = jQuery("input[name='screeningDate']").val();
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
                        alert('Must choose a screening');
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
                        alert('Must choose a seat');
                        return false;
                    }
                    let totalElem = jQuery('#totalPrice');
                    let container = jQuery('#selected-seat-list');
                    container.html('');
                    let price = selectedScreening.data('price');
                    let total = 0;
                    for (let seat of selectedSeats) {
                        let seatPrice = seat.type === 'VIP' ? price * 1.1 : price;
                        total += seatPrice;
                        container.append(`<div class="col-4">
                                <div class="rounded text-white p-2 ${seat.type === 'VIP' ? 'bg-success' : 'bg-primary'}">
                                    ${seat.text}<br/>
                                    ${seat.type}<br/>
                                    ${seatPrice.toPrecision(3)}$
                                </div>
                            </div>`);
                    }
                    totalElem.text(total.toPrecision(3) + '$');
                    return true;
                default: return true;
            }
        }
    });

    jQuery("input[name='screeningDate']").datetimepicker({ timepicker: false, format: 'm/d/Y' });

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

    jQuery("input[name='screeningDate']").change(function () {
        screeningDate = jQuery(this).val();
    });

    jQuery("#btnSearchScreening").click(function () {
        if (room === null) {
            alert('Choose a room');
            return;
        }
        var container = jQuery("#screening-list");
        container.html('');
        jQuery.getJSON(`${baseUrl}/ajax/getScreenings?movieId=${movieId}&roomId=${room.id}&screeningDate=${screeningDate}`)
            .done(function (data) {
                if (data.length === 0) {
                    container.append("<div class='text-center'>Screening data not found</div>")
                    return;
                }
                for (let item of data) {
                    container.append(`<div class="col col-sm-6 mt-sm-0 mt-3">
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

    $("#formBooking").submit(function (e) {
        e.preventDefault();
        if (selectedScreening == null || selectedSeats.length === 0) return;
        let formData = $('#formBooking').serializeArray();
        let reservation = {};
        for (let item of formData) {
            reservation[item.name] = item.value;
        }
        reservation["ScreeningId"] = selectedScreening.data('id');
        reservation["Seats"] = selectedSeats.map(i => i.id);
        jQuery.ajax({
            url: `${baseUrl}/ajax/booking`,
            data: JSON.stringify(reservation),
            headers: {
                "Content-type": "application/json"
            },
            method: "post"
        }).done(function (res) {
            if (res.error) {
                alert(res.error);
            }
            else {
                alert(res.message);
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
                }
            ]
        });
    }
});