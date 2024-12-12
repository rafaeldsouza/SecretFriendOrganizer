# Secret Friend Organizer

**Secret Friend Organizer** is a modern application designed to simplify the process of organizing Secret Friend (Secret Santa) events. It combines a robust backend with a user-friendly frontend to deliver a seamless experience for creating groups, adding members, managing gift recommendations, and performing random draws.

## **Project Overview**
### **Backend**
- Built with **.NET Core 8**.
- Manages user authentication, group functionality, and business logic.
- Integrated with **Keycloak** for secure authentication and authorization.
- Uses **PostgreSQL** as the database and **Entity Framework Core** for ORM.
- Logging implemented with **Serilog** (currently outputs to console).

### **Frontend**
- Built with **Vue 3** and **TypeScript**.
- Provides a dynamic and responsive user interface.
- Implements **JWT-based authentication** to interact securely with the backend.
- Manages routing with **Vue Router** and API communication with **Axios**.

## **Technologies Used**
| **Technology**      | **Purpose**                               |
|----------------------|-------------------------------------------|
| .NET Core 8          | Backend framework                        |
| Vue.js 3             | Frontend framework                       |
| TypeScript           | Ensures type safety                      |
| PostgreSQL           | Relational database                      |
| Keycloak             | Identity and Access Management           |
| Entity Framework Core| ORM for database interactions            |
| Docker               | Containerization                        |
| Docker Compose       | Orchestration of services               |
| Serilog              | Logging                                  |

## **Project Structure**
The repository is divided into two main parts:
1. **\`Secret-Friend-Organizer\`** (Frontend):
   - Located in the \`Secret-Friend-Organizer\` directory.
   - Provides the user interface for interacting with the application.

2. **\`SecretFriendOrganizerApi\`** (Backend):
   - Located in the \`SecretFriendOrganizerApi\` directory.
   - Contains the API for managing the application's business logic and data.

## **Setup Instructions**
### **Prerequisites**
- Docker and Docker Compose installed.
- Node.js and npm installed for frontend development (optional for production).
- Keycloak configured with a \`SecretFriend\` realm and appropriate client.

### **Steps to Run**
1. **Clone the Repository**:
   \`\`\`bash
   git clone https://github.com/rafaeldsouza/SecretFriendOrganizer.git
   cd SecretFriendOrganizer
   \`\`\`

2. **Start All Services**:
   Use Docker Compose to start the backend, frontend, and Keycloak services:
   \`\`\`bash
   docker-compose up --build
   \`\`\`

3. **Access the Application**:
   - **Frontend**: \`http://localhost:5173\`
   - **Backend**: \`https://localhost:7031\`
   - **Keycloak**: \`http://localhost:8080\`

4. **Additional Steps**:
   - Run database migrations for the backend:
     \`\`\`bash
     dotnet ef database update --project SecretFriendOrganizerApi/SecretFriendOrganizer.Infrastructure --startup-project SecretFriendOrganizerApi/SecretFriendOrganizer.Api
     \`\`\`

## **Key Features**
### **Backend**
- **Authentication**: Secure user authentication with **JWT** tokens and **Keycloak**.
- **Group Management**: Create groups, add members, and manage Secret Friend draws.
- **Logging**: Centralized logging with **Serilog**.

### **Frontend**
- **User Interface**: Responsive design for managing groups and viewing results.
- **Secure Communication**: Uses JWT tokens for API calls.
- **User Actions**: Sign up, login, join groups, and manage gift recommendations.

## **Areas for Improvement**
While the application is functional, several enhancements can be made:
- **Logging**: Integrate centralized logging systems like ELK Stack or Seq.
- **Refresh Tokens**: Implement a refresh token mechanism for seamless session renewal.
- **UI/UX Enhancements**: Improve the frontend design for better user experience.
- **Testing**: Expand unit and integration test coverage for both frontend and backend.
- **Localization**: Add support for multiple languages to make the app globally accessible.

## **License**
This project is licensed under the MIT License.

## **Acknowledgments**
Special thanks to the contributors and the open-source community for providing tools and libraries that power this application.

## **Contact**
For questions or support, please reach out to:

Name: Rafael Conceição
Email: rafaeldsouza@gmail.com
GitHub: https://github.com/rafaeldsouza/SecretFriendOrganizer


