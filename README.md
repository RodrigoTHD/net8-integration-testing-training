# Training: Integration Testing in .NET 8

This repository contains test projects examples for the .NET ecosystem used in the `Integration Testing in .NET 8` course.

## Learning Objectives

- Understand and define the purpose of integration testing.
- Identify benefits and challenges of integration testing.
- Organize test files, folders, and test data effectively.
- Integrate the main project and manage test dependencies.
- Compare MSTest, xUnit and NUnit, understanding similarities and differences.
- Set up and write integration tests using Nunit.
- Use WebApplicationFactory for ASP.NET Core applications.
- Set up test services and mock dependencies.
- Testing Query Parameters and Request Headers and Status Codes.
- Utilize test lifecycle features in NUnit ([SetUp], [TearDown]).
- Write and manage async integration tests in .NET.
- Implement async setup and teardown methods.
- Avoid pitfalls in asynchronous testing.
- Understand BDD and write feature files with Cucumber.
- Use SpecFlow for integration test execution.
- Decide between real and mocked dependencies in tests.
- Configure Dependency Injection for integration tests.
- Use mocking tools and techniques effectively.
- Integrate reporting tools for test results.

## Table of Contents

### `Basic Topics`

#### Module 1: Introduction to Integration Testing

1. What is Integration Testing?
2. Setting Up a Testing Environment in .NET 8

#### Module 2: ASP.NET Core Integration Testing Fundamentals

1. Working with ASP.NET Core TestHost
2. WebApplicationFactory in .NET
3. Configuring Test Environments and Dependencies

#### Module 3: Dependency Injection and Service Mocking

1 Introduction to Dependency Injection in .NET
2 Overview of mocking libraries in .NET
3 Advanced Dependency Injection

#### Module 4: HTTP Client Testing and Route Verification

1. Introduction to HttpClient in .NET
2. Testing HTTP Endpoints with HttpClient
3. Testing Query Parameters and Request Headers and Status Codes

#### Module 5: Error Handling, Logging, and Diagnostics

1. Error Handling in Integration Tests
2. Structured Logging in Tests
3. Using Diagnostics and Tracing for Debugging

### `Advanced Topics`

#### Module 6: Introduction to Behavior-Driven Development (BDD) and Cucumber

1. BDD Concepts
2. Cucumber and SpecFlow Overview
3. Setting Up a SpecFlow Project in .NET

#### Module 7: Implementing Step Definitions and Running Integration Tests

1. Creating a Feature File for ASP.NET Core APIs
2. Writing Step Definitions for API Scenarios
3. Handling Reusable Steps and Common Test Logic
4. Running Tests and Generating Reports
