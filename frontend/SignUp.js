document.addEventListener('DOMContentLoaded', () => {
    const signupForm = document.getElementById('signupForm');
    const messageDiv = document.getElementById('message');

    signupForm.addEventListener('submit', async (event) => {
        event.preventDefault();

        const fullName = document.getElementById('fullName').value;
        const email = document.getElementById('email').value;
        const password = document.getElementById('password').value;

        const role = "customer";

        const userData = {
            fullName: fullName,
            email: email,
            passwordHash: password,
            role: role 
        };

        try {
            const response = await fetch('https://localhost:7090/api/Users', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(userData)
            });

            const data = await response.json();

            if (response.ok) {
                messageDiv.textContent = `Đăng ký thành công!`;
                messageDiv.className = 'message success';
                signupForm.reset();
            } else {
                messageDiv.textContent = data.title || data.errors ?
                    JSON.stringify(data.errors || data.title) :
                    'Đăng ký thất bại. Vui lòng thử lại.';
                messageDiv.className = 'message error';
            }
        } catch (error) {
            console.error('Lỗi khi gửi yêu cầu đăng ký:', error);
            messageDiv.textContent = 'Không thể kết nối đến máy chủ. Vui lòng thử lại sau.';
            messageDiv.className = 'message error';
        }
    });
});