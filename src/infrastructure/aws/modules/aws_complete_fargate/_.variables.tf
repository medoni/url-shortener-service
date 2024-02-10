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

variable "container-port" {
  type = number
  description = "container-port"
}

variable "docker-build-context" {
  type = string
  description = "docker-build-context"
}

variable "docker-build-dockerfile" {
  type = string
  description = "docker-build-dockerfile"
}

variable "health-check-path" {
  type = string
  description = "health-check-path"
}

variable "vpc" {
  type = object({
    vpc_id            = string
    subnet_ids        = list(string)
    security_group_id = string
  })
  description = "vpc settings"
}

variable "alb" {
  type = object({
    alb_id               = string
    target_groups        = map(any)
    default_target_group = any
  })
}