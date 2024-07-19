# Microservices Architecture with .NET

This project demonstrates a microservices architecture using .NET. It includes Product and Order services for managing product and order information stored in MSSQL. The project employs RabbitMQ and MassTransit for message queueing and MongoDB for stock management.

## Overview

When a user places an order, the order details are added to a queue using MassTransit and RabbitMQ. The Stock Service then checks the stock levels in MongoDB. If sufficient stock is available, the necessary information is sent to another queue for the Payment Service to process the payment.

## Services and Technologies

### Product and Order Services
- **MSSQL**: Used to store product and order information.
- **ASP.NET Core**: Framework for building the web services.
- **Entity Framework Core**: ORM for database operations.

### Message Queueing
- **MassTransit**: A .NET library for message-based applications.
- **RabbitMQ**: A message broker for queueing the orders and stock information.

### Stock Service
- **MongoDB**: NoSQL database for storing product stock information.
- **MassTransit and RabbitMQ**: Used to communicate between the Order Service and Stock Service.

### Payment Service
- **MassTransit and RabbitMQ**: Consumes the stock information queue and processes the payment.

## Architecture

1. **Order Placement**: 
   - When a user places an order, the order details are sent to a queue using RabbitMQ.

2. **Stock Check**:
   - The Stock Service listens to the order queue, retrieves the order details, and checks the stock levels in MongoDB.

3. **Stock Validation**:
   - If sufficient stock is available, the Stock Service sends the stock information to another queue for the Payment Service.

4. **Payment Processing**:
   - The Payment Service consumes the stock information queue and processes the payment.

## Why These Technologies?

- **MSSQL**: Provides a reliable relational database for storing structured data.
- **MongoDB**: Suitable for managing large volumes of unstructured data, such as product stocks.
- **MassTransit**: Simplifies the integration of message-based communication in .NET applications.
- **RabbitMQ**: Efficient message broker that supports complex routing and reliability.
- **ASP.NET Core**: A high-performance framework for building modern, cloud-based, internet-connected applications.

## Getting Started


### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/Microservices.Example.git
   cd Microservices.Example
