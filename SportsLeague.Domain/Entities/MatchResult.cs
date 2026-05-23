using SportsLeague.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MatchResult : AuditBase
{
    public int MatchId { get; set; }
    public int HomeGoals { get; set; }
    public int AwayGoals { get; set; }
    public string? Observations { get; set; }

    // Navigation Property
    public Match Match { get; set; } = null!;


}
