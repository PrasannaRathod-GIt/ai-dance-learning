# main.py
# FastAPI application entrypoint (minimal, runnable).
# Keeps logic intentionally tiny so you can verify the site/server quickly.

from fastapi import FastAPI
from fastapi.staticfiles import StaticFiles
from backend.app.api.v1 import endpoints as endpoints_mod

app = FastAPI(title="ai-dance-learning backend - minimal")

# Include API router (v1)
app.include_router(endpoints_mod.router, prefix="/api/v1")

# Serve static files (backend/static/index.html will be served at /)
app.mount("/", StaticFiles(directory="backend/static", html=True), name="static")
