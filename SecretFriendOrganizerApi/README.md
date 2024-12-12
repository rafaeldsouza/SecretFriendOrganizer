# Secret Friend Organizer API

**Secret Friend Organizer API** is the backend service for managing the Secret Friend Organizer application. It provides endpoints for user management, group management, and Secret Friend (Secret Santa) draw functionality. This API integrates seamlessly with **Keycloak** for secure authentication and authorization.

## **Features**
- **Authentication and Authorization**:
  - Powered by **Keycloak**.
  - Secure token-based authentication (JWT).
- **Group Management**:
  - Create, update, and manage groups.
  - Add members with gift recommendations.
  - Perform draws to assign Secret Friends.
- **Layered Architecture**:
  - Clean and modular codebase for maintainability.
- **Logs**:
  - Structured logging using **Serilog**.

## **Technologies Used**
- **.NET Core 8**: API framework.
- **Entity Framework Core**: ORM for database interactions.
- **PostgreSQL**: Relational database.
- **Keycloak**: Identity and Access Management (IAM) for authentication and authorization.
- **Serilog**: Logging framework for structured logs.
- **Docker**: Containerization for simplified deployment.
- **Docker Compose**: Orchestration for API, PostgreSQL, and Keycloak.

## **Project Structure**
The backend follows a layered architecture to separate concerns:
- **\`SecretFriendOrganizer.Api\`**: Hosts the API endpoints.
- **\`SecretFriendOrganizer.Application\`**: Contains business logic, DTOs, and service interfaces.
- **\`SecretFriendOrganizer.Domain\`**: Defines the core domain models and business rules.
- **\`SecretFriendOrganizer.Infrastructure\`**: Manages database context, repositories, and Keycloak integration.
- **\`SecretFriendOrganizer.Tests\`**: Unit and integration tests.

## **Setup and Usage**
### **Requirements**
- **Docker** and **Docker Compose** installed.
- **Keycloak** configured with a \`SecretFriend\` realm.

### **Steps to Run**
1. **Clone the Repository**:
   \`\`\`bash
   git clone https://github.com/rafaeldsouza/SecretFriendOrganizer.git
   cd secret-friend-organizer-api
   \`\`\`

2. **Start the Application**:
   \`\`\`bash
   docker-compose up --build
   \`\`\`

3. **Access the Application**:
   - **API**: \`https://localhost:7031\`
   - **Keycloak Admin Console**: \`http://localhost:8080\`

4. **Run Migrations**:
   Initialize the database schema using Entity Framework Core:
   \`\`\`bash
   dotnet ef database update --project SecretFriendOrganizer.Infrastructure --startup-project SecretFriendOrganizer.Api
   \`\`\`

## **Key Features**
### **Authentication**
- Integrates with **Keycloak** for secure user authentication and authorization.
- Uses **JWT (JSON Web Tokens)** for API requests.

### **Group Management**
- **Create Groups**: Allows users to create groups and become administrators.
- **Add Members**: Users can join groups and suggest gifts.
- **Perform Draws**: The group administrator can assign Secret Friends randomly.

### **Logging**
- Logs are managed using **Serilog**.
- Currently outputs logs to the console.

## **Areas for Improvement**
### **Logging**
- Integrate with centralized logging systems like:
  - **Elasticsearch + Kibana (ELK Stack)**.
  - **Seq**.
  - **AWS CloudWatch**.
  - **Azure Application Insights**.

### **Token Management**
- Implement **refresh tokens** for seamless token renewal without reauthentication.

### **Testing**
- Expand test coverage, including:
  - **Unit Tests**: For all service methods.
  - **Integration Tests**: For Keycloak and database interactions.

### **Error Handling**
- Add middleware for unified error handling.
- Implement structured error responses.

## **Contributing**
Contributions are welcome! Please fork the repository, create a new branch, and submit a pull request.

## **License**
This project is licensed under the MIT License.

## **Contact**
For questions or support, please reach out to:

Name: Rafael Conceição \
Email: rafaeldsouza@gmail.com \
GitHub: https://github.com/rafaeldsouza/SecretFriendOrganizer
