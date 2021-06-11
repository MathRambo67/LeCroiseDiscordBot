using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Services
{
    public class UserService
    {
        private readonly InfrastructureContext _context;

        public UserService(InfrastructureContext context)
        {
            _context = context;
        }
    }
}
