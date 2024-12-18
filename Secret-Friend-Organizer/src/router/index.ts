import { createRouter, createWebHistory } from "vue-router";
import HomeView from "../views/HomeView.vue";
import LoginView from "../views/LoginView.vue";
import SignUpView from "../views/SignUpView.vue";
import CreateGroupView from "../views/CreateGroupView.vue";
import MainLayout from "../components/MainLayout.vue";
import authService from "../services/authService";

const routes = [
  {
    path: '/',
    component: MainLayout,
    children: [    
      { path: 'create-group', name: 'CreateGroupView', component: CreateGroupView, meta: { requiresAuth: true } },
      { path: 'user-groups', name: 'UserGroupsView', component: () => import('../views/UserGroupsView.vue'), meta: { requiresAuth: true } },
      { path: 'join-group', name: 'JoinGroupView', component: () => import('../views/JoinGroupView.vue'), meta: { requiresAuth: true } }
    ]
  },
  { path: '', name: 'HomeView', component: HomeView },
  { path: '/login', name: 'LoginView', component: LoginView },
  { path: '/signup', name: 'SignUpView', component: SignUpView }
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL || '/'),
  routes,
});

router.beforeEach(async (to, from, next) => {
  if (to.matched.some(record => record.meta.requiresAuth)) {
    const isAuthenticated = await authService.isAuthenticated();
    if (!isAuthenticated) {
      return next({ name: 'LoginView' });
    }
  }
  next(); 
});

export default router;