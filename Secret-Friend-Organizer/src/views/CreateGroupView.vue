<template>
  <div class="create-group-container">
    <h1>Create Group</h1>
    <form @submit.prevent="handleCreateGroup">
      <div class="form-group">
        <input v-model="groupName" type="text" required placeholder="Group Name" />
      </div>
      <div class="form-group">
        <textarea v-model="description" placeholder="Description"></textarea>
      </div>
      <button type="submit">Create Group</button>
    </form>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue';
import api from '../services/api';

export default defineComponent({
  name: 'CreateGroupView',
  setup() {
    const groupName = ref('');
    const description = ref('');

    const handleCreateGroup = async () => {
      try {
        const creatorUserId = localStorage.getItem('userId');
        const response = await api.post('/Group/create', {
          groupName: groupName.value,
          description: description.value,
          creatorUserId,
        });

        console.log('Group created successfully:', response.data);
        alert('Group created successfully');
      } catch (error) {
        console.error('Failed to create group:', error.message || error);
        alert('Failed to create group');
      }
    };

    return {
      groupName,
      description,
      handleCreateGroup,
    };
  },
});
</script>

<style scoped>
.create-group-container {
  max-width: 600px;
  margin: 0 auto;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}
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
