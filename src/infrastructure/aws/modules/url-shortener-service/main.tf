module "url-shortener-service" {
  source = "../aws_complete_fargate"

  region                  = "eu-central-1"
  project-name-short      = var.project-name-short
  short-env-name          = var.short-env-name
  docker-image            = var.uss-docker-image
  docker-build-context    = "../../../.."
  docker-build-dockerfile = "deployables/UrlShortenerService/Dockerfile"
}