# Social Media Platform - CQRS and Event Sourcing Project

This project is an implementation of a social media platform using the CQRS (Command Query Responsibility Segregation) and event sourcing architectural patterns. The project utilizes Kafka as the message broker, MongoDB for storing event-sourced aggregates, and SQL Server for the read model.

> **Warning**
> This project is in the initial phase. Once the bases of code works, I'll update the DOC with setup and run instructions.

## Table of Contents

- [Introduction](#introduction)
- [Architecture](#architecture)
- [Features](#features)
- [Getting Started](#getting-started)
- [Setup](#setup)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)

## Introduction

This project is designed to showcase how CQRS and event sourcing can be used to build a social media platform. It leverages Kafka for event-driven communication between microservices, MongoDB for storing event-sourced aggregates, and SQL Server for efficient querying of the read model.

## Architecture

The project follows a microservices architecture with the following components:

- Command Microservices: Responsible for handling commands and producing events.
- Event Store: MongoDB database to store event-sourced aggregates.
- Read Microservices: Responsible for querying data and maintaining the read model.
- Message Broker: Kafka for asynchronous communication between microservices.

## Features

- User registration and authentication
- Posting and commenting on posts
- Following and unfollowing users
- Real-time updates using Kafka
- Efficient querying using read model

## Getting Started

To get started with the project, follow these steps:

1. Clone the repository: `git clone https://github.com/your-username/social-media-cqrs-event-sourcing.git`
2. Navigate to the project directory: `cd social-media-cqrs-event-sourcing`

## Setup

1. Install and set up Kafka on your machine.
2. Set up MongoDB and create the required databases and collections.
3. Set up SQL Server and create the necessary tables for the read model.

## Usage

1. Start the command microservices to handle user actions and produce events.
2. Start the event store service using MongoDB.
3. Start the read microservices to handle queries and populate the read model.
4. Monitor Kafka topics for real-time updates.
5. Access the user interface or API endpoints to interact with the social media platform.

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
