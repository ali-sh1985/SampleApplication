using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SampleApplication.Domain.Entities;
using SampleApplication.Domain.Repositories;

namespace SampleApplication.Data.EntityFramework.Repositories
{
    internal class ItemRepository : Repository<Item>, IItemRepository
    {
        internal ItemRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
