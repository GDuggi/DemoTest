using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSRiskManager
{
    public class RiskManagerWindowState
    {
        public int win_id { get; set; }
        public int lower_split_pos { get; set; }
        public int port_split_pos { get; set; }
        public int pnl_split_pos { get; set; }
        public string window_frame { get; set; }
        public string display_Resolution { get; set; }
        public string open_windows { get; set; }
    }
}
