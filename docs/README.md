# Prerequisites
The first step in getting started is to ensure that you have, or to install the necessary requisite frameworks, libraries and tools. Follow the links below to install each of the following if you have not already.

__Required__
- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)  
We use DOTNET 9 for our server side libraries and applications.

- [NodeJS v22](https://nodejs.org/en/download)  
We use NodeJS for our client side packages and UI.

- [PNPM v10](https://pnpm.io/installation)  
__We recommend installing globally__, we use the PNPM package manager for our client side packages.

- [Vite v5](https://vite.dev/guide/)  
__We recommend installing globally__, we use Vite as our builder and bundler tool for client side packages.

__Optional__
- [Docker](https://www.docker.com/)  
We use Docker to build our microservice application into a reusable container.

- [Visual Studio Community](https://visualstudio.microsoft.com/vs/community/)  
We recommend using the full visual studio (community or paid) when working with dotnet libraries and applications.

- [VS Code](https://code.visualstudio.com/)  
We also use and recommend using VS Code for various files. We support coding the dotnet libraries and applications with VS Code as well just recommend the full visual studio.

# Solution Structure
This solution contains various projects for shared dotnet libraries, server side applications, our portal website ui, and shared client side libraries. Please see the README files, and documentation of each of these for more details on each but here are a few important projects to note (and where to start looking).

__Client Side Monorepository__ ([digivance_auth](../digivance_auth/))  
The digivance_auth project is a "Monorepository" written in typescript and build using the PNPM package manager and Vite build tools.

__Server Side API__ ([Digivance.Auth.Api](../Digivance.Auth.Api/))  
The Digivance.Auth.Api project is a dotnet minimal api application written in C#. This application also hosts a production build of the client side "app" package (e.g. the portal site ui).  E.g. this is the "server" you host to offer Digivance Auth services.

# Make it go
There are 2 main ways that you can go about debugging and running everything. The first and recommended way is to start both the digivance_auth and Digivance.Auth.Api projects in debug mode. This will launch a browser to the digivance_auth project, this project will proxy api calls to the Digivance.Auth.Api project.

The simpler way, is that you can simply launch the Digivance.Auth.Api project in debug mode and open a browser to http://localhost:5000. This project has a prebuild event that will create a production build the digivance_auth project and copy it to the Digivance.Auth.Api projects /wwwroot folder.  When doing this, you do not have hot reload of the client code but you will have the most recent every time you start the Digivance.Auth.Api project.

# What's next?
We plan and build in public on github, please join us on our issues board. If you are looking to contribute please check out our guidelines and standards:

- [Coding standards](./STANDARDS.md)
- [How to contribute](./CONTRIBUTING.md)
