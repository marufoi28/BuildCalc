import { computed, ref } from 'vue';

export const useTodoList = () => {
    const todoList = ref<{ id: number; task: string; checked: boolean}[]>([]);
    const ls = localStorage.todoList;

    todoList.value = ls ? JSON.parse(ls) : [];

    const findById = (id: number) => {
        return todoList.value.find((todo) => todo.id === id);
    };

    const findIndexById = (id: number) => {
        return todoList.value.findIndex((todo) => todo.id === id);
    }

    const editId = ref(-1);

    const show = (id: number) => {
        const todo = findById(id);
        editId.value = id;
        return todo?.task;
    }

    const edit = (task: string) => {
        const todo = findById(editId.value);
        const idx = findIndexById(editId.value);

        if(todo){
            todo.task = task;
            todoList.value.splice(idx, 1, todo);
            localStorage.todoList = JSON.stringify(todoList.value);
            editId.value = -1;
        }
    };

    const del = (id: number) => {
        const todo = findById(id);
        if(todo){
            const delMsg = '「' + todo.task + '」を削除しますか？';
            if(!confirm(delMsg)) return;

            const idx = findIndexById(id);
            todoList.value.splice(idx, 1);
            localStorage.todoList = JSON.stringify(todoList.value);
        }
    }

    const add = (task: string) => {
        const id = new Date().getTime();

        todoList.value.push({ id: id, task: task, checked: false })

        localStorage.todoList = JSON.stringify(todoList.value);
    }

    const check = (id: number) => {
        const todo = findById(id);
        const idx = findIndexById(id);

        if(todo){
            todo.checked = !todo.checked;
            todoList.value.splice(idx, 1, todo);
            localStorage.todoList = JSON.stringify(todoList.value);
        }
    };

    const countFin = computed(() => {
        console.log('computed');
        const finArr = todoList.value.filter((todo) => todo.checked);
        return finArr.length;
    })

    return {todoList, add, show, edit, del, check, countFin};
}