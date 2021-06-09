jQuery(document).ready(function($){
    $('<style>'+
        '.scrollTop{ display:none; z-index:9999; position:fixed;'+
            'bottom:20px; left:5%; width:88px; height:125px;'+
            'background:url(https://biznessystem.ru/img/arrow.png) 0 0 no-repeat; }' +
        '.scrollTop:hover{ background-position:0 -133px;}'
    +'</style>').appendTo('body');
    var
    speed = 550,
    $scrollTop = $('<a href="#" class="scrollTop">').appendTo('body');		
    $scrollTop.click(function(e){
        e.preventDefault();
        $( 'html:not(:animated),body:not(:animated)' ).animate({ scrollTop: 0}, speed );
    });

    //появление
    function show_scrollTop(){
        ( $(window).scrollTop() > 1000 ) ? $scrollTop.fadeIn(700) : $scrollTop.fadeOut(700);
    }
    $(window).scroll( function(){ show_scrollTop(); } );
    show_scrollTop();
});