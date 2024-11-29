document.addEventListener("DOMContentLoaded", function () {
    const registerForm = document.getElementById("registerForm");
    if (registerForm) {
        registerForm.addEventListener("submit", function (event) {
            const password = document.getElementById("password").value;
            const confirmPassword = document.getElementById("confirmPassword").value;

            // Patrón actualizado
            const passwordPattern = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{7,}$/;

            if (!passwordPattern.test(password)) {
                alert("La contraseña debe tener al menos 7 caracteres, incluir una letra mayúscula, una letra minúscula, un número y un carácter especial.");
                event.preventDefault();
            } else if (password !== confirmPassword) {
                alert("Las contraseñas no coinciden.");
                event.preventDefault();
            }
        });
    }
});
