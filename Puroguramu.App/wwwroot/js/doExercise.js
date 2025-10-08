document.addEventListener('DOMContentLoaded', () => {
    initializeToastrNotifications();
    initializeTestResults();
    initializeCodeEditors();
    initializeModalHandlers();
    initializeSubmitButton();
});

function initializeToastrNotifications() {
    const toastrConfig = localStorage.getItem('toastr');
    if (toastrConfig) {
        const config = JSON.parse(toastrConfig);
        toastr[config.type](config.message);
        localStorage.removeItem('toastr');
    }
}

function initializeTestResults() {
    const testResults = localStorage.getItem('testResults');
    const testResultsElement = document.getElementById('testResults');

    if (testResults && testResultsElement) {
        const results = JSON.parse(testResults);
        const testResultsHtml = buildTestResultsHtml(results);
        testResultsElement.innerHTML = testResultsHtml;

        localStorage.removeItem('testResults');
        localStorage.removeItem('testStatus');
    }
}

function buildTestResultsHtml(results) {
    return results.map((test, index) => {
        const separator = index > 0 ? '<div class="border border-white my-1 opacity-50"></div>' : '';
        const resultClass = test.status === 'Passed' ? 'text-custom-green' : 'text-custom-red';
        const resultText = test.status === 'Failed' ? test.error : test.status;

        return `${separator}<div class="flex flex-col">
            <p>${test.label}</p>
            <p class="${resultClass}">${resultText}</p>
        </div>`;
    }).join('');
}

function initializeCodeEditors() {
    const codeEditorElement = document.getElementById('codeEditor');
    if (!codeEditorElement) return;

    const userEditor = ace.edit('codeEditor');
    userEditor.setTheme('ace/theme/dracula');
    userEditor.session.setMode('ace/mode/csharp');
    userEditor.setOption('showPrintMargin', false);

    const stubCode = codeEditorElement.getAttribute('data-stub');
    if (stubCode) {
        userEditor.session.setValue(JSON.parse(stubCode));
    }

    const solutionEditorElement = document.getElementById('solutionEditor');
    if (solutionEditorElement) {
        const solutionEditor = ace.edit('solutionEditor');
        solutionEditor.setTheme('ace/theme/dracula');
        solutionEditor.session.setMode('ace/mode/csharp');
        solutionEditor.setOption('showPrintMargin', false);
        solutionEditor.setReadOnly(true);

        const solutionCode = solutionEditorElement.getAttribute('data-solution');
        if (solutionCode) {
            solutionEditor.session.setValue(JSON.parse(solutionCode));
        }
    }
}

function initializeModalHandlers() {
    initializeResetModal();
    initializeSolutionModal();
}

function initializeResetModal() {
    const resetButton = document.getElementById('resetButton');
    const cancelResetButton = document.getElementById('cancelResetButton');
    const resetModal = document.getElementById('resetConfirmModal');
    const codeEditor = document.getElementById('codeEditor');

    if (resetButton && resetModal) {
        resetButton.addEventListener('click', () => {
            codeEditor.style.opacity = '0.5';
            resetModal.classList.remove('hidden');
        });
    }

    if (cancelResetButton && resetModal) {
        cancelResetButton.addEventListener('click', () => {
            codeEditor.style.opacity = '1';
            resetModal.classList.add('hidden');
        });
    }
}

function initializeSolutionModal() {
    const solutionButton = document.getElementById('solutionButton');
    const cancelSolutionButton = document.getElementById('cancelSolutionButton');
    const solutionModal = document.getElementById('solutionConfirmModal');
    const codeEditor = document.getElementById('codeEditor');

    if (solutionButton && solutionModal) {
        solutionButton.addEventListener('click', () => {
            codeEditor.style.opacity = '0.5';
            solutionModal.classList.remove('hidden');
        });
    }

    if (cancelSolutionButton && solutionModal) {
        cancelSolutionButton.addEventListener('click', () => {
            codeEditor.style.opacity = '1';
            solutionModal.classList.add('hidden');
        });
    }
}

function initializeSubmitButton() {
    const submitButton = document.getElementById('submitButton');
    if (!submitButton) return;

    submitButton.addEventListener('click', async () => {
        const editor = ace.edit('codeEditor');
        const userCode = editor.getValue();
        const exerciseId = parseInt(submitButton.getAttribute('data-exercise-id'));

        const originalButtonHtml = submitButton.innerHTML;
        submitButton.classList.add('cursor-not-allowed', 'no-hover-opacity');
        submitButton.innerHTML = createLoadingSpinner();
        submitButton.disabled = true;

        try {
            const result = await submitCode(userCode, exerciseId);
            handleSubmissionResult(result);
        } catch (error) {
            handleSubmissionError(error);
        } finally {
            submitButton.classList.remove('cursor-not-allowed', 'no-hover-opacity');
            submitButton.innerHTML = originalButtonHtml;
            submitButton.disabled = false;
        }
    });
}

function createLoadingSpinner() {
    return `<div class="flex justify-center">
        <svg class="animate-spin h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
            <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
            <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
        </svg>
    </div>`;
}

async function submitCode(userCode, exerciseId) {
    const antiForgeryToken = document.querySelector('input[name="__RequestVerificationToken"]').value;

    const response = await fetch('?handler=UpdateProgress', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': antiForgeryToken
        },
        body: JSON.stringify({
            UserCode: userCode,
            ExerciseId: exerciseId
        })
    });

    if (!response.ok) {
        throw new Error('Network response was not ok');
    }

    return await response.json();
}

function handleSubmissionResult(result) {
    const { status, results } = result;

    localStorage.setItem('testResults', JSON.stringify(results));

    let notificationType, notificationMessage;
    if (status === 'Passed') {
        notificationType = 'success';
        notificationMessage = 'Exercice validé!';
    } else if (status === 'Failed') {
        notificationType = 'error';
        notificationMessage = 'Exercice raté!';
    } else {
        notificationType = 'error';
        notificationMessage = status;
    }

    localStorage.setItem('toastr', JSON.stringify({
        type: notificationType,
        message: notificationMessage
    }));

    window.location.reload();
}

function handleSubmissionError(error) {
    console.error('Submission error:', error);
    localStorage.setItem('toastr', JSON.stringify({
        type: 'error',
        message: 'Erreur de soumission!'
    }));
    window.location.reload();
}
