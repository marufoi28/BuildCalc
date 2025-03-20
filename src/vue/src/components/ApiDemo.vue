// Vue.js側のコード (src/components/ApiDemo.vue)

<template>
  <div class="api-demo">
    <h1>Weather Forecast</h1>

    <div class="controls">
      <button @click="fetchData" :disabled="loading">
        {{ loading ? "Loading..." : "Get Forecast" }}
      </button>
    </div>

    <div v-if="error" class="error">
      {{ error }}
    </div>

    <div v-if="forecasts.length > 0" class="results">
      <table>
        <thead>
          <tr>
            <th>Date</th>
            <th>Temperature (°C)</th>
            <th>Summary</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="forecast in forecasts" :key="forecast.date">
            <td>{{ formatDate(forecast.date) }}</td>
            <td>{{ forecast.temperatureC }}</td>
            <td>{{ forecast.summary }}</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script>
export default {
  name: "ApiDemo",
  data() {
    return {
      forecasts: [],
      loading: false,
      error: null,
    };
  },
  methods: {
    async fetchData() {
      this.loading = true;
      this.error = null;

      try {
        // APIのURLを指定 - 開発環境と本番環境で変わる可能性があります
        const apiUrl =
          process.env.NODE_ENV === "development"
            ? "http://localhost:8080/WeatherForecast"
            : "https://dotnetvue-630537362311.us-central1.run.app/weatherforest";

        const response = await fetch(apiUrl);

        if (!response.ok) {
          throw new Error(`API returned status: ${response.status}`);
        }

        this.forecasts = await response.json();
      } catch (err) {
        console.error("Error fetching data:", err);
        this.error = `Failed to fetch data: ${err.message}`;
      } finally {
        this.loading = false;
      }
    },
    formatDate(dateString) {
      const date = new Date(dateString);
      return date.toLocaleDateString();
    },
  },
};
</script>

<style scoped>
.api-demo {
  max-width: 800px;
  margin: 0 auto;
  padding: 20px;
}

.controls {
  margin: 20px 0;
}

button {
  padding: 10px 20px;
  background-color: #42b983;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

button:disabled {
  background-color: #ccc;
  cursor: not-allowed;
}

.error {
  color: #ff4444;
  margin: 20px 0;
  padding: 10px;
  border: 1px solid #ff4444;
  border-radius: 4px;
}

table {
  width: 100%;
  border-collapse: collapse;
}

th,
td {
  padding: 12px;
  text-align: left;
  border-bottom: 1px solid #ddd;
}

th {
  background-color: #f2f2f2;
}
</style>
