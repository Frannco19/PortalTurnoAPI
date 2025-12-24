using Application.Interfaces.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService
    {
        private readonly IGenericRepository<User> _repo;

        public UserService(IGenericRepository<User> repo)
        {
            _repo = repo;
        }

        public async Task<IReadOnlyList<User>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }


        public async Task<User> CreateAsync(User user)
        {
            if (string.IsNullOrWhiteSpace(user.FullName))
                throw new Exception("FullName is required.");

            if (string.IsNullOrWhiteSpace(user.Email))
                throw new Exception("Email is required.");

            if (string.IsNullOrWhiteSpace(user.PasswordHash))
                throw new Exception("PasswordHash is required.");

            user.IsActive = true;
            user.CreatedAt = DateTime.UtcNow;

            await _repo.AddAsync(user);
            await _repo.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(int id, User updated)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                throw new Exception("USER_NOT_FOUND");

            existing.FullName = updated.FullName;
            existing.Email = updated.Email;
            existing.Role = updated.Role;
            existing.IsActive = updated.IsActive;

            // Password: lo ideal es endpoint aparte, pero para modo clase:
            if (!string.IsNullOrWhiteSpace(updated.PasswordHash))
                existing.PasswordHash = updated.PasswordHash;

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
