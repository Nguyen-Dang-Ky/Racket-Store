const API = 'https://localhost:7090'

const cartItemsContainer = document.getElementById('cart-items')
const cartTotalPriceElement = document.getElementById('cart-total')
const checkoutButton = document.querySelector('.checkout-btn')
const cartAlert = document.getElementById('cartAlert')
const noItemMessage = document.querySelector('.no-items-message')

/*  HÀM LẤY URL HÌNH ẢNH */
const getImageUrl = (imagePath) => {
    if (!imagePath)
    {
        return 'https://via.placeholder.com/100x100/DDDDDD/000000?text=No+Image'
    }
    if (imagePath.startsWith('PICTURE:'))
    {
        return `PICTURE/${imagePath.split(':')[1]}`
    }
    return imagePath
}

/*  HÀM GỬI API REQUEST */
async function apiRequest (url, method = 'GET', data = null)
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
        const clonedResponse = response.clone()

        let dataFromResponse = null
        try
        {
            dataFromResponse = await response.json()
        }
        catch (jsonError)
        {
            try
            {
                dataFromResponse = { message: await clonedResponse.text() || clonedResponse.statusText }
            }
            catch (textError)
            {
                dataFromResponse = { message: `Phản hồi không đọc được: ${textError.message}` };
            }
        }
        if (!response.ok)
        {
            const errorMessage = dataFromResponse.message || `Lỗi API (${response.status}): ${response.statusText}`
            throw new Error(errorMessage)
        }
        return dataFromResponse
    }
    catch (error)
    {
        console.error('Error when send API request', error)
        throw error
    }
}

/*  HÀM LẤY USER ID TỪ LOCAL STORAGE */
function getLoggedInUserId()
{
    const currentUserJson = localStorage.getItem('currentUser')
    if (currentUserJson)
    {
        try
        {
            const currentUser = JSON.parse(currentUserJson)
            return currentUser.userId
        }
        catch (e) 
        {
            console.error('Error when explain currentUser from localStorage:', e)
        }
    }
    return null
}

/*  HÀM HIỂN THỊ THÔNG BÁO GIỎ HÀNG   */
function displayCartAlert(message, type)
{
    if (cartAlert)
    {
        cartAlert.textContent = message
        cartAlert.classList.remove('success', 'error')
        cartAlert.classList.add(type)
        cartAlert.style.display = 'block'
        cartAlert.classList.add('show')
        
        setTimeout(() => {
           cartAlert.classList.remove('show')
           setTimeout(() => {
                cartAlert.style.display = 'none'
           }, 500); 
        }, 3000);
    }
    else
    {
        alert(message)
    }
}

/*  HÀM HIỂN THỊ CÁC SẢN PHẨM TRONG GIỎ HÀNG */
async function displayCartItems()
{
    console.log("displayCartItems function started.")
    const userId = getLoggedInUserId()

    if (!userId)
    {
        displayCartAlert('You have to be login to view cart.', 'error')
        cartItemsContainer.innerHTML = ''
        noItemMessage.style.display = 'block'
        cartTotalPriceElement.textContent = '0 VNĐ'
        setTimeout(() => {
           window.location.href = 'Login.html' 
        }, 1500);
        return
    }

    cartItemsContainer.innerHTML = ''
    noItemMessage.style.display = 'none'
    let totalPrice = 0

    try 
    {
        const cartItems = await apiRequest(`${API}/api/CartItem/${userId}`, 'GET')

        if (!cartItems || cartItems.length === 0)
        {
            if (noItemMessage) noItemMessage.style.display = 'block'
            if (cartTotalPriceElement) cartTotalPriceElement.textContent = '0 VNĐ'
            console.log("Cart is empty.")
            return
        }

        noItemMessage.style.display = 'none'

        cartItems.forEach(item => {
            const itemPrice = item.price || 0
            const itemTotal = item.quantity * itemPrice
            totalPrice += itemTotal;

            const cartItemDiv = document.createElement('div');
            cartItemDiv.classList.add('cart-item');
            cartItemDiv.dataset.cartItemId = item.cartItemId;

            const imageUrl = getImageUrl(item.imageURL); // Sửa: Dùng item.imageUrl
            console.log("Render cart item:", item);

            cartItemDiv.innerHTML = `
                <img src="${imageUrl}" alt="${item.racketName}" class="cart-item-image">
                <div class="cart-item-details">
                    <h3>${item.racketName}</h3>
                    <p>Giá: ${itemPrice.toLocaleString('vi-VN')} VNĐ</p>
                </div>
                <div class="cart-item-quantity-controls">
                    <button class="decrease-quantity" data-cart-item-id="${item.cartId}">-</button>
                    <input type="number" value="${item.quantity}" min="1" class="item-quantity-input" data-cart-item-id="${item.cartId}">
                    <button class="increase-quantity" data-cart-item-id="${item.cartId}">+</button>
                </div>
                <div class="cart-item-total">${itemTotal.toLocaleString('vi-VN')} VNĐ</div>
                <div class="cart-item-remove">
                    <button class="remove-item-btn" data-cart-item-id="${item.cartId}">
                        <i class="fas fa-trash-alt"></i> Xóa
                    </button>
                </div>
            `;
            cartItemsContainer.appendChild(cartItemDiv);
        })
        cartTotalPriceElement.textContent = totalPrice.toLocaleString('vi-VN') + ' VNĐ'
        attachCartEventListeners()
        console.log("Event listeners attached after rendering cart items.")
    }
    catch (error) 
    {
        console.error('Lỗi khi lấy giỏ hàng:', error);
        displayCartAlert(error.message || 'Không thể tải giỏ hàng. Vui lòng đảm bảo API của bạn đang chạy và có thể truy cập.', 'error');
        if(noItemMessage) noItemMessage.style.display = 'block';
        if (cartTotalPriceElement) cartTotalPriceElement.textContent = '0 VNĐ';
    }
}
/*  HÀM CẬP NHẬT QUANTITY UI CỤC BỘ VÀ GỌI API */
async function updateCartItemQuantityUiAndBackend(cartItemId, newQuantity, inputElement)    
{
    const cartItemDiv = inputElement.closest('.cart-item');
    const itemPriceElement = cartItemDiv.querySelector('.cart-item-details p')
    const itemTotalElement = cartItemDiv.querySelector('.cart-item-total')

    const priceText = itemPriceElement.textContent.replace('Giá:', '').replace('VNĐ', '').trim()
    const itemPrice = parseFloat(priceText.replace(/\./g, '').replace(/,/g, '.'))

    if (isNaN(itemPrice))
    {
        console.error("Không thể phân tích giá sản phẩm từ UI:", priceText)
        displayCartAlert('Lỗi: Không thể cập nhật giá sản phẩm cục bộ. Đang tải lại giỏ hàng.', 'error')
        await displayCartItems()
        return
    }
    const newItemTotal = newQuantity * itemPrice
    itemTotalElement.textContent = newItemTotal.toLocaleString('vi-VN') + ' VNĐ'

    updateCartTotalLocally()
    try 
    {
        const response = await apiRequest(`${API}/api/CartItem/${cartItemId}`, 'PUT', { quantity: newQuantity })
        displayCartAlert(response.message || 'Update success!', 'success')
    } 
    catch (error) 
    {
        console.error('Lỗi khi cập nhật số lượng:', error)
        displayCartAlert(error.message || 'Có lỗi xảy ra khi cập nhật số lượng. Đang tải lại giỏ hàng...', 'error')
        await displayCartItems()
    }
}

function updateCartTotalLocally() {
    let currentTotal = 0;
    document.querySelectorAll('.cart-item').forEach(itemDiv => {
        const itemTotalElement = itemDiv.querySelector('.cart-item-total');
        if (itemTotalElement) {
            const totalText = itemTotalElement.textContent.replace('VNĐ', '').trim()
            const itemTotal = parseFloat(totalText.replace(/\./g, '').replace(/,/g, '.'))
            if (!isNaN(itemTotal)) {
                currentTotal += itemTotal
            }
        }
    });
    cartTotalPriceElement.textContent = currentTotal.toLocaleString('vi-VN') + ' VNĐ'
}

/* HÀM GẮN CÁC SỰ KIỆN CHO GIỎ HÀNG */
function attachCartEventListeners()
{
    document.querySelectorAll('.decrease-quantity').forEach(button => {
        button.onclick = async (event) => {
            const cartItemId = event.currentTarget.dataset.cartItemId
            // console.log("Cart Item ID:", cartItemId)
            const input = document.querySelector(`.item-quantity-input[data-cart-item-id="${cartItemId}"]`);
            let currentQuantity = parseInt(input.value);
            if (currentQuantity > 1)
            {
                const newQuantity = currentQuantity - 1;
                input.value = newQuantity;
                await updateCartItemQuantityUiAndBackend(cartItemId, newQuantity, input);
            }
            else
            {
                if (confirm('Quantity will be 0. Do you want to delete'))
                {
                    await removeCartItem(cartItemId);
                }
            }
        }
    })

    document.querySelectorAll('.increase-quantity').forEach(button => {
        button.onclick = async (event) => {
            const cartItemId = event.currentTarget.dataset.cartItemId;

            const input = document.querySelector(`.item-quantity-input[data-cart-item-id="${cartItemId}"]`);
            let currentQuantity = parseInt(input.value);
            const newQuantity = currentQuantity + 1;
            input.value = newQuantity;
            await updateCartItemQuantityUiAndBackend(cartItemId, newQuantity, input);
        }
    })

    document.querySelectorAll('.item-quantity-input').forEach(input => {
        input.onchange = async (event) => {
            const cartItemId = event.currentTarget.dataset.cartItemId;
            let newQuantity = parseInt(event.target.value);
            if (isNaN(newQuantity) || newQuantity <= 0)
            {
                displayCartAlert('Quantity is not ... ');
                await displayCartItems();
                return;
            }
            await updateCartItemQuantityUiAndBackend(cartItemId, newQuantity, input);
        }
    })

    document.querySelectorAll('.remove-item-btn').forEach(button => {
        button.onclick = async (event) => {
            const cartItemId = event.currentTarget.dataset.cartItemId;
            if (confirm('Bạn có chắc chắn muốn xóa sản phẩm này khỏi giỏ hàng không?')) {
                await removeCartItem(cartItemId);
            }
        }
    })

    if (checkoutButton)
    {
        checkoutButton.onclick = () => {
            displayCartAlert('function pay will be improve later', 'success');
            TODO
        }
    }
}
/*  HÀM GỬI YÊU CẦU CẬP NHẬT SỐ LƯỢNG ĐẾN BACKEND */


async function removeCartItem(cartItemId)
{
    try 
    {
        const response = await apiRequest(`${API}/api/CartItem/${cartItemId}`, 'DELETE')
        displayCartAlert(response.message || 'Product have been delete from cart!', 'success')
        await displayCartItems()
    }
    catch (error)
    {
        console.error('error when delete product!', error)
        displayCartAlert(error.message || 'Có lỗi xảy ra khi xóa sản phẩm.', 'error')
    }
}

document.addEventListener('DOMContentLoaded', displayCartItems)