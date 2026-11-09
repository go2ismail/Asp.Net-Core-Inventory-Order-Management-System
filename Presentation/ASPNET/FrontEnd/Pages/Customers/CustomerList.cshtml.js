const App = {
    setup() {
        const state = Vue.reactive({
            mainData: [],
            deleteMode: false,
            customerGroupListLookupData: [],
            customerCategoryListLookupData: [],
            secondaryData: [],
            mainTitle: null,
            manageContactTitle: 'Manage Contact',
            id: '',
            name: '',
            number: '',
            customerGroupId: null,
            customerCategoryId: null,
            description: '',
            street: '',
            city: '',
            state: '',
            zipCode: '',
            country: '',
            phoneNumber: '',
            faxNumber: '',
            emailAddress: '',
            website: '',
            whatsApp: '',
            linkedIn: '',
            facebook: '',
            instagram: '',
            twitterX: '',
            tikTok: '',
            errors: {
                name: '',
                customerGroupId: '',
                customerCategoryId: '',
                street: '',
                city: '',
                state: '',
                zipCode: '',
                country: '',
                phoneNumber: '',
                emailAddress: '',
            },
            isSubmitting: false
        });

        const mainGridRef = Vue.ref(null);
        const mainModalRef = Vue.ref(null);
        const manageContactModalRef = Vue.ref(null);
        const secondaryGridRef = Vue.ref(null);
        const nameRef = Vue.ref(null);
        const numberRef = Vue.ref(null);
        const streetRef = Vue.ref(null);
        const cityRef = Vue.ref(null);
        const stateRef = Vue.ref(null);
        const zipCodeRef = Vue.ref(null);
        const countryRef = Vue.ref(null);
        const phoneNumberRef = Vue.ref(null);
        const faxNumberRef = Vue.ref(null);
        const emailAddressRef = Vue.ref(null);
        const websiteRef = Vue.ref(null);
        const whatsAppRef = Vue.ref(null);
        const linkedInRef = Vue.ref(null);
        const facebookRef = Vue.ref(null);
        const instagramRef = Vue.ref(null);
        const twitterXRef = Vue.ref(null);
        const tikTokRef = Vue.ref(null);
        const customerGroupIdRef = Vue.ref(null);
        const customerCategoryIdRef = Vue.ref(null);

        const services = {
            getMainData: async () => {
                try {
                    const response = await AxiosManager.get('/Customer/GetCustomerList', {});
                    return response;
                } catch (error) {
                    throw error;
                }
            },
            createMainData: async (name, customerGroupId, customerCategoryId, description, street, city, state, zipCode, country, phoneNumber, faxNumber, emailAddress, website, whatsApp, linkedIn, facebook, instagram, twitterX, tikTok, createdById) => {
                try {
                    const response = await AxiosManager.post('/Customer/CreateCustomer', {
                        name, customerGroupId, customerCategoryId, description, street, city, state, zipCode, country, phoneNumber, faxNumber, emailAddress, website, whatsApp, linkedIn, facebook, instagram, twitterX, tikTok, createdById
                    });
                    return response;
                } catch (error) {
                    throw error;
                }
            },
            updateMainData: async (id, name, customerGroupId, customerCategoryId, description, street, city, state, zipCode, country, phoneNumber, faxNumber, emailAddress, website, whatsApp, linkedIn, facebook, instagram, twitterX, tikTok, updatedById) => {
                try {
                    const response = await AxiosManager.post('/Customer/UpdateCustomer', {
                        id, name, customerGroupId, customerCategoryId, description, street, city, state, zipCode, country, phoneNumber, faxNumber, emailAddress, website, whatsApp, linkedIn, facebook, instagram, twitterX, tikTok, updatedById
                    });
                    return response;
                } catch (error) {
                    throw error;
                }
            },
            deleteMainData: async (id, deletedById) => {
                try {
                    const response = await AxiosManager.post('/Customer/DeleteCustomer', {
                        id, deletedById
                    });
                    return response;
                } catch (error) {
                    throw error;
                }
            },
            getCustomerGroupListLookupData: async () => {
                try {
                    const response = await AxiosManager.get('/CustomerGroup/GetCustomerGroupList', {});
                    return response;
                } catch (error) {
                    throw error;
                }
            },
            getCustomerCategoryListLookupData: async () => {
                try {
                    const response = await AxiosManager.get('/CustomerCategory/GetCustomerCategoryList', {});
                    return response;
                } catch (error) {
                    throw error;
                }
            },
            getSecondaryData: async (customerId) => {
                try {
                    const response = await AxiosManager.get('/CustomerContact/GetCustomerContactByCustomerIdList?customerId=' + customerId, {});
                    return response;
                } catch (error) {
                    throw error;
                }
            },
            createSecondaryData: async (name, jobTitle, phoneNumber, emailAddress, description, customerId, createdById) => {
                try {
                    const response = await AxiosManager.post('/CustomerContact/CreateCustomerContact', {
                        name, jobTitle, phoneNumber, emailAddress, description, customerId, createdById
                    });
                    return response;
                } catch (error) {
                    throw error;
                }
            },
            updateSecondaryData: async (id, name, jobTitle, phoneNumber, emailAddress, description, customerId, updatedById) => {
                try {
                    const response = await AxiosManager.post('/CustomerContact/UpdateCustomerContact', {
                        id, name, jobTitle, phoneNumber, emailAddress, description, customerId, updatedById
                    });
                    return response;
                } catch (error) {
                    throw error;
                }
            },
            deleteSecondaryData: async (id, deletedById) => {
                try {
                    const response = await AxiosManager.post('/CustomerContact/DeleteCustomerContact', {
                        id, deletedById
                    });
                    return response;
                } catch (error) {
                    throw error;
                }
            },
        };

        const methods = {
            populateCustomerGroupListLookupData: async () => {
                const response = await services.getCustomerGroupListLookupData();
                state.customerGroupListLookupData = response?.data?.content?.data;
            },
            populateCustomerCategoryListLookupData: async () => {
                const response = await services.getCustomerCategoryListLookupData();
                state.customerCategoryListLookupData = response?.data?.content?.data;
            },
            populateMainData: async () => {
                const response = await services.getMainData();
                state.mainData = response?.data?.content?.data.map(item => ({
                    ...item,
                    createdAtUtc: new Date(item.createdAtUtc)
                }));
            },
            populateSecondaryData: async (customerId) => {
                const response = await services.getSecondaryData(customerId);
                state.secondaryData = response?.data?.content?.data.map(item => ({
                    ...item,
                    createdAtUtc: new Date(item.createdAtUtc)
                }));
            },
        };

        const customerGroupListLookup = {
            obj: null,
            create: () => {
                if (state.customerGroupListLookupData && Array.isArray(state.customerGroupListLookupData)) {
                    customerGroupListLookup.obj = new ej.dropdowns.DropDownList({
                        dataSource: state.customerGroupListLookupData,
                        fields: { value: 'id', text: 'name' },
                        placeholder: 'Select a Customer Group',
                        change: (e) => {
                            state.customerGroupId = e.value;
                        }
                    });
                    customerGroupListLookup.obj.appendTo(customerGroupIdRef.value);
                } else {
                    console.error('Customer Group list lookup data is not available or invalid.');
                }
            },
            refresh: () => {
                if (customerGroupListLookup.obj) {
                    customerGroupListLookup.obj.value = state.customerGroupId;
                }
            },
        };

        const customerCategoryListLookup = {
            obj: null,
            create: () => {
                if (state.customerCategoryListLookupData && Array.isArray(state.customerCategoryListLookupData)) {
                    customerCategoryListLookup.obj = new ej.dropdowns.DropDownList({
                        dataSource: state.customerCategoryListLookupData,
                        fields: { value: 'id', text: 'name' },
                        placeholder: 'Select a Customer Category',
                        change: (e) => {
                            state.customerCategoryId = e.value;
                        }
                    });
                    customerCategoryListLookup.obj.appendTo(customerCategoryIdRef.value);
                } else {
                    console.error('Customer Category list lookup data is not available or invalid.');
                }
            },
            refresh: () => {
                if (customerCategoryListLookup.obj) {
                    customerCategoryListLookup.obj.value = state.customerCategoryId;
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

        const streetText = {
            obj: null,
            create: () => {
                streetText.obj = new ej.inputs.TextBox({
                    placeholder: 'Enter Street',
                });
                streetText.obj.appendTo(streetRef.value);
            },
            refresh: () => {
                if (streetText.obj) {
                    streetText.obj.value = state.street;
                }
            }
        };

        const cityText = {
            obj: null,
            create: () => {
                cityText.obj = new ej.inputs.TextBox({
                    placeholder: 'Enter City',
                });
                cityText.obj.appendTo(cityRef.value);
            },
            refresh: () => {
                if (cityText.obj) {
                    cityText.obj.value = state.city;
                }
            }
        };

        const stateText = {
            obj: null,
            create: () => {
                stateText.obj = new ej.inputs.TextBox({
                    placeholder: 'Enter State',
                });
                stateText.obj.appendTo(stateRef.value);
            },
            refresh: () => {
                if (stateText.obj) {
                    stateText.obj.value = state.state;
                }
            }
        };

        const zipCodeText = {
            obj: null,
            create: () => {
                zipCodeText.obj = new ej.inputs.TextBox({
                    placeholder: 'Enter Zip Code',
                });
                zipCodeText.obj.appendTo(zipCodeRef.value);
            },
            refresh: () => {
                if (zipCodeText.obj) {
                    zipCodeText.obj.value = state.zipCode;
                }
            }
        };

        const countryText = {
            obj: null,
            create: () => {
                countryText.obj = new ej.inputs.TextBox({
                    placeholder: 'Enter Country',
                });
                countryText.obj.appendTo(countryRef.value);
            },
            refresh: () => {
                if (countryText.obj) {
                    countryText.obj.value = state.country;
                }
            }
        };

        const phoneNumberText = {
            obj: null,
            create: () => {
                phoneNumberText.obj = new ej.inputs.TextBox({
                    placeholder: 'Enter Phone Number',
                });
                phoneNumberText.obj.appendTo(phoneNumberRef.value);
            },
            refresh: () => {
                if (phoneNumberText.obj) {
                    phoneNumberText.obj.value = state.phoneNumber;
                }
            }
        };

        const faxNumberText = {
            obj: null,
            create: () => {
                faxNumberText.obj = new ej.inputs.TextBox({
                    placeholder: 'Enter Fax Number',
                });
                faxNumberText.obj.appendTo(faxNumberRef.value);
            },
            refresh: () => {
                if (faxNumberText.obj) {
                    faxNumberText.obj.value = state.faxNumber;
                }
            }
        };

        const emailAddressText = {
            obj: null,
            create: () => {
                emailAddressText.obj = new ej.inputs.TextBox({
                    placeholder: 'Enter Email Address',
                });
                emailAddressText.obj.appendTo(emailAddressRef.value);
            },
            refresh: () => {
                if (emailAddressText.obj) {
                    emailAddressText.obj.value = state.emailAddress;
                }
            }
        };

        const websiteText = {
            obj: null,
            create: () => {
                websiteText.obj = new ej.inputs.TextBox({
                    placeholder: 'Enter Website',
                });
                websiteText.obj.appendTo(websiteRef.value);
            },
            refresh: () => {
                if (websiteText.obj) {
                    websiteText.obj.value = state.website;
                }
            }
        };

        const whatsAppText = {
            obj: null,
            create: () => {
                whatsAppText.obj = new ej.inputs.TextBox({
                    placeholder: 'Enter WhatsApp',
                });
                whatsAppText.obj.appendTo(whatsAppRef.value);
            },
            refresh: () => {
                if (whatsAppText.obj) {
                    whatsAppText.obj.value = state.whatsApp;
                }
            }
        };

        const linkedInText = {
            obj: null,
            create: () => {
                linkedInText.obj = new ej.inputs.TextBox({
                    placeholder: 'Enter LinkedIn',
                });
                linkedInText.obj.appendTo(linkedInRef.value);
            },
            refresh: () => {
                if (linkedInText.obj) {
                    linkedInText.obj.value = state.linkedIn;
                }
            }
        };

        const facebookText = {
            obj: null,
            create: () => {
                facebookText.obj = new ej.inputs.TextBox({
                    placeholder: 'Enter Facebook',
                });
                facebookText.obj.appendTo(facebookRef.value);
            },
            refresh: () => {
                if (facebookText.obj) {
                    facebookText.obj.value = state.facebook;
                }
            }
        };

        const instagramText = {
            obj: null,
            create: () => {
                instagramText.obj = new ej.inputs.TextBox({
                    placeholder: 'Enter Instagram',
                });
                instagramText.obj.appendTo(instagramRef.value);
            },
            refresh: () => {
                if (instagramText.obj) {
                    instagramText.obj.value = state.instagram;
                }
            }
        };

        const twitterXText = {
            obj: null,
            create: () => {
                twitterXText.obj = new ej.inputs.TextBox({
                    placeholder: 'Enter Twitter/X',
                });
                twitterXText.obj.appendTo(twitterXRef.value);
            },
            refresh: () => {
                if (twitterXText.obj) {
                    twitterXText.obj.value = state.twitterX;
                }
            }
        };

        const tikTokText = {
            obj: null,
            create: () => {
                tikTokText.obj = new ej.inputs.TextBox({
                    placeholder: 'Enter TikTok',
                });
                tikTokText.obj.appendTo(tikTokRef.value);
            },
            refresh: () => {
                if (tikTokText.obj) {
                    tikTokText.obj.value = state.tikTok;
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
            () => state.customerGroupId,
            (newVal, oldVal) => {
                state.errors.customerGroupId = '';
                customerGroupListLookup.refresh();
            }
        );

        Vue.watch(
            () => state.customerCategoryId,
            (newVal, oldVal) => {
                state.errors.customerCategoryId = '';
                customerCategoryListLookup.refresh();
            }
        );

        Vue.watch(
            () => state.street,
            (newVal, oldVal) => {
                state.errors.street = '';
                streetText.refresh();
            }
        );

        Vue.watch(
            () => state.city,
            (newVal, oldVal) => {
                state.errors.city = '';
                cityText.refresh();
            }
        );

        Vue.watch(
            () => state.state,
            (newVal, oldVal) => {
                state.errors.state = '';
                stateText.refresh();
            }
        );

        Vue.watch(
            () => state.zipCode,
            (newVal, oldVal) => {
                state.errors.zipCode = '';
                zipCodeText.refresh();
            }
        );

        Vue.watch(
            () => state.country,
            (newVal, oldVal) => {
                state.errors.country = '';
                countryText.refresh();
            }
        );

        Vue.watch(
            () => state.phoneNumber,
            (newVal, oldVal) => {
                state.errors.phoneNumber = '';
                phoneNumberText.refresh();
            }
        );

        Vue.watch(
            () => state.emailAddress,
            (newVal, oldVal) => {
                state.errors.emailAddress = '';
                emailAddressText.refresh();
            }
        );

        const handler = {
            handleSubmit: async function () {
                try {
                    state.isSubmitting = true;
                    await new Promise(resolve => setTimeout(resolve, 200));

                    let isValid = true;

                    if (!state.name) {
                        state.errors.name = 'Name is required.';
                        isValid = false;
                    }
                    if (!state.customerGroupId) {
                        state.errors.customerGroupId = 'Customer Group is required.';
                        isValid = false;
                    }
                    if (!state.customerCategoryId) {
                        state.errors.customerCategoryId = 'Customer Category is required.';
                        isValid = false;
                    }
                    if (!state.street) {
                        state.errors.street = 'Street is required.';
                        isValid = false;
                    }
                    if (!state.city) {
                        state.errors.city = 'City is required.';
                        isValid = false;
                    }
                    if (!state.state) {
                        state.errors.state = 'State is required.';
                        isValid = false;
                    }
                    if (!state.zipCode) {
                        state.errors.zipCode = 'Zip Code is required.';
                        isValid = false;
                    }
                    if (!state.country) {
                        state.errors.country = 'Country is required.';
                        isValid = false;
                    }
                    if (!state.phoneNumber) {
                        state.errors.phoneNumber = 'Phone Number is required.';
                        isValid = false;
                    }
                    if (!state.emailAddress) {
                        state.errors.emailAddress = 'Email Address is required.';
                        isValid = false;
                    }

                    if (!isValid) return;

                    const response = state.id === ''
                        ? await services.createMainData(state.name, state.customerGroupId, state.customerCategoryId, state.description, state.street, state.city, state.state, state.zipCode, state.country, state.phoneNumber, state.faxNumber, state.emailAddress, state.website, state.whatsApp, state.linkedIn, state.facebook, state.instagram, state.twitterX, state.tikTok, StorageManager.getUserId())
                        : state.deleteMode
                            ? await services.deleteMainData(state.id, StorageManager.getUserId())
                            : await services.updateMainData(state.id, state.name, state.customerGroupId, state.customerCategoryId, state.description, state.street, state.city, state.state, state.zipCode, state.country, state.phoneNumber, state.faxNumber, state.emailAddress, state.website, state.whatsApp, state.linkedIn, state.facebook, state.instagram, state.twitterX, state.tikTok, StorageManager.getUserId());

                    if (response.data.code === 200) {
                        await methods.populateMainData();
                        mainGrid.refresh();

                        if (!state.deleteMode) {
                            state.mainTitle = 'Edit Customer';
                            state.id = response?.data?.content?.data.id ?? '';
                            state.number = response?.data?.content?.data.number ?? '';
                            state.name = response?.data?.content?.data.name ?? '';
                            state.customerGroupId = response?.data?.content?.data.customerGroupId ?? null;
                            state.customerCategoryId = response?.data?.content?.data.customerCategoryId ?? null;
                            state.description = response?.data?.content?.data.description ?? '';
                            state.street = response?.data?.content?.data.street ?? '';
                            state.city = response?.data?.content?.data.city ?? '';
                            state.state = response?.data?.content?.data.state ?? '';
                            state.zipCode = response?.data?.content?.data.zipCode ?? '';
                            state.country = response?.data?.content?.data.country ?? '';
                            state.phoneNumber = response?.data?.content?.data.phoneNumber ?? '';
                            state.faxNumber = response?.data?.content?.data.faxNumber ?? '';
                            state.emailAddress = response?.data?.content?.data.emailAddress ?? '';
                            state.website = response?.data?.content?.data.website ?? '';
                            state.whatsApp = response?.data?.content?.data.whatsApp ?? '';
                            state.linkedIn = response?.data?.content?.data.linkedIn ?? '';
                            state.facebook = response?.data?.content?.data.facebook ?? '';
                            state.instagram = response?.data?.content?.data.instagram ?? '';
                            state.twitterX = response?.data?.content?.data.twitterX ?? '';
                            state.tikTok = response?.data?.content?.data.tikTok ?? '';

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

        const resetFormState = () => {
            state.id = '';
            state.number = '';
            state.name = '';
            state.customerGroupId = null;
            state.customerCategoryId = null;
            state.description = '';
            state.street = '';
            state.city = '';
            state.state = '';
            state.zipCode = '';
            state.country = '';
            state.phoneNumber = '';
            state.faxNumber = '';
            state.emailAddress = '';
            state.website = '';
            state.whatsApp = '';
            state.linkedIn = '';
            state.facebook = '';
            state.instagram = '';
            state.twitterX = '';
            state.tikTok = '';
            state.errors = {
                name: '',
                customerGroupId: '',
                customerCategoryId: '',
                street: '',
                city: '',
                state: '',
                zipCode: '',
                country: '',
                phoneNumber: '',
                emailAddress: '',
            };
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
                    groupSettings: { columns: ['customerCategoryName'] },
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
                        { field: 'number', headerText: 'Number', width: 150, minWidth: 150 },
                        { field: 'name', headerText: 'Name', width: 200, minWidth: 200 },
                        { field: 'customerGroupName', headerText: 'Group', width: 200, minWidth: 200 },
                        { field: 'customerCategoryName', headerText: 'Category', width: 200, minWidth: 200 },
                        { field: 'street', headerText: 'Street', width: 200, minWidth: 200 },
                        { field: 'phoneNumber', headerText: 'Phone', width: 200, minWidth: 200 },
                        { field: 'emailAddress', headerText: 'Email', width: 200, minWidth: 200 },
                        { field: 'createdAtUtc', headerText: 'Created At UTC', width: 150, format: 'yyyy-MM-dd HH:mm' }
                    ],
                    toolbar: [
                        'ExcelExport', 'Search',
                        { type: 'Separator' },
                        { text: 'Add', tooltipText: 'Add', prefixIcon: 'e-add', id: 'AddCustom' },
                        { text: 'Edit', tooltipText: 'Edit', prefixIcon: 'e-edit', id: 'EditCustom' },
                        { text: 'Delete', tooltipText: 'Delete', prefixIcon: 'e-delete', id: 'DeleteCustom' },
                        { type: 'Separator' },
                        { text: 'Manage Contact', tooltipText: 'Manage Contact', id: 'ManageContactCustom' },
                    ],
                    beforeDataBound: () => { },
                    dataBound: function () {
                        mainGrid.obj.toolbarModule.enableItems(['EditCustom', 'DeleteCustom', 'ManageContactCustom'], false);
                        mainGrid.obj.autoFitColumns(['name', 'customerGroupName', 'customerCategoryName', 'street', 'phoneNumber', 'emailAddress', 'createdAtUtc']);
                    },
                    excelExportComplete: () => { },
                    rowSelected: () => {
                        if (mainGrid.obj.getSelectedRecords().length == 1) {
                            mainGrid.obj.toolbarModule.enableItems(['EditCustom', 'DeleteCustom', 'ManageContactCustom'], true);
                        } else {
                            mainGrid.obj.toolbarModule.enableItems(['EditCustom', 'DeleteCustom', 'ManageContactCustom'], false);
                        }
                    },
                    rowDeselected: () => {
                        if (mainGrid.obj.getSelectedRecords().length == 1) {
                            mainGrid.obj.toolbarModule.enableItems(['EditCustom', 'DeleteCustom', 'ManageContactCustom'], true);
                        } else {
                            mainGrid.obj.toolbarModule.enableItems(['EditCustom', 'DeleteCustom', 'ManageContactCustom'], false);
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
                            state.mainTitle = 'Add Customer';
                            resetFormState();
                            mainModal.obj.show();
                        }

                        if (args.item.id === 'EditCustom') {
                            state.deleteMode = false;
                            if (mainGrid.obj.getSelectedRecords().length) {
                                const selectedRecord = mainGrid.obj.getSelectedRecords()[0];
                                state.mainTitle = 'Edit Customer';
                                state.id = selectedRecord.id ?? '';
                                state.number = selectedRecord.number ?? '';
                                state.name = selectedRecord.name ?? '';
                                state.customerGroupId = selectedRecord.customerGroupId ?? null;
                                state.customerCategoryId = selectedRecord.customerCategoryId ?? null;
                                state.description = selectedRecord.description ?? '';
                                state.street = selectedRecord.street ?? '';
                                state.city = selectedRecord.city ?? '';
                                state.state = selectedRecord.state ?? '';
                                state.zipCode = selectedRecord.zipCode ?? '';
                                state.country = selectedRecord.country ?? '';
                                state.phoneNumber = selectedRecord.phoneNumber ?? '';
                                state.faxNumber = selectedRecord.faxNumber ?? '';
                                state.emailAddress = selectedRecord.emailAddress ?? '';
                                state.website = selectedRecord.website ?? '';
                                state.whatsApp = selectedRecord.whatsApp ?? '';
                                state.linkedIn = selectedRecord.linkedIn ?? '';
                                state.facebook = selectedRecord.facebook ?? '';
                                state.instagram = selectedRecord.instagram ?? '';
                                state.twitterX = selectedRecord.twitterX ?? '';
                                state.tikTok = selectedRecord.tikTok ?? '';
                                mainModal.obj.show();
                            }
                        }

                        if (args.item.id === 'DeleteCustom') {
                            state.deleteMode = true;
                            if (mainGrid.obj.getSelectedRecords().length) {
                                const selectedRecord = mainGrid.obj.getSelectedRecords()[0];
                                state.mainTitle = 'Delete Customer?';
                                state.id = selectedRecord.id ?? '';
                                mainModal.obj.show();
                            }
                        }

                        if (args.item.id === 'ManageContactCustom') {
                            if (mainGrid.obj.getSelectedRecords().length) {
                                const selectedRecord = mainGrid.obj.getSelectedRecords()[0];
                                state.id = selectedRecord.id ?? '';
                                state.manageContactTitle = 'Manage Contact';
                                await methods.populateSecondaryData(state.id);
                                secondaryGrid.refresh();
                                manageContactModal.obj.show();
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

        const manageContactModal = {
            obj: null,
            create: () => {
                manageContactModal.obj = new bootstrap.Modal(manageContactModalRef.value, {
                    backdrop: 'static',
                    keyboard: false
                });
            }
        };

        const secondaryGrid = {
            obj: null,
            create: async (dataSource) => {
                secondaryGrid.obj = new ej.grids.Grid({
                    height: '240px',
                    dataSource: dataSource,
                    allowFiltering: true,
                    allowSorting: true,
                    allowSelection: true,
                    allowGrouping: false,
                    allowTextWrap: true,
                    allowResizing: true,
                    allowPaging: true,
                    allowExcelExport: true,
                    editSettings: { allowEditing: true, allowAdding: true, allowDeleting: true, showDeleteConfirmDialog: true, mode: 'Normal', allowEditOnDblClick: true },
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
                        { field: 'name', headerText: 'Name', width: 200, minWidth: 200, validationRules: { required: true } },
                        { field: 'jobTitle', headerText: 'Job Title', width: 200, minWidth: 200, validationRules: { required: true } },
                        { field: 'phoneNumber', headerText: 'Phone', width: 200, minWidth: 200, validationRules: { required: true } },
                        { field: 'emailAddress', headerText: 'Email', width: 200, minWidth: 200, validationRules: { required: true } },
                        { field: 'description', headerText: 'Description', width: 400, minWidth: 400 },
                        { field: 'createdAtUtc', headerText: 'Created At UTC', width: 150, format: 'yyyy-MM-dd HH:mm' }
                    ],
                    toolbar: [
                        'ExcelExport', 'Add', 'Edit', 'Delete', 'Update', 'Cancel', 'Search'
                    ],
                    beforeDataBound: () => { },
                    dataBound: function () {
                        secondaryGrid.obj.toolbarModule.enableItems(['Edit', 'Delete'], false);
                        secondaryGrid.obj.autoFitColumns(['name', 'jobTitle', 'phoneNumber', 'emailAddress', 'description', 'createdAtUtc']);
                    },
                    excelExportComplete: () => { },
                    rowSelected: () => {
                        if (secondaryGrid.obj.getSelectedRecords().length == 1) {
                            secondaryGrid.obj.toolbarModule.enableItems(['Edit', 'Delete'], true);
                        } else {
                            secondaryGrid.obj.toolbarModule.enableItems(['Edit', 'Delete'], false);
                        }
                    },
                    rowDeselected: () => {
                        if (secondaryGrid.obj.getSelectedRecords().length == 1) {
                            secondaryGrid.obj.toolbarModule.enableItems(['Edit', 'Delete'], true);
                        } else {
                            secondaryGrid.obj.toolbarModule.enableItems(['Edit', 'Delete'], false);
                        }
                    },
                    rowSelecting: () => {
                        if (secondaryGrid.obj.getSelectedRecords().length) {
                            secondaryGrid.obj.clearSelection();
                        }
                    },
                    actionComplete: async (args) => {
                        if (args.requestType === 'save' && args.action === 'add') {
                            console.log(state);
                            const response = await services.createSecondaryData(
                                args.data.name, args.data.jobTitle, args.data.phoneNumber, args.data.emailAddress, args.data.description, state.id, StorageManager.getUserId()
                            );
                            await methods.populateSecondaryData(state.id);
                            secondaryGrid.refresh();
                            Swal.fire({
                                icon: 'success',
                                title: 'Save Successful',
                                timer: 2000,
                                showConfirmButton: false
                            });
                        }
                        if (args.requestType === 'save' && args.action === 'edit') {
                            const response = await services.updateSecondaryData(
                                args.data.id, args.data.name, args.data.jobTitle, args.data.phoneNumber, args.data.emailAddress, args.data.description, state.id, StorageManager.getUserId()
                            );
                            await methods.populateSecondaryData(state.id);
                            secondaryGrid.refresh();
                            Swal.fire({
                                icon: 'success',
                                title: 'Update Successful',
                                timer: 2000,
                                showConfirmButton: false
                            });
                        }
                        if (args.requestType === 'delete') {
                            const response = await services.deleteSecondaryData(
                                args.data[0].id, StorageManager.getUserId()
                            );
                            await methods.populateSecondaryData(state.id);
                            secondaryGrid.refresh();
                            Swal.fire({
                                icon: 'success',
                                title: 'Delete Successful',
                                timer: 2000,
                                showConfirmButton: false
                            });
                        }
                    }
                });
                secondaryGrid.obj.appendTo(secondaryGridRef.value);
            },
            refresh: () => {
                secondaryGrid.obj.setProperties({ dataSource: state.secondaryData });
            }
        };

        Vue.onMounted(async () => {
            try {
                await SecurityManager.authorizePage(['Customers']);
                await SecurityManager.validateToken();

                await methods.populateMainData();
                await mainGrid.create(state.mainData);
                await methods.populateCustomerGroupListLookupData();
                customerGroupListLookup.create();
                await methods.populateCustomerCategoryListLookupData();
                customerCategoryListLookup.create();
                nameText.create();
                numberText.create();
                streetText.create();
                cityText.create();
                stateText.create();
                zipCodeText.create();
                countryText.create();
                phoneNumberText.create();
                faxNumberText.create();
                emailAddressText.create();
                websiteText.create();
                whatsAppText.create();
                linkedInText.create();
                facebookText.create();
                instagramText.create();
                twitterXText.create();
                tikTokText.create();
                mainModal.create();
                manageContactModal.create();
                secondaryGrid.create([]);
            } catch (e) {
                console.error('page init error:', e);
            } finally {
                
            }
        });

        return {
            mainGridRef,
            mainModalRef,
            manageContactModalRef,
            secondaryGridRef,
            nameRef,
            numberRef,
            streetRef,
            cityRef,
            stateRef,
            zipCodeRef,
            countryRef,
            phoneNumberRef,
            faxNumberRef,
            emailAddressRef,
            websiteRef,
            whatsAppRef,
            linkedInRef,
            facebookRef,
            instagramRef,
            twitterXRef,
            tikTokRef,
            customerGroupIdRef,
            customerCategoryIdRef,
            state,
            handler,
        };
    }
};

Vue.createApp(App).mount('#app');