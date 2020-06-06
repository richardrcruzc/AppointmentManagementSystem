using AmsApi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmsApi.Repository
{
    public interface IServiceCategoryRepository: IRepository<ServiceCategory>
    {
        Task<ServiceCategory> GetByDescription(string description);
    }
}
