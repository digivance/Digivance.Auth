import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
    plugins: [react()],
    build: {
        outDir: '../../Digivance.Auth.Api/wwwroot'
    },

    server: {
        proxy: {
            '/api/': {
                changeOrigin: true,
                secure: false,
                target: 'http://localhost:5000'
            }
        }
    }
});
