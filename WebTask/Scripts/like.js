function changeLike(senders) {
    var button = $(senders);
    senders.disabled = true;
    var photoId = current;
    var value = button.val();
    var splt = value.split("-");
    var isSelected = splt[0].startsWith("Like");
    var count = parseInt(splt[1]);
    count += isSelected ? +1 : -1;
    setLikeState(button, count, isSelected);
    $.post("/Photo/ChangeLike", { photoId, isSelected })
        .done(updateLikeInternal)
        .always(function () { senders.disabled=false; });
}

function setLikeState(button, count, isSelected) {
    var prefix = isSelected ? "Dislike" : "Like";
    var text = "{0}-{1}".formatPattern(prefix, count);
    button.val(text);
}

function updateLike() {
    var photoId = current;
    $.getJSON("/Photo/GetLike", { photoId })
        .done(updateLikeInternal);
}

function updateLikeInternal(model) {
    var button = $("#like_button_send-" + model.PhotoId);
    setLikeState(button, model.Count, model.IsMy);
}

document.addEventListener("DOMContentLoaded", function () {
    setInterval(updateLike, 10000);
});
