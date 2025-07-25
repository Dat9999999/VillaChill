document.getElementById("restrictOwnerBtn").addEventListener("click", function () {
    // Lọc những dòng có status là "Overdue"
    const overdueRows = table.getData().filter(row => row.status === "Overdue");

    if (overdueRows.length === 0) {
        alert("Không có owner nào quá hạn để restrict.");
        return;
    }

    // Lấy danh sách ownerId (loại bỏ trùng)
    const ownerIds = [...new Set(overdueRows.map(row => row.ownerId))];

    if (!confirm(`Restrict ${ownerIds.length} owner(s)?`)) return;

    fetch("/OwnerSettlement/RestrictOverdueOwners", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(ownerIds)
    })
        .then(res => {
            if (!res.ok) throw new Error("Request failed");
            return res.json();
        })
        .then(result => {
            alert("Đã restrict thành công.");
            location.reload();
        })
        .catch(err => {
            console.error(err);
            alert("Có lỗi xảy ra khi restrict.");
        });
});