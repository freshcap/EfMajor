# EfMajor
Entity Framework Core generic base classes for a developer friendly implementation.

The Unit of Work represents an instance of the DbContext for a specific database, and should be short-lived within the scope of its context. A common use case in an API would inject the UOW into your services - it will stay alive and then be disposed within each API call.

This library uses a repository/unit-of-work pattern, taking advantage of the built-in Entity Framework functionality for the basic use case. Take a look in the test project at the QueryTests class for what developer code might look like.

The basic use case can simply use the default Repository Factory; A deeper dive would let you customize individual repositories (per table) to apply custom behavior.
