document.addEventListener('DOMContentLoaded', function () {
    const token = document.querySelector('input[name="__RequestVerificationToken"]').value;
    let linkedListData = [];
    let isAnimating = false;
    let currentListType = 'circular';
    let nodePositions = [];
    const NODE_RADIUS = 30;
    const CIRCLE_RADIUS = 150;
    const CENTER_X = 400;
    const CENTER_Y = 200;
    const nodeCounter = document.getElementById('nodeCount');

    // UI Elements
    const listTypeSelect = document.getElementById('listTypeSelect');
    const operationSelect = document.getElementById('operationSelect');
    const subOperationSelect = document.getElementById('subOperationSelect');
    const dataInput = document.getElementById('dataInput');
    const indexInput = document.getElementById('indexInput');
    const executeBtn = document.getElementById('executeBtn');
    const clearBtn = document.getElementById('clearBtn');
    const linkedListContainer = document.getElementById('linkedListContainer');
    const subOperationGroup = document.getElementById('subOperationGroup');
    const dataGroup = document.getElementById('dataGroup');
    const indexGroup = document.getElementById('indexGroup');

    // Sub operations
    const subOperations = {
        insert: ['InsertFront', 'InsertBack', 'InsertAt'],
        delete: ['RemoveFront', 'RemoveBack', 'RemoveAt', 'Clear'],
        utilities: ['UpdateData', 'Search']
    };

    // Initialize SVG
    const svgNS = "http://www.w3.org/2000/svg";
    const svg = document.createElementNS(svgNS, "svg");
    svg.setAttribute('id', 'linkedListSVG');
    svg.setAttribute('viewBox', '0 0 800 400');

    // Define arrowhead markers
    const defs = document.createElementNS(svgNS, "defs");

    // Regular arrowhead
    const marker = document.createElementNS(svgNS, "marker");
    marker.setAttribute('id', 'arrowhead');
    marker.setAttribute('markerWidth', '10');
    marker.setAttribute('markerHeight', '7');
    marker.setAttribute('refX', '9');
    marker.setAttribute('refY', '3.5');
    marker.setAttribute('orient', 'auto');
    const arrowPolygon = document.createElementNS(svgNS, "polygon");
    arrowPolygon.setAttribute('points', '0 0, 10 3.5, 0 7');
    arrowPolygon.setAttribute('fill', '#495057');
    marker.appendChild(arrowPolygon);
    defs.appendChild(marker);

    // Circular arrowhead
    const circularMarker = document.createElementNS(svgNS, "marker");
    circularMarker.setAttribute('id', 'circular-arrowhead');
    circularMarker.setAttribute('markerWidth', '10');
    circularMarker.setAttribute('markerHeight', '7');
    circularMarker.setAttribute('refX', '9');
    circularMarker.setAttribute('refY', '3.5');
    circularMarker.setAttribute('orient', 'auto');
    const circularArrowPolygon = document.createElementNS(svgNS, "polygon");
    circularArrowPolygon.setAttribute('points', '0 0, 10 3.5, 0 7');
    circularArrowPolygon.setAttribute('fill', '#ff6b6b');
    circularMarker.appendChild(circularArrowPolygon);
    defs.appendChild(circularMarker);

    svg.appendChild(defs);
    linkedListContainer.appendChild(svg);

    // Initialize
    clearList(true);

    // List type selection - clear on change
    listTypeSelect.addEventListener('change', function () {
        currentListType = this.value;
        clearList(true); // Full reset with backend clear
    });

    // Operation selection
    operationSelect.addEventListener('change', function () {
        const selectedOperation = this.value;
        subOperationSelect.innerHTML = '<option value="">Select sub-operation</option>';
        subOperationGroup.style.display = selectedOperation ? 'block' : 'none';
        dataGroup.style.display = 'none';
        indexGroup.style.display = 'none';
        executeBtn.disabled = true;

        if (selectedOperation) {
            subOperations[selectedOperation].forEach(op => {
                const option = document.createElement('option');
                option.value = op.toLowerCase();
                option.textContent = op;
                subOperationSelect.appendChild(option);
            });
        }
    });

    // Sub operation selection
    subOperationSelect.addEventListener('change', function () {
        const selectedSubOp = this.value;
        dataGroup.style.display = 'none';
        indexGroup.style.display = 'none';
        executeBtn.disabled = true;

        if (operationSelect.value === 'insert') {
            dataGroup.style.display = 'block';
            if (selectedSubOp === 'insertat') {
                indexGroup.style.display = 'block';
            }
        }
        else if (operationSelect.value === 'delete') {
            if (selectedSubOp === 'removeat') {
                indexGroup.style.display = 'block';
            }
            else if (selectedSubOp === 'clear') {
                executeBtn.disabled = false;
                return;
            }
        }
        else if (operationSelect.value === 'utilities') {
            if (selectedSubOp === 'updatedata') {
                dataGroup.style.display = 'block';
                indexGroup.style.display = 'block';
            }
            else if (selectedSubOp === 'search') {
                indexGroup.style.display = 'block';
            }
        }

        validateInputs();
    });

    // Clear button
    clearBtn.addEventListener('click', async function () {
        await clearList(true); // Full reset with backend clear
    });

    // Input validation
    function validateInputs() {
        const operation = operationSelect.value;
        const subOperation = subOperationSelect.value;
        const hasData = dataInput.value.trim() !== '';
        const hasIndex = indexInput.value !== '';

        if (!operation || !subOperation) {
            executeBtn.disabled = true;
            return;
        }

        if (operation === 'insert') {
            executeBtn.disabled = !hasData || (subOperation === 'insertat' && !hasIndex);
        }
        else if (operation === 'delete') {
            executeBtn.disabled = (subOperation === 'removeat' && !hasIndex) && subOperation !== 'clear';
        }
        else if (operation === 'utilities') {
            if (subOperation === 'updatedata') {
                executeBtn.disabled = !(hasData && hasIndex);
            }
            else if (subOperation === 'search') {
                executeBtn.disabled = !hasIndex;
            }
        }
    }

    // Input event listeners
    dataInput.addEventListener('input', validateInputs);
    indexInput.addEventListener('input', validateInputs);

    // Execute operation
    executeBtn.addEventListener('click', async function () {
        if (isAnimating) return;
        isAnimating = true;

        const requestData = {
            operation: operationSelect.value,
            subOperation: subOperationSelect.value,
            data: dataInput.value,
            index: indexInput.value ? parseInt(indexInput.value) : 0
        };

        try {
            // Show traversal animation for specific operations
            if ((requestData.operation === 'insert' && requestData.subOperation === 'insertat') ||
                (requestData.operation === 'delete' && requestData.subOperation === 'removeat') ||
                (requestData.operation === 'utilities' && (requestData.subOperation === 'updatedata' || requestData.subOperation === 'search'))) {
                await animateTraversal(requestData.index);
            }

            // Execute the actual operation
            const response = await fetch('?handler=ExecuteOperation', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': token
                },
                body: JSON.stringify(requestData)
            });

            const result = await response.json();
            if (result.success) {
                linkedListData = result.list || [];
                updateVisualization();

                dataInput.value = '';
                indexInput.value = '';
                executeBtn.disabled = true;
            } else {
                showErrorMessage(result.message || 'Operation failed');
            }
        } catch (error) {
            console.error('Error:', error);
            showErrorMessage('Operation failed');
        } finally {
            isAnimating = false;
        }
    });

    function clearList(fullReset = false) {
        return new Promise((resolve) => {
            if (fullReset) {
                fetch('?handler=ExecuteOperation', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'RequestVerificationToken': token
                    },
                    body: JSON.stringify({
                        operation: 'delete',
                        subOperation: 'clear'
                    })
                }).then(response => response.json())
                    .then(result => {
                        if (result.success) {
                            linkedListData = [];
                            updateVisualization();
                        }
                        resolve();
                    });
            } else {
                linkedListData = [];
                updateVisualization();
                resolve();
            }

            // Reset inputs
            operationSelect.value = '';
            subOperationSelect.innerHTML = '<option value="">Select sub-operation</option>';
            dataInput.value = '';
            indexInput.value = '';
            subOperationGroup.style.display = 'none';
            dataGroup.style.display = 'none';
            indexGroup.style.display = 'none';
        });
    }

    function updateVisualization() {
        // Clear SVG
        while (svg.childNodes.length > 1) {
            svg.removeChild(svg.lastChild);
        }

        // Update node counter
        nodeCounter.textContent = linkedListData.length;

        if (linkedListData.length === 0) {
            showEmptyState();
            return;
        }

        // Remove empty state if exists
        const emptyState = linkedListContainer.querySelector('.empty-state');
        if (emptyState) {
            linkedListContainer.removeChild(emptyState);
        }

        // Calculate node positions
        nodePositions = [];

        if (currentListType === 'circular') {
            // Circular layout with curved arrows
            const angleStep = (2 * Math.PI) / linkedListData.length;
            linkedListData.forEach((node, i) => {
                const angle = i * angleStep;
                nodePositions.push({
                    x: CENTER_X + CIRCLE_RADIUS * Math.cos(angle),
                    y: CENTER_Y + CIRCLE_RADIUS * Math.sin(angle)
                });
            });
        } else {
            // Linear layout
            const startX = 100;
            const startY = 200;
            const spacing = Math.max(120, 800 / Math.max(1, linkedListData.length));

            linkedListData.forEach((node, i) => {
                nodePositions.push({
                    x: startX + i * spacing,
                    y: startY
                });
            });
        }

        // Draw connections first (so they appear behind nodes)
        drawConnections();

        // Draw nodes
        linkedListData.forEach((node, i) => {
            drawNode(node, i);
        });

        // Add HEAD label
        const headLabel = document.createElementNS(svgNS, "text");
        headLabel.setAttribute('class', 'head-label');
        headLabel.setAttribute('x', nodePositions[0].x);
        headLabel.setAttribute('y', nodePositions[0].y - NODE_RADIUS - 15);
        headLabel.textContent = 'HEAD';
        svg.appendChild(headLabel);
    }

    function drawNode(node, index) {
        const pos = nodePositions[index];

        // Create node group
        const nodeGroup = document.createElementNS(svgNS, "g");
        nodeGroup.setAttribute('class', index === 0 ? 'list-node head-node' : 'list-node');
        nodeGroup.setAttribute('transform', `translate(${pos.x}, ${pos.y})`);
        nodeGroup.setAttribute('data-index', index);

        // Node circle
        const circle = document.createElementNS(svgNS, "circle");
        circle.setAttribute('r', NODE_RADIUS);
        circle.setAttribute('stroke', index === 0 ? '#28a745' : '#343a40');
        nodeGroup.appendChild(circle);

        // Node data
        const dataText = document.createElementNS(svgNS, "text");
        dataText.setAttribute('class', 'node-data');
        dataText.setAttribute('y', 5);
        dataText.textContent = node.data;
        nodeGroup.appendChild(dataText);

        // Node index
        const indexText = document.createElementNS(svgNS, "text");
        indexText.setAttribute('class', 'node-index');
        indexText.setAttribute('y', -NODE_RADIUS - 5);
        indexText.textContent = `Index: ${index}`;
        nodeGroup.appendChild(indexText);

        // Node next info
        const nextValue = currentListType === 'circular' && index === linkedListData.length - 1 ?
            linkedListData[0].data :
            index < linkedListData.length - 1 ? linkedListData[index + 1].data : 'NULL';

        const nextText = document.createElementNS(svgNS, "text");
        nextText.setAttribute('class', 'node-next');
        nextText.setAttribute('x', NODE_RADIUS + 15);
        nextText.setAttribute('y', NODE_RADIUS + 20);
        nextText.textContent = `→ ${nextValue}`;
        nodeGroup.appendChild(nextText);

        // Node prev info (only for doubly)
        if (currentListType === 'doubly') {
            const prevValue = index > 0 ? linkedListData[index - 1].data : 'NULL';
            const prevText = document.createElementNS(svgNS, "text");
            prevText.setAttribute('class', 'node-prev');
            prevText.setAttribute('x', -NODE_RADIUS - 15);
            prevText.setAttribute('y', NODE_RADIUS + 20);
            prevText.textContent = `← ${prevValue}`;
            nodeGroup.appendChild(prevText);
        }

        svg.appendChild(nodeGroup);
    }

    function drawConnections() {
        linkedListData.forEach((node, i) => {
            if (i < linkedListData.length - 1) {
                // Draw forward arrow to next node
                const startPos = nodePositions[i];
                const endPos = nodePositions[i + 1];
                drawArrow(startPos, endPos, 'arrow');
            }

            // Draw backward arrow for doubly
            if (currentListType === 'doubly' && i > 0) {
                const startPos = nodePositions[i];
                const endPos = nodePositions[i - 1];
                drawArrow(startPos, endPos, 'arrow');
            }

            // Draw circular connection for last node
            if (currentListType === 'circular' && i === linkedListData.length - 1) {
                drawCircularArrow(nodePositions[i], nodePositions[0]);
            }
        });
    }

    function drawArrow(startPos, endPos, className) {
        const line = document.createElementNS(svgNS, "line");
        line.setAttribute('class', className);
        line.setAttribute('x1', startPos.x + NODE_RADIUS * Math.cos(Math.atan2(endPos.y - startPos.y, endPos.x - startPos.x)));
        line.setAttribute('y1', startPos.y + NODE_RADIUS * Math.sin(Math.atan2(endPos.y - startPos.y, endPos.x - startPos.x)));
        line.setAttribute('x2', endPos.x - NODE_RADIUS * Math.cos(Math.atan2(endPos.y - startPos.y, endPos.x - startPos.x)));
        line.setAttribute('y2', endPos.y - NODE_RADIUS * Math.sin(Math.atan2(endPos.y - startPos.y, endPos.x - startPos.x)));
        svg.appendChild(line);
    }

    function drawCircularArrow(startPos, endPos) {
        // Calculate middle point for the curve
        const midX = (startPos.x + endPos.x) / 2;
        const midY = (startPos.y + endPos.y) / 2;

        // Calculate control points for a nice curve
        const angle = Math.atan2(endPos.y - startPos.y, endPos.x - startPos.x);
        const controlDist = CIRCLE_RADIUS * 0.6;
        const ctrl1X = startPos.x + controlDist * Math.cos(angle + Math.PI / 2);
        const ctrl1Y = startPos.y + controlDist * Math.sin(angle + Math.PI / 2);
        const ctrl2X = endPos.x + controlDist * Math.cos(angle + Math.PI / 2);
        const ctrl2Y = endPos.y + controlDist * Math.sin(angle + Math.PI / 2);

        // Create path for the curved arrow
        const path = document.createElementNS(svgNS, "path");
        path.setAttribute('class', 'circular-arrow');
        path.setAttribute('d', `M ${startPos.x} ${startPos.y} 
                               C ${ctrl1X} ${ctrl1Y}, 
                                 ${ctrl2X} ${ctrl2Y}, 
                                 ${endPos.x} ${endPos.y}`);
        svg.appendChild(path);
    }

    async function animateTraversal(targetIndex) {
        if (targetIndex < 0 || targetIndex >= linkedListData.length) return;

        // Create pointer
        const pointer = document.createElementNS(svgNS, "circle");
        pointer.setAttribute('class', 'search-pointer');
        pointer.setAttribute('r', 8);
        svg.appendChild(pointer);

        for (let i = 0; i <= targetIndex; i++) {
            const pos = nodePositions[i];
            const node = svg.querySelector(`g[data-index="${i}"]`);

            // Move pointer
            pointer.setAttribute('cx', pos.x);
            pointer.setAttribute('cy', pos.y);

            // Highlight node
            node.classList.add('highlight');
            await new Promise(resolve => setTimeout(resolve, 500));

            // Remove highlight if not target
            if (i < targetIndex) {
                node.classList.remove('highlight');
            }
        }

        // Remove pointer
        setTimeout(() => {
            svg.removeChild(pointer);
        }, 500);
    }

    function showEmptyState() {
        // Clear SVG
        while (svg.childNodes.length > 1) {
            svg.removeChild(svg.lastChild);
        }

        // Reset viewBox
        svg.setAttribute('viewBox', '0 0 800 400');

        // Add empty state (only if not already exists)
        if (!linkedListContainer.querySelector('.empty-state')) {
            const emptyState = document.createElement('div');
            emptyState.className = 'empty-state';
            emptyState.innerHTML = `
                <i class="bi bi-list"></i>
                <p>The list is currently empty</p>
            `;
            linkedListContainer.appendChild(emptyState);
        }
    }

    function showErrorMessage(message) {
        const alertDiv = document.createElement('div');
        alertDiv.className = 'alert alert-danger alert-dismissible fade show';
        alertDiv.innerHTML = `
            <i class="bi bi-exclamation-triangle"></i> ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        `;
        document.body.appendChild(alertDiv);

        setTimeout(() => {
            alertDiv.remove();
        }, 5000);
    }
});