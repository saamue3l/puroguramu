document.addEventListener('DOMContentLoaded', () => {
    initializeErrorMessages();
    initializeSortOrder();
    initializeModal();
});

function initializeErrorMessages() {
    const errorMessage = document.body.getAttribute('data-error-message');
    if (errorMessage) {
        toastr.error(errorMessage);
    }
}

function initializeSortOrder() {
    const sortOrderSelect = document.getElementById('sortOrder');
    if (!sortOrderSelect) return;

    const savedSortOrder = localStorage.getItem('sortOrder');
    if (savedSortOrder) {
        sortOrderSelect.value = savedSortOrder;
    }

    sortOrderSelect.addEventListener('change', function() {
        const sortOrder = this.value;
        localStorage.setItem('sortOrder', sortOrder);
        window.location.href = `?sortOrder=${sortOrder}`;
    });
}

function initializeModal() {
    const modal = document.getElementById('confirmModal');
    if (!modal) return;

    modal.addEventListener('click', (event) => {
        if (event.target.id === 'confirmModal') {
            closeModal();
        }
    });
}

function openModal() {
    const modal = document.getElementById('confirmModal');
    if (modal) {
        modal.style.display = 'flex';
    }
}

function closeModal() {
    const modal = document.getElementById('confirmModal');
    if (modal) {
        modal.style.display = 'none';
    }
}
