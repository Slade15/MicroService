using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MicroService.DBContexts;
using MicroService.Models;
using MicroService.Dtos;
using MicroService.Repositories;
using static MicroService.Dtos.Dtos;

namespace MicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonnesController : ControllerBase
    {
        private readonly IPersonneRepository _repository;

        public PersonnesController(IPersonneRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // GET: api/PersonneModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetPersonneDTO>>> GetPersonnesAsync([FromQuery] string? prenom = "", [FromQuery] string? nom = "")
        {
            IEnumerable<PersonneModel> PersonnesList = await _repository.GetItemsAsync(prenom, nom);

            if (PersonnesList is null || PersonnesList.Count() == 0)
            {
                return NotFound();
            }

            var result = PersonnesList.Select(p => p.AsDto());

            return Ok(result);
        }

        // GET: api/PersonneModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetPersonneDTO>> GetPersonneAsync(Guid id)
        {
            var personne = await _repository.GetItemAsync(id);

            if (personne == null)
            {
                return NotFound();
            }

            return Ok(personne.AsDto());
        }

        // PUT: api/PersonneModels/5        
        [HttpPut("{id}")]
        public async Task<ActionResult> PutPersonneAsync(Guid id, UpdatePersonneDTO personneDto)
        {

            var personne = await _repository.GetItemAsync(id);
            if (personne == null)
            {
                return NotFound();
            }


            personne.Nom = personneDto.Nom;
            personne.Prenom = personneDto.Prenom;

            try
            {

                await _repository.UpdateItemAsync(personne);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/PersonneModels
        
        [HttpPost]
        public async Task<ActionResult<GetPersonneDTO>> PostPersonneAsync(CreatePersonneDTO personne)
        {

            PersonneModel personneToAdd = new PersonneModel(personne.Prenom, personne.Nom);

            await _repository.AddItemAsync(personneToAdd);
                
            
            return CreatedAtAction(nameof(GetPersonnesAsync), new { id = personneToAdd.Id }, personneToAdd.AsDto());
        }

        // DELETE: api/PersonneModels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonneAsync(Guid id)
        {
            var personne = await _repository.GetItemAsync(id);
            if (personne == null)
            {
                return NotFound();
            }

            await _repository.DeleteItemAsync(personne.Id);

            return NoContent();
        }

 
    }
}
