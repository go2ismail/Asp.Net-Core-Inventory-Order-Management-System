const App = {
    setup() {
        const state = Vue.reactive({
            mainData: [],
            deleteMode: false,
            vendorGroupListLookupData: [],
            vendorCategoryListLookupData: [],
            secondaryData: [],
            mainTitle: null,
            manageContactTitle: 'Manage Contact',
            id: '',
            name: '',
            number: '',
            vendorGroupId: null,
            vendorCategoryId: null,
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
                vendorGroupId: '',
                vendorCategoryId: '',
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
        const vendorGroupIdRef = Vue.ref(null);
        const vendorCategoryIdRef = Vue.ref(null);

        const services = {
            getMainData: async () => {
                try {
                    const response = await AxiosManager.get('/Vendor/GetVendorList', {});
                    return response;
                } catch (error) {
                    throw error;
                }
            },
            createMainData: async (name, vendorGroupId, vendorCategoryId, description, street, city, state, zipCode, country, phoneNumber, faxNumber, emailAddress, website, whatsApp, linkedIn, facebook, instagram, twitterX, tikTok, createdById) => {
                try {
                    const response = await AxiosManager.post('/Vendor/CreateVendor', {
                        name, vendorGroupId, vendorCategoryId, description, street, city, state, zipCode, country, phoneNumber, faxNumber, emailAddress, website, whatsApp, linkedIn, facebook, instagram, twitterX, tikTok, createdById
                    });
                    return response;
                } catch (error) {
                    throw error;
                }
            },
            updateMainData: async (id, name, vendorGroupId, vendorCategoryId, description, street, city, state, zipCode, country, phoneNumber, faxNumber, emailAddress, website, whatsApp, linkedIn, facebook, instagram, twitterX, tikTok, updatedById) => {
                try {
                    const response = await AxiosManager.post('/Vendor/UpdateVendor', {
                        id, name, vendorGroupId, vendorCategoryId, description, street, city, state, zipCode, country, phoneNumber, faxNumber, emailAddress, website, whatsApp, linkedIn, facebook, instagram, twitterX, tikTok, updatedById
                    });
                    return response;
                } catch (error) {
                    throw error;
                }
            },
            deleteMainData: async (id, deletedById) => {
                try {
                    const response = await AxiosManager.post('/Vendor/DeleteVendor', {
                        id, deletedById
                    });
                    return response;
                } catch (error) {
                    throw error;
                }
            },
            getVendorGroupListLookupData: async () => {
                try {
                    const response = await AxiosManager.get('/VendorGroup/GetVendorGroupList', {});
                    return response;
                } catch (error) {
                    throw error;
                }
            },
            getVendorCategoryListLookupData: async () => {
                try {
                    const response = await AxiosManager.get('/VendorCategory/GetVendorCategoryList', {});
                    return response;
                } catch (error) {
                    throw error;
                }
            },
            getSecondaryData: async (vendorId) => {
                try {
                    const response = await AxiosManager.get('/VendorContact/GetVendorContactByVendorIdList?vendorId=' + vendorId, {});
                    return response;
                } catch (error) {
                    throw error;
                }
            },
            createSecondaryData: async (name, jobTitle, phoneNumber, emailAddress, description, vendorId, createdById) => {
                try {
                    const response = await AxiosManager.post('/VendorContact/CreateVendorContact', {
                        name, jobTitle, phoneNumber, emailAddress, description, vendorId, createdById
                    });
                    return response;
                } catch (error) {
                    throw error;
                }
            },
            updateSecondaryData: async (id, name, jobTitle, phoneNumber, emailAddress, description, vendorId, updatedById) => {
                try {
                    const response = await AxiosManager.post('/VendorContact/UpdateVendorContact', {
                        id, name, jobTitle, phoneNumber, emailAddress, description, vendorId, updatedById
                    });
                    return response;
                } catch (error) {
                    throw error;
                }
            },
            deleteSecondaryData: async (id, deletedById) => {
                try {
                    const response = await AxiosManager.post('/VendorContact/DeleteVendorContact', {
                        id, deletedById
                    });
                    return response;
                } catch (error) {
                    throw error;
                }
            },
        };

        const methods = {
            populateVendorGroupListLookupData: async () => {
                const response = await services.getVendorGroupListLookupData();
                state.vendorGroupListLookupData = response?.data?.content?.data;
            },
            populateVendorCategoryListLookupData: async () => {
                const response = await services.getVendorCategoryListLookupData();
                state.vendorCategoryListLookupData = response?.data?.content?.data;
            },
            populateMainData: async () => {
                const response = await services.getMainData();
                state.mainData = response?.data?.content?.data.map(item => ({
                    ...item,
                    createdAtUtc: new Date(item.createdAtUtc)
                }));
            },
            populateSecondaryData: async (vendorId) => {
                const response = await services.getSecondaryData(vendorId);
                state.secondaryData = response?.data?.content?.data.map(item => ({
                    ...item,
                    createdAtUtc: new Date(item.createdAtUtc)
                }));
            },
        };

        const vendorGroupListLookup = {
            obj: null,
            create: () => {
                if (state.vendorGroupListLookupData && Array.isArray(state.vendorGroupListLookupData)) {
                    vendorGroupListLookup.obj = new ej.dropdowns.DropDownList({
                        dataSource: state.vendorGroupListLookupData,
                        fields: { value: 'id', text: 'name' },
                        placeholder: 'Select a Vendor Group',
                        change: (e) => {
                            state.vendorGroupId = e.value;
                        }
                    });
                    vendorGroupListLookup.obj.appendTo(vendorGroupIdRef.value);
                } else {
                    console.error('Vendor Group list lookup data is not available or invalid.');
                }
            },
            refresh: () => {
                if (vendorGroupListLookup.obj) {
                    vendorGroupListLookup.obj.value = state.vendorGroupId;
                }
            },
        };

        const vendorCategoryListLookup = {
            obj: null,
            create: () => {
                if (state.vendorCategoryListLookupData && Array.isArray(state.vendorCategoryListLookupData)) {
                    vendorCategoryListLookup.obj = new ej.dropdowns.DropDownList({
                        dataSource: state.vendorCategoryListLookupData,
                        fields: { value: 'id', text: 'name' },
                        placeholder: 'Select a Vendor Category',
                        change: (e) => {
                            state.vendorCategoryId = e.value;
                        }
                    });
                    vendorCategoryListLookup.obj.appendTo(vendorCategoryIdRef.value);
                } else {
                    console.error('Vendor Category list lookup data is not available or invalid.');
                }
            },
            refresh: () => {
                if (vendorCategoryListLookup.obj) {
                    vendorCategoryListLookup.obj.value = state.vendorCategoryId;
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
            () => state.vendorGroupId,
            (newVal, oldVal) => {
                state.errors.vendorGroupId = '';
                vendorGroupListLookup.refresh();
            }
        );

        Vue.watch(
            () => state.vendorCategoryId,
            (newVal, oldVal) => {
                state.errors.vendorCategoryId = '';
                vendorCategoryListLookup.refresh();
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
                    if (!state.vendorGroupId) {
                        state.errors.vendorGroupId = 'Vendor Group is required.';
                        isValid = false;
                    }
                    if (!state.vendorCategoryId) {
                        state.errors.vendorCategoryId = 'Vendor Category is required.';
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
                        ? await services.createMainData(state.name, state.vendorGroupId, state.vendorCategoryId, state.description, state.street, state.city, state.state, state.zipCode, state.country, state.phoneNumber, state.faxNumber, state.emailAddress, state.website, state.whatsApp, state.linkedIn, state.facebook, state.instagram, state.twitterX, state.tikTok, StorageManager.getUserId())
                        : state.deleteMode
                            ? await services.deleteMainData(state.id, StorageManager.getUserId())
                            : await services.updateMainData(state.id, state.name, state.vendorGroupId, state.vendorCategoryId, state.description, state.street, state.city, state.state, state.zipCode, state.country, state.phoneNumber, state.faxNumber, state.emailAddress, state.website, state.whatsApp, state.linkedIn, state.facebook, state.instagram, state.twitterX, state.tikTok, StorageManager.getUserId());

                    if (response.data.code === 200) {
                        await methods.populateMainData();
                        mainGrid.refresh();

                        if (!state.deleteMode) {
                            state.mainTitle = 'Edit Vendor';
                            state.id = response?.data?.content?.data.id ?? '';
                            state.number = response?.data?.content?.data.number ?? '';
                            state.name = response?.data?.content?.data.name ?? '';
                            state.vendorGroupId = response?.data?.content?.data.vendorGroupId ?? null;
                            state.vendorCategoryId = response?.data?.content?.data.vendorCategoryId ?? null;
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
            state.vendorGroupId = null;
            state.vendorCategoryId = null;
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
                vendorGroupId: '',
                vendorCategoryId: '',
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
                    groupSettings: { columns: ['vendorCategoryName'] },
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
                        { field: 'vendorGroupName', headerText: 'Group', width: 200, minWidth: 200 },
                        { field: 'vendorCategoryName', headerText: 'Category', width: 200, minWidth: 200 },
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
                        mainGrid.obj.autoFitColumns(['name', 'vendorGroupName', 'vendorCategoryName', 'street', 'phoneNumber', 'emailAddress', 'createdAtUtc']);
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
                            state.mainTitle = 'Add Vendor';
                            resetFormState();
                            mainModal.obj.show();
                        }

                        if (args.item.id === 'EditCustom') {
                            state.deleteMode = false;
                            if (mainGrid.obj.getSelectedRecords().length) {
                                const selectedRecord = mainGrid.obj.getSelectedRecords()[0];
                                state.mainTitle = 'Edit Vendor';
                                state.id = selectedRecord.id ?? '';
                                state.number = selectedRecord.number ?? '';
                                state.name = selectedRecord.name ?? '';
                                state.vendorGroupId = selectedRecord.vendorGroupId ?? null;
                                state.vendorCategoryId = selectedRecord.vendorCategoryId ?? null;
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
                                state.mainTitle = 'Delete Vendor?';
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
                await SecurityManager.authorizePage(['Vendors']);
                await SecurityManager.validateToken();

                await methods.populateMainData();
                await mainGrid.create(state.mainData);
                await methods.populateVendorGroupListLookupData();
                vendorGroupListLookup.create();
                await methods.populateVendorCategoryListLookupData();
                vendorCategoryListLookup.create();
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
            vendorGroupIdRef,
            vendorCategoryIdRef,
            state,
            handler,
        };
    }
};

Vue.createApp(App).mount('#app');
