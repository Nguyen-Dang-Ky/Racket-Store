// SCRIPT riêng cho trang ProductDetail.html
const API_BASE_URL = 'https://localhost:7090'; // Đảm bảo đúng API của bạn

function getRacketIdFromUrl() {
    const params = new URLSearchParams(window.location.search);
    return params.get('id'); 
}

// Hàm để chuẩn hóa đường dẫn ảnh
const getImageUrl = (imagePath) => {
    if (!imagePath) {
        return 'https://via.placeholder.com/200x200/DDDDDD/000000?text=No+Image';
    }
    if (imagePath.startsWith('PICTURE:')) {
        return `PICTURE/${imagePath.split(':')[1]}`;
    }
    return imagePath;
};

// Hàm hiển thị chi tiết sản phẩm
async function loadProductDetails() {
    const racketId = getRacketIdFromUrl();
    const productDetailContent = document.getElementById('product-detail-content');

    if (!racketId) {
        productDetailContent.innerHTML = '<p>Product ID not found in URL.</p>';
        return;
    }

    try {
        const response = await fetch(`${API_BASE_URL}/api/Rackets/${racketId}`);
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        const racketDetails = await response.json();

        if (!racketDetails) {
            productDetailContent.innerHTML = '<p>Product not found.</p>';
            return;
        }
        const responseBrands = await fetch(`${API_BASE_URL}/api/Brand`); // Đổi tên biến để tránh nhầm lẫn
        if (!responseBrands.ok) {
            throw new Error(`HTTP error! status: ${responseBrands.status}`);
        }
        const brands = await responseBrands.json(); 

        const brandName = brands.find(b => b.brandId === racketDetails.brandId)?.brandName || 'N/A';

        const imageUrl = getImageUrl(racketDetails.imageURL);

        productDetailContent.innerHTML = `
            <div class="detail-image-section">
                <img src="${imageUrl}" alt="${racketDetails.racketName}" class="detail-image">
            </div>
            <div class="detail-info-section">
                <h1>${racketDetails.racketName}</h1>
                <p><strong>Brand:</strong> ${brandName || 'N/A'}</p>
                <p><strong>Type:</strong> ${racketDetails.type || 'N/A'}</p>
                <p class="detail-price">$${racketDetails.price ? racketDetails.price.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 }) : 'N/A'}</p>
                <p><strong>Quantity:</strong> ${racketDetails.quantity || 'N/A'}</p>
                <p><strong>Code:</strong> ${racketDetails.racketCode || 'N/A'}</p>
                <p><strong>Description:</strong> ${racketDetails.description || 'No description available.'}</p>
                <button class="detail-add-to-cart-btn" data-racket-id="${racketDetails.racketId}">Add to Cart</button>
            </div>
        `;

        // Add to Cart button event listener for this page
        const addToCartBtn = productDetailContent.querySelector('.detail-add-to-cart-btn');
        if (addToCartBtn) {
            addToCartBtn.addEventListener('click', () => {
                const clickedRacketId = addToCartBtn.dataset.racketId;
                console.log(`Add racket ${clickedRacketId} to cart from detail page`);
                alert(`Đã thêm vợt ${clickedRacketId} vào giỏ hàng!`);
                // Implement your add to cart logic here
            });
        }

    } catch (error) {
        console.error('Error loading product details:', error);
        productDetailContent.innerHTML = '<p>Error loading product details. Please try again.</p>';
    }
}

// Gọi hàm khi DOM đã tải xong
document.addEventListener('DOMContentLoaded', loadProductDetails);