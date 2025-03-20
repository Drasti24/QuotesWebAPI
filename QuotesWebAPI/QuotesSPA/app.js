const apiUrl = "http://localhost:5035/api/quotes";

// Load all quotes
function loadQuotes() {
    fetch(apiUrl)
        .then(response => response.json())
        .then(data => {
            const list = document.getElementById("quotesList");
            list.innerHTML = ""; // Clear list
            data.forEach(quote => {
                const li = document.createElement("li");
                li.innerHTML = `"${quote.text}" - ${quote.author} 
                    <strong>(Likes: ${quote.likes})</strong>
                    <button onclick="likeQuote(${quote.id})">❤️ Like</button>`;
                list.appendChild(li);
            });
        })
        .catch(error => console.error("Error loading quotes:", error));
}

// Like a quote
function likeQuote(id) {
    fetch(`${apiUrl}/${id}/like`, {
        method: "PATCH"
    })
        .then(response => response.json())
        .then(data => {
            document.getElementById("message").innerText = data.message;
            loadQuotes(); // Refresh the list
        })
        .catch(error => console.error("Error liking quote:", error));
}
