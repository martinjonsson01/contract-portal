function closeModal(id) {
    var modalElement = document.querySelector(id)
    var modal = bootstrap.Modal.getOrCreateInstance(modalElement)
    modal.hide();
}
