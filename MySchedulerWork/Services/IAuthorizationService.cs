using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MySchedulerWork.Models;

namespace MySchedulerWork.Services
{
    public interface IAuthorizationService
    {
        public List<User> GetUsers();
        public void AddUser(User user);
        public string GetToken(string username, string password);
        public ClaimsIdentity GetIdentity(string username, string password);
    }
}
