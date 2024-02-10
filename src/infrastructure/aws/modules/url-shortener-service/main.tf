# module "url-shortener-service" {
#   source = "../aws_complete_fargate"

#   region                  = var.region.name
#   project-name-short      = var.project.short
#   short-env-name          = var.env.short
#   docker-image            = var.uss-docker-image
#   container-port          = 8080
#   docker-build-context    = "../../../.."
#   docker-build-dockerfile = "deployables/UrlShortenerService/Dockerfile"
#   health-check-path       = "/"

#   vpc = {
#     vpc_id            = module.aws_vpc.vpc_id
#     subnet_ids        = module.aws_vpc.private_subnets
#     security_group_id = module.aws_vpc.default_security_group_id
#   }

#   alb = {
#     alb_id               = module.alb.id
#     target_groups        = module.alb.target_groups
#     default_target_group = module.alb.target_groups["ecs_fargate"]
#   }
# }