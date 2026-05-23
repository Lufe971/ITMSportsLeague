using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace SportsLeague.Domain.Services
{
    public class SponsorService : ISponsorService
    {
        private readonly ISponsorRepository _sponsorRepo;
        private readonly ITournamentSponsorRepository _tsRepo;
        private readonly IGenericRepository<Tournament> _tournamentRepo;

        public SponsorService(
            ISponsorRepository sponsorRepo,
            ITournamentSponsorRepository tsRepo,
            IGenericRepository<Tournament> tournamentRepo)
        {
            _sponsorRepo = sponsorRepo;
            _tsRepo = tsRepo;
            _tournamentRepo = tournamentRepo;
        }

        public async Task<Sponsor> CreateAsync(Sponsor sponsor)
        {
            if (await _sponsorRepo.ExistsByNameAsync(sponsor.Name))
                throw new InvalidOperationException("Duplicate name");

            if (!IsValidEmail(sponsor.ContactEmail))
                throw new InvalidOperationException("Invalid email");

            sponsor.CreatedAt = DateTime.UtcNow;

            return await _sponsorRepo.CreateAsync(sponsor);
        }

        public async Task<IEnumerable<Sponsor>> GetAllAsync()
            => await _sponsorRepo.GetAllAsync();

        public async Task<Sponsor?> GetByIdAsync(int id)
            => await _sponsorRepo.GetByIdAsync(id);

        public async Task UpdateAsync(int id, Sponsor sponsor)
        {
            var existing = await _sponsorRepo.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException();

            existing.Name = sponsor.Name;
            existing.ContactEmail = sponsor.ContactEmail;
            existing.Phone = sponsor.Phone;
            existing.WebsiteUrl = sponsor.WebsiteUrl;
            existing.Category = sponsor.Category;
            existing.UpdatedAt = DateTime.UtcNow;

            await _sponsorRepo.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int id)
        {
            var sponsor = await _sponsorRepo.GetByIdAsync(id);
            if (sponsor == null)
                throw new KeyNotFoundException();

            await _sponsorRepo.DeleteAsync(id);
        }

        // RELACIÓN N:M

        public async Task LinkToTournamentAsync(int sponsorId, int tournamentId, decimal contractAmount)
        {
            var sponsor = await _sponsorRepo.GetByIdAsync(sponsorId);
            if (sponsor == null)
                throw new KeyNotFoundException("Sponsor not found");

            var tournament = await _tournamentRepo.GetByIdAsync(tournamentId);
            if (tournament == null)
                throw new KeyNotFoundException("Tournament not found");

            if (await _tsRepo.ExistsAsync(tournamentId, sponsorId))
                throw new InvalidOperationException("Already linked");

            if (contractAmount <= 0)
                throw new InvalidOperationException("Invalid contract amount");

            var ts = new TournamentSponsor
            {
                SponsorId = sponsorId,
                TournamentId = tournamentId,
                ContractAmount = contractAmount,
                JoinedAt = DateTime.UtcNow
            };

            await _tsRepo.CreateAsync(ts);
        }

        public async Task<IEnumerable<TournamentSponsor>> GetTournamentsBySponsorAsync(int sponsorId)
        {
            return await _tsRepo.GetByTournamentIdAsync(sponsorId);
        }

        public async Task UnlinkAsync(int sponsorId, int tournamentId)
        {
            var list = await _tsRepo.GetAllAsync();
            var entity = list.FirstOrDefault(x => x.SponsorId == sponsorId && x.TournamentId == tournamentId);

            if (entity == null)
                throw new KeyNotFoundException();

            await _tsRepo.DeleteAsync(entity.Id);
        }

        private bool IsValidEmail(string email)
        {
            return new System.Net.Mail.MailAddress(email).Address == email;
        }
    }
}
