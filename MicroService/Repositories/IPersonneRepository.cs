using MicroService.Models;

namespace MicroService.Repositories
{
    public interface IPersonneRepository
    {

        public Task<IEnumerable<PersonneModel>> GetItemsAsync();
        public Task<IEnumerable<PersonneModel>> GetItemsAsync(string prenom, string nom);
        public Task<PersonneModel> GetItemAsync(Guid id);
        public Task UpdateItemAsync(PersonneModel item);
        public Task DeleteItemAsync(Guid id);
        public Task <PersonneModel> AddItemAsync(PersonneModel item);




    }
}
