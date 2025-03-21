<script setup lang="ts">
import { ref } from 'vue';
import { useTodoList } from '@/composables/useTodoList';

const todo = ref<string | undefind>();
const isEdit = ref(false);
const { todoList, add, show, edit, del} = useTodoList();

const addTodo = () => {
    if(!todo.value) return;
    add(todo.value);
    todo.value = '';
}

const showTodo = (id: number) => {
    todo.value = show(id);
    if(todo.value){
        isEdit.value = true;
    }
};

const editTodo = () => {
    if(!todo.value) return;
    edit(todo.value);
    isEdit.value = false;
    todo.value = '';
};

const deleteTodo = (id: number) => {
    isEdit.value = false;
    del(id);
}
</script>

<template>
    <div>
        <input type="text" class="todo_input" v-model="todo" placeholder="+ TODOを入力">
        <button class="btn green" @click="editTodo" v-show="isEdit">変更</button>
        <button class="btn" @click="addTodo" v-show="!isEdit">追加</button>
    </div>
    <div class="box_list">
        <div class="todo_list" v-for="todo in todoList" :key="todo.id">
            <div class="todo">
                <input type="checkbox" class="check" />
                <label>{{ todo.task }}</label>
            </div>
            <div class="btns">
                <button class="btn green" @click="showTodo(todo.id)">編</button>
                <button class="btn pink" @click="deleteTodo(todo.id)">削</button>
            </div>
        </div>
    </div>
</template>

<style scoped>
.box_list {
    display: flex;
    flex-direction: column;
    gap: 4px;
    margin-top: 20px;
}

.todo_list {
    display: flex;
    gap: 8px;
    align-items: center;
}

.todo {
    width: 250px;
    padding: 6px 8px;
    border: 1px solid #ccc;
    border-radius: 6px;
}

.check {
    margin-right: 12px;
    transform: scale(1.6);
}

.btns {
    display: flex;
    gap: 4px;
}

.todo_input {
    width: 250px;
    padding: 6px 8px;
    margin-right: 8px;
    font-size: 18px;
    border: 1px solid #aaa;
    border-radius: 6px;
}
.btn {
    position: relative;
    padding: 6px 8px;
    font-size: 14px;
    color: #fff;
    text-align: centers;
    background-color: #03a9f4;
    border: 1px solid #eee;
    border-radius: 6px;
}

.btn:active {
    top: 1px;
}

.green {
    background-color: #00c853;
}

.pink {
    background-color: #ff4081;
}
</style>