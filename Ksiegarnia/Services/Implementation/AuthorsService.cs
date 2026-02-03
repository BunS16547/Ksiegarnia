using Ksiegarnia.ModelMappers;
using Ksiegarnia.Models;
using Ksiegarnia.Repositories;
using Ksiegarnia.ViewModels.Authors;

namespace Ksiegarnia.Services.Implementation;

public class AuthorsService : EfModelService<AuthorModel, AuthorViewModel>, IAuthorsService {
    private readonly IAuthorsRepository _authorsRepository;

    public AuthorsService(
        IAuthorsRepository repository,
        IModelMapper<AuthorModel, AuthorViewModel> mapper) : base(repository, mapper) {
        _authorsRepository = repository;
    }

    public async Task<bool> HasUniqueNameAsync(string authorName, int? currentId) {
        var foundAuthor = await _authorsRepository.GetByNameAsync(authorName);
        return foundAuthor == null || foundAuthor.Id == currentId;
    }
}