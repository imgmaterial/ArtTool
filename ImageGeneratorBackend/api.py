from fastapi import FastAPI, File, UploadFile
from fastapi.responses import JSONResponse
from diffusers import StableDiffusionPipeline
from pydantic import BaseModel
import torch
from PIL import Image
import io
from diffusers import (
    PNDMScheduler,
    DDIMScheduler,
    EulerAncestralDiscreteScheduler,
    LMSDiscreteScheduler,
    DDPMScheduler,
    DPMSolverMultistepScheduler,
    UniPCMultistepScheduler,
)

class ImageModel(BaseModel):
    prompt:str
    seed:int
    sampling_steps:int

app = FastAPI()

model_path ="models/Soushiki/SoushikiV1.0.safetensors"
pipeline = StableDiffusionPipeline.from_single_file(
    model_path,
    torch_dtype=torch.float16,
    use_safetensors=True
).to("cuda")
pipeline.scheduler = DDIMScheduler.from_config(pipeline.scheduler.config)

@app.post("/generate_image/")
async def generate_image(image_model:ImageModel):

    generator = torch.Generator(device="cuda").manual_seed(image_model.seed)
    image = pipeline(image_model.prompt, num_inference_steps=image_model.sampling_steps, generator = generator).images[0]
    img_byte_arr = io.BytesIO()
    image.save(img_byte_arr, format='PNG')
    img_byte_arr = img_byte_arr.getvalue()
    return JSONResponse(
            content={
                "status": "success",
                "message": "Image generated successfully",
                "format": "PNG",
                "image_bytes": img_byte_arr.hex(), 
            })


if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app, host="0.0.0.0", port=8000)