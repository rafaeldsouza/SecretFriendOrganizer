<template>
    <div>
      <h1>My Groups</h1>
      <table v-if="!isLoading">
        <thead>
          <tr>
            <th>Group Name</th>
            <th>Is Drawn</th>
            <th>Assigned Friend</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="group in groups" :key="group.groupId">
            <td>{{ group.groupName }}</td>
            <td>{{ group.isDrawn ? 'Yes' : 'No' }}</td>
            <td>
              <div v-if="group.assignedFriend">
                {{ group.assignedFriend }}
              </div>
              <div v-else-if="group.isDrawn">
                <button @click="fetchAssignedFriend(group.groupId)">View Assigned Friend</button>
              </div>
            </td>
            <td>
              <button v-if="group.isAdmin && !group.isDrawn" @click="performDraw(group.groupId)">Perform Draw</button>
            </td>
          </tr>
        </tbody>
      </table>
      <p v-else>Loading...</p>
    </div>
  </template>
  
  <script lang="ts">
  import { defineComponent, ref, onMounted } from 'vue';
  import api from '../services/api';
  import { UserGroupDto } from '../types/UserGroupDto';
  
  export default defineComponent({
    name: 'UserGroupsView',
    setup() {
      const groups = ref<(UserGroupDto & { assignedFriend?: string })[]>([]);
      const isLoading = ref(true);
  
      const fetchGroups = async () => {
        try {
          const userId = localStorage.getItem('userId');
          const response = await api.get(`/Group/user-groups/${userId}`);
          groups.value = response.data.groups.map((group: UserGroupDto) => ({
            ...group,
            assignedFriend: null
          }));
        } catch (error) {
          console.error('Failed to fetch groups:', error.message || error);
        } finally {
          isLoading.value = false;
        }
      };
  
      const fetchAssignedFriend = async (groupId: string) => {
        try {
          const userId = localStorage.getItem('userId');
          const response = await api.get(`/Group/assigned-friend/${userId}/${groupId}`);
          const group = groups.value.find(g => g.groupId === groupId);
          if (group) {
            group.assignedFriend = response.data.assignedFriend;
          }
        } catch (error) {
          console.error('Failed to fetch assigned friend:', error.message || error);
        }
      };
  
      const performDraw = async (groupId: string) => {
        try {
          const adminUserId = localStorage.getItem('userId');
          await api.post(`/Group/perform-draw/${groupId}`, { adminUserId });
          alert('Draw performed successfully');
          fetchGroups(); // Atualizar a lista de grupos após realizar o sorteio
        } catch (error) {
          console.error('Failed to perform draw:', error.message || error);
        }
      };
  
      onMounted(fetchGroups);
  
      return {
        groups,
        isLoading,
        fetchAssignedFriend,
        performDraw,
      };
    },
  });
  </script>
  
  <style scoped>
  table {
    width: 100%;
    border-collapse: collapse;
    margin-bottom: 20px;
  }
  thead th {
    background-color: #f0f0f0;
    padding: 10px;
    text-align: left;
  }
  tbody td {
    padding: 10px;
    border-bottom: 1px solid #ddd;
  }
  button {
    padding: 5px 10px;
    border: none;
    border-radius: 4px;
    background-color: #007bff;
    color: white;
    cursor: pointer;
  }
  button:disabled {
    background-color: #ccc;
    cursor: not-allowed;
  }
  </style>
  