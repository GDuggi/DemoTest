using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public class InboundDocUserFlagDto
    {
        public Int32 Id { get; set; }
        public Int32 InboundDocId { get; set; }
        public string InboundUser { get; set; }
        public string FlagType { get; set; }
        public string Comments { get; set; }

        //Not in table but passed as param to p_update_user_flag stored proc
        public string UpdateDeleteInd { get; set; }
    }
}
