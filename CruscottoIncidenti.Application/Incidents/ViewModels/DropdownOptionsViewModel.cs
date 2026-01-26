using System.Collections;
using System.Collections.Generic;

namespace CruscottoIncidenti.Application.Incidents.ViewModels
{
    public class DropdownOptionsViewModel
    {
        public ICollection Threats { get; set; } = new Dictionary<int, string>();

        public ICollection Scenarios { get; set; } = new Dictionary<int, string>();

        public ICollection Origins { get; set; } = new Dictionary<int, string>();
    }
}
