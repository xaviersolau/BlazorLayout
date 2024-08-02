
class ScrollManager {

  constructor() {
    this.scrollObserverReferences = [];
  }

  registerScrollCallBack(callbackObjetReference, elementReferenceId, element) {
    console.log("registerScrollCallBack");
    console.log(elementReferenceId);

    element.addEventListener('scroll', this.scrollCallback)

    callbackObjetReference.invokeMethodAsync('ScrollAsync', element.scrollWidth, element.scrollLeft, element.clientWidth, element.scrollHeight, element.scrollTop, element.clientHeight);

    this.scrollObserverReferences.push({
      elementReferenceId: elementReferenceId,
      element: element,
      callbackObject: callbackObjetReference,
    });
  }

  unregisterScrollCallBack(elementReferenceId) {
    console.log("unregisterScrollCallBack");
    console.log(elementReferenceId);

    for (var i = 0; i < this.scrollObserverReferences.length; i++) {

      if (this.scrollObserverReferences[i].elementReferenceId === elementReferenceId) {

        this.scrollObserverReferences[i].element.removeEventListener('scroll', this.scrollCallback);
        this.scrollObserverReferences.splice(i, 1);

        break;
      }
    }
  }

  scrollTo(element, left, top) {
    console.log("scrollTo");
    console.log(left);
    console.log(top);

    if (left >= 0) {
      element.scrollLeft = left;
    }

    if (top >= 0) {
      element.scrollTop = top;
    }
  }

  ping() {
    console.log("ping");
  }

  scrollCallback(event) {
    scrollManager.processScrollCallback(event);
  }

  processScrollCallback(event) {
    console.log("processScrollCallback");

    for (var i = 0; i < this.scrollObserverReferences.length; i++) {

      const obsRef = this.scrollObserverReferences[i];

      if (obsRef.element === event.currentTarget) {

        obsRef.callbackObject.invokeMethodAsync('ScrollAsync', event.currentTarget.scrollWidth, event.currentTarget.scrollLeft, event.currentTarget.clientWidth, event.currentTarget.scrollHeight, event.currentTarget.scrollTop, event.currentTarget.clientHeight);

        break;
      }
    }
  }
}

export var scrollManager = new ScrollManager();

