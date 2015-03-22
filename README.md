UserManagementApplication
---

A simple user management application developed using an n-tier application design.

This is also a poor attempt at DDD, so expect a lot of poor object placement.


###Features: 
---
* Uses WCF remoting for client-service communication. Hosted using a CLI host.
* Client implemented using MVP on a WPF frontend (_very_ limited capability. Switching to MVVM is still in the works).

###Build Information:
---
* Developed using Visual Studio 2012 under .NET 4.5.1
* Unit tests created using Moq, FluentAssertions, and Xunit (enable nuget package restore to import these easily)
