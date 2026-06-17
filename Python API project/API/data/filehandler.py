import json
from typing import Dict, Any
from pathlib import Path

'''
Útmutató a fájl függvényeinek a használatához

Új felhasználó hozzáadása:

new_user = {
    "id": 4,  # Egyedi felhasználó azonosító
    "name": "Szilvás Szabolcs",
    "email": "szabolcs@plumworld.com"
}

Felhasználó hozzáadása a JSON fájlhoz:

add_user(new_user)

Hozzáadunk egy új kosarat egy meglévő felhasználóhoz:

new_basket = {
    "id": 104,  # Egyedi kosár azonosító
    "user_id": 2,  # Az a felhasználó, akihez a kosár tartozik
    "items": []  # Kezdetben üres kosár
}

add_basket(new_basket)

Új termék hozzáadása egy felhasználó kosarához:

user_id = 2
new_item = {
    "item_id": 205,
    "name": "Szilva",
    "brand": "Stanley",
    "price": 7.99,
    "quantity": 3
}

Termék hozzáadása a kosárhoz:

add_item_to_basket(user_id, new_item)

Hogyan használd a fájlt?

Importáld a függvényeket a filehandler.py modulból:

from filehandler import (
    add_user,
    add_basket,
    add_item_to_basket,
)

 - Hiba esetén ValuErrort kell dobni, lehetőség szerint ezt a 
   kliens oldalon is jelezni kell.

'''

# A JSON fájl elérési útja
JSON_FILE_PATH = "data/data.json"

def load_json() -> Dict[str, Any]:
    #Get the path
    path = Path(JSON_FILE_PATH) 
    
    #Valid path check
    if not path.exists(): 
        raise ValueError(f"The path {JSON_FILE_PATH} does not exist :c")
    
    #Get the data
    try:
        with open(JSON_FILE_PATH, "r", encoding="utf-8") as file:
            data = json.load(file)
            return data
    except json.JSONDecodeError:
        raise ValueError("JSON value error :c")

def save_json(data: Dict[str, Any]) -> None:
    try:
        with open(JSON_FILE_PATH, "w", encoding="utf-8") as file:
            json.dump(data, file, indent=2, ensure_ascii=False)
    except Exception:
        raise ValueError(f"Something went wrong :c")
    

def add_user(user: Dict[str, Any]) -> None:
    #Valid type
    if not isinstance(user, dict):
        raise ValueError("User is not type of dict")
    #Valid user to be added
    if "id" not in user or "name" not in user or "email" not in user:
        raise ValueError("Missing value (name/email/id)")
    #Het data
    data = load_json()
    users = data.get("Users", [])
    #No duplicates
    for u in users:
        if u.get("id") == user["id"]:
            raise ValueError(f"User already exists: {user['id']}")
        
    #Add user
    users.append(user)
    data["Users"] = users
    save_json(data)
    

def add_basket(basket: Dict[str, Any]) -> None:
    #Valid type
    if not isinstance(basket, dict):
        raise ValueError("Basket is not type of dict")
    #Valid basket to be added
    if "id" not in basket or "user_id" not in basket:
        raise ValueError("Missing value (id/user_id)")
    
    #Get data
    data = load_json()
    baskets = data.get("Baskets", [])
    users = data.get("Users", [])

    #User exists
    valid_user = any(u["id"] == basket["user_id"] for u in users)
    if not valid_user:
        raise ValueError(f"No user with this id: {basket['user_id']}")
    #Basket id is not duplicate
    for b in baskets:
        if b.get("id") == basket["id"]:
            raise ValueError(f"This basket id is already in use for id: {basket["id"]}")

    #Add basket
    baskets.append(basket)
    data["Baskets"] = baskets
    save_json(data)


def add_item_to_basket(user_id: int, item: Dict[str, Any]) -> None:
    #Valid types
    if not isinstance(user_id, int):
        raise ValueError(f"User id is no type of int")
    if not isinstance(item, Dict):
        raise ValueError(f"Item is no type of Dict")
    #Valid item to be added
    if "item_id" not in item or "name" not in item or "price" not in item or "quantity" not in item:
        raise ValueError("Missing value (id/name/price/quantity)")
    
    #Get data
    data = load_json()
    baskets = data.get("Baskets", [])

    #Add item
    basket_found = False
    for basket in baskets:
        if(basket.get("user_id") == user_id):
            basket_found = True
            #No duplicate
            for item_in_basket in basket.get("items", []):
                if item_in_basket.get("item_id") == item["item_id"]:
                    raise ValueError(f"Item already in basket :c")
            basket["items"].append(item)
            break

    if not basket_found:
        raise ValueError(f"No user with id of {user_id}")
    
    data["Baskets"] = baskets
    save_json(data)
    