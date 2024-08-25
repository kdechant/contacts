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
    public void GetContact()
    {
        // Arrange
        var repositoryMock = new Mock<IContactRepository>();
        repositoryMock
            .Setup(r => r.GetContactByID(1))
            .Returns(new Contact { FirstName = "Arthur", LastName = "Dent", Email = "dent@arthur.dent", PhoneNumber = "123-123-1234" });

        var controller = new ContactsController(repositoryMock.Object);

        // Act
        var contact = controller.GetContact(1).Value;

        // Assert
        Assert.NotNull(contact);
        repositoryMock.Verify(r => r.GetContactByID(1));
        Assert.Equal("Arthur", contact.FirstName);
        Assert.Equal("Dent", contact.LastName);
    }

    [Fact]
    public void GetNonExistentContact()
    {
        // Arrange
        var repositoryMock = new Mock<IContactRepository>();

        var controller = new ContactsController(repositoryMock.Object);

        // Act
        var response = controller.GetContact(2);

        // Assert
        Assert.IsType<NotFoundResult>(response.Result);
    }
    #endregion

    [Fact]
    public void GetAllContacts()
    {
        // Arrange
        var repositoryMock = new Mock<IContactRepository>();
        repositoryMock
            .Setup(r => r.GetContacts())
        .Returns(new[]
            {
            new Contact { FirstName = "Arthur", LastName = "Dent", Email = "dent@arthur.dent", PhoneNumber = "123-123-1234" },
            new Contact { FirstName = "Ford", LastName = "Prefect", Email = "ford@example.com", PhoneNumber = "555-555-4242" }
            });

        var controller = new ContactsController(repositoryMock.Object);

        // Act
        var contacts = controller.GetContacts().Value;

        // Assert
        Assert.NotNull(contacts);
        var contactsArray = contacts.ToArray();

        Assert.NotEmpty(contactsArray);
        repositoryMock.Verify(r => r.GetContacts());
        Assert.Equal("Arthur", contactsArray[0].FirstName);
        Assert.Equal("Ford", contactsArray[1].FirstName);
    }

    [Fact]
    public void AddContact()
    {
        // Arrange
        var repositoryMock = new Mock<IContactRepository>();
        var controller = new ContactsController(repositoryMock.Object);
        var contact = new Contact { FirstName = "Tricia", LastName = "McMillan", Email = "trillian@example.com", PhoneNumber = "555-555-4242" };

        // Act
        controller.PostContact(contact);

        // Assert
        repositoryMock.Verify(r => r.InsertContact(It.IsAny<Contact>()));
        repositoryMock.Verify(r => r.Save());
    }

    [Fact]
    public void UpdateContact()
    {
        // Arrange
        var contact = new Contact { Id = 1, FirstName = "DentArthurDent", LastName = "Dent", Email = "dent@arthurdent.com", PhoneNumber = "123-123-1234" };
        var repositoryMock = new Mock<IContactRepository>();
        repositoryMock
            .Setup(r => r.GetContactByID(1))
            .Returns(contact);

        var controller = new ContactsController(repositoryMock.Object);

        // Act
        var response = controller.PutContact(1, contact);

        // Assert
        // Successful PUT just returns a "204 No Content" HTTP response
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public void DeleteContact()
    {

        // Arrange
        var contact = new Contact { Id = 1, FirstName = "DentArthurDent", LastName = "Dent", Email = "dent@arthurdent.com", PhoneNumber = "123-123-1234" };
        var repositoryMock = new Mock<IContactRepository>();
        repositoryMock
            .Setup(r => r.GetContactByID(1))
            .Returns(contact);

        var controller = new ContactsController(repositoryMock.Object);

        // Act
        var response = controller.DeleteContact(1);

        // Assert
        // Successful DELETE just returns a "204 No Content" HTTP response
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public void DeleteNonExistentContact()
    {

        // Arrange
        var repositoryMock = new Mock<IContactRepository>();

        var controller = new ContactsController(repositoryMock.Object);

        // Act
        var response = controller.DeleteContact(999);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }
}
