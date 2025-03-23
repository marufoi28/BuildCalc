<script setup lang="ts">
import BaseButton from '@/components/BaseButton.vue';

const emit = defineEmits<{
    'add-click': [void];
}>();
</script>

<template>
    <BaseButton color="blue" @on-click="emit('add-click')">追加</BaseButton>
</template>