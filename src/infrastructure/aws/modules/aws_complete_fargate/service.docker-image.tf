resource "docker_image" "docker_image" {
  name          = "${aws_ecr_repository.ecr_repository.repository_url}:${var.project-name-short}-${var.short-env-name}-image-latest"

  build {
    context    = var.docker-build-context
    dockerfile = var.docker-build-dockerfile
    tag        = [aws_ecr_repository.ecr_repository.repository_url]
    # build_arg = {
    #   foo : "zoo"
    # }
    # label = {
    #   author : "zoo"
    # }
  }
}

resource "docker_registry_image" "docker_registry_image" {
  name = docker_image.docker_image.name
}