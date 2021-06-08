using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Models
{
    public class Server
    {
        public ulong ServerId { get; set; }
        public string ServerPrefix { get; set; }
    
    }
}
