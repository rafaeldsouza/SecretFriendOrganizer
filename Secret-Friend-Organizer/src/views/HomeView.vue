<template>
  <div class="home">
    <div class="overlay">
      <h1>Secret Friend Organizer</h1>
      <p>
        Welcome to Secret Friend Organizer! Organize your gift exchange events effortlessly. 
        Create groups, invite friends, and draw your secret friend.
      </p>
      <div class="actions">
        <router-link to="/login" class="button">Login</router-link>
        <router-link to="/signup" class="button">Sign Up</router-link>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';

export default defineComponent({
  name: 'HomeView',
});
</script>

<style>
.home {
  background-image: url('/confraternizacao.gif');
  background-size: cover;
  background-position: center;
  height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
  position: relative;
}

.overlay {
  background-color: rgba(0, 0, 0, 0.5); /* Adiciona um filtro escuro para destacar o texto */
  color: white;
  padding: 20px;
  text-align: center;
  border-radius: 10px;
}

h1 {
  font-size: 2.5rem;
  margin-bottom: 1rem;
}

p {
  font-size: 1.25rem;
  margin-bottom: 1.5rem;
}

.actions {
  display: flex;
  justify-content: center;
  gap: 20px;
}

.button {
  background-color: #42b983;
  color: white;
  padding: 10px 20px;
  text-decoration: none;
  border-radius: 5px;
  font-size: 16px;
  transition: background-color 0.3s;
}

.button:hover {
  background-color: #36a073;
}
</style>
