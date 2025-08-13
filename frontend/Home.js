document.addEventListener('DOMContentLoaded', function() {
    const authLinksContainer = document.getElementById('auth-links')
    const userToken = localStorage.getItem('userToken')
    let currentUser = null

    try {
        const currentUserString = localStorage.getItem('currentUser')
        if (currentUserString)
        {
            currentUser = JSON.parse(currentUserString)
        }
    } catch(e) {
        console.error('Cannot parse user data from localStorage:', e)
    }

    if (userToken && currentUser){
        
        const displayName = currentUser.fullName || currentUser.email || 'Người dùng'
        
        if (authLinksContainer){
            authLinksContainer.innerHTML = `
                <p>
                    <a href="profile.html" class="nav-link">${displayName}</a> /
                    <a href="#" class="nav-link" id="logout-link">Đăng Xuất</a>
                </p>
            `

            const logoutLink = document.getElementById('logout-Link')
            if (logoutLink) {
                logoutLink.addEventListener('click', function (event) {
                    event.preventDefault()
                    localStorage.removeItem('userToken')
                    localStorage.removeItem('currentUser')
                    window.location.reload()
                })
            }
        }
    } else {
        if (authLinksContainer) {
            authLinksContainer.innerHTML = `
                <p>
                    <a href="login.html" class="nav-link">LOGIN</a> /
                    <a href="SignUp.html.html" class="nav-link">SIGNUP</a>
                </p>
            `
        }
    }
})