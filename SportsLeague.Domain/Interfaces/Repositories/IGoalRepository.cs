using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories;

public interface IGoalRepository : IGenericRepository<Goal>
{
    Task<IEnumerable<Goal>> GetByMatchAsync(int matchId);
    Task<IEnumerable<Goal>> GetByMatchWithDetailsAsync(int matchId);
}

