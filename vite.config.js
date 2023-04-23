import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
import mkcert from "vite-plugin-mkcert";

export default defineConfig({
  plugins: [react(), mkcert()],
  server: {
    https: true,
    port: "8080",
  },
  root: "./src",
  build: {
    outDir: "../dist",
  },
});
