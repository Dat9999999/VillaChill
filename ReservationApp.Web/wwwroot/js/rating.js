const container = document.querySelector(".ratings-box");

const observer = new IntersectionObserver((entries, obs) => {
    entries.forEach(entry => {
        if (entry.isIntersecting) {
            const el = entry.target;
            const villaId = el.getAttribute("data-villa-id");
            const alreadyLoaded = el.getAttribute("data-loaded");

            if (alreadyLoaded === "true") return;

            // Gọi API
            fetch(`${GetRatingById}+${villaId}`)
                .then(res => res.json())
                .then(data => {
                    renderRatings(el, data);
                    el.setAttribute("data-loaded", "true"); // Đánh dấu đã load
                    obs.unobserve(el); // Không theo dõi nữa
                })
                .catch(err => {
                    console.error(err);
                    el.innerHTML = "<p class='text-danger p-2'>Failed to load ratings.</p>";
                });
        }
    });
}, { threshold: 0.3 });

observer.observe(container);

function renderRatings(container, ratings) {
    if (!ratings || ratings.length === 0) {
        container.innerHTML += `<p class='text-body-secondary p-2'>No comments yet. Be the first to review this villa!</p>`;
        return;
    }

    const items = ratings.map(r => `
            <div class="d-flex mb-3">
                <img src="/images/placeholder.png" class="rounded-circle me-3" style="width: 40px; height: 40px;" alt="avatar" />
                <div class="w-100" style="background-color: #f0f2f5; border-radius: 16px; padding: 10px 15px;">
                    <div class="fw-semibold text-dark mb-1" style="font-size: 14px;">
                        ${r.customerName}
                        <span class="text-body-secondary" style="font-size: 12px;"> · ${new Date(r.date).toLocaleDateString()}</span>
                    </div>
                    <div class="text-dark mb-1" style="font-size: 15px;">
                        ${r.comment || "No comment"}
                    </div>
                    <span class="badge bg-success">Score: ${r.score} / 10</span>
                </div>
            </div>
        `);
    container.innerHTML = container.querySelector(".sticky-top").outerHTML + items.join("");
}