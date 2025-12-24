using Application.Interfaces.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ClientService
    {
        private readonly IGenericRepository<Client> _repo;

        public ClientService(IGenericRepository<Client> repo)
        {
            _repo = repo;
        }

        public async Task<IReadOnlyList<Client>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Client?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }


        public async Task<Client> CreateAsync(Client client)
        {
            if (string.IsNullOrWhiteSpace(client.FullName))
                throw new Exception("FullName is required.");

            if (string.IsNullOrWhiteSpace(client.Phone))
                throw new Exception("Phone is required.");

            client.IsActive = true;
            client.CreatedAt = DateTime.UtcNow;

            await _repo.AddAsync(client);
            await _repo.SaveChangesAsync();
            return client;
        }

        public async Task<Client> UpdateAsync(int id, Client updated)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                throw new Exception("CLIENT_NOT_FOUND");

            existing.FullName = updated.FullName;
            existing.Phone = updated.Phone;
            existing.Email = updated.Email;
            existing.Notes = updated.Notes;
            existing.IsActive = updated.IsActive;

            _repo.Update(existing);
            await _repo.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;

            existing.IsActive = false;

            _repo.Update(existing);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
