const NumberFormatManager = {
    formatToLocale: (number) => {
        const userLocale = navigator.language || 'en-US';

        const formatter = new Intl.NumberFormat(userLocale, {
            style: 'decimal',
            minimumFractionDigits: 2,
            maximumFractionDigits: 2,
        });

        if (typeof number !== 'number') {
            console.warn("Input must be a number, default value returned: 0");
            number = 0;
        }
        return formatter.format(number);
    },

};