$(document).ready(function(){
	var submenu = $('#category-menu>li>div');
	var menu = $('#category-menu');
	var h_menu = $('#category-menu').height();
	var h_submenu = $('#category-menu li div').height();
	$('#category-menu').change(function(){
		 h_menu = $('#category-menu').height();
	});
	$('#category-menu').hover(function()
	{
			h_menu = $('#category-menu').height();
			$('#category-menu>li>div').css('min-height',h_menu-10);
	})
	if( h_submenu < h_menu){
		$('#category-menu>li>div').css('min-height',h_menu-10);
		//$('#category-menu li div').css('min-height',459);
	}
})