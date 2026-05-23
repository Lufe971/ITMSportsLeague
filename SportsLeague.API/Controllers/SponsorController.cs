using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SponsorController : ControllerBase
    {
        private readonly ISponsorService _service;
        private readonly IMapper _mapper;

        public SponsorController(ISponsorService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // ✅ GET ALL
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sponsors = await _service.GetAllAsync();
            var result = _mapper.Map<IEnumerable<SponsorResponseDTO>>(sponsors);
            return Ok(result);
        }

        // ✅ GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var sponsor = await _service.GetByIdAsync(id);
            if (sponsor == null) return NotFound();

            return Ok(_mapper.Map<SponsorResponseDTO>(sponsor));
        }

        // ✅ POST → 🔥 201 CREATED
        [HttpPost]
        public async Task<IActionResult> Create(SponsorRequestDTO dto)
        {
            try
            {
                var sponsor = _mapper.Map<Sponsor>(dto);
                var created = await _service.CreateAsync(sponsor);

                var response = _mapper.Map<SponsorResponseDTO>(created);

                return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message); // 409
            }
        }

        // ✅ PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SponsorRequestDTO dto)
        {
            try
            {
                var sponsor = _mapper.Map<Sponsor>(dto);
                await _service.UpdateAsync(id, sponsor);

                return NoContent(); // 204
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // ✅ DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // 🔥 POST vincular
        [HttpPost("{id}/tournaments")]
        public async Task<IActionResult> Link(int id, TournamentSponsorRequestDTO dto)
        {
            try
            {
                await _service.LinkToTournamentAsync(id, dto.TournamentId, dto.ContractAmount);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        // 🔥 GET torneos del sponsor
        [HttpGet("{id}/tournaments")]
        public async Task<IActionResult> GetTournaments(int id)
        {
            var data = await _service.GetTournamentsBySponsorAsync(id);
            var result = _mapper.Map<IEnumerable<TournamentSponsorResponseDTO>>(data);
            return Ok(result);
        }

        // DELETE desvincular
        [HttpDelete("{id}/tournaments/{tid}")]
        public async Task<IActionResult> Unlink(int id, int tid)
        {
            try
            {
                await _service.UnlinkAsync(id, tid);
                return NoContent();
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
