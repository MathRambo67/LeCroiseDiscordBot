using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class ServerService
    {
        private readonly InfrastructureContext _context;


        public ServerService(InfrastructureContext context)
        {
            _context = context;
        }

        //CRUD 

        public async Task AddOrUpdateGuild(ulong id, string prefix)
        {
            var server = await _context.Servers.FindAsync(id);

            if (server == null)
            {
                _context.Add(new Server { ServerId = id, ServerPrefix = prefix });
            }
            else
            {
                server.ServerPrefix = prefix;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<string> GetGuildPrefix(ulong id)
        {
            var query = await _context.Servers
                        .Where(x => x.ServerId == id)
                        .Select(x => x.ServerPrefix)
                        .FirstOrDefaultAsync();

            return await Task.FromResult(query);
        }

        public async Task RemoveGuild(ulong id)
        {
            var query = await _context.Servers
                        .Where(x => x.ServerId == id)
                        .FirstOrDefaultAsync();

            _context.Servers.Remove(query);

           await _context.SaveChangesAsync();
        }
    }

}
