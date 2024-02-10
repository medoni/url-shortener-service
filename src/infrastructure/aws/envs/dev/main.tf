
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

  uss-docker-image = "url-shortener-service-latest"

  default_tags = var.default_tags
}