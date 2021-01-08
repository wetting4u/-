//畫面載入後事件
$(function () {
    //綁定新增文章按鈕點擊事件
    //因為新增文章按鈕是後來用AJAX取回產生，故採用$(document).delegate
    $(document).delegate('#EditArticleModal #editBtn', 'click', function () {
        $('#EditArticleModal form').submit();
    });

    //當按下新增留言0.2秒後重整頁面
    $("#createMessage").click(function () {
        setTimeout(function () { location.reload(); }, 200);
    });
});