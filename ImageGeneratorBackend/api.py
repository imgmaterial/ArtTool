from fastapi import FastAPI, File, UploadFile
from fastapi.responses import JSONResponse
from diffusers import StableDiffusionPipeline, StableDiffusionImg2ImgPipeline
from pydantic import BaseModel
import torch
from PIL import Image
from diffusers.utils import make_image_grid, load_image
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

class ImageModelImg2Img(BaseModel):
    prompt:str
    seed:int
    sampling_steps:int
    hex_string:str

app = FastAPI()

model_path ="models/Soushiki/SoushikiV1.0.safetensors"
pipeline = StableDiffusionPipeline.from_single_file(
    model_path,
    torch_dtype=torch.float16,
    use_safetensors=True
).to("cuda")
pipeline.scheduler = DDIMScheduler.from_config(pipeline.scheduler.config)

img2imgpipeline = StableDiffusionImg2ImgPipeline.from_single_file(
    model_path,
    torch_dtype=torch.float16,
    use_safetensors=True
).to("cuda")
img2imgpipeline.scheduler = DDIMScheduler.from_config(img2imgpipeline.scheduler.config)


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

@app.post("/generate_image_img2img/")
async def generate_image(image_model:ImageModelImg2Img):
    print("++++++++++++++++++++++++++++++++++++++++++++")
    print(f"Prompt : {image_model.prompt}")
    print(f"Sampling steps {image_model.sampling_steps}")
    print(f"Seed : {image_model.seed}")
    print("++++++++++++++++++++++++++++++++++++++++++++")
    gen_strength = 0.8
    if (image_model.sampling_steps*gen_strength < 1):
        return JSONResponse(
            content={"status": "error", "message": "Low number of sampling steps after strength application. Increase the number of sampling steps"},
            status_code=400
        )
    image_bytes = bytes.fromhex(image_model.hex_string)
    expected_size = 500 * 500 * 4  # RGBA
    if len(image_bytes) != expected_size:
        return JSONResponse(
            content={"status": "error", "message": "Invalid image size"},
            status_code=400
        )
    input_image = Image.frombytes("RGBA", (500,500),image_bytes)
    input_image = load_image(input_image)
    generator = torch.Generator(device="cuda").manual_seed(image_model.seed)
    image = img2imgpipeline(image_model.prompt,image=input_image, num_inference_steps=image_model.sampling_steps, generator = generator, strength = gen_strength).images[0]
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