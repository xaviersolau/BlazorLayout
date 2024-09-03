
class ResizeManager {

  #callbackReferences = [];
  #observerReferences = [];

  #enableLogsOption = false;
  #callbackDelay = 250;
  #eventBurstBoxingCallback = false;
  #callbackTimeoutId = null;

  constructor() {
    window.addEventListener("resize", this.callbackResize);
  }

  setupModule(enableLogs, callbackDelay, eventBurstBoxingCallback) {
    this.#enableLogsOption = enableLogs;
    this.#callbackDelay = callbackDelay;
    this.#eventBurstBoxingCallback = eventBurstBoxingCallback;

    this.#consoleLog("Set up Resize module",
    {
      enableLogs: enableLogs,
      callbackDelay: callbackDelay,
      eventBurstBoxingCallback: eventBurstBoxingCallback
    });
  }

  registerResizeCallback(callbackObjetReference, elementReferenceId, element) {
    this.#consoleLog("registerResizeCallback",
    {
      elementReferenceId: elementReferenceId
    });

    const size = { width: element.clientWidth, height: element.clientHeight };

    callbackObjetReference.invokeMethodAsync('ResizeAsync', size.width, size.height)

    this.#callbackReferences.push({
      elementReferenceId: elementReferenceId,
      element: element,
      callbackObject: callbackObjetReference,
      size: size
    })

  }

  unregisterResizeCallback(elementReferenceId) {
    this.#consoleLog("unregisterResizeCallback",
    {
      elementReferenceId: elementReferenceId
    });

    for (var i = 0; i < this.#callbackReferences.length; i++) {

      if (this.#callbackReferences[i].elementReferenceId === elementReferenceId) {
        this.#callbackReferences.splice(i, 1);

        break;
      }
    }
  }

  registerMutationObserver(elementReferenceId, element) {
    this.#consoleLog("registerMutationObserver",
    {
      elementReferenceId: elementReferenceId
    });

    const observer = new MutationObserver(this.callbackObserver);
    observer.observe(element, { attributes: true });

    this.#observerReferences.push({
      element: element,
      elementReferenceId: elementReferenceId,
      observer: observer
    });

  }

  unregisterMutationObserver(elementReferenceId) {
    this.#consoleLog("unregisterMutationObserver",
    {
      elementReferenceId: elementReferenceId
    });

    for (var i = 0; i < this.#observerReferences.length; i++) {
      const item = this.#observerReferences[i];
      if (item.elementReferenceId === elementReferenceId) {

        const element = item.element;
        this.#consoleLog(element);

        item.observer.disconnect(element);

        this.#observerReferences.splice(i, 1);

        return;
      }
    }

  }

  ping() {
    this.#consoleLog("ping");
  }

  processCallbackReferences() {
    if (this.#callbackReferences.length > 0) {

      if (this.#callbackTimeoutId == null && this.#eventBurstBoxingCallback) {
        this.#callbackTimeoutHandler(false);
      }

      if (this.#callbackDelay != null && this.#callbackDelay > 0) {
        clearTimeout(this.#callbackTimeoutId);
        this.#callbackTimeoutId = setTimeout(
          function () {
            resizeManager.#callbackTimeoutHandler(true);
          },
          this.#callbackDelay
        );
      }
    }
  }

  #callbackTimeoutHandler(fromTimer) {
    this.#consoleLog("processing all resize callback references.",
    {
      fromTimer: fromTimer,
      callbackReferences: this.#callbackReferences
    });

    this.#callbackReferences
      .forEach(ref => {
        const width = ref.element.clientWidth;
        const height = ref.element.clientHeight;
        if (ref.size.width != width || ref.size.height != height) {

          ref.size.width = width;
          ref.size.height = height;

          this.#consoleLog("invoke Callback Method");
          ref.callbackObject.invokeMethodAsync('ResizeAsync', width, height)
        }
      });

    this.#callbackTimeoutId = null;
  }

  callbackObserver(mutationsList) {
    resizeManager.processCallbackReferences();
  }

  callbackResize(event) {
    resizeManager.processCallbackReferences();
  }

  #consoleLog(message, data) {
    if (this.#enableLogsOption) {
      if (data === undefined) {
        console.log(message);
      } else {
        console.log([message, data]);
      }
    }
  }
}

export const resizeManager = new ResizeManager();

