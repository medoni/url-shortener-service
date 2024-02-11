module "aws_vpc" {
  source  = "terraform-aws-modules/vpc/aws"
  version = "~> 5.5.2"

  name = "${var.project.name}-${var.env.short}-vpc"
  cidr = "10.0.0.0/16"

  azs             = ["${var.region.name}a", "${var.region.name}b", "${var.region.name}c"]
  private_subnets = ["10.0.1.0/24", "10.0.2.0/24", "10.0.3.0/24"]
  public_subnets  = ["10.0.101.0/24", "10.0.102.0/24", "10.0.103.0/24"]

  enable_nat_gateway = true
  single_nat_gateway = true

  tags = var.default_tags
}