function formatNumberShort(n) {
    return new Intl.NumberFormat('vi-VN').format(n) + " VND";
}