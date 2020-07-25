$(function () {

    $('a[href="#"]').click(function (e) {
        e.preventDefault();
    });
    // MobileMenu
    //=====================================================================

    $('.main-header__mobile-menu-toggle a').click(function () {
        if ($('.mobile-menu').hasClass('.mobile-menu--open')) {
            $('.mobile-menu').removeClass('mobile-menu--open');
        }
        else {
            $('.mobile-menu').addClass('mobile-menu--open');
        }
    });

    $('.mobile-menu__close-menu').click(function () {
        $('.mobile-menu').removeClass('mobile-menu--open');
    });
    $('.main-header__mobile-menu-toggle a,.mobile-menu').click(function (event) {
        event.stopPropagation();
    });
    $(document).click(function () {
        $('.mobile-menu').removeClass('mobile-menu--open');
    });

    $('.mobile-menu nav > ul > li').each(function () {
        $(this).has('ul').addClass('has-child').prepend('<i class="dk-arrow-down"></i>')
    });
    $('.mobile-menu nav > ul > li').on('click', '>i', function () {
        if ($(this).parent().hasClass('open')) {
            $(this).parent().removeClass('open').find('>ul').slideUp()
        } else {
            $('.mobile-menu nav > ul > li > ul').slideUp().parent().removeClass('open');
            $(this).parent().addClass('open').find('>ul').slideDown()
        }
    });


    var scroll = $(window).scrollTop();
    var header_offset = $('.main-header__bottom').offset().top;
    if (scroll >= header_offset) {
        $('body').addClass('header-fixed')
    }
    else {
        $('body').removeClass('header-fixed')
    }

    $(window).scroll(function () {
        scroll = $(window).scrollTop();
        if (scroll >= header_offset) {
            $('body').addClass('header-fixed')
        }
        else {
            $('body').removeClass('header-fixed')
        }
    });


    // Tabs
    //=====================================================================

    $(document).on('click', '.tabs li[data-tab]', function () {

        tab = $(this).data('tab');
        $(this).addClass('active')
            .siblings()
            .removeClass('active');

        $(this).closest('.tabs')
            .find('.tab-content[data-tabtarget="' + tab + '"]')
            .addClass('active')
            .siblings()
            .removeClass('active');
    });


    // Toaster Alert
    //=====================================================================

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

    // Toast.fire({
    //     icon: 'success',
    //     title: 'توستر',
    //     footer: 'این تست توستر است',
    // });


    // Main Slider
    //=====================================================================

    if ($('.main-slider__slider').length > 0) {
        var detail_image = new Swiper('.main-slider__slider .swiper-container', {
            loop: true,
            slidesPerView: 1,
            autoplay: {
                delay: 1500,
                disableOnInteraction: false
            },
        });
    }


    // Product Slider
    //=====================================================================

    if ($('.product--slider').length > 0) {
        var detail_image = new Swiper('.product--slider .swiper-container', {
            loop: true,
            slidesPerView: 2,
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
    }




    // Specials Countdown
    //=====================================================================
    $('.product [data-countdown]').each(function (i, e) {
        let el = $(e);
        let time = el.data('countdown');
        el.countdown(time).on('update.countdown', function (event) {
            el.html(event.strftime('%-H:%-M:%S'));
        });
    });









    // DETAIL

    var product_rate = $('.single-product__rate__number').data('rate');
    $('.single-product__rate__inner label').on('mouseenter', function () {
        var rate = $(this).data('rate');
        $('.single-product__rate__number').html(rate);
    });

    $('.single-product__rate__inner').on('mouseleave', function () {
        $('.single-product__rate__number').html(product_rate);
    });

    $('.single-product__rate__inner label').on('click', function () {
        rate = $(this).data('rate');
        $('.single-product__rate__number').html(rate).attr('data-rate', rate);
        product_rate = rate;

        Toast.fire({
            icon: 'success',
            title: 'ثبت شد',
            footer: 'امتیاز شما ثبت شد',
        });

        // AJAX HERE
        console.log('User Rate:', rate);
    });


    var detail_image_thumb = new Swiper('.single-product__image__thumb-slider .swiper-container', {
        slidesPerView: 4,
        spaceBetween: 15,
        centeredSlide: 1,
        watchSlidesVisibility: true,
        watchSlidesProgress: true,
    });

    var detail_image = new Swiper('.single-product__image__main-slider .swiper-container', {
        simulateTouch: false,
        thumbs: {
            swiper: detail_image_thumb
        },
        effect: 'fade',
        fadeEffect: {
            crossFade: true
        },
    });

    $('.zoom').zoom();

    $('[data-bgcolor]').each(function (i, e) {
        $(e).css('background-color', $(e).data('bgcolor'))
    });

    $('[data-color]').each(function (i, e) {
        $(e).css('color', $(e).data('color'))
    });


    var comments_slider = new Swiper('.comments--slider .swiper-container', {
        slidesPerView: 1,
        spaceBetween: 20,
        breakpoints: {
            992: {
                slidesPerView: 2.2
            }
        }
    });

    // CART

    $('.cart__item__quantity i').on('click', function () {
        let el = $(this);
        let type = el.data('type');
        let span = el.siblings('span');
        let input = el.siblings('input');
        let val = parseInt(span.data('value'));
        let max = parseInt(span.data('max'));
        let min = parseInt(span.data('min'));
        if (type == 'plus' && val < max) {
            input.val(val + 1);
            span.text(val + 1).data('value', val + 1);
        }
        else if (type == 'minus' && val > min) {
            input.val(val - 1);
            span.text(val - 1).data('value', val - 1);
        }
    })

    //=============================//
    // r.dadkhah.tehrani@gmail.com //
    //=============================//
});