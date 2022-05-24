import code
from django.shortcuts import render
from requests import request

from . import codes

# Create your views here.


def index(req):
    q = req.GET.get('q', '')
    h = codes.myHash(q)
    rsp = codes.response(h)
    ok = codes.checkResponse(h, rsp)
    nok = codes.checkResponse(h, rsp + 1)
    return render(req, 'hashing/index.html',
                  {'q': q, 'h': h, 'r': rsp, 'ok': ok, 'nok': nok})
