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
            document.getElementById("bulkPayBtn").addEventListener("click", function () {
                const selectedRows = table.getSelectedData(); // lấy dữ liệu các dòng đã chọn

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
                    .then(res => res.text()) // 👈 KHÔNG dùng .json()
                    .then(url => {
                        window.location.href = url; // ✅ chuyển hướng đúng
                    })
                    .catch(err => {
                        console.error("Lỗi redirect:", err);
                        alert("Không tạo được link thanh toán!");
                    });
            });

        });
});