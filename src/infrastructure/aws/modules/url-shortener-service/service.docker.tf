resource "docker_image" "docker_image" {
  name          = "${module.ecr.repository_url}:${var.docker.image_tag}"

  build {
    context    = var.docker.build.context
    dockerfile = var.docker.build.dockerfile
    tag        = [module.ecr.repository_url]
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
  keep_remotely = true
}