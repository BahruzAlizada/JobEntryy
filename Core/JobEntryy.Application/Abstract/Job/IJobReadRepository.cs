﻿using JobEntryy.Application.Repositories;
using JobEntryy.Domain.Entities;

namespace JobEntryy.Application.Abstract
{
    public interface IJobReadRepository : IReadRepository<Job>
    {
        Task<int> CompanyJobCountAsync(int userId);
    }
}
