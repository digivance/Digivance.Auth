# Digivance Auth
This repository contains various projects that power the Digivance Authentication and Authorization microservices. These microservices are an open source system allowing developers to easily implement user account and permission functionality to their applications.

  📢 **Digivance.Auth scaffolding complete?**  
Yes, it seems that way! We have code started, unit tests, workflows and automated CI/CD pipelines.  LG!!

  🔗 **See our beta site in action** (the current dev branch is live here!)  
[Digivance Auth Service (Beta)](https://auth-api-beta.gentlefield-3e921a70.eastus2.azurecontainerapps.io/)

  ➡️ **Come join us in Discord**  
https://discord.gg/xbn8CxdhhK

## Project Structure
This repository provides the following library and application projects, please see the readme files for each folder for more details on each.

- digivance_auth
  - This is a typescript project containing our React UI application and NPM api client packages.

- Digivance.Auth.Api
  - This is a dotnet minimal api that hosts our restful auth services.

- Digivance.Auth.Client
  - This is a C# shared library project that provides helpers for calling the auth api services via C# code.

- Digivance.Auth.Data
  - This is our shared data layer, containing the core commands and models for our auth services.

- Digivance.Auth.Data.EntityFramework
  - This shared library defines our entity framework entities and and services.

# More information

- [Licensing](./docs/LICENSE.md)
- [Getting started](./docs/README.md)
- [Coding standards](./docs/STANDARDS.md)
- [How to contribute](./docs/CONTRIBUTING.md)
