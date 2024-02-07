# # resource "aws_lb" "fargate_lb" {
# #   name               = "${var.project-name-short}-${var.short-env-name}-fargate-lb"
# #   internal           = false
# #   load_balancer_type = "application"
# #   security_groups    = [aws_security_group.ecs_security_group.id]
# #   subnets            = aws_subnet.ecs_subnet_ids[*].id

# #   enable_deletion_protection = false
# #   enable_http2 = true
# #   enable_cross_zone_load_balancing = true
# # }

# # resource "aws_lb_listener" "fargate_lb_listener" {
# #   load_balancer_arn = aws_lb.fargate_lb.arn
# #   port              = 80
# #   protocol          = "HTTP"

# #   default_action {
# #     type             = "fixed-response"
# #     fixed_response {
# #       content_type = "text/plain"
# #       status_code  = "200"
# #       message_body = "OK"
# #     }
# #   }
# # }

# resource "aws_alb" "ecs_cluster_alb" {
#   name            = "${var.project-name-short}-${var.short-env-name}-fargate-alb"
#   internal        = false
#   security_groups = [aws_security_group.ecs_security_group.id]
#   subnets         = aws_subnet.ecs_subnet_ids[*].id
# }

# resource "aws_alb_listener" "ecs_alb_https_listener" {
#   load_balancer_arn = aws_alb.ecs_cluster_alb.arn
#   port              = 443
#   protocol          = "HTTPS"
#   ssl_policy        = "ELBSecurityPolicy-TLS-1-2-2017-01"
#   certificate_arn   = aws_acm_certificate.ecs_domain_certificate.arn

#   default_action {
#     type             = "forward"
#     target_group_arn = aws_alb_target_group.ecs_default_target_group.arn
#   }

#   # depends_on = [aws_alb_target_group.ecs_default_target_group]
# }

# resource "aws_alb_target_group" "ecs_default_target_group" {
#   name     = "${var.project-name-short}-${var.short-env-name}-fargate-target-group"
#   port     = 80
#   protocol = "HTTP"
#   vpc_id   = aws_vpc.production-vpc.id
# }