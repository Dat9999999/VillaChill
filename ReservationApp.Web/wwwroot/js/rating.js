document.getElementById("reviewForm").addEventListener("submit", async function (e) {
    e.preventDefault();

    const score = parseInt(document.getElementById("score").value);
    const comment = document.getElementById("comment").value;
    const bookingId = parseInt(document.getElementById("bookingId").value);
    const VillaId = parseInt(document.getElementById("villaId").value);
    const Name = document.getElementById("Name").value;


    if (isNaN(score) || score < 0 || score > 10) {
        alert("Please enter a valid score between 0 and 10.");
        return;
    }
    console.log(Name);

    try {
        const response = await fetch("/rating/create", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ bookingId, score, comment, VillaId, Name })
        });

        if (response.ok) {
            const data = await response.json();
            alert(data.message);
        } else {
            const err = await response.text();
            alert("Error: " + err);
        }

        const modal = bootstrap.Modal.getInstance(document.getElementById("ratingModal"));
        modal.hide();
        this.reset();
    } catch (error) {
        console.error("Error:", error);
        alert("Sends rating failed. Please try again later.");
    }
});
