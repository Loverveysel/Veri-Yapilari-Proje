document.addEventListener('DOMContentLoaded', function () {
    const token = document.querySelector('input[name="__RequestVerificationToken"]').value;
    let linkedListData = [];
    let draggedNode = null;

    // UI Elements
    const operationSelect = document.getElementById('operationSelect');
    const subOperationSelect = document.getElementById('subOperationSelect');
    const dataInput = document.getElementById('dataInput');
    const indexInput = document.getElementById('indexInput');
    const executeBtn = document.getElementById('executeBtn');
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

    // Initialize with empty list
    showEmptyState();

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
        const requestData = {
            operation: operationSelect.value,
            subOperation: subOperationSelect.value,
            data: dataInput.value,
            index: indexInput.value ? parseInt(indexInput.value) : 0
        };

        try {
            const response = await fetch('?handler=ExecuteOperation', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': token
                },
                body: JSON.stringify(requestData)
            });

            const result = await response.json();
            console.log('Server Response:', result);

            if (result.success) {
                if (requestData.operation === 'delete' && requestData.subOperation === 'clear') {
                    showSuccessMessage('List cleared successfully!');
                    setTimeout(showEmptyState, 1500);
                } else {
                    updateVisualization(result.list);
                }

                // Reset inputs
                dataInput.value = '';
                indexInput.value = '';
                executeBtn.disabled = true;
            } else {
                showErrorMessage(result.message || 'Operation failed');
            }
        } catch (error) {
            console.error('Error:', error);
            showErrorMessage('Network error occurred');
        }
    });

    // Visualization functions
    function updateVisualization(nodes) {
        linkedListContainer.innerHTML = '';

        if (!nodes || nodes.length === 0) {
            showEmptyState();
            return;
        }

        // Circular positioning
        const centerX = linkedListContainer.offsetWidth / 2;
        const centerY = linkedListContainer.offsetHeight / 2;
        const radius = Math.min(centerX, centerY) * 0.7;
        const angleStep = (2 * Math.PI) / nodes.length;

        nodes.forEach((node, i) => {
            const angle = i * angleStep - Math.PI / 2; // Start from top
            const x = centerX + radius * Math.cos(angle) - 50;
            const y = centerY + radius * Math.sin(angle) - 50;

            const nodeElement = document.createElement('div');
            nodeElement.className = 'node-element';
            nodeElement.style.left = `${x}px`;
            nodeElement.style.top = `${y}px`;
            nodeElement.innerHTML = `
                <div class="node-content">
                    <div class="node-data">${node.data}</div>
                    <div class="node-index">${i}</div>
                </div>
            `;

            // Add arrow (except last node)
            if (i < nodes.length - 1) {
                const arrow = document.createElement('div');
                arrow.className = 'node-arrow';
                arrow.innerHTML = '→';
                nodeElement.appendChild(arrow);
            }

            // Store index for reference
            nodeElement.dataset.index = i;
            linkedListContainer.appendChild(nodeElement);
        });

        // Circular connection (if more than 1 node)
        if (nodes.length > 1) {
            const svg = document.createElementNS("http://www.w3.org/2000/svg", "svg");
            svg.setAttribute('width', '100%');
            svg.setAttribute('height', '100%');
            svg.style.position = 'absolute';
            svg.style.top = '0';
            svg.style.left = '0';
            svg.style.zIndex = '1';

            const firstAngle = -Math.PI / 2;
            const lastAngle = (nodes.length - 1) * angleStep - Math.PI / 2;

            const path = document.createElementNS("http://www.w3.org/2000/svg", "path");
            const startX = centerX + radius * Math.cos(lastAngle) + 50;
            const startY = centerY + radius * Math.sin(lastAngle) + 50;
            const endX = centerX + radius * Math.cos(firstAngle) + 50;
            const endY = centerY + radius * Math.sin(firstAngle) + 50;

            // Bezier curve for smooth connection
            const controlX = centerX;
            const controlY = centerY - radius * 1.5;

            path.setAttribute('d',
                `M ${startX} ${startY}
                 Q ${controlX} ${controlY}, ${endX} ${endY}`);
            path.setAttribute('stroke', '#3498db');
            path.setAttribute('stroke-width', '2');
            path.setAttribute('fill', 'none');
            path.setAttribute('stroke-dasharray', '5,3');

            svg.appendChild(path);
            linkedListContainer.appendChild(svg);

            // Circular arrow
            const arrow = document.createElement('div');
            arrow.className = 'node-arrow';
            arrow.innerHTML = '↻';
            arrow.style.position = 'absolute';
            arrow.style.left = `${endX - 40}px`;
            arrow.style.top = `${endY - 60}px`;
            arrow.style.color = '#e74c3c';
            arrow.style.fontSize = '28px';
            arrow.style.animation = 'bounce 2s infinite';
            linkedListContainer.appendChild(arrow);
        }
    }

    function showEmptyState() {
        linkedListContainer.innerHTML = `
            <div class="empty-state">
                <i class="bi bi-list"></i>
                <p>The list is currently empty</p>
            </div>
        `;
    }

    function showSuccessMessage(message) {
        const alertDiv = document.createElement('div');
        alertDiv.className = 'alert alert-success';
        alertDiv.innerHTML = `<i class="bi bi-check-circle"></i> ${message}`;
        linkedListContainer.prepend(alertDiv);

        setTimeout(() => {
            alertDiv.remove();
        }, 3000);
    }

    function showErrorMessage(message) {
        const alertDiv = document.createElement('div');
        alertDiv.className = 'alert alert-danger';
        alertDiv.innerHTML = `<i class="bi bi-exclamation-triangle"></i> ${message}`;
        document.body.prepend(alertDiv);

        setTimeout(() => {
            alertDiv.remove();
        }, 5000);
    }

    // Drag and drop functionality
    document.addEventListener('mousedown', function (e) {
        if (e.target.closest('.node-element')) {
            draggedNode = e.target.closest('.node-element');
            draggedNode.style.cursor = 'grabbing';
            draggedNode.style.zIndex = '100';
            draggedNode.style.boxShadow = '0 10px 30px rgba(0,0,0,0.3)';
        }
    });

    document.addEventListener('mousemove', function (e) {
        if (draggedNode) {
            const containerRect = linkedListContainer.getBoundingClientRect();
            const x = e.clientX - containerRect.left - 50;
            const y = e.clientY - containerRect.top - 50;

            // Keep within container bounds
            const boundedX = Math.max(0, Math.min(containerRect.width - 100, x));
            const boundedY = Math.max(0, Math.min(containerRect.height - 100, y));

            draggedNode.style.left = `${boundedX}px`;
            draggedNode.style.top = `${boundedY}px`;
        }
    });

    document.addEventListener('mouseup', function () {
        if (draggedNode) {
            draggedNode.style.cursor = 'grab';
            draggedNode.style.zIndex = '10';
            draggedNode.style.boxShadow = '0 4px 15px rgba(0,0,0,0.15)';
            draggedNode = null;
        }
    });

    // Node click to expand
    linkedListContainer.addEventListener('click', function (e) {
        const node = e.target.closest('.node-element');
        if (node) {
            // Close other expanded nodes
            document.querySelectorAll('.node-expanded').forEach(n => {
                if (n !== node) n.classList.remove('node-expanded');
            });

            node.classList.toggle('node-expanded');
            if (node.classList.contains('node-expanded')) {
                const nextIndex = (parseInt(node.dataset.index) + 1) % linkedListData.length;
                node.innerHTML = `
                    <div class="node-content">
                        <div class="node-data">${linkedListData[node.dataset.index].data}</div>
                        <div class="node-index">${node.dataset.index}</div>
                        <div class="node-details">
                            <div>Next: ${linkedListData[nextIndex].data}</div>
                            <button class="btn btn-sm btn-info mt-2">Details</button>
                        </div>
                    </div>
                `;
            } else {
                node.innerHTML = `
                    <div class="node-content">
                        <div class="node-data">${linkedListData[node.dataset.index].data}</div>
                        <div class="node-index">${node.dataset.index}</div>
                    </div>
                `;
            }
        }
    });

    // Initial animation
    linkedListContainer.style.opacity = '0';
    setTimeout(() => {
        linkedListContainer.style.opacity = '1';
    }, 300);
});