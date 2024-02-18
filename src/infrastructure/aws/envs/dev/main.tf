
module "url-shortener-service" {
  source = "../../modules/url-shortener-service"

  project = {
    name  = "url-shortener-service"
    short = "uss"
  }

  env = {
    name  = "dev"
    short = "d"
  }

  region = {
    name  = "eu-central-1"
    short = "ec1"
  }

  docker = {
    image_name = "url-shortener-service"
    image_tag  = var.docker_image_tag 
    build = {
      context    = "../../../../"
      dockerfile = "deployables/UrlShortenerService/Dockerfile"
    }
    container = {
      name = "url-shortener-service"
      port = 80
    }
  }

  default_tags = var.default_tags
}