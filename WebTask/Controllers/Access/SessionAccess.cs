using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WebTask.DataAccess;

namespace WebTask.Controllers.Access
{
    public class SessionAccess
    {
        public void AddSession(Guid? id, string ip, string path)
        {
            using (var context = new UserSessionDbContext())
            {
                var session = new UserSession
                {
                    UserIp = ip,
                    UserId = id,
                    Path = path
                };
                context.UserSessions.Add(session);
                context.SaveChanges();
            }
        }

        public UserSession GetLastVisit(Guid id, string path)
        {
            using (var context = new UserSessionDbContext())
            {
                var now = DateTime.Now.AddSeconds(-5);
                return context.UserSessions
                    .Where(x => x.UserId == id && x.Path == path && x.Modify < now)
                    .OrderByDescending(x => x.Modify)
                    .FirstOrDefault();
            }            
        }

        public UsersCount GetUsersCount()
        {
            return new UsersCount
            {
                Day = CountUsers(DateTime.Now.AddDays(-1), DateTime.Now),
                All = CountUsers(DateTime.MinValue, DateTime.Now),
            };
        }

        private int CountUsers(DateTime from, DateTime to)
        {
            using (var context = new UserSessionDbContext())
            {
                var inRange = context.UserSessions
                    .Where(x => @from < x.Modify && x.Modify < to);
                var group = inRange.ToLookup(x => x.UserId);
                var byId = group.Count;
                if (group[null].Any())
                    byId--;
                var byIp = group[null].GroupBy(x=>x.UserIp).Sum(UniqueFlter);
                return byId + byIp;
            }
        }

        private static int UniqueFlter(IEnumerable<UserSession> userSessions)
        {
            var count = 1;
            userSessions = userSessions.OrderBy(x => x.Modify);
            var previous = userSessions.First();
            foreach (var current in userSessions.Skip(1))
            {
                if (previous.Modify.AddHours(1) < current.Modify)
                    count++;
                previous = current;
            }
            return count;
        }

        public async Task<List<UserSession>> GetUserSessions(string userId)
        {
            var id = Guid.Parse(userId);
            using (var context = new UserSessionDbContext())
            {
                return await context.UserSessions
                    .Where(x => x.UserId == id)
                    .Where(x => !x.Path.Contains("?"))
                    .OrderByDescending(x=>x.Modify)
                    .Take(10)
                    .ToListAsync();
            }
        }
    }

    public class UsersCount
    {
        public int Day { get; set; }
        public int All { get; set; }
    }
}