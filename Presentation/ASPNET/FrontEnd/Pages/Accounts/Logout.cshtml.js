const App = {
    setup() {
        const state = Vue.reactive({
            isSubmitting: false,
        });

        const logout = async () => {
            const response = await AxiosManager.post('/Security/Logout', {});
            return response;
        }

        const showError = (message) => {
            Swal.fire({
                icon: 'error',
                title: 'An Error Occurred',
                text: message || 'Please try again.',
                timer: 2000,
                showConfirmButton: false
            });
        }

        const redirectHome = () => {
            setTimeout(() => {
                window.location.href = '/';
            }, 2000);
        }

        const handleSubmit = async () => {

            try {
                state.isSubmitting = true;
                await new Promise(resolve => setTimeout(resolve, 300));

                const response = await logout();
                StorageManager.clearStorage();

                if (response.data.code === 200) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Logout Successful',
                        text: 'You are being redirected...',
                        timer: 2000,
                        showConfirmButton: false
                    });
                } else {
                    showError(response.data.message ?? 'Logout Failed.');
                }

                redirectHome();

            } catch (error) {
                StorageManager.clearStorage();
                showError(error.response?.data?.message || 'Please try again.');
                redirectHome();
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
