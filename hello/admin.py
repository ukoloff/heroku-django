from django.contrib import admin

# Register your models here.

from .models import Greeting

admin.site.register(Greeting)