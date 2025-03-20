// vue/src/App.vue
<template>
  <div id="app">
    <header>
      <h1>Vue + .NET API Demo</h1>
    </header>
    <main>
      <ApiDemo />
    </main>
  </div>
</template>

<script>
import ApiDemo from "./components/ApiDemo.vue";

export default {
  name: "App",
  components: {
    ApiDemo,
  },
};
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
