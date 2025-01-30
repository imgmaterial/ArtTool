from fastapi import FastAPI, File, UploadFile
from diffusers import StableDiffusionPipeline
import torch
from PIL import Image
import io

app = FastAPI()

# Load default model
model_id = "models/waifu-diffusion"
pipeline = StableDiffusionPipeline.from_pretrained(model_id)

@app.post("/generate_image/{prompt}")
async def generate_image(prompt: str):
    image = pipeline(prompt).images[0]
    img_byte_arr = io.BytesIO()
    image.save("temp", format='PNG')
    img_byte_arr = img_byte_arr.getvalue()
    return {"image": img_byte_arr}

if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app, host="0.0.0.0", port=8000)