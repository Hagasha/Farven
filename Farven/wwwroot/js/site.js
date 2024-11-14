// Função para atualizar o ícone do tema
function updateThemeIcon(isDark) {
    const icon = document.getElementById('theme-icon');
    if (icon) {
        icon.className = isDark ? 'fas fa-sun' : 'fas fa-moon';
    }
}

// Função para adicionar o tema escuro aos elementos
function applyDarkTheme() {
    document.body.classList.add('dark-theme');
    document.querySelector('.navbar').classList.add('dark-theme');
    document.querySelector('.footer').classList.add('dark-theme');
    updateThemeIcon(true);
}

// Função para remover o tema escuro dos elementos
function removeDarkTheme() {
    document.body.classList.remove('dark-theme');
    document.querySelector('.navbar').classList.remove('dark-theme');
    document.querySelector('.footer').classList.remove('dark-theme');
    updateThemeIcon(false);
}

// Função para alternar entre tema claro e escuro
function toggleTheme() {
    const isDarkTheme = document.body.classList.contains('dark-theme');

    if (isDarkTheme) {
        removeDarkTheme();
        localStorage.setItem('theme', 'light');
    } else {
        applyDarkTheme();
        localStorage.setItem('theme', 'dark');
    }
}

// Carrega o tema salvo ao abrir a página
document.addEventListener('DOMContentLoaded', function () {
    const savedTheme = localStorage.getItem('theme');
    if (savedTheme === 'dark') {
        applyDarkTheme();
    } else {
        updateThemeIcon(false);
    }
});