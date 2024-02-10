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
    image = "${aws_ecr_repository.ecr_repository.repository_url}/${var.docker-image}"
    portMappings = [{
      containerPort = var.container-port,
      hostPort      = var.container-port
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
  desired_count   = 1

  network_configuration {
    subnets          = var.vpc.subnet_ids
    security_groups  = [var.vpc.security_group_id]
    assign_public_ip = true
  }

  load_balancer {
    target_group_arn = aws_alb_target_group.ecs_lb_tg.arn
    container_name   = var.docker-image
    container_port   = var.container-port
  }

  # lifecycle {
  #   ignore_changes = [task_definition, desired_count]
  # }

  # depends_on = [aws_lb_listener.fargate_lb_listener]
}