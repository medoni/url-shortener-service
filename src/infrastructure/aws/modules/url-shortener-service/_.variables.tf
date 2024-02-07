
variable "project-name-short" {
  type = string
  description = "Short project name"
  default = "uss"
}

variable "short-env-name" {
  type = string
  description = "Short environment name"
}

variable "uss-docker-image" {
  type        = string
  description = "url-shortener-service docker image including version"
}