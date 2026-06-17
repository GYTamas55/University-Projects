from typing import List
from pydantic import BaseModel, Field, EmailStr

'''

Útmutató a fájl használatához:

Az osztályokat a schema alapján ki kell dolgozni.

A schema.py az adatok küldésére és fogadására készített osztályokat tartalmazza.
Az osztályokban az adatok legyenek validálva.
 - az int adatok nem lehetnek negatívak.
 - az email mező csak e-mail formátumot fogadhat el.
 - Hiba esetén ValuErrort kell dobni, lehetőség szerint ezt a 
   kliens oldalon is jelezni kell.

'''

ShopName='Boltxd'

class User(BaseModel):
    id: int = Field(..., ge=0, description="Distinct user id (not negative)")
    name: str = Field(..., min_length=1, description="Username (must not be blank)")
    email: EmailStr = Field(..., description="User email")

    class Config:
        schema_example = {
            "User_example": {
                "id": 1,
                "name": "Durandai Durrano Dave",
                "email": "durandai@miskolc.com"
            }
        }




class Item(BaseModel):
    item_id: int = Field(..., ge=0, description="Item id (not negative)")
    name: str = Field(..., min_length=1, description="Item name (must not be blank)")
    brand: str = Field(..., min_length=1, description="Item brand (must not be negative)")
    price: float = Field(..., ge=0, description="Item price (must not be negative)")
    quantity: int = Field(..., ge=0, description="Item quantity (not negative)")

    class Config:
         schema_example = {
            "Item_example": {
                "item_id": 201,
                "name": "Alma",
                "brand": "Jonatán",
                "price": 9.99,
                "quantity": 2
            }
        }

class Basket(BaseModel):
    id: int = Field(..., ge=0, description="Basket id (not negative)")
    user_id: int = Field(..., ge=0, description="The id of the basket's owner")
    items: List[Item] = Field(default_factory=list, description="Items in the basket (list)")

    class Config:
         schema_example = {
            "Basket_example": {
                "id": 101,
                "user_id": 1,
                "items": [
                    {
                        "item_id": 201,
                        "name": "Alma",
                        "brand": "Jonatán",
                        "price": 9.99,
                        "quantity": 2
                    }
                ]
            }
        }
