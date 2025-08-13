function togglePasswordVisibility()
{
    const passwordField = document.getElementById('password')
    const passwordToggleIcon = document.getElementById('passwordToggleIcon')
    
    if (passwordField.type === 'password')
    {
        passwordField.type = 'text'
        passwordToggleIcon.classList.remove('fa-eye-slash')
        passwordToggleIcon.classList.add('fa-eye')
    }
    else 
    {
        passwordField.type = 'password'
        passwordToggleIcon.classList.remove('fa-eye')
        passwordToggleIcon.classList.add('fa-eye-slash')
    }
}

document.getElementById('loginForm').addEventListener('submit', async function(event) {

    event.preventDefault()

    const email = document.getElementById('email').value
    const password = document.getElementById('password').value
    const emailError = document.getElementById('emailError')
    const passwordError = document.getElementById('passwordError')
    const loginAlert = document.getElementById('loginAlert')
    const loginButton = document.querySelector('.login-button')

    emailError.textContent = ''
    passwordError.textContent = ''
    loginAlert.style.display = 'none'
    loginAlert.className = 'alert-message'

    let isValid = true

    if (email === '')
    {
        emailError.textContent = 'Please enter email'
        isValid = false
    }
    // else if (!validateEmail(email))
    // {
    //     emailError.textContent = 'Email is incorrect'
    // }

    if (password === '')
    {
        passwordError.textContent = 'Please enter your password'
        isValid = false
    }

    if (isValid)
    {
        loginButton.disabled = true
        loginButton.textContent = 'Logging....'

        try {
            const response = await fetch('https://localhost:7090/api/Users/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({email, password})
            })
            const data = await response.json()

            if (response.ok)
            {
                loginAlert.textContent = data.message || 'Login success!'
                loginAlert.classList.add('sucess')
                loginAlert.style.display = 'block'

                if (data.token)
                {
                    localStorage.setItem('userToken', data.token)
                }
                const userObject = {
                    userId: data.userId,
                    fullName: data.fullName,
                    email: data.email,
                    role: data.role
                }
                localStorage.setItem('currentUser', JSON.stringify(userObject))
                console.log('User information have been saved:', userObject)

                setTimeout(() => {
                    window.location.href = 'Home.html'
                }, 2000);
            } else {
                loginAlert.textContent = data.message || 'Đăng nhập thất bại. Vui lòng kiểm tra lại Email và Mật khẩu.';
                loginAlert.classList.add('error');
                loginAlert.style.display = 'block';
            }
        }
        catch (error){
            console.error('Lỗi khi gửi yêu cầu đăng nhập:', error);
            loginAlert.textContent = 'Đã xảy ra lỗi mạng. Vui lòng thử lại sau.';
            loginAlert.classList.add('error');
            loginAlert.style.display = 'block';
        } finally {
            loginButton.disabled = false;
            loginButton.textContent = 'Đăng Nhập';
        }
    }
})

function validateEmail(email)
{
    const re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(email).toLowerCase());
}
