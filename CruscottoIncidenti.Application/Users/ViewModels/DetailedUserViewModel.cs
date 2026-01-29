using System.Collections.Generic;

namespace CruscottoIncidenti.Application.User.ViewModels
{
    public class DetailedUserViewModel
    {
        public int Id { get; set; }

        public int? CreatedBy { get; set; }

        public string Created { get; set; }

        public int? LastModifiedBy { get; set; }

        public string LastModified { get; set; }

        public string Username { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public bool IsEnabled { get; set; }

        //public ICollection<RoleViewModel> AllRoles { get; set; }

        public IList<int> Roles { get; set; }

        public bool ShouldBeDeleted { get; set; }
    }
}
