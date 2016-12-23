using System;
using WebTask.Controllers.Access;
using WebTask.DataAccess;

namespace WebTask.Controllers.Format
{
    public class UserInfoTextFormat
    {
        public string FormatUserCount(UsersCount usersCount, string lastVisit = "")
        {
            return $"День:{usersCount.Day}, Все:{usersCount.All}. {lastVisit}";
        }

        public string FormatBrowserInfo(string ip, int screenWidth, int screenHeight, string browser, int version)
        {
            return $"Ip: {ip}, Браузер: {browser}.{version}, Разрешение: {screenWidth}x{screenHeight}";
        }

        public string FormatLastVisit(UserSession session)
        {
            if (session == null)
                return "Первый визит страницы";
            var time = session.Modify;
            return $"Были тут {time.TimeAgo()}";
        }

    }
}