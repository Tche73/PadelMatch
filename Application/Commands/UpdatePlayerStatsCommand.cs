using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands
{
    public class UpdatePlayerStatsCommand
    {
        public int UserId { get; set; }
        public bool IsWin { get; set; }
        public DateTime? MatchDate { get; set; }
    }

}
