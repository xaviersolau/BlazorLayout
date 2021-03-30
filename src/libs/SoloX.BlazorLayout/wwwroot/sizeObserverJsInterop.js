
class ResizeManager {

    constructor() {
        window.addEventListener("resize", this.callbackResize);

        this.callbackReferences = [];
        this.observerReferences = [];
    }

    registerResizeCallBack(callbackObjetReference, elementReferenceId, element) {
        console.log("registerResizeCallBack");
        console.log(elementReferenceId);

        let size = { width: element.clientWidth, height: element.clientHeight };

        callbackObjetReference.invokeMethodAsync('ResizeAsync', size.width, size.height)

        this.callbackReferences.push({
            elementReferenceId: elementReferenceId,
            element: element,
            callbackObject: callbackObjetReference,
            size: size
        })

    }

    unregisterResizeCallBack(elementReferenceId) {
        console.log("unregisterResizeCallBack");
        console.log(elementReferenceId);

        for (var i = 0; i < this.callbackReferences.length; i++) {

            if (this.callbackReferences[i].elementReferenceId === elementReferenceId) {
                this.callbackReferences.splice(i, 1);

                break;
            }
        }

    }

    registerMutationObserver(elementReferenceId, element) {
        console.log("registerMutationObserver");
        console.log(elementReferenceId);

        let observer = new MutationObserver(this.callbackObserver);
        observer.observe(element, { attributes: true });

        this.observerReferences.push({
            element: element,
            elementReferenceId: elementReferenceId,
            observer: observer
        });

    }

    unregisterMutationObserver(elementReferenceId) {
        console.log("unregisterMutationObserver");
        console.log(elementReferenceId);

        for (var i = 0; i < this.observerReferences.length; i++) {
            let item = this.observerReferences[i];
            if (item.elementReferenceId === elementReferenceId) {

                let element = item.element;
                console.log(element);

                item.observer.disconnect(element);

                this.observerReferences.splice(i, 1);

                return;
            }
        }

    }

    ping() {
        console.log("ping");
    }

    processCallbackReferences() {

        console.log("processCallbackReferences");

        if (this.callbackReferences.length > 0) {
            clearTimeout(this.callbackTimeoutId);

            this.callbackTimeoutId = setTimeout(
                function () {
                    console.log("processing all callback references...");
                    console.log(resizeManager.callbackReferences);
                    resizeManager.callbackReferences
                        .forEach(ref => {
                            let width = ref.element.clientWidth;
                            let height = ref.element.clientHeight;
                            if (ref.size.width != width || ref.size.height != height) {

                                ref.size.width = width;
                                ref.size.height = height;

                                ref.callbackObject.invokeMethodAsync('ResizeAsync', width, height)
                            }
                        });
                },
                250
            );
        }
    }

    callbackObserver(mutationsList) {
        resizeManager.processCallbackReferences();
    }

    callbackResize(event) {
        resizeManager.processCallbackReferences();
    }

}

export var resizeManager = new ResizeManager();

