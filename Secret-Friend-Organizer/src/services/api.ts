import axios from 'axios';

const api = axios.create({
  baseURL: 'https://localhost:7031/api', // URL do backend
});

// Interceptor to add the Authorization header
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('accessToken');
    if (token) {
      config.headers['Authorization'] = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

export default api;
