const API = 'https://localhost:7090'

const brandListContainer = document.getElementById('brand-list')
const productSectionContainer = document.querySelector('.product-sections')
const cartAlert = document.getElementById('cartAlert')

//  HÀM LẤY HÌNH ẢNH
const getImageUrl = (imagePath) => {
    if(!imagePath) 
    {
        return 'https://via.placeholder.com/200x200/DDDDDD/000000?text=No+Image'
    }
    if (imagePath.startsWith('PICTURE:')) {
        return `PICTURE/${imagePath.split(':')[1]}`
    }
    return imagePath
}

//  HÀM GỬI API REQUEST
async function apiRequest(url, method = 'GET', data = null)
{
    const headers = {
        'Content-Type': 'application/json',
    }

    const userToken = localStorage.getItem('userToken')
    if (userToken)
    {
        headers['Authorization'] = `Bearer ${userToken}`
    }

    const config = {
        method: method,
        headers: headers,
    }
    if (data)
    {
        config.body = JSON.stringify(data)
    }

    try 
    {
        const response = await fetch(url, config)
        let responseData = null

        try
        {
            responseData = await response.json()
        }
        catch (e) 
        {
            responseData = { message: await response.text() || response.statusText }
        }

        if (!response.ok)
        {
            const errorMessage  = responseData.message || `Lỗi API (${response.status}): ${response.statusText}`
            throw new Error(errorMessage)
        }
        return responseData
    }
    catch (error)
    {
        console.error('lỗi khi gửi API request', error)
        throw error
    }
}

//  HÀM LẤY USER ID TỪ LOCAL STORAGE
function getLoggedInUserId() 
{
    const currentUserJson = localStorage.getItem('currentUser')
    if (currentUserJson)
    {
        try
        {
            const currentUser = JSON.parse(currentUserJson)
            return currentUser.userId
        } catch(e)
        {
            console.error("Lỗi khi phân tích currentUser từ localStorage:", e)
            localStorage.removeItem('currentUser')
            localStorage.removeItem('userToken')
            return null
        }
    }
    return null
}

//  HÀM HIỂN THỊ THÔNG BÁO GIỎ HÀNG
function displayCartAlert(message, type) 
{
    if (cartAlert)
    {
        cartAlert.textContent = message;
        cartAlert.style.display = 'block';
        cartAlert.style.backgroundColor = type === 'success' ? '#d4edda' : '#f8d7da';
        cartAlert.style.color = type === 'success' ? '#155724' : '#721c24';
        cartAlert.style.border = type === 'success' ? '1px solid #c3e6cb' : '1px solid #f5c6cb';
        cartAlert.style.padding = '10px';
        cartAlert.style.borderRadius = '5px';
        cartAlert.style.marginBottom = '15px';

        setTimeout(() => {
            cartAlert.style.display = 'none'
        }, 3000)
    }
    else
    {
        alert(message)
    }
}

async function fetchAndDisplayBrands()
{
    try {
        const brands = await apiRequest(`${API}/api/Brand`);

        if (brandListContainer) {
            brandListContainer.innerHTML = '';
            if (brands.length === 0) {
                brandListContainer.innerHTML = '<p>Không có thương hiệu nào.</p>';
                return;
            }

            brands.forEach(brand => {
                const brandItem = document.createElement('a');
                brandItem.href = `#brand-${brand.brandId}`;
                brandItem.classList.add('brand-item');
                brandItem.textContent = brand.brandName;
                brandListContainer.appendChild(brandItem);
            });
        }
    } catch (error) {
        console.error('Lỗi khi lấy thương hiệu:', error);
        if (brandListContainer) {
            brandListContainer.innerHTML = '<p>Không thể tải thương hiệu. Vui lòng đảm bảo API Brands của bạn đang chạy và có thể truy cập.</p>';
        }
    }
}

async function fetchAndDisplayRackets() {
    try {
        const rackets = await apiRequest(`${API}/api/Rackets`)

        if (productSectionContainer) {
            productSectionContainer.innerHTML = ''

            if (rackets.length === 0) {
                productSectionContainer.innerHTML = '<p>Không có sản phẩm nào.</p>'
                return
            }

            const mainProductCategorySection = document.createElement('section')
            mainProductCategorySection.classList.add('product-category')

            const categoryTitle = document.createElement('h2')
            categoryTitle.classList.add('category-title')
            categoryTitle.textContent = 'RACKET BADMINTON'

            const productGrid = document.createElement('div')
            productGrid.classList.add('product-grid')
            productGrid.id = 'all-rackets-grid'

            rackets.forEach(racket => {
                const productCard = document.createElement('div')
                productCard.classList.add('product-card');
                productCard.dataset.racketId = racket.racketId

                const imageUrl = getImageUrl(racket.imageURL)

                productCard.innerHTML = `
                    <img src="${imageUrl}"
                        alt="${racket.racketName}" class="product-image">
                    <h3 class="product-name">${racket.racketName}</h3>
                    <p class="product-price">$${racket.price ? racket.price.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 }) : 'N/A'}</p>
                    <button class="add-to-cart-btn" data-racket-id="${racket.racketId}">Add to Cart</button>
                `;
                productGrid.appendChild(productCard)


                const addToCartBtn = productCard.querySelector('.add-to-cart-btn')
                if (addToCartBtn) {
                    addToCartBtn.addEventListener('click', async (event) => {
                        event.stopPropagation()
                        
                        const racketId = parseInt(event.target.dataset.racketId)
                        const userId = getLoggedInUserId() 

                        if (!userId) {
                            displayCartAlert('Bạn cần đăng nhập để thêm sản phẩm vào giỏ hàng.', 'error')
                            setTimeout(() => {
                                window.location.href = 'Login.html'
                            }, 1500)
                            return
                        }
                        
                        const newCartItem = {
                            userId: userId, 
                            racketId: racketId,
                            quantity: 1 
                        };

                        try {

                            const response = await apiRequest(`${API}/api/CartItem/add`, 'POST', newCartItem);
                            console.log('Sản phẩm đã được thêm vào giỏ hàng:', response);
                            displayCartAlert('Sản phẩm đã được thêm vào giỏ hàng thành công!', 'success');

                        } catch (error) {
                            console.error('Lỗi khi thêm sản phẩm vào giỏ hàng:', error);
                            displayCartAlert('Có lỗi xảy ra khi thêm sản phẩm vào giỏ hàng. Vui lòng thử lại.', 'error');
                        }
                    });
                }

                productCard.addEventListener('click', (event) => {
                    if (!event.target.classList.contains('add-to-cart-btn')) {
                        const clickedRacketId = event.currentTarget.dataset.racketId
                        window.location.href = `ProductDetails.html?id=${clickedRacketId}`
                    }
                })
            })

            mainProductCategorySection.appendChild(categoryTitle);
            mainProductCategorySection.appendChild(productGrid);
            productSectionContainer.appendChild(mainProductCategorySection);
        }
    } catch (error) {
        console.error('Lỗi khi lấy vợt:', error);
        if (productSectionContainer) {
            productSectionContainer.innerHTML = '<p>Không thể tải vợt. Vui lòng đảm bảo API Rackets của bạn đang chạy và có thể truy cập.</p>';
        }
    }
}

document.addEventListener('DOMContentLoaded', () => {
    fetchAndDisplayBrands()
    fetchAndDisplayRackets()
})