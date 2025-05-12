const operationMap = {
    insert: ["insertBack", "insertFront", "insertAt"],
    delete: ["remove", "removeAt", "removeFront", "removeBack"],
    utilities: ["updateData", "clear", "search"]
};

function onOperationChange() {
    const op = document.getElementById("operation").value;
    const subOpsContainer = document.getElementById("subOperations");

    subOpsContainer.innerHTML = "";

    if (!op) {
        toggleInputs(false, false);
        return;
    }

    operationMap[op].forEach(subOp => {
        const radio = document.createElement("input");
        radio.type = "radio";
        radio.name = "subOp";
        radio.value = subOp;
        radio.id = subOp;
        radio.onchange = () => onSubOperationChange(subOp);

        const label = document.createElement("label");
        label.htmlFor = subOp;
        label.innerText = subOp;

        subOpsContainer.appendChild(radio);
        subOpsContainer.appendChild(label);
        subOpsContainer.appendChild(document.createElement("br"));
    });
}

function onSubOperationChange(selected) {
    const needsData = ["insertBack", "insertFront", "insertAt", "updateData"].includes(selected);
    const needsIndex = ["insertAt", "removeAt", "search", "updateData"].includes(selected);

    document.getElementById("dataInput").disabled = !needsData;
    document.getElementById("indexInput").disabled = !needsIndex;
    document.getElementById("executeBtn").disabled = false;
}

function toggleInputs(data, index) {
    document.getElementById("dataInput").disabled = !data;
    document.getElementById("indexInput").disabled = !index;
    document.getElementById("executeBtn").disabled = true;
}

async function onExecute() {
    const op = document.getElementById("operation").value;
    const radios = document.getElementsByName("subOp");
    const selectedSubOp = Array.from(radios).find(r => r.checked)?.value;

    const data = document.getElementById("dataInput").value;
    const index = parseInt(document.getElementById("indexInput").value || "0");

    const payload = {
        operation: op,
        subOperation: selectedSubOp,
        data: data,
        index: index
    };

    const res = await fetch("/Index?handler=Execute", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(payload)
    });

    const nodes = await res.json();
    drawList(nodes);
    function drawList(nodes) {
        const container = document.getElementById("listVisualization");
        container.innerHTML = "";

        nodes.forEach(n => {
            const div = document.createElement("div");
            div.className = "node";
            div.innerText = `${n.i}: ${n.data}`;
            container.appendChild(div);
        });
    }

}
