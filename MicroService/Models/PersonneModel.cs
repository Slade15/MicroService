using System.ComponentModel.DataAnnotations;

namespace MicroService.Models
{
    public record PersonneModel
    {
        [Key]
        public Guid Id { get; private set; }

        [Required(ErrorMessage = $"{nameof(Prenom)} is required.")]
        public string Prenom { get; set; }

        [Required(ErrorMessage = $"{nameof(Nom)} is required.")]
        public string Nom { get; set; }

        public PersonneModel(string prenom, string nom)
        {
            Id = Guid.NewGuid();
            Prenom = prenom;
            Nom = nom;
        }


    }
}
