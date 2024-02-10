
variable "default_tags" {
  type        = map(string)
  description = "Used for tagging resources"
  default     = {
    "Project"     = "url-shortener",
    "Terraform"   = "true",
    "Environment" = "dev"
  }
}