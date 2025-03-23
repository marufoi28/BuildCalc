<script setup lang="ts">
type Props = {
    color: 'blue' | 'green' | 'pink';
}

type Emits = {
    'on-click': [ str: string, id: number];
}
const props = defineProps<Props>();
const emit = defineEmits<Emits>();

const handleClick = (str: string, id: number) => {
    emit('on-click', str, id);
}
</script>

<template>
    <button class="btn" :class="color" @click="handleClick('あああ', 123)"><slot /></button>
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