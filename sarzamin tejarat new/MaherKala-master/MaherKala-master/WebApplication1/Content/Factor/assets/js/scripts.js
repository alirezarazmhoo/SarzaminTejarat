$( function() {

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

    //=============================//
    // r.dadkhah.tehrani@gmail.com //
    //=============================//
});