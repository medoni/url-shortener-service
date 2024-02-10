module "alb" {
  source  = "terraform-aws-modules/alb/aws"
  version = "~> 9.6.0"

  name    = "${var.project.name}-${var.env.short}-alb"
  vpc_id  = module.aws_vpc.vpc_id
  subnets = module.aws_vpc.private_subnets

  security_group_ingress_rules = {
    all_http = {
      from_port   = 80
      to_port     = var.docker.container.port
      ip_protocol = "tcp"
      description = "HTTP web traffic"
      cidr_ipv4   = "0.0.0.0/0"
    }
    # all_https = {
    #   from_port   = 443
    #   to_port     = 443
    #   ip_protocol = "tcp"
    #   description = "HTTPS web traffic"
    #   cidr_ipv4   = "0.0.0.0/0"
    # }
  }
  security_group_egress_rules = {
    all = {
      ip_protocol = "-1"
      cidr_ipv4   = "10.0.0.0/16"
    }
  }

  # access_logs = {
  #   bucket = "${var.project.name}-${var.env.short}-alb-access-logs"
  # }

  listeners = {
    ex-http-https-redirect = {
      port     = 80
      protocol = "HTTP"
      # redirect = {
      #   port        = "443"
      #   protocol    = "HTTPS"
      #   status_code = "HTTP_301"
      # }
      forward = {
        target_group_key = "ecs_fargate"
      }
    }
    # ex-https = {
    #   port            = 443
    #   protocol        = "HTTPS"
    #   certificate_arn = "arn:aws:iam::123456789012:server-certificate/test_cert-123456789012"

    #   forward = {
    #     target_group_key = "ecs_fargate"
    #   }
    # }
  }

  target_groups = {
    ecs_fargate = {
      name_prefix      = "h1"
      protocol         = "HTTP"
      port             = var.docker.container.port
      target_type      = "ip"

      # Theres nothing to attach here in this definition. Instead,
      # ECS will attach the IPs of the tasks to this target group
      create_attachment = false
    }
  }

  tags = var.default_tags
}