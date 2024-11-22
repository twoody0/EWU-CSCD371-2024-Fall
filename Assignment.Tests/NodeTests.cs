﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Assignment;
using System.Globalization;
using System.Collections;

namespace Assignment.Tests;

[TestClass]
public class NodeTests
{
    [DataTestMethod]
    [DataRow(1)]
    [DataRow(11231)]
    [DataRow("MyDataValue")]
    [DataRow("AnotherValue")]
    public void Constructor_ValidInput_CreatesValidNode<T>(T value)
    {
        // Arrange
        Node<T> node = new(value);

        // Act

        // Assert
        Assert.IsNotNull(node);
        Assert.AreEqual(value, node.Value);
    }

    [DataTestMethod]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow("SomeData")]
    [DataRow("AnotherData")]
    public void ToString_ValidCall_ReturnsString<T>(T val)
    {
        // Arrange
        Node<T> node = new(val);

        // Act

        // Assert
        Assert.IsNotNull(node);
        Assert.AreEqual(val?.ToString(), node.ToString());
    }

    [TestMethod]
    public void ToString_NullValue_ReturnsNull()
    {
        // Arrange
        Node<string> node = new(null!);

        // Act

        // Assert
        Assert.IsNotNull(node);
        Assert.IsNull(node.ToString());
    }

    [TestMethod]
    public void Next_NoSet_ReturnsBaseNode()
    {
        // Arrange
        Node<int> node = new(1);

        // Act

        // Assert
        Assert.IsNotNull(node);
        Assert.IsNotNull(node.Next);
        Assert.AreEqual(node, node.Next);
        Assert.AreSame(node, node.Next);
    }

    [TestMethod]
    public void Append_ValidChange_AddsNodeAfterCurrent()
    {
        // Arrange
        Node<int> node = new(1);

        // Act
        node.Append(2);
        node.Append(3);

        // Assert
        Assert.IsNotNull(node);
        Assert.IsNotNull(node.Next);
        Assert.IsNotNull(node.Next.Next);
        Assert.AreEqual(1, node.Value);
        Assert.AreEqual(3, node.Next.Value);
        Assert.AreEqual(2, node.Next.Next.Value);
        Assert.AreSame(node, node.Next.Next.Next);
    }

    [DataTestMethod]
    [DataRow(1, 2, 3, 3, true)]
    [DataRow(1, 2, 3, 4, false)]
    [DataRow("SomeData", "AnotherData", "ThirdData", "AnotherData", true)]
    [DataRow("SomeData", "AnotherData", "ThirdData", "NotPresent", false)]
    public void Exists_ValidInput_ReturnsTrue<T>(T val, T val2, T val3, T expectedValue, bool expectedResult)
    {
        // Arrange
        Node<T> node = new(val);
        node.Append(val2);
        node.Append(val3);

        // Act
        bool exists = node.Exists(expectedValue);

        // Assert
        Assert.AreEqual(expectedResult, exists);
    }

    [TestMethod]
    public void Exists_NullValues_ReturnsTrue()
    {
        // Arrange
        Node<string> node = new(null!);

        // Act
        bool exists = node.Exists(null);

        // Assert
        Assert.IsTrue(exists);
    }

    [TestMethod]
    public void Exists_SameValues_ReturnsTrue()
    {
        // Arrange
        Node<string> node = new("SomeData");
        // Act
        bool exists = node.Exists("SomeData");
        // Assert
        Assert.IsTrue(exists);
    }

    [TestMethod]
    public void Exists_ValueIsNotNullExpectedValueIsNull_ReturnsFalse()
    {
        // Arrange
        Node<string> node = new("SomeData");

        // Act
        bool exists = node.Exists(null);

        // Assert
        Assert.IsFalse(exists);
    }

    [TestMethod]
    public void Exists_ValueIsNullExpectedValueNotNull_ReturnsFalse()
    {
        // Arrange
        Node<string> node = new(null!);

        // Act
        bool exists = node.Exists("SomeData");

        // Assert
        Assert.IsFalse(exists);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Append_DuplicateValues_ThrowsException()
    {
        // Arrange
        Node<int> node = new(1);
        node.Append(2);

        // Act
        node.Append(1);
    }

    [TestMethod]
    public void Clear_ValidLoop_RemovesNodes()
    {
        // Arrange
        Node<int> node = new(1);
        node.Append(2);
        node.Append(3);

        // Act
        node.Clear();

        // Assert
        Assert.AreEqual(node, node.Next);
        Assert.AreSame(node, node.Next);
    }

    [TestMethod]
    public void GetEnumerator_ValidLoop_ReturnsEnumerator()
    {
        // Arrange
        Node<int> node = new(1);
        node.Append(2);
        node.Append(3);

        // Act
        IEnumerator<int> enumerator = node.GetEnumerator();

        // Assert
        Assert.IsNotNull(enumerator);
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual(1, enumerator.Current);
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual(3, enumerator.Current);
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual(2, enumerator.Current);
        Assert.IsFalse(enumerator.MoveNext());
    }

    [TestMethod]
    public void GetEnumerator_NonGeneric_ReturnsEnumerator()
    {
        // Arrange
        Node<int> node = new(1);
        node.Append(2);
        node.Append(3);

        // Act
        IEnumerable enumerableNode = (IEnumerable)node;
        IEnumerator enumerator = enumerableNode.GetEnumerator();

        // Assert
        Assert.IsNotNull(enumerator);

        // Enumerate through the collection and verify
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual(1, enumerator.Current);
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual(3, enumerator.Current);
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual(2, enumerator.Current);
        Assert.IsFalse(enumerator.MoveNext());
    }

    [TestMethod]
    public void Node_ChildItems_ShouldReturnLimitedItems()
    {
        // Arrange
        Node<int> node = new(1);
        node.Append(2);
        node.Append(3);

        // Act
        List<int> limitedItems = node.ChildItems(2).ToList();

        // Assert
        Assert.IsNotNull(limitedItems, "The limited items collection should not be null.");
        Assert.AreEqual(2, limitedItems.Count, "The limited items collection should contain exactly 2 items.");
        Assert.IsTrue(limitedItems.SequenceEqual([1, 3]), "The items in the list do not match the expected limited values.");
    }

    [TestMethod]
    public void ChildItems_MaximumIsZero_ShouldReturnEmptyCollection()
    {
        // Arrange
        Node<int> node = new(1);
        node.Append(2);
        node.Append(3);

        // Act
        List<int> items = node.ChildItems(0).ToList();

        // Assert
        Assert.IsNotNull(items, "The items collection should not be null.");
        Assert.AreEqual(0, items.Count, "The items collection should be empty when maximum is 0.");
    }

    [TestMethod]
    public void ChildItems_MaximumExceedsNodeCount_ShouldReturnAllNodes()
    {
        // Arrange
        Node<int> node = new(1);
        node.Append(2);
        node.Append(3);

        // Act
        List<int> items = node.ChildItems(10).ToList();

        // Assert
        Assert.IsNotNull(items, "The items collection should not be null.");
        Assert.AreEqual(3, items.Count, "The items collection should contain all nodes when maximum exceeds the node count.");
        Assert.IsTrue(items.SequenceEqual([1, 3, 2]), "The items collection does not match the expected values.");
    }
}