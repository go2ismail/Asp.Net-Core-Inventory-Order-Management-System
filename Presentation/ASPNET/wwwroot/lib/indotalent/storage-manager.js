const STORAGE_KEYS = {
    ACCESS_TOKEN: 'accessToken',
    REFRESH_TOKEN: 'refreshToken',
    IS_AUTHENTICATED: 'isAuthenticated',
    FIRST_NAME: 'firstName',
    LAST_NAME: 'lastName',
    EMAIL: 'email',
    USER_ID: 'userId',
    USER_ROLES: 'userRoles',
    MENU_NAVIGATION: 'menuNavigation',
    AVATAR: 'avatar',
    COMPANY: 'company'
};

const StorageManager = {
    save: (key, value) => {
        try {
            localStorage.setItem(key, JSON.stringify(value));
        } catch (error) {
            console.error('Failed to save data to localStorage', error);
        }
    },

    get: (key) => {
        try {
            const value = localStorage.getItem(key);
            return value ? JSON.parse(value) : null;
        } catch (error) {
            console.error('Failed to retrieve data from localStorage', error);
            return null;
        }
    },

    remove: (key) => {
        try {
            localStorage.removeItem(key);
        } catch (error) {
            console.error('Failed to remove data from localStorage', error);
        }
    },

    clearStorage: () => {
        try {
            localStorage.clear();
        } catch (error) {
            console.error('Failed to clear localStorage', error);
        }
    },

    saveAccessToken: (token) => StorageManager.save(STORAGE_KEYS.ACCESS_TOKEN, token),
    getAccessToken: () => StorageManager.get(STORAGE_KEYS.ACCESS_TOKEN),
    removeAccessToken: () => StorageManager.remove(STORAGE_KEYS.ACCESS_TOKEN),

    saveRefreshToken: (token) => StorageManager.save(STORAGE_KEYS.REFRESH_TOKEN, token),
    getRefreshToken: () => StorageManager.get(STORAGE_KEYS.REFRESH_TOKEN),
    removeRefreshToken: () => StorageManager.remove(STORAGE_KEYS.REFRESH_TOKEN),

    saveIsAuthenticated: (status) => StorageManager.save(STORAGE_KEYS.IS_AUTHENTICATED, status),
    getIsAuthenticated: () => StorageManager.get(STORAGE_KEYS.IS_AUTHENTICATED),
    removeIsAuthenticated: () => StorageManager.remove(STORAGE_KEYS.IS_AUTHENTICATED),

    saveFirstName: (firstName) => StorageManager.save(STORAGE_KEYS.FIRST_NAME, firstName),
    getFirstName: () => StorageManager.get(STORAGE_KEYS.FIRST_NAME),
    removeFirstName: () => StorageManager.remove(STORAGE_KEYS.FIRST_NAME),

    saveLastName: (lastName) => StorageManager.save(STORAGE_KEYS.LAST_NAME, lastName),
    getLastName: () => StorageManager.get(STORAGE_KEYS.LAST_NAME),
    removeLastName: () => StorageManager.remove(STORAGE_KEYS.LAST_NAME),

    saveEmail: (email) => StorageManager.save(STORAGE_KEYS.EMAIL, email),
    getEmail: () => StorageManager.get(STORAGE_KEYS.EMAIL),
    removeEmail: () => StorageManager.remove(STORAGE_KEYS.EMAIL),

    saveUserId: (userId) => StorageManager.save(STORAGE_KEYS.USER_ID, userId),
    getUserId: () => StorageManager.get(STORAGE_KEYS.USER_ID),
    removeUserId: () => StorageManager.remove(STORAGE_KEYS.USER_ID),

    saveUserRoles: (roles) => StorageManager.save(STORAGE_KEYS.USER_ROLES, roles),
    getUserRoles: () => StorageManager.get(STORAGE_KEYS.USER_ROLES),
    removeUserRoles: () => StorageManager.remove(STORAGE_KEYS.USER_ROLES),

    saveMenuNavigation: (navigations) => StorageManager.save(STORAGE_KEYS.MENU_NAVIGATION, navigations),
    getMenuNavigation: () => StorageManager.get(STORAGE_KEYS.MENU_NAVIGATION),
    removeMenuNavigation: () => StorageManager.remove(STORAGE_KEYS.MENU_NAVIGATION),

    saveAvatar: (avatar) => StorageManager.save(STORAGE_KEYS.AVATAR, avatar),
    getAvatar: () => StorageManager.get(STORAGE_KEYS.AVATAR),
    removeAvatar: () => StorageManager.remove(STORAGE_KEYS.AVATAR),

    saveCompany: (company) => StorageManager.save(STORAGE_KEYS.COMPANY, company),
    getCompany: () => StorageManager.get(STORAGE_KEYS.COMPANY),
    removeCompany: () => StorageManager.remove(STORAGE_KEYS.COMPANY),

    saveLoginResult: (data) => {
        StorageManager.saveAccessToken(data?.content?.data?.accessToken);
        StorageManager.saveRefreshToken(data?.content?.data?.refreshToken);
        StorageManager.saveFirstName(data?.content?.data?.firstName);
        StorageManager.saveLastName(data?.content?.data?.lastName);
        StorageManager.saveEmail(data?.content?.data?.email);
        StorageManager.saveUserId(data?.content?.data?.userId);
        StorageManager.saveUserRoles(data?.content?.data?.roles);
        StorageManager.saveMenuNavigation(data?.content?.data?.menuNavigation);
        StorageManager.saveIsAuthenticated(StorageManager.getUserId() != null);
        StorageManager.saveAvatar(data?.content?.data?.avatar);
    }
};
