<template>
    <div>
      <h1>Join Group</h1>
      <form @submit.prevent="handleJoinGroup">
        <div class="form-group">
          <label for="groupId">Group</label>
          <select id="groupId" v-model="groupId" required>
            <option value="">Select a Group</option>
            <option v-for="group in validGroups" :key="group.groupId" :value="group.groupId">
              {{ group.groupName }} - {{ group.description }}
            </option>
          </select>
        </div>
        <div class="form-group">
          <label for="giftRecommendation">Gift Recommendation</label>
          <input id="giftRecommendation" v-model="giftRecommendation" type="text" placeholder="Enter Gift Recommendation" />
        </div>
        <button type="submit">Join Group</button>
      </form>
    </div>
  </template>
  
  <script lang="ts">
  import { defineComponent, ref, onMounted } from 'vue';
  import api from '../services/api';
  import { ValidGroupDto } from '../types/ValidGroupDto';
  
  export default defineComponent({
    name: 'JoinGroupView',
    setup() {
      const groupId = ref('');
      const giftRecommendation = ref('');
      const validGroups = ref<ValidGroupDto[]>([]);
  
      const fetchValidGroups = async () => {
        try {
          const response = await api.get('/Group/valid-groups');
          validGroups.value = response.data.groups;
        } catch (error) {
          console.error('Failed to fetch valid groups:', error.message || error);
        }
      };
  
      const handleJoinGroup = async () => {
        try {
          const userId = localStorage.getItem('userId');
          const response = await api.post('/Group/join-group', {
            groupId: groupId.value,
            userId,
            giftRecommendation: giftRecommendation.value,
          });
  
          console.log('Joined group successfully:', response.data);
          alert('Joined group successfully');
        } catch (error) {
          console.error('Failed to join group:', error.message || error);
          alert('Failed to join group');
        }
      };
  
      onMounted(fetchValidGroups);
  
      return {
        groupId,
        giftRecommendation,
        validGroups,
        handleJoinGroup,
      };
    },
  });
  </script>
  
  <style scoped>
  .form-group {
    margin-bottom: 16px;
  }
  button {
    padding: 10px 20px;
    border: none;
    border-radius: 4px;
    background-color: #007bff;
    color: white;
    cursor: pointer;
  }
  </style>
  