const loginForm = document.getElementById("loginForm");
const registerForm = document.getElementById("registerForm");
const loginMessage = document.getElementById("loginMessage");
const registerMessage = document.getElementById("registerMessage");

loginForm.addEventListener("submit", async (event) => {
    event.preventDefault();
    setMessage(loginMessage, "Entrando...");

    const payload = {
        email: document.getElementById("loginEmail").value.trim(),
        senha: document.getElementById("loginSenha").value
    };

    try {
        const usuario = await HabitusApi.login(payload);
        localStorage.setItem("habitusUsuario", JSON.stringify(usuario));
        setMessage(loginMessage, usuario.mensagem || "Login realizado com sucesso.", "success");
        window.location.href = "dashboard.html";
    } catch (error) {
        setMessage(loginMessage, error.message, "error");
    }
});

registerForm.addEventListener("submit", async (event) => {
    event.preventDefault();
    setMessage(registerMessage, "Cadastrando...");

    const payload = {
        nome: document.getElementById("registerNome").value.trim(),
        email: document.getElementById("registerEmail").value.trim(),
        senha: document.getElementById("registerSenha").value
    };

    try {
        const usuario = await HabitusApi.register(payload);
        setMessage(registerMessage, usuario.mensagem || "Cadastro realizado com sucesso. Agora faça login.", "success");
        registerForm.reset();
        document.getElementById("loginEmail").value = payload.email;
    } catch (error) {
        setMessage(registerMessage, error.message, "error");
    }
});
