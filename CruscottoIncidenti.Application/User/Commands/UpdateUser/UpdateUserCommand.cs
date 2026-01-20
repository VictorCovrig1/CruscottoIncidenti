using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CruscottoIncidenti.Application.Common.Exceptions;
using CruscottoIncidenti.Application.Interfaces;
using MediatR;

namespace CruscottoIncidenti.Application.User.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<bool>
    {
        public int EditorId { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public List<int> Roles { get; set; }

        public bool IsEnabled { get; set; }
    }

    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, bool>
    {
        private readonly ICruscottoIncidentiDbContext _context;

        public UpdateUserHandler(ICruscottoIncidentiDbContext context)
            => _context = context;

        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);

            var dublicatedEmailUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower() && user.Id != request.UserId);

            if (dublicatedEmailUser != null)
                throw new DublicatedEntityException
                    ($"Another user with the same email ({dublicatedEmailUser.Email}) already exists");

            var dublicatedUsernameUser = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName.ToLower() == request.Username.ToLower() && user.Id != request.UserId);

            if (dublicatedUsernameUser != null)
                throw new DublicatedEntityException
                    ($"Another user with the same username ({dublicatedUsernameUser.UserName}) already exists");

            string encrypted = string.Empty;

            user.LastModifiedBy = request.EditorId;
            user.LastModified = DateTime.UtcNow;
            user.UserName = request.Username;
            user.Email = request.Email;
            user.FullName = request.FullName;
            user.Roles = await _context.Roles.Where(x => request.Roles.Contains(x.Id)).ToListAsync(cancellationToken);

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
