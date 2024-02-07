variable "region" {
  type = string
  description = "region"
}

variable "project-name-short" {
  type = string
  description = "Short project name"
}

variable "short-env-name" {
  type = string
  description = "Short environment name"
}

variable "docker-image" {
  type = string
  description = "docker-image"
}

variable "docker-build-context" {
  type = string
  description = "docker-build-context"
}

variable "docker-build-dockerfile" {
  type = string
  description = "docker-build-dockerfile"
}