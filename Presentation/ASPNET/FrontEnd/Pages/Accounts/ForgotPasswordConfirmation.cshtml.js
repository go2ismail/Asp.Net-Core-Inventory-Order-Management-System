const App = {
    setup() {
        const state = Vue.reactive({
            isSubmitting: false,
        });

        const forgotPasswordConfirmation = async (email, code, tempPassword) => {
            try {
                const response = await AxiosManager.get(`/Security/ForgotPasswordConfirmation?email=${email}&code=${code}&tempPassword=${tempPassword}`, {});
                return response;
            } catch (error) {
                console.error('Error during ForgotPasswordConfirmation:', error);
                throw error;
            }
        }


        Vue.onMounted(async () => {
            try {
                state.isSubmitting = true;
                await new Promise(resolve => setTimeout(resolve, 300));

                const params = new URLSearchParams(window.location.search);
                const email = params.get('email');
                const code = params.get('code');
                const tempPassword = params.get('tempPassword');

                if (email && code && tempPassword) {
                    const response = await forgotPasswordConfirmation(email, code, tempPassword);
                    if (response.data.code === 200) {
                        Swal.fire({
                            icon: 'success',
                            title: 'Password Reset Successful',
                            text: 'You are being redirected...',
                            timer: 2000,
                            showConfirmButton: false
                        });
                        setTimeout(() => {
                            window.location.href = '/Accounts/Login';
                        }, 2000);
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Password Reset Failed',
                            text: response.data.message ?? 'Please check your data.',
                            confirmButtonText: 'Try Again'
                        });
                    }
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Password Reset Failed',
                        text: 'Email or code is missing in the URL query string.',
                        confirmButtonText: 'Try Again'
                    });
                }

            } catch (e) {
                console.error('page init error:', e);
            } finally {
                state.isSubmitting = false;
            }
        });

        return {
            state
        };
    }
};

Vue.createApp(App).mount('#app');
