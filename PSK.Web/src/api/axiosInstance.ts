import axios from 'axios';

const axiosInstance = axios.create({
    baseURL: 'http://localhost:5041',
    timeout: 5000,
    headers: {
        'Content-Type': 'application/json',
    },
});

axiosInstance.interceptors.request.use(
    (config) => {
        // const token = localStorage.getItem('token');
        const token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI3Mzk4OTM0Yy0xMjE5LTRiNGMtYjY0Mi1lNDA3YWFhYjUyYTQiLCJlbWFpbCI6InVzZXIxQGV4YW1wbGUuY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6InN0cmluZyBXZEdhSnpqVEV4UmttZmZ1dll6YyIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlVzZXIiLCJleHAiOjE3NDgwMjgyMDUsImlzcyI6IlBTSy5BcGlTZXJ2aWNlIiwiYXVkIjoiUFNLLkNsaWVudCJ9.R3D2bZRusW35waQMwpVnqFPkC9O4wFc456siv2rEIVc"
        //TODO temp hardcoded
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

axiosInstance.interceptors.response.use(
    (response) => response,
    (error) => {
        console.error('API Error:', error);
        return Promise.reject(error);
    }
);

export default axiosInstance;