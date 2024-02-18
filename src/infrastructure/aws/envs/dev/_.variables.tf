
variable "default_tags" {
  type        = map(string)
  description = "Used for tagging resources"
  default     = {
    "Project"     = "url-shortener",
    "Terraform"   = "true",
    "Environment" = "dev"
  }
}

variable "docker_image_tag" {
  type = string
  description = "docker_image_tag"
}