# Digivance.Auth.Api
This project contains our primary web application for the Digivance Auth Microservices. This application exposes minimal api endpoints as well serves our React portal application.

This project expects a production build of the digivance_auth/app package to exist in /wwwroot. The project will run without it but will not serve the UI portal application. When we publish, we create that production build of the digivance_auth/app package.

Locally while debugging, you can and should set multiple startup projects. Select both this and the digivance_auth project.  The digivance_auth project will launch a browser and everything will work as expected.

This application leverages ASP minimal api endpoints to provide it's API functionality. These can be found in the Endpoints folder and is where you should get started.
