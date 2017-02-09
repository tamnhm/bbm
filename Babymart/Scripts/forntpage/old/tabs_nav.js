$(document).ready(function(){
    $(".tab-item:not(:first)").hide();
    $('.tab-list ul li a').click(function(){
        $('.tab-list ul li a').removeClass("active");
        $('.tab-item').hide();
        $(this).addClass("active");
        var activeTab = $(this).attr("href");
        $(activeTab).show();
        return false;
    })
    
});