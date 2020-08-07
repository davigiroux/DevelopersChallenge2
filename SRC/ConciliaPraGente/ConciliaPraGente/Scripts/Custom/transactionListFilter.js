const bankTransactionsElements = document.querySelectorAll(".dataLine");
const elementsArray = Array.from(bankTransactionsElements);
const bankTransactions = elementsArray.map(e => {
    return {
        Description: e.cells[0].innerText,
        Date: e.cells[1].innerText,
        Value: e.cells[2].innerText,
        Type: e.cells[3].innerText,
        isVisible: true,
        element: e
    }
});
let filteredBankTransactions = [];

document.querySelector("#descriptionFilter").addEventListener("input", e => filterWithDescription(e));
document.querySelector("#valueFilter").addEventListener("input", e => filterWithValue(e));
document.querySelector("#typeFilter").addEventListener("change", e => filterWithType(e));

function filterWithDescription(e) {
    const textToFilter = e.target.value;
    if (textToFilter.length <= 0) {
        makeAllElementsVisible();
        return;
    };

    filteredBankTransactions = bankTransactions;
    filteredBankTransactions = filteredBankTransactions.map(bt => bt.Description.toUpperCase().includes(textToFilter.toUpperCase()) ? bt : { ...bt, isVisible: false });

    filteredBankTransactions.forEach(bt => {
        bt.element.style.display = bt.isVisible ? "table-row" : "none";
    });
}

function filterWithValue(e) {
    const textToFilter = e.target.value;
    if (textToFilter.length <= 0) {
        makeAllElementsVisible();
        return;
    };

    filteredBankTransactions = bankTransactions;
    filteredBankTransactions = filteredBankTransactions.map(bt => {
        const value = getNumberFromFormattedText(bt.Value);
        const valueToFilter = Number(textToFilter.replace(",", "."));

        return value === valueToFilter ? bt : { ...bt, isVisible: false };
    });

    filteredBankTransactions.forEach(bt => {
        bt.element.style.display = bt.isVisible ? "table-row" : "none";
    });
}

function filterWithType(e) {
    const optionSelected = e.target.value;
    if (optionSelected === "AMBOS") {
        makeAllElementsVisible();
        return;
    };
    filteredBankTransactions = bankTransactions;
    filteredBankTransactions = filteredBankTransactions.map(bt => bt.Type.toUpperCase().includes(optionSelected.toUpperCase()) ? bt : { ...bt, isVisible: false });

    filteredBankTransactions.forEach(bt => {
        bt.element.style.display = bt.isVisible ? "table-row" : "none";
    });
}

function makeAllElementsVisible() {
    bankTransactions.forEach(bt => {
        bt.element.style.display = "table-row";
    });
}

function getNumberFromFormattedText(text) {
    let numberInText = text.substring(3);
    numberInText = removeChar(numberInText, ".");
    numberInText = numberInText.replace(",", ".");
    return Number(numberInText);
}

function removeChar(string, char) {
    let isCharInString = string.includes(char);
    let newString = string;
    while (isCharInString) {
        if (!newString.includes(char))
            isCharInString = false;
        else {
            const i = newString.indexOf(char);
            const stringInArray = newString.split('');
            stringInArray.splice(i, 1);
            newString = stringInArray.join(''); 
        }
    }
    return newString;
}