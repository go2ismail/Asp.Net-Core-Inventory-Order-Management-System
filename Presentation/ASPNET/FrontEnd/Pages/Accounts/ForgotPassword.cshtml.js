const App = {
    setup() {
        const state = Vue.reactive({
            email: '',
            isSubmitting: false,
            errors: {
                email: ''
            }
        });

        const validateForm = () => {
            state.errors.email = '';
            let isValid = true;

            if (!state.email) {
                state.errors.email = 'Email is required.';
                isValid = false;
            } else if (!/\S+@\S+\.\S+/.test(state.email)) {
                state.errors.email = 'Please enter a valid email address.';
                isValid = false;
            }

            return isValid;
        };


        const forgotPassword = async (email) => {
            try {
                const response = await AxiosManager.post('/Security/ForgotPassword', {
                    email
                });
                return response;
            } catch (error) {
                throw error;
            }
        }

        const handleSubmit = async () => {

            try {
                state.isSubmitting = true;
                await new Promise(resolve => setTimeout(resolve, 300));

                if (!validateForm()) return;

                const response = await forgotPassword(state.email);
                if (response.data.code === 200) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Reset Password Successful',
                        text: 'Please check your email. You are being redirected...',
                        timer: 2000,
                        showConfirmButton: false
                    });
                    setTimeout(() => {
                        window.location.href = '/';
                    }, 2000);
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Reset Password Failed',
                        text: response.data.message ?? 'Please check your data.',
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

