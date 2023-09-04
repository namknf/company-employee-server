﻿using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class AddressRepository : RepositoryBase<Address>, IAddressRepository
    {
        public AddressRepository(DataContext dataContext) : base(dataContext)
        {
        }

        public void AnyMethodFromAddressRepository()
        {
            throw new NotImplementedException();
        }
    }
}
