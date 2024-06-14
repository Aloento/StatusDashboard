let dataGrid;
const observer = new MutationObserver((mutationsList) => {
    mutationsList.forEach((mutation) => {
        if (mutation.type === "childList") {
            const added = mutation.addedNodes;
            added.forEach((node) => {
                if (node.nodeType === Node.ELEMENT_NODE) {
                    const cells = node.querySelectorAll(".tbody__cell");
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
function refresh() {
    if (dataGrid)
        observer.disconnect();
    dataGrid = document.querySelector("#EventGrid");
    dataGrid.hideBorder = true;
    observer.observe(dataGrid.shadowRoot, {
        childList: true,
        subtree: true,
    });
}
export function setFields(fields) {
    refresh();
    const dec = new TextDecoder();
    const str = JSON.parse(dec.decode(fields));
    dataGrid.fields = str;
}
export function setRows(rows) {
    dataGrid.rows = rows;
}
