using Ardalis.Result;
using Ardalis.SharedKernel;

namespace COH.UseCases.Contributors.Delete;

public record DeleteContributorCommand(int ContributorId) : ICommand<Result>;
