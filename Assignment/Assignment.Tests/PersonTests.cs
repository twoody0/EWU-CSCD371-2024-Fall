﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Assignment.Tests;

[TestClass]
public class PersonTests
{
    [DataTestMethod]
    [DataRow("John", "Doe", "john.doe@example.com")]
    public void Constructor_ValidInputs_Initializes(string firstName, string lastName, string emailAddress)
    {
        // Arrange
        Address address = new("123 Main St", "Anytown", "Anystate", "12345");
        Person person = new(firstName, lastName, address, emailAddress);

        // Act

        // Assert
        Assert.AreEqual(firstName, person.FirstName);
        Assert.AreEqual(lastName, person.LastName);
        Assert.AreEqual(address, person.Address);
        Assert.AreEqual(emailAddress, person.EmailAddress);
    }

    [DataTestMethod]
    [DataRow(null, "Doe", "john.doe@example.com")]
    [DataRow("John", null, "john.doe@example.com")]
    [DataRow("John", "Doe", null)]
    public void Constructor_InvalidInputs_ThrowsException(string firstName, string lastName, string emailAddress)
    {
        // Arrange
        Address address = new("123 Main St", "Anytown", "Anystate", "12345");

        // Act

        // Assert
        Assert.ThrowsException<ArgumentNullException>(() => new Person(firstName, lastName, address, emailAddress));
    }
}