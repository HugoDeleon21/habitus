const API_BASE_URL = "http://localhost:5112";

async function apiRequest(path, options = {}) {
    const response = await fetch(`${API_BASE_URL}${path}`, {
        headers: {
            "Content-Type": "application/json",
            ...(options.headers || {})
        },
        ...options
    });

    const contentType = response.headers.get("content-type") || "";
    const hasJson = contentType.includes("application/json");
    const data = hasJson ? await response.json() : null;

    if (!response.ok) {
        const message = data?.mensagem || data?.mensagemErro || data?.message || "Erro ao comunicar com a API.";
        throw new Error(message);
    }

    return data;
}

const HabitusApi = {
    register(payload) {
        return apiRequest("/api/Auth/register", {
            method: "POST",
            body: JSON.stringify(payload)
        });
    },

    login(payload) {
        return apiRequest("/api/Auth/login", {
            method: "POST",
            body: JSON.stringify(payload)
        });
    },

    listarHabitos() {
        return apiRequest("/api/habitos");
    },

    criarHabito(payload) {
        return apiRequest("/api/habitos", {
            method: "POST",
            body: JSON.stringify(payload)
        });
    },

    atualizarHabito(id, payload) {
        return apiRequest(`/api/habitos/${id}`, {
            method: "PUT",
            body: JSON.stringify(payload)
        });
    },

    excluirHabito(id) {
        return apiRequest(`/api/habitos/${id}`, {
            method: "DELETE"
        });
    },

    alternarConclusao(id) {
        return apiRequest(`/api/habitos/${id}/concluir`, {
            method: "PATCH"
        });
    },

    metricasDoDia(data) {
        return apiRequest(`/api/habitos/metricas/dia/${data}`);
    }
};

function setMessage(element, message, type = "") {
    element.textContent = message;
    element.classList.remove("success", "error");

    if (type) {
        element.classList.add(type);
    }
}

function formatDateForApi(date = new Date()) {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, "0");
    const day = String(date.getDate()).padStart(2, "0");

    return `${year}-${month}-${day}`;
}
