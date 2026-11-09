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
                await SecurityManager.authorizePage(['TransactionReports']);
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
                mainGrid.obj = new ej.grids.Grid({
                    height: '240px',
                    dataSource: dataSource,
                    allowFiltering: true,
                    allowSorting: true,
                    allowSelection: true,
                    allowGrouping: true,
                    groupSettings: {
                        columns: ['productName']
                    },
                    allowTextWrap: true,
                    allowResizing: true,
                    allowPaging: true,
                    allowExcelExport: true,
                    filterSettings: { type: 'CheckBox' },
                    sortSettings: { columns: [{ field: 'createdAtUtc', direction: 'Descending' }] },
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
                        { field: 'warehouseName', headerText: 'Warehouse', width: 100 },
                        { field: 'productName', headerText: 'Product', width: 100 },
                        { field: 'movementDate', headerText: 'Movement Date', width: 100, format: 'yyyy-MM-dd' },
                        { field: 'number', headerText: 'Number', width: 100 },
                        { field: 'movement', headerText: 'Movement', width: 100, type: 'number', format: 'N2', textAlign: 'Right' },
                        { field: 'transTypeName', headerText: 'Trans Type', width: 100 },
                        { field: 'stock', headerText: 'Stock', width: 150, type: 'number', format: '+0.00;-0.00;0.00', textAlign: 'Right' },
                        { field: 'statusName', headerText: 'Status', width: 100 },
                        { field: 'moduleName', headerText: 'Module', width: 100 },
                        { field: 'moduleCode', headerText: 'Module Code', width: 100 },
                        { field: 'moduleNumber', headerText: 'Module Number', width: 100 },
                        { field: 'warehouseFromName', headerText: 'Warehouse From', width: 100 },
                        { field: 'warehouseToName', headerText: 'Warehouse To', width: 100 },
                        { field: 'createdAtUtc', headerText: 'Created At UTC', width: 150, format: 'yyyy-MM-dd HH:mm' }
                    ],
                    aggregates: [
                        {
                            columns: [
                                {
                                    type: 'Sum',
                                    field: 'stock',
                                    groupCaptionTemplate: 'Stock: ${Sum}',
                                    format: 'N2'
                                }
                            ]
                        }
                    ],
                    toolbar: [
                        'ExcelExport', 'Search',
                        { type: 'Separator' },
                    ],
                    beforeDataBound: () => { },
                    dataBound: function () {
                        mainGrid.obj.autoFitColumns(['warehouseName', 'productName', 'movementDate', 'number', 'movement', 'transTypeName', 'statusName', 'moduleName', 'moduleCode', 'moduleNumber', 'warehouseFromName', 'warehouseToName', 'createdAtUtc']);
                    },
                    excelExportComplete: () => { },
                    rowSelected: () => {
                        if (mainGrid.obj.getSelectedRecords().length == 1) {
                        } else {
                        }
                    },
                    rowDeselected: () => {
                        if (mainGrid.obj.getSelectedRecords().length == 1) {
                        } else {
                        }
                    },
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

        return {
            mainGridRef,
            state,
        };
    }
};

Vue.createApp(App).mount('#app');