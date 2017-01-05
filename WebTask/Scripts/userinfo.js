function updateImageUserInfo() {
    var url = 'UserInfo/UsersInfo?width={0}&height={1}';
    var w = window,
        d = document,
        e = d.documentElement,
        g = d.getElementsByTagName('body')[0],
        x = w.innerWidth || e.clientWidth || g.clientWidth,
        y = w.innerHeight || e.clientHeight || g.clientHeight;
    url = url.formatPattern(x, y);
    var img = $("<img />").attr('src', url)
    .on('load', function () {
        if (!this.complete || typeof this.naturalWidth == "undefined" || this.naturalWidth == 0) {
            alert('broken image!');
        } else {
            $("#user_info_container").append(img);
        }
    });
}
document.addEventListener("DOMContentLoaded", updateImageUserInfo);
