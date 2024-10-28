
class ScrollManager {

  #scrollObserverReferencesMap = new Map();

  #eventTargetMap = new Map();

  #enableLogsOption = false;
  #callbackDelay = 250;
  #eventBurstBoxingCallback = false;
  #callbackTimeoutId = null;

  setupModule(enableLogs, callbackDelay, eventBurstBoxingCallback) {
    this.#enableLogsOption = enableLogs;
    this.#callbackDelay = callbackDelay;
    this.#eventBurstBoxingCallback = eventBurstBoxingCallback;

    this.#consoleLog(
      "Set up Scroll module",
      {
        enableLogs: enableLogs,
        callbackDelay: callbackDelay,
        eventBurstBoxingCallback: eventBurstBoxingCallback
      });
  }

  registerScrollCallback(callbackObjetReference, elementReferenceId, element) {
    this.#consoleLog("registerScrollCallback",
    {
      elementReferenceId: elementReferenceId
    });

    element.addEventListener('scroll', this.scrollCallback, { passive: true })
    element.addEventListener('touchmove', this.scrollCallback, { passive: true })

    callbackObjetReference.invokeMethodAsync('ScrollAsync', element.scrollWidth, element.scrollLeft, element.clientWidth, element.scrollHeight, element.scrollTop, element.clientHeight);

    this.#scrollObserverReferencesMap.set(element, {
      elementReferenceId: elementReferenceId,
      element: element,
      callbackObject: callbackObjetReference,
    });
  }

  unregisterScrollCallback(elementReferenceId) {
    this.#consoleLog("unregisterScrollCallback",
    {
      elementReferenceId: elementReferenceId
    });

    this.#scrollObserverReferencesMap
      .keys()
      .every((observerReferenceKey, index, array) => {
        const observerReference = this.#scrollObserverReferencesMap.get(observerReferenceKey);

        if (observerReference.elementReferenceId === elementReferenceId) {
          observerReference.element.removeEventListener('touchmove', this.scrollCallback);
          observerReference.element.removeEventListener('scroll', this.scrollCallback);

          this.#scrollObserverReferencesMap.delete(observerReferenceKey);

          return false;
        }

        return true;
      });
  }

  scrollTo(element, left, top) {
    this.#consoleLog("scrollTo",
    {
      left: left,
      top: top
    });

    if (left >= 0) {
      element.scrollLeft = left;
    }

    if (top >= 0) {
      element.scrollTop = top;
    }
  }

  ping() {
    this.#consoleLog("ping");
  }

  scrollCallback(event) {
    scrollManager.processScrollCallback(event);
  }

  processScrollCallback(event) {
    if (this.#scrollObserverReferencesMap.size > 0) {

      this.#eventTargetMap.set(event.target, event.target);

      if ((this.#callbackTimeoutId == null && this.#eventBurstBoxingCallback)
        || this.#callbackDelay == null
        || this.#callbackDelay <= 0) {
        this.#callbackTimeoutHandler(false);
      }

      if (this.#callbackDelay != null && this.#callbackDelay > 0) {
        clearTimeout(this.#callbackTimeoutId);
        this.#callbackTimeoutId = setTimeout(
          function () {
            scrollManager.#callbackTimeoutHandler(true);
          },
          this.#callbackDelay
        );
      }
    }
  }

  #callbackTimeoutHandler(fromTimer) {
    this.#consoleLog("processing all scroll callback references. From timer = " + fromTimer);

    this.#eventTargetMap
      .keys()
      .forEach(eventTargetKey => {

        const eventTarget = this.#eventTargetMap.get(eventTargetKey);
        this.#eventTargetMap.delete(eventTargetKey);

        const obsRef = this.#scrollObserverReferencesMap.get(eventTarget);

        if (obsRef !== undefined) {
          this.#consoleLog("invoke Callback Method");
          obsRef.callbackObject.invokeMethodAsync('ScrollAsync', eventTarget.scrollWidth, eventTarget.scrollLeft, eventTarget.clientWidth, eventTarget.scrollHeight, eventTarget.scrollTop, eventTarget.clientHeight);
        }

      });

    this.#callbackTimeoutId = null;
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

export const scrollManager = new ScrollManager();

