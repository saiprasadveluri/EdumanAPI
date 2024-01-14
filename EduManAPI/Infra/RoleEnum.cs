using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduManAPI.Infra
{
    public enum RoleEnum
    {
        Student=1,
        Teacher=2,
        OrgAdmin=3,
        SiteAdmin=4,
        Cashier=5
    }

    public enum ChalanStatusEnum
    {
        Active=1,
        Inactive=2,
        Paid=3
    }
}
