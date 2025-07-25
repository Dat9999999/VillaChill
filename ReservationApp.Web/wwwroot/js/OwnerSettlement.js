document.addEventListener("DOMContentLoaded", function () {
    fetch("/OwnerSettlement/GetAllSettlements")
        .then(response => response.json())
        .then(data => {
            console.log(data);
            const table = new Tabulator("#myTable", {
                data: data,
                layout: "fitColumns",
                height: "100%",
                pagination: "local",
                paginationSize: 5,
                columns: [
                    { title: "", formatter: "rowSelection", titleFormatter: "rowSelection", hozAlign: "center", headerSort: false, width: 50 },
                    { title: "Booking ID", field: "bookingId", sorter: "number", width: 100 },
                    { title: "Owner ID", field: "ownerId", sorter: "string" },
                    { title: "Amount", field: "amount", sorter: "number", formatter: "money", formatterParams: { symbol: "₫", thousand: ",", precision: 0 } },
                    { title: "Commission (%)", field: "commissionRate", sorter: "number", formatter: "money", formatterParams: { symbol: "", precision: 2 } },
                    { title: "Status", field: "status", sorter: "string" },
                    {
                        title: "Due Date",
                        field: "dueDate",
                        sorter: "datetime",
                        hozAlign: "center",
                        formatter: function(cell) {
                            const value = cell.getValue();
                            return value ? dayjs(value).format("DD/MM/YYYY") : "";
                        }
                    },
                ]
            });
            table.setFilter("status", "=", "Unpaid");
            document.getElementById("status-filter").addEventListener("change", function () {
                const value = this.value;
                if (value) {
                    table.setFilter("status", "=", value);
                } else {
                    table.clearFilter();
                }
            });
            const bulkPayBtn = document.getElementById("bulkPayBtn");
            if (bulkPayBtn) {
                bulkPayBtn.addEventListener("click", function () {
                    const selectedRows = table.getSelectedData();

                    if (selectedRows.length === 0) {
                        alert("Vui lòng chọn ít nhất 1 dòng để thanh toán.");
                        return;
                    }

                    const bookingIds = selectedRows.map(row => row.bookingId);

                    if (!confirm(`Xác nhận thanh toán ${bookingIds.length} booking?`)) return;

                    fetch("/OwnerSettlement/BulkPay", {
                        method: "POST",
                        headers: {
                            "Content-Type": "application/json"
                        },
                        body: JSON.stringify(bookingIds)
                    })
                        .then(res => res.text())
                        .then(url => {
                            window.location.href = url;
                        })
                        .catch(err => {
                            console.error("Lỗi redirect:", err);
                            alert("Không tạo được link thanh toán!");
                        });
                });
            }
            const restrictBtn = document.getElementById("restrictOwnerBtn");
            if (restrictBtn) {
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
            }
        });
});