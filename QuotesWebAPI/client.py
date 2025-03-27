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
        print("1 Load Quotes from File (one-time)")
        print("2 Get All Quotes")
        print("3 Add a New Quote")
        print("4 Get a Random Quote")
        print("5 Like a Quote")
        print("6 Delete a Quote")
        print("7 Exit")
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
        elif choice == "7":
            print("  Exiting. Have a great day!")
            break
        else:
            print("  Invalid choice. Try again!")


if __name__ == "__main__":
    main()
