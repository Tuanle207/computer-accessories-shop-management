﻿using SE214L22.Data.Entity.AppProduct;
using System.Collections.Generic;

namespace SE214L22.Data.Interfaces.Repositories
{
    public interface IOrderProductRepository : IBaseRepository<OrderProduct>
    {
        void DeleteAllByOrderId(int orderId);
        IEnumerable<OrderProduct> GetOrderProductsByOrderId(int orderId);
    }
}