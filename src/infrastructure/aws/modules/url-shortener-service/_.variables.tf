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

variable "docker" {
  type = object({
    image_name = string
    build = object({
      context = string
      dockerfile = string  
    })
  })
  description = "Information about the used docker"
}

variable "default_tags" {
  type        = map(string)
  description = "Used for tagging resources"
}