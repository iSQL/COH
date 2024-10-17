COH - City on hands, Playground for clean arhitecture in .NET 8 and a prototype for application to fulfill a city management, eg. haircut resevations, event management, handyman review and lookout, etc. .

# Clean Architecture

A starting point for Clean Architecture with ASP.NET Core. [Clean Architecture](https://8thlight.com/blog/uncle-bob/2012/08/13/the-clean-architecture.html) is just the latest in a series of names for the same loosely-coupled, dependency-inverted architecture. You will also find it named [hexagonal](http://alistair.cockburn.us/Hexagonal+architecture), [ports-and-adapters](http://www.dossier-andreas.net/software_architecture/ports_and_adapters.html), or [onion architecture](http://jeffreypalermo.com/blog/the-onion-architecture-part-1/).

This architecture is used in the [DDD Fundamentals course](https://www.pluralsight.com/courses/fundamentals-domain-driven-design) by [Steve Smith](https://ardalis.com) and [Julie Lerman](https://thedatafarm.com/).

:school: Contact Steve's company, [NimblePros](https://nimblepros.com/), for Clean Architecture or DDD training and/or implementation assistance for your team.

## Take the Course!

[Learn about how to implement Clean Architecture](https://academy.nimblepros.com/p/intro-to-clean-architecture) from [NimblePros](https://nimblepros.com) trainers [Sarah "sadukie" Dutkiewicz](https://blog.nimblepros.com/author/sadukie/) and [Steve "ardalis" Smith](https://blog.nimblepros.com/author/ardalis/).

## Troubleshooting Chrome Errors

By default the site uses HTTPS and expects you to have a self-signed developer certificate for localhost use. If you get an error with Chrome [see this answer](https://stackoverflow.com/a/31900210/13729) for mitigation instructions.

## Learn More

- [Live Stream Recordings Working on Clean Architecture](https://www.youtube.com/c/Ardalis/search?query=clean%20architecture)
- [DotNetRocks Podcast Discussion with Steve "ardalis" Smith](https://player.fm/series/net-rocks/clean-architecture-with-steve-smith)
- [Fritz and Friends Streaming Discussion with Steve "ardalis" Smith](https://www.youtube.com/watch?v=k8cZUW4MS3I)

## Where To Validate

Validation of user input is a requirement of all software applications. The question is, where does it make sense to implement it in a concise and elegant manner? This solution template includes 4 separate projects, each of which might be responsible for performing validation as well as enforcing business invariants (which, given validation should already have occurred, are usually modeled as exceptions).

The domain model itself should generally rely on object-oriented design to ensure it is always in a consistent state. It leverages encapsulation and limits public state mutation access to achieve this, and it assumes that any arguments passed to it have already been validated, so null or other improper values yield exceptions, not validation results, in most cases.

The use cases / application project includes the set of all commands and queries the system supports. It's frequently responsible for validating its own command and query objects.

The Web project includes all API endpoints, which include their own request and response types, following the [REPR pattern](https://deviq.com/design-patterns/repr-design-pattern). The FastEndpoints library includes built-in support for validation using FluentValidation on the request types. This is a natural place to perform input validation as well.

Having validation occur both within the API endpoints and then again at the use case level is redundant, so in this template the choice has been made to validate at the edge of the application, in the API endpoints. This means some future consumer of the Use Cases project will also need to be responsible for its own validation as well, but in the vast majority of cases there won't be any other consumers of the use cases outside of the API endpoints.

## The Core Project

The Core project is the center of the Clean Architecture design, and all other project dependencies should point toward it. As such, it has very few external dependencies. The Core project should include the Domain Model including things like:

- Entities
- Aggregates
- Value Objects
- Domain Events
- Domain Event Handlers
- Domain Services
- Specifications
- Interfaces
- DTOs (sometimes)

## The Use Cases Project

An optional project, I've included it because many folks were demanding it and it's easier to remove than to add later. This is also often referred to as the *Application* or *Application Services* layer. The Use Cases project is organized following CQRS into Commands and Queries. Commands mutate the domain model and thus should always use Repository abstractions for their data access. Queries are readonly, and thus do not need to use the repository pattern, but instead can use whatever query service or approach is most convenient. However, since the Use Cases project is set up to depend on Core and not directly on Infrastructure, there will still need to be abstractions defined for its data access. And it *can* use things like specifications, which can sometimes help encapsulate query logic as well as result type mapping. But it doesn't *have* to use repository/specification - it can just issue a SQL query or call a stored procedure if that's the most efficient way to get the data.

Although this is an optional project to include (without it, your API endpoints would just work directly with the domain model or query services), it does provide a nice UI-ignorant place to add automated tests.

## The Infrastructure Project

Most of your application's dependencies on external resources should be implemented in classes defined in the Infrastructure project. These classes should implement interfaces defined in Core. If you have a very large project with many dependencies, it may make sense to have multiple Infrastructure projects (e.g. Infrastructure.Data), but for most projects one Infrastructure project with folders works fine. The sample includes data access and domain event implementations, but you would also add things like email providers, file access, web api clients, etc. to this project so they're not adding coupling to your Core or UI projects.

The Infrastructure project depends on `Microsoft.EntityFrameworkCore.SqlServer` and `Autofac`. The former is used because it's built into the default ASP.NET Core templates and is the least common denominator of data access. If desired, it can easily be replaced with a lighter-weight ORM like Dapper. Autofac (formerly StructureMap) is used to allow wireup of dependencies to take place closest to where the implementations reside. In this case, an InfrastructureRegistry class can be used in the Infrastructure class to allow wireup of dependencies there, without the entry point of the application even having to have a reference to the project or its types. [Learn more about this technique](https://ardalis.com/avoid-referencing-infrastructure-in-visual-studio-solutions). The current implementation doesn't include this behavior - it's something I typically cover and have students add themselves in my workshops.

## The Web Project

The entry point of the application is the ASP.NET Core web project. This is actually a console application, with a `public static void Main` method in `Program.cs`. It currently uses the default MVC organization (Controllers and Views folders) as well as most of the default ASP.NET Core project template code. This includes its configuration system, which uses the default `appsettings.json` file plus environment variables, and is configured in `Startup.cs`. The project delegates to the `Infrastructure` project to wire up its services using Autofac.

## The SharedKernel Project

Many solutions will also reference a separate **Shared Kernel** project/package. I recommend creating a separate SharedKernel project and solution if you will require sharing code between multiple [bounded contexts](https://ardalis.com/encapsulation-boundaries-large-and-small/) (see [DDD Fundamentals](https://www.pluralsight.com/courses/domain-driven-design-fundamentals)). I further recommend this be published as a NuGet package (most likely privately within your organization) and referenced as a NuGet dependency by those projects that require it.

Previously a project for SharedKernel was included in this project. However, for the above reasons I've made it a separate package, [Ardalis.SharedKernel](https://github.com/ardalis/Ardalis.SharedKernel), which you should replace with your own when you use this template.

If you want to see another [example of a SharedKernel package, the one I use in my updated Pluralsight DDD course is on NuGet here](https://www.nuget.org/packages/PluralsightDdd.SharedKernel/).


## The Test Projects

Test projects could be organized based on the kind of test (unit, functional, integration, performance, etc.) or by the project they are testing (Core, Infrastructure, Web), or both. For this simple starter kit, the test projects are organized based on the kind of test, with unit, functional and integration test projects existing in this solution. In terms of dependencies, there are three worth noting:

- [xunit](https://www.nuget.org/packages/xunit) I'm using xunit because that's what ASP.NET Core uses internally to test the product. It works great and as new versions of ASP.NET Core ship, I'm confident it will continue to work well with it.

- [NSubstitute](https://www.nuget.org/packages/NSubstitute) I'm using NSubstitute as a mocking framework for white box behavior-based tests. If I have a method that, under certain circumstances, should perform an action that isn't evident from the object's observable state, mocks provide a way to test that. I could also use my own Fake implementation, but that requires a lot more typing and files. NSubstitute is great once you get the hang of it, and assuming you don't have to mock the world (which we don't in this case because of good, modular design).

- [Microsoft.AspNetCore.TestHost](https://www.nuget.org/packages/Microsoft.AspNetCore.TestHost) I'm using TestHost to test my web project using its full stack, not just unit testing action methods. Using TestHost, you make actual HttpClient requests without going over the wire (so no firewall or port configuration issues). Tests run in memory and are very fast, and requests exercise the full MVC stack, including routing, model binding, model validation, filters, etc.

# Patterns Used

This solution template has code built in to support a few common patterns, especially Domain-Driven Design patterns. Here is a brief overview of how a few of them work.

## Domain Events

Domain events are a great pattern for decoupling a trigger for an operation from its implementation. This is especially useful from within domain entities since the handlers of the events can have dependencies while the entities themselves typically do not. In the sample, you can see this in action with the `ToDoItem.MarkComplete()` method. The following sequence diagram demonstrates how the event and its handler are used when an item is marked complete through a web API endpoint.

![Domain Event Sequence Diagram](https://user-images.githubusercontent.com/782127/75702680-216ce300-5c73-11ea-9187-ec656192ad3b.png)

## Related Projects

- [ApiEndpoints](https://github.com/ardalis/apiendpoints)
- [GuardClauses](https://github.com/ardalis/guardclauses)
- [HttpClientTestExtensions](https://github.com/ardalis/HttpClientTestExtensions)
- [Result](https://github.com/ardalis/result)
- [SharedKernel](https://github.com/ardalis/Ardalis.SharedKernel)
- [SmartEnum](https://github.com/ardalis/SmartEnum)
- [Specification](https://github.com/ardalis/specification)
