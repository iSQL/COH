using Ardalis.Result;
using Ardalis.SharedKernel;

namespace COH.UseCases.Contributors.List;

public record ListContributorsQuery(int? Skip, int? Take) : IQuery<Result<IEnumerable<ContributorDTO>>>;
