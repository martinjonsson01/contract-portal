function closeModal(id) {
    const modalElement = document.querySelector(id);
    if (!modalElement) return;
    const modal = bootstrap.Modal.getOrCreateInstance(modalElement);
    modal.hide();
}

function showModal(selector) {
    const modalElement = document.querySelector(selector);
    if (!modalElement) return;
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

function initializeSJWidget() {
    const SJ = window.SJ;
    if (SJ == null) {
        console.log("window: ", window);
        console.log("this: ", this);

        console.error("Could not initialize SJ widget: can't find `window.SJ`");
        return;
    }

    const configuration = {
        micrositeId: "0f46a766-8653-40ef-9e24-1759650a6b14",
        language: "sv"
    };
    SJ.widget.init(configuration);
}
