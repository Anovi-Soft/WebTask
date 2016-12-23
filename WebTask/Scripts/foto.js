var constant = "#pho";
var current = 0;

if ("onhelp" in window) {
    window.onhelp = function () {
        $("#myModalBox").modal('show'); 
        return false;
    }
}

addEventListener("keydown", function(event) {
    if (event.keyCode == 112){
		window.event.returnValue = false;
        $("#myModalBox").modal('show');
    }
    if (event.keyCode == 39){
        prev_or_next('next');
    }
    if (event.keyCode == 37){
        prev_or_next('prev');
    }
    if (event.keyCode == 27){
        $(constant+current).modal('hide');
    }
});

function prev_or_next(direct){
    id = constant + current;
    $(id).modal('hide');
    if (direct == 'next'){
        current++;
        if (current == 29){
            current = 11;
        }
    }
    if (direct == 'prev'){
        current--;
        if (current == 10){
            current = 28;
        }
    }
    window.location.href = "#" + current;
    id = constant + current;
    $(id).modal('show');
    $(id).on('hide.bs.modal',function(){
        window.location.href = "#";
    });
}
 
function getImg(num){
    window.location.href = "#" + num;
    current = num;
    id = constant + num;
    $('#loader').show();
    $(id).modal("show");
    $('.image').load(function(){
        $(this).siblings('.loading').hide();
    });
    $(id).on('hide.bs.modal',function(){
        window.location.href = "#";
    });
}

function init() {
    window.onhashchange = locationUpdate;
    window.onundo = locationUpdate;
    locationUpdate();
}

function locationUpdate() {
    var splt = window.location.toString().split("#");
    if (splt.length > 1 && splt[1].length > 0) {
        getImg(parseInt(splt[1]));
    }else {
        window.location = "#";
    }
}

// $( "picu" ).on( "click", function() {
    // var newBgSrc = "../img/" + current + ".jpg";
  // $( this ).css( "background", "url(" + newBgSrc + ")" );
// });

function changeBg() {
    var newBgSrc = "../img/" + current + ".jpg";
    $("body").css("background", "url(" + newBgSrc + ")");
}