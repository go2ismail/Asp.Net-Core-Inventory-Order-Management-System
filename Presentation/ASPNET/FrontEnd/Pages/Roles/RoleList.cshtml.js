const App = {
    setup() {
        const state = Vue.reactive({
            mainData: []
        });


        const mainGridRef = Vue.ref(null);

        const services = {
            getMainData: async () => {
                try {
                    const response = await AxiosManager.get('/Security/GetRoleList', {});
                    return response;
                } catch (error) {
                    throw error;
                }
            },
        };

        const mainGrid = {
            obj: null,
            create: async (dataSource) => {
                mainGrid.obj = new ej.grids.Grid({
                    height: '240px',
                    dataSource: dataSource,
                    allowFiltering: true,
                    allowSorting: true,
                    allowSelection: true,
                    allowGrouping: true,
                    allowTextWrap: true,
                    allowResizing: true,
                    allowPaging: true,
                    allowExcelExport: true,
                    filterSettings: { type: 'CheckBox' },
                    sortSettings: { columns: [{ field: 'name', direction: 'Descending' }] },
                    pageSettings: { currentPage: 1, pageSize: 50, pageSizes: ["10", "20", "50", "100", "200", "All"] },
                    selectionSettings: { persistSelection: true, type: 'Single' },
                    autoFit: true,
                    showColumnMenu: true,
                    gridLines: 'Horizontal',
                    columns: [
                        { type: 'checkbox', width: 60 },
                        {
                            field: 'id', isPrimaryKey: true, headerText: 'Id', visible: false
                        },
                        { field: 'name', headerText: 'Name', width: 300, minWidth: 300 },
                    ],
                    toolbar: [
                        'ExcelExport', 'Search',
                    ],
                    beforeDataBound: () => { },
                    dataBound: function () {
                        mainGrid.obj.autoFitColumns(['name']);
                    },
                    excelExportComplete: () => { },
                    rowSelected: () => { },
                    rowDeselected: () => { },
                    rowSelecting: () => {
                        if (mainGrid.obj.getSelectedRecords().length) {
                            mainGrid.obj.clearSelection();
                        }
                    },
                    toolbarClick: (args) => {
                        if (args.item.id === 'MainGrid_excelexport') {
                            mainGrid.obj.excelExport();
                        }
                    }
                });

                mainGrid.obj.appendTo(mainGridRef.value);
            },
            refresh: () => {
                mainGrid.obj.setProperties({ dataSource: state.mainData });
            }
        };

        const methods = {
            populateMainData: async () => {
                const response = await services.getMainData();
                state.mainData = response?.data?.content?.data;
            },

            init: async () => {
                try {
                    await SecurityManager.authorizePage(['Roles']);
                    await SecurityManager.validateToken();
                    await methods.populateMainData();
                    await mainGrid.create(state.mainData);
                } catch (e) {
                    console.error('page init error:', e);
                } finally {
                    
                }
            },
        };

        Vue.onMounted(() => {
            methods.init();
        });

        return {
            state,
            mainGridRef
        };
    }
};

Vue.createApp(App).mount('#app');