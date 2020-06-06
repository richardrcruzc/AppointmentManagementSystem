using AmsApi.Data;
using AmsApi.Domain;
using AmsApi.Infraestructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmsApi.Repository
{
    public class ServiceCategoryRepository : Repository<CategoryModel>
    {
         
        public ServiceCategoryRepository(AmsApiDbContext context) : base(context) { }

        public Task<CategoryModel> GetByDescription(string description)
        {
            return this.GetByDescription(description);
        }
    }
}
