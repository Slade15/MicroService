using FluentAssertions;
using MicroService.DBContexts;
using MicroService.Models;
using MicroService.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MicroServiceTest
{
    public class PersonneRepositoryTests
    {
        private readonly PersonneContext _context;
        private readonly PersonneRepository _repo;

        public PersonneRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<PersonneContext>()
             .UseInMemoryDatabase(databaseName: "PersonneDb")
             .Options;

            //init 
            _context = new PersonneContext(options);
            _context.Personnes.Add(new PersonneModel("Sébastien", "Marchal"));
            _context.Personnes.Add(new PersonneModel("noslein", "moreira"));
            _context.SaveChanges();

            _repo = new PersonneRepository(_context);

        }        

        [Fact]
        public async Task GetItemsAsync_WithEmptyParam_ReturnAllItem()
        {
            //Arange
         

            //Act

            var result = await _repo.GetItemsAsync("", "");
            //Assert            

            result.Should().BeEquivalentTo(_context.Personnes);

        }

        [Fact]
        public async Task GetItemsAsync_WithMatchingPrenom_ReturnMathcingItems()
        {
            //Arange


            //Act

            var result = await _repo.GetItemsAsync("séb", "");
            //Assert            

            result.Should().OnlyContain(item=> item.Prenom == "Sébastien");

        }

        [Fact]
        public async Task GetItemsAsync_WithMatchingNom_ReturnMathcingItems()
        {
            //Arange


            //Act

            var result = await _repo.GetItemsAsync("", "ra");
            //Assert            

            result.Should().OnlyContain(item => item.Nom == "moreira");

        }

        [Fact]
        public async Task AddItemAsync_WithItemToCreate_ReturnCreatedItem()
        {
            //Arange
            PersonneModel newItem = new PersonneModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());


            //Act

            var result = await _repo.AddItemAsync(newItem);
            //Assert            

            result.Should().BeEquivalentTo(newItem);

        }

        [Fact]
        public async Task GetItemAsync_WithExistingItem_ReturnExistingItem()
        {
            //Arange

            PersonneModel existingItem = _context.Personnes.First();

            //Act

            var result = await _repo.GetItemAsync(existingItem.Id);
            //Assert            

            result.Should().BeEquivalentTo(existingItem);


        }
        [Fact]
        public async Task GetItemAsync_WithNotExistingItem_ReturnNull()
        {
            //Arange


            //Act

            var result = await _repo.GetItemAsync(It.IsAny<Guid>());
            //Assert            

            result.Should().BeNull();


        }

        [Fact]
        public async Task GetItemsAsync_WithExistingItems_ReturAllItems()
        {
            //Arange

            //Act

            var result = await _repo.GetItemsAsync();
            //Assert            

            result.Should().HaveCount(_context.Personnes.Count());


        }
        [Fact]
        public async Task UpdateItemAsync_WithExistingItems_ItemUpdated()
        {
            //Arange

            PersonneModel existingItem = _context.Personnes.First();

            existingItem.Nom = "newName";
            existingItem.Prenom = "NewPrenom";

            //Act

            await _repo.UpdateItemAsync(existingItem);
            //Assert            

            existingItem.Should().BeEquivalentTo(_context.Personnes.First());


        }

        [Fact]
        public async Task DeleteItemAsync_WithExistingItems_ItemDeleted()
        {
            //Arange

            PersonneModel existingItem = _context.Personnes.First();

            //Act

            await _repo.DeleteItemAsync(existingItem.Id);
            //Assert            

            _context.Personnes.Should().NotContain(existingItem);
            


        }



    }
}
