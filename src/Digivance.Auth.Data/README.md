# Digivance.Auth.Data
This folder contains our shared data models and CQRS contract models library. This library is published publicly to nuget and is consumed by many other projects in this repository: Digivance.Auth.Api, Digivance.Auth.Client, Digivance.Auth.Data.EntityFramework.

This library is organized under basic CQRS separation considerations, such as:

[Commands](./Commands) are data models sent from a client to a server asking for something, such as to create, update, delete or list records.

[Models](./Models) are data models sent from a server to a client and represent our core data objects.

[Services](./Services) are interfaces that explain how a server or client might execute commands and receive models.
