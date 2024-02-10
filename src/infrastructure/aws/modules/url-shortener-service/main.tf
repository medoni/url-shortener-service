module "url-shortener-service" {
  source = "../aws_complete_fargate"

  region                  = var.region.name
  project-name-short      = var.project.short
  short-env-name          = var.env.short
  docker-image            = var.uss-docker-image
  container-port          = 8080
  docker-build-context    = "../../../.."
  docker-build-dockerfile = "deployables/UrlShortenerService/Dockerfile"
  health-check-path       = "/"

  vpc = {
    vpc_id            = module.aws_vpc.vpc_id
    subnet_ids        = module.aws_vpc.private_subnets
    security_group_id = module.aws_vpc.default_security_group_id
  }
}

# module "aws_vpc" {
#   source = "../aws/vpc"

#   region                          = var.region
#   env                             = var.env
#   project                         = var.project
#   default_tags                    = var.default_tags
# }

module "aws_vpc" {
  source = "terraform-aws-modules/vpc/aws"

  name = "${var.project.name}-${var.env.short}-vpc"
  cidr = "10.0.0.0/16"

  azs             = ["${var.region.name}a", "${var.region.name}b", "${var.region.name}b"]
  private_subnets = ["10.0.1.0/24", "10.0.2.0/24", "10.0.3.0/24"]
  public_subnets  = ["10.0.101.0/24", "10.0.102.0/24", "10.0.103.0/24"]

  tags = var.default_tags
}