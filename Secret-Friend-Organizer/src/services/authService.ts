import api from './api';

class AuthService {
  public async login(username: string, password: string): Promise<void> {
    try {
      const response = await api.post('/User/authenticate', {
        username,
        password
      });

      const data = response.data;
      localStorage.setItem('accessToken', data.data.access_token);
      localStorage.setItem('refreshToken', data.data.refresh_token);
      localStorage.setItem('username', data.data.username);
      localStorage.setItem('userId', data.data.user_id);
      localStorage.setItem('email', data.data.email);
      
      const expiresIn = data.data.expires_in; 
      const expirationDate = new Date().getTime() + expiresIn * 1000;
      localStorage.setItem('tokenExpiration', expirationDate.toString());
    } catch (error) {
      console.error('Login failed:', error.message || error);
    }
  }

  async refreshToken(): Promise<boolean> {
    const refreshToken = localStorage.getItem('refreshToken');
    if (!refreshToken) {
      this.logout();
      return false;
    }

    try {
      const response = await api.post(`/User/refreshtoken`, { refreshToken });

      const { accessToken, refreshToken: newRefreshToken, expiresIn } = response.data;

      localStorage.setItem('accessToken', accessToken);
      localStorage.setItem('refreshToken', newRefreshToken);

      const expirationDate = new Date().getTime() + expiresIn * 1000; 
      localStorage.setItem('tokenExpiration', expirationDate.toString());

      return true;
    } catch (error) {
      console.error('Failed to refresh token:', error);
      this.logout();
      return false;
    }
  }

  public logout(): void {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('username');
    localStorage.removeItem('userId');
    localStorage.removeItem('email');
    localStorage.removeItem('tokenExpiration');
  }

  async isAuthenticated(): Promise<boolean> {
    const token = localStorage.getItem('accessToken');
    const expirationDate = localStorage.getItem('tokenExpiration');
  
    if (!token || !expirationDate) {
      return false;
    }
  
    const now = new Date().getTime();
  
    if (now >= parseInt(expirationDate)) {
      const refreshed = await this.refreshToken();
      return refreshed; 
    }
  
    return true; 
  }

  public getAccessToken(): string | null {
    return localStorage.getItem('accessToken');
  }
}

export default new AuthService();
