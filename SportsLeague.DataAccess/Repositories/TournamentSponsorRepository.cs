using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsLeague.DataAccess.Repositories
{
    public class TournamentSponsorRepository : GenericRepository<TournamentSponsor>, ITournamentSponsorRepository
    {

        public TournamentSponsorRepository(LeagueDbContext context) : base(context)
        {
            
        }

        public async Task<IEnumerable<TournamentSponsor>> GetByTournamentIdAsync(int tournamentId)
        {
            return await _context.TournamentSponsors
                .Include(ts => ts.Tournament)
                .Include(ts => ts.Sponsor)
                .Where(ts => ts.TournamentId == tournamentId)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int tournamentId, int sponsorId)
        {
            return await _context.TournamentSponsors
                .AnyAsync(ts => ts.TournamentId == tournamentId && ts.SponsorId == sponsorId);

        }
    }

}
