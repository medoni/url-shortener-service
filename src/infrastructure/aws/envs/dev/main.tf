
module "url-shortener-service" {
  source = "../../modules/url-shortener-service"

  short-env-name   = "d"
  uss-docker-image = "url-shortener-service-latest"
}