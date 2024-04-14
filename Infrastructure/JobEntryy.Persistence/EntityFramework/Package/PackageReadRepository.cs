﻿using JobEntryy.Application.Abstract;
using JobEntryy.Domain.Entities;
using JobEntryy.Persistence.Repositories;

namespace JobEntryy.Persistence.EntityFramework
{
    public class PackageReadRepository : ReadRepository<Package>,IPackageReadRepository
    {
    }
}
