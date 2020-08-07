const transactionsElements = document.querySelectorAll(".dataLine");


function saveBankTransactions() {
    const elements = Array.from(transactionsElements);
    const bankTransactions = elements.map(e => {
        return {
            Description: e.cells[0].innerText,
            Date: e.cells[1].innerText,
            Value: e.cells[2].innerText,
            Type: e.cells[3].innerText
        }
    });
    showLoading();
    document.querySelector("#uploaderButton").disabled = true;

    fetch("/BankTransaction/SaveBankTransactions",
            {
                method: "POST",
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(bankTransactions)
        })
        .then(res => res.json())
        .then(data => {
            hideLoading();
            document.querySelector("#uploaderButton").disabled = false;
            $('#uploadConfirmation').modal('hide');

            if (data.message == "success")
                showSuccessMessage();

        }).catch(err => {
            hideLoading();
            document.querySelector("#uploaderButton").disabled = false;
            $('#uploadConfirmation').modal('hide');
            console.error(err);
        });
}

function showLoading() {
    const loadingEl = document.querySelector("#loading");
    console.log(loadingEl);

    loadingEl.style.display = "inline";
}

function hideLoading() {
    const loadingEl = document.querySelector("#loading");

    loadingEl.style.display = "none";
}

function showSuccessMessage() {
    document.querySelector("#successMessageWrapper").style.display = "block";
}

function showErrorMessage(error) {
    document.querySelector("#errorMessageWrapper").style.display = "block";
    document.querySelector("#errorMessage").innerText = error;
}