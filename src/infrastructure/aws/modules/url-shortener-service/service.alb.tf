module "alb" {
  source  = "terraform-aws-modules/alb/aws"
  version = "~> 9.6.0"

  name    = "${var.project.name}-${var.env.short}-alb"
  vpc_id  = module.aws_vpc.vpc_id
  subnets = module.aws_vpc.private_subnets

  security_group_ingress_rules = {
    all_http = {
      from_port   = 80
      to_port     = 80
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
      target_id        = "arn:aws:ecs:eu-central-1:730335564775:cluster/uss-d-ecs-fargate"
      name_prefix      = "h1"
      protocol         = "HTTP"
      port             = 80
      target_type      = "instance"
    }
  }

  tags = var.default_tags
}