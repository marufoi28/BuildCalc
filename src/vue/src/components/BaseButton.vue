<script setup lang="ts">
type Props = {
    color: string;
}

type Emits = {
    'on-click': [void];
}
const props = defineProps<Props>();
const emit = defineEmits<Emits>();

const handleClick = () => {
    emit('on-click');
}
</script>

<template>
    <button class="btn" :class="color" @click="handleClick"><slot /></button>
</template>

<style scoped>
.btn {
    padding: 8px;
    font-size: 14px;
    color: #fff;
    text-align: center;
    border-radius: 6px;
}

.green {
    background-color: #00c853;
}

.blue {
    background-color: #03a9f4;
}

.pink {
    background-color: #dd4081;
}
</style>