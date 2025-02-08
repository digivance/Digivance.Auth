# Coding standards
We will continue to evolve and expand upon these standards as our project progresses, but for some quick explanations of what standards and patterns we mean to follow:

__Simple CQRS__:  
We intend for our business logic to be produced via service classes and act on command objects.

__Minimal API__:  
We use ASP minimal API pattern to receieve HTTP requests, enforce permissions and execute services via commands.

__Interfaces__:  
We use interfaces for our service classes and very sparingly elsewhere. We want to minimize obfuscation of code wherever possible by prefering concrete typing.

__Entity Framework__:  
At this time we implement server side service classes along with Entity Framework allowing us to persist data records to any compatible database.

__React__:  
We use React via the typescript language with Vite and PNPM. Functional programming with modern recommended practices.

__PNPM / Monorepository__:  
Our digivance_auth project is a PNPM monorepository (workspaces) project.
