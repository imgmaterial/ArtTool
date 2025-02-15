
from enum import Enum
from diffusers import StableDiffusionPipeline, StableDiffusionImg2ImgPipeline, StableDiffusionXLImg2ImgPipeline, StableDiffusionXLPipeline
import torch

class PipelineType(Enum):
    SD_text2img = 3
    SD_img2img = 0
    SDXL_text2img = 4
    SDXL_img2img = 1



class PipelineManager:

    def __init__(self, pipeline_type, model):
        self.pipeline_type = pipeline_type
        self.model = model
        self.pipeline = self.create_pipeline(self.pipeline_type, self.model)

    def set_pipeline(self, pipeline_type, model):
        self.pipeline_type = pipeline_type
        self.model = model
        self.pipeline = self.create_pipeline(self.pipeline_type, self.model)
    
    def create_pipeline(self, pipeline_type, model):
        match (pipeline_type):
            case PipelineType.SD_text2img:
                pipeline = StableDiffusionPipeline.from_single_file(
                model,
                torch_dtype=torch.float16,
                use_safetensors=True
                ).to("cuda")
            case PipelineType.SD_img2img:
                pipeline = StableDiffusionImg2ImgPipeline.from_single_file(
                model,
                torch_dtype=torch.float16,
                use_safetensors=True
                ).to("cuda")
            case PipelineType.SDXL_text2img:
                pipeline = StableDiffusionXLPipeline.from_single_file(
                model,
                torch_dtype=torch.float16,
                use_safetensors=True
                ).to("cuda")
            case PipelineType.SDXL_img2img:
                pipeline = StableDiffusionXLImg2ImgPipeline.from_single_file(
                model,
                torch_dtype=torch.float16,
                use_safetensors=True
                ).to("cuda")
        return pipeline



