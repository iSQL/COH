using Ardalis.Result;
using Ardalis.SharedKernel;

namespace COH.UseCases.Contributors.Update;

public record UpdateContributorCommand(int ContributorId, string NewName) : ICommand<Result<ContributorDTO>>;
