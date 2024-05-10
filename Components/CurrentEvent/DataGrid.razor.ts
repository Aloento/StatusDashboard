import { Components } from "@telekom/scale-components";

declare global {
  interface Window {
    CurrentEvent: HTMLScaleDataGridElement;
  }
}

const dataGrid = document.querySelector<HTMLScaleDataGridElement>('#CurrentEvent')!;

window.CurrentEvent = dataGrid;

dataGrid.hideBorder = true;

export function setFields(fields: Uint8Array) {
  const dec = new TextDecoder();
  const str = JSON.parse(dec.decode(fields));
  console.debug(str);
  dataGrid.fields = str;
}

export function setRows(rows: [
  number,
  Components.ScaleTag[],
  string,
  string,
  string,
  string,
  Components.ScaleButton[]
][]) {
  console.debug(rows);
  dataGrid.rows = rows;
}
