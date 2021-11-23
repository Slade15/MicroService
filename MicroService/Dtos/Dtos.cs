using System.ComponentModel.DataAnnotations;

namespace MicroService.Dtos
{
    public class Dtos
    {
        public record GetPersonneDTO(Guid Id,string Prenom,string Nom );
        public record CreatePersonneDTO([Required] string Prenom,[Required] string Nom );
        public record UpdatePersonneDTO([Required] string Prenom, [Required] string Nom);
    }
}
