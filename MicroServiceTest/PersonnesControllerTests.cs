using FluentAssertions;
using FluentAssertions.Equivalency;
using MicroService;
using MicroService.Controllers;
using MicroService.Models;
using MicroService.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using static MicroService.Dtos.Dtos;

namespace MicroServiceTest
{
    public class PersonnesControllerTests
    {
        private readonly Mock<IPersonneRepository> _repositoryStub = new();
        private readonly Random _rand = new();
        
        [Fact]
        public async Task GetPersonneAsync_WithNotExistingPersonne_ReturnNotFound()
        {
            //Arange

            _repositoryStub.Setup(r => r.GetItemAsync(It.IsAny<Guid>()))
             .ReturnsAsync((PersonneModel)null);

            var controler = new PersonnesController(_repositoryStub.Object);

            //Act

            var result = await controler.GetPersonneAsync(Guid.NewGuid());
            //Assert            
            result.Result.Should().BeOfType<NotFoundResult>();


        }
        [Fact]
        public async Task GetPersonneAsync_WithExistingPersonne_ReturnExpectedPersonne()
        {
            //Arange

            var expectedPersonne = CreateRandomPersonne();

            _repositoryStub.Setup(r => r.GetItemAsync(It.IsAny<Guid>()))
             .ReturnsAsync(expectedPersonne);

            var controler = new PersonnesController(_repositoryStub.Object);

            //Act

            var result = await controler.GetPersonneAsync(Guid.NewGuid());
            //Assert

            var resultDTO = ((ObjectResult)result.Result).Value as GetPersonneDTO;


            resultDTO.Should().BeEquivalentTo(
                expectedPersonne,
                option=> option.ComparingByMembers<PersonneModel>() );

        }

        [Fact]
        public async Task GetPersonnesAsync_WithExistingPersonnes_ReturnAllPersonnes()
        {
            //Arange

            var expectedPersonnesArray = new[] { CreateRandomPersonne(), CreateRandomPersonne(), CreateRandomPersonne() };

            _repositoryStub.Setup(r => r.GetItemsAsync(It.IsAny<string>(), It.IsAny<string>()))
             .ReturnsAsync(expectedPersonnesArray);

            var controler = new PersonnesController(_repositoryStub.Object);

            //Act

            var result = await controler.GetPersonnesAsync();
            //Assert

            var resultPersonneArray = ((ObjectResult)result.Result).Value as IEnumerable<GetPersonneDTO>;

            resultPersonneArray.Should().BeEquivalentTo(
                expectedPersonnesArray,
                option => option.ComparingByMembers<PersonneModel>());

        }
        [Fact]        
        public async Task GetPersonnesAsync_WithNotExistingPersonnes_ReturnNotFound()
        {
            //Arange
      
            _repositoryStub.Setup(r => r.GetItemsAsync(It.IsAny<string>(), It.IsAny<string>()))
             .ReturnsAsync((IEnumerable<PersonneModel>)null);

            var controler = new PersonnesController(_repositoryStub.Object);

            //Act
   
            var result = await controler.GetPersonnesAsync("prenom", "nom");
            //Assert

            result.Result.Should().BeOfType<NotFoundResult>();

        }
        [Fact]
        public async Task PostPersonneAsync_WithPersonneToCreate_ReturnCreatedPersonne()
        {
            //Arange

            var personneToCreate = new CreatePersonneDTO(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());          

            var controler = new PersonnesController(_repositoryStub.Object);

            //Act
            var result = await controler.PostPersonneAsync(personneToCreate);

            //Assert

            var createdPersonne = (result.Result as CreatedAtActionResult).Value as GetPersonneDTO;
            
            createdPersonne.Should().NotBeNull();

            personneToCreate.Should().BeEquivalentTo(
                personneToCreate,
                option => option.ComparingByMembers<GetPersonneDTO>().ExcludingMissingMembers()
                );
            
            createdPersonne.Id.Should().NotBeEmpty();

        }

        [Fact]
        public async Task PutPersonneAsync_WithPersonneToCreate_ReturnNoContent()
        {

            //Arange

            var existingPersonne = CreateRandomPersonne();

            _repositoryStub.Setup(r => r.GetItemAsync(It.IsAny<Guid>()))
             .ReturnsAsync(existingPersonne);

            var controler = new PersonnesController(_repositoryStub.Object);

            var id = existingPersonne.Id;
            UpdatePersonneDTO updatedPersonne = new UpdatePersonneDTO(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()) ;

            //Act

            var result = await controler.PutPersonneAsync(id, updatedPersonne);
            //Assert

            result.Should().BeOfType<NoContentResult>();
        }
        [Fact]
        public async Task PutPersonneAsync_WithPersonneNotExist_ReturnNoFound()
        {

            //Arange

            var existingPersonne = CreateRandomPersonne();

            _repositoryStub.Setup(r => r.GetItemAsync(It.IsAny<Guid>()))
              .ReturnsAsync((PersonneModel)null);

            var controler = new PersonnesController(_repositoryStub.Object);

            var id = existingPersonne.Id;
            UpdatePersonneDTO updatedPersonne = new UpdatePersonneDTO(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            //Act

            var result = await controler.PutPersonneAsync(id, updatedPersonne);
            //Assert

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task PutPersonneAsync_WithException_ReturnNoFound()
        {

            //Arange

            var existingPersonne = CreateRandomPersonne();

            _repositoryStub.Setup(r => r.GetItemAsync(It.IsAny<Guid>()))
               .ReturnsAsync(existingPersonne);

            _repositoryStub.Setup(r => r.UpdateItemAsync(existingPersonne))            
              .Throws<DbUpdateConcurrencyException>();

            var controler = new PersonnesController(_repositoryStub.Object);

            var id = existingPersonne.Id;
            UpdatePersonneDTO updatedPersonne = new UpdatePersonneDTO(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            //Act

            var result = await controler.PutPersonneAsync(id, updatedPersonne);
            //Assert

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeletePersonneAsync_WithExistingPersonne_ReturnNoContent()
        {

            //Arange

            var existingPersonne = CreateRandomPersonne();

            _repositoryStub.Setup(r => r.GetItemAsync(It.IsAny<Guid>()))
               .ReturnsAsync(existingPersonne);
   
            var controler = new PersonnesController(_repositoryStub.Object);

            //Act

            var result = await controler.DeletePersonneAsync(existingPersonne.Id);
            //Assert

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeletePersonneAsync_WithNotExistingPersonne_ReturnNoFound()
        {

            //Arange

            var existingPersonne = CreateRandomPersonne();

            _repositoryStub.Setup(r => r.GetItemAsync(It.IsAny<Guid>()))
              .ReturnsAsync((PersonneModel)null);

            var controler = new PersonnesController(_repositoryStub.Object);

            //Act

            var result = await controler.DeletePersonneAsync(existingPersonne.Id);
            //Assert

            result.Should().BeOfType<NotFoundResult>();
        }

        private PersonneModel CreateRandomPersonne()
        {
            return new PersonneModel("prenom"+ _rand.Next(1000), "nom" + _rand.Next(1000));
        }
    }
}