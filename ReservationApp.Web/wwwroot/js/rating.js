document.getElementById("reviewForm").addEventListener("submit", function (e) {
    e.preventDefault();

    const score = parseInt(document.getElementById("score").value);
    const comment = document.getElementById("comment").value;
    const bookingId = document.getElementById("bookingId").value;

    if (isNaN(score) || score < 0 || score > 10) {
        alert("Vui lòng nhập điểm từ 0 đến 10.");
        return;
    }

    alert(`Booking: ${bookingId}\nĐiểm: ${score}/10\nNhận xét: ${comment}`);
    const modal = bootstrap.Modal.getInstance(document.getElementById("ratingModal"));
    modal.hide(); // đóng modal
    this.reset(); // xóa form
});