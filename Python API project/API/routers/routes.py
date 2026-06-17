from schemas.schema import User, Basket, Item
from fastapi.responses import JSONResponse, RedirectResponse
from fastapi import FastAPI, HTTPException, Request, Response, Cookie
from fastapi import APIRouter
from data import filehandler
from data import filereader
'''

Útmutató a fájl használatához:

- Minden route esetén adjuk meg a response_modell értékét (típus)
- Ügyeljünk a típusok megadására
- A függvények visszatérési értéke JSONResponse() legyen
- Minden függvény tartalmazzon hibakezelést, hiba esetén dobjon egy HTTPException-t
- Az adatokat a data.json fájlba kell menteni.
- A HTTP válaszok minden esetben tartalmazzák a 
  megfelelő Státus Code-ot, pl 404 - Not found, vagy 200 - OK

'''

routers = APIRouter()

@routers.post('/adduser', response_model=User)
def adduser(user: User) -> User:
    try:
        filehandler.add_user(user.model_dump())
        return JSONResponse(content=user.model_dump(), status_code=201)
    #Errors
    except ValueError as error:
        raise HTTPException(status_code = 400, detail = str(error))
    except Exception as randomError:
        raise HTTPException(status_code = 500, detail = str(randomError))

@routers.post('/addshoppingbag')
def addshoppingbag(userid: int) -> str:
    try:
        #Get data
        data = filehandler.load_json()
        baskets =  data.get("Baskets", [])
        #Generate data
        max_id = max((b.get("id", 0) for b in baskets), default = 100)
        new_id = max_id + 1
        new_basket = {"id": new_id, "user_id": userid, "items": []}
        filehandler.add_basket(new_basket)
        return JSONResponse(content={"message": "Basket added C:", "basket_id": new_id}, status_code=201)
    #Errors
    except ValueError as error:
        message = str(error).lower()
        if "This basket id is already in use for id:" in message or "No user with this id:" in message or "Missing" in message or "Basket is not type of dict" in message:
            raise HTTPException(status_code=404, detail=str(error))
        raise HTTPException(status_code=400, detail=str(error))
    except Exception as randomError:
        raise HTTPException(status_code = 500, detail = str(randomError))
        


@routers.post('/additem', response_model=Basket)
def additem(userid: int, item: Item) -> Basket:
    try:
        #Add item
        filehandler.add_item_to_basket(userid, item.model_dump())
        #Get data
        data = filehandler.load_json()
        baskets = data.get("Baskets", [])
        #Search for basket
        basket = None
        for b in baskets:
            if b.get("user_id") == userid:
                basket = b
                break
        
        if basket is None:
            raise HTTPException(status_code = 404, detail=f"No basket for this user: {userid}")
        return JSONResponse(content = basket, status_code=200)
    #Errors
    except ValueError as error:
        raise HTTPException(status_code=400, detail=str(error))
    except Exception as randomError:
        raise HTTPException(status_code=500, detail=str(randomError))

@routers.put('/updateitem')
def updateitem(userid: int, itemid: int, updateItem: Item) -> Basket:
    try:
        #Get data
        data = filehandler.load_json()
        baskets = data.get("Baskets", [])
        #Search for basket
        basket = None
        for b in baskets:
            if b.get("user_id") == userid:
                basket = b
                break
        
        if basket is None:
            raise HTTPException(status_code = 404, detail=f"No basket for this user: {userid}")
        
        #Search for item
        items = basket.get("items", [])
        index = None
        for indx, i in enumerate(items):
            if i.get("item_id") == itemid:
                index = indx
                break
        if index is None:
            raise HTTPException(status_code = 404, detail=f"No item in user's basket for this item id: {itemid}")

        #Update
        basket["items"][index] = updateItem.model_dump()
        filehandler.save_json(data)
        return basket
    #Errors
    except ValueError as error:
        raise HTTPException(status_code=400, detail=str(error))
    except Exception as randomError:
        raise HTTPException(status_code=500, detail=str(randomError))


@routers.delete('/deleteitem')
def deleteitem(userid: int, itemid: int) -> Basket:
    #Get data
    try:
        data = filehandler.load_json()
        baskets = data.get("Baskets", [])
        #Search for basket
        basket = None
        for b in baskets:
            if b.get("user_id") == userid:
                basket = b
                break
        
        if basket is None:
            raise HTTPException(status_code = 404, detail=f"No basket for this user: {userid}")
        #Deletion
        original_length = len(basket.get(("items"), []))
        new_items = []
        for item in basket.get("items", []):
            if item.get("item_id") != itemid:
                new_items.append(item)
        
        if original_length == len(new_items):
            raise HTTPException(status_code=404, detail=f"Item {itemid} not found in basket.")
        #Update items to the remaining items
        basket["items"] = new_items
        filehandler.save_json(data)
        return JSONResponse(content=basket, status_code=200)
    #Errors
    except ValueError as error:
        raise HTTPException(status_code=400, detail=str(error))
    except Exception as randomError:
        raise HTTPException(status_code=500, detail=str(randomError))
@routers.get('/user')
def user(userid: int) -> User:
    try:
        user = filereader.get_user_by_id(userid)
        return JSONResponse(content=user, status_code=200)
    #Errors
    except ValueError as error:
        raise HTTPException(status_code=400, detail=str(error))
    except Exception as randomError:
        raise HTTPException(status_code=500, detail=str(randomError))

@routers.get('/users')
def users() -> list[User]:
    try:
        all_users = filereader.get_all_users()
        return JSONResponse(content=all_users, status_code=200)
    #Errors
    except ValueError as error:
        raise HTTPException(status_code=400, detail=str(error))
    except Exception as randomError:
        raise HTTPException(status_code=500, detail=str(randomError))
    
@routers.get('/shoppingbag')
def shoppingbag(userid: int) -> list[Item]:
    try:
        items = filereader.get_basket_by_user_id(userid)
        return JSONResponse(content=items, status_code=200)
    #Errors
    except ValueError as error:
        raise HTTPException(status_code=400, detail=str(error))
    except Exception as randomError:
        raise HTTPException(status_code=500, detail=str(randomError))

@routers.get('/getusertotal')
def getusertotal(userid: int) -> float:
    try:
        total = filereader.get_total_price_of_basket(userid)
        return JSONResponse(content={"total": total}, status_code=200)
    #Errors
    except ValueError as error:
        raise HTTPException(status_code=400, detail=str(error))
    except Exception as randomError:
        raise HTTPException(status_code=500, detail=str(randomError))



