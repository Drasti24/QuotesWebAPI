import requests
import random

API_URL = "http://localhost:5035/api/quotes"


def load_quotes_from_file(filename="quotes.txt"):
    try:
        with open(filename, "r", encoding="utf-8") as file:
            lines = file.readlines()

        for line in lines:
            parts = line.strip().split(" - ")
            if len(parts) == 2:
                text, author = parts
            else:
                text, author = parts[0], "Unknown"

            response = requests.post(API_URL, json={"text": text, "author": author})
            if response.status_code == 201:
                print(f"Added: \"{text}\" - {author}")
            else:
                print(f"Failed to add: \"{text}\" - {author}")

    except FileNotFoundError:
        print("quotes.txt file not found!")


def get_all_quotes():
    """Fetches and displays all quotes from the API, including tags."""
    response = requests.get(API_URL)
    if response.status_code == 200:
        quotes = response.json()
        print("\n All Quotes (File + API):")
        for quote in quotes:
            tags = [tag["name"] for tag in quote.get("tags", [])]
            print(f"[{quote['id']}] \"{quote['text']}\" - {quote['author']} (Likes: {quote['likes']})")
            print(f"     Tags: {', '.join(tags) if tags else 'None'}")
    else:
        print("  Error fetching quotes.")



def add_quote():
    """Manually add a new quote via user input."""
    text = input("Enter quote text: ").strip()
    author = input("Enter author (or leave blank for 'Unknown'): ").strip() or "Unknown"

    response = requests.post(API_URL, json={"text": text, "author": author})
    if response.status_code == 201:
        print(f"  Quote added successfully!")
    else:
        print(f"  Error: {response.text}")


def get_random_quote():
    """Fetches a random quote from the API."""
    response = requests.get(API_URL)
    if response.status_code == 200:
        quotes = response.json()
        if quotes:
            random_quote = random.choice(quotes)
            print(f"\n Random Quote: [{random_quote['id']}] \"{random_quote['text']}\" - {random_quote['author']} (Likes: {random_quote['likes']})")
        else:
            print("  No quotes found in the database!")
    else:
        print(f"  Error fetching quotes: {response.text}")


def edit_quote():
    quote_id = input("Enter the ID of the quote you want to edit: ").strip()

    # Step 1: Fetch the quote
    response = requests.get(f"{API_URL}/{quote_id}")
    if response.status_code != 200:
        print("  Quote not found!")
        return

    quote = response.json()
    current_text = quote["text"]
    current_author = quote.get("author", "Unknown")
    current_tags = [tag["name"] for tag in quote.get("tags", [])]

    # Step 2: Display current values
    print(f"\nCurrent Text   : {current_text}")
    print(f"Current Author : {current_author}")
    print(f"Current Tags   : {', '.join(current_tags) if current_tags else 'None'}")

    # Step 3: Ask for new input
    new_text = input("Enter new quote text (leave blank to keep): ").strip()
    new_author = input("Enter new author (leave blank to keep): ").strip()

    updated_data = {
        "text": new_text if new_text else current_text,
        "author": new_author if new_author else current_author
    }

    # Step 4: Send the update
    response = requests.put(f"{API_URL}/{quote_id}", json=updated_data)
    if response.status_code == 200:
        print("  ✅ Quote updated successfully!")
    else:
        print("  ❌ Failed to update quote.")

def like_quote():
    quote_id = input("Enter the ID of the quote you want to like: ").strip()
    response = requests.patch(f"{API_URL}/{quote_id}/like")
    if response.status_code == 200:
        print(f"  Quote {quote_id} liked successfully!")
    else:
        print("  Failed to like the quote.")


def get_most_liked():
    count = input("How many top liked quotes? (Default 5): ").strip() or "5"
    response = requests.get(f"{API_URL}/topliked?count={count}")
    if response.status_code == 200:
        print("\n🔥 Most Liked Quotes:")
        for q in response.json():
            print(f"[{q['id']}] \"{q['text']}\" - {q['author']} (Likes: {q['likes']})")
    else:
        print("Failed to fetch most liked quotes.")

def assign_tag():
    quote_id = input("Enter the Quote ID: ").strip()
    tag = input("Enter tag to assign: ").strip()
    response = requests.post(f"{API_URL}/{quote_id}/tags", json=tag)
    if response.status_code == 200:
        print("  Tag assigned successfully.")
    else:
        print("  Failed to assign tag.")

def get_by_tag():
    tag = input("Enter tag to search quotes by: ").strip()
    response = requests.get(f"{API_URL}/bytag/{tag}")
    if response.status_code == 200:
        quotes = response.json()
        if quotes:
            print(f"\nQuotes with tag '{tag}':")
            for q in quotes:
                print(f"[{q['id']}] \"{q['text']}\" - {q['author']} (Likes: {q['likes']})")
        else:
            print("  No quotes found with that tag.")
    else:
        print("  Error fetching quotes by tag.")

def delete_quote():
    """Deletes a quote by ID."""
    quote_id = input("Enter the ID of the quote you want to delete: ").strip()
    response = requests.delete(f"{API_URL}/{quote_id}")
    
    if response.status_code == 204:
        print(f"  Quote ID {quote_id} deleted successfully!")
    else:
        print("  Failed to delete quote.")


def main():
    while True:
        print("\n Quotes CLI Menu:")
        print("1 Load Quotes from File (one-time)")
        print("2 Get All Quotes")
        print("3 Add a New Quote")
        print("4 Get a Random Quote")
        print("5 Like a Quote")
        print("6 Delete a Quote")
        print("7 Edit a Quote")
        print("8 Get most liked quotes")
        print("9 Assign tag to a quote")
        print("10 Get quote by tag")
        print("11 Exit")
        choice = input("Choose an option: ").strip()

        if choice == "1":
            load_quotes_from_file()
        elif choice == "2":
            get_all_quotes()
        elif choice == "3":
            add_quote()
        elif choice == "4":
            get_random_quote()
        elif choice == "5":
            like_quote()
        elif choice == "6":
            delete_quote()
        elif choice=="7":
            edit_quote()
        elif choice=="8":
            get_most_liked()
        elif choice=="9":
            assign_tag()
        elif choice=="10":
            get_by_tag()
        elif choice == "11":
            print("  Exiting. Have a great day!")
            break
        else:
            print("  Invalid choice. Try again!")


if __name__ == "__main__":
    main()
