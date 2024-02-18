terraform {
  required_version = ">= 0.13"
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "= 5.35.0"
    }

    docker = {
      source  = "kreuzwerker/docker"
      version = "3.0.2"
    }
  }
}

provider "aws" {
  region = "eu-central-1"
}

provider "docker" {
  host = "npipe:////.//pipe/docker_engine"
}

terraform {
  backend "s3" {
    bucket = "devops-terraform-b31a89ebc35b"
    key    = "tfstate/url-shortener-service"
    region = "eu-central-1"
  }
}