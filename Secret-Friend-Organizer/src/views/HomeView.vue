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
<style src="../assets/styles/home.css"></style>