variable "project" {
  type = object({
    name  = string
    short = string 
  })
  description = "Information about the project"
}

variable "env" {
  type = object({
    name  = string
    short = string  
  })
  description = "Information about the environment"
}

variable "region" {
  type = object({
    name  = string
    short = string   
  })
  description = "Information about the region"
}

variable "uss-docker-image" {
  type        = string
  description = "url-shortener-service docker image including version"
}

variable "default_tags" {
  type        = map(string)
  description = "Used for tagging resources"
}