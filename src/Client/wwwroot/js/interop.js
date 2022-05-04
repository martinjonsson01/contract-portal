function closeModal(id) {
    const modalElement = document.querySelector(id);
    const modal = bootstrap.Modal.getOrCreateInstance(modalElement);
    modal.hide();
}

function focusElement(selector) {
    const element = document.querySelector(selector);
    element.focus();
}
