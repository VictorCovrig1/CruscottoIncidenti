using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using CruscottoIncidenti.Application.Interfaces;
using MediatR;

namespace CruscottoIncidenti.Application.User.Commands.DeleteUser
{
    public class DeleteUserCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly ICruscottoIncidentiDbContext _context;

        public DeleteUserHandler(ICruscottoIncidentiDbContext context)
            => _context = context;

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == request.Id);

            if (user != null)
            {
                user.Roles.Clear();
                _context.Users.Remove(user);
                return await _context.SaveChangesAsync(cancellationToken) > 0;
            }
            else
                return false;
        }
    }
}
