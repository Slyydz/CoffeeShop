const urlBean = "https://localhost:5001/api/beanvariety/";
const urlCoffee = "https://localhost:5001/api/Coffee/"

const button = document.querySelector("#run-button");
const addButton = document.querySelector("#addButton");
const runCoffee = document.querySelector("#run-button-coffee")
const addCoffee = document.querySelector("#addButtonCoffee")


button.addEventListener("click", () => {
    getAllBeanVarieties()
    .then(beanVarieties => {
        console.log(beanVarieties);
        loadVarieties(beanVarieties);
    })
})


addButton.addEventListener("click", () => {
    const newObj = {
        Name: document.querySelector("input[name='beanVarietyName']").value,
        Region: document.querySelector("input[name='beanVarietyRegion']").value,
        Notes: document.querySelector("textarea[name='beanVarietyNotes']").value
    }

    createBean(newObj).then(() => {
        document.querySelector("input[name='beanVarietyName']").value = "";
        document.querySelector("input[name='beanVarietyRegion']").value = "";
        document.querySelector("textarea[name='beanVarietyNotes']").value = "";

    })
})

runCoffee.addEventListener("click", () => {
    getAllCoffee().then(coffee => {
        console.log(coffee);
        loadCoffee(coffee);
    })
})

addCoffee.addEventListener("click", () => {
    const newObj = {
        Title: document.querySelector("input[name='coffeeTitle']").value,
        BeanVarietyId: document.querySelector("input[name='coffeeBeanId']").value
    }

    createCoffee(newObj).then(() => {
        document.querySelector("input[name='coffeeBeanId']").value = "";
        document.querySelector("input[name='coffeeTitle']").value = "";
    })
})

function getAllBeanVarieties() {
    return fetch(urlBean).then(resp => resp.json());
}

function getAllCoffee() {
    return fetch(urlCoffee).then(resp => resp.json())
}

function loadVarieties(beanVarieties)
{
    console.log(beanVarieties)
    let app = document.querySelector(".populateVarieties")

    let htmlStr = `
    <ol>
        ${beanVarieties.map(getAllBeanVarieties => '<li>' + getAllBeanVarieties.name + '</li>').join('')}
    </ol>`;

    app.innerHTML = htmlStr;
}

function loadCoffee(coffee){
    console.log(coffee)
    let app = document.querySelector(".populateCoffee")

    app.innerHTML = `
    <ol>
    ${coffee.map(getCoffee => '<li>' + getCoffee.title + '</li>').join('')}
    </ol>`
}

const createBean = postObj => {
    return fetch(urlBean, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(postObj)
  
    })
        .then(response => response.json())
}

const createCoffee = coffeeObj => {
    return fetch(urlCoffee, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(coffeeObj)
  
    })
        .then(response => response.json())
}



