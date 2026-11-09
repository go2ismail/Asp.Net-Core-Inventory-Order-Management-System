const App = {
    setup() {
        const state = Vue.reactive({
            email: '',
            password: '',
            confirmPassword: '',
            firstName: '',
            lastName: '',
            companyName: '',
            isSubmitting: false,
            errors: {
                email: '',
                password: '',
                confirmPassword: '',
                firstName: '',
                lastName: ''
            }
        });

        const validateForm = () => {
            // Reset errors
            state.errors.email = '';
            state.errors.password = '';
            state.errors.confirmPassword = '';
            state.errors.firstName = '';
            state.errors.lastName = '';

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

            if (!state.confirmPassword) {
                state.errors.confirmPassword = 'Confirm Password is required.';
                isValid = false;
            } else if (state.password !== state.confirmPassword) {
                state.errors.confirmPassword = 'Passwords do not match.';
                isValid = false;
            }

            if (!state.firstName) {
                state.errors.firstName = 'First Name is required.';
                isValid = false;
            }

            if (!state.lastName) {
                state.errors.lastName = 'Last Name is required.';
                isValid = false;
            }

            return isValid;
        };

        const handleSubmit = async () => {

            try {
                state.isSubmitting = true;
                await new Promise(resolve => setTimeout(resolve, 300));

                if (!validateForm()) return;

                const response = await AxiosManager.post('/Security/Register', {
                    email: state.email,
                    password: state.password,
                    confirmPassword: state.confirmPassword,
                    firstName: state.firstName,
                    lastName: state.lastName,
                    companyName: state.companyName
                });

                if (response.data.code === 200) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Register Successful',
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
                        title: 'Register Failed',
                        text: response.data.message || 'Please check your data.',
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
