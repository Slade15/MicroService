using MicroService.DBContexts;
using MicroService.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroService.Repositories
{
    public class PersonneRepository : IPersonneRepository
    {
        private readonly PersonneContext _context;
        public PersonneRepository(PersonneContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<PersonneModel> AddItemAsync(PersonneModel item)
        {
            _context.Personnes.Add(item);
            await _context.SaveChangesAsync();

            return await GetItemAsync(item.Id);

        }

        public async Task<PersonneModel> GetItemAsync(Guid id)
        {
            return await _context.Personnes.FindAsync(id);
        }

        public async Task<IEnumerable<PersonneModel>> GetItemsAsync()
        {
            return await _context.Personnes.ToListAsync();
        }

        public async Task<IEnumerable<PersonneModel>> GetItemsAsync(string prenom, string nom)
        {
            IEnumerable<PersonneModel> result;
            ////pas utilisé CONTAINS car dit début et fin 
            //if (!string.IsNullOrEmpty(prenom) && string.IsNullOrEmpty(nom))
            //{

            //    result = await _context.Personnes
            //          .Where(p =>
            //          EF.Functions.Collate(p.Prenom, "NOCASE").StartsWith(prenom) ||
            //          EF.Functions.Collate(p.Prenom, "NOCASE").EndsWith(prenom)
            //            )
            //          .ToListAsync();
            //}
            //else if (!string.IsNullOrEmpty(nom) && string.IsNullOrEmpty(prenom))
            //{
            //    result = await _context.Personnes
            //          .Where(p =>
            //          EF.Functions.Collate(p.Nom, "NOCASE").StartsWith(nom) ||
            //          EF.Functions.Collate(p.Nom, "NOCASE").EndsWith(nom)
            //            )
            //          .ToListAsync();
            //}
            //else //both
            //{
            //    result = await _context.Personnes
            //         .Where(p =>
            //            (EF.Functions.Collate(p.Prenom, "NOCASE").StartsWith(prenom) || EF.Functions.Collate(p.Prenom, "NOCASE").EndsWith(prenom)) &&
            //            (EF.Functions.Collate(p.Nom, "NOCASE").StartsWith(nom) || EF.Functions.Collate(p.Nom, "NOCASE").EndsWith(nom))
            //          )
            //         .ToListAsync();
            //}

           result = await GetItemsAsync();
            if(!string.IsNullOrWhiteSpace(prenom))
            {
                result = result.Where(p => p.Prenom.Contains(prenom, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrWhiteSpace(nom))
            {
                result = result.Where(p => p.Nom.Contains(nom, StringComparison.OrdinalIgnoreCase));
            }
            return result;
        }

        public async Task UpdateItemAsync(PersonneModel item)
        {
            if (PersonneExists(item.Id))
            {
                var p = _context.Personnes.Single(p => p.Id == item.Id);
                p.Nom = item.Nom;
                p.Prenom = item.Prenom;

                await _context.SaveChangesAsync();
            }

            await Task.CompletedTask;
        }

        public async Task DeleteItemAsync(Guid id)
        {
            var v = _context.Personnes.Find(id);
            if (v != null)
            {
                _context.Personnes.Remove(v);
                await _context.SaveChangesAsync();
            }

            await Task.CompletedTask;
        }

        private bool PersonneExists(Guid id)
        {
            return _context.Personnes.Any(e => e.Id == id);
        }

    }
}
