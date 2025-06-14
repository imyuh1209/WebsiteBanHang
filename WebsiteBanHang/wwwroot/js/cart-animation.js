/**
 * Cart Animation - Hiệu ứng sản phẩm bay vào giỏ hàng
 */

document.addEventListener('DOMContentLoaded', function() {
    console.log('DOM đã sẵn sàng'); // Debug
    
    // Lấy tham chiếu đến nút giỏ hàng trong header
    const cartButton = document.querySelector('.btn-cart');
    console.log('Nút giỏ hàng:', cartButton); // Debug
    
    // Phương pháp 1: Bắt sự kiện submit của form
    const addToCartForms = document.querySelectorAll('form[action*="/Cart/AddToCart"]');
    console.log('Số lượng form tìm thấy:', addToCartForms.length); // Debug
    
    addToCartForms.forEach((form, index) => {
        console.log('Đăng ký sự kiện cho form #', index, form); // Debug
        
        form.addEventListener('submit', function(e) {
            handleAddToCart(e, this, cartButton);
        });
    });
    
    // Phương pháp 2: Bắt sự kiện click của nút thêm vào giỏ hàng
    const addToCartButtons = document.querySelectorAll('.add-to-cart-btn');
    console.log('Số lượng nút thêm vào giỏ hàng:', addToCartButtons.length); // Debug
    
    addToCartButtons.forEach((button, index) => {
        console.log('Đăng ký sự kiện click cho nút #', index, button); // Debug
        
        button.addEventListener('click', function(e) {
            const form = this.closest('form');
            if (form) {
                handleAddToCart(e, form, cartButton);
            }
        });
    });
    
    // Hàm xử lý thêm vào giỏ hàng
    function handleAddToCart(e, form, cartButton) {
        console.log('Xử lý thêm vào giỏ hàng!'); // Debug
        e.preventDefault(); // Ngăn chặn form submit mặc định
        
        // Lấy vị trí của nút thêm vào giỏ hàng
        const submitButton = form.querySelector('button[type="submit"]');
        console.log('Submit button:', submitButton); // Debug
        
        const buttonRect = submitButton.getBoundingClientRect();
        const buttonX = buttonRect.left + buttonRect.width / 2;
        const buttonY = buttonRect.top + buttonRect.height / 2;
        
        console.log('Vị trí nút thêm vào giỏ hàng:', buttonX, buttonY); // Debug
        
        // Lấy vị trí của nút giỏ hàng trong header
        const cartRect = cartButton.getBoundingClientRect();
        const cartX = cartRect.left + cartRect.width / 2;
        const cartY = cartRect.top + cartRect.height / 2;
        
        console.log('Vị trí nút giỏ hàng:', cartX, cartY); // Debug
        
        // Tạo phần tử hiệu ứng
        createFlyingElement(buttonX, buttonY, cartX, cartY);
        
        // Submit form sau khi animation hoàn thành
        setTimeout(() => {
            form.submit();
        }, 1000); // Thời gian phải lớn hơn thời gian animation
    }
    
    // Hàm tạo phần tử bay vào giỏ hàng
    function createFlyingElement(startX, startY, endX, endY) {
        console.log('Tạo phần tử bay với vị trí:', startX, startY, endX, endY); // Debug
        
        // Tạo phần tử
        const flyingElement = document.createElement('div');
        flyingElement.className = 'flying-item';
        flyingElement.innerHTML = '<i class="bi bi-box"></i>';
        
        // Thiết lập style trực tiếp
        flyingElement.style.position = 'fixed';
        flyingElement.style.zIndex = '9999';
        flyingElement.style.width = '50px';
        flyingElement.style.height = '50px';
        flyingElement.style.backgroundColor = '#fff';
        flyingElement.style.borderRadius = '50%';
        flyingElement.style.display = 'flex';
        flyingElement.style.alignItems = 'center';
        flyingElement.style.justifyContent = 'center';
        flyingElement.style.boxShadow = '0 2px 10px rgba(0, 0, 0, 0.2)';
        flyingElement.style.pointerEvents = 'none';
        flyingElement.style.transition = 'all 1s cubic-bezier(0.18, 0.89, 0.32, 1.28)';
        
        // Thêm vào body
        document.body.appendChild(flyingElement);
        console.log('Đã thêm phần tử bay vào body'); // Debug
        
        // Thiết lập vị trí ban đầu
        flyingElement.style.left = `${startX}px`;
        flyingElement.style.top = `${startY}px`;
        
        // Kích hoạt animation sau một khoảng thời gian ngắn
        setTimeout(() => {
            console.log('Bắt đầu animation'); // Debug
            flyingElement.style.transform = 'scale(0.5)';
            flyingElement.style.left = `${endX}px`;
            flyingElement.style.top = `${endY}px`;
            flyingElement.style.opacity = '0';
        }, 50); // Tăng thời gian delay để đảm bảo phần tử đã được render
        
        // Xóa phần tử sau khi animation hoàn thành
        setTimeout(() => {
            console.log('Kết thúc animation, xóa phần tử'); // Debug
            document.body.removeChild(flyingElement);
            
            // Hiệu ứng nhấp nháy giỏ hàng
            cartButton.classList.add('cart-highlight');
            setTimeout(() => {
                cartButton.classList.remove('cart-highlight');
            }, 700);
        }, 1000);
    }
});