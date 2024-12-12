# Secret Friend Organizer Frontend

The **Secret Friend Organizer Frontend** is the user interface for managing Secret Friend (Secret Santa) groups. Built with **Vue 3** and **TypeScript**, it provides an intuitive way to create groups, join existing groups, and manage group draws. The application is fully integrated with **Keycloak** for secure authentication and authorization.

## **Features**
- **Authentication**:
  - Integrated with **Keycloak** using JWT tokens.
  - Protects routes and ensures secure user sessions.
- **Group Management**:
  - View available groups and join them with gift recommendations.
  - Manage group membership and view assigned Secret Friends after a draw.
- **Routing**:
  - Organized navigation between views such as Login, Sign Up, Group Details, and Join Group.
- **API Integration**:
  - Centralized API calls using Axios with JWT token handling.

## **Technologies Used**
- **Vue 3**: Progressive JavaScript framework for building the user interface.
- **TypeScript**: Static typing for safer and more robust code.
- **Axios**: HTTP client for API communication, configured with interceptors for token management.
- **Vue Router**: Client-side routing for navigation between views.
- **Keycloak**: Identity and Access Management (IAM) for secure authentication and role management.
- **Vite**: Fast and modern build tool for frontend projects.

## **Project Setup**
### **Requirements**
- Backend API should be running and accessible.
- Keycloak should be configured with the correct realm and client.

### **Steps to Run**
1. **Clone the Repository**:
   \`\`\`bash
   git clone https://github.com/rafaeldsouza/SecretFriendOrganizer.git
   cd Secret-Friend-Organizer
   \`\`\`

2. **Install Dependencies**:
   \`\`\`bash
   npm install
   \`\`\`

3. **Run the Development Server**:
   \`\`\`bash
   npm run dev
   \`\`\`

4. **Access the Application**:
   - The frontend will be accessible at \`http://localhost:5173\` (default Vite port).

5. **Build for Production**:
   To create a production-ready build:
   \`\`\`bash
   npm run build
   \`\`\`

## **Key Features**
### **Authentication**
- Integrated with **Keycloak** for user authentication and session management.
- Routes are protected, ensuring only authenticated users can access certain views.

### **Routing**
- **Vue Router** manages navigation across:
  - **Login** and **Sign Up** pages.
  - User Group List.
  - Group Details for managing and viewing draws.

### **API Requests**
- Centralized in \`src/services/api.ts\` using **Axios**.
- Includes interceptors for appending JWT tokens to each request automatically.

### **User Interface**
- Basic but functional interface demonstrating the app’s core features.
- Forms for user actions (e.g., login, sign-up, join group).
- Grids and lists to display groups and their details.

## **Areas for Improvement**
### **Token Management**
- Implement **refresh token** logic to ensure seamless session renewal.

### **UI/UX Enhancements**
- Improve layout and add visual elements for a polished user experience.
- Incorporate responsive design for mobile compatibility.

### **Error Handling**
- Add comprehensive error handling for API calls.
- Display user-friendly error messages and feedback.

### **Testing**
- Implement **unit tests** for components and services to ensure reliability.
- Add **end-to-end (E2E)** tests using tools like Cypress.

## **Backend Integration**
- The backend is built with **.NET Core** and integrates **Keycloak** for authentication.
- The API handles group and user management.
- Refer to the backend repository for detailed setup and documentation.

## **Conclusion**
The **Secret Friend Organizer Frontend** demonstrates a solid implementation of modern frontend technologies and integration with a secure authentication system. While functional, it provides room for enhancements in design, user experience, and testing, making it a great foundation for further development.


## **Contact**
For questions or support, please reach out to:

Name: Rafael Conceição \
Email: rafaeldsouza@gmail.com \
GitHub: https://github.com/rafaeldsouza/SecretFriendOrganizer
