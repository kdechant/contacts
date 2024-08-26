using ContactManager.Controllers;
using ContactManager.Data;
using ContactManager.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ContactManager.ContactManager.Tests;

public class ContactsControllerTest
{
    #region GetContact
    [Fact]
    public async Task GetContact()
    {
        // Arrange
        var repositoryMock = new Mock<IContactRepository>();
        repositoryMock
            .Setup(r => r.GetContactByIdAsync(1))
            .ReturnsAsync(new Contact { FirstName = "Arthur", LastName = "Dent", Email = "dent@arthur.dent", PhoneNumber = "123-123-1234" });

        var controller = new ContactsController(repositoryMock.Object);

        // Act
        var result = await controller.GetContact(1);
        var contact = result.Value;

        // Assert
        Assert.NotNull(contact);
        repositoryMock.Verify(r => r.GetContactByIdAsync(1), Times.Once);
        Assert.Equal("Arthur", contact.FirstName);
        Assert.Equal("Dent", contact.LastName);
    }

    [Fact]
    public async Task GetNonExistentContact()
    {
        // Arrange
        var repositoryMock = new Mock<IContactRepository>();

        var controller = new ContactsController(repositoryMock.Object);

        // Act
        var response = await controller.GetContact(2);

        // Assert
        Assert.IsType<NotFoundResult>(response.Result);
    }
    #endregion

    [Fact]
    public async Task GetAllContacts()
    {
        // Arrange
        var repositoryMock = new Mock<IContactRepository>();
        repositoryMock
            .Setup(r => r.GetContactsAsync())
        .ReturnsAsync(new[]
            {
            new Contact { FirstName = "Arthur", LastName = "Dent", Email = "dent@arthur.dent", PhoneNumber = "123-123-1234" },
            new Contact { FirstName = "Ford", LastName = "Prefect", Email = "ford@example.com", PhoneNumber = "555-555-4242" }
            });

        var controller = new ContactsController(repositoryMock.Object);

        // Act
        var response = await controller.GetContacts();
        var contacts = response.Value;

        // Assert
        Assert.NotNull(contacts);
        var contactsArray = contacts.ToArray();

        Assert.NotEmpty(contactsArray);
        repositoryMock.Verify(r => r.GetContactsAsync());
        Assert.Equal("Arthur", contactsArray[0].FirstName);
        Assert.Equal("Ford", contactsArray[1].FirstName);
    }

    [Fact]
    public async Task AddContact()
    {
        // Arrange
        var repositoryMock = new Mock<IContactRepository>();
        var controller = new ContactsController(repositoryMock.Object);
        var contact = new Contact { FirstName = "Tricia", LastName = "McMillan", Email = "trillian@example.com", PhoneNumber = "555-555-4242" };

        // Act
        await controller.PostContact(contact);

        // Assert
        repositoryMock.Verify(r => r.InsertContact(It.IsAny<Contact>()));
        repositoryMock.Verify(r => r.SaveAsync());
    }

    [Fact]
    public async Task UpdateContact()
    {
        // Arrange
        var contact = new Contact { Id = 1, FirstName = "DentArthurDent", LastName = "Dent", Email = "dent@arthurdent.com", PhoneNumber = "123-123-1234" };
        var repositoryMock = new Mock<IContactRepository>();
        repositoryMock
            .Setup(r => r.GetContactByIdAsync(1))
            .ReturnsAsync(contact);

        var controller = new ContactsController(repositoryMock.Object);

        // Act
        var response = await controller.PutContact(1, contact);

        // Assert
        // Successful PUT just returns a "204 No Content" HTTP response
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task DeleteContact()
    {
        // Arrange
        var contact = new Contact { Id = 1, FirstName = "DentArthurDent", LastName = "Dent", Email = "dent@arthurdent.com", PhoneNumber = "123-123-1234" };
        var repositoryMock = new Mock<IContactRepository>();
        repositoryMock
            .Setup(r => r.GetContactByIdAsync(1))
            .ReturnsAsync(contact);

        var controller = new ContactsController(repositoryMock.Object);

        // Act
        var response = await controller.DeleteContact(1);

        // Assert
        // Successful DELETE just returns a "204 No Content" HTTP response
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task DeleteNonExistentContact()
    {

        // Arrange
        var repositoryMock = new Mock<IContactRepository>();

        var controller = new ContactsController(repositoryMock.Object);

        // Act
        var response = await controller.DeleteContact(999);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }
}
