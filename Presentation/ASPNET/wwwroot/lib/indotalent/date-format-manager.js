const DateFormatManager = {
    formatToLocale: (date) => {

        const formatter = new Intl.DateTimeFormat('en-CA', {
            year: 'numeric',
            month: '2-digit',
            day: '2-digit',
        });
        const newDate = new Date(date)
        return formatter.format(newDate);
    },

};