resource "aws_ecs_cluster" "ecs_cluster" {
  name = "${var.project-name-short}-${var.short-env-name}-ecs-fargate"
}

resource "aws_ecs_task_definition" "ecs_task" {
  family                   = "${var.project-name-short}-${var.short-env-name}-ecs-task"
  network_mode             = "awsvpc"
  requires_compatibilities = ["FARGATE"]
  cpu                      = 256
  memory                   = 512

  execution_role_arn = aws_iam_role.ecs_execution_role.arn

  container_definitions = jsonencode([{
    name  = var.docker-image
    image = aws_ecr_repository.ecr_repository.repository_url
    portMappings = [{
      containerPort = 8080,
      hostPort      = 8080,
    }]
  }])
}

resource "aws_iam_role" "ecs_execution_role" {
  name = "${var.project-name-short}-${var.short-env-name}-ecs-execution-role"

  assume_role_policy = jsonencode({
    Version = "2012-10-17",
    Statement = [{
      Action = "sts:AssumeRole",
      Effect = "Allow",
      Principal = {
        Service = "ecs-tasks.amazonaws.com",
      },
    }],
  })
}

resource "aws_ecs_service" "ecs_service" {
  name            = "${var.project-name-short}-${var.short-env-name}-ecs-service"
  cluster         = aws_ecs_cluster.ecs_cluster.id
  task_definition = aws_ecs_task_definition.ecs_task.arn
  launch_type     = "FARGATE"

  network_configuration {
    subnets          = aws_subnet.ecs_subnet_ids[*].id
    security_groups  = [aws_security_group.ecs_security_group.id]
    assign_public_ip = true
  }
}

resource "aws_vpc" "ecs_vpc" {
  cidr_block = "10.0.0.0/16"
}

resource "aws_subnet" "ecs_subnet_ids" {
  count = 2

  cidr_block = "10.0.${count.index + 1}.0/24"
  vpc_id     = aws_vpc.ecs_vpc.id
}

resource "aws_security_group" "ecs_security_group" {
  name        = "${var.project-name-short}-${var.short-env-name}-security-group"
  description = "Allow inbound traffic on port 80"
  vpc_id      = "${aws_vpc.ecs_vpc.id}"

  ingress {
    from_port = 80
    to_port   = 8080
    protocol  = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }
}