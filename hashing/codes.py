from hashlib import sha1
from random import choice


def myHash(data: str) -> int:
    result = 0
    for b in sha1(data.encode()).digest:
        result = (result * 256 + b) % 2147483647
    return result


def response(req: int, salt: str = '') -> int:
    if len(salt) != 1:
        salt = chr(choice(range(ord('A'), ord('Z')+1)))
    return myHash(f"<{req}|{salt}|ReSpOnSe>")


def checkResponse(req: int, resp: int) -> bool:
    return any(resp == response(req, chr(c))
               for c in choice(range(ord('A'), ord('Z')+1)))
