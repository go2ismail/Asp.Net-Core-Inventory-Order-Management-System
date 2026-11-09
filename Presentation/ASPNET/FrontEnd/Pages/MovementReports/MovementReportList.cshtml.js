const App = {
    setup() {
        const state = Vue.reactive({
            mainData: []
        });

        const mainGridRef = Vue.ref(null);

        const services = {
            getMainData: async () => {
                try {
                    const response = await AxiosManager.get('/InventoryTransaction/GetInventoryTransactionList', {});
                    return response;
                } catch (error) {
                    throw error;
                }
            },
        };

        const methods = {
            populateMainData: async () => {
                const response = await services.getMainData();
                state.mainData = response?.data?.content?.data.map(item => ({
                    ...item,
                    createdAtUtc: new Date(item.createdAtUtc),
                    movementDate: new Date(item.movementDate)
                }));
            },
            onMainModalHidden: () => {
            }
        };

        Vue.onMounted(async () => {
            try {
                await SecurityManager.authorizePage(['MovementReports']);
                await SecurityManager.validateToken();

                await methods.populateMainData();
                await mainGrid.create(state.mainData);

            } catch (e) {
                console.error('page init error:', e);
            } finally {
                
            }
        });

        const mainGrid = {
            obj: null,
            create: async (dataSource) => {
                mainGrid.obj = new ej.pivotview.PivotView({
                    height: '240px',
                    width: '100%',
                    dataSourceSettings: {
                        dataSource: dataSource,
                        expandAll: true,
                        filters: [],
                        columns: [
                            { name: 'moduleCode' },
                        ],
                        rows: [
                            { name: 'productName' },
                            { name: 'warehouseName' }
                        ],
                        values: [
                            { name: 'stock' }
                        ],
                    },
                    showToolbar: true,
                    showFieldList: false,
                    displayOption: { view: "Table" },
                    showGroupingBar: false,
                    showValuesButton: true,
                    groupingBarSettings: { showFieldsPanel: true },
                });

                mainGrid.obj.appendTo(mainGridRef.value);
            },
            refresh: () => {
                mainGrid.obj.setProperties({ dataSource: state.mainData });
            }
        };

        return {
            mainGridRef,
            state,
        };
    }
};

Vue.createApp(App).mount('#app');