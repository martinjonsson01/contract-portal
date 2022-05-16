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

function scrollToElement(selector) {
    document.querySelector(selector).scrollIntoView({behavior: 'smooth', block: 'start'});
}

function showModal(selector) {
    const modal = new bootstrap.Modal(document.querySelector(selector), {});
    modal.show();
}
