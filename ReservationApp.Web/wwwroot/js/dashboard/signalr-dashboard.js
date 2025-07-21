const connection = new signalR.HubConnectionBuilder()
    .withUrl("/dashboardHub")
    .build();

connection.on("UserRegistered", function (data) {
    console.log("📡 UserRegistered received:", data);
    loadUserRadialChart();
});

connection.on("BookingComplete", function (data) {
    console.log("📡 BookingComplete received:", data);
    loadTotalBookingRadialChart();
});

connection.start().then(function () {
    console.log("✅ SignalR Connected.");
}).catch(function (err) {
    console.error("❌ SignalR Connection Failed:", err.toString());
});

connection.onclose(function (err) {
    console.error("⚠️ SignalR Connection closed:", err);
});