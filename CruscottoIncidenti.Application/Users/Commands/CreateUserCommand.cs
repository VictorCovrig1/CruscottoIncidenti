using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CruscottoIncidenti.Application.Interfaces;
using MediatR;
using System.Data.Entity;
using CruscottoIncidenti.Application.Common.Exceptions;
using System.Collections.Generic;
using CruscottoIncidenti.Domain.Entities;

namespace CruscottoIncidenti.Application.User.Commands
{
    public class CreateUserCommand : IRequest<bool>
    {
        public int CreatorId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public List<int> Roles { get; set; }
    }

    public class CreateUserHandler : IRequestHandler<CreateUserCommand, bool>
    {
        private readonly ICruscottoIncidentiDbContext _context;

        public CreateUserHandler(ICruscottoIncidentiDbContext context)
            => _context = context;

        public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var dublicatedEmailUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower());
            
            if (dublicatedEmailUser != null)
                throw new CustomException
                    ($"User with the same email ({dublicatedEmailUser.Email}) already exists");

            var dublicatedUsernameUser = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName.ToLower() == request.Username.ToLower());

            if (dublicatedUsernameUser != null)
                throw new CustomException
                    ($"User with the same username ({dublicatedUsernameUser.UserName}) already exists");

            string encrypted = string.Empty;

            if (request.Password != request.ConfirmPassword)
                throw new CustomException("Password and Confirm Password doesn't match");

            using (SHA256 hash = SHA256.Create())
            {
                encrypted = string.Concat(hash
                    .ComputeHash(Encoding.UTF8.GetBytes(request.Password))
                    .Select(item => item.ToString("x2")));
            }

            var user = new Domain.Entities.User
            {
                CreatedBy = request.CreatorId,
                Created = DateTime.UtcNow,
                UserName = request.Username,
                Password = encrypted,
                Email = request.Email,
                FullName = request.FullName
            };

            foreach (var roleId in request.Roles)
            {
                user.UserRoles.Add(new UserToRole()
                {
                    RoleId = roleId,
                    UserId = user.Id
                });
            }

            _context.Users.Add(user);

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
