﻿//DRASTI PATEL
//MARCH 30, 2025
//PROLEM ANALYSIS 03

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
                li.innerHTML = `[ID: ${quote.id}] "${quote.text}" - ${quote.author} 
                <strong>(Likes: ${quote.likes})</strong>
                <button onclick="likeQuote(${quote.id})">❤️ Like</button>
                <br>
                <span>📌 Tags: ${quote.tags?.map(t => t.name).join(", ") || "None"}</span>`;
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

// Edita an exiting quote using input fields
function editQuote() {
    const id = document.getElementById("editQuoteId").value;
    const text = document.getElementById("editQuoteText").value;
    const author = document.getElementById("editQuoteAuthor").value;

    if (!id || !text) {
        document.getElementById("editMessage").innerText = "Please enter both ID and new quote text.";
        return;
    }

    fetch(`${apiUrl}/${id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ id, text, author })
    })
        .then(response => {
            if (response.ok) {
                document.getElementById("editMessage").innerText = "Quote updated successfully!";
                loadQuotes(); // Refresh UI
            } else {
                return response.text().then(msg => {
                    document.getElementById("editMessage").innerText = "Failed to update: " + msg;
                });
            }
        })
        .catch(error => {
            console.error("Error updating quote:", error);
            document.getElementById("editMessage").innerText = "Something went wrong.";
        });
}

// Displays top N most liked quotes
function loadMostLiked() {
    const count = document.getElementById("likedCount").value || 10;
    fetch(`${apiUrl}/topliked?count=${count}`)
        .then(response => {
            if (!response.ok) throw new Error("Failed to fetch top liked quotes");
            return response.json();
        })
        .then(data => {
            const list = document.getElementById("mostLikedList");
            list.innerHTML = "";
            data.forEach(quote => {
                const li = document.createElement("li");
                li.innerHTML = `[${quote.id}] "${quote.text}" - ${quote.author || "Unknown"} ❤️ ${quote.likes}`;
                list.appendChild(li);
            });
        })
        .catch(error => console.error("Error loading most liked quotes:", error));
}

//fetches all tag suggestions for tag autocomplete (used in datalist)
function loadTagSuggestions() {
    fetch("http://localhost:5035/api/tags")
        .then(response => response.json())
        .then(tags => {
            const datalist = document.getElementById("tagSuggestions");
            datalist.innerHTML = "";
            tags.forEach(tag => {
                const option = document.createElement("option");
                option.value = tag.name;
                datalist.appendChild(option);
            });
        })
        .catch(error => console.error("Error loading tag suggestions:", error));
}

// Gets quotes that are associated with a specific tag
function getQuotesByTag() {
    const tag = document.getElementById("searchTag").value.trim();
    if (!tag) {
        alert("Please enter a tag to search.");
        return;
    }

    console.log("Searching quotes for tag:", tag);

    fetch(`${apiUrl}/bytag/${encodeURIComponent(tag)}`)
        .then(response => {
            if (!response.ok) {
                throw new Error(`Failed to fetch quotes for tag "${tag}" (Status: ${response.status})`);
            }
            return response.json();
        })
        .then(data => {
            const list = document.getElementById("taggedQuotesList");
            list.innerHTML = "";

            if (data.length === 0) {
                list.innerHTML = `<li>No quotes found for tag "${tag}".</li>`;
                return;
            }

            data.forEach(quote => {
                const li = document.createElement("li");
                li.innerHTML = `[ID: ${quote.id}] "${quote.text}" - ${quote.author} 
                <strong>(Likes: ${quote.likes})</strong>
                <button onclick="likeQuote(${quote.id})">❤️ Like</button><br>
                <span>📌 Tags: ${quote.tags?.map(t => t.name).join(", ") || "None"}</span>`;
                list.appendChild(li);
            });
        })
        .catch(error => {
            console.error("Error fetching quotes by tag:", error);
            alert("Failed to load quotes. See console for details.");
        });
}

//Fetches quote by ID and displays it
function getQuoteById() {
    const id = document.getElementById("quoteIdSearch").value;
    if (!id) {
        document.getElementById("quoteByIdResult").innerHTML = "Please enter a Quote ID.";
        return;
    }

    fetch(`${apiUrl}/${id}`)
        .then(response => {
            if (!response.ok) throw new Error("Quote not found");
            return response.json();
        })
        .then(quote => {
            document.getElementById("quoteByIdResult").innerHTML = `
                <strong>[ID: ${quote.id}]</strong> "${quote.text}" - ${quote.author || "Unknown"}<br>
                ❤️ Likes: ${quote.likes}<br>
                📌 Tags: ${quote.tags?.map(t => t.name).join(", ") || "None"}
            `;
        })
        .catch(error => {
            document.getElementById("quoteByIdResult").innerHTML = "Quote not found.";
            console.error("Error fetching quote by ID:", error);
        });
}

// Loads an existing quote into the edit fields for editing
function fetchQuoteForEdit() {
    const id = document.getElementById("editQuoteId").value;

    if (!id) {
        document.getElementById("editMessage").innerText = "Please enter a Quote ID.";
        return;
    }

    fetch(`${apiUrl}/${id}`)
        .then(response => {
            if (!response.ok) throw new Error("Quote not found");
            return response.json();
        })
        .then(quote => {
            document.getElementById("editQuoteText").value = quote.text;
            document.getElementById("editQuoteAuthor").value = quote.author || "";
            document.getElementById("editMessage").innerText = "Quote loaded. You can now edit.";
        })
        .catch(error => {
            console.error("Error fetching quote:", error);
            document.getElementById("editMessage").innerText = "Quote not found.";
        });
}

// Runs automatically on page load
window.onload = () => {
    //loadQuotes();
    loadTagSuggestions();
};
