﻿using NatuurlikBase.Models;

namespace NatuurlikBase.Repository.IRepository
{
    public interface IViewProductsByName
    {
        Task<List<Product>> ExecuteAsync(string name = "");
    }
}