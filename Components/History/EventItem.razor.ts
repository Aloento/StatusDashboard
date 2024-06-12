export function onLabel(label: HTMLLabelElement) {
  const prev = label.previousElementSibling;
  if (prev && prev instanceof HTMLElement) {
    prev.style.paddingBottom = "0";
    prev.classList.add("mb-6");
  }
}
