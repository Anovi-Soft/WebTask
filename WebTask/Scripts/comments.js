function sendComment(sender) {
    var photoId = current;
    var textBox = $("#comments_text_input-" + photoId);
    var text = textBox.val();
    textBox.val("");
    var commentBlock = createCommentBlock(photoId, userName, text, true);
    sender.disabled = true;
    $.post("/Photo/SendComment", { photoId, userName, text })
      .done(function (model) {
          commentBlock.attr("id", model.CommentId);
          commentsState[photoId] = model.NewState;
        })
      .fail(function(data) {
          commentBlock.remove();
          alert("Something wrong: \r\n" + data);
      })
      .always(function(data) {
            sender.disabled = false;
      });
}

function createCommentBlock(imageId, name, text, isMy, id) {
    if (id) {
        if ($("#" + id).length)
            throw "Объект уже существует";
    } else {
        id = uniqId();
    }
    var button = isMy ? buttonRemovePattern : "";
    var source = commentPattern.formatPattern(id, name, text, button);
    $("#comments_box-" + imageId).append(source);
    return $("#" + id);
}

function uniqId() {
    return Math.round(new Date().getTime() + (Math.random() * 100));
}

function updateComments() {
    var data = { photoId: current, state: commentsState[current] };
    $.getJSON("/Photo/UpdateComments", data)
    .done(function(changes) {
            $.each(changes, updateCommentsInternal);
        });
}

function removeComment(sender) {
    var photoId = current;
    var commentId = $(sender).parent().attr("id");
    $.post("/Photo/RemoveComment", { photoId, commentId })
      .done(function (newState) {
          $("#" + commentId).remove();
            commentsState[photoId] = newState;
        });
}

function updateCommentsInternal(i, change) {
    commentsState[change.PhotoId] = change.Id;
    var comment = change.Comment;
    if (change.Status == 0) {
        var isMy = comment.AuthorId === userId;
        createCommentBlock(change.PhotoId, comment.Name, comment.Text, isMy, comment.Id);
    } else {
        $("#"+ comment.Id).remove();
    }
        
}
document.addEventListener("DOMContentLoaded", function() {
    setInterval(updateComments, 10000);
});
