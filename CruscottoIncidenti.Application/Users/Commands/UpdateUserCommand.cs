using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CruscottoIncidenti.Application.Common.Exceptions;
using CruscottoIncidenti.Application.Interfaces;
using CruscottoIncidenti.Application.Users.ViewModels;
using CruscottoIncidenti.Domain.Entities;
using MediatR;

namespace CruscottoIncidenti.Application.User.Commands
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserViewModel, Unit>
    {
        private readonly ICruscottoIncidentiDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public UpdateUserHandler(ICruscottoIncidentiDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(UpdateUserViewModel request, CancellationToken cancellationToken)
        {
            var dublicatedEmailUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower() && u.Id != request.Id);

            if (dublicatedEmailUser != null)
                throw new CustomException
                    ($"Another user with the same email ({dublicatedEmailUser.Email}) already exists");

            var user = await _context.Users.Include(u => u.UserRoles).FirstOrDefaultAsync(u => u.Id == request.Id);

            if (user == null)
                throw new CustomException($"User ({request.Id}) not found");

            user.LastModifiedBy = _currentUserService.UserId;
            user.LastModified = DateTime.UtcNow;
            user.Email = request.Email;
            user.FullName = request.FullName;
            user.IsEnabled = request.IsEnabled;

            user.UserRoles.Clear();

            var roles = _context.Roles.Where(x => request.Roles.Contains(x.Id)).ToList();

            foreach (var role in roles)
            {
                user.UserRoles.Add(new UserToRole
                {
                    UserId = user.Id,
                    RoleId = role.Id
                });
            } 

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
