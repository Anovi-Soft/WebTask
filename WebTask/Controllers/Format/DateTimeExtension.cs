using System;

namespace WebTask.Controllers.Format
{
    public static class DateTimeExtension
    {
        public static string TimeAgo(this DateTime time)
        {
            var shift = DateTime.Now.Subtract(time);
            if (shift.Days > 0)
                return $"{shift.Days.FormatCount("день", "дня", "дней")} назад";
            if (shift.Hours > 0)
                return $"{shift.Hours.FormatCount("час", "часа", "часов")} назад";
            if (shift.Minutes > 0)
                return $"{shift.Minutes.FormatCount("минуту", "минуты", "минут")} назад";
            if (shift.Seconds > 5)
                return $"{shift.Seconds.FormatCount("секунду", "секунды", "секунд")} назад";
            return "только-что";
        }

        private static string FormatCount(this int number, string one, string many, string more)
        {
            if (number == 1)
                return $"{one}";
            switch (number)
            {
                case 1:
                    return $"{number} {one}";
                case 2:
                case 3:
                case 4:
                    return $"{number} {many}";
            }
            return $"{number} {more}";
        }
    }
}