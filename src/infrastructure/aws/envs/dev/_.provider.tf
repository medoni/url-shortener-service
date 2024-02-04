terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "= 5.35.0"
    }
  }
}

provider "aws" {
  region = "eu-central-1"
}

terraform {
  backend "s3" {
    bucket = "devops-terraform-b31a89ebc35b"
    key    = "tfstate/url-shortener-service"
    region = "eu-central-1"
  }
}