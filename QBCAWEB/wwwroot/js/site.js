// QBCAWEB JavaScript functions

// API Base URL
const API_BASE_URL = '/api';

// Load products from API
async function loadProducts() {
    try {
        const response = await fetch(`${API_BASE_URL}/product`);
        const result = await response.json();

        if (result.success) {
            displayProducts(result.data);
        } else {
            console.error('Error loading products:', result.message);
            showError('Không thể tải danh sách sản phẩm');
        }
    } catch (error) {
        console.error('Network error:', error);
        showError('Lỗi kết nối mạng');
    }
}

// Display products in the grid
function displayProducts(products) {
    const productList = document.getElementById('product-list');

    if (!products || products.length === 0) {
        productList.innerHTML = '<p>Không có sản phẩm nào.</p>';
        return;
    }

    productList.innerHTML = products.map(product => `
        <div class="product-card">
            <h3>${product.name}</h3>
            <p>${product.description}</p>
            <div class="price">${formatPrice(product.price)}</div>
            <span class="category">${product.category}</span>
            <div class="product-actions">
                <button onclick="viewProduct(${product.id})">Xem chi tiết</button>
            </div>
        </div>
    `).join('');
}

// Format price to Vietnamese currency
function formatPrice(price) {
    return new Intl.NumberFormat('vi-VN', {
        style: 'currency',
        currency: 'VND'
    }).format(price);
}

// View product details
async function viewProduct(productId) {
    try {
        const response = await fetch(`${API_BASE_URL}/product/${productId}`);
        const result = await response.json();

        if (result.success) {
            showProductModal(result.data);
        } else {
            showError('Không thể tải thông tin sản phẩm');
        }
    } catch (error) {
        console.error('Error loading product details:', error);
        showError('Lỗi kết nối mạng');
    }
}

// Show product in modal (simple alert for demo)
function showProductModal(product) {
    alert(`
Tên: ${product.name}
Mô tả: ${product.description}
Giá: ${formatPrice(product.price)}
Danh mục: ${product.category}
Ngày tạo: ${new Date(product.createdDate).toLocaleDateString('vi-VN')}
    `);
}

// Show error message
function showError(message) {
    alert(`Lỗi: ${message}`);
}

// Load users (for admin)
async function loadUsers() {
    try {
        const response = await fetch(`${API_BASE_URL}/user`);
        const result = await response.json();

        if (result.success) {
            console.log('Users loaded:', result.data);
            return result.data;
        } else {
            console.error('Error loading users:', result.message);
        }
    } catch (error) {
        console.error('Network error:', error);
    }
}

// Initialize page
document.addEventListener('DOMContentLoaded', function () {
    console.log('QBCAWEB initialized');

    // Load products on page load
    loadProducts();

    // Smooth scrolling for navigation
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();

            const target = document.querySelector(this.getAttribute('href'));
            if (target) {
                target.scrollIntoView({
                    behavior: 'smooth'
                });
            }
        });
    });
});