const usuario = JSON.parse(localStorage.getItem("habitusUsuario") || "null");

if (!usuario) {
    window.location.href = "index.html";
    throw new Error("Usuário não autenticado.");
}

const userName = document.getElementById("userName");
const logoutButton = document.getElementById("logoutButton");
const refreshButton = document.getElementById("refreshButton");
const habitForm = document.getElementById("habitForm");
const habitFormTitle = document.getElementById("habitFormTitle");
const saveHabitButton = document.getElementById("saveHabitButton");
const habitId = document.getElementById("habitId");
const habitNome = document.getElementById("habitNome");
const habitDescricao = document.getElementById("habitDescricao");
const habitData = document.getElementById("habitData");
const habitMessage = document.getElementById("habitMessage");
const habitsList = document.getElementById("habitsList");
const cancelEditButton = document.getElementById("cancelEditButton");
const totalHabitos = document.getElementById("totalHabitos");
const habitosConcluidos = document.getElementById("habitosConcluidos");
const taxaConclusao = document.getElementById("taxaConclusao");
const metricProgressBar = document.getElementById("metricProgressBar");

let habitosDoUsuario = [];

userName.textContent = usuario?.nome || "usuário";
habitData.value = formatDateForApi();

logoutButton.addEventListener("click", () => {
    localStorage.removeItem("habitusUsuario");
    window.location.href = "index.html";
});

refreshButton.addEventListener("click", async () => {
    await carregarDashboard();
});

cancelEditButton.addEventListener("click", () => {
    resetHabitForm();
});

habitForm.addEventListener("submit", async (event) => {
    event.preventDefault();

    const payload = {
        nome: habitNome.value.trim(),
        descricao: habitDescricao.value.trim(),
        data: habitData.value
    };

    if (!payload.nome || !payload.descricao || !payload.data) {
        setMessage(habitMessage, "Informe nome, descrição e data.", "error");
        return;
    }

    try {
        if (habitId.value) {
            await HabitusApi.atualizarHabito(habitId.value, payload);
            setMessage(habitMessage, "Hábito atualizado com sucesso.", "success");
        } else {
            await HabitusApi.criarHabito({
                ...payload,
                usuarioId: usuario.id
            });
            setMessage(habitMessage, "Hábito criado com sucesso.", "success");
        }

        resetHabitForm(false);
        await carregarDashboard();
    } catch (error) {
        setMessage(habitMessage, error.message, "error");
    }
});

async function carregarDashboard() {
    await Promise.all([
        carregarHabitos(),
        carregarMetricasDoDia()
    ]);
}

async function carregarHabitos() {
    habitsList.innerHTML = '<div class="empty-state">Carregando hábitos...</div>';

    try {
        const habitos = await HabitusApi.listarHabitos();
        habitosDoUsuario = habitos
            .filter((habito) => habito.usuarioId === usuario.id)
            .sort((a, b) => a.data.localeCompare(b.data) || a.id - b.id);

        renderHabitos();
    } catch (error) {
        habitsList.innerHTML = `<div class="empty-state">${error.message}</div>`;
    }
}

async function carregarMetricasDoDia() {
    const hoje = formatDateForApi();

    try {
        const metricas = await HabitusApi.metricasDoDia(hoje);
        const progresso = metricas.taxaConclusao ?? 0;

        totalHabitos.textContent = metricas.totalHabitos ?? 0;
        habitosConcluidos.textContent = metricas.habitosConcluidos ?? 0;
        taxaConclusao.textContent = `${progresso}%`;
        atualizarBarraProgresso(progresso);
    } catch {
        totalHabitos.textContent = "-";
        habitosConcluidos.textContent = "-";
        taxaConclusao.textContent = "-";
        atualizarBarraProgresso(0);
    }
}

function renderHabitos() {
    if (habitosDoUsuario.length === 0) {
        habitsList.innerHTML = '<div class="empty-state">Nenhum hábito cadastrado para este usuário.</div>';
        return;
    }

    habitsList.innerHTML = "";

    habitosDoUsuario.forEach((habito) => {
        const card = document.createElement("article");
        card.className = `habit-card${habito.concluido ? " done" : ""}`;
        const status = habito.concluido ? "Concluído" : "Pendente";
        const statusClass = habito.concluido ? "completed" : "pending";

        card.innerHTML = `
            <div class="habit-title">
                <div>
                    <span class="habit-label">Hábito</span>
                    <h3>${escapeHtml(habito.nome)}</h3>
                </div>
                <span class="status-pill ${statusClass}">
                    <span class="status-dot" aria-hidden="true"></span>
                    ${status}
                </span>
            </div>
            <p class="habit-description">${escapeHtml(habito.descricao)}</p>
            <div class="habit-meta">
                <span>Data</span>
                <strong>${escapeHtml(formatDateForDisplay(habito.data))}</strong>
            </div>
            <div class="habit-actions">
                <button type="button" class="toggle-button" data-action="toggle">${habito.concluido ? "Desmarcar" : "Concluir"}</button>
                <button type="button" class="ghost-button" data-action="edit">Editar</button>
                <button type="button" class="danger-button" data-action="delete">Excluir</button>
            </div>
        `;

        card.querySelector('[data-action="toggle"]').addEventListener("click", () => alternarConclusao(habito.id));
        card.querySelector('[data-action="edit"]').addEventListener("click", () => preencherEdicao(habito));
        card.querySelector('[data-action="delete"]').addEventListener("click", () => excluirHabito(habito.id));

        habitsList.appendChild(card);
    });
}

async function alternarConclusao(id) {
    try {
        await HabitusApi.alternarConclusao(id);
        await carregarDashboard();
    } catch (error) {
        setMessage(habitMessage, error.message, "error");
    }
}

function preencherEdicao(habito) {
    habitId.value = habito.id;
    habitNome.value = habito.nome;
    habitDescricao.value = habito.descricao;
    habitData.value = formatDateForInput(habito.data);
    habitFormTitle.textContent = "Editar hábito";
    saveHabitButton.textContent = "Salvar edição";
    cancelEditButton.classList.remove("hidden");
    setMessage(habitMessage, "Editando hábito selecionado.");
    habitNome.focus();
}

async function excluirHabito(id) {
    const deveExcluir = window.confirm("Deseja excluir este hábito?");

    if (!deveExcluir) {
        return;
    }

    try {
        await HabitusApi.excluirHabito(id);
        setMessage(habitMessage, "Hábito excluído com sucesso.", "success");
        await carregarDashboard();
    } catch (error) {
        setMessage(habitMessage, error.message, "error");
    }
}

function resetHabitForm(clearMessage = true) {
    habitForm.reset();
    habitId.value = "";
    habitData.value = formatDateForApi();
    habitFormTitle.textContent = "Cadastrar hábito";
    saveHabitButton.textContent = "Salvar hábito";
    cancelEditButton.classList.add("hidden");

    if (clearMessage) {
        setMessage(habitMessage, "");
    }
}

function atualizarBarraProgresso(valor) {
    const progresso = Number(valor);
    const progressoSeguro = Number.isFinite(progresso)
        ? Math.min(Math.max(progresso, 0), 100)
        : 0;

    metricProgressBar.style.width = `${progressoSeguro}%`;
}

function formatDateForDisplay(value) {
    const [year, month, day] = formatDateForInput(value).split("-");

    if (!year || !month || !day) {
        return value || "";
    }

    return `${day.padStart(2, "0")}/${month.padStart(2, "0")}/${year}`;
}

function formatDateForInput(value) {
    return String(value || "").split("T")[0];
}

function escapeHtml(value) {
    return String(value)
        .replaceAll("&", "&amp;")
        .replaceAll("<", "&lt;")
        .replaceAll(">", "&gt;")
        .replaceAll('"', "&quot;")
        .replaceAll("'", "&#039;");
}

carregarDashboard();
