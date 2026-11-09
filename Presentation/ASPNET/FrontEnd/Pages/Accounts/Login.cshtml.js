const App = {
    setup() {
        const state = Vue.reactive({
            email: '',
            password: '',
            isSubmitting: false,
            errors: {
                email: '',
                password: ''
            }
        });

        const validateForm = () => {
            state.errors.email = '';
            state.errors.password = '';
            let isValid = true;

            if (!state.email) {
                state.errors.email = 'Email is required.';
                isValid = false;
            } else if (!/\S+@\S+\.\S+/.test(state.email)) {
                state.errors.email = 'Please enter a valid email address.';
                isValid = false;
            }

            if (!state.password) {
                state.errors.password = 'Password is required.';
                isValid = false;
            } else if (state.password.length < 6) {
                state.errors.password = 'Password must be at least 6 characters.';
                isValid = false;
            }

            return isValid;
        };

        const handleSubmit = async () => {

            try {
                state.isSubmitting = true;
                await new Promise(resolve => setTimeout(resolve, 300));

                if (!validateForm()) {
                    return;
                }

                const response = await AxiosManager.post('/Security/Login', {
                    email: state.email,
                    password: state.password
                });

                if (response.data.code === 200) {
                    StorageManager.saveLoginResult(response.data);

                    Swal.fire({
                        icon: 'success',
                        title: 'Login Successful',
                        text: 'You are being redirected...',
                        timer: 2000,
                        showConfirmButton: false
                    });

                    setTimeout(() => {
                        window.location.href = '/Profiles/MyProfile';
                    }, 2000);
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Login Failed',
                        text: response.data.message || 'Please check your credentials.',
                        confirmButtonText: 'Try Again'
                    });
                }
            } catch (error) {
                Swal.fire({
                    icon: 'error',
                    title: 'An Error Occurred',
                    text: error.response?.data?.message || 'Please try again.',
                    confirmButtonText: 'OK'
                });
            } finally {
                state.isSubmitting = false;
            }
        };

        return {
            state,
            handleSubmit
        };
    }
};

Vue.createApp(App).mount('#app');

