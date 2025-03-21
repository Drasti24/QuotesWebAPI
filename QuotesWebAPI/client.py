import requests
import random

API_URL = "http://localhost:5035/api/quotes"


def load_quotes_from_file(filename):
    """Loads quotes from file ONLY (does NOT fetch API quotes)."""
    try:
        with open(filename, "r", encoding="utf-8") as file:
            lines = file.readlines()

        print("\n Quotes from File:")
        for line in lines:
            parts = line.strip().split(" - ")
            text, author = (parts[0], parts[1]) if len(parts) == 2 else (parts[0], "Unknown")
            print(f" \"{text}\" - {author}")

    except FileNotFoundError:
        print(f"  File {filename} not found!")


def get_all_quotes():
    """Fetches and displays all quotes from the API."""
    response = requests.get(API_URL)
    if response.status_code == 200:
        quotes = response.json()
        print("\n All Quotes (File + API):")
        for quote in quotes:
            print(f"[{quote['id']}] \"{quote['text']}\" - {quote['author']} (Likes: {quote['likes']})")
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



def like_quote():
    """Allows the user to like a quote by ID."""
    quote_id = input("Enter the ID of the quote you want to like: ").strip()
    response = requests.patch(f"{API_URL}/{quote_id}/like")
    
    if response.status_code == 200:
        print("  Quote {quote_id} liked successfully!")
    else:
        print("  Failed to like the quote.")


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
        print("1️ Load Quotes from File")
        print("2️ Get All Quotes (API + File)")
        print("3️ Add a New Quote")
        print("4️ Get a Random Quote")
        print("5️ Like a Quote")
        print("6️ Delete a Quote")
        print("7️ Exit")
        choice = input("Choose an option: ").strip()

        if choice == "1":
            filename = input("Enter the file name (quotes.txt): ").strip() or "quotes.txt"
            load_quotes_from_file(filename)
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
        elif choice == "7":
            print("  Exiting. Have a great day!")
            break
        else:
            print("  Invalid choice. Try again!")


if __name__ == "__main__":
    main()
