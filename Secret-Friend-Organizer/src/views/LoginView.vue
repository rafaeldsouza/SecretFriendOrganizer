<template>
  <div class="login-container">
    <h1>Login</h1>
    <form @submit.prevent="handleLogin">
      <div class="form-group">
        <input id="username" v-model="username" type="text" required placeholder="Enter your username" />
      </div>
      <div class="form-group">
        <input id="password" v-model="password" type="password" required placeholder="Enter your password" />
      </div>
      <button type="submit">Login</button>
    </form>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue';
import { useRouter } from 'vue-router'; // Importar o useRouter
import authService from '../services/authService'; // Importar o serviço de autenticação

export default defineComponent({
  name: 'LoginView',
  setup() {
    const username = ref('');
    const password = ref('');
    const router = useRouter(); // Usar o router

    const handleLogin = async () => {
      try {
        await authService.login(username.value, password.value);
        if (authService.isAuthenticated()) {
          console.log('Login Successful');
          alert('Login Successful');
          router.push({ name: 'HomeView' }); // Redirecionar para a Home
        } else {
          alert('Login Failed');
        }
      } catch (error) {
        console.error('Login Failed:', error.message || error);
        alert('Login Failed: ' + (error.message || 'Unknown error'));
      }
    };

    return {
      username,
      password,
      handleLogin,
    };
  },
});
</script>

<style src="../assets/styles/login.css"></style>
