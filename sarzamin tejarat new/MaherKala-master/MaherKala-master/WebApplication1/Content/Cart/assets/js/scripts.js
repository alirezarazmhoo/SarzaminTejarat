$( function() {

    // Cart Produc Quantity Value

    $('.cart__item__quantity i').on('click',function(){
        let el = $(this);
        let type = el.data('type');
        let span = el.siblings('span');
        let input = el.siblings('input');
        let val = parseInt(span.data('value'));
        let max = parseInt(span.data('max'));
        let min = parseInt(span.data('min'));
        if(type == 'plus' && val < max)
        {   
            input.val(val+1);
            span.text(val+1).data('value',val+1);
        }
        else if(type == 'minus' && val > min)
        {
            input.val(val-1);
            span.text(val-1).data('value',val-1);
        }
    })


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