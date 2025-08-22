window.initJalaliDatePicker = () => {
    jalaliDatepicker.startWatch({
        date: true,
        time: false,
        persianDigits: true,
        autoShow: true,
        autoHide: true,
        separatorChars: {
            date: '/',
            time: ':',
            between: ' '
        }
    });
};
window.addEventListener('DOMContentLoaded', () => {
    jalaliDatepicker.startWatch();
})
window.openModal = (id)=>{
    const modal = document.getElementById(id);
    if (modal) {
        modal.show();
        const input = modal.querySelector('[data-jdp]');
        if (input) {
            // ساخت تقویم با container داخل modal
            jalaliDatepicker.startWatch(input, {
                container: document.querySelector('dialog'), // این خط مهمه!
                autoShow: true,
                autoHide: true,
                persianDigits: true
            });
        }
    }
}
window.closeModal = (id)=>{
    const modal = document.getElementById(id);
    if (modal) {
        modal.close();
    }
}
