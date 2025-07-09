fetch('https://provinces.open-api.vn/api/p/')
    .then(response => response.json())
    .then(data => {
        const select = document.getElementById('province');
        select.innerHTML = ''; // Clear loading option

        data.forEach(province => {
            const option = document.createElement('option');
            option.value = province.name;
            option.textContent = province.name;
            select.appendChild(option);
        });
    })
    .catch(error => {
        console.error('Lỗi tải dữ liệu:', error);
    });