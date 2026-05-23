using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsLeague.Domain.Entities;


public class Referee : AuditBase

{

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Nationality { get; set; } = string.Empty;
    // Agregar dentro de la clase Referee: 



    public ICollection<Match> Matches { get; set; } = new List<Match>();

}