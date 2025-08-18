using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using upmData.Models;

namespace upmDomain.UserTools
{
    internal class UserMapper
    {
        public static UserDto Map(upmData.Models.User user)
        {
            return new UserDto
            {
                Active = user.Active,
                CodeUser = user.CodeUser,
                CreateBy = user.CreateBy,
                UserEmail = user.Email,
                UserId = user.Id,
                UserName = user.UserName
            };
        }
    }
}
