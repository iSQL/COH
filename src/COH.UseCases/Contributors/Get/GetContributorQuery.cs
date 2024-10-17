using Ardalis.Result;
using Ardalis.SharedKernel;

namespace COH.UseCases.Contributors.Get;

public record GetContributorQuery(int ContributorId) : IQuery<Result<ContributorDTO>>;
