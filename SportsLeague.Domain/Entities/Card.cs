using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Card : AuditBase
{
    public int MatchId { get; set; }
    public int PlayerId { get; set; }
    public int Minute { get; set; }
    public CardType Type { get; set; }

    // Navigation Properties
    public Match Match { get; set; } = null!;
    public Player Player { get; set; } = null!;
}
