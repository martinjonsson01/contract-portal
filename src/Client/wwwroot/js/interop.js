function closeModal(id) {
    const modalElement = document.querySelector(id);
    const modal = bootstrap.Modal.getOrCreateInstance(modalElement);
    modal.hide();
}

function showModal(selector) {
    const modalElement = document.querySelector(selector);
    const modal = bootstrap.Modal.getOrCreateInstance(modalElement);
    modal.show();
}

function registerModalCloseCallback(selector, dotNetHelper) {
    const modalElement = document.querySelector(selector);
    modalElement.addEventListener('hidden.bs.modal', function (event) {
        dotNetHelper.invokeMethodAsync("OnModalClose");
    });
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
