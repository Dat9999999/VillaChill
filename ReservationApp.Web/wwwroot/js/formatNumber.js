function formatNumberShort(n) {
    if (n >= 1_000_000_000) return (n / 1_000_000_000).toFixed(1) + 'B VND';
    if (n >= 1_000_000) return (n / 1_000_000).toFixed(1) + 'M VND';
    if (n >= 1_000) return (n / 1_000).toFixed(1) + 'K VND';
    return n.toLocaleString('vi-VN') + ' VND'; // hiển thị dạng "1.000 VND"
}
