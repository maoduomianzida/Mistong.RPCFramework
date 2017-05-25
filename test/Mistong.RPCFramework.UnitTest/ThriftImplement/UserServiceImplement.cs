using Mistong.Services.UserService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift;

namespace Mistong.RPCFramework.UnitTest
{
    public class UserServiceImplement : UserService.Iface
    {
        private List<UserInfo> _userList;

        public UserServiceImplement()
        {
            _userList = new List<UserInfo>();
        }

        public bool Add(UserInfo user)
        {
            if(user == null) throw new ArgumentNullException(nameof(user));
            if(_userList.Exists(tmp => tmp.UserID == user.UserID))
            {
                throw new TException("已经存在了相同ID的用户");
            }

            _userList.Add(user);

            return true;
        }

        public List<UserInfo> GetAll()
        {
            return _userList;
        }

        public UserInfo GetUser(int UserID)
        {
            return _userList.SingleOrDefault(tmp => tmp.UserID == UserID);
        }
    }
}