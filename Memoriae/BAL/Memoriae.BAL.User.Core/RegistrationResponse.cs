using System;
using System.Collections.Generic;
using System.Text;

namespace Memoriae.BAL.User.Core
{
    public class RegistrationResponse
    {
        public bool Success { get; set; }

        public IEnumerable<string> Errors { get; set; }
    }
}
