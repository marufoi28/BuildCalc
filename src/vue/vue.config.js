const { defineConfig } = require("@vue/cli-service");
module.exports = defineConfig({
  lintOnSave: false,
  devServer: {
    port: 8888,
    proxy: {
      "/ws": {
        target: "ws://localhost:8080",
        ws: true,
        changeOrigin: true,
      },
    },
  },
});
