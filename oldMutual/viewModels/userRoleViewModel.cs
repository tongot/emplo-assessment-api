using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace oldMutual.viewModels
{
    public class userRoleViewModel
    {
        public string userId { get; set; }
        public List<viewModelRoles> userRoles { get; set; }
    }
}