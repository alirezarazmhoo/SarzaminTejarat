$( function() {

    // Disable a with # href
    //=====================================================================

    $('a[href="#"]').click(function(e){
        e.preventDefault();
    });


    // Mobile Menu
    //=====================================================================

    $('.main-header__mobile-menu-toggle a').click(function(){
        if($('.mobile-menu').hasClass('.mobile-menu--open'))
        {
            $('.mobile-menu').removeClass('mobile-menu--open');
        }
        else
        {
            $('.mobile-menu').addClass('mobile-menu--open');
        }
    });

    $('.mobile-menu__close-menu').click(function(){
        $('.mobile-menu').removeClass('mobile-menu--open');
    });
    $('.main-header__mobile-menu-toggle a,.mobile-menu').click(function(event){
        event.stopPropagation();
    });
    $(document).click(function(){
        $('.mobile-menu').removeClass('mobile-menu--open');
    });

    $('.mobile-menu nav > ul > li').each(function() {
        $(this).has('ul').addClass('has-child').prepend('<i class="dk-arrow-down"></i>')
    });
    $('.mobile-menu nav > ul > li').on('click', '>i', function() {
        if ($(this).parent().hasClass('open')) {
            $(this).parent().removeClass('open').find('>ul').slideUp()
        } else {
            $('.mobile-menu nav > ul > li > ul').slideUp().parent().removeClass('open');
            $(this).parent().addClass('open').find('>ul').slideDown()
        }
    });


    // Header Fix Scroll
    //=====================================================================


    var scroll = $(window).scrollTop();
    var header_offset = $('.main-header__bottom').offset().top;
    if(scroll >= header_offset)
    {
        $('body').addClass('header-fixed')
    }
    else
    {
        $('body').removeClass('header-fixed')
    }

    $(window).scroll(function(){
        scroll = $(window).scrollTop();
        if(scroll >= header_offset)
        {
            $('body').addClass('header-fixed')
        }
        else
        {
            $('body').removeClass('header-fixed')
        }
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

    //=============================//
    // r.dadkhah.tehrani@gmail.com //
    //=============================//
});