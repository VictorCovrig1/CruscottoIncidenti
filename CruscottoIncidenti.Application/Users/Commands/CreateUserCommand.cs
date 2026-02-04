using System;
using System.Threading;
using System.Threading.Tasks;
using CruscottoIncidenti.Application.Interfaces;
using MediatR;
using System.Data.Entity;
using CruscottoIncidenti.Application.Common.Exceptions;
using CruscottoIncidenti.Domain.Entities;
using CruscottoIncidenti.Application.Users.ViewModels;
using CruscottoIncidenti.Application.Common.Utils;

namespace CruscottoIncidenti.Application.User.Commands
{

    public class CreateUserHandler : IRequestHandler<CreateUserViewModel,  Unit>
    {
        private readonly ICruscottoIncidentiDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public CreateUserHandler(ICruscottoIncidentiDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(CreateUserViewModel request, CancellationToken cancellationToken)
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

            var user = new Domain.Entities.User
            {
                CreatedBy = _currentUserService.UserId,
                Created = DateTime.UtcNow,
                UserName = request.Username,
                Password = PasswordHelper.EncryptPassword(request.Password),
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
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
