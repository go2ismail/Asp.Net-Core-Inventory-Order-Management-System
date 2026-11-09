const App = {
    setup() {
        const state = Vue.reactive({
            isSubmitting: false,
        });

        const logout = async () => {
            try {
                const response = await AxiosManager.post('/Security/Logout', {
                    userId: StorageManager.getUserId()
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

                const response = await logout();
                if (response.data.code === 200) {
                    StorageManager.clearStorage();

                    Swal.fire({
                        icon: 'success',
                        title: 'Logout Successful',
                        text: 'You are being redirected...',
                        timer: 2000,
                        showConfirmButton: false
                    });
                    setTimeout(() => {
                        window.location.href = '/';
                    }, 2000);
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Logout Failed',
                        text: response.data.message ?? 'Logout Failed.',
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



