# Adform Squares API

## Prerequisites

Before you begin, ensure you have the following installed on your system:

1. Docker
2. Docker Compose
3. A clone of this repository on your local machine

## Steps to Run the Application

1. **Clone the Repository**
```
git clone https://github.com/devJUKI/adform-squares-api
cd adform-squares-api/adform-squares-api
```

2. **Build and Start the Application**

Run the following command to build and start the services defined in the docker-compose.yml file:
```
docker-compose up --build
```
This command will:

- Build the Docker images for the application services if they are not already built.
- Start the containers.

3. **Access the Application**

Once the containers are running, you can access the application using the following URLs:

- Web API: http://localhost:8080/
- Grafana: http://localhost:3000/ (username & password - admin)
- Prometheus http://localhost:9090/ (all Prometheus data is visible in Grafana)

## Endpoints

### User Authentication

#### **Register New User**
**POST** `/api/v1/register`

Register a new user.

- **Request Body**:
  - Content-Type: `application/json`, `text/json`, `application/*+json`
  - Schema: [RegisterViewModel](#registerviewmodel)

- **Responses**:
  - `200 OK`: Successfully registered the user.

---

### User Points

#### **Get User Squares**
**GET** `/api/v1/users/{userId}/squares`

Retrieve a list of squares for a given user.

- **Path Parameters**:
  - `userId` (UUID): Unique identifier of the user.

- **Responses**:
  - `200 OK`: Successfully retrieved the data.

---

#### **Add Points**
**POST** `/api/v1/users/{userId}/points`

Add a new point for a given user.

- **Path Parameters**:
  - `userId` (UUID): Unique identifier of the user.

- **Request Body**:
  - Content-Type: `application/json`, `text/json`, `application/*+json`
  - Schema: [PointViewModel](#pointviewmodel)

- **Responses**:
  - `200 OK`: Successfully added the point.

---

#### **Import Points**
**POST** `/api/v1/users/{userId}/points/import`

Import multiple points for a given user.

- **Path Parameters**:
  - `userId` (UUID): Unique identifier of the user.

- **Request Body**:
  - Content-Type: `application/json`, `text/json`, `application/*+json`
  - Schema: Array of [PointViewModel](#pointviewmodel)

- **Responses**:
  - `200 OK`: Successfully imported the points.

---

#### **Delete Point**
**DELETE** `/api/v1/users/{userId}/points/{pointId}`

Delete a specific point for a given user.

- **Path Parameters**:
  - `userId` (UUID): Unique identifier of the user.
  - `pointId` (int32): Unique identifier of the point.

- **Responses**:
  - `200 OK`: Successfully deleted the point.

## Schemas

### **PointViewModel**

Represents a point with x and y coordinates.

- **Properties**:
  - `x` (int32): X-coordinate of the point.
  - `y` (int32): Y-coordinate of the point.

- **Additional Properties**: Not allowed.

---

### **RegisterViewModel**

Represents the data required to register a new user.

- **Properties**:
  - `name` (string, nullable): Name of the user.

- **Additional Properties**: Not allowed.

## Notes

- The system was created using .NET 9.

- I chose MS SQL Server as the database due to its integration with Visual Studio. However, if the system remains unchanged in the future, a NoSQL database could be considered instead, as it might yield faster results when handling large amounts of data.

- I decided to create a single project (API) because the system's scope is very small. Separate projects for business logic, API, persistence, etc., would only complicate development. If you'd like to see a system I’ve created with separate projects (or random API created for training), you can check them out here:
    - [GitHub - Car Parts Shop API (Clean Architecture, Multiple Projects, MediatR, Global Exception Handling, Fluent Validation)](https://github.com/devJUKI/car-parts-shop-api-clean-architecture)
    - [GitHub - Events Management API (Minimal APIs, Result Pattern, Fluent Validation)](https://github.com/devJUKI/event-management-api)

- I opted not to use libraries like MediatR or FluentValidation, etc. for the same reason – the small scope of the system.

- I chose GUID instead of incremental integer for the user ID for security purposes (pretending that system has authorization).

- I used a global exception handling middleware since it is convenient and easy to work with. It keeps the code cleaner compared to using the Result pattern. Additionally, global exception handling allows easy logging integration.

- Normally, I move SaveChangesAsync for DbContext to a separate function to perform multiple operations before saving. However, in this system, since only one operation needs to be performed at a time, I save changes immediately.

- Since the requirements allowed the use of a square algorithm found online, I didn’t investigate it's efficiency and used it as-is. This means the non-functional 5-second requirement might be violated with a large number of points if algorithm is inefficient.

- The .env file should be added to .gitignore, but I left it in to make it easier to run the system without additional configuration.

- I don’t have a lot of experience with SLI monitoring. Most of my experience has been working with application or message broker logs using Elasticsearch. Therefore, I tried integrating OpenTelemetry with Prometheus (and Grafana) since I found information online that Prometheus can be used for SLI measurement. I’m aware that custom metrics can be created in the code itself, but I ran out of time. As a result, the system currently only has basic metrics obtained from several OpenTelemetry packages.