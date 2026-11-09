const App = {
    setup() {
        const state = Vue.reactive({
            mainData: [],
            deleteMode: false,
            productGroupListLookupData: [],
            unitMeasureListLookupData: [],
            mainTitle: null,
            id: '',
            name: '',
            number: '',
            unitPrice: '',
            description: '',
            productGroupId: null,
            unitMeasureId: null,
            physical: false,
            errors: {
                name: '',
                unitPrice: '',
                productGroupId: '',
                unitMeasureId: ''
            },
            isSubmitting: false
        });

        const mainGridRef = Vue.ref(null);
        const mainModalRef = Vue.ref(null);
        const productGroupIdRef = Vue.ref(null);
        const unitMeasureIdRef = Vue.ref(null);
        const nameRef = Vue.ref(null);
        const numberRef = Vue.ref(null);
        const unitPriceRef = Vue.ref(null);

        const validateForm = function () {
            state.errors.name = '';
            state.errors.unitPrice = '';
            state.errors.productGroupId = '';
            state.errors.unitMeasureId = '';

            let isValid = true;

            if (!state.name) {
                state.errors.name = 'Name is required.';
                isValid = false;
            }
            if (!state.unitPrice) {
                state.errors.unitPrice = 'Unit price is required.';
                isValid = false;
            } else if (!/^\d+(\.\d{1,2})?$/.test(state.unitPrice)) {
                state.errors.unitPrice = 'Unit price must be a numeric value with up to two decimal places.';
                isValid = false;
            }
            if (!state.productGroupId) {
                state.errors.productGroupId = 'ProductGroup is required.';
                isValid = false;
            }
            if (!state.unitMeasureId) {
                state.errors.unitMeasureId = 'UnitMeasure is required.';
                isValid = false;
            }

            return isValid;
        };

        const resetFormState = () => {
            state.id = '';
            state.name = '';
            state.number = '';
            state.unitPrice = '';
            state.description = '';
            state.productGroupId = null;
            state.unitMeasureId = null;
            state.physical = false;
            state.errors = {
                name: '',
                unitPrice: '',
                productGroupId: '',
                unitMeasureId: ''
            };
        };

        const services = {
            getMainData: async () => {
                try {
                    const response = await AxiosManager.get('/Product/GetProductList', {});
                    return response;
                } catch (error) {
                    throw error;
                }
            },
            createMainData: async (name, unitPrice, physical, description, productGroupId, unitMeasureId, createdById) => {
                try {
                    const response = await AxiosManager.post('/Product/CreateProduct', {
                        name, unitPrice, physical, description, productGroupId, unitMeasureId, createdById
                    });
                    return response;
                } catch (error) {
                    throw error;
                }
            },
            updateMainData: async (id, name, unitPrice, physical, description, productGroupId, unitMeasureId, updatedById) => {
                try {
                    const response = await AxiosManager.post('/Product/UpdateProduct', {
                        id, name, unitPrice, physical, description, productGroupId, unitMeasureId, updatedById
                    });
                    return response;
                } catch (error) {
                    throw error;
                }
            },
            deleteMainData: async (id, deletedById) => {
                try {
                    const response = await AxiosManager.post('/Product/DeleteProduct', {
                        id, deletedById
                    });
                    return response;
                } catch (error) {
                    throw error;
                }
            },
            getProductGroupListLookupData: async () => {
                try {
                    const response = await AxiosManager.get('/ProductGroup/GetProductGroupList', {});
                    return response;
                } catch (error) {
                    throw error;
                }
            },
            getUnitMeasureListLookupData: async () => {
                try {
                    const response = await AxiosManager.get('/UnitMeasure/GetUnitMeasureList', {});
                    return response;
                } catch (error) {
                    throw error;
                }
            },
        };

        const methods = {
            populateProductGroupListLookupData: async () => {
                const response = await services.getProductGroupListLookupData();
                state.productGroupListLookupData = response?.data?.content?.data;
            },
            populateUnitMeasureListLookupData: async () => {
                const response = await services.getUnitMeasureListLookupData();
                state.unitMeasureListLookupData = response?.data?.content?.data;
            },
            populateMainData: async () => {
                const response = await services.getMainData();
                state.mainData = response?.data?.content?.data.map(item => ({
                    ...item,
                    createdAtUtc: new Date(item.createdAtUtc)
                }));
            },
        };

        const productGroupListLookup = {
            obj: null,
            create: () => {
                if (state.productGroupListLookupData && Array.isArray(state.productGroupListLookupData)) {
                    productGroupListLookup.obj = new ej.dropdowns.DropDownList({
                        dataSource: state.productGroupListLookupData,
                        fields: { value: 'id', text: 'name' },
                        placeholder: 'Select a Product Group',
                        popupHeight: '200px',
                        change: (e) => {
                            state.productGroupId = e.value;
                        }
                    });
                    productGroupListLookup.obj.appendTo(productGroupIdRef.value);
                } else {
                    console.error('ProductGroup list lookup data is not available or invalid.');
                }
            },
            refresh: () => {
                if (productGroupListLookup.obj) {
                    productGroupListLookup.obj.value = state.productGroupId;
                }
            },
        };

        const unitMeasureListLookup = {
            obj: null,
            create: () => {
                if (state.unitMeasureListLookupData && Array.isArray(state.unitMeasureListLookupData)) {
                    unitMeasureListLookup.obj = new ej.dropdowns.DropDownList({
                        dataSource: state.unitMeasureListLookupData,
                        fields: { value: 'id', text: 'name' },
                        placeholder: 'Select a Unit Measure',
                        popupHeight: '200px',
                        change: (e) => {
                            state.unitMeasureId = e.value;
                        }
                    });
                    unitMeasureListLookup.obj.appendTo(unitMeasureIdRef.value);
                } else {
                    console.error('UnitMeasure list lookup data is not available or invalid.');
                }
            },
            refresh: () => {
                if (unitMeasureListLookup.obj) {
                    unitMeasureListLookup.obj.value = state.unitMeasureId;
                }
            },
        };

        const nameText = {
            obj: null,
            create: () => {
                nameText.obj = new ej.inputs.TextBox({
                    placeholder: 'Enter Name',
                });
                nameText.obj.appendTo(nameRef.value);
            },
            refresh: () => {
                if (nameText.obj) {
                    nameText.obj.value = state.name;
                }
            }
        };

        const numberText = {
            obj: null,
            create: () => {
                numberText.obj = new ej.inputs.TextBox({
                    placeholder: '[auto]',
                    readonly: true
                });
                numberText.obj.appendTo(numberRef.value);
            },
            refresh: () => {
                if (numberText.obj) {
                    numberText.obj.value = state.number;
                }
            }
        };

        const unitPriceNumber = {
            obj: null,
            create: () => {
                unitPriceNumber.obj = new ej.inputs.NumericTextBox({
                    format: 'n2',
                    placeholder: 'Enter Unit Price',
                    min: 0,
                    step: 0.01,
                    validateDecimalOnType: true
                });
                unitPriceNumber.obj.appendTo(unitPriceRef.value);
            },
            refresh: () => {
                if (unitPriceNumber.obj) {
                    unitPriceNumber.obj.value = state.unitPrice;
                }
            }
        };

        Vue.watch(
            () => state.name,
            (newVal, oldVal) => {
                state.errors.name = '';
                nameText.refresh();
            }
        );

        Vue.watch(
            () => state.number,
            (newVal, oldVal) => {
                numberText.refresh();
            }
        );

        Vue.watch(
            () => state.unitPrice,
            (newVal, oldVal) => {
                state.errors.unitPrice = '';
                unitPriceNumber.refresh();
            }
        );

        Vue.watch(
            () => state.productGroupId,
            (newVal, oldVal) => {
                state.errors.productGroupId = '';
                productGroupListLookup.refresh();
            }
        );

        Vue.watch(
            () => state.unitMeasureId,
            (newVal, oldVal) => {
                state.errors.unitMeasureId = '';
                unitMeasureListLookup.refresh();
            }
        );

        const handler = {
            handleSubmit: async function () {
                try {
                    state.isSubmitting = true;
                    await new Promise(resolve => setTimeout(resolve, 300));

                    if (!validateForm()) {
                        return;
                    }

                    const response = state.id === ''
                        ? await services.createMainData(state.name, state.unitPrice, state.physical, state.description, state.productGroupId, state.unitMeasureId, StorageManager.getUserId())
                        : state.deleteMode
                            ? await services.deleteMainData(state.id, StorageManager.getUserId())
                            : await services.updateMainData(state.id, state.name, state.unitPrice, state.physical, state.description, state.productGroupId, state.unitMeasureId, StorageManager.getUserId());

                    if (response.data.code === 200) {
                        await methods.populateMainData();
                        mainGrid.refresh();

                        if (!state.deleteMode) {
                            state.mainTitle = 'Edit Product';
                            state.id = response?.data?.content?.data.id ?? '';
                            state.number = response?.data?.content?.data.number ?? '';
                            state.name = response?.data?.content?.data.name ?? '';
                            state.unitPrice = response?.data?.content?.data.unitPrice ?? '';
                            state.description = response?.data?.content?.data.description ?? '';
                            state.productGroupId = response?.data?.content?.data.productGroupId ?? '';
                            state.unitMeasureId = response?.data?.content?.data.unitMeasureId ?? '';
                            state.physical = response?.data?.content?.data.physical ?? false;

                            Swal.fire({
                                icon: 'success',
                                title: state.deleteMode ? 'Delete Successful' : 'Save Successful',
                                text: 'Form will be closed...',
                                timer: 2000,
                                showConfirmButton: false
                            });
                            setTimeout(() => {
                                mainModal.obj.hide();
                            }, 2000);

                        } else {
                            Swal.fire({
                                icon: 'success',
                                title: 'Delete Successful',
                                text: 'Form will be closed...',
                                timer: 2000,
                                showConfirmButton: false
                            });
                            setTimeout(() => {
                                mainModal.obj.hide();
                                resetFormState();
                            }, 2000);
                        }

                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: state.deleteMode ? 'Delete Failed' : 'Save Failed',
                            text: response.data.message ?? 'Please check your data.',
                            confirmButtonText: 'Try Again'
                        });
                    }

                } catch (error) {
                    Swal.fire({
                        icon: 'error',
                        title: 'An Error Occurred',
                        text: error.response?.data?.message ?? 'Please try again.',
                        confirmButtonText: 'OK'
                    });
                } finally {
                    state.isSubmitting = false;
                }
            },
        };

        Vue.onMounted(async () => {
            try {
                await SecurityManager.authorizePage(['Products']);
                await SecurityManager.validateToken();

                await methods.populateMainData();
                await mainGrid.create(state.mainData);
                await methods.populateProductGroupListLookupData();
                productGroupListLookup.create();
                await methods.populateUnitMeasureListLookupData();
                unitMeasureListLookup.create();

                nameText.create();
                numberText.create();
                unitPriceNumber.create();

                mainModal.create();
                mainModalRef.value?.addEventListener('hidden.bs.modal', () => {
                    resetFormState();
                });

            } catch (e) {
                console.error('page init error:', e);
            } finally {
                
            }
        });

        Vue.onUnmounted(() => {
            mainModalRef.value?.removeEventListener('hidden.bs.modal', resetFormState);
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
                        columns: ['productGroupName']
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
                        { field: 'number', headerText: 'Number', width: 200, minWidth: 200 },
                        { field: 'name', headerText: 'Name', width: 200, minWidth: 200 },
                        { field: 'productGroupName', headerText: 'Product Group', width: 150, minWidth: 150 },
                        { field: 'unitPrice', headerText: 'Unit Price', width: 150, minWidth: 150, format: 'N2' },
                        { field: 'unitMeasureName', headerText: 'Unit Measure', width: 150, minWidth: 150 },
                        { field: 'physical', headerText: 'Physical Product', width: 200, minWidth: 200, textAlign: 'Center', type: 'boolean', displayAsCheckBox: true },
                        { field: 'createdAtUtc', headerText: 'Created At UTC', width: 150, format: 'yyyy-MM-dd HH:mm' }
                    ],
                    toolbar: [
                        'ExcelExport', 'Search',
                        { type: 'Separator' },
                        { text: 'Add', tooltipText: 'Add', prefixIcon: 'e-add', id: 'AddCustom' },
                        { text: 'Edit', tooltipText: 'Edit', prefixIcon: 'e-edit', id: 'EditCustom' },
                        { text: 'Delete', tooltipText: 'Delete', prefixIcon: 'e-delete', id: 'DeleteCustom' },
                        { type: 'Separator' },
                    ],
                    beforeDataBound: () => { },
                    dataBound: function () {
                        mainGrid.obj.toolbarModule.enableItems(['EditCustom', 'DeleteCustom'], false);
                        mainGrid.obj.autoFitColumns(['number', 'name', 'productGroupName', 'unitPrice', 'unitMeasureName', 'physical', 'createdAtUtc']);
                    },
                    excelExportComplete: () => { },
                    rowSelected: () => {
                        if (mainGrid.obj.getSelectedRecords().length == 1) {
                            mainGrid.obj.toolbarModule.enableItems(['EditCustom', 'DeleteCustom'], true);
                        } else {
                            mainGrid.obj.toolbarModule.enableItems(['EditCustom', 'DeleteCustom'], false);
                        }
                    },
                    rowDeselected: () => {
                        if (mainGrid.obj.getSelectedRecords().length == 1) {
                            mainGrid.obj.toolbarModule.enableItems(['EditCustom', 'DeleteCustom'], true);
                        } else {
                            mainGrid.obj.toolbarModule.enableItems(['EditCustom', 'DeleteCustom'], false);
                        }
                    },
                    rowSelecting: () => {
                        if (mainGrid.obj.getSelectedRecords().length) {
                            mainGrid.obj.clearSelection();
                        }
                    },
                    toolbarClick: async (args) => {
                        if (args.item.id === 'MainGrid_excelexport') {
                            mainGrid.obj.excelExport();
                        }

                        if (args.item.id === 'AddCustom') {
                            state.deleteMode = false;
                            state.mainTitle = 'Add Product';
                            resetFormState();
                            mainModal.obj.show();
                        }

                        if (args.item.id === 'EditCustom') {
                            state.deleteMode = false;
                            if (mainGrid.obj.getSelectedRecords().length) {
                                const selectedRecord = mainGrid.obj.getSelectedRecords()[0];
                                state.mainTitle = 'Edit Product';
                                state.id = selectedRecord.id ?? '';
                                state.number = selectedRecord.number ?? '';
                                state.name = selectedRecord.name ?? '';
                                state.unitPrice = selectedRecord.unitPrice ?? '';
                                state.description = selectedRecord.description ?? '';
                                state.productGroupId = selectedRecord.productGroupId ?? '';
                                state.unitMeasureId = selectedRecord.unitMeasureId ?? '';
                                state.physical = selectedRecord.physical ?? false;
                                mainModal.obj.show();
                            }
                        }

                        if (args.item.id === 'DeleteCustom') {
                            state.deleteMode = true;
                            if (mainGrid.obj.getSelectedRecords().length) {
                                const selectedRecord = mainGrid.obj.getSelectedRecords()[0];
                                state.mainTitle = 'Delete Product?';
                                state.id = selectedRecord.id ?? '';
                                state.number = selectedRecord.number ?? '';
                                state.name = selectedRecord.name ?? '';
                                state.unitPrice = selectedRecord.unitPrice ?? '';
                                state.description = selectedRecord.description ?? '';
                                state.productGroupId = selectedRecord.productGroupId ?? '';
                                state.unitMeasureId = selectedRecord.unitMeasureId ?? '';
                                state.physical = selectedRecord.physical ?? false;
                                mainModal.obj.show();
                            }
                        }
                    }
                });

                mainGrid.obj.appendTo(mainGridRef.value);
            },
            refresh: () => {
                mainGrid.obj.setProperties({ dataSource: state.mainData });
            }
        };

        const mainModal = {
            obj: null,
            create: () => {
                mainModal.obj = new bootstrap.Modal(mainModalRef.value, {
                    backdrop: 'static',
                    keyboard: false
                });
            }
        };

        return {
            mainGridRef,
            mainModalRef,
            productGroupIdRef,
            unitMeasureIdRef,
            nameRef,
            numberRef,
            unitPriceRef,
            state,
            handler,
        };
    }
};

Vue.createApp(App).mount('#app');