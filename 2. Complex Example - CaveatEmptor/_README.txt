Model Readme
============

Getting Started
===============

This model serves to demonstrate the techniques shown in the book. 

To get started, simply run the tests in the Test folder, using NUnit or ReSharper or your favourite test runner.

Notes
=====

Note that the entities are all serializable, and all the database column mappings are explicitly stated in the mappings.
These won't be necessary for all projects, so you need to strip out anything that isn't necessary for your projects.

Mappings with XML or .NET Attributes?
=====================================

Mappings are done using the NHibernate Mapping Attributes library. You can see the .NET attributes in the entities, above the classes and their properties. 

If you prefer the idea of using XML over .NET attributes, take a look at the Mapping.hbm.xml. This was generated from the attributes.

A 3rd way of mapping in NHibernate is the Automatic Mapping feature of James Gregory's Fluent NHibernate project (google it!).

Obviously you should chose which method suits you best when developing your own solutions.

What's Demonstrated In The Entities?
====================================

Some complex mappings are shown in the entity classes in this Model folder. Also, some interesting techniques are covered.

Some examples:

Address.cs
	How to implement a immutable value type component
	How to map columns to properties using Mapping.Attributes
	Implementing Equals
	Using GetHashCode with a *business* key

BankAccount.cs
	Implementing and mapping inheritance
	Implementing IComparable
	Using generated key columns
	Mapping read ony property (see date Created)
	Bi-directional assocation to user
	SQL-Server date hack

Bid.cs
	Bi-Directional associations
	
CategorizedItem.cs
	Composite keys
	Not null columns
	
Category.cs		
	Cascading deletes
	Collection batching
	Native (db decides) key columns
	Mapping a tree structure
	Handling relationships at both ends
	
CreditCard.cs
	Mapping an Enum
	Inheritance mapping

Customer.cs
	Many-to-one association
	
User.cs
	Mapping 2 address components on one entity
	





