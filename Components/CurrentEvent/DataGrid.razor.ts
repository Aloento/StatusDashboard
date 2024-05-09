declare global {
  interface Window {
    CurrentEvent: HTMLScaleDataGridElement;
  }
}

const dataGrid = document.querySelector<HTMLScaleDataGridElement>('#CurrentEvent')!;

window.CurrentEvent = dataGrid;

dataGrid.hideBorder = true;

dataGrid.rows = [
  [1, 'John', '12:30'],
  [2, 'Mary', '2:12'],
  [3, 'Patek', '16:01'],
  [4, 'Heidi', '3:15'],
  [5, 'Muhammad', '21:45'],
];

dataGrid.localization = {
  sortBy: 'Sort By',
  toggle: 'Toogle Visibility',
  select: 'Select / Deselect All'
};

export function setFields(fields: string) {
  console.debug(JSON.parse(fields));
  dataGrid.fields = fields;
}
