function domReady(fn) {
    if (
        document.readyState === "complete" ||
        document.readyState === "interactive"
    ) {
        setTimeout(fn, 1000);
    } else {
        document.addEventListener("DOMContentLoaded", fn);
    }
}

domReady(function () {

    const bookingId = document.getElementById("my-qr-reader").dataset.bookingId;
    // If found you qr code
    function onScanSuccess(decodeText, decodeResult) {
        console.log("Scanned:", decodeText);

        fetch('/Booking/CheckinByQR', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                bookingId: bookingId,
                checkinToken: decodeText
            })
        })
            .then(res => {
                if (!res.ok) throw new Error("Server error");
                return res.json();
            })
            .then(data => {
                if (data.success) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Check-in thành công!',
                        showConfirmButton: false,
                        timer: 2000
                    }).then(() => {
                        window.location.href = data.redirectUrl;
                        htmlscanner.clear(); // stop scanner after success
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Mã QR không hợp lệ'
                    });
                }
            })
            .catch(err => {
                alert("❌ Error: invalid QR" + err.message);
            });

        
    }

    let htmlscanner = new Html5QrcodeScanner(
        "my-qr-reader",
        { fps: 10, qrbos: 250 }
    );
    htmlscanner.render(onScanSuccess);
});