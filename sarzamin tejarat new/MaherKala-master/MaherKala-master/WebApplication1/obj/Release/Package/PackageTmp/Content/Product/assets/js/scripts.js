$( function() {

    const Toast = Swal.mixin({
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true,
        onOpen: (toast) => {
            toast.addEventListener('mouseenter', Swal.stopTimer)
            toast.addEventListener('mouseleave', Swal.resumeTimer)
        }
    });

    // Produc Rating star
    //=====================================================================

    var product_rate = $('.single-product__rate__number').data('rate');
    $('.single-product__rate__inner label').on('mouseenter',function(){
        var rate = $(this).data('rate');
        $('.single-product__rate__number').html(rate);
    })

    $('.single-product__rate__inner').on('mouseleave',function(){
        $('.single-product__rate__number').html(product_rate);
    })

    $('.single-product__rate__inner label').on('click',function(){
        rate = $(this).data('rate');
        $('.single-product__rate__number').html(rate).attr('data-rate',rate);
        product_rate = rate;

        Toast.fire({
            icon: 'success',
            title: 'ثبت شد',
            footer: 'امتیاز شما ثبت شد',
        });

        // AJAX HERE
        console.log('User Rate:',rate);
    })


    // Produc Thumb Slider
    //=====================================================================

    var detail_image_thumb = new Swiper('.single-product__image__thumb-slider .swiper-container', {
        slidesPerView: 4,
        spaceBetween: 15,
        centeredSlide: 1,
        watchSlidesVisibility: true,
        watchSlidesProgress: true,
    })

    // Produc Image Slider
    //=====================================================================

    var detail_image = new Swiper('.single-product__image__main-slider .swiper-container', {
        simulateTouch:false,
        thumbs: {
            swiper: detail_image_thumb
        },
        effect: 'fade',
        fadeEffect: {
            crossFade: true
        },
    })

    // Produc Image Zoom
    //=====================================================================

    $('.zoom').zoom()


    // Gets background-color code from HTML and set --> Ex: data-bgcolor="#aaa222"
    //=====================================================================

    $('[data-bgcolor]').each(function(i,e){
        $(e).css('background-color',$(e).data('bgcolor'))
    })


    // Gets color code from HTML and set --> Ex: data-color="#fff"
    //=====================================================================

    $('[data-color]').each(function(i,e){
        $(e).css('color',$(e).data('color'))
    })


    // Comments Slider
    //=====================================================================

    var comments_slider = new Swiper('.comments--slider .swiper-container', {
        slidesPerView: 1,
        spaceBetween: 20,
        breakpoints: {
            992: {
              slidesPerView: 2.2
            }
        }
    })

    // Product related slider
    //=====================================================================

    var related_slider = new Swiper('.product--slider .swiper-container', {
        loop: true,
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
    })


    //=============================//
    // r.dadkhah.tehrani@gmail.com //
    //=============================//
});