from PIL import Image
import numpy as np
from tqdm import tqdm
import json

size = 256
image_spacing_size = 8  # how much to skip each image gen
iterations = 64

x_border_width = 8
y_border_width = 8

hard_path = 'C:\\Unity Projects\\XRAY [WebGL]\\Assets\\Scripts\\py\\'


def gen_config():
  config_settings = {
    "min": size-iterations,
    "max": size,
    "spacing": image_spacing_size
  }

  with open(f'{hard_path}\\config.json', 'w', encoding='utf-8') as f:
    str_ = json.dumps(config_settings,
                      indent=4,
                      ensure_ascii=False)
    f.write(str_)


def gen_cookie(size, x_border_width, y_border_width):
  intersection_width = 4
  white = 255
  black = 0

  pixels = []
  for i in range(size):
    row_pixels = []
    for j in range(size):

      if i > size - y_border_width or j > size - x_border_width:
        #print('i: ', i, ' j: ', j)
        pixel = (black, black, black)

      # vert intersection
      elif j < ((size / 2) + intersection_width) and j > ((size / 2) - intersection_width):
        #print((size / 2) + (border_width / 2), (size / 2) - (border_width / 2))
        pixel = (black, black, black) 

      # hor intersection
      elif i < ((size / 2) + intersection_width) and i > ((size / 2) - intersection_width):
        pixel = (black, black, black)

      elif i > y_border_width and j > x_border_width:
        pixel = (white, white, white)

      else:
        pixel = (black, black, black)

      row_pixels.append(pixel)
    pixels.append(row_pixels)

  array = np.array(pixels, dtype=np.uint8)

  new_image = Image.fromarray(array)
  new_image.save(f'{hard_path}/cookies/{size - x_border_width}_{size - y_border_width}.jpg')


for x in tqdm(range(image_spacing_size, iterations+1)):  # +1 to include last iteration in range()
  for y in range(image_spacing_size, iterations+1):
    if not x % image_spacing_size and not y % image_spacing_size:
      gen_cookie(size, x, y)
gen_config()
