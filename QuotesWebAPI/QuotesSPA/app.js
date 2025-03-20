const apiUrl = "http://localhost:5035/api/quotes";

// Load all quotes and update UI
function loadQuotes() {
    fetch(apiUrl)
        .then(response => {
            if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);
            return response.json();
        })
        .then(data => {
            console.log("Quotes loaded:", data); // Debugging log
            const list = document.getElementById("quotesList");
            list.innerHTML = ""; // Clear the list before updating
            data.forEach(quote => {
                const li = document.createElement("li");
                li.innerHTML = `"${quote.text}" - ${quote.author} 
                    <strong>(Likes: ${quote.likes})</strong>
                    <button onclick="likeQuote(${quote.id})">❤️ Like</button>
                    <button onclick="showTags(${quote.id})">📌 Show Tags</button>
                    <span id="tags-${quote.id}">📌 Tags: ${quote.tags?.map(t => t.name).join(", ") || "None"}</span>`;
                list.appendChild(li);
            });
        })
        .catch(error => console.error("Error loading quotes:", error));
}

// Like a quote
function likeQuote(id) {
    fetch(`${apiUrl}/${id}/like`, { method: "PATCH" })
        .then(response => response.json())
        .then(data => {
            console.log("Quote liked:", data);
            loadQuotes(); // Refresh the list
        })
        .catch(error => console.error("Error liking quote:", error));
}

// Assign a tag to a quote
function assignTag() {
    const quoteId = document.getElementById("quoteIdForTag").value;
    const tagName = document.getElementById("tagName").value;

    if (!quoteId || !tagName) {
        document.getElementById("tagMessage").innerText = "Please enter a Quote ID and a Tag Name.";
        return;
    }

    fetch(`${apiUrl}/${quoteId}/tags`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(tagName)
    })
        .then(response => response.json())
        .then(data => {
            console.log("Tag assigned:", data);
            document.getElementById("tagMessage").innerText = data.message;
            showTags(quoteId);
        })
        .catch(error => console.error("Error assigning tag:", error));
}

// Show tags for a quote
function showTags(id) {
    fetch(`${apiUrl}/${id}`)
        .then(response => response.json())
        .then(quote => {
            console.log("Tags loaded for quote:", quote);
            const tagSpan = document.getElementById(`tags-${id}`);
            tagSpan.innerHTML = `📌 Tags: ${quote.tags?.map(t => t.name).join(", ") || "None"}`;
        })
        .catch(error => console.error("Error loading tags:", error));
}

// Add a new quote
function addQuote() {
    const text = document.getElementById("quoteText").value;
    const author = document.getElementById("quoteAuthor").value;

    if (!text) {
        alert("Please enter a quote text!");
        return;
    }

    fetch(apiUrl, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ text, author })
    })
        .then(response => response.json())
        .then(() => {
            console.log("Quote added!");
            loadQuotes(); // Refresh list
        })
        .catch(error => console.error("Error adding quote:", error));
}
