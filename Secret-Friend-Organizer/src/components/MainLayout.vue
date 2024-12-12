<template>
    <div>
      <nav>
        <ul>
          <li><router-link to="/">Home</router-link></li>
          <li><router-link to="/create-group">Create Group</router-link></li>
          <li><router-link to="/user-groups">My Groups</router-link></li>
          <li><router-link to="/join-group">Join Group</router-link></li>
          <li><button @click="handleLogout">Logout</button></li>
        </ul>
      </nav>
      <main>
        <router-view></router-view>
      </main>
    </div>
  </template>
  
  <script lang="ts">
  import { defineComponent } from 'vue';
  import authService from '../services/authService';
  
  export default defineComponent({
    name: 'MainLayout',
    setup() {
      const handleLogout = () => {
        authService.logout();
        window.location.href = '/login';
      };
  
      return {
        handleLogout,
      };
    },
  });
  </script>
  
  <style scoped>
  nav ul {
    list-style: none;
    padding: 0;
    display: flex;
    gap: 10px;
  }
  nav ul li {
    display: inline;
  }
  nav ul li button {
    background: none;
    border: none;
    cursor: pointer;
    color: blue;
    text-decoration: underline;
  }
  </style>
  