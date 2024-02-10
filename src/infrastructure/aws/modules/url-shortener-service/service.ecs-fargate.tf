module "ecs_cluster" {
  source = "terraform-aws-modules/ecs/aws"
  version = "~> 5.8.0"

  cluster_name = "${var.project.name}-${var.env.short}-fargate"

  fargate_capacity_providers = {
    # FARGATE = {
    #   default_capacity_provider_strategy = {
    #     weight = 50
    #     base   = 20
    #   }
    # }
    FARGATE_SPOT = {
      default_capacity_provider_strategy = {
        weight = 100
      }
    }
  }

  services = {
    uss_backend = {
      cpu    = 256
      memory = 512

      # Container definition(s)
      container_definitions = {
        (var.docker.container.name) = {
          cpu       = 256
          memory    = 512
          essential = true
          image     = "${module.ecr.repository_url}/${var.docker.image_name}"

          # health_check = {
          #   command = ["CMD-SHELL", "curl -f http://localhost:${local.container_port}/health || exit 1"]
          # }

          port_mappings = [
            {
              name          = var.docker.container.name
              containerPort = var.docker.container.port
              hostPort      = var.docker.container.port
              protocol      = "tcp"
            }
          ]
        }
      }

      load_balancer = {
        service = {
          target_group_arn = module.alb.target_groups["ecs_fargate"].arn
          container_name   = var.docker.container.name
          container_port   = var.docker.container.port
        }
      }

      tasks_iam_role_name        = "${var.project.short}-${var.env.short}-ecs-fargate-role"
      tasks_iam_role_description = "Example tasks IAM role for ${var.project.name}-${var.env.short}-ecs-fargate-role"
      tasks_iam_role_policies = {
        ReadOnlyAccess = "arn:aws:iam::aws:policy/ReadOnlyAccess"
      }
      tasks_iam_role_statements = [
        {
          actions   = ["s3:List*"]
          resources = ["arn:aws:s3:::*"]
        }
      ]

      subnet_ids = module.aws_vpc.private_subnets
      security_group_rules = {
        alb_ingress_3000 = {
          type                     = "ingress"
          from_port                = var.docker.container.port
          to_port                  = var.docker.container.port
          protocol                 = "tcp"
          description              = "Service port"
          source_security_group_id = module.alb.security_group_id
        }
        egress_all = {
          type        = "egress"
          from_port   = 0
          to_port     = 0
          protocol    = "-1"
          cidr_blocks = ["0.0.0.0/0"]
        }
      }
    }
  }

  tags = var.default_tags
}