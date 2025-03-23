<script setup lang="ts">
import { onMounted, ref } from 'vue';

export type PostResponse = {
    useId: number;
    id: number;
    title: string;
    body: string;
}

const posts = ref<PostResponse[]>([]);
const fetchDate = async () => {
    try{
        const response = await fetch('https://jsonplaceholder.typicode.com/posts');
        posts.value = await response.json();
    } catch(error){
        console.error('Error fetching data', error);
    }
};

onMounted(fetchDate);
</script>

<template>
    <ul>
        <li v-for="post in posts" :key="post.id">
            {{ post.id }}
            <router-link :to="`/blog/${post.id}`">{{ post.title }}</router-link>
        </li>
    </ul>
</template>

<style scoped>
ul {
    margin-top: 12px;
}

li {
    padding: 8px;
    margin-bottom: 8px;
    list-style: none;
    border: 1px solid #ccc;
}

li:hover {
    background-color: #eee;
}

a {
    color: #333;
    list-style: none;
}
</style>