﻿using ApiEcommerce.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Services.Interfaces
{
    public interface ICategoryService
    {       
        Task<IEnumerable<Category>> GetCategories();
        Category GetById(int id);
        int Count();
        void Create(Category category);
        void Update(Category category);
        void Delete(int id);

    }
}
