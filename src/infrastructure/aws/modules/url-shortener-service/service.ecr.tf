module "ecr" {
  source = "terraform-aws-modules/ecr/aws"
  version = "~> 1.6.0"

  repository_name = "${var.project.short}-${var.env.short}-ecr-repo"

  repository_read_write_access_arns = [data.aws_caller_identity.current.arn]
  repository_lifecycle_policy = jsonencode({
    "rules": [
      {
        "rulePriority": 1,
        "description": "Expire images older than 14 days",
        "selection": {
          "tagStatus": "any",
          "countType": "sinceImagePushed",
          "countUnit": "days",
          "countNumber": 14
        },
        "action": {
          "type": "expire"
        }
      }
    ]
  })

  manage_registry_scanning_configuration = true
  registry_scan_type                     = "ENHANCED"
  registry_scan_rules = [
    {
      scan_frequency = "SCAN_ON_PUSH"
      filter         = "*"
      filter_type    = "WILDCARD"
      }, {
      scan_frequency = "CONTINUOUS_SCAN"
      filter         = "example"
      filter_type    = "WILDCARD"
    }
  ]

  tags = var.default_tags
}