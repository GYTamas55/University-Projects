import json
from typing import Dict, Any, List
from pathlib import Path

'''
Útmutató a féjl használatához:

Felhasználó adatainak lekérdezése:

user_id = 1
user = get_user_by_id(user_id)
print(f"Felhasználó adatai: {user}")

Felhasználó kosarának tartalmának lekérdezése:

user_id = 1
basket = get_basket_by_user_id(user_id)
print(f"Felhasználó kosarának tartalma: {basket}")

Összes felhasználó lekérdezése:

users = get_all_users()
print(f"Összes felhasználó: {users}")

Felhasználó kosarában lévő termékek összárának lekérdezése:

user_id = 1
total_price = get_total_price_of_basket(user_id)
print(f"A felhasználó kosarának összára: {total_price}")

Hogyan futtasd?

Importáld a függvényeket a filehandler.py modulból:

from filereader import (
    get_user_by_id,
    get_basket_by_user_id,
    get_all_users,
    get_total_price_of_basket
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
        with open(JSON_FILE_PATH, "r", encoding="utf-8") as date:
            return json.load(date)
    except json.JSONDecodeError:
        raise ValueError("JSON value error :c")
    

def get_user_by_id(user_id: int) -> Dict[str, Any]:
    #Valid id type
    if not isinstance(user_id, int):
        raise ValueError("The id must be an integer :c")
    #Get the data
    data = load_json()
    users = data.get("Users", [])

    #Search for the user by id
    for user in users:
        if user.get("id") == user_id:
            return user
    raise ValueError(f"No user with the id of {user_id} :c")



def get_basket_by_user_id(user_id: int) -> List[Dict[str, Any]]:
    #Valid id type
    if not isinstance(user_id, int):
        raise ValueError("The id must be an integer :c")
    #Get the data
    data = load_json()
    baskets = data.get("Baskets", [])
    #Search for the basket by user id
    for basket in baskets:
        if basket.get("user_id") == user_id:
            return basket.get("items", [])
    raise ValueError(f"No basket with the user id of {user_id} :c")



def get_all_users() -> List[Dict[str, Any]]:
    data = load_json()
    users = data.get("Users", [])
    return users



def get_total_price_of_basket(user_id: int) -> float:
    #Get data
    items = get_basket_by_user_id(user_id)
    value = 0.0

    for index, item in enumerate(items):
        #Valid item
        if not isinstance(item, dict):
            raise ValueError(f"Invalid item at {index}")
        #Valid price and quantity
        if "price" not in item or "quantity" not in item:
            raise ValueError(f"Invalid price or quantity at {index}")
        
        #Get price and quantity
        try:
            price = float(item["price"])
            quantity = int(item["quantity"])
        #Valid type for quantity and price
        except(TypeError, ValueError):
            raise ValueError(f"Item at {index} has wrong type of quantity/price")
        if price < 0 or quantity < 0:
            raise ValueError(f"Item at {index} has negative quantity/price")
        #Add the value of current items to total value
        value += price * quantity
    
    #Result
    return value