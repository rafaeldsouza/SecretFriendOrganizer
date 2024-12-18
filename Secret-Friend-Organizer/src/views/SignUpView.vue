<template>
  <div class="signup-container">
    <h1>Sign Up</h1>
    <form @submit.prevent="handleSignUp">
      <div class="form-group">
        <input id="username" v-model="username" type="text" required placeholder="Enter your username" />
      </div>
      <div class="form-group">
        <input id="email" v-model="email" type="email" required placeholder="Enter your email" />
      </div>
      <div class="form-group">
        <input id="password" v-model="password" type="password" required placeholder="Enter your password" />
      </div>
      <button type="submit">Sign Up</button>
    </form>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue';
import api from '../services/api'; // Usar o serviço de API

export default defineComponent({
  name: 'SignUpView',
  setup() {
    const username = ref('');
    const email = ref('');
    const password = ref('');

    const handleSignUp = async () => {
      try {
        const response = await api.post('/User/create', {
          username: username.value,
          email: email.value,
          password: password.value,
        });

        console.log('Sign Up Successful:', response.data);
        alert('Sign Up Successful');
        // Redirecionar para a página de login
        window.location.href = '/login';
      } catch (error) {
        console.error('Sign Up Failed:', error.message || error);
        alert('Sign Up Failed: ' + (error.message || 'Unknown error'));
      }
    };

    return {
      username,
      email,
      password,
      handleSignUp,
    };
  },
});
</script>

<style src="/src/assets/styles/signup.css"></style>
