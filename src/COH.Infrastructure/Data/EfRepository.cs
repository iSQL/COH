﻿using Ardalis.SharedKernel;
using Ardalis.Specification.EntityFrameworkCore;

namespace COH.Infrastructure.Data;

// inherit from Ardalis.Specification type
public class EfRepository<T>(AppDbContext dbContext) :
  RepositoryBase<T>(dbContext), IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
{
}
