const countriesContainer = document.getElementById("countries");
const countryCount = document.getElementById("country-count");

const countryForm = document.getElementById("country-form");
const countryNameInput = document.getElementById("country-name");
const countryCapitalInput = document.getElementById("country-capital");


async function getCountries() {
    try {
        const response = await fetch("http://localhost:5134/countries");

        if (!response.ok) {
            throw new Error("Failed to load countries");
        }

        const countries = await response.json();

        displayCountries(countries);
    } catch (error) {
        console.error("Error fetching countries:", error);

        countriesContainer.innerHTML = `
            <p>Unable to load countries.</p>
        `;
    }
}


function displayCountries(countries) {
    countriesContainer.innerHTML = "";

    countryCount.textContent = `${countries.length} countries`;

    countries.forEach(country => {
        const card = document.createElement("div");

        card.classList.add("country-card");

        card.innerHTML = `
            <h3>${country.name}</h3>
            <p>Capital: ${country.capital}</p>
        `;

        countriesContainer.appendChild(card);
    });
}


countryForm.addEventListener("submit", async (event) => {
    event.preventDefault();

    const name = countryNameInput.value.trim();
    const capital = countryCapitalInput.value.trim();

    try {
        const response = await fetch("http://localhost:5134/countries", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                name,
                capital
            })
        });

        if (!response.ok) {
            throw new Error("Failed to add country");
        }

        countryForm.reset();

        await getCountries();

    } catch (error) {
        console.error("Error adding country:", error);
    }
});


getCountries();