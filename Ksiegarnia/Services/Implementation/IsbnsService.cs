using Ksiegarnia.ModelMappers;
using Ksiegarnia.Models;
using Ksiegarnia.Repositories;
using Ksiegarnia.ViewModels.Isbns;

namespace Ksiegarnia.Services.Implementation;

public class IsbnsService : EfModelService<IsbnModel, IsbnViewModel>, IIsbnsService {

    private readonly IIsbnsRepository _isbnsRepository;

    public IsbnsService(
        IIsbnsRepository repository,
        IModelMapper<IsbnModel, IsbnViewModel> mapper) : base(repository, mapper) {
        _isbnsRepository = repository;
    }


    public async Task<bool> HasUniqueValueAsync(string isbnValue, int? currentId) {
        var foundIsbn = await _isbnsRepository.GetByValueAsync(isbnValue);
        
        return foundIsbn == null || foundIsbn.Id == currentId;
    }
}