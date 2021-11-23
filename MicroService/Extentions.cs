using MicroService.Dtos;
using MicroService.Models;
using static MicroService.Dtos.Dtos;

namespace MicroService
{
    public static class Extentions
    {
        public static GetPersonneDTO AsDto(this PersonneModel personne)
        {
            return new GetPersonneDTO(personne.Id, personne.Prenom, personne.Nom);              
           
        }
    }
}
