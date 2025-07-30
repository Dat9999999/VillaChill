document.addEventListener('DOMContentLoaded', function () {
    const amenitiesBtn = document.getElementById('tab-amenities-btn');
    const reviewsBtn = document.getElementById('tab-reviews-btn');
    const amenitiesTab = document.getElementById('tab-amenities');
    const reviewsTab = document.getElementById('tab-reviews');

    amenitiesBtn.addEventListener('click', function () {
        amenitiesTab.style.display = 'block';
        reviewsTab.style.display = 'none';

        amenitiesBtn.classList.add('active');
        reviewsBtn.classList.remove('active');
    });

    reviewsBtn.addEventListener('click', function () {
        amenitiesTab.style.display = 'none';
        reviewsTab.style.display = 'block';

        reviewsBtn.classList.add('active');
        amenitiesBtn.classList.remove('active');
    });
});