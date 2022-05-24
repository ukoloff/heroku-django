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
  return render(req, 'hashing/index.html',
    {'q': q, 'h': h, 'r': rsp, 'ok': ok})
