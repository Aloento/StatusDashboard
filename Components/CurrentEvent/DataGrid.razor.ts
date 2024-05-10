import { Components } from "@telekom/scale-components";

const dataGrid = document.querySelector<HTMLScaleDataGridElement>("#CurrentEvent")!;
dataGrid.hideBorder = true;

const observer = new MutationObserver((mutationsList) => {
  mutationsList.forEach((mutation) => {
    if (mutation.type === "childList") {
      const added = mutation.addedNodes as NodeListOf<HTMLElement>;

      added.forEach((node) => {
        if (node.nodeType === Node.ELEMENT_NODE) {
          const cells = node.querySelectorAll(".tbody__cell") as NodeListOf<HTMLDivElement>;

          cells.forEach((cell) => {
            if (cell.querySelector(".tbody__actions")) {
              cell.style.paddingTop = "0";
              cell.style.paddingBottom = "0";
            }
          });
        }
      });
    }
  });
});

observer.observe(dataGrid.shadowRoot!, {
  childList: true,
  subtree: true,
});

export function setFields(fields: Uint8Array) {
  const dec = new TextDecoder();
  const str = JSON.parse(dec.decode(fields));
  console.debug(str);
  dataGrid.fields = str;
}

export function setRows(
  rows: [
    number,
    Components.ScaleTag[],
    string,
    string,
    string,
    string,
    Components.ScaleButton[]
  ][]
) {
  console.debug(rows);
  dataGrid.rows = rows;
}
