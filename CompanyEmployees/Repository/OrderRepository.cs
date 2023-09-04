﻿using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    internal class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(DataContext dataContext) : base(dataContext)
        {
        }

        public void AnyMethodFromOrderRepository()
        {
            throw new NotImplementedException();
        }
    }
}
