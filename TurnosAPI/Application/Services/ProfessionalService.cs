using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Services
{
    public class ProfessionalService
    {
        private readonly IGenericRepository<Professional> _repo;

        public ProfessionalService(IGenericRepository<Professional> repo)
        {
            _repo = repo;
        }

        public async Task<IReadOnlyList<Professional>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Professional?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }


        public async Task<Professional> CreateAsync(Professional professional)
        {
            if (string.IsNullOrWhiteSpace(professional.FullName))
                throw new Exception("FullName is required.");

            if (string.IsNullOrWhiteSpace(professional.Specialty))
                throw new Exception("Specialty is required.");

            professional.IsActive = true;
            professional.CreatedAt = DateTime.UtcNow;

            await _repo.AddAsync(professional);
            await _repo.SaveChangesAsync();
            return professional;
        }

        public async Task<Professional> UpdateAsync(int id, Professional updated)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                throw new Exception("PROFESSIONAL_NOT_FOUND");

            existing.FullName = updated.FullName;
            existing.Specialty = updated.Specialty;
            existing.Email = updated.Email;
            existing.Phone = updated.Phone;
            existing.IsActive = updated.IsActive;

            _repo.Update(existing);
            await _repo.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;

            // Soft delete
            existing.IsActive = false;

            _repo.Update(existing);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
