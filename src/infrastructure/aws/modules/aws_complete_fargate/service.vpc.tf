# resource "aws_vpc" "ecs_vpc" {
#   cidr_block           = "10.0.0.0/16"
#   enable_dns_support   = true
#   enable_dns_hostnames = true
# }

# resource "aws_subnet" "ecs_subnet_ids" {
#   count = 2

#   cidr_block              = "10.0.${count.index + 1}.0/24"
#   vpc_id                  = aws_vpc.ecs_vpc.id
#   map_public_ip_on_launch = true
# }

# resource "aws_security_group" "ecs_security_group" {
#   name        = "${var.project-name-short}-${var.short-env-name}-security-group"
#   description = "Allow inbound traffic on port 80"
#   vpc_id      = "${aws_vpc.ecs_vpc.id}"

#   ingress {
#     from_port = 80
#     to_port   = var.container-port
#     protocol  = "tcp"
#     cidr_blocks = ["0.0.0.0/0"]
#   }
# }

# resource "aws_internet_gateway" "ecs_igw" {
#   vpc_id = aws_vpc.ecs_vpc.id
# }

# resource "aws_route_table" "ecs_rt" {
#   vpc_id = aws_vpc.ecs_vpc.id

#   route {
#     cidr_block = "0.0.0.0/0"
#     gateway_id = aws_internet_gateway.ecs_igw.id
#   }
# }

# resource "aws_route_table_association" "public_subnet_asso" {
#   count = length(aws_subnet.ecs_subnet_ids)

#   subnet_id      = element(aws_subnet.ecs_subnet_ids.*.id, count.index)
#   route_table_id = aws_route_table.ecs_rt.id
# }

# # resource "aws_vpc_endpoint" "ecr_dkr" {
# #   vpc_id       = aws_vpc.ecs_vpc.id
# #   service_name = "com.amazonaws.${var.region}.ecr.dkr"

# #   vpc_endpoint_type = "Interface"
# #   security_group_ids = [aws_security_group.ecs_security_group.id]
# #   subnet_ids = aws_subnet.ecs_subnet_ids[*].id
# # }

# # resource "aws_vpc_endpoint" "ecr_api" {
# #   vpc_id       = aws_vpc.ecs_vpc.id
# #   service_name = "com.amazonaws.${var.region}.ecr.api"

# #   vpc_endpoint_type = "Interface"
# #   private_dns_enabled = true
# #   security_group_ids = [aws_security_group.ecs_security_group.id]
# #   subnet_ids = aws_subnet.ecs_subnet_ids[*].id
# # }

# # resource "aws_vpc_endpoint" "s3" {
# #   vpc_id       = aws_vpc.ecs_vpc.id
# #   service_name = "com.amazonaws.${var.region}.s3"

# #   vpc_endpoint_type = "Gateway"
# #   route_table_ids = aws_route_table.ecs_rt[*].id
# #   policy = data.aws_iam_policy_document.s3_ecr_access.json
# # }

# # resource "aws_vpc_endpoint" "ecs-telemetry" {
# #   vpc_id       = aws_vpc.ecs_vpc.id
# #   service_name = "com.amazonaws.${var.region}.ecs-telemetry"
# #   vpc_endpoint_type = "Interface"
# #   private_dns_enabled = true
# #   security_group_ids = [aws_security_group.ecs_security_group.id]
# #   subnet_ids = aws_subnet.ecs_subnet_ids[*].id
# # }