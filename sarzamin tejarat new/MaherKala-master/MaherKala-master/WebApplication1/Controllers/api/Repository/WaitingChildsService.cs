using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Controllers.api.Repository
{
    public class WaitingChildsService
    {
        DBContext _context = new DBContext();
       
        public WaitingChildsService(DBContext context)
        {
            this._context = context;
        }
        public async Task<IEnumerable<MarketerUser>> GetChildUser(int Id)
        {
            return await _context.MarketerUsers.Where(p=>p.Parent_Id == Id).ToListAsync();
        }
    }
}