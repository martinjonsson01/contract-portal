function closeModal(id) {
    const modalElement = document.querySelector(id);
    const modal = bootstrap.Modal.getOrCreateInstance(modalElement);
    modal.hide();
}

function focusElement(selector) {
    const element = document.querySelector(selector);
    element.focus();
}

function registerActivityCallback(dotNetHelper) {
    document.onmousemove = resetTimeDelay;
    document.onkeydown = resetTimeDelay;

    function resetTimeDelay() {
        dotNetHelper.invokeMethodAsync("ResetTimer");
    }
}
