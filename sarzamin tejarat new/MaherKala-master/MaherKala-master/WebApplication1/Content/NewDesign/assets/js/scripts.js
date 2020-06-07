$( function() {


    // Main Slider
    //=====================================================================

    var main_slider = new Swiper('.main-slider__slider .swiper-container', {
        loop: true,
        slidesPerView: 1,
        autoplay: {
          delay: 1500,
          disableOnInteraction: false
        },
    })


    // Product Slider
    //=====================================================================

    var product_slider = new Swiper('.product--slider .swiper-container', {
        loop: false,
        slidesPerView: 1,
        spaceBetween: 20,
        breakpoints: {
            1400: {
              slidesPerView: 6.5
            },
            1200: {
              slidesPerView: 5.5
            },
            992: {
              slidesPerView: 4.5
            },
            768: {
              slidesPerView: 3.5
            },
            480: {
              slidesPerView: 2
            }
        }
    });


    // Specials product countdown
    //=====================================================================
    $('.product [data-countdown]').each(function(i,e){
        let el = $(e);
        let time = el.data('countdown');
        el.countdown(time).on('update.countdown', function(event) {
            el.html(event.strftime('%-H:%-M:%S'));
        });
    });


    //=============================//
    // r.dadkhah.tehrani@gmail.com //
    //=============================//
});