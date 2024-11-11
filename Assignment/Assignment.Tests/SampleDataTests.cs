﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Assignment.Tests;

[TestClass]
public class SampleDataTests
{
    [TestMethod]
    public void CsvRows_LoadDataFromCsv_SkipFirstRow()
    {
        // Arrange
        SampleData sampleData = new("People.csv");

        // Act
        List<string> csvRows = sampleData.CsvRows.ToList();

        // Assert
        Assert.IsNotNull(csvRows);
        Assert.IsTrue(csvRows.Count > 0);
        Assert.AreEqual("1,Priscilla,Jenyns,pjenyns0@state.gov,7884 Corry Way,Helena,MT,70577", csvRows[0]);
    }

    [TestMethod]
    public void GetUniqueSortedListOfStates_GivenCsvRows_ReturnsUniqueSortedListOfStates()
    {
        // Arrange
        SampleData sampleData = new("People.csv");

        // Act
        List<string> states = sampleData.GetUniqueSortedListOfStatesGivenCsvRows().ToList();
        string expectedState = states.OrderBy(state => state).First();

        // Assert
        Assert.IsNotNull(states);
        Assert.IsTrue(states.Count > 0);
        Assert.AreEqual(expectedState, states[0]);
        Assert.AreEqual(states.Count, states.Distinct().Count());
        Assert.IsTrue(states.SequenceEqual(states.OrderBy(s => s)), "The list should be sorted alphabetically using LINQ verification.");
    }

    [TestMethod]
    public void GetUniqueSortedListOfStates_HardCodedCsvRows_ReturnsUniqueSortedListOfStates()
    {
        // Arrange
        Mock<ISampleData> mockSampleData = new();
        mockSampleData.Setup(data => data.CsvRows).Returns(new List<string>
            {
                "1,Priscilla,Jenyns,pjenyns0@state.gov,7884 Corry Way,Helena,MT,70577",
                "2,John,Doe,jdoe1@example.com,1234 Elm St,Springfield,IL,62704",
                "3,Jane,Smith,jsmith2@example.com,5678 Oak St,Springfield,IL,62704",
                "4,Emily,Jones,ejones3@example.com,9101 Pine St,Helena,MT,70577",
                "5,Michael,Brown,mbrown4@example.com,1122 Maple St,Denver,CO,80203"
            });

        mockSampleData.Setup(list => list.GetUniqueSortedListOfStatesGivenCsvRows()).Returns(
            mockSampleData.Object.CsvRows
                .Select(row => row.Split(',')[6])
                .Distinct()
                .OrderBy(state => state)
        );

        // Act
        List<string> states = mockSampleData.Object.GetUniqueSortedListOfStatesGivenCsvRows().ToList();
        string expectedState = states.OrderBy(state => state).First();

        // Assert
        Assert.IsNotNull(states);
        Assert.IsTrue(states.Count > 0);
        Assert.AreEqual(expectedState, states[0]);
        Assert.AreEqual(states.Count, states.Distinct().Count());
        Assert.IsTrue(states.SequenceEqual(states.OrderBy(s => s)), "The list should be sorted alphabetically using LINQ verification.");
    }

    [TestMethod]
    [ExpectedException(typeof(FileNotFoundException))]
    public void Constructor_ShouldThrowFileNotFoundException_WhenFileDoesNotExist()
    {
        // Arrange & Act
        SampleData sampleData = new("NonExistentFile.csv");

        // Assert - Expecting a FileNotFoundException
    }

    [TestMethod]
    [ExpectedException(typeof(FormatException))]
    public void Constructor_ShouldThrowFormatException_WhenHeaderIsNull()
    {
        // Arrange
        string fileName = "TestFile.csv";
        File.WriteAllText(fileName, ""); // Create an empty file to simulate null header

        try
        {
            // Act
            SampleData sampleData = new(fileName);
        }
        finally
        {
            // Cleanup
            File.Delete(fileName);
        }
    }

    [TestMethod]
    [ExpectedException(typeof(FormatException))]
    public void Constructor_ShouldThrowFormatException_WhenHeaderDoesNotMatchExpected()
    {
        // Arrange
        string fileName = "TestFile.csv";
        File.WriteAllText(fileName, "Id,FirstName,LastName,Email,Street,City,State,PostalCode"); // Incorrect header format

        try
        {
            // Act
            SampleData sampleData = new(fileName);
        }
        finally
        {
            // Cleanup
            File.Delete(fileName);
        }
    }
}