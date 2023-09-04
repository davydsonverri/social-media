
# Social Media Platform using CQRS and Event Sourcing

This project is an implementation of a social media platform using the CQRS (Command Query Responsibility Segregation) and event sourcing architectural patterns. The project utilizes Kafka as the message broker, MongoDB for storing event-sourced aggregates, and SQL Server for the read model.

## Table of Contents

- [Introduction](#introduction)
- [Architecture](#architecture)
- [Features](#features)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Setup](#setup)
- [Usage - Visual Studio 2022](#usage---visual-studio-2022)
- [Contributing](#contributing)
- [License](#license)

## Introduction

This project designed to showcase how CQRS and event sourcing can be implemented to build a social media platform. It leverages Kafka for event-driven communication between microservices, MongoDB for storing event-sourced aggregates, and SQL Server for efficient querying as read model.

## Architecture

Will be available soon

## Features

- Publish and edit posts
- Commenting and liking posts
- CQRS
- Event Sourcing with MongoDB as EventStore
- SQL Server as read database for queries

## Getting Started

To get a local copy up and running follow these steps.

### Prerequisites
#### Docker
Regardless of your operating system, you must install a container virtualization tool to run all infrastructure resources(Apache Kafka, MongoDB and others). We suggest [Docker](https://www.docker.com/), but feel free to use one you prefer.

- [Install Docker Desktop on Windows](https://docs.docker.com/desktop/install/windows-install/)
- [Install Docker Desktop on Linux](https://docs.docker.com/desktop/install/linux-install/)

After installation run following code on terminal:

    docker --version
You should see something like this:

    Docker version 24.0.5, build ced0996

#### .NET Core 7.0
This project were build on top of .NET 7.0. Make sure you have it installed.

[Install .NET 7.0 (Windows, Linux, MacOS)](https://dotnet.microsoft.com/pt-br/download/dotnet/7.0)

After installation run following code on terminal:

    dotnet --list-sdks
You should see something like this:

    7.0.400 [C:\Program Files\dotnet\sdk]

## Setup
### Clone source code
First, you should clone repository: 

    git clone https://github.com/davydsonsantana/social-media.git

### Set SQL Server connection string
Open file `social-media/src/Post.Query.Api/appsettings.Development.json` and set connection string as follow:

    "ConnectionStrings": {
       "SqlServer": "Server=localhost,1433;Database=SocialMedia;User Id=sa;Password=FA22lFI9gRpUzfe;Encrypt=False;"
    }

### Startup infrastructure
Open terminal and navigate to project directory: 

    cd social-media
Navigate to docker directory: 

    cd docker
Startup container as follow:

    docker-compose up -d

You should see something like this on you Docker Desktop:

![image](https://github.com/davydsonsantana/social-media/assets/1733168/ffa8bfb2-32fb-4db3-b50c-52a244e2c753)


## Usage - Visual Studio 2022

1. Open solution file `social-media/src/SocialMedia.sln`.
2. Open `Configure Startup Projects...` as follow:
   
   ![image](https://github.com/davydsonsantana/social-media/assets/1733168/c9925c91-ca05-4a2f-8e85-aaa6e5bed9a7)
   
3. Setup `Multiple startup projects` as follow:
   
   ![image](https://github.com/davydsonsantana/social-media/assets/1733168/2cbfad05-0008-44f6-beca-0323ab0ee268)
   
4. Just click on `Start` or press F5.
  
    ![image](https://github.com/davydsonsantana/social-media/assets/1733168/1ff27b8b-4912-4533-af4b-f9c3aeca8ed1)

5. Two Swagger tabs will be open on your browser, first for `Post.Command.Api` and seccond one for `Post.Query.Api`.
6. Enjoy.

## Contributing

Contributions are welcome! If you'd like to contribute to the project, please follow these steps:

1. Fork the repository.
2. Create a new branch for your feature/bugfix: `git checkout -b feature-name`
3. Make your changes and commit them.
4. Push your changes to your fork: `git push origin feature-name`
5. Submit a pull request to the main repository.

## License

This project is licensed under the [MIT License](LICENSE).

---
