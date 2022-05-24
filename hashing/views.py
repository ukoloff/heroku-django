from django.shortcuts import render
from requests import request

# Create your views here.

def index(req):
  return render(req, 'hashing/index.html', {'q': req.GET.get('q', '')})
