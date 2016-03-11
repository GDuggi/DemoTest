using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public class UserFiltersOpsmgrDto
    {
        public Int32 Id { get; set; }
        public string UserId { get; set; }
        public string Descr { get; set; }
        public string FilterExpr { get; set; }
    }
}
