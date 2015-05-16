using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SampleApplication.Domain.Entities;

namespace SampleApplication.Domain.Repositories
{
    public interface IItemRepository:IRepository<Item>
    {
    }
}
